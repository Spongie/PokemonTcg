using Entities;
using System;
using System.Collections.Generic;
using TCGCards;
using UnityEngine;

namespace Assets.Code._2D
{
    public class EnergyResourceManager : MonoBehaviour
    {
        public Texture2D icon_atlas;

        private void Awake()
        {
            Icons = new Dictionary<EnergyTypes, Sprite>();

            foreach (var sprite in Resources.LoadAll<Sprite>(icon_atlas.name))
            {
                var type = (EnergyTypes)Enum.Parse(typeof(EnergyTypes), sprite.name);
                Icons.Add(type, sprite);
            }
        }

        public Dictionary<EnergyTypes, Sprite> Icons { get; private set; }
    }
}
