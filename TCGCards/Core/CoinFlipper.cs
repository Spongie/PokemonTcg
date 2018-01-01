using System;

namespace TCGCards.Core
{
    public static class CoinFlipper
    {
        private static Random random = new Random();
        public static readonly bool HEADS = true;
        public static readonly bool TAILS = false;

        public static bool FlipCoin()
        {
            return random.Next(2) > 0;
        }

        public static int FlipCoins(int count)
        {
            int heads = 0;

            for(int _ = 0; _ < count; _++)
            {
                heads += random.Next(2);
            }

            return heads;
        }
    }
}
