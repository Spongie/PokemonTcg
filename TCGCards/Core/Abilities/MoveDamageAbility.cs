using CardEditor.Views;
using System.Collections.Generic;
using System.Linq;
using TCGCards.Core.Messages;

namespace TCGCards.Core.Abilities
{
    public class MoveDamageAbility : Ability
    {
        private int amount;

        public MoveDamageAbility() :this(null)
        {

        }

        public MoveDamageAbility(PokemonCard pokemonOwner) : base(pokemonOwner)
        {
            TriggerType = TriggerType.Activation;
            Usages = int.MaxValue;
        }

        [DynamicInput("Damage to move")]
        public int Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                FirePropertyChanged();
            }
        }


        protected override void Activate(Player owner, Player opponent, int damageTaken, GameField game)
        {
            var availablePokemons = new List<PokemonCard>(owner.BenchedPokemon);
            availablePokemons.Add(owner.ActivePokemonCard);

            var sourcePokemons = availablePokemons.Where(x => x.DamageCounters > 0).OfType<Card>().ToList();

            var pickFirstMessage = new PickFromListMessage(sourcePokemons, 1).ToNetworkMessage(owner.Id);
            var selectedSource = owner.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(pickFirstMessage).Cards.First();

            var availableTargets = availablePokemons.Where(x => (x.Hp - x.DamageCounters) > Amount).OfType<Card>().ToList();

            var pickTargetMessage = new PickFromListMessage(availableTargets, 1).ToNetworkMessage(owner.Id);
            var target = owner.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(pickTargetMessage).Cards.First();

            foreach (var pokemon in availablePokemons)
            {
                if (pokemon.Id.Equals(selectedSource))
                {
                    pokemon.DamageCounters -= Amount;
                }
                else if (pokemon.Id.Equals(target))
                {
                    pokemon.DealDamage(Amount, game, PokemonOwner, false);
                }
            }
        }
    }
}
