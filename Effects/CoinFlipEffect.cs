using Entities.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Entities.Effects
{
    public class CoinFlipEffect : Effect
    {
        public CoinFlipEffect()
        {
            Parameters = new ObservableCollection<Parameter>
            {
                new Parameter { Name = "Amount", Value = "1" }
            };
            Name = "Flip Coins";
        }

        public override void Process(Dictionary<EffectValues, object> values)
        {
            int coinsToFlip = int.Parse((string)Parameters.First().Value);

            int heads = CoinFlipper.FlipCoins(coinsToFlip);
            int tails = coinsToFlip - heads;

            values.Add(EffectValues.HeadsCount, heads);
            values.Add(EffectValues.HeadsCount, tails);
        }
    }
}
