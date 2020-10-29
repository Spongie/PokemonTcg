using CardEditor.Views;
using Entities.Models;
using NetworkingCore;
using System.Linq;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace TCGCards.TrainerEffects
{
    public class SwapActivePokemonEffect : DataModel, ITrainerEffect
    {
        public string EffectType
        {
            get
            {
                return "Swap Active Pokemon";
            }
        }

        private bool opponents;

        [DynamicInput("Targets opponent?", InputControl.Boolean)]
        public bool Opponents
        {
            get { return opponents; }
            set
            {
                opponents = value;
                FirePropertyChanged();
            }
        }


        public void Process(GameField game, Player caster, Player opponent)
        {
            NetworkMessage message;

            if (opponents)
            {
                message = new SelectFromOpponentBenchMessage(1).ToNetworkMessage(game.Id);
            }
            else
            {
                message = new SelectFromYourBenchMessage(1).ToNetworkMessage(game.Id);
            }

            NetworkId selectedId = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message).Cards.First();

            var targetPlayer = opponents ? opponent : caster;

            var currentActive = targetPlayer.ActivePokemonCard;
            targetPlayer.ActivePokemonCard = null;

            targetPlayer.SetActivePokemon(targetPlayer.BenchedPokemon.First(x => x.Id.Equals(selectedId)));

            targetPlayer.BenchedPokemon.Add(currentActive);
        }

        public bool CanCast(GameField game, Player caster, Player opponent)
        {
            if (opponents)
            {
                return opponent.BenchedPokemon.Any();
            }

            return caster.BenchedPokemon.Any();
        }
    }
}
