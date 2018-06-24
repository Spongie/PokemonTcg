using System.Linq;

namespace CardCreator
{
    public class WeaknessResistenceInfo
    {
        public WeaknessResistenceInfo(PokemonTcgSdk.Models.PokemonCard card)
        {
            if (card.Weaknesses != null && card.Weaknesses.Any())
            {
                Weakness = card.Weaknesses.First().Type;
            }
            if (card.Resistances != null && card.Resistances.Any())
            {
                Resistence = card.Resistances.First().Type;
            }
            RetreatCost = card.RetreatCost == null ? 0 : card.RetreatCost.Count;
        }

        public string Weakness { get; set; }
        public string Resistence { get; set; }
        public int RetreatCost { get; set; }
    }
}
