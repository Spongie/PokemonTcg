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

        public override bool CanActivate(GameField game, Player caster, Player opponent)
        {
            var availablePokemons = new List<PokemonCard>(caster.BenchedPokemon);
            availablePokemons.Add(caster.ActivePokemonCard);

            if (availablePokemons.Where(x => x.DamageCounters > 0).Count() < 2)
            {
                return false;
            }

            return base.CanActivate(game, caster, opponent);
        }

        protected override void Activate(Player owner, Player opponent, int damageTaken, GameField game)
        {
            var availablePokemons = new List<PokemonCard>(owner.BenchedPokemon);
            availablePokemons.Add(owner.ActivePokemonCard);

            var sourcePokemons = availablePokemons.Where(x => x.DamageCounters > 0).OfType<Card>().ToList();

            var pickFirstMessage = new PickFromListMessage(sourcePokemons, 1).ToNetworkMessage(owner.Id);
            var selectedSource = (PokemonCard)game.Cards[owner.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(pickFirstMessage).Cards.First()];

            var availableTargets = availablePokemons.Where(x => (x.Hp - x.DamageCounters) > Amount).OfType<Card>().ToList();
            availableTargets.Remove(selectedSource);

            PokemonCard target;

            if (availableTargets.Count == 1)
            {
                target = (PokemonCard)availableTargets.First();
            }
            else
            {
                var pickTargetMessage = new PickFromListMessage(availableTargets, 1).ToNetworkMessage(owner.Id);
                target = (PokemonCard)game.Cards[owner.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(pickTargetMessage).Cards.First()];
            }
            

            selectedSource.Heal(Amount, game);
            target.DealDamage(Amount, game, PokemonOwner, false);
        }
    }
}
