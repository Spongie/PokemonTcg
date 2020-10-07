using System;
using System.Collections.Generic;
using System.Linq;
using NetworkingCore;
using TCGCards.Core.GameEvents;
using TCGCards.Core.Messages;
using TCGCards.Core.SpecialAbilities;

namespace TCGCards.Core
{
    public class GameField
    {
        public const int StartingHandsize = 7;
        public const int PrizeCardCount = 6;
        public const int ConfusedDamage = 30;
        public const int BenchMaxSize = 5;
        private readonly object lockObject = new object();
        private readonly HashSet<NetworkId> playersSetStartBench;
        
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

        public int FlipCoins(int coins)
        {
            var heads = CoinFlipper.FlipCoins(coins);

            GameLog.AddMessage($"Flips 2 coins and gets {heads} heads");

            return heads;
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
                evolution.EvolvedThisTurn = true;
            }

            for (var i = 0; i < ActivePlayer.BenchedPokemon.Count; i++)
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

            for (var i = 0; i < 20; i++)
            {
                ActivePlayer.Deck.Cards.Push(new TestPokemonCard(ActivePlayer));
                NonActivePlayer.Deck.Cards.Push(new TestPokemonCard(NonActivePlayer));
            }
        }

        public void OnActivePokemonSelected(NetworkId ownerId, PokemonCard activePokemon)
        {
            Player owner = Players.First(p => p.Id.Equals(ownerId));

            if (activePokemon.Stage == 0)
            {
                GameLog.AddMessage($"{owner.NetworkPlayer?.Name} is setting {activePokemon.GetName()} as active");
                owner.SetActivePokemon(activePokemon);
            }
            else
            {
                return;
            }

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
            IEnumerable<CostModifierAbility> costModifierAbilities = GetAllPassiveAbilities()
                .OfType<CostModifierAbility>()
                .Where(ability => ability.IsActive()
                    && ability.ModifierType == PassiveModifierType.RetreatCost
                    && !ability.GetUnAffectedCards().Contains(ActivePlayer.ActivePokemonCard.Id));

            var retreatCost = ActivePlayer.ActivePokemonCard.RetreatCost + costModifierAbilities.Sum(ability => ability.ExtraCost.Sum(x => x.Amount));

            return card.AttachedEnergy.Sum(energy => energy.GetEnergry().Amount) >= retreatCost;
        }

        public void OnPokemonRetreated(PokemonCard replacementCard, IEnumerable<EnergyCard> payedEnergy)
        {
            if (!CanRetreat(ActivePlayer.ActivePokemonCard))
            {
                GameLog.AddMessage("Tried to retreat but did not have enough energy");
                return;
            }

            foreach (Ability ability in NonActivePlayer.GetAllPokemonCards().Select(pokemon => pokemon.Ability).Where(ability => ability?.TriggerType == TriggerType.OpponentRetreats))
            {
                GameLog.AddMessage($"Ability {ability.Name} triggers becasue of retreat");
                ability.Trigger(NonActivePlayer, ActivePlayer, 0, GameLog);
            }
            
            ActivePlayer.RetreatActivePokemon(replacementCard, payedEnergy);
        }

