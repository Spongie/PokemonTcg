using Assets.Scripts.Game;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Cards
{
    public abstract class Attack
    {
        private string _Name;
        private string _Description;
        private List<Energy> _Cost;

        public string Name
        {
            get
            {
                return _Name;
            }

            set
            {
                _Name = value;
            }
        }

        public string Description
        {
            get
            {
                return _Description;
            }

            set
            {
                _Description = value;
            }
        }

        public List<Energy> Cost
        {
            get
            {
                return _Cost;
            }

            set
            {
                _Cost = value;
            }
        }

        public abstract int GetDamage(Player owner, Player opponent);

        protected int GetDamageAfterResistanceAndWeakness(int baseDamage, IPokemonCard attacker, IPokemonCard target)
        {
            int realDamage = baseDamage;

            if (target.Resistance == attacker.PokemonType)
            {
                realDamage -= 30;
            }
            if (target.Weakness == attacker.PokemonType)
            {
                realDamage *= 2;
            }

            return Math.Max(realDamage, 0);
        }
    }
}