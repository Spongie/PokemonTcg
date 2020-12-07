using Entities.Models;
using TCGCards.Core;
using TCGCards.Core.GameEvents;

namespace TCGCards.TrainerEffects
{
    public class CreateFossilePokemon : DataModel, IEffect
    {
        public string EffectType
        {
            get
            {
                return "Create fossile Pokémon";
            }
        }

        public bool CanCast(GameField game, Player caster, Player opponent)
        {
            return GameField.BenchMaxSize - caster.BenchedPokemon.Count > 0;
        }

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand, GameField game)
        {
            
        }

        public void Process(GameField game, Player caster, Player opponent, PokemonCard pokemonSource)
        {
            var pokemon = new FossilePokemon(game.CurrentTrainerCard);
            caster.BenchedPokemon.Add(pokemon);
            game.SendEventToPlayers(new PokemonAddedToBenchEvent() { Player = caster.Id, Pokemon = pokemon });
        }
    }
}
