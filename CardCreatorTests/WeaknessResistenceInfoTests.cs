using Microsoft.VisualStudio.TestTools.UnitTesting;
using CardCreator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace CardCreator.Tests
{
    [TestClass()]
    public class WeaknessResistenceInfoTests
    {
        [TestMethod()]
        public void ParseTest_Resistence()
        {
            var weaknessResistence = CreateWeaknessResistenceFromHtml();
            Assert.AreEqual("Fighting", weaknessResistence.Resistence);
        }

        [TestMethod()]
        public void ParseTest_Weakness()
        {
            var weaknessResistence = CreateWeaknessResistenceFromHtml();
            Assert.AreEqual("Psychic", weaknessResistence.Weakness);
        }

        [TestMethod()]
        public void ParseTest_Retreat()
        {
            var weaknessResistence = CreateWeaknessResistenceFromHtml();
            Assert.AreEqual(0, weaknessResistence.RetreatCost);
        }

        private static WeaknessResistenceInfo CreateWeaknessResistenceFromHtml()
        {
            var document = new HtmlDocument();
            var htmlNode = new HtmlNode(HtmlNodeType.Element, document, 0) { Name = "ppp" };

            var weakness = new HtmlNode(HtmlNodeType.Element, document, 0)
            {
                InnerHtml = "Weakness: Psychic ×2",
                Name = "text"
            };
            htmlNode.AppendChild(weakness);
            htmlNode.AppendChild(new HtmlNode(HtmlNodeType.Element, document, 1)
            {
                InnerHtml = "<br>",
                Name = "br1"
            });
            var resistence = new HtmlNode(HtmlNodeType.Element, document, 2)
            {
                InnerHtml = "Resistance: Fighting -30",
                Name = "text2"
            };
            htmlNode.AppendChild(resistence);
            htmlNode.AppendChild(new HtmlNode(HtmlNodeType.Element, document, 3)
            {
                InnerHtml = "<br>",
                Name = "br2"
            });

            var retreat = new HtmlNode(HtmlNodeType.Element, document, 4)
            {
                InnerHtml = "Retreat Cost: 0",
                Name = "text3"
            };
            htmlNode.AppendChild(retreat);

            return WeaknessResistenceInfo.Parse(htmlNode);
        }
    }
}