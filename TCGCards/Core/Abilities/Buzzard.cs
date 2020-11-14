using NetworkingCore;
using System.Collections.Generic;
using System.Linq;
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
            Name = "Buzzard";
            Description = "At any time during your turn (before your attack) you may Knock Out Electrode and attach it to 1 of your other Pokémon. If you do, chose a type of Energy. Electrode is now an Energy card (instead of a Pokémon) that provides 2 energy of that type. This power can't be used if Electrode is Asleep, Confused, or Paralyzed.";
            TriggerType = TriggerType.Activation;
        }

        protected override void Activate(Player owner, Player opponent, int damageTaken, GameField game)
        {
            NetworkId selectedId = null;
            do
            {
                var response = owner.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(new SelectFromYourPokemonMessage("Select one of your other pokemon to attach Electrode to").ToNetworkMessage(NetworkId.Generate()));
                selectedId = response.Cards.First();
            } while (selectedId == null || selectedId.Equals(PokemonOwner.Id));

            var colorResponse = owner.NetworkPlayer.SendAndWaitForResponse<SelectColorMessage>(new SelectColorMessage("Select energy type to become").ToNetworkMessage(NetworkId.Generate()));

            var buzzardEnergy = new BuzzardEnergy(PokemonOwner, colorResponse.Color);

            PokemonCard selectedPokemon = owner.GetAllPokemonCards().First(x => x.Id.Equals(selectedId));
            selectedPokemon.AttachedEnergy.Add(buzzardEnergy);

            foreach (var energy in new List<EnergyCard>(PokemonOwner.AttachedEnergy))
            {
                PokemonOwner.DiscardEnergyCard(energy);
            }

            PokemonOwner.DamageCounters = 999;
        }
    }
}
