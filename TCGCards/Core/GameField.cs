using System;
using System.Collections.Generic;
using System.Linq;
using Entities;
using NetworkingCore;
using NetworkingCore.Messages;
using Newtonsoft.Json;
using TCGCards.Core.Abilities;
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
        private Queue<bool> forcedFlips;
        
        public GameField()
        {
            Id = NetworkId.Generate();
            Players = new List<Player>();
            playersSetStartBench = new HashSet<NetworkId>();
            DamageStoppers = new List<DamageStopper>();
            TemporaryPassiveAbilities = new List<PassiveAbility>();
            GameState = GameFieldState.WaitingForConnection;
            GameLog = new GameLog();
            forcedFlips = new Queue<bool>();
        }

        public GameField WithFlips(params bool[] flips)
        {
            foreach (var flip in flips)
            {
                forcedFlips.Enqueue(flip);
            }

            return this;
        }

        public bool IsSuccessfulFlip(bool flipCoin, bool checkLastFlip, bool checkTails)
        {
            var targetValue = checkTails ? 0 : 1;
            var lastValue = LastCoinFlipResult ? 1 : 0;

            if (checkLastFlip && lastValue != targetValue)
            {
                return false;
            }
            else if (flipCoin && FlipCoins(1) != targetValue)
            {
                return false;
            }

            return true;
        }

        public int FlipCoins(int coins)
        {
            int heads;

            if (forcedFlips.Count > 0)
            {
                heads = 0;
                while (forcedFlips.Count > 0)
                {
                    if (forcedFlips.Dequeue())
                    {
                        heads++;
                    }
                }
            }
            else
            {
                heads = CoinFlipper.FlipCoins(coins);
            }

            GameLog?.AddMessage($"Flips {coins} coins and gets {heads} heads");

            var results = new List<bool>();

            for (int i = 0; i < heads; i++)
            {
                results.Add(true);
            }
            for (int i = 0; i < coins - heads; i++)
            {
                results.Add(false);
            }

            SendEventToPlayers(new CoinsFlippedEvent(results));

            LastCoinFlipResult = heads > 0;
            LastCoinFlipHeadCount = heads;

            return heads;
        }

        public void AddPlayer(Player player)
        {
            Players.Add(player);

            foreach (var card in player.Deck.Cards)
            {
                Cards.Add(card.Id, card);

                var pokemon = card as PokemonCard;

                if (pokemon != null)
                {
                    foreach (var attack in pokemon.Attacks)
                    {
                        Attacks.Add(attack.Id, attack);
                    }

                    if (pokemon.Ability != null)
                    {
                        Abilities.Add(pokemon.Ability.Id, pokemon.Ability);
                    }
                }
            }

            foreach (var pokemon in player.GetAllPokemonCards())
            {
                Cards.Add(pokemon.Id, pokemon);

                foreach (var energy in pokemon.AttachedEnergy)
                {
                    Cards.Add(energy.Id, energy);
                }
            }
        }

        public int FlipCoinsUntilTails()
        {
            int heads = 0;

            while (true)
            {
                if (CoinFlipper.FlipCoin() == CoinFlipper.TAILS)
                {
                    break;
                }

                heads++;
            }

            GameLog?.AddMessage($"Flips {heads + 1} coins and gets {heads} heads");

            var results = new List<bool>();

            for (int i = 0; i < heads; i++)
            {
                results.Add(true);
            }
            results.Add(false);

            SendEventToPlayers(new CoinsFlippedEvent(results));

            LastCoinFlipResult = heads > 0;
            LastCoinFlipHeadCount = heads;

            return heads;
        }

        internal Player GetOpponentOf(Player player)
        {
            return Players.FirstOrDefault(p => !p.Id.Equals(player.Id));
        }

        public void SendEventToPlayers(Event gameEvent)
        {
            SendEventMessage(gameEvent, ActivePlayer);
            SendEventMessage(gameEvent, NonActivePlayer);
        }

        public void EvolvePokemon(PokemonCard basePokemon, PokemonCard evolution, bool ignoreAllChecks = false)
        {
            if (!ignoreAllChecks && !ActivePlayer.Id.Equals(basePokemon.Owner.Id) || !ActivePlayer.Id.Equals(evolution.Owner.Id))
            {
                GameLog.AddMessage("Evolution stopped by epic 1337 anti-cheat");
                return;
            }

            if (!ignoreAllChecks && GetAllPassiveAbilities().Any(ability => ability.ModifierType == PassiveModifierType.StopEvolutions))
            {
                GameLog.AddMessage("Evolution stopped by ability");
                return;
            }

            if (!ignoreAllChecks && (!basePokemon.CanEvolve() || !basePokemon.CanEvolveTo(evolution)))
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
            else
            {
                basePokemon.Evolve(evolution);
                evolution.EvolvedThisTurn = true;
                ActivePlayer.BenchedPokemon.ReplaceWith(basePokemon, evolution);
            }

            bool triggerEnterPlay = false;

            if (ActivePlayer.Hand.Contains(evolution))
            {
                ActivePlayer.Hand.Remove(evolution);
                triggerEnterPlay = true;
            }

            evolution.RevealToAll();

            SendEventToPlayers(new PokemonEvolvedEvent
            {
                TargetPokemonId = basePokemon.Id,
                NewPokemonCard = evolution
            });

            if (triggerEnterPlay)
            {
                TriggerAbilityOfType(TriggerType.EnterPlay, evolution);
            }

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
                ActivePlayer.Deck.Cards.Push(new PokemonCard(ActivePlayer));
                NonActivePlayer.Deck.Cards.Push(new PokemonCard(NonActivePlayer));
            }
        }

        public void OnActivePokemonSelected(NetworkId ownerId, PokemonCard activePokemon)
        {
            Player owner = Players.First(p => p.Id.Equals(ownerId));

            if (GameState != GameFieldState.BothSelectingActive)
            {
                if (!ActivePlayer.Id.Equals(ownerId))
                {
                    GameLog.AddMessage($"{owner?.NetworkPlayer?.Name} tried to play a pokemon when not allowed");
                    return;
                }
            }

            if (activePokemon.Stage == 0)
            {
                GameLog.AddMessage($"{owner.NetworkPlayer?.Name} is setting {activePokemon.GetName()} as active");
                owner.SetActivePokemon(activePokemon);
                if (GameState != GameFieldState.BothSelectingActive)
                {
                    SendEventToPlayers(new PokemonBecameActiveEvent
                    {
                        NewActivePokemon = activePokemon
                    });
                }
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
                        if (playersSetStartBench.Count == 2)
                        {
                            Players.ForEach(x => x.SetPrizeCards(PrizeCardCount));
                            GameState = GameFieldState.InTurn;
                            SendEventToPlayers(new GameSyncEvent { Game = this });
                        }
                        else
                        {
                            GameState = GameFieldState.BothSelectingBench;
                            SendEventToPlayers(new GameSyncEvent { Game = this, Info = "Select Pokémons to add to your starting bench" });
                            return;
                        }
                    }
                    else
                    {
                        PushInfoToPlayer("Opponent is selecting active...", owner);
                    }

                    SendEventMessage(new GameSyncEvent { Game = this }, Players.First(x => x.Id.Equals(ownerId)));
                }
            }

            PushGameLogUpdatesToPlayers();
        }

        public bool CanRetreat(PokemonCard card)
        {
            List<RetreatCostModifierAbility> costModifierAbilities = GetAllPassiveAbilities()
                .OfType<RetreatCostModifierAbility>()
                .Where(ability => ability.IsActive(this)
                    && ability.ModifierType == PassiveModifierType.RetreatCost
                    && !ability.GetUnAffectedCards().Contains(ActivePlayer.ActivePokemonCard.Id)).ToList();

            var retreatCost = ActivePlayer.ActivePokemonCard.RetreatCost + costModifierAbilities.Sum(ability => ability.Amount);

            return card.AttachedEnergy.Sum(energy => energy.GetEnergry().Amount) >= retreatCost;
        }

        public int GetRetreatCostModified()
        {
            List<RetreatCostModifierAbility> costModifierAbilities = GetAllPassiveAbilities()
                .OfType<RetreatCostModifierAbility>()
                .Where(ability => ability.IsActive(this)
                    && ability.ModifierType == PassiveModifierType.RetreatCost
                    && !ability.GetUnAffectedCards().Contains(ActivePlayer.ActivePokemonCard.Id)).ToList();

            return costModifierAbilities.Sum(ability => ability.Amount);
        }

        public void PokemonRetreated(PokemonCard replacementCard, List<EnergyCard> payedEnergy)
        {
            if (!ActivePlayer.Id.Equals(replacementCard.Owner.Id))
            {
                return;
            }

            if (!CanRetreat(ActivePlayer.ActivePokemonCard))
            {
                GameLog.AddMessage("Tried to retreat but did not have enough energy");
                return;
            }

            foreach (var pokemon in NonActivePlayer.GetAllPokemonCards())
            {
                TriggerAbilityOfType(TriggerType.OpponentRetreats, pokemon, 0, ActivePlayer.ActivePokemonCard);
            }
            
            ActivePlayer.RetreatActivePokemon(replacementCard, new List<EnergyCard>(payedEnergy), this);
            CheckDeadPokemon();
        }

        public void AddPokemonToBench(Player owner, List<PokemonCard> selectedPokemons)
        {
            if (GameState != GameFieldState.BothSelectingBench)
            {
                if (!ActivePlayer.Id.Equals(owner.Id))
                {
                    GameLog.AddMessage($"{owner?.NetworkPlayer?.Name} tried to play a pokemon when not allowed");
                    return;
                }
            }
            
            foreach (PokemonCard pokemon in selectedPokemons)
            {
                if (owner.BenchedPokemon.Count < BenchMaxSize && pokemon.Stage == 0)
                {
                    int index = owner.BenchedPokemon.GetNextFreeIndex();
                    owner.SetBenchedPokemon(pokemon);
                    pokemon.RevealToAll();
                    SendEventToPlayers(new PokemonAddedToBenchEvent()
                    {
                        Pokemon = pokemon,
                        Player = owner.Id,
                        Index = index
                    });
                }
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
                        SendEventToPlayers(new GameSyncEvent { Game = this });
                    }
                    else
                    {
                        SendEventMessage(new GameSyncEvent { Game = this, Info = "Opponent is still selecting Pokémons" }, Players.First(x => x.Id.Equals(owner.Id)));
                    }
                }
            }
        }

        public void StartGame()
        {
            GameLog.AddMessage("Game starting");
            ActivePlayer = Players[new Random().Next(2)];
            NonActivePlayer = Players.First(p => !p.Id.Equals(ActivePlayer.Id));

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

            ActivePlayer.OnCardsDrawn += PlayerDrewCards;
            ActivePlayer.OnCardsDiscarded += PlayerDiscardedCards;
            NonActivePlayer.OnCardsDrawn += PlayerDrewCards;
            NonActivePlayer.OnCardsDiscarded += PlayerDiscardedCards;

            PushGameLogUpdatesToPlayers();
        }

        private void PlayerDiscardedCards(object sender, PlayerCardDraw e)
        {
            SendEventToPlayers(new CardsDiscardedEvent()
            {
                Cards = new List<Card>(e.Cards),
                Player = ((Player)sender).Id,
            });
        }

        private void PlayerDrewCards(object sender, PlayerCardDraw e)
        {
            var gameEvent = new DrawCardsEvent()
            {
                Player = ActivePlayer.Id,
                Cards = new List<Card>(e.Cards)
            };

            SendEventToPlayers(gameEvent);
        }

        public void ActivateAbility(Ability ability)
        {
            if (GetAllPassiveAbilities().Any(x => x.ModifierType == PassiveModifierType.StopAbilities))
            {
                GameLog.AddMessage($"{ability.Name} stopped by ability");
                return;
            }

            GameLog.AddMessage($"{ActivePlayer.NetworkPlayer?.Name} activates ability {ability.Name}");

            SendEventToPlayers(new AbilityActivatedEvent() { PokemonId = ability.PokemonOwner.Id });
            
            ability.Trigger(ActivePlayer, NonActivePlayer, 0, this);

            CheckDeadPokemon();

            PushGameLogUpdatesToPlayers();
        }

        public PokemonCard CurrentDefender { get; set; }

        public void Attack(Attack attack)
        {
            if (attack.Disabled || !attack.CanBeUsed(this, ActivePlayer, NonActivePlayer) || !ActivePlayer.ActivePokemonCard.CanAttack() || !ActivePlayer.ActivePokemonCard.Attacks.Contains(attack))
            {
                GameLog.AddMessage($"Attack not used because FirstTurn: {FirstTurn} or Disabled: {attack.Disabled} or CanBeUsed:{attack.CanBeUsed(this, ActivePlayer, NonActivePlayer)}");
                PushGameLogUpdatesToPlayers();
                return;
            }

            GameState = GameFieldState.Attacking;

            GameLog.AddMessage($"{ActivePlayer.NetworkPlayer?.Name} activates attack {attack.Name}");

            attack.PayExtraCosts(this, ActivePlayer, NonActivePlayer);
            CurrentDefender = NonActivePlayer.ActivePokemonCard;

            if (ActivePlayer.ActivePokemonCard.IsConfused && FlipCoins(1) == 0)
            {
                HitItselfInConfusion();

                if (!IgnorePostAttack)
                {
                    PostAttack();
                }
                return;
            }

            SendEventToPlayers(new PokemonAttackedEvent() { Player = ActivePlayer.Id });

            TriggerAbilityOfType(TriggerType.Attacks, ActivePlayer.ActivePokemonCard);
            TriggerAbilityOfType(TriggerType.Attacked, NonActivePlayer.ActivePokemonCard);

            var abilities = new List<Ability>();
            abilities.AddRange(NonActivePlayer.ActivePokemonCard.GetAllActiveAbilities(this, NonActivePlayer, ActivePlayer));
            abilities.AddRange(ActivePlayer.ActivePokemonCard.GetAllActiveAbilities(this, NonActivePlayer, ActivePlayer));

            foreach (var ability in abilities.OfType<IAttackStoppingAbility>())
            {
                if (ability.IsStopped(this, ActivePlayer.ActivePokemonCard, NonActivePlayer.ActivePokemonCard))
                {
                    if (!IgnorePostAttack)
                    {
                        PostAttack();
                    }
                    return;
                }
            }

            DealDamageWithAttack(attack);

            attack.ProcessEffects(this, ActivePlayer, NonActivePlayer);

            CurrentDefender = null;
            GameState = GameFieldState.PostAttack;

            if (!IgnorePostAttack)
            {
                PostAttack();
            }
        }

        private void DealDamageWithAttack(Attack attack)
        {
            Damage damage = attack.GetDamage(ActivePlayer, NonActivePlayer, this);
            damage.NormalDamage = DamageCalculator.GetDamageAfterWeaknessAndResistance(damage.NormalDamage, ActivePlayer.ActivePokemonCard, NonActivePlayer.ActivePokemonCard, attack);

            if (DamageStoppers.Any(x => x.IsDamageIgnored(damage.NormalDamage + damage.DamageWithoutResistAndWeakness)))
            {
                GameLog.AddMessage("Damage ignored because of effect");
                if (!IgnorePostAttack)
                {
                    PostAttack();
                }
                return;
            }

            var dealtDamage = NonActivePlayer.ActivePokemonCard.DealDamage(damage, this, ActivePlayer.ActivePokemonCard, !attack.IgnoreEffects);
            attack.OnDamageDealt(dealtDamage, ActivePlayer, this);

            if (!damage.IsZero()) 
            {
                TriggerAbilityOfType(TriggerType.TakesDamage, NonActivePlayer.ActivePokemonCard, damage.NormalDamage + damage.DamageWithoutResistAndWeakness);
                TriggerAbilityOfType(TriggerType.DealsDamage, ActivePlayer.ActivePokemonCard, damage.NormalDamage + damage.DamageWithoutResistAndWeakness);
            }
        }

        private void HitItselfInConfusion()
        {
            GameLog.AddMessage($"{ActivePlayer.ActivePokemonCard.GetName()} hurt itself in its confusion");
            ActivePlayer.ActivePokemonCard.DealDamage(new Damage(0, ConfusedDamage), this, ActivePlayer.ActivePokemonCard, false);

            if (ActivePlayer.ActivePokemonCard.Ability?.TriggerType == TriggerType.DealsDamage)
            {
                ActivePlayer.ActivePokemonCard.Ability.SetTarget(ActivePlayer.ActivePokemonCard);
                ActivePlayer.ActivePokemonCard.Ability.Trigger(ActivePlayer, NonActivePlayer, ConfusedDamage, this);
                ActivePlayer.ActivePokemonCard.Ability.SetTarget(null);
            }
        }

        private void PostAttack()
        {
            if (AbilityTriggeredByDeath())
            {
                GameLog.AddMessage(NonActivePlayer.ActivePokemonCard.Ability.Name + "triggered by dying");
                NonActivePlayer.ActivePokemonCard.KnockedOutBy = ActivePlayer.ActivePokemonCard;
                NonActivePlayer.ActivePokemonCard.Ability.Trigger(NonActivePlayer, ActivePlayer, 0, this);
            }

            DamageStoppers.ForEach(damageStopper => damageStopper.TurnsLeft--);
            DamageStoppers = DamageStoppers.Where(damageStopper => damageStopper.TurnsLeft > 0).ToList();

            CheckDeadPokemon();
            EndTurn();
        }

        private bool AbilityTriggeredByDeath()
        {
            return NonActivePlayer.ActivePokemonCard.IsDead()
                            && NonActivePlayer.ActivePokemonCard.Ability?.TriggerType == TriggerType.KilledByAttack
                            && NonActivePlayer.ActivePokemonCard.Ability.CanActivate(this, NonActivePlayer, ActivePlayer);
        }

        public void PlayPokemon(PokemonCard pokemon)
        {
            if (!pokemon.Owner.Id.Equals(ActivePlayer.Id))
            {
                GameLog.AddMessage($"{NonActivePlayer?.NetworkPlayer?.Name} Tried to play a pokemon on his opponents turn");
                return;
            }

            ActivePlayer.PlayCard(pokemon);

            TriggerAbilityOfType(TriggerType.EnterPlay, pokemon);
        }

        public void PlayTrainerCard(TrainerCard trainerCard)
        {
            if (!ActivePlayer.Hand.Contains(trainerCard))
            {
                GameLog.AddMessage($"{ActivePlayer?.NetworkPlayer?.Name} Tried to play a trainer ({trainerCard.Name}) card not in his hand");
                return;
            }

            if (GetAllPassiveAbilities().Any(ability => ability.ModifierType == PassiveModifierType.StopTrainerCast))
            {
                GameLog.AddMessage($"{trainerCard.Name} stopped by ability");
                return;
            }
            else if (!trainerCard.CanCast(this, ActivePlayer, NonActivePlayer))
            {
                GameLog.AddMessage($"{trainerCard.Name} could not be cast because something is missing");
                return;
            }

            if (trainerCard.IsStadium())
            {
                PlayStadiumCard(trainerCard);
                return;
            }

            trainerCard.RevealToAll();
            CurrentTrainerCard = trainerCard;

            GameLog.AddMessage(ActivePlayer.NetworkPlayer?.Name + " Plays " + trainerCard.GetName());
            PushGameLogUpdatesToPlayers();

            ActivePlayer.Hand.Remove(trainerCard);

            var trainerEvent = new TrainerCardPlayed()
            {
                Card = trainerCard,
                Player = ActivePlayer.Id
            };

            SendEventToPlayers(trainerEvent);

            trainerCard.Process(this, ActivePlayer, NonActivePlayer);
            
            if (trainerCard.AddToDiscardWhenCasting)
            {
                ActivePlayer.DiscardPile.Add(trainerCard);
            }

            CurrentTrainerCard = null;

            if (ActivePlayer.IsDead)
            {
                GameLog.AddMessage($"{ActivePlayer.NetworkPlayer?.Name} loses because they drew to many cards");
                EndGame(NonActivePlayer.Id);
            }

            SendEventToPlayers(new GameInfoEvent { });
        }

        private void PlayStadiumCard(TrainerCard trainerCard)
        {
            trainerCard.RevealToAll();
            CurrentTrainerCard = trainerCard;

            GameLog.AddMessage(ActivePlayer.NetworkPlayer?.Name + " Plays " + trainerCard.GetName());
            PushGameLogUpdatesToPlayers();

            ActivePlayer.Hand.Remove(trainerCard);

            var trainerEvent = new StadiumCardPlayedEvent()
            {
                Card = trainerCard,
                Player = ActivePlayer.Id
            };

            SendEventToPlayers(trainerEvent);

            trainerCard.Process(this, ActivePlayer, NonActivePlayer);
            StadiumCard = trainerCard;

            if (trainerCard.AddToDiscardWhenCasting)
            {
                ActivePlayer.DiscardPile.Add(trainerCard);
            }

            CurrentTrainerCard = null;

            if (ActivePlayer.IsDead)
            {
                GameLog.AddMessage($"{ActivePlayer.NetworkPlayer?.Name} loses because they drew to many cards");
                EndGame(NonActivePlayer.Id);
            }

            SendEventToPlayers(new GameInfoEvent { });
        }

        private void SendEventMessage(Event playEvent, Player target)
        {
            var message = new EventMessage(playEvent.Copy());
            message.GameEvent.GameField = CreateGameInfo(target == ActivePlayer);

            target?.NetworkPlayer?.Send(message.ToNetworkMessage(Id));
        }

        public GameFieldInfo CreateGameInfo(bool forActive)
        {
            if (ActivePlayer == null || NonActivePlayer == null)
            {
                return new GameFieldInfo();
            }

            var activePlayer = new PlayerInfo
            {
                Id = ActivePlayer.Id,
                ActivePokemon = ActivePlayer.ActivePokemonCard,
                BenchedPokemon = ActivePlayer.BenchedPokemon,
                CardsInDeck = ActivePlayer.Deck.Cards.Count,
                CardsInDiscard = ActivePlayer.DiscardPile,
                CardsInHand = ActivePlayer.Hand.Count,
                PrizeCards = ActivePlayer.PrizeCards
            };

            var nonActivePlayer = new PlayerInfo
            {
                Id = NonActivePlayer.Id,
                ActivePokemon = NonActivePlayer.ActivePokemonCard,
                BenchedPokemon = NonActivePlayer.BenchedPokemon,
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

        private bool IsDraw()
        {
            return ActivePlayer.ActivePokemonCard.IsDead() && ActivePlayer.BenchedPokemon.ValidPokemonCards.All(pokemon => pokemon.IsDead())
                && NonActivePlayer.ActivePokemonCard.IsDead() && NonActivePlayer.BenchedPokemon.ValidPokemonCards.All(pokemon => pokemon.IsDead());
        }

        private void CheckDeadPokemon()
        {
            if (IsDraw())
            {
                GameLog.AddMessage("It's a draw!");
                EndGame(Id);
                return;
            }

            CheckDeadBenchedPokemon(NonActivePlayer);
            CheckDeadBenchedPokemon(ActivePlayer);

            if (NonActivePlayer.ActivePokemonCard != null && NonActivePlayer.ActivePokemonCard.IsDead())
            {
                GameLog.AddMessage(NonActivePlayer.ActivePokemonCard.GetName() + " Dies");

                NonActivePlayer.ActivePokemonCard.KnockedOutBy = ActivePlayer.ActivePokemonCard;

                SendEventToPlayers(new PokemonDiedEvent
                {
                    Pokemon = NonActivePlayer.ActivePokemonCard
                });

                TriggerAbilityOfType(TriggerType.Dies, NonActivePlayer.ActivePokemonCard);
                TriggerAbilityOfType(TriggerType.Kills, ActivePlayer.ActivePokemonCard);

                NonActivePlayer.ActivePokemonCard.KnockedOutBy = ActivePlayer.ActivePokemonCard;

                PushGameLogUpdatesToPlayers();

                if (ActivePlayer.PrizeCards.Count == 1 && NonActivePlayer.ActivePokemonCard.PrizeCards > 0)
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
                    PushInfoToPlayer("Opponent is selecting a prize card", NonActivePlayer);
                    ActivePlayer.SelectPrizeCard(NonActivePlayer.ActivePokemonCard.PrizeCards, this);
                }

                NonActivePlayer.KillActivePokemon();
                if (NonActivePlayer.BenchedPokemon.Count > 0)
                {
                    PushInfoToPlayer("Opponent is selecting a new active Pokémon", ActivePlayer);
                    NonActivePlayer.SelectActiveFromBench(this);
                }
                else
                {
                    GameLog.AddMessage(NonActivePlayer.NetworkPlayer?.Name + $" has no pokémon left, {ActivePlayer.NetworkPlayer?.Name} wins the game");
                    EndGame(ActivePlayer.Id);
                    return;
                }
            }

            PushGameLogUpdatesToPlayers();

            if (ActivePlayer.ActivePokemonCard != null && ActivePlayer.ActivePokemonCard.IsDead())
            {
                GameLog.AddMessage(ActivePlayer.ActivePokemonCard.GetName() + "Dies");

                SendEventToPlayers(new PokemonDiedEvent
                {
                    Pokemon = ActivePlayer.ActivePokemonCard
                });

                TriggerAbilityOfType(TriggerType.Dies, ActivePlayer.ActivePokemonCard);

                var prizeCardValue = ActivePlayer.ActivePokemonCard.PrizeCards;
                ActivePlayer.ActivePokemonCard.KnockedOutBy = NonActivePlayer.ActivePokemonCard;
                ActivePlayer.KillActivePokemon();

                if (ActivePlayer.BenchedPokemon.Count > 0)
                {
                    PushInfoToPlayer("Opponent is selecting a prize card", ActivePlayer);
                    NonActivePlayer.SelectPrizeCard(prizeCardValue, this);
                    PushInfoToPlayer("Opponent is selecting a new active Pokémon", NonActivePlayer);
                    ActivePlayer.SelectActiveFromBench(this);
                }
                else
                {
                    GameLog.AddMessage(ActivePlayer.NetworkPlayer?.Name + $" has no pokémon left, {NonActivePlayer.NetworkPlayer?.Name} wins the game");
                    EndGame(NonActivePlayer.Id);
                    return;
                }
            }

            PushGameLogUpdatesToPlayers();
        }

        private void CheckDeadBenchedPokemon(Player player)
        {
            var opponent = GetOpponentOf(player);
            var killedPokemons = new List<PokemonCard>();
            foreach (PokemonCard pokemon in player.BenchedPokemon.ValidPokemonCards)
            {
                if (pokemon == null || !pokemon.IsDead())
                {
                    continue;
                }

                SendEventToPlayers(new PokemonDiedEvent
                {
                    Pokemon = pokemon
                });

                killedPokemons.Add(pokemon);

                TriggerAbilityOfType(TriggerType.Dies, pokemon);

                if (opponent.PrizeCards.Count <= 1 && pokemon.PrizeCards > 0)
                {
                    GameLog.AddMessage(opponent.NetworkPlayer?.Name + " wins the game");
                    EndGame(opponent.Id);
                    return;
                }
                else
                {
                    PushInfoToPlayer("Opponent is selecting a prize card", player);
                    opponent.SelectPrizeCard(pokemon.PrizeCards, this);
                }
            }

            foreach (var pokemon in killedPokemons)
            {
                player.KillBenchedPokemon(pokemon);
            }
        }

        public void TriggerAbilityOfType(TriggerType triggerType, PokemonCard pokemon, int damage = 0, PokemonCard target = null)
        {
            if (GetAllPassiveAbilities().Any(x => x.ModifierType == PassiveModifierType.StopAbilities))
            {
                return;
            }

            if (StadiumCard.Ability != null && StadiumCard.Ability.TriggerType == triggerType)
            {
                StadiumCard.Ability.Trigger(ActivePlayer, NonActivePlayer, damage, this);
            }

            var abilities = new List<Ability>();
            abilities.AddRange(pokemon.TemporaryAbilities);

            if (pokemon.Ability != null)
            {
                abilities.Add(pokemon.Ability);
            }

            foreach (var ability in abilities)
            {
                if (ability is IAttackStoppingAbility)
                {
                    continue;
                }

                var other = Players.First(player => !player.Id.Equals(ability.PokemonOwner.Owner));
                if (ability.TriggerType == triggerType && ability.CanActivate(this, ability.PokemonOwner.Owner, other))
                {
                    GameLog.AddMessage($"Ability {ability.Name} from {ability.PokemonOwner.Name} triggers...");
                    SendEventToPlayers(new AbilityActivatedEvent
                    {
                        PokemonId = pokemon.Id
                    });
                    ability.SetTarget(target);
                    ability.Trigger(pokemon.Owner, Players.First(x => !x.Id.Equals(pokemon.Owner.Id)), damage, this);
                    ability.SetTarget(null);
                }
            }
        }

        private void EndGame(NetworkId winner)
        {
            GameState = GameFieldState.GameOver;
            foreach (Player player in Players)
            {
                player.NetworkPlayer?.Send(new GameOverMessage(winner).ToNetworkMessage(Id));
            }

            ActivePlayer.OnCardsDrawn -= PlayerDrewCards;
            ActivePlayer.OnCardsDiscarded -= PlayerDiscardedCards;
            NonActivePlayer.OnCardsDrawn -= PlayerDrewCards;
            NonActivePlayer.OnCardsDiscarded -= PlayerDiscardedCards;
        }

        public void EndTurn()
        {
            if (GameState == GameFieldState.GameOver || GameState == GameFieldState.TurnEnding)
            {
                return;
            }

            GameState = GameFieldState.TurnEnding;
            TemporaryPassiveAbilities.ForEach(x => x.TurnsLeft--);
            TemporaryPassiveAbilities = TemporaryPassiveAbilities.Where(x => x.TurnsLeft > 0 || !x.LimitedByTime).ToList();

            ActivePlayer.EndTurn(this);
            ActivePlayer.TurnsTaken++;

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

            NonActivePlayer.EndTurn(this);
            CheckDeadPokemon();
            SwapActivePlayer();
            FirstTurn = false;
            StartNextTurn();

            PushGameLogUpdatesToPlayers();
            SendEventToPlayers(new GameSyncEvent { Game = this });
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

        public bool IsAbilitiesBlocked()
        {
            return GetAllPassiveAbilities().Count(x => x.ModifierType == PassiveModifierType.StopAbilities) > 0;
        }

        public List<PassiveAbility> GetAllPassiveAbilities()
        {
            var passiveAbilities = new List<PassiveAbility>();

            if (ActivePlayer != null)
            {
                passiveAbilities.AddRange(ActivePlayer.GetAllPokemonCards().Where(p => p.Ability != null).Select(pokemon => pokemon.Ability).OfType<PassiveAbility>());
            }
            if (NonActivePlayer != null)
            {
                passiveAbilities.AddRange(NonActivePlayer.GetAllPokemonCards().Where(p => p.Ability != null).Select(pokemon => pokemon.Ability).OfType<PassiveAbility>());
            }

            if (StadiumCard != null && StadiumCard.Ability is PassiveAbility)
            {
                passiveAbilities.Add((PassiveAbility)StadiumCard.Ability);
            }

            passiveAbilities.AddRange(TemporaryPassiveAbilities);

            if (passiveAbilities.Any(ability => ability.ModifierType == PassiveModifierType.NoPokemonPowers))
                return new List<PassiveAbility>() { passiveAbilities.First(ability => ability.ModifierType == PassiveModifierType.NoPokemonPowers) };

            var idOfStopper = passiveAbilities.FirstOrDefault(x => x.ModifierType == PassiveModifierType.StopAbilities);

            if (idOfStopper != null)
            {
                return passiveAbilities.Where(x => x.Id.Equals(idOfStopper) || x.IsBuff).ToList();
            }
            
            return passiveAbilities.Where(a => a.CanActivate(this, a.PokemonOwner.Owner, GetOpponentOf(a.PokemonOwner.Owner))).ToList();
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

        private void PushInfoToPlayer(string info, Player player)
        {
            var message = new InfoMessage(info);
            player.NetworkPlayer.Send(message.ToNetworkMessage(Id));
        }

        public bool AskYesNo(Player caster, string info)
        {
            var message = new YesNoMessage() { Message = info }.ToNetworkMessage(caster.Id);

            var response = caster.NetworkPlayer.SendAndWaitForResponse<YesNoMessage>(message).AnsweredYes;

            LastYesNo = response;

            return response;
        }

        public GameFieldState GameState { get; set; }
        public NetworkId Id { get; set; }
        public List<Player> Players { get; set; } = new List<Player>();
        public Player ActivePlayer { get; set; }
        public Player NonActivePlayer { get; set; }
        public GameLog GameLog { get; set; } = new GameLog();
        public List<DamageStopper> DamageStoppers { get; set; }
        public List<PassiveAbility> TemporaryPassiveAbilities { get; set; }
        public bool PrizeCardsFaceUp { get; set; }
        public bool FirstTurn { get; set; } = true;
        public bool IgnorePostAttack { get; set; }
        public TrainerCard StadiumCard { get; set; }

        [JsonIgnore]
        public TrainerCard CurrentTrainerCard { get; set; }
        public bool LastCoinFlipResult { get; set; }
        public int LastCoinFlipHeadCount { get; set; }
        
        [JsonIgnore]
        public Dictionary<NetworkId, Card> Cards { get; set; } = new Dictionary<NetworkId, Card>();
        
        [JsonIgnore]
        public Dictionary<NetworkId, Attack> Attacks { get; set; } = new Dictionary<NetworkId, Attack>();

        [JsonIgnore]
        public Dictionary<NetworkId, Ability> Abilities { get; set; } = new Dictionary<NetworkId, Ability>();
        
        public NetworkId Format { get; set; }
        public int LastDiscard { get; set; }
        public bool LastYesNo { get; set; }
    }
}
