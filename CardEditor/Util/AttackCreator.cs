using Entities;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using TCGCards;
using TCGCards.Attacks;
using TCGCards.Core;
using TCGCards.TrainerEffects;

namespace CardEditor.Util
{
    public static class AttackCreator
    {
        private const string firstPartApplyEffect = "Flip a coin. If heads, the Defending Pokémon is now";
        private const string firstPartForceEffect = "The Defending Pokémon is now ";
        
        public static bool TryAddEffects(ref Attack baseAttack, PokemonTcgSdk.Models.Attack sdkAttack)
        {
            if (sdkAttack.Text.ToLower().StartsWith(firstPartApplyEffect.ToLower()))
            {
                StatusEffect? effect = stringToStatusEffect(baseAttack.Description.Substring(firstPartApplyEffect.Length));
                if (effect == null)
                {
                    return false;
                }
                else
                {
                    baseAttack.Effects.Add(new ApplyStatusEffect()
                    {
                        FlipCoin = true,
                        TargetingMode = TargetingMode.OpponentActive,
                        StatusEffect = effect.Value
                    });
                    return true;
                }
            }
            else if (sdkAttack.Text.ToLower().StartsWith(firstPartForceEffect.ToLower()))
            {
                StatusEffect? effect = stringToStatusEffect(baseAttack.Description.Substring(firstPartForceEffect.Length));
                if (effect == null)
                {
                    return false;
                }
                else
                {
                    baseAttack.Effects.Add(new ApplyStatusEffect()
                    {
                        FlipCoin = false,
                        TargetingMode = TargetingMode.OpponentActive,
                        StatusEffect = effect.Value
                    });
                    return true;
                }
            }
            else if (sdkAttack.Text.ToLower().Trim() == "Flip a coin. If tails, this attack does nothing.")
            {
                var cost = baseAttack.Cost.ToList();
                var damage = baseAttack.Damage;

                baseAttack = new AttackFailsOnTails
                {
                    Name = sdkAttack.Name,
                    Description = sdkAttack.Text,
                    Cost = new ObservableCollection<Energy>(cost),
                    Damage = damage
                };
            }
            else if (Regex.IsMatch(sdkAttack.Text, @"Flip \d+ coins[.] This attack does \d+ damage times the number of heads[.]"))
            {
                var cost = baseAttack.Cost.ToList();
                int coins = int.Parse(Regex.Matches(sdkAttack.Text, @"\d+")[0].Value);
                int damage = int.Parse(Regex.Matches(sdkAttack.Text, @"\d+")[1].Value);

                baseAttack = new FlipCoinAttack
                {
                    Name = sdkAttack.Name,
                    Description = sdkAttack.Text,
                    Coins = coins,
                    Damage = damage,
                    Cost = new ObservableCollection<Energy>(cost)
                };
                return true;
            }
            else if (sdkAttack.Text == "If the Defending Pokémon tries to attack during your opponent's next turn, your opponent flips a coin. If tails, that attack does nothing.")
            {
                var cost = baseAttack.Cost.ToList();
                var damage = baseAttack.Damage;

                baseAttack = new ApplyAttackFailOnTails
                {
                    Name = sdkAttack.Name,
                    Description = sdkAttack.Text,
                    Cost = new ObservableCollection<Energy>(cost),
                    Damage = damage
                };

                return true;
            }

            return false;
        }

        private static StatusEffect? stringToStatusEffect(string value)
        {
            switch (value.ToLower().Trim().Replace(".", string.Empty))
            {
                case "poisoned":
                    return StatusEffect.Poison;
                case "paralyzed":
                    return StatusEffect.Paralyze;
                case "asleep":
                    return StatusEffect.Burn;
                case "confused":
                    return StatusEffect.Confuse;
                case "burned":
                    return StatusEffect.Burn;
                default:
                    return null;
            }
        }
    }
}
