using TCGCards.Core;

namespace TCGCards
{
    public abstract class TrainerCard : Card
    {
        public TrainerCard() : base(null)
        {
                
        }

        public TrainerCard(Player owner) : base(owner)
        {
        }

        public abstract void Process(GameField game, Player caster, Player opponent);

        public virtual bool CanCast(GameField game, Player caster, Player opponent) => true;

        public override string GetName() => Name;

        public string Name { get; set; }
        public string Description { get; set; }
    }
}
