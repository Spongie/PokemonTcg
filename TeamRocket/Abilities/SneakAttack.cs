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
            Description = "When you play Dark Golbat from your hand choose 1 of your opponents Pokemon an deal 10 damage to it";
        }

        protected override void Activate(Player owner, Player opponent, int damageTaken, GameLog log)
        {
            var response = owner.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(new SelectOpponentPokemonMessage(1).ToNetworkMessage(owner.Id));

            foreach (var id in response.Cards)
            {
                if (opponent.ActivePokemonCard.Id.Equals(id))
                {
                    opponent.ActivePokemonCard.DealDamage(new Damage(10), log);
                }
                else
                {
                    foreach (var pokemon in opponent.BenchedPokemon)
                    {
                        if (pokemon.Id.Equals(id))
                        {
                            pokemon.DealDamage(new Damage(10), log);
                            break;
                        }
                    }
                }
            }
        }
    }
}
