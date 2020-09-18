using TCGCards;
using TCGCards.Core;

namespace TeamRocket.TrainerCards
{
    public class HereComesTeamRocket : TrainerCard
    {
        public HereComesTeamRocket()
        {
            Name = "Here Comes Team Rocket";
            Description = "Each player plays with his or her prize cards face up for the rest of the game";
            Set = Singleton.Get<Set>();
        }

        public override void Process(GameField game, Player caster, Player opponent)
        {
            game.PrizeCardsFaceUp = true;

            foreach (var card in caster.PrizeCards)
            {
                card.IsRevealed = true;
            }
            foreach (var card in opponent.PrizeCards)
            {
                card.IsRevealed = true;
            }
        }
    }
}
