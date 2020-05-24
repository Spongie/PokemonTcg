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
            Id = NetworkId.Generate();
            Players = new List<Player>();
            playersSetStartBench = new HashSet<NetworkId>();
            AttackStoppers = new List<AttackStopper>();
            DamageStoppers = new List<DamageStopper>();
            TemporaryPassiveAbilities = new List<TemporaryPassiveAbility>();
            GameState = GameFieldState.WaitingForConnection;
            GameLog = new GameLog();
        }

        public void RevealCardsTo(List<NetworkId> pickedCards, Player nonActivePlayer)
        {
            foreach (var card in pickedCards)
            {
                //card.IsRevealed = true;
            }
            //TODO: Complete this
        }

        public void EvolvePokemon(PokemonCard basePokemon, PokemonCard evolution)
        {
            if (!basePokemon.CanEvolve() || !basePokemon.CanEvolveTo(evolution))
            {
                return;
            }

            GameLog.AddMessage($"Evolving {basePokemon.GetName()} to {evolution.GetName()}");

            if (ActivePlayer.ActivePokemonCard.Id.Equals(basePokemon.Id))
            {
                basePokemon.Evolve(evolution);
                ActivePlayer.ActivePokemonCard = evolution;
            }

            for (int i = 0; i < ActivePlayer.BenchedPokemon.Count; i++)
            {
                if (ActivePlayer.BenchedPokemon[i].Id.Equals(basePokemon.Id))
                {
                    basePokemon.Evolve(evolution);
                    ActivePlayer.BenchedPokemon[i] = evolution;
                }
            }

            if (ActivePlayer.Hand.Contains(evolution))
            {
                ActivePlayer.Hand.Remove(evolution);
            }

            evolution.IsRevealed = true;

            PushGameLogUpdatesToPlayers();
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

            GameLog.AddMessage($"{owner.NetworkPlayer?.Name} is setting {activePokemon.GetName()} as active");

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

            PushGameLogUpdatesToPlayers();
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
                ability.Trigger(NonActivePlayer, ActivePlayer, 0, GameLog);
            }
            
            ActivePlayer.RetreatActivePokemon(replacementCard, payedEnergy);
        }

        public void OnBenchPokemonSelected(Player owner, IEnumerable<PokemonCard> selectedPokemons)
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

        public Card FindCardById(NetworkId id)
        {
            foreach (var player in Players)
            {
                foreach (var card in player.Hand)
                {
                    if (card.Id.Equals(id))
                    {
                        return card;
                    }
                }

                foreach (var pokemon in player.BenchedPokemon)
                {
                    if (pokemon.Id.Equals(id))
                    {
                        return pokemon;
                    }

                    foreach (var energy in pokemon.AttachedEnergy)
                    {
                        if (energy.Id.Equals(id))
                        {
                            return energy;
                        }
                    }
                }

                if (player.ActivePokemonCard != null)
                {
                    if (player.ActivePokemonCard.Id.Equals(id))
                    {
                        return player.ActivePokemonCard;
                    }

                    foreach (var energy in player.ActivePokemonCard.AttachedEnergy)
                    {
                        if (energy.Id.Equals(id))
                        {
                            return energy;
                        }
                    }
                }

                foreach (var card in player.Deck.Cards)
                {
                    if (card.Id.Equals(id))
                    {
                        return card;
                    }
                }

                foreach (var card in player.DiscardPile)
                {
                    if (card.Id.Equals(id))
                    {
                        return card;
                    }
                }

                foreach (var card in player.PrizeCards)
                {
                    if (card.Id.Equals(id))
                    {
                        return card;
                    }
                }
            }

            return null;
        }

        public Attack FindAttackById(NetworkId attackId)
        {
            foreach (var player in Players)
            {
                if (player.ActivePokemonCard != null)
                {
                    foreach (var attack in player.ActivePokemonCard.Attacks)
                    {
                        if (attack.Id.Equals(attackId))
                        {
                            return attack;
                        }
                    }
                }

                foreach (var pokemon in player.BenchedPokemon)
                {
                    foreach (var attack in pokemon.Attacks)
                    {
                        if (attack.Id.Equals(attackId))
                        {
                            return attack;
                        }
                    }
                }
            }

            return null;
        }

        public Ability FindAbilityById(NetworkId id)
        {
            foreach (var player in Players)
            {
                foreach (var card in player.Hand.OfType<PokemonCard>())
                {
                    if (card.Ability.Id.Equals(id))
                    {
                        return card.Ability;
                    }
                }

                foreach (var pokemon in player.BenchedPokemon.OfType<PokemonCard>())
                {
                    if (pokemon.Ability.Id.Equals(id))
                    {
                        return pokemon.Ability;
                    }
                }

                if (player.ActivePokemonCard != null && player.ActivePokemonCard.Ability.Id.Equals(id))
                {
                    return player.ActivePokemonCard.Ability;
                }

                foreach (var card in player.Deck.Cards.OfType<PokemonCard>())
                {
                    if (card.Ability.Id.Equals(id))
                    {
                        return card.Ability;
                    }
                }

                foreach (var card in player.DiscardPile.OfType<PokemonCard>())
                {
                    if (card.Ability.Id.Equals(id))
                    {
                        return card.Ability;
                    }
                }

                foreach (var card in player.PrizeCards.OfType<PokemonCard>())
                {
                    if (card.Ability.Id.Equals(id))
                    {
                        return card.Ability;
                    }
                }
            }

            return null;
        }

        public void StartGame()
        {
            GameLog.AddMessage("Game starting");
            ActivePlayer = Players[new Random().Next(2)];
            NonActivePlayer = Players.First(p => !p.Id.Equals(ActivePlayer.Id));

            GameLog.AddMessage($"{ActivePlayer.NetworkPlayer?.Name} goes first");

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
            PushGameLogUpdatesToPlayers();
        }

        public void ActivateAbility(Ability ability)
        {
            GameLog.AddMessage($"{ActivePlayer.NetworkPlayer?.Name} activates ability {ability.Name}");

            ability.Trigger(ActivePlayer, NonActivePlayer, 0, GameLog);

            PushGameLogUpdatesToPlayers();
        }

        public void Attack(Attack attack)
        {
            GameLog.AddMessage($"{ActivePlayer.NetworkPlayer?.Name} activates attack {attack.Name}");

            if (ActivePlayer.ActivePokemonCard.IsConfused && CoinFlipper.FlipCoin() == CoinFlipper.TAILS)
            {
                GameLog.AddMessage($"{ActivePlayer.ActivePokemonCard.GetName()} hurt itself in its confusion");
                ActivePlayer.ActivePokemonCard.DealDamage(new Damage(0, ConfusedDamage), GameLog);

                if (ActivePlayer.ActivePokemonCard.Ability?.TriggerType == TriggerType.DealsDamage)
                {
                    ActivePlayer.ActivePokemonCard.Ability.SetTarget(ActivePlayer.ActivePokemonCard);
                    ActivePlayer.ActivePokemonCard.Ability.Trigger(ActivePlayer, NonActivePlayer, 30, GameLog);
                    ActivePlayer.ActivePokemonCard.Ability.SetTarget(null);
                }

                if (!IgnorePostAttack)
                {
                    PostAttack();
                }

                return;
            }

            if (ActivePlayer.ActivePokemonCard.Ability?.TriggerType == TriggerType.Attacks)
            {
                GameLog.AddMessage($"{ActivePlayer.ActivePokemonCard.Ability.Name} is triggered by the attack");
                ActivePlayer.ActivePokemonCard.Ability?.Trigger(ActivePlayer, NonActivePlayer, 0, GameLog);
            }

            if (!AttackStoppers.Any(x => x.IsAttackIgnored(NonActivePlayer.ActivePokemonCard)) && !ActivePlayer.ActivePokemonCard.AttackStoppers.Any(x => x.IsAttackIgnored(NonActivePlayer.ActivePokemonCard)))
            {
                if (!DamageStoppers.Any(x => x.IsDamageIgnored()))
                {
                    var damage = attack.GetDamage(ActivePlayer, NonActivePlayer);
                    damage.NormalDamage = GetDamageAfterWeaknessAndResistance(damage.NormalDamage, ActivePlayer.ActivePokemonCard, NonActivePlayer.ActivePokemonCard);

                    var dealtDamage = NonActivePlayer.ActivePokemonCard.DealDamage(damage, GameLog);
                    attack.OnDamageDealt(dealtDamage, ActivePlayer);

                    if (NonActivePlayer.ActivePokemonCard.Ability?.TriggerType == TriggerType.TakesDamage && !damage.IsZero())
                    {
                        GameLog.AddMessage(NonActivePlayer.ActivePokemonCard.Ability.Name + "triggered by taking damage");
                        NonActivePlayer.ActivePokemonCard.Ability?.Trigger(NonActivePlayer, ActivePlayer, damage.NormalDamage + damage.DamageWithoutResistAndWeakness, GameLog);
                    }
                    if (ActivePlayer.ActivePokemonCard.Ability?.TriggerType == TriggerType.DealsDamage && !damage.IsZero())
                    {
                        GameLog.AddMessage(NonActivePlayer.ActivePokemonCard.Ability.Name + "triggered by dealing damage");
                        ActivePlayer.ActivePokemonCard.Ability.SetTarget(NonActivePlayer.ActivePokemonCard);
                        ActivePlayer.ActivePokemonCard.Ability?.Trigger(ActivePlayer, NonActivePlayer, damage.NormalDamage + damage.DamageWithoutResistAndWeakness, GameLog);
                        ActivePlayer.ActivePokemonCard.Ability.SetTarget(null);
                    }
                }
                else
                {
                    GameLog.AddMessage("Damage ignored because of effect");
                }

                attack.ProcessEffects(this, ActivePlayer, NonActivePlayer);
            }
            else
            {
                GameLog.AddMessage("Attack fully ignored because of effect");
            }

            if (!IgnorePostAttack)
            {
                PostAttack();
            }
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
                GameLog.AddMessage(NonActivePlayer.ActivePokemonCard.Ability.Name + "triggered by dying");
                NonActivePlayer.ActivePokemonCard.KnockedOutBy = ActivePlayer.ActivePokemonCard;
                NonActivePlayer.ActivePokemonCard.Ability.Trigger(NonActivePlayer, ActivePlayer, 0, GameLog);
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
                pokemon.Ability?.Trigger(ActivePlayer, NonActivePlayer, 0, GameLog);
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
            if(NonActivePlayer.ActivePokemonCard != null && NonActivePlayer.ActivePokemonCard.IsDead())
            {
                GameLog.AddMessage(NonActivePlayer.ActivePokemonCard.GetName() + "Dies");

                NonActivePlayer.ActivePokemonCard.KnockedOutBy = ActivePlayer.ActivePokemonCard;

                if(NonActivePlayer.ActivePokemonCard.Ability?.TriggerType == TriggerType.Dies)
                    NonActivePlayer.ActivePokemonCard.Ability?.Trigger(NonActivePlayer, ActivePlayer, 0, GameLog);
                if(ActivePlayer.ActivePokemonCard.Ability?.TriggerType == TriggerType.Kills)
                    ActivePlayer.ActivePokemonCard.Ability?.Trigger(ActivePlayer, NonActivePlayer, 0, GameLog);

                NonActivePlayer.ActivePokemonCard.KnockedOutBy = ActivePlayer.ActivePokemonCard;

                PushGameLogUpdatesToPlayers();

                if (ActivePlayer.PrizeCards.Count == 1)
                {
                    //TODO Handle game win
                }
                else
                {
                    ActivePlayer.SelectPriceCard(1);
                }

                NonActivePlayer.KillActivePokemon();
                if (NonActivePlayer.BenchedPokemon.Any())
                {
                    NonActivePlayer.SelectActiveFromBench();
                }//TODO Handle game win
            }

            PushGameLogUpdatesToPlayers();

            if (ActivePlayer.ActivePokemonCard != null &&ActivePlayer.ActivePokemonCard.IsDead())
            {
                GameLog.AddMessage(ActivePlayer.ActivePokemonCard.GetName() + "Dies");
                ActivePlayer.ActivePokemonCard.KnockedOutBy = NonActivePlayer.ActivePokemonCard;
                ActivePlayer.KillActivePokemon();
                NonActivePlayer.SelectPriceCard(1);
                
                if (ActivePlayer.BenchedPokemon.Any())
                {
                    ActivePlayer.SelectActiveFromBench();
                } //TODO Handle game win
            }

            //TODO CHECK DEAD BENCHED POKEMON

            PushGameLogUpdatesToPlayers();
        }

        public void EndTurn()
        {
            TemporaryPassiveAbilities.ForEach(x => x.TurnsLeft--);
            TemporaryPassiveAbilities = TemporaryPassiveAbilities.Where(x => x.TurnsLeft > 0).ToList();

            ActivePlayer.EndTurn();
            NonActivePlayer.EndTurn();
            CheckDeadPokemon();
            SwapActivePlayer();
            FirstTurn = false;
            StartNextTurn();

            PushGameLogUpdatesToPlayers();
        }

        private void StartNextTurn()
        {
            ActivePlayer.ResetTurn();
            NonActivePlayer.ResetTurn();
            ActivePlayer.DrawCards(1);
            
            if (ActivePlayer.ActivePokemonCard != null)
            {
                ActivePlayer.ActivePokemonCard.DamageTakenLastTurn = 0;
            }

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

        public void PushGameLogUpdatesToPlayers()
        {
            var message = new GameLogAddMessage(GameLog.NewMessages).ToNetworkMessage(NetworkId.Generate());

            foreach (var player in Players)
            {
                player.NetworkPlayer?.Send(message);
            }

            GameLog.CommitMessages();
        }

        public GameFieldState GameState { get; set; }
        public NetworkId Id { get; set; }
        public List<Player> Players { get; set; }
        public Player ActivePlayer { get; set; }
        public Player NonActivePlayer { get; set; }
        public int Mode { get; set; }
        public GameLog GameLog { get; set; } = new GameLog();
        public List<AttackStopper> AttackStoppers { get; set; }
        public List<DamageStopper> DamageStoppers { get; set; }
        public List<TemporaryPassiveAbility> TemporaryPassiveAbilities { get; set; }
        public bool PrizeCardsFaceUp { get; set; }
        public bool FirstTurn { get; set; } = true;
        public bool IgnorePostAttack { get; set; }
    }
}
