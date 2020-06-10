using System;
using System.Linq;

namespace TCGCards.Core.Abilities
{
    public class EnergyTypeOverrideTemporaryAbility : TemporaryAbility
    {
        public EnergyTypeOverrideTemporaryAbility(PokemonCard owner, EnergyTypes[] sourceTypes, EnergyTypes newType) : base(owner, 1)
        {
            TriggerType = TriggerType.EnergyUsage;

            if (sourceTypes.Contains(EnergyTypes.All))
            {
                SourceTypes = Enum.GetValues(typeof(EnergyTypes)).OfType<EnergyTypes>().ToArray();
            }
            else
            {
                SourceTypes = sourceTypes;
            }

            NewType = newType;
        }

        protected override void Activate(Player owner, Player opponent, int damageTaken, GameLog log)
        {
            
        }

        public EnergyTypes[] SourceTypes { get; }
        public EnergyTypes NewType { get; }
    }
}
