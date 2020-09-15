using System.Linq;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.TrainerCards
{
    public class ProfessorOak : TrainerCard
    {
        public ProfessorOak()
        {
            Name = "Professor Oak";
            Description = "Discard your hand, then draw 7 cards";
        }

        public override void Process(GameField game, Player caster, Player opponent)
        {
            caster.DiscardCards(caster.Hand.Select(x => x.Id).Except(new[] { Id }));
            caster.DrawCards(7);
        }
    }
}
