using CardEditor.Views;
using Entities.Models;
using NetworkingCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace TCGCards.TrainerEffects
{
    public class HealEffect : DataModel, IEffect
    {
        private TargetingMode targetingMode;
        private int amount;

        [DynamicInput("Heal amount")]
        public int Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Targeting type", InputControl.Dropdown, typeof(TargetingMode))]
        public TargetingMode TargetingMode
        {
            get { return targetingMode; }
            set
            {
                targetingMode = value;
                FirePropertyChanged();
            }
        }

        public string EffectType
        {
            get
            {
                return "Heal";
            }
        }

        public bool CanCast(GameField game, Player caster, Player opponent) => true;

        public void Process(GameField game, Player caster, Player opponent)
        {
            PokemonCard target;
            NetworkMessage message;
            NetworkId selectedId;

            switch (TargetingMode)
            {
                case TargetingMode.YourActive:
                    target = caster.ActivePokemonCard;
                    break;
                case TargetingMode.YourBench:
                    message = new SelectFromYourBenchMessage(1).ToNetworkMessage(game.Id);
                    selectedId = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message).Cards.First();
                    target = (PokemonCard)game.FindCardById(selectedId);
                    break;
                case TargetingMode.YourPokemon:
                    message = new SelectFromYourPokemonMessage().ToNetworkMessage(game.Id);
                    selectedId = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message).Cards.First();
                    target = (PokemonCard)game.FindCardById(selectedId);
                    break;
                case TargetingMode.OpponentActive:
                    target = opponent.ActivePokemonCard;
                    break;
                case TargetingMode.OpponentBench:
                    message = new SelectFromOpponentBenchMessage(1).ToNetworkMessage(game.Id);
                    selectedId = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message).Cards.First();
                    target = (PokemonCard)game.FindCardById(selectedId);
                    break;
                case TargetingMode.OpponentPokemon:
                    message = new SelectFromOpponentBenchMessage(1).ToNetworkMessage(game.Id);
                    selectedId = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message).Cards.First();
                    target = (PokemonCard)game.FindCardById(selectedId);
                    break;
                case TargetingMode.AnyPokemon:
                    throw new NotImplementedException("TargetingMode.AnyPokemon not implemented in fullheal");
                default:
                    target = caster.ActivePokemonCard;
                    break;
            }

            target.DamageCounters -= Amount;
            
            if (target.DamageCounters < 0)
            {
                target.DamageCounters = 0;
            }
        }
    }
}
