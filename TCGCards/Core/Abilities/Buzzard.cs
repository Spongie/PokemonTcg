using NetworkingCore;
using System.Collections.Generic;
using System.Linq;
using TCGCards.Core.GameEvents;
using TCGCards.Core.Messages;
using TCGCards.EnergyCards;

namespace TCGCards.Core.Abilities
{
    public class Buzzard : Ability
    {
        public Buzzard() :this(null)
        {

        }

        public Buzzard(PokemonCard pokemonOwner) : base(pokemonOwner)
        {
            Name = "Buzzap";
            Description = "At any time during your turn (before your attack) you may Knock Out Electrode and attach it to 1 of your other Pokémon. If you do, chose a type of Energy. Electrode is now an Energy card (instead of a Pokémon) that provides 2 energy of that type. This power can't be used if Electrode is Asleep, Confused, or Paralyzed.";
            TriggerType = TriggerType.Activation;
        }

        public override bool CanActivate(GameField game, Player caster, Player opponent)
        {
            return caster.BenchedPokemon.Count > 0 && base.CanActivate(game, caster, opponent);
        }

        protected override void Activate(Player owner, Player opponent, int damageTaken, GameField game)
        {
            NetworkId selectedId = null;
            do
            {
                var response = owner.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(new SelectFromYourPokemonMessage("Select one of your other pokemon to attach Electrode to").ToNetworkMessage(NetworkId.Generate()));
                selectedId = response.Cards.FirstOrDefault();
            } while (selectedId == null || selectedId.Equals(PokemonOwner.Id));

            var colorResponse = owner.NetworkPlayer.SendAndWaitForResponse<SelectColorMessage>(new SelectColorMessage("Select energy type to become").ToNetworkMessage(NetworkId.Generate()));

            var buzzardEnergy = new BuzzardEnergy(PokemonOwner, colorResponse.Color)
            {
                ImageUrl = PokemonOwner.ImageUrl,
                SetCode = PokemonOwner.SetCode
            };

            game.SendEventToPlayers(new PokemonDiedEvent() { Pokemon = PokemonOwner });

            game.Cards.Add(buzzardEnergy.Id, buzzardEnergy);
            PokemonCard selectedPokemon = (PokemonCard)game.Cards[selectedId];
            selectedPokemon.AttachEnergy(buzzardEnergy, game);

            PokemonOwner.ClearStatusEffects();
            PokemonOwner.DamageCounters = 0;
            PokemonOwner.Owner.DiscardPile.AddRange(PokemonOwner.AttachedEnergy);
            PokemonOwner.AttachedEnergy.Clear();

            if (PokemonOwner.EvolvedFrom != null)
            {
                PokemonOwner.Owner.DiscardPile.Add(PokemonOwner.EvolvedFrom);
                PokemonOwner.EvolvedFrom = null;
            }

            if (owner.ActivePokemonCard == PokemonOwner)
            {
                owner.ActivePokemonCard = null;
                owner.SelectActiveFromBench(game);
            }
            else
            {
                owner.BenchedPokemon.Remove(PokemonOwner);
            }

            game.GameState = GameFieldState.AbilitySpecial;
            game.SendEventToPlayers(new GameInfoEvent());

            game.PushInfoToPlayer("Opponent is selecting a prize card", owner);
            opponent.SelectPrizeCard(1, game);

            game.GameState = GameFieldState.InTurn;
            game.SendEventToPlayers(new GameInfoEvent());
        }
    }
}
