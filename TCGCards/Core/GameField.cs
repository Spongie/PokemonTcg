using System;
using System.Collections.Generic;
using System.Linq;
using Entities;
using NetworkingCore;
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
        
        public GameField()
        {
            Id = NetworkId.Generate();
            Players = new List<Player>();
            playersSetStartBench = new HashSet<NetworkId>();
            AttackStoppers = new List<AttackStopper>();
            DamageStoppers = new List<DamageStopper>();
            TemporaryPassiveAbilities = new List<PassiveAbility>();
            GameState = GameFieldState.WaitingForConnection;
            GameLog = new GameLog();
        }

        public int FlipCoins(int coins)
        {
            var heads = CoinFlipper.FlipCoins(coins);

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

            return heads;
        }

        public void SendEventToPlayers(Event gameEvent)
        {
            SendEventMessage(gameEvent, ActivePlayer);
            SendEventMessage(gameEvent, NonActivePlayer);
        }

        public void RevealCardsTo(List<NetworkId> pickedCards, Player nonActivePlayer)
        {
            foreach (var card in pickedCards.Select(id => FindCardById(id)))
            {
                card.IsRevealed = true;
            }
            //TODO: Complete this
        }

        public void EvolvePokemon(PokemonCard basePokemon, PokemonCard evolution)
        {
            if (GetAllPassiveAbilities().Any(ability => ability.ModifierType == PassiveModifierType.StopEvolutions))
            {
                GameLog.AddMessage("Evolution stopped by ability");
                return;
            }

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
                    evolution.EvolvedThisTurn = true;
                }
            }

            if (ActivePlayer.Hand.Contains(evolution))
            {
                ActivePlayer.Hand.Remove(evolution);
            }

            evolution.IsRevealed = true;

            SendEventToPlayers(new PokemonEvolvedEvent
            {
                TargetPokemonId = basePokemon.Id,
                NewPokemonCard = evolution
            });

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
                        GameState = GameFieldState.BothSelectingBench;
                        SendEventToPlayers(new GameSyncEvent { Game = this, Info = "Select Pokémons to add to your starting bench" });
                        return;
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
            IEnumerable<RetreatCostModifierAbility> costModifierAbilities = GetAllPassiveAbilities()
                .OfType<RetreatCostModifierAbility>()
                .Where(ability => ability.IsActive(this)
                    && ability.ModifierType == PassiveModifierType.RetreatCost
                    && !ability.GetUnAffectedCards().Contains(ActivePlayer.ActivePokemonCard.Id));

            var retreatCost = ActivePlayer.ActivePokemonCard.RetreatCost + costModifierAbilities.Sum(ability => ability.Amount);

            return card.AttachedEnergy.Sum(energy => energy.GetEnergry().Amount) >= retreatCost;
        }

        public void OnPokemonRetreated(PokemonCard replacementCard, IEnumerable<EnergyCard> payedEnergy)
        {
            if (!CanRetreat(ActivePlayer.ActivePokemonCard))
            {
                GameLog.AddMessage("Tried to retreat but did not have enough energy");
                return;
            }

            foreach (var pokemon in NonActivePlayer.GetAllPokemonCards())
            {
                TriggerAbilityOfType(TriggerType.OpponentRetreats, pokemon);
            }
            
            ActivePlayer.RetreatActivePokemon(replacementCard, payedEnergy, this);
        }

        public void OnBenchPokemonSelected(Player owner, IEnumerable<PokemonCard> selectedPokemons)
        {
            foreach (PokemonCard pokemon in selectedPokemons)
            {
                if (owner.BenchedPokemon.Count < BenchMaxSize && pokemon.Stage == 0)
                {
                    owner.SetBenchedPokemon(pokemon);
                    pokemon.IsRevealed = true;
                    SendEventToPlayers(new PokemonAddedToBenchEvent()
                    {
                        Pokemon = pokemon,
                        Player = owner.Id
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

                    if (pokemon.AttachedEnergy == null)
                    {
                        continue;
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
                Cards = e.Cards,
                Player = ((Player)sender).Id,
            });
        }

        private void PlayerDrewCards(object sender, PlayerCardDraw e)
        {
            var gameEvent = new DrawCardsEvent()
            {
                Amount = e.Amount,
                Player = ActivePlayer.Id,
                Cards = e.Cards
            };

            SendEventToPlayers(gameEvent);
        }

        public void ActivateAbility(Ability ability)
        {
            GameLog.AddMessage($"{ActivePlayer.NetworkPlayer?.Name} activates ability {ability.Name}");

            SendEventToPlayers(new AbilityActivatedEvent() { PokemonId = ability.PokemonOwner.Id });
            
            ability.Trigger(ActivePlayer, NonActivePlayer, 0, this);

            CheckDeadPokemon();

            PushGameLogUpdatesToPlayers();
        }

        public void Attack(Attack attack)
        {
            if (attack.Disabled || !attack.CanBeUsed(this, ActivePlayer, NonActivePlayer) || !ActivePlayer.ActivePokemonCard.CanAttack())
            {
                GameLog.AddMessage($"Attack not used becasue GameFirst: {FirstTurn} Disabled: {attack.Disabled} or CanBeUsed:{attack.CanBeUsed(this, ActivePlayer, NonActivePlayer)}");
                PushGameLogUpdatesToPlayers();
                return;
            }

            GameLog.AddMessage($"{ActivePlayer.NetworkPlayer?.Name} activates attack {attack.Name}");

            attack.PayExtraCosts(this, ActivePlayer, NonActivePlayer);

            if (ActivePlayer.ActivePokemonCard.IsConfused && FlipCoins(1) == 0)
            {
                HitItselfInConfusion();
                return;
            }

            SendEventToPlayers(new PokemonAttackedEvent() { Player = ActivePlayer.Id });

            TriggerAbilityOfType(TriggerType.Attacks, ActivePlayer.ActivePokemonCard);
            TriggerAbilityOfType(TriggerType.Attacked, NonActivePlayer.ActivePokemonCard);

            var abilities = new List<Ability>();
            abilities.AddRange(NonActivePlayer.ActivePokemonCard.TemporaryAbilities);

            if (NonActivePlayer.ActivePokemonCard.Ability != null)
            {
                abilities.Add(NonActivePlayer.ActivePokemonCard.Ability);
            }

            foreach (var ability in abilities.OfType<AttackStopperAbility>())
            {
                if (ability.IsStopped(this))
                {
                    return;
                }
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
            damage.NormalDamage = GetDamageAfterWeaknessAndResistance(damage.NormalDamage, ActivePlayer.ActivePokemonCard, NonActivePlayer.ActivePokemonCard, attack);

            if (DamageStoppers.Any(x => x.IsDamageIgnored(damage.NormalDamage + damage.DamageWithoutResistAndWeakness)))
            {
                GameLog.AddMessage("Damage ignored because of effect");
                if (!IgnorePostAttack)
                {
                    PostAttack();
                }
                return;
            }

            var dealtDamage = NonActivePlayer.ActivePokemonCard.DealDamage(damage, this, ActivePlayer.ActivePokemonCard, true);
            attack.OnDamageDealt(dealtDamage, ActivePlayer);

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

            if (!IgnorePostAttack)
            {
                PostAttack();
            }
        }

        private int GetDamageAfterWeaknessAndResistance(int damage, PokemonCard attacker, PokemonCard defender, Attack attack)
        {
            var realDamage = damage;

            if (!attack.ApplyWeaknessResistance)
            {
                return realDamage;
            }

            if (defender.Resistance == attacker.Type)
            {
                realDamage -= 30;
            }
            if (defender.Weakness == attacker.Type)
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
                NonActivePlayer.ActivePokemonCard.Ability.Trigger(NonActivePlayer, ActivePlayer, 0, this);
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

            TriggerAbilityOfType(TriggerType.EnterPlay, pokemon);
        }

        public void PlayTrainerCard(TrainerCard trainerCard)
        {
            if (GetAllPassiveAbilities().Any(ability => ability.ModifierType == PassiveModifierType.StopTrainerCast))
            {
                return;
            }

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
            
            ActivePlayer.DiscardPile.Add(trainerCard);

            CurrentTrainerCard = null;

            SendEventToPlayers(new GameInfoEvent());

            if (ActivePlayer.IsDead)
            {
                GameLog.AddMessage($"{ActivePlayer.NetworkPlayer?.Name} loses because they drew to many cards");
                EndGame(NonActivePlayer.Id);
            }
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
                BenchedPokemon = ActivePlayer.BenchedPokemon.OfType<Card>().ToList(),
                CardsInDeck = ActivePlayer.Deck.Cards.Count,
                CardsInDiscard = ActivePlayer.DiscardPile,
                CardsInHand = ActivePlayer.Hand.Count,
                PrizeCards = ActivePlayer.PrizeCards
            };

            var nonActivePlayer = new PlayerInfo
            {
                Id = NonActivePlayer.Id,
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
                    ActivePlayer.SelectPriceCard(NonActivePlayer.ActivePokemonCard.PrizeCards, this);
                }

                NonActivePlayer.KillActivePokemon();
                if (NonActivePlayer.BenchedPokemon.Any())
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

                ActivePlayer.ActivePokemonCard.KnockedOutBy = NonActivePlayer.ActivePokemonCard;
                ActivePlayer.KillActivePokemon();
                
                if (ActivePlayer.BenchedPokemon.Any())
                {
                    PushInfoToPlayer("Opponent is selecting a prize card", ActivePlayer);
                    NonActivePlayer.SelectPriceCard(ActivePlayer.ActivePokemonCard.PrizeCards, this);
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

            foreach (PokemonCard pokemon in NonActivePlayer.BenchedPokemon)
            {
                if (!pokemon.IsDead())
                {
                    continue;
                }

                SendEventToPlayers(new PokemonDiedEvent
                {
                    Pokemon = pokemon
                });

                TriggerAbilityOfType(TriggerType.Dies, pokemon);
                
                if (ActivePlayer.PrizeCards.Count <= 1 && pokemon.PrizeCards > 0)
                {
                    GameLog.AddMessage(ActivePlayer.NetworkPlayer?.Name + " wins the game");
                    EndGame(ActivePlayer.Id);
                    return;
                }
                else
                {
                    PushInfoToPlayer("Opponent is selecting a prize card", NonActivePlayer);
                    ActivePlayer.SelectPriceCard(pokemon.PrizeCards, this);
                }
            }

            foreach (PokemonCard pokemon in ActivePlayer.BenchedPokemon)
            {
                if (!pokemon.IsDead())
                {
                    continue;
                }

                SendEventToPlayers(new PokemonDiedEvent
                {
                    Pokemon = pokemon
                });

                TriggerAbilityOfType(TriggerType.Dies, pokemon);

                if (NonActivePlayer.PrizeCards.Count <= 1 && pokemon.PrizeCards > 0)
                {
                    GameLog.AddMessage(NonActivePlayer.NetworkPlayer?.Name + " wins the game");
                    EndGame(NonActivePlayer.Id);
                    return;
                }
                else
                {
                    PushInfoToPlayer("Opponent is selecting a prize card", ActivePlayer);
                    NonActivePlayer.SelectPriceCard(pokemon.PrizeCards, this);
                }
            }

            PushGameLogUpdatesToPlayers();
        }

        public void TriggerAbilityOfType(TriggerType triggerType, PokemonCard pokemon, int damage = 0)
        {
            var abilities = new List<Ability>();
            abilities.AddRange(pokemon.TemporaryAbilities);

            if (pokemon.Ability != null)
            {
                abilities.Add(pokemon.Ability);
            }

            foreach (var ability in abilities)
            {
                if (ability is AttackStopperAbility)
                {
                    continue;
                }

                if (ability.TriggerType == triggerType && ability.CanActivate())
                {
                    GameLog.AddMessage($"Ability {ability.Name} from {ability.PokemonOwner.Name} triggers..."); ;
                    ability.Trigger(pokemon.Owner, Players.First(x => !x.Id.Equals(pokemon.Owner.Id)), damage, this);
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
        }

        public void EndTurn()
        {
            if (GameState == GameFieldState.GameOver)
            {
                return;
            }

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

        private void PushInfoToPlayer(string info, Player player)
        {
            var message = new InfoMessage(info);
            player.NetworkPlayer.Send(message.ToNetworkMessage(Id));
        }

        public GameFieldState GameState { get; set; }
        public NetworkId Id { get; set; }
        public List<Player> Players { get; set; }
        public Player ActivePlayer { get; set; }
        public Player NonActivePlayer { get; set; }
        public GameLog GameLog { get; set; } = new GameLog();
        public List<AttackStopper> AttackStoppers { get; set; }
        public List<DamageStopper> DamageStoppers { get; set; }
        public List<PassiveAbility> TemporaryPassiveAbilities { get; set; }
        public bool PrizeCardsFaceUp { get; set; }
        public bool FirstTurn { get; set; } = true;
        public bool IgnorePostAttack { get; set; }
        public TrainerCard CurrentTrainerCard { get; set; }
    }
}
