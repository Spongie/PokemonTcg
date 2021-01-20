using System.Linq;
using CardEditor.Views;
using Entities;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace TCGCards.Attacks
{
    public class Bonfire  : Attack
    {
        private bool discardEnergyForeachHeads;
        private EnergyTypes typeToDiscard;
        private int coins;
        private int damagePerHeads;

        [DynamicInput("Number of coins")]
        public int Coins
        {
            get { return coins; }
            set
            {
                coins = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Damage per heads")]
        public int DamagePerHeads
        {
            get { return damagePerHeads; }
            set
            {
                damagePerHeads = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Type to discard", InputControl.Dropdown, typeof(EnergyTypes))]
        public EnergyTypes TypeToDiscard
        {
            get { return typeToDiscard; }
            set
            {
                typeToDiscard = value;
                FirePropertyChanged();
            }
        }


        [DynamicInput("Discard for each head", InputControl.Boolean)]
        public bool DiscardEnergyForeachHeads
        {
            get { return discardEnergyForeachHeads; }
            set
            {
                discardEnergyForeachHeads = value;
                FirePropertyChanged();
            }
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            int heads = game.FlipCoins(Coins);

            if (heads > 0 && DiscardEnergyForeachHeads)
            {
                var choices = owner.ActivePokemonCard.AttachedEnergy.Where(e => TypeToDiscard == EnergyTypes.All || e.EnergyType == TypeToDiscard).OfType<Card>().ToList();

                if (choices.Count <= heads)
                {
                    foreach (var energy in choices)
                    {
                        owner.ActivePokemonCard.DiscardEnergyCard((EnergyCard)energy, game);
                    }
                }
                else
                {
                    var response = owner.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(new PickFromListMessage(choices, heads).ToNetworkMessage(game.Id));

                    foreach (var cardId in response.Cards)
                    {
                        owner.ActivePokemonCard.DiscardEnergyCard((EnergyCard)game.Cards[cardId], game);
                    }
                }
            }

            int damage = damagePerHeads * heads;

            foreach (var pokemon in opponent.GetAllPokemonCards())
            {
                pokemon.DealDamage(damage, game, owner.ActivePokemonCard, true);
            }

            base.ProcessEffects(game, owner, opponent);
        }
    }
}