using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using UnityEngine;

namespace Assets.Code
{
    public class GameFieldController : MonoBehaviour
    {
        GameField gameField;
        public HandController handController;
        public HandController opponentHandController;
        public PrizeZoneController playerPrizeZone;
        public PrizeZoneController opponentPrizeZone;

        void Start()
        {
            //gameField = new GameField();
            //gameField.InitTest();
            //gameField.ActivePlayer.Hand.Add(new TestEkans(gameField.ActivePlayer));
            //gameField.ActivePlayer.Hand.Add(new TestEkans(gameField.ActivePlayer));
            //gameField.ActivePlayer.Hand.Add(new TestEkans(gameField.ActivePlayer));
            //gameField.ActivePlayer.Hand.Add(new GrassEnergy());
            //gameField.ActivePlayer.Hand.Add(new GrassEnergy());
            //gameField.ActivePlayer.Hand.Add(new GrassEnergy());
            //gameField.ActivePlayer.Hand.Add(new GrassEnergy());

            ////gameField.ActivePlayer.ActivePokemonCard = new TestEkans(gameField.ActivePlayer);
            ////gameField.ActivePlayer.ActivePokemonCard.AttachedEnergy.Add(new GrassEnergy());
            ////gameField.ActivePlayer.ActivePokemonCard.AttachedEnergy.Add(new GrassEnergy());
            ////gameField.ActivePlayer.ActivePokemonCard.AttachedEnergy.Add(new GrassEnergy());

            //gameField.ActivePlayer.Deck.Cards.Push(new TestEkans(gameField.ActivePlayer));
            //gameField.ActivePlayer.Deck.Cards.Push(new GrassEnergy());
            //gameField.ActivePlayer.Deck.Cards.Push(new GrassEnergy());
            //gameField.ActivePlayer.Deck.Cards.Push(new GrassEnergy());
            //gameField.ActivePlayer.Deck.Cards.Push(new GrassEnergy());

            //gameField.ActivePlayer.PrizeCards.Add(new GrassEnergy());
            //gameField.ActivePlayer.PrizeCards.Add(new GrassEnergy());
            //gameField.ActivePlayer.PrizeCards.Add(new GrassEnergy());
            //gameField.ActivePlayer.PrizeCards.Add(new GrassEnergy());
            //gameField.ActivePlayer.PrizeCards.Add(new GrassEnergy());
            //gameField.ActivePlayer.PrizeCards.Add(new GrassEnergy());

            //gameField.NonActivePlayer.Hand.Add(new TestEkans(gameField.ActivePlayer));
            //gameField.NonActivePlayer.Hand.Add(new TestEkans(gameField.ActivePlayer));
            //gameField.NonActivePlayer.Hand.Add(new TestEkans(gameField.ActivePlayer));
            //gameField.NonActivePlayer.Hand.Add(new GrassEnergy());
            //gameField.NonActivePlayer.Hand.Add(new GrassEnergy());
            //gameField.NonActivePlayer.Hand.Add(new GrassEnergy());
            //gameField.NonActivePlayer.Hand.Add(new GrassEnergy());

            ////gameField.NonActivePlayer.ActivePokemonCard = new TestEkans(gameField.NonActivePlayer);
            ////gameField.NonActivePlayer.ActivePokemonCard.AttachedEnergy.Add(new GrassEnergy());
            ////gameField.NonActivePlayer.ActivePokemonCard.AttachedEnergy.Add(new GrassEnergy());
            ////gameField.NonActivePlayer.ActivePokemonCard.AttachedEnergy.Add(new GrassEnergy());

            //gameField.NonActivePlayer.Deck.Cards.Push(new TestEkans(gameField.ActivePlayer));
            //gameField.NonActivePlayer.Deck.Cards.Push(new GrassEnergy());
            //gameField.NonActivePlayer.Deck.Cards.Push(new GrassEnergy());
            //gameField.NonActivePlayer.Deck.Cards.Push(new GrassEnergy());
            //gameField.NonActivePlayer.Deck.Cards.Push(new GrassEnergy());

            //gameField.NonActivePlayer.PrizeCards.Add(new GrassEnergy());
            //gameField.NonActivePlayer.PrizeCards.Add(new GrassEnergy());
            //gameField.NonActivePlayer.PrizeCards.Add(new GrassEnergy());
            //gameField.NonActivePlayer.PrizeCards.Add(new GrassEnergy());
            //gameField.NonActivePlayer.PrizeCards.Add(new GrassEnergy());
            //gameField.NonActivePlayer.PrizeCards.Add(new GrassEnergy());

            //handController.SetHand(gameField.ActivePlayer.Hand);
            //opponentHandController.SetHand(gameField.NonActivePlayer.Hand);
            //playerPrizeZone.SetPrizeCards(gameField.ActivePlayer.PrizeCards);
            //opponentPrizeZone.SetPrizeCards(gameField.NonActivePlayer.PrizeCards);
        }

        void Update()
        {

        }
    }
}