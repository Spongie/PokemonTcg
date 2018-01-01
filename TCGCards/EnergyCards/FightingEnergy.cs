﻿namespace TCGCards.EnergyCards
{
    public class FightingEnergy : IEnergyCard
    {
        public FightingEnergy()
        {
            EnergyType = EnergyTypes.Fighting;
        }

        public override Energy GetEnergry()
        {
            return new Energy(EnergyTypes.Fighting, 1);
        }

        public override string GetName()
        {
            return "Fightning Energy";
        }
    }
}
