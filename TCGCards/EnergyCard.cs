using Entities;
using TCGCards;
using TCGCards.Core;

public abstract class EnergyCard : Card
{
    public EnergyCard() : base(null)
    {
    }

    public EnergyTypes EnergyType { get; protected set; }

    public abstract Energy GetEnergry();

    public abstract void OnAttached(PokemonCard attachedTo, bool fromHand);

    public virtual void OnPutInDiscard(Player owner) { }

    public bool IsBasic { get; set; } = true;
}
