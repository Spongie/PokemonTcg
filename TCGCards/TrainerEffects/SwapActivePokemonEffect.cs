using CardEditor.Views;
using Entities.Models;
using NetworkingCore;
using System.Linq;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace TCGCards.TrainerEffects
{
    public class SwapActivePokemonEffect : DataModel, IEffect
    {
        public string EffectType
        {
            get
            {
                return "Swap Active Pokemon";
            }
        }

        private bool opponents;
        private bool opponentChooses;

        [DynamicInput("Opponent choices?", InputControl.Boolean)]
        public bool OpponentChooses
        {
            get { return opponentChooses; }
            set
            {
                opponentChooses = value;
                FirePropertyChanged();
            }
        }

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

            Player selectedPlayer = opponentChooses ? opponent : caster;
            NetworkId selectedId = selectedPlayer.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message).Cards.First();

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

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand)
        {
            throw new System.NotImplementedException();
        }
    }
}
