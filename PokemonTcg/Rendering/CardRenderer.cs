using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PokemonTcg.Assets;
using TCGCards;

namespace PokemonTcg.Rendering
{
    public class CardRenderer
    {
        public const int benchCardWidth = 125;
        public const int benchCardHeight = 150;
        public const int benchCardSpacing = 10;
        public const int cardWidth = 150;
        public const int cardWidthSpacing = 10;
        public const int cardHeight = 250;
        public const int activeCardWidth = 150;
        public const int activeCardWidthSpacing = 10;
        public const int activeCardHeight = 250;

        public CardRenderer(ICard card, CardMode cardMode, Point position)
        {
            Card = card;
            BasePosition = position;
            RealPosition = BasePosition;
            Texture = TextureManager.Instance.LoadCardTexture(card);
            Mode = cardMode;
            AllowIsHovered = true;
        }

        public Point BasePosition { get; set; }

        public Point RealPosition { get; set; }

        public CardMode Mode { get; set; }

        public ICard Card { get; set; }

        public Texture2D Texture { get; set; }

        public bool IsHovered { get; protected set; }

        public bool AllowIsHovered { get; set; }

        public bool HoverExit { get; set; }

        public bool HoverEnter { get; set; }

        public int Width
        {
            get
            {
                switch(Mode)
                {
                    case CardMode.Hand:
                        return cardWidth;
                    case CardMode.Bench:
                        return benchCardWidth;
                    case CardMode.Active:
                        return activeCardWidth;
                    case CardMode.Prize:
                        return 0;
                    default:
                        return 0;
                }
            }
        }

        public int Height
        {
            get
            {
                switch(Mode)
                {
                    case CardMode.Hand:
                        return cardHeight;
                    case CardMode.Bench:
                        return benchCardHeight;
                    case CardMode.Active:
                        return activeCardHeight;
                    case CardMode.Prize:
                        return 0;
                    default:
                        return 0;
                }
            }
        }

        public int Spacing
        {
            get
            {
                switch(Mode)
                {
                    case CardMode.Hand:
                        return cardWidthSpacing;
                    case CardMode.Bench:
                        return benchCardSpacing;
                    case CardMode.Prize:
                        return 0;
                    default:
                        return 0;
                }
            }
        }

        public void Update()
        {
            var size = IsHovered ? new Point(Width * 2, Height * 2) : new Point(Width, Height);
            if (new Rectangle(RealPosition, size).Contains(InputManager.MousePosition))
            {
                if(!IsHovered && AllowIsHovered)
                {
                    HoverEnter = true;
                    IsHovered = true;
                    RealPosition = new Point(BasePosition.X - (Width / 2), BasePosition.Y - Height);
                }
            }
            else
            {
                if (IsHovered)
                    HoverExit = true;

                IsHovered = false;
                RealPosition = BasePosition;
            }
        }

        public void Render(SpriteBatch spriteBatch)
        {
            var size = IsHovered ? new Point(Width * 2, Height * 2) : new Point(Width, Height);
            spriteBatch.Draw(Texture, new Rectangle(RealPosition, size), Color.White);
        }
    }
}
