using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code._2D
{
    public class SpriteCache
    {
        private static SpriteCache instance;
        public Dictionary<string, Sprite> cache;

        private SpriteCache()
        {
            cache = new Dictionary<string, Sprite>();
        }

        public static SpriteCache Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SpriteCache();
                }

                return instance;
            }
        }
    }
}
