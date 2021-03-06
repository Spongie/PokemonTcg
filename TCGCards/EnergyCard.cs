﻿using Entities;
using System.Collections.ObjectModel;
using TCGCards;
using TCGCards.Core;
using TCGCards.TrainerEffects;

public class EnergyCard : Card
{
    private bool isBasic = true;
    private EnergyTypes energyType;
    private int amount;
    private ObservableCollection<IEffect> effects = new ObservableCollection<IEffect>();
    private bool attachedIconOverridden;
    private EnergyOverriders energyOverrideType;

    public EnergyCard() : base(null)
    {
    }

    public virtual Energy GetEnergry() { return new Energy(EnergyType, amount); }

    public virtual void OnAttached(PokemonCard attachedTo, bool fromHand, GameField game) 
    {
        foreach (var effect in Effects)
        {
            effect.OnAttachedTo(attachedTo, fromHand, game);
        }
    }

    public virtual void OnPutInDiscard(Player owner) { }

    public override string GetName() => Name;

    public ObservableCollection<IEffect> Effects
    {
        get { return effects; }
        set
        {
            effects = value;
            FirePropertyChanged();
        }
    }

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

    public EnergyOverriders EnergyOverrideType
    {
        get { return energyOverrideType; }
        set
        {
            energyOverrideType = value;
            FirePropertyChanged();
        }
    }

    public bool AttachedIconOverridden
    {
        get { return attachedIconOverridden; }
        set
        {
            attachedIconOverridden = value;
            FirePropertyChanged();
        }
    }

}
