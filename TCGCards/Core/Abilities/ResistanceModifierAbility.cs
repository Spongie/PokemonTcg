using CardEditor.Views;

namespace TCGCards.Core.Abilities
{
    public class ResistanceModifierAbility : Ability, IResistanceModifier
    {
        private string attackerName;

        private float modifier;
        private string defenderName;

        [DynamicInput("Modifier (0-1 = %)")]
        public float Modifier
        {
            get { return modifier; }
            set
            {
                modifier = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Change this attacker name")]
        public string AttackerName
        {
            get { return attackerName; }
            set
            {
                attackerName = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Change this defender name")]
        public string DefenderName
        {
            get { return defenderName; }
            set
            {
                defenderName = value;
                FirePropertyChanged();
            }
        }

        public int GetModifiedResistance(PokemonCard attacker, PokemonCard defender)
        {
            if (!string.IsNullOrEmpty(AttackerName) && attacker.Name.ToLower().Contains(AttackerName.ToLower()))
            {
                if (Modifier > 1)
                {
                    return (int)Modifier;
                }

                return (int)(defender.ResistanceAmount * Modifier);
            }

            if (!string.IsNullOrEmpty(DefenderName) && attacker.Name.ToLower().Contains(DefenderName.ToLower()))
            {
                if (Modifier > 1)
                {
                    return (int)Modifier;
                }

                return (int)(defender.ResistanceAmount * Modifier);
            }

            return defender.ResistanceAmount;
        }
    }
}
