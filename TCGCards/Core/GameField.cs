using System;
using System.Collections.Generic;
using System.Linq;
using NetworkingCore;
using TCGCards.Core.SpecialAbilities;

namespace TCGCards.Core
{
    public class GameField
    {
        public const int StartingHandsize = 7;
        public const int PriceCards = 1;
        public const int ConfusedDamage = 30;
        public const int BenchMaxSize = 5;
        private object lockObject = new object();
        private HashSet<NetworkId> playersSetStartBench;

        public GameField()
        {
            Players = new List<Player>();
            StateQueue = new Queue<GameFieldState>();
            playersSetStartBench = new HashSet<NetworkId>();
            AttackStoppers = new List<AttackStopper>();
            DamageStoppers = new List<DamageStopper>();
            PassiveAbilities = new List<PassiveAbility>();
            TemporaryPassiveAbilities = new List<TemporaryPassiveAbility>();
            GameState = GameFieldState.WaitingForConnection;
        }

        public void RevealCardsTo(List<Card> pickedCards, Player nonActivePlayer)
        {
            //TODO: Complete this
        }

        public void InitTest()
        {
            Players.Add(new Player { Id = NetworkId.Generate() });
            Players.Add(new Player { Id = NetworkId.Generate() });
            ActivePlayer = Players[0];
            NonActivePlayer = Players[1];

            for (int i = 0; i < 20; i++)
            {
                ActivePlayer.Deck.Cards.Push(new TestPokemonCard(ActivePlayer));
                NonActivePlayer.Deck.Cards.Push(new TestPokemonCard(NonActivePlayer));
            }
        }

        public void OnActivePokemonSelected(NetworkId ownerId, PokemonCard activePokemon)
        {
            var owner = Players.First(p => p.Id.Equals(ownerId));

            owner.SetActivePokemon(activePokemon);

            lock (lockObject)
            {
                if (GameState == GameFieldState.BothSelectingActive)
                {
                    if (Players.All(p => p.ActivePokemonCard != null))
                        GotoNextState();
                }
                else
                {
                    GotoNextState();
                }
            }
        }

        public void OnPokemonRetreated(PokemonCard replacementCard, IEnumerable<EnergyCard> payedEnergy)
        {
            if (NonActivePlayer.ActivePokemonCard.Ability?.TriggerType == TriggerType.OpponentRetreats)
                NonActivePlayer.ActivePokemonCard.Ability?.Trigger(NonActivePlayer, ActivePlayer, 0);

            int retreatCost = ActivePlayer.ActivePokemonCard.RetreatCost + GetAllPassiveAbilities().OfType<CostModifierAbility>().Where(ability => ability.ModifierType == PassiveModifierType.RetreatCost).Sum(ability => ability.ExtraCost.Sum(x => x.Amount));

            if (retreatCost <= payedEnergy.Sum(energy => energy.GetEnergry().Amount))
                ActivePlayer.RetreatActivePokemon(replacementCard, payedEnergy);
        }

        public void OnBenchPokemonSelected(Player owner, PokemonCard selectedPokemon)
        {
            owner.SetBenchedPokemon(selectedPokemon);

            if (GameState == GameFieldState.BothSelectingBench)
            {
                lock (lockObject)
                {
                    playersSetStartBench.Add(owner.Id);
                    if (playersSetStartBench.Count == 2)
                    {
                        Players.ForEach(x => x.SetPrizeCards(PriceCards));
                        GotoNextState();
                    }
                }
            }
            else
            {
                GotoNextState();
            }
        }

        private void GotoNextState()
        {
            if(StateQueue.Any())
                GameState = StateQueue.Dequeue();

            if(GameState == GameFieldState.EndAttack)
                PostAttack();
        }

        public void StartGame()
        {
            ActivePlayer = Players[new Random().Next(2)];
            NonActivePlayer = Players.First(p => !p.Id.Equals(ActivePlayer.Id));

            foreach (var player in Players)
            {
                do
                {
                    foreach (var card in player.Hand)
                    {
                        player.Deck.Cards.Push(card);
                    }

                    player.Hand.Clear();

                    player.Deck.Shuffle();
                    player.DrawCards(StartingHandsize);
                } while (!player.Hand.OfType<PokemonCard>().Any(p => p.Stage == 0));
                //TODO: Actual mulligan rules
            }

            GameState = GameFieldState.BothSelectingActive;
            StateQueue.Enqueue(GameFieldState.BothSelectingBench);
            StateQueue.Enqueue(GameFieldState.InTurn);
        }

