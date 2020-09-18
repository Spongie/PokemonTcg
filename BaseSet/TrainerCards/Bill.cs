using TCGCards;
using TCGCards.Core;

namespace BaseSet.TrainerCards
{
    public class Bill : TrainerCard
    {
        public Bill()
        {
            Name = "Bill";
            Set = Singleton.Get<Set>();
            Description = "Draw 2 cards.";
        }

        public override void Process(GameField game, Player caster, Player opponent)
        {
            caster.DrawCards(2);
        }
    }
}
