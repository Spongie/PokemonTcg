using TCGCards;
using TCGCards.Core;

namespace BaseSet.TrainerCards
{
    public class FullHeal : TrainerCard
    {
        public FullHeal()
        {
            Name = "Full Heal";
            Description = "Your active pokemon is no longer asleep, confused, paralyzed or Poisoned";
            Set = Singleton.Get<Set>();
        }
        public override void Process(GameField game, Player caster, Player opponent)
        {
            caster.ActivePokemonCard.IsAsleep = false;
            caster.ActivePokemonCard.IsConfused = false;
            caster.ActivePokemonCard.IsParalyzed = false;
            caster.ActivePokemonCard.IsPoisoned = false;
        }
    }
}
