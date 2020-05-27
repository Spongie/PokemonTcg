using TCGCards;

namespace TeamRocket
{
    public class Set : IPokemonSet
    {
        public string GetBaseFolder() => "TeamRocket";

        public string GetSetCode() => "base5";
    }
}
