using TCGCards;

public abstract class EnergyCard : Card
{
    public EnergyCard() : base(null)
    {
    }

    public EnergyTypes EnergyType { get; protected set; }

    public abstract Energy GetEnergry();

    public bool IsBasic { get; set; } = true;
}
