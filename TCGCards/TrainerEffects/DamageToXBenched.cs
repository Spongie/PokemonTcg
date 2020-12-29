using CardEditor.Views;
using Entities.Models;
using System.Linq;
using TCGCards.Core;
using TCGCards.Core.Messages;
using TCGCards.TrainerEffects.Util;

namespace TCGCards.TrainerEffects
{
    public class DamageToXBenched : DataModel, IEffect
    {
        private int amountOfTargets;
        private int damage;
        private CoinFlipConditional coinflipConditional = new CoinFlipConditional();

        [DynamicInput("Condition", InputControl.Dynamic)]
        public CoinFlipConditional CoinFlipConditional
        {
            get { return coinflipConditional; }
            set
            {
                coinflipConditional = value;
                FirePropertyChanged();
            }
        }


        [DynamicInput("Damage")]
        public int Damage
        {
            get { return damage; }
            set
            {
                damage = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Nr. of Targets")]
        public int AmountOfTargets
        {
            get { return amountOfTargets; }
            set
            {
                amountOfTargets = value;
                FirePropertyChanged();
            }
        }

        public string EffectType
        {
            get
            {
                return "Deals damage to multiple benched";
            }
        }

        public bool CanCast(GameField game, Player caster, Player opponent)
        {
            return true;
        }

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand, GameField game)
        {
            
        }

        public void Process(GameField game, Player caster, Player opponent, PokemonCard pokemonSource)
        {
            if (!CoinFlipConditional.IsOk(game, caster))
            {
                return;
            }

            if (opponent.BenchedPokemon.Count <= AmountOfTargets)
            {
                foreach (var pokemon in opponent.BenchedPokemon.ValidPokemonCards)
                {
                    pokemon.DealDamage(Damage, game, pokemonSource, true);
                }

                return;
            }

            var message = new SelectFromOpponentBenchMessage(AmountOfTargets).ToNetworkMessage(game.Id);
            var response = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message);

            foreach (var pokemon in response.Cards.Select(id => game.Cards[id]).OfType<PokemonCard>())
            {
                pokemon.DealDamage(Damage, game, pokemonSource, true);
            }
        }
    }
}
