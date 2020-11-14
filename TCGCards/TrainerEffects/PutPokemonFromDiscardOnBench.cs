using CardEditor.Views;
using Entities.Models;
using System;
using System.Linq;
using TCGCards.Core;
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

        [DynamicInput("Targets opponent", InputControl.Boolean)]
        public bool TargetsOpponent
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
                return "PutPokemon From Discard On Bench";
            }
        }

        public bool CanCast(GameField game, Player caster, Player opponent)
        {
            if (targetsOpponent)
            {
                return GameField.BenchMaxSize - opponent.BenchedPokemon.Count > 0;
            }

            return GameField.BenchMaxSize - caster.BenchedPokemon.Count > 0;
        }

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand)
        {
            
        }

        public void Process(GameField game, Player caster, Player opponent)
        {
            var target = TargetsOpponent ? opponent : caster;

            var pokemons = target.DiscardPile.OfType<PokemonCard>().Where(pokemon => pokemon.Stage == 0).ToList();

            if (pokemons.Count == 0)
            {
                return;
            }

            var message = new PickFromListMessage(pokemons, 1).ToNetworkMessage(game.Id);
            var response = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message).Cards.First();

            var card = game.FindCardById(response);

            var pokemonCard = (PokemonCard)card;

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
        }
    }
}
