using BaseSet.PokemonCards;
using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Messages;
using TCGCards.Core.SpecialAbilities;

namespace BaseSet.Attacks
{
    internal class Barrier : Attack
    {
        private Mewtwo mewtwo;

        public Barrier(Mewtwo mewtwo)
        {
            this.mewtwo = mewtwo;
            Name = "Barrier";
            Description = "Discard 1 Energy card attached to Mewtwo in order to use this attack. During your opponent's next turn, prevent all effects of attacks, including damage, done to Mewtwo.";
			DamageText = "0";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Psychic, 2)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 0;
        }
        
        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            var pickFromListMessage = new PickFromListMessage(mewtwo.AttachedEnergy, 1);
            var response = owner.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(pickFromListMessage.ToNetworkMessage(game.Id));

            foreach (var cardId in response.Cards)
            {
                var energyCard = (EnergyCard)game.FindCardById(cardId);
                mewtwo.AttachedEnergy.Remove(energyCard);
                owner.DiscardPile.Add(energyCard);
            }

            if (!CoinFlipper.FlipCoin())
            {
                game.GameLog.AddMessage(owner.NetworkPlayer.Name + " Flips a coin and it's tails, nothing happens");
                return;
            }

            game.GameLog.AddMessage(owner.NetworkPlayer.Name + " Flips a coin and it's heads, effect applied");

            game.AttackStoppers.Add(new AttackStopper((defender) =>
            {
                return defender.Id == mewtwo.Id;
            }));
        }
    }
}
