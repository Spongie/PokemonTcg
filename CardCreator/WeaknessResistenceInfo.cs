using HtmlAgilityPack;

namespace CardCreator
{
    public class WeaknessResistenceInfo
    {
        public string Weakness { get; set; }
        public string Resistence { get; set; }
        public int RetreatCost { get; set; }

        public static WeaknessResistenceInfo Parse(HtmlNode node)
        {
            string weakness = node.ChildNodes[0].InnerHtml.Substring("Weakness: ".Length).Replace("×2", string.Empty).Trim();
            string resistence = node.ChildNodes[2].InnerHtml.Contains("none") ? string.Empty : node.ChildNodes[2].InnerHtml.Substring("Resistence: ".Length);
            resistence = resistence == string.Empty ? string.Empty : resistence.Substring(0, resistence.IndexOf('-') - 1).Trim();
            int retreatCost = int.Parse(node.ChildNodes[4].InnerHtml.Replace("Retreat Cost: ", string.Empty));

            return new WeaknessResistenceInfo
            {
                Weakness = weakness,
                Resistence = resistence,
                RetreatCost = retreatCost
            };
        }
    }
}
