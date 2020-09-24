﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Entities
{
    public static class CoinFlipper
    {
        private static Random random = new Random();
        public static readonly bool HEADS = true;
        public static readonly bool TAILS = false;

        public static Queue<bool> ForcedNextFlips = new Queue<bool>();

        public static bool FlipCoin()
        {
            if (ForcedNextFlips.Any())
            {
                return ForcedNextFlips.Dequeue();
            }

            return random.Next(2) > 0;
        }

        public static int FlipCoins(int count)
        {
            int heads = 0;

            for(int _ = 0; _ < count; _++)
            {
                if (ForcedNextFlips.Any())
                {
                    heads += ForcedNextFlips.Dequeue() ? 1 : 0;
                    continue;
                }

                heads += random.Next(2);
            }

            return heads;
        }
    }
}