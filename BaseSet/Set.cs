using TCGCards;

namespace BaseSet
{
    public class Set : IPokemonSet
    {
        public string GetSetCode() => "base1";
        public string GetBaseFolder() => "BaseSet";
    }
}
