using Microsoft.VisualStudio.TestTools.UnitTesting;
using TCGCards;
using TCGCards.Core;

namespace TCGCardsTests
{
    [TestClass]
    public class CardTests
    {
        [TestMethod]
        public void GetLogicalNameTest()
        {
            var card = new TestCard(null);

            Assert.AreEqual("Cards\\TCGCardsTests\\TCGCardsTests\\TestCard", card.GetLogicalName());
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
