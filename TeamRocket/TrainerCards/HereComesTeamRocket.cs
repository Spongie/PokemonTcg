using TCGCards;
using TCGCards.Core;

namespace TeamRocket.TrainerCards
{
    public class HereComesTeamRocket : TrainerCard
    {
        public override string GetName()
        {
            return "Each player plays with his or her prize cards face up for the rest of the game";
        }

        public override void Process(GameField game, Player caster, Player opponent)
        {
            game.PrizeCardsFaceUp = true;
        }
    }
}
