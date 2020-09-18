using System.Linq;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace BaseSet.TrainerCards
{
    public class GustOfWind : TrainerCard
    {
        public GustOfWind()
        {
            Name = "Gust of Wind";
            Description = "Choose 1 of your opponent's Benched Pokémon and switch it with their Active Pokémon";
            Set = Singleton.Get<Set>();
        }
        public override void Process(GameField game, Player caster, Player opponent)
        {
            var selectedId = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(new SelectFromOpponentBenchMessage(1).ToNetworkMessage(game.Id)).Cards.First();

            var currentActive = opponent.ActivePokemonCard;
            opponent.ActivePokemonCard = null;

            opponent.SetActivePokemon(opponent.BenchedPokemon.First(x => x.Id.Equals(selectedId)));

            opponent.BenchedPokemon.Add(currentActive);
        }
    }
}
