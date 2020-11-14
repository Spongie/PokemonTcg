using CardEditor.Views;
using Entities.Models;
using System.Linq;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace TCGCards.TrainerEffects
{
    public class PutPokemonFromDiscardOnBench : DataModel, IEffect
    {
        private bool targetsOpponent;

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

            target.BenchedPokemon.Add((PokemonCard)card);
            target.DiscardPile.Remove(card);
        }
    }
}
