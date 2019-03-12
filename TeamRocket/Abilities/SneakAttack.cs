using System.Linq;
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
            Name = "Sneak Attack";
            Description = "When you player Dark Golbat from your hand choose 1 of your opponents Pokemon an deal 10 damage to it";
        }

        protected override void Activate(Player owner, Player opponent, int damageTaken)
        {
            var response = owner.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(new SelectOpponentPokemon(1).ToNetworkMessage(owner.Id));
            response.Cards.OfType<PokemonCard>().ToList().ForEach(x => x.DealDamage(new Damage(10)));
        }
    }
}
