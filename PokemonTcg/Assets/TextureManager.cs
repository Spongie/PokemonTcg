using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TCGCards;

namespace PokemonTcg.Assets
{
    public class TextureManager
    {
        private static TextureManager instance;

        private Dictionary<Type, Texture2D> textures;
        private ContentManager contentManager;

        public TextureManager(ContentManager content)
        {
            textures = new Dictionary<Type, Texture2D>();
            contentManager = content;
            instance = this;
            Cardback = content.Load<Texture2D>("Cards\\CardBack");
        }

        public static TextureManager Instance
        {
            get
            {
                return instance;
            }
        }

        public Texture2D Cardback { get; internal set; }

        public Texture2D LoadCardTexture(ICard card)
        {
            var cardType = card.GetType();

            if(textures.ContainsKey(cardType))
                return textures[cardType];

            var texture = contentManager.Load<Texture2D>(card.GetLogicalName());

            textures.Add(cardType, texture);

            return texture;
        }
    }
}
