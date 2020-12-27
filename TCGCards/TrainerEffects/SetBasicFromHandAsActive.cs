using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCGCards.Core;
using TCGCards.Core.Deckfilters;
using TCGCards.Core.GameEvents;
using TCGCards.Core.Messages;

namespace TCGCards.TrainerEffects
{
    public class SetBasicFromHandAsActive : IEffect
    {
        public string EffectType
        {
            get
            {
                return "Set basic Pokémon from hand as active";
            }
        }

        public bool CanCast(GameField game, Player caster, Player opponent)
        {
            return caster.BenchedPokemon.Count < GameField.BenchMaxSize && caster.Hand.OfType<PokemonCard>().Any(x => x.Stage == 0);
        }

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand, GameField game)
        {
            
        }

        public void Process(GameField game, Player caster, Player opponent, PokemonCard pokemonSource)
        {
            caster.ForceRetreatActivePokemon(null, game);

            var message = new DiscardCardsMessage(1, new List<IDeckFilter>() { new BasicPokemonFilter() })
            {
                Info = "Select a new Active Pokémon from your hand"
            };
            var response = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message.ToNetworkMessage(game.Id));

            var pokemon = (PokemonCard)game.Cards[response.Cards[0]];
            caster.SetActivePokemon(pokemon);

            game.SendEventToPlayers(new PokemonBecameActiveEvent()
            {
                NewActivePokemon = pokemon
            });
        }
    }
}
