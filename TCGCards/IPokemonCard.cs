using System.Collections.Generic;

namespace TCGCards
{
    public abstract class IPokemonCard : ICard
    {
        private int hp;
        private int damageCounters;
        private int stage;
        private IPokemonCard evolvesFrom;
        private EnergyTypes pokemonType;
        private EnergyTypes resistance;
        private EnergyTypes weakness;
        private List<Attack> attacks;

        public int DamageCounters
        {
            get
            {
                return damageCounters;
            }

            set
            {
                damageCounters = value;
            }
        }

        public int Hp
        {
            get
            {
                return hp;
            }

            set
            {
                hp = value;
            }
        }

        public int Stage
        {
            get
            {
                return stage;
            }

            set
            {
                stage = value;
            }
        }

        public IPokemonCard EvolvesFrom
        {
            get
            {
                return evolvesFrom;
            }

            set
            {
                evolvesFrom = value;
            }
        }

        public EnergyTypes PokemonType
        {
            get
            {
                return pokemonType;
            }

            set
            {
                pokemonType = value;
            }
        }

        public EnergyTypes Resistance
        {
            get
            {
                return resistance;
            }

            set
            {
                resistance = value;
            }
        }

        public EnergyTypes Weakness
        {
            get
            {
                return weakness;
            }

            set
            {
                weakness = value;
            }
        }

        public List<Attack> Attacks
        {
            get
            {
                return attacks;
            }

            set
            {
                attacks = value;
            }
        }
    }
}
