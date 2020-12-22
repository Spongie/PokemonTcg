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
        private bool isChoice; 
        private bool onlyOnCoinflip;
        private bool requireUsable = true;

        [DynamicInput("Require Target Usable", InputControl.Boolean)]
        public bool RequireTargetUsabled
        {
            get { return requireUsable; }
            set
            {
                requireUsable = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Is Choice?", InputControl.Boolean)]
        public bool IsChoice
        {
            get { return isChoice; }
            set
            {
                isChoice = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Opponent chooces?", InputControl.Boolean)]
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

        [DynamicInput("Only on coinflip", InputControl.Boolean)]
        public bool OnlyOnCoinFlip
        {
            get { return onlyOnCoinflip; }
            set
            {
                onlyOnCoinflip = value;
                FirePropertyChanged();
            }
        }


        public void Process(GameField game, Player caster, Player opponent, PokemonCard pokemonSource)
        {
            if (!RequireTargetUsabled)
            {
                if (opponents && opponent.BenchedPokemon.Count == 0)
                {
                    return;
                }

                if (!opponents && caster.BenchedPokemon.Count == 0)
                {
                    return;
                }
            }

            if (OnlyOnCoinFlip && game.FlipCoins(1) == 0)
            {
                return;
            }

            Player selectedPlayer = opponentChooses ? opponent : caster;
            NetworkMessage message;

            if (selectedPlayer == caster)
            {
                if (opponents)
                {
                    message = new SelectFromOpponentBenchMessage(1).ToNetworkMessage(game.Id);
                    opponent.NetworkPlayer.Send(new InfoMessage("Opponent is selecting a new active pokemon for you").ToNetworkMessage(game.Id));
                }
                else
                {
                    message = new SelectFromYourBenchMessage(1).ToNetworkMessage(game.Id);
                    opponent.NetworkPlayer.Send(new InfoMessage("Opponent is selecting a new active pokemon").ToNetworkMessage(game.Id));
                }
            }
            else
            {
                if (opponents)
                {
                    message = new SelectFromYourBenchMessage(1).ToNetworkMessage(game.Id);
                    caster.NetworkPlayer.Send(new InfoMessage("Opponent is selecting a new active pokemon for you").ToNetworkMessage(game.Id));
                }
                else
                {
                    message = new SelectFromOpponentBenchMessage(1).ToNetworkMessage(game.Id);
                    caster.NetworkPlayer.Send(new InfoMessage("Opponent is selecting a new active pokemon").ToNetworkMessage(game.Id));
                }
            }
            
            NetworkId selectedId = selectedPlayer.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message).Cards.First();

            var targetPlayer = opponents ? opponent : caster;

            targetPlayer.ForceRetreatActivePokemon((PokemonCard)game.Cards[selectedId], game);
        }

        public bool CanCast(GameField game, Player caster, Player opponent)
        {
            if (!RequireTargetUsabled)
            {
                return true;
            }

            if (opponents)
            {
                return opponent.BenchedPokemon.Count > 0;
            }

            return caster.BenchedPokemon.Count > 0;
        }

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand, GameField game)
        {
            
        }
    }
}
