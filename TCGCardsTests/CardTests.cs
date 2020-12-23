using System.IO;
using Xunit;
using TCGCards;
using TCGCards.Core;

namespace TCGCardsTests
{
    public class CardTests
    {
        [Fact]
        public void GetLogicalNameTest()
        {
            var card = new TestCard(null);

            Assert.Equal(Path.Combine("Cards", "TCGCardsTests", "TCGCardsTests", "TestCard"), card.GetLogicalName());
        }
    }

    class TestCard : Card
    {
        public TestCard(Player owner) : base(owner)
        {
        }

        public override string GetName()
        {
            return "ASD";
        }
    }
}
