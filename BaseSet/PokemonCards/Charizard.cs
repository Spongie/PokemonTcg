using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;
using TCGCards.Core.Abilities;

namespace BaseSet.PokemonCards
{
    public class Charizard : PokemonCard
    {
        public Charizard(Player owner) : base(owner)
        {
            PokemonName = "Charizard";
			EvolvesFrom = PokemonNames.Charmeleon;
            Hp = 120;
            PokemonType = EnergyTypes.Fire;
            RetreatCost = 3;
            Weakness = EnergyTypes.Water;
			Resistance = EnergyTypes.Fighting;
			Stage = 2;
            Attacks = new List<Attack>
            {
				new FireSpin()
            };
            Ability = new EnergyBurn(this);
        }

        private class EnergyBurn : Ability
        {
            public EnergyBurn(PokemonCard pokemonOwner) : base(pokemonOwner)
            {
                Name = "Energy Burn";
                Description = "As often as you like during your turn (before your attack), you may turn all Energy attached to Charizard into Fire Energy for the rest of the turn. This power can't be used if Charizard is Asleep, Confused, or Paralyzed.";
                Usages = int.MaxValue;
                TriggerType = TriggerType.Activation;
            }

            protected override void Activate(Player owner, Player opponent, int damageTaken, GameLog log)
            {
                PokemonOwner.TemporaryAbilities.Add(new EnergyTypeOverrideTemporaryAbility(PokemonOwner, new[] { EnergyTypes.All }, EnergyTypes.Fire));
            }
        }
    }
}
