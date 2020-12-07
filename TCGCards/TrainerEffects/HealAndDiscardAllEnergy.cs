using Entities.Models;
using TCGCards.Core;

namespace TCGCards.TrainerEffects
{
    public class HealAndDiscardAllEnergy : DataModel, IEffect
    {
        public string EffectType
        {
            get
            {
                return "Heal and Discard all attached energy";
            }
        }

        public bool CanCast(GameField game, Player caster, Player opponent)
        {
            return true;
        }

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand, GameField game)
        {
            
        }

        public void Process(GameField game, Player caster, Player opponent, PokemonCard pokemonSource)
        {
            foreach (var pokemon in caster.GetAllPokemonCards())
            {
                if (pokemon.DamageCounters == 0)
                {
                    continue;
                }

                pokemon.Heal(99999, game);

                while (pokemon.AttachedEnergy.Count > 0)
                {
                    var energyCard = pokemon.AttachedEnergy[0];
                    pokemon.DiscardEnergyCard(energyCard, game);
                }
            }
        }
    }
}
