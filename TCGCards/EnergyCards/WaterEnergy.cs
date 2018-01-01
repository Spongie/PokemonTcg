﻿namespace TCGCards.EnergyCards
{
    public class WaterEnergy : IEnergyCard
    {
        public WaterEnergy()
        {
            EnergyType = EnergyTypes.Water;
        }

        public override Energy GetEnergry()
        {
            return new Energy(EnergyTypes.Water, 1);
        }

        public override string GetName()
        {
            return "Water Energy";
        }
    }
}
