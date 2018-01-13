using TCGCards.Core;

namespace TCGCards.PokemonCards.TeamRocket.Attacks
{
    public class Vanish : Attack
    {
        public override int GetDamage(Player owner, Player opponent)
        {
            return 0;
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            owner.Deck.Cards.Push(owner.ActivePokemonCard);
            owner.Deck.Shuffle();
            
        }
    }
}
