using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;
using System.Linq;
using TCGCards.Core.Messages;

namespace BaseSet.PokemonCards
{
    public class Alakazam : PokemonCard
    {
        public Alakazam(Player owner) : base(owner)
        {
            PokemonName = "Alakazam";
            Set = Singleton.Get<Set>();
            EvolvesFrom = PokemonNames.Kadabra;
            Hp = 80;
            PokemonType = EnergyTypes.Psychic;
            RetreatCost = 3;
            Weakness = EnergyTypes.Psychic;
			Resistance = EnergyTypes.None;
			Stage = 2;
            Attacks = new List<Attack>
            {
				new ConfuseRay()
            };
            Ability = new DamageSwap(this);
        }

        private class DamageSwap : Ability
        {
            public DamageSwap(PokemonCard pokemonOwner) : base(pokemonOwner)
            {
                TriggerType = TriggerType.Activation;
                Usages = int.MaxValue;
            }

            protected override void Activate(Player owner, Player opponent, int damageTaken, GameLog log)
            {
                var availablePokemons = new List<PokemonCard>(owner.BenchedPokemon);
                availablePokemons.Add(owner.ActivePokemonCard);

                var sourcePokemons = availablePokemons.Where(x => x.DamageCounters > 0).ToList();

                var pickFirstMessage = new PickFromListMessage(sourcePokemons, 1).ToNetworkMessage(owner.Id);
                var selectedSource = owner.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(pickFirstMessage).Cards.First();

                var availableTargets = availablePokemons.Where(x => (x.Hp - x.DamageCounters) > 10);

                var pickTargetMessage = new PickFromListMessage(availableTargets, 1).ToNetworkMessage(owner.Id);
                var target = owner.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(pickTargetMessage).Cards.First();

                foreach (var pokemon in availablePokemons)
                {
                    if (pokemon.Id.Equals(selectedSource))
                    {
                        pokemon.DamageCounters -= 10;
                    }
                    else if (pokemon.Id.Equals(target))
                    {
                        pokemon.DamageCounters += 10;
                    }
                }
            }
        }
    }
}
