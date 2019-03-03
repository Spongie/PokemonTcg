using System;
using System.Collections.Generic;
using System.Linq;
using NetworkingCore;
using TCGCards.Core.Messages;
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
            playersSetStartBench = new HashSet<NetworkId>();
            AttackStoppers = new List<AttackStopper>();
            DamageStoppers = new List<DamageStopper>();
            TemporaryPassiveAbilities = new List<TemporaryPassiveAbility>();
            GameState = GameFieldState.WaitingForConnection;
        }

        public void RevealCardsTo(List<Card> pickedCards, Player nonActivePlayer)
        {
            //TODO: Complete this
        }

        public void EvolvePokemon(PokemonCard basePokemon, PokemonCard evolution)
        {
            if (!basePokemon.CanEvolve() || !basePokemon.CanEvolveTo(evolution))
            {
                return;
            }

            if (ActivePlayer.ActivePokemonCard.Id.Equals(basePokemon.Id))
            {
                ActivePlayer.ActivePokemonCard = basePokemon.Evolve(evolution);
            }

            for (int i = 0; i < ActivePlayer.BenchedPokemon.Count; i++)
            {
                if (ActivePlayer.BenchedPokemon[i].Id.Equals(basePokemon.Id))
                {
                    ActivePlayer.BenchedPokemon[i] = basePokemon.Evolve(evolution);
                }
            }
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
                    if (!owner.Hand.OfType<PokemonCard>().Any(card => card.Stage == 0))
                    {
                        playersSetStartBench.Add(owner.Id);
                    }
                    if (Players.All(p => p.ActivePokemonCard != null))
                    {
                        GameState = GameFieldState.BothSelectingBench;
                    }
                }
            }
        }

        public bool CanRetreat(PokemonCard card)
        {
            var costModifierAbilities = GetAllPassiveAbilities()
                .OfType<CostModifierAbility>()
                .Where(ability => ability.IsActive()
                    && ability.ModifierType == PassiveModifierType.RetreatCost
                    && !ability.GetUnAffectedCards().Contains(ActivePlayer.ActivePokemonCard.Id));

            int retreatCost = ActivePlayer.ActivePokemonCard.RetreatCost + costModifierAbilities.Sum(ability => ability.ExtraCost.Sum(x => x.Amount));

            return card.AttachedEnergy.Sum(energy => energy.GetEnergry().Amount) >= retreatCost;
        }

        public void OnPokemonRetreated(PokemonCard replacementCard, IEnumerable<EnergyCard> payedEnergy)
        {
            if (!CanRetreat(ActivePlayer.ActivePokemonCard))
            {
                return;
            }

            foreach (var ability in NonActivePlayer.GetAllPokemonCards().Select(pokemon => pokemon.Ability).Where(ability => ability?.TriggerType == TriggerType.OpponentRetreats))
            {
                ability.Trigger(NonActivePlayer, ActivePlayer, 0);
            }
            
            ActivePlayer.RetreatActivePokemon(replacementCard, payedEnergy);
        }

        public void OnBenchPokemonSelected(Player owner, List<PokemonCard> selectedPokemons)
        {
            foreach (var pokemon in selectedPokemons)
            {
                owner.SetBenchedPokemon(pokemon);
            }

            if (GameState == GameFieldState.BothSelectingBench)
            {
                lock (lockObject)
                {
                    playersSetStartBench.Add(owner.Id);
                    if (playersSetStartBench.Count == 2)
                    {
                        Players.ForEach(x => x.SetPrizeCards(PriceCards));
                        GameState = GameFieldState.InTurn;
                    }
                }
            }
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
        }

        public void ActivateAbility(Ability ability)
        {
            ability.Trigger(ActivePlayer, NonActivePlayer, 0);
        }

        public void Attack(Attack attack)
        {
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
                    {
                        ActivePlayer.ActivePokemonCard.Ability.SetTarget(NonActivePlayer.ActivePokemonCard);
                        ActivePlayer.ActivePokemonCard.Ability?.Trigger(ActivePlayer, NonActivePlayer, damage.NormalDamage + damage.DamageWithoutResistAndWeakness);
                        ActivePlayer.ActivePokemonCard.Ability.SetTarget(null);
                    }
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
            if (AbilityTriggeredByDeath())
            {
                NonActivePlayer.ActivePokemonCard.KnockedOutBy = ActivePlayer.ActivePokemonCard;
                NonActivePlayer.ActivePokemonCard.Ability.Trigger(NonActivePlayer, ActivePlayer, 0);
            }

            AttackStoppers.ForEach(attackStopper => attackStopper.TurnsLeft--);
            DamageStoppers.ForEach(damageStopper => damageStopper.TurnsLeft--);

            AttackStoppers = AttackStoppers.Where(attackStopper => attackStopper.TurnsLeft > 0).ToList();
            DamageStoppers = DamageStoppers.Where(damageStopper => damageStopper.TurnsLeft > 0).ToList();

            CheckDeadPokemon();
            EndTurn();
        }

        private bool AbilityTriggeredByDeath()
        {
            return NonActivePlayer.ActivePokemonCard.IsDead()
                            && NonActivePlayer.ActivePokemonCard.Ability?.TriggerType == TriggerType.KilledByAttack
                            && NonActivePlayer.ActivePokemonCard.Ability.CanActivate();
        }

        public void PlayPokemon(PokemonCard pokemon)
        {
            ActivePlayer.PlayCard(pokemon);

            if(pokemon.Ability?.TriggerType == TriggerType.EnterPlay)
                pokemon.Ability?.Trigger(ActivePlayer, NonActivePlayer, 0);
        }

        public void PlayerTrainerCard(TrainerCard trainerCard)
        {
            if (GetAllPassiveAbilities().Any(ability => ability.ModifierType == PassiveModifierType.StopTrainerCast))
                return;

            trainerCard.Process(this, ActivePlayer, NonActivePlayer);
            ActivePlayer.DiscardCard(trainerCard);
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

                if (ActivePlayer.PrizeCards.Count == 1)
                {
                    //TODO Handle game win
                }
                else
                {
                    ActivePlayer.SelectPriceCard(1);
                }
                
                NonActivePlayer.SelectActiveFromBench();
            }
            if(ActivePlayer.ActivePokemonCard.IsDead())
            {
                ActivePlayer.ActivePokemonCard.KnockedOutBy = NonActivePlayer.ActivePokemonCard;
                NonActivePlayer.SelectPriceCard(1);
                ActivePlayer.SelectActiveFromBench();
            }
        }

        public void EndTurn()
        {
            TemporaryPassiveAbilities.ForEach(x => x.TurnsLeft--);
            TemporaryPassiveAbilities = TemporaryPassiveAbilities.Where(x => x.TurnsLeft > 0).ToList();

            ActivePlayer.EndTurn();
            NonActivePlayer.EndTurn();
            CheckDeadPokemon();
            SwapActivePlayer();
            StartNextTurn();
        }

        private void StartNextTurn()
        {
            ActivePlayer.ResetTurn();
            NonActivePlayer.ResetTurn();
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
            var passiveAbilities = new List<PassiveAbility>(ActivePlayer.GetAllPokemonCards().Select(pokemon => pokemon.Ability).OfType<PassiveAbility>());
            passiveAbilities.AddRange(NonActivePlayer.GetAllPokemonCards().Select(pokemon => pokemon.Ability).OfType<PassiveAbility>());
            passiveAbilities.AddRange(TemporaryPassiveAbilities);

            if (passiveAbilities.Any(ability => ability.ModifierType == PassiveModifierType.NoPokemonPowers))
                return new List<PassiveAbility>();
            
            return passiveAbilities;
        }

        public GameFieldState GameState { get; set; }

        public List<Player> Players { get; set; }
        public Player ActivePlayer { get; set; }
        public Player NonActivePlayer { get; set; }
        public int Mode { get; set; }

        public List<AttackStopper> AttackStoppers { get; set; }
        public List<DamageStopper> DamageStoppers { get; set; }
        public List<TemporaryPassiveAbility> TemporaryPassiveAbilities { get; set; }
        public bool PrizeCardsFaceUp { get; set; }
    }
}
