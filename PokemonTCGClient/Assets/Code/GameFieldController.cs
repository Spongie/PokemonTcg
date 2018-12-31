using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TCGCards.EnergyCards;
using TestCards;
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

namespace TestCards
{
    public class TestEkans : PokemonCard
    {
        public TestEkans(Player owner) : base(owner)
        {
            PokemonName = "Ekans";
            Stage = 0;
            Hp = 50;
            PokemonType = EnergyTypes.Grass;
            RetreatCost = 1;
            Weakness = EnergyTypes.Psychic;
            Resistance = EnergyTypes.None;
            Attacks = new List<Attack>
            {
                new Bite(),
                new PoisonSting()
            };

        }
    }

    internal class PoisonSting : Attack
    {
        public PoisonSting()
        {
            Name = "Poison Sting";
            Description = "Flip a coin. If heads, the Defending Pokémon is now Poisoned.";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Grass, 2)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 20;
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            if (CoinFlipper.FlipCoin())
            {
                opponent.ActivePokemonCard.IsPoisoned = true;
            }
        }
    }

    internal class Bite : Attack
    {
        public Bite()
        {
            Name = "Bite";
            Description = string.Empty;
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Colorless, 1),
                new Energy(EnergyTypes.Grass, 1)
            };
        }


        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 20;
        }
    }
}
