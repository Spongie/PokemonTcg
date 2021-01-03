using CardEditor.Views;
using Entities.Models;
using TCGCards.Core;

namespace TCGCards.TrainerEffects.Util
{
    public class CoinFlipConditional : DataModel, IEffectCondition
    {
        private bool useLastCoin;
        private bool checkTails;
        private bool flipCoin;
        private int coinsToFlip = 1;
        private int successesForBonus = 1;

        public static CoinFlipConditional CreateFromObject(IEffect source)
        {
            var type = source.GetType();
            var conditional = new CoinFlipConditional();

            foreach (var prop in type.GetProperties())
            {
                if (prop.Name.ToLower() == "coinflip")
                {
                    conditional.FlipCoin = (bool)prop.GetValue(source);
                }
                else if (prop.Name.ToLower() == "flipcoin")
                {
                    conditional.FlipCoin = (bool)prop.GetValue(source);
                }
                else if (prop.Name.ToLower() == "onlyoncoinflip")
                {
                    conditional.FlipCoin = (bool)prop.GetValue(source);
                }
                else if (prop.Name.ToLower() == "checktails")
                {
                    conditional.CheckTails = (bool)prop.GetValue(source);
                }
                else if (prop.Name.ToLower() == "uselastcoin")
                {
                    conditional.UseLastCoin = (bool)prop.GetValue(source);
                }
                else if (prop.Name.ToLower() == "coins")
                {
                    conditional.CoinsToFlip = (int)prop.GetValue(source);
                }
                else if (prop.Name.ToLower() == "coinstoflip")
                {
                    conditional.CoinsToFlip = (int)prop.GetValue(source);
                }
                else if (prop.Name.ToLower() == "headsforeffect")
                {
                    conditional.SuccessessForBonus = (int)prop.GetValue(source);
                }
                else if (prop.Name.ToLower() == "headsforsuccess")
                {
                    conditional.SuccessessForBonus = (int)prop.GetValue(source);
                }
            }

            return conditional;
        }

        [DynamicInput("Use last coin flip?", InputControl.Boolean)]
        public bool UseLastCoin
        {
            get { return useLastCoin; }
            set
            {
                useLastCoin = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Trigger on tails instead?", InputControl.Boolean)]
        public bool CheckTails
        {
            get { return checkTails; }
            set
            {
                checkTails = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Flip Coin?", InputControl.Boolean)]
        public bool FlipCoin
        {
            get { return flipCoin; }
            set
            {
                flipCoin = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Coins to flip")]
        public int CoinsToFlip
        {
            get { return coinsToFlip; }
            set
            {
                coinsToFlip = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Successess required")]
        public int SuccessessForBonus
        {
            get { return successesForBonus; }
            set
            {
                successesForBonus = value;
                FirePropertyChanged();
            }
        }

        public bool IsOk(GameField game, Player caster)
        {
            var targetValue = CheckTails ? 0 : 1;
            var lastValue = game != null && game.LastCoinFlipResult ? 1 : 0;
            if (UseLastCoin)
            {
                return lastValue == targetValue;
            }
            else if (FlipCoin && CoinsToFlip > 1)
            {
                int heads = game.FlipCoins(CoinsToFlip);

                bool result;
                if (CheckTails)
                {
                    var tails = coinsToFlip - heads;
                    result = tails >= SuccessessForBonus;
                    game.LastCoinFlipResult = result;
                    return result;
                }

                result = heads >= SuccessessForBonus;
                game.LastCoinFlipResult = result;
                return result;
            }
            else if (FlipCoin && game.FlipCoins(1) != targetValue)
            {
                game.LastCoinFlipResult = false;
                return false;
            }

            game.LastCoinFlipResult = true;
            return true;
        }
    }
}
