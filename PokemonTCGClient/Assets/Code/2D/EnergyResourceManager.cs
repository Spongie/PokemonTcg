using Entities;
using System;
using System.Collections.Generic;
using System.IO;
using TCGCards;
using UnityEngine;

namespace Assets.Code._2D
{
    public class EnergyResourceManager : MonoBehaviour
    {
        public static EnergyResourceManager Instance;
        public Texture2D icon_atlas;
        public Sprite DoubleColorless;
        public Sprite Special;
        public Sprite BuzzapIcon;

        private void Awake()
        {
            Instance = this;
            Icons = new Dictionary<EnergyTypes, Sprite>();

            foreach (var sprite in Resources.LoadAll<Sprite>(icon_atlas.name))
            {
                var type = (EnergyTypes)Enum.Parse(typeof(EnergyTypes), sprite.name);
                Icons.Add(type, sprite);
            }
        }

        public Sprite GetSpriteForEnergyCard(EnergyCard energyCard)
        {
            if (energyCard.AttachedIconOverridden)
            {
                switch (energyCard.EnergyOverrideType)
                {
                    case EnergyOverriders.Buzzap:
                        return BuzzapIcon;
                }
            }

            if (energyCard.Amount == 2 && energyCard.EnergyType == EnergyTypes.Colorless)
            {
                return DoubleColorless;
            }
            if (!energyCard.IsBasic)
            {
                return Special;
            }

            return Icons[energyCard.EnergyType];
        }

        public Dictionary<EnergyTypes, Sprite> Icons { get; private set; }
    }
}
