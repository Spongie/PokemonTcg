using Microsoft.VisualStudio.TestTools.UnitTesting;
using HtmlAgilityPack;

namespace CardCreator.Tests
{
    [TestClass()]
    public class AttackTests
    {
        [TestMethod()]
        public void ParseTest_Cost()
        {
            Attack attack = CreateAttackFromHtml();

            Assert.AreEqual("PPC", attack.Cost);
        }

        [TestMethod()]
        public void ParseTest_Name()
        {
            Attack attack = CreateAttackFromHtml();

            Assert.AreEqual("Teleport Blast", attack.Name);
        }

        [TestMethod()]
        public void ParseTest_Damage()
        {
            Attack attack = CreateAttackFromHtml();

            Assert.AreEqual("30", attack.Damage);
        }

        [TestMethod()]
        public void ParseTest_Description()
        {
            Attack attack = CreateAttackFromHtml();

            Assert.AreEqual("You may switch Dark Alakazam with 1 of your Benched Pokémon. (Do the damage before switching the Pokémon.)", attack.Description);
        }

        [TestMethod()]
        public void ParseTest_Cost2()
        {
            Attack attack = CreateAttackFromHtml2();

            Assert.AreEqual("P", attack.Cost);
        }

        [TestMethod()]
        public void ParseTest_Name2()
        {
            Attack attack = CreateAttackFromHtml2();

            Assert.AreEqual("Dizziness", attack.Name);
        }

        [TestMethod()]
        public void ParseTest_Damage2()
        {
            Attack attack = CreateAttackFromHtml2();

            Assert.AreEqual("0", attack.Damage);
        }

        [TestMethod()]
        public void ParseTest_Description2()
        {
            Attack attack = CreateAttackFromHtml2();

            Assert.AreEqual("Draw a card.", attack.Description);
        }

        [TestMethod()]
        public void ParseTest_Description_Plus()
        {
            Attack attack = CreateAttackFromHtml_Plus();

            Assert.AreEqual("Flip a coin. If heads, this attack does 20 damage plus 20 more damage. If tails, this attack does 20 damage.", attack.Description);
        }

        [TestMethod()]
        public void ParseTest_Damage_Plus()
        {
            Attack attack = CreateAttackFromHtml_Plus();

            Assert.AreEqual("20", attack.Damage);
        }

        [TestMethod()]
        public void ParseTest_NeedsMode_Plus()
        {
            Attack attack = CreateAttackFromHtml_Plus();

            Assert.IsTrue(attack.NeedsMore);
        }

        private static Attack CreateAttackFromHtml()
        {
            var document = new HtmlDocument();
            string input = "[P][P][C] Teleport Blast: 30 damage. You may switch Dark Alakazam with 1 of your Benched Pokémon. (Do the damage before switching the Pokémon.)";
            var htmlNode = new HtmlNode(HtmlNodeType.Element, document, 0) { Name = "ppp" };
            
            var textNode = new HtmlNode(HtmlNodeType.Element, document, 0)
            {
                InnerHtml = input,
                Name = "text"
            };
            htmlNode.AppendChild(textNode);

            var attack = Attack.Parse(htmlNode);
            return attack;
        }

        private static Attack CreateAttackFromHtml2()
        {
            var document = new HtmlDocument();
            string input = "[P] Dizziness: Draw a card.";
            var htmlNode = new HtmlNode(HtmlNodeType.Element, document, 0) { Name = "ppp" };

            var textNode = new HtmlNode(HtmlNodeType.Element, document, 0)
            {
                InnerHtml = input,
                Name = "text"
            };
            htmlNode.AppendChild(textNode);

            var attack = Attack.Parse(htmlNode);
            return attack;
        }

        private static Attack CreateAttackFromHtml_Plus()
        {
            var document = new HtmlDocument();
            string input = "[F][C] Anger: 20+ damage. Flip a coin. If heads, this attack does 20 damage plus 20 more damage. If tails, this attack does 20 damage.";
            var htmlNode = new HtmlNode(HtmlNodeType.Element, document, 0) { Name = "ppp" };

            var textNode = new HtmlNode(HtmlNodeType.Element, document, 0)
            {
                InnerHtml = input,
                Name = "text"
            };
            htmlNode.AppendChild(textNode);

            var attack = Attack.Parse(htmlNode);
            return attack;
        }
    }
}