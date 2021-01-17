using CardEditor.Views;
using Entities.Models;
using System;
using System.Linq;
using TCGCards.Core;
using TCGCards.Core.GameEvents;
using TCGCards.Core.Messages;

namespace TCGCards.TrainerEffects
{
    public class PutPokemonFromDiscardOnBench : DataModel, IEffect
    {
        private bool targetsOpponent;
        private float withRemainingHealth;

        [DynamicInput("Health to come back to (0 = full, 0.X for X%)")]
        public float WithRemainingHealth
        {
            get { return withRemainingHealth; }
            set
            {
                withRemainingHealth = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Allow target opponent?", InputControl.Boolean)]
        public bool AskYesNoToTargetOpponent
        {
            get { return targetsOpponent; }
            set
            {
                targetsOpponent = value;
                FirePropertyChanged();
            }
        }

        public string EffectType
        {
            get
            {
                return "Put Pokemon From Discard On Bench";
            }
        }

        public bool CanCast(GameField game, Player caster, Player opponent)
        {
            if (targetsOpponent)
            {
                return opponent.DiscardPile.OfType<PokemonCard>().Any() && GameField.BenchMaxSize - opponent.BenchedPokemon.Count > 0;
            }

            return caster.DiscardPile.OfType<PokemonCard>().Any() && GameField.BenchMaxSize - caster.BenchedPokemon.Count > 0;
        }

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand, GameField game)
        {
            
        }

        public void Process(GameField game, Player caster, Player opponent, PokemonCard pokemonSource)
        {
            Player target;

            if (AskYesNoToTargetOpponent && game.AskYesNo(caster, "Put Pokémon from opponents discard onto their bench?"))
            {
                target = opponent;
            }
            else
            {
                target = caster;
            }

            var pokemons = target.DiscardPile.OfType<PokemonCard>().Where(pokemon => pokemon.Stage == 0).OfType<Card>().ToList();

            if (pokemons.Count == 0)
            {
                return;
            }

            var message = new PickFromListMessage(pokemons, 1).ToNetworkMessage(game.Id);
            var response = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message).Cards.First();

            var card = game.Cards[response];

            var pokemonCard = (PokemonCard)card;

            int index = target.BenchedPokemon.GetNextFreeIndex();
            target.BenchedPokemon.Add(pokemonCard);
            target.DiscardPile.Remove(card);

            if (WithRemainingHealth > 0 && WithRemainingHealth < 1)
            {
                var health = (int)Math.Ceiling(pokemonCard.Hp * WithRemainingHealth);

                if (health.ToString().Last() == '5')
                {
                    health += 5;
                }

                pokemonCard.DamageCounters = health;
            }
            else if (WithRemainingHealth > 1)
            {
                pokemonCard.DamageCounters = pokemonCard.Hp - (int)WithRemainingHealth;
            }
            else
            {
                pokemonCard.DamageCounters = 0;
            }

            game.SendEventToPlayers(new PokemonAddedToBenchEvent() { Pokemon = pokemonCard, Player = target.Id, Index = index });
        }
    }
}
