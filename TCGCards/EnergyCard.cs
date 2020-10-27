using Entities;
using TCGCards;
using TCGCards.Core;

public class EnergyCard : Card
{
    private bool isBasic = true;
    private EnergyTypes energyType;
    private int amount;

    public EnergyCard() : base(null)
    {
    }

    public virtual Energy GetEnergry() { return new Energy(EnergyType, amount); }

    public virtual void OnAttached(PokemonCard attachedTo, bool fromHand) { }

    public virtual void OnPutInDiscard(Player owner) { }

    public override string GetName() => Name;

    public int Amount
    {
        get { return amount; }
        set
        {
            amount = value;
            FirePropertyChanged();
        }
    }

    public EnergyTypes EnergyType
    {
        get { return energyType; }
        set
        {
            energyType = value;
            FirePropertyChanged();
        }
    }


    public bool IsBasic
    {
        get { return isBasic; }
        set
        {
            isBasic = value;
            FirePropertyChanged();
        }
    }

}
