using TCGCards.Core;

namespace TCGCards.TrainerEffects
{
    public class BounceAnyNumberEffect : BouncePokemonEffect
    {
        public override void Process(GameField game, Player caster, Player opponent, PokemonCard pokemonSource)
        {
            if (CoinFlip && game.FlipCoins(Coins) < HeadsForSuccess)
            {
                return;
            }

            while (CanCast(game, caster, opponent))
            {
                if (!game.AskYesNo(caster, "Return a Pokémon to your hand?"))
                {
                    break;
                }

                PerformBounce(game, caster, opponent);
            }
        }
    }
}
