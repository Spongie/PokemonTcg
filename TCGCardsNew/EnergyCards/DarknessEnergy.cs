﻿namespace TCGCards.EnergyCards
{
    public class DarknessEnergy : IEnergyCard
    {
        public DarknessEnergy()
        {
            EnergyType = EnergyTypes.Darkness;
        }

        public override Energy GetEnergry()
        {
            return new Energy(EnergyTypes.Darkness, 1);
        }

        public override string GetName()
        {
            return "Darkness Energy";
        }
    }
}