        public void Attack(Attack attack)
        {
            if (StateQueue.Any())
                StateQueue.Dequeue();

            StateQueue.Enqueue(GameFieldState.EndAttack);

            if (ActivePlayer.ActivePokemonCard.IsConfused && CoinFlipper.FlipCoin() == CoinFlipper.TAILS)
            {
                ActivePlayer.ActivePokemonCard.DealDamage(new Damage(0, ConfusedDamage));

                if (ActivePlayer.ActivePokemonCard.Ability?.TriggerType == TriggerType.DealsDamage)
                {
                    ActivePlayer.ActivePokemonCard.Ability.SetTarget(ActivePlayer.ActivePokemonCard);
                    ActivePlayer.ActivePokemonCard.Ability.Trigger(ActivePlayer, NonActivePlayer, 30);
                    ActivePlayer.ActivePokemonCard.Ability.SetTarget(null);
                }

                PostAttack();
                return;
            }

            if (ActivePlayer.ActivePokemonCard.Ability?.TriggerType == TriggerType.Attacks)
                ActivePlayer.ActivePokemonCard.Ability?.Trigger(ActivePlayer, NonActivePlayer, 0);

            if (!AttackStoppers.Any(x => x.IsAttackIgnored()) && !ActivePlayer.ActivePokemonCard.AttackStoppers.Any(x => x.IsAttackIgnored()))
            {
                if (!DamageStoppers.Any(x => x.IsDamageIgnored()))
                {
                    var damage = attack.GetDamage(ActivePlayer, NonActivePlayer);
                    damage.NormalDamage = GetDamageAfterWeaknessAndResistance(damage.NormalDamage, ActivePlayer.ActivePokemonCard, NonActivePlayer.ActivePokemonCard);

                    NonActivePlayer.ActivePokemonCard.DealDamage(damage);

                    if (NonActivePlayer.ActivePokemonCard.Ability?.TriggerType == TriggerType.TakesDamage && !damage.IsZero())
                        NonActivePlayer.ActivePokemonCard.Ability?.Trigger(NonActivePlayer, ActivePlayer, damage.NormalDamage + damage.DamageWithoutResistAndWeakness);
                    if (ActivePlayer.ActivePokemonCard.Ability?.TriggerType == TriggerType.DealsDamage && !damage.IsZero())
                        ActivePlayer.ActivePokemonCard.Ability?.Trigger(ActivePlayer, NonActivePlayer, damage.NormalDamage + damage.DamageWithoutResistAndWeakness);
                }

                attack.ProcessEffects(this, ActivePlayer, NonActivePlayer);
            }

            PostAttack();
        }

        private int GetDamageAfterWeaknessAndResistance(int damage, PokemonCard attacker, PokemonCard defender)
        {
            int realDamage = damage;

            if (defender.Resistance == attacker.PokemonType)
            {
                realDamage -= 30;
            }
            if (defender.Weakness == attacker.PokemonType)
            {
                realDamage *= 2;
            }

            return Math.Max(realDamage, 0);
        }

        public void PostAttack()
        {
            AttackStoppers.ForEach(attackStopper => attackStopper.TurnsLeft--);
            DamageStoppers.ForEach(damageStopper => damageStopper.TurnsLeft--);

            AttackStoppers = AttackStoppers.Where(attackStopper => attackStopper.TurnsLeft > 0).ToList();
            DamageStoppers = DamageStoppers.Where(damageStopper => damageStopper.TurnsLeft > 0).ToList();

            CheckDeadPokemon();
            CheckEndTurn();
        }

        public void PlayPokemon(PokemonCard pokemon)
        {
            ActivePlayer.PlayCard(pokemon);

            if(pokemon.Ability?.TriggerType == TriggerType.EnterPlay)
                pokemon.Ability?.Trigger(ActivePlayer, NonActivePlayer, 0);
        }

        public void PlayerTrainerCard(TrainerCard trainerCard)
        {
            if (PassiveAbilities.Any(ability => ability.ModifierType == PassiveModifierType.StopTrainerCast))
                return;

            trainerCard.Process(this, ActivePlayer, NonActivePlayer);
            ActivePlayer.Hand.Remove(trainerCard);
            ActivePlayer.DiscardPile.Add(trainerCard);
        }

