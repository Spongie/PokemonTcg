using TCGCards;

public abstract class IEnergyCard : ICard
{
    public IEnergyCard() : base(null)
    {
    }

    public EnergyTypes EnergyType { get; protected set; }

    public abstract Energy GetEnergry();
}
