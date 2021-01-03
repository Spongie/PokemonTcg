using System.Linq;
using Entities.Models;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace TCGCards.TrainerEffects
{
    public class DiscardEnergyFromAllOpponentsPokemon : DataModel, IEffect
    {
        public string EffectType
        {
            get
            {
                return "Discard 1 Energy from all opponents Pokémon";
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
            foreach (var pokemon in opponent.GetAllPokemonCards())
            {
                if (pokemon.AttachedEnergy.Count == 1)
                {
                    pokemon.DiscardEnergyCard(pokemon.AttachedEnergy[0], game);
                }
                else
                {
                    var message = new PickFromListMessage(pokemon.AttachedEnergy.OfType<Card>().ToList(), 1) { Info = "Pick 1 Energy to discard from " + pokemon.Name };
                    var response = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message.ToNetworkMessage(game.Id));
                    pokemon.DiscardEnergyCard((EnergyCard)game.Cards[response.Cards[0]], game);
                }
            }
        }
    }
}
