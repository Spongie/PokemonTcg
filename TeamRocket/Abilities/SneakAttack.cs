using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace TeamRocket.Abilities
{
    public class SneakAttack : Ability
    {
        public SneakAttack(PokemonCard pokemonOwner) : base(pokemonOwner)
        {
            TriggerType = TriggerType.EnterPlay;
        }

        protected override void Activate(Player owner, Player opponent, int damageTaken)
        {
            var response = owner.NetworkPlayer.SendAndWaitForResponse<PokemonCardListMessage>(new SelectOpponentPokemon(1).ToNetworkMessage(owner.Id));
            response.Pokemons.ForEach(x => x.DealDamage(new Damage(10)));
        }
    }
}