        public void OnBenchPokemonSelected(Player owner, IEnumerable<PokemonCard> selectedPokemons)
        {
            foreach (PokemonCard pokemon in selectedPokemons)
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
                        Players.ForEach(x => x.SetPrizeCards(PrizeCardCount));
                        GameState = GameFieldState.InTurn;
                    }
                }
            }
        }

        public Card FindCardById(NetworkId id)
        {
            foreach (Player player in Players)
            {
                foreach (Card card in player.Hand)
                {
                    if (card.Id.Equals(id))
                    {
                        return card;
                    }
                }

                foreach (PokemonCard pokemon in player.GetAllPokemonCards())
                {
                    if (pokemon.Id.Equals(id))
                    {
                        return pokemon;
                    }

                    foreach (EnergyCard energy in pokemon.AttachedEnergy)
                    {
                        if (energy.Id.Equals(id))
                        {
                            return energy;
                        }
                    }
                }

                foreach (Card card in player.DiscardPile)
                {
                    if (card.Id.Equals(id))
                    {
                        return card;
                    }
                }

                foreach (Card card in player.PrizeCards)
                {
                    if (card.Id.Equals(id))
                    {
                        return card;
                    }
                }

                foreach (Card card in player.Deck.Cards)
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
            foreach (Player player in Players)
            {
                foreach (PokemonCard pokemon in player.GetAllPokemonCards())
                {
                    foreach (Attack attack in pokemon.Attacks)
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
            foreach (Player player in Players)
            {
                foreach (PokemonCard card in player.Hand.OfType<PokemonCard>())
                {
                    if (card.Ability != null && card.Ability.Id.Equals(id))
                    {
                        return card.Ability;
                    }
                }

                foreach (PokemonCard pokemon in player.GetAllPokemonCards())
                {
                    if (pokemon.Ability != null && pokemon.Ability.Id.Equals(id))
                    {
                        return pokemon.Ability;
                    }
                }

                foreach (PokemonCard card in player.DiscardPile.OfType<PokemonCard>())
                {
                    if (card.Ability != null && card.Ability.Id.Equals(id))
                    {
                        return card.Ability;
                    }
                }

                foreach (PokemonCard card in player.PrizeCards.OfType<PokemonCard>())
                {
                    if (card.Ability != null && card.Ability.Id.Equals(id))
                    {
                        return card.Ability;
                    }
                }

                foreach (PokemonCard card in player.Deck.Cards.OfType<PokemonCard>())
                {
                    if (card.Ability != null && card.Ability.Id.Equals(id))
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

            ActivePlayer.OnCardsDrawn += PlayerDrewCards;
            NonActivePlayer.OnCardsDrawn += PlayerDrewCards;

            GameLog.AddMessage($"{ActivePlayer.NetworkPlayer?.Name} goes first");

            foreach (Player player in Players)
            {
                do
                {
                    foreach (Card card in player.Hand)
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

        private void PlayerDrewCards(object sender, PlayerCardDraw e)
        {
            var gameEvent = new DrawCardsEvent(CreateGameInfo(true))
            {
                Amount = e.Amount,
                Player = ActivePlayer.Id,
                Cards = e.Cards
            };

            SendEventMessage(gameEvent, ActivePlayer);

            gameEvent.GameField = CreateGameInfo(false);
            gameEvent.Cards = new List<Card>();

            SendEventMessage(gameEvent, NonActivePlayer);
        }

        public void ActivateAbility(Ability ability)
        {
            GameLog.AddMessage($"{ActivePlayer.NetworkPlayer?.Name} activates ability {ability.Name}");

            ability.Trigger(ActivePlayer, NonActivePlayer, 0, GameLog);

            CheckDeadPokemon();

            PushGameLogUpdatesToPlayers();
        }

        public void Attack(Attack attack)
        {
            if (attack.Disabled || !attack.CanBeUsed(this, ActivePlayer, NonActivePlayer))
            {
                GameLog.AddMessage($"Attack not used becasue GameFirst: {FirstTurn} Disabled: {attack.Disabled} or CanBeUsed:{attack.CanBeUsed(this, ActivePlayer, NonActivePlayer)}");
                return;
            }

            GameLog.AddMessage($"{ActivePlayer.NetworkPlayer?.Name} activates attack {attack.Name}");

            if (ActivePlayer.ActivePokemonCard.IsConfused && CoinFlipper.FlipCoin() == CoinFlipper.TAILS)
            {
                HitItselfInConfusion();
                return;
            }

            if (ActivePlayer.ActivePokemonCard.Ability?.TriggerType == TriggerType.Attacks)
            {
                GameLog.AddMessage($"{ActivePlayer.ActivePokemonCard.Ability.Name} is triggered by the attack");
                ActivePlayer.ActivePokemonCard.Ability?.Trigger(ActivePlayer, NonActivePlayer, 0, GameLog);
            }

            if (AttackStoppers.Any(x => x.IsAttackIgnored(NonActivePlayer.ActivePokemonCard)) || ActivePlayer.ActivePokemonCard.AttackStoppers.Any(x => x.IsAttackIgnored(NonActivePlayer.ActivePokemonCard)))
            {
                GameLog.AddMessage("Attack fully ignored because of effect");
                if (!IgnorePostAttack)
                {
                    PostAttack();
                }
                return;
            }

            if (DamageStoppers.Any(x => x.IsDamageIgnored()))
            {
                GameLog.AddMessage("Damage ignored because of effect");
                if (!IgnorePostAttack)
                {
                    PostAttack();
                }
                return;
            }

            DealDamageWithAttack(attack);

            attack.ProcessEffects(this, ActivePlayer, NonActivePlayer);

            if (!IgnorePostAttack)
            {
                PostAttack();
            }
        }

        private void DealDamageWithAttack(Attack attack)
        {
            Damage damage = attack.GetDamage(ActivePlayer, NonActivePlayer, this);
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

        private void HitItselfInConfusion()
        {
            GameLog.AddMessage($"{ActivePlayer.ActivePokemonCard.GetName()} hurt itself in its confusion");
            ActivePlayer.ActivePokemonCard.DealDamage(new Damage(0, ConfusedDamage), GameLog);

            if (ActivePlayer.ActivePokemonCard.Ability?.TriggerType == TriggerType.DealsDamage)
            {
                ActivePlayer.ActivePokemonCard.Ability.SetTarget(ActivePlayer.ActivePokemonCard);
                ActivePlayer.ActivePokemonCard.Ability.Trigger(ActivePlayer, NonActivePlayer, ConfusedDamage, GameLog);
                ActivePlayer.ActivePokemonCard.Ability.SetTarget(null);
            }

            if (!IgnorePostAttack)
            {
                PostAttack();
            }
        }

        private int GetDamageAfterWeaknessAndResistance(int damage, PokemonCard attacker, PokemonCard defender)
        {
            var realDamage = damage;

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

        public void PlayTrainerCard(TrainerCard trainerCard)
        {
            if (GetAllPassiveAbilities().Any(ability => ability.ModifierType == PassiveModifierType.StopTrainerCast))
                return;

            GameLog.AddMessage(ActivePlayer.NetworkPlayer?.Name + " Plays " + trainerCard.GetName());
            PushGameLogUpdatesToPlayers();

            var trainerEvent = new TrainerCardPlayed(CreateGameInfo(true))
            {
                Card = trainerCard,
                Player = ActivePlayer.Id
            };

            SendEventMessage(trainerEvent, ActivePlayer);

            trainerEvent.GameField = CreateGameInfo(false);
            SendEventMessage(trainerEvent, NonActivePlayer);

            trainerCard.Process(this, ActivePlayer, NonActivePlayer);
            ActivePlayer.DiscardCard(trainerCard);

            if (ActivePlayer.IsDead)
            {
                GameLog.AddMessage($"{ActivePlayer.NetworkPlayer?.Name} loses because they drew to many cards");
                EndGame(NonActivePlayer.Id);
            }
        }

        private void SendEventMessage(Event playEvent, Player target)
        {
            var message = new EventMessage(playEvent).ToNetworkMessage(Id);

            target.NetworkPlayer?.Send(message);
        }

        public GameFieldInfo CreateGameInfo(bool forActive)
        {
            var activePlayer = new PlayerInfo
            {
                Id = ActivePlayer.Id,
                ActivePokemon = ActivePlayer.ActivePokemonCard,
                BenchedPokemon = ActivePlayer.BenchedPokemon.OfType<Card>().ToList(),
                CardsInDeck = ActivePlayer.Deck.Cards.Count,
                CardsInDiscard = ActivePlayer.DiscardPile,
                CardsInHand = ActivePlayer.Hand.Count,
                PrizeCards = ActivePlayer.PrizeCards
            };

            var nonActivePlayer = new PlayerInfo
            {
                Id = ActivePlayer.Id,
                ActivePokemon = NonActivePlayer.ActivePokemonCard,
                BenchedPokemon = NonActivePlayer.BenchedPokemon.OfType<Card>().ToList(),
                CardsInDeck = NonActivePlayer.Deck.Cards.Count,
                CardsInDiscard = NonActivePlayer.DiscardPile,
                CardsInHand = NonActivePlayer.Hand.Count,
                PrizeCards = NonActivePlayer.PrizeCards
            };

            if (forActive)
            {
                return new GameFieldInfo
                {
                    Me = activePlayer,
                    Opponent = nonActivePlayer,
                    CardsInMyHand = ActivePlayer.Hand,
                    CurrentState = GameState,
                    ActivePlayer = ActivePlayer.Id
                };
            }
            else
            {
                return new GameFieldInfo
                {
                    Me = nonActivePlayer,
                    Opponent = activePlayer,
                    CardsInMyHand = NonActivePlayer.Hand,
                    CurrentState = GameState,
                    ActivePlayer = ActivePlayer.Id
                };
            }
        }

        private void CheckDeadPokemon()
        {
            if(NonActivePlayer.ActivePokemonCard != null && NonActivePlayer.ActivePokemonCard.IsDead())
            {
                GameLog.AddMessage(NonActivePlayer.ActivePokemonCard.GetName() + " Dies");

                NonActivePlayer.ActivePokemonCard.KnockedOutBy = ActivePlayer.ActivePokemonCard;

                if(NonActivePlayer.ActivePokemonCard.Ability?.TriggerType == TriggerType.Dies)
                    NonActivePlayer.ActivePokemonCard.Ability?.Trigger(NonActivePlayer, ActivePlayer, 0, GameLog);
                if(ActivePlayer.ActivePokemonCard.Ability?.TriggerType == TriggerType.Kills)
                    ActivePlayer.ActivePokemonCard.Ability?.Trigger(ActivePlayer, NonActivePlayer, 0, GameLog);

                NonActivePlayer.ActivePokemonCard.KnockedOutBy = ActivePlayer.ActivePokemonCard;

                PushGameLogUpdatesToPlayers();

                if (ActivePlayer.PrizeCards.Count == 1)
                {
                    GameLog.AddMessage(NonActivePlayer.NetworkPlayer?.Name + $" has no pokémon left, {ActivePlayer.NetworkPlayer?.Name} wins the game");
                    EndGame(ActivePlayer.Id);
                    return;
                }
                else if (NonActivePlayer.BenchedPokemon.Count == 0)
                {
                    GameLog.AddMessage(ActivePlayer.NetworkPlayer?.Name + " wins the game");
                    EndGame(ActivePlayer.Id);
                    return;
                }
                else
                {
                    SendUpdateToPlayers();
                    ActivePlayer.SelectPriceCard(1);
                }

                NonActivePlayer.KillActivePokemon();
                if (NonActivePlayer.BenchedPokemon.Any())
                {
                    SendUpdateToPlayers();
                    NonActivePlayer.SelectActiveFromBench();
                }
                else
                {
                    GameLog.AddMessage(NonActivePlayer.NetworkPlayer?.Name + $" has no pokémon left, {ActivePlayer.NetworkPlayer?.Name} wins the game");
                    EndGame(ActivePlayer.Id);
                    return;
                }
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
                }
                else
                {
                    GameLog.AddMessage(ActivePlayer.NetworkPlayer?.Name + $" has no pokémon left, {NonActivePlayer.NetworkPlayer?.Name} wins the game");
                    EndGame(NonActivePlayer.Id);
                    return;
                }
            }

            foreach (PokemonCard pokemon in NonActivePlayer.BenchedPokemon)
            {
                if (!pokemon.IsDead())
                {
                    continue;
                }

                if (ActivePlayer.PrizeCards.Count <= 1)
                {
                    GameLog.AddMessage(ActivePlayer.NetworkPlayer?.Name + " wins the game");
                    EndGame(ActivePlayer.Id);
                    return;
                }
                else
                {
                    PushStateToPlayer(NonActivePlayer);
                    PushInfoToPlayer("Opponent is selecting a prize card", NonActivePlayer);
                    ActivePlayer.SelectPriceCard(1);
                }
            }

            foreach (PokemonCard pokemon in ActivePlayer.BenchedPokemon)
            {
                if (!pokemon.IsDead())
                {
                    continue;
                }

                if (NonActivePlayer.PrizeCards.Count <= 1)
                {
                    GameLog.AddMessage(NonActivePlayer.NetworkPlayer?.Name + " wins the game");
                    EndGame(NonActivePlayer.Id);
                    return;
                }
                else
                {
                    PushStateToPlayer(ActivePlayer);
                    PushInfoToPlayer("Opponent is selecting a prize card", ActivePlayer);
                    NonActivePlayer.SelectPriceCard(1);
                }
            }

            PushGameLogUpdatesToPlayers();
        }

        private void EndGame(NetworkId winner)
        {
            GameState = GameFieldState.GameOver;
            foreach (Player player in Players)
            {
                player.NetworkPlayer?.Send(new GameOverMessage(winner).ToNetworkMessage(Id));
            }
        }

        public void EndTurn()
        {
            TemporaryPassiveAbilities.ForEach(x => x.TurnsLeft--);
            TemporaryPassiveAbilities = TemporaryPassiveAbilities.Where(x => x.TurnsLeft > 0).ToList();

            ActivePlayer.EndTurn();

            foreach (var pokemon in ActivePlayer.GetAllPokemonCards())
            {
                foreach (var attack in pokemon.Attacks)
                {
                    attack.Disabled = false;
                }
            }

            foreach (var pokemon in ActivePlayer.GetAllPokemonCards())
            {
                pokemon.AbilityDisabled = false;
            }

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

            if (ActivePlayer.IsDead)
            {
                GameLog.AddMessage($"{ActivePlayer.NetworkPlayer?.Name} loses because they drew to many cards");
                EndGame(NonActivePlayer.Id);
                return;
            }
            
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

            foreach (Player player in Players)
            {
                player.NetworkPlayer?.Send(message);
            }

            GameLog.CommitMessages();
        }

        private void PushStateToPlayer(Player player)
        {
            var gameMessage = new GameFieldMessage(this);
            player.NetworkPlayer.Send(gameMessage.ToNetworkMessage(Id));
        }

        private void PushInfoToPlayer(string info, Player player)
        {
            var message = new InfoMessage(info);
            player.NetworkPlayer.Send(message.ToNetworkMessage(Id));
        }

        private void SendUpdateToPlayers()
        {
            foreach (var player in Players)
            {
                var gameMessage = new GameFieldMessage(this);
                player.NetworkPlayer.Send(gameMessage.ToNetworkMessage(Id));
            }
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