        private void CheckDeadPokemon()
        {
            if(NonActivePlayer.ActivePokemonCard.IsDead())
            {
                NonActivePlayer.ActivePokemonCard.KnockedOutBy = ActivePlayer.ActivePokemonCard;

                if(NonActivePlayer.ActivePokemonCard.Ability?.TriggerType == TriggerType.Dies)
                    NonActivePlayer.ActivePokemonCard.Ability?.Trigger(NonActivePlayer, ActivePlayer, 0);
                if(ActivePlayer.ActivePokemonCard.Ability?.TriggerType == TriggerType.Kills)
                    ActivePlayer.ActivePokemonCard.Ability?.Trigger(ActivePlayer, NonActivePlayer, 0);

                NonActivePlayer.ActivePokemonCard.KnockedOutBy = ActivePlayer.ActivePokemonCard;
                StateQueue.Enqueue(GameFieldState.ActivePlayerSelectingPrize);
                StateQueue.Enqueue(GameFieldState.UnActivePlayerSelectingFromBench);
            }
            if(ActivePlayer.ActivePokemonCard.IsDead())
            {
                ActivePlayer.ActivePokemonCard.KnockedOutBy = NonActivePlayer.ActivePokemonCard;
                StateQueue.Enqueue(GameFieldState.UnActivePlayerSelectingPrize);
                StateQueue.Enqueue(GameFieldState.ActivePlayerSelectingFromBench);
            }
        }

        private void CheckEndTurn()
        {
            if(StateQueue.Any())
                GameState = StateQueue.Dequeue();
            else
                EndTurn();

            if (GameState == GameFieldState.EndAttack)
                EndTurn();
        }

        public void SelectPrizeCard(Card prizeCard)
        {
            if(GameState == GameFieldState.ActivePlayerSelectingPrize)
                ActivePlayer.DrawPrizeCard(prizeCard);
            else if(GameState == GameFieldState.UnActivePlayerSelectingPrize)
                NonActivePlayer.DrawPrizeCard(prizeCard);

            CheckEndTurn();
        }

        public void EndTurn()
        {
            TemporaryPassiveAbilities.ForEach(x => x.TurnsLeft--);
            TemporaryPassiveAbilities = TemporaryPassiveAbilities.Where(x => x.TurnsLeft > 0).ToList();

            ActivePlayer.EndTurn();
            NonActivePlayer.EndTurn();
            CheckDeadPokemon();

            if(StateQueue.Any())
                CheckEndTurn();
            else
            {
                SwapActivePlayer();
                StartNextTurn();
            }
        }

        private void StartNextTurn()
        {
            ActivePlayer.ResetTurn();
            NonActivePlayer.ResetTurn();
            StateQueue.Clear();

            GameState = GameFieldState.TurnStarting;
            ActivePlayer.DrawCards(1);

            GameState = GameFieldState.InTurn;
        }

        public void SwapActivePlayer()
        {
            ActivePlayer = Players.First(x => !x.Id.Equals(ActivePlayer.Id));
            NonActivePlayer = Players.First(p => !p.Id.Equals(ActivePlayer.Id));
        }

        public List<PassiveAbility> GetAllPassiveAbilities()
        {
            if (PassiveAbilities.Any(ability => ability.ModifierType == PassiveModifierType.NoPokemonPowers))
                return new List<PassiveAbility>();

            var passiveAbilities = new List<PassiveAbility>(PassiveAbilities);
            passiveAbilities.AddRange(TemporaryPassiveAbilities);

            return passiveAbilities;
        }

        public GameFieldState GameState { get; set; }

        public List<Player> Players { get; set; }
        public Player ActivePlayer { get; set; }
        public Player NonActivePlayer { get; set; }
        public int Mode { get; set; }
        public Queue<GameFieldState> StateQueue { get; set; }

        public List<AttackStopper> AttackStoppers { get; set; }
        public List<DamageStopper> DamageStoppers { get; set; }
        public List<PassiveAbility> PassiveAbilities { get; set; }
        public List<TemporaryPassiveAbility> TemporaryPassiveAbilities { get; set; }
        public bool PrizeCardsFaceUp { get; set; }
    }
}
