using HtmlAgilityPack;

namespace CardDownloader
{
    public struct PokemonSet
    {
        public PokemonSet(HtmlNode sourceNode)
        {
            var baseName = sourceNode.InnerHtml.Replace("amp;", "");

            if(sourceNode.InnerHtml.IndexOf('(') > 0)
                Name = baseName.Substring(0, baseName.IndexOf('(')).Trim();
            else
                Name = baseName.Trim();

            Link = sourceNode.Attributes["href"].Value;
        }

        public string Name { get; set; }
        public string Link { get; set; }
    }
}
