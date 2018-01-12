﻿using Microsoft.Xna.Framework;
using System.Collections.Generic;
using TCGCards.Core;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace PokemonTcg.Rendering
{
    public class PlayerRenderer
    {
        private List<CardRenderer> handCards;
        private List<CardRenderer> benchedCards;
        private CardRenderer activeRenderer;

        public PlayerRenderer(Player player)
        {
            handCards = new List<CardRenderer>();
            benchedCards = new List<CardRenderer>();

            var totalCardsize = (CardRenderer.cardWidth + CardRenderer.cardWidthSpacing) * player.Hand.Count;
            var totalEmptySpace = 1920 - totalCardsize;

            var handBasePosition = new Point(totalEmptySpace / 2, 950);

            foreach(var card in player.Hand)
            {
                handCards.Add(new CardRenderer(card, CardMode.Hand, handBasePosition));
                handBasePosition = new Point(handBasePosition.X + CardRenderer.cardWidth + CardRenderer.cardWidthSpacing, handBasePosition.Y);
            }

            var benchBasePosition = new Point(600, 780);
            foreach(var card in player.BenchedPokemon)
            {
                benchedCards.Add(new CardRenderer(card, CardMode.Bench, benchBasePosition));
                benchBasePosition = new Point(benchBasePosition.X + CardRenderer.benchCardWidth + CardRenderer.benchCardSpacing, benchBasePosition.Y);
            }

            activeRenderer = new CardRenderer(player.ActivePokemonCard, CardMode.Active, new Point((1920 / 2) - (CardRenderer.activeCardWidth / 2), 500));
        }

        public void Update()
        {
            foreach(var renderer in handCards.Concat(benchedCards).Concat(new[] { activeRenderer }))
            {
                renderer.Update();
                CheckHoverEnterExit(renderer, handCards.Concat(benchedCards));
            }
        }

        private void CheckHoverEnterExit(CardRenderer renderer, IEnumerable<CardRenderer> renderSet)
        {
            if(renderer.HoverExit)
            {
                foreach(var rend in renderSet)
                {
                    rend.AllowIsHovered = true;
                }
            }
            else if(renderer.HoverEnter)
            {
                foreach(var rend in renderSet)
                {
                    rend.AllowIsHovered = false;
                }
            }

            renderer.HoverEnter = false;
            renderer.HoverExit = false;
        }

        internal void Render(SpriteBatch spriteBatch)
        {
            foreach(var renderer in handCards.Concat(benchedCards).Concat(new[] { activeRenderer }).OrderBy(x => x.IsHovered))
            {
                renderer.Render(spriteBatch);
            }
        }
    }
}
