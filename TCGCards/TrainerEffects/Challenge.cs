using NetworkingCore.Messages;
using System.Linq;
using TCGCards.Core;
using TCGCards.TrainerEffects.Util;

namespace TCGCards.TrainerEffects
{
    public class Challenge : IEffect
    {
        public string EffectType
        {
            get
            {
                return "Challenge";
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
            if (caster.BenchedPokemon.Count == GameField.BenchMaxSize && opponent.BenchedPokemon.Count == GameField.BenchMaxSize)
            {
                caster.DrawCards(2);
                return;
            }

            var message = new YesNoMessage() { Message = "Do you accept the challenge?" }.ToNetworkMessage(caster.Id);

            if (!opponent.NetworkPlayer.SendAndWaitForResponse<YesNoMessage>(message).AnsweredYes)
            {
                caster.DrawCards(2);
                return;
            }

            var casterResponse = DeckSearchUtil.SearchDeck(game, caster, CardUtil.GetCardFilters(CardType.BasicPokemon), GameField.BenchMaxSize - caster.BenchedPokemon.Count);
            var opponentResponse = DeckSearchUtil.SearchDeck(game, opponent, CardUtil.GetCardFilters(CardType.BasicPokemon), GameField.BenchMaxSize - opponent.BenchedPokemon.Count);


            game.AddPokemonToBench(caster, casterResponse.OfType<PokemonCard>().ToList());
            game.AddPokemonToBench(opponent, opponentResponse.OfType<PokemonCard>().ToList());

            caster.Deck.Shuffle();
            opponent.Deck.Shuffle();
        }
    }
}
