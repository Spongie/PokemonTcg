using Assets.Scripts.Cards;

public abstract class IEnergyCard : ICard
{
    protected EnergyTypes _EnergyType;

    public EnergyTypes EnergyType
    {
        get
        {
            return _EnergyType;
        }

        set
        {
            _EnergyType = value;
        }
    }

    public abstract Energy GetEnergry();
}
