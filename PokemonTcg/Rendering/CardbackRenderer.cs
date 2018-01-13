using Microsoft.Xna.Framework;
using PokemonTcg.Assets;
using TCGCards;

namespace PokemonTcg.Rendering
{
    public class CardbackRenderer : CardRenderer
    {
        public CardbackRenderer(ICard card, CardMode cardMode, Point position, bool opponent) :base(card, cardMode, position, opponent)
        {
            Texture = TextureManager.Instance.Cardback;
        }

        public CardbackRenderer(ICard card, CardMode cardMode, Point position) : base(card, cardMode, position)
        {
            Texture = TextureManager.Instance.Cardback;
        }
    }
}
