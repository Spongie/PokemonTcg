using CardEditor.Models;
using Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TCGCards;

namespace CardEditor.Util
{
    public static class PokemonCreator
    {
		public static PokemonCard CreateCardFromSdkCard(JsonPokemon pokemonSdk)
		{
			return CreateCardFromSdkCard(pokemonSdk.Card);
		}

		public static PokemonCard CreateCardFromSdkCard(PokemonTcgSdk.Models.PokemonCard pokemonSdk)
        {
			var pokemon = new PokemonCard
			{
				Name = pokemonSdk.Name,
				PokemonName = pokemonSdk.Name,
				Hp = int.Parse(pokemonSdk.Hp),
				ImageUrl = pokemonSdk.ImageUrlHiRes,
				EvolvesFrom = pokemonSdk.EvolvesFrom.Replace("-", string.Empty),
				SetCode = pokemonSdk.SetCode,
				Stage = SubTypToStage(pokemonSdk.SubType),
				RetreatCost = pokemonSdk.ConvertedRetreatCost,
				Type = convertFullTypeToType(pokemonSdk.Types.First())
			};

			pokemon.PokemonName = pokemon.PokemonName.Replace("Brock's", string.Empty)
				.Replace("Misty's", string.Empty)
				.Replace("Sabrina's", string.Empty)
				.Replace("Erika's", string.Empty)
				.Replace("Lt. Surge's", string.Empty)
				.Replace("Blaine's", string.Empty)
				.Replace("Giovanni's", string.Empty)
				.Replace("Koga's", string.Empty)
				.Replace("Rocket's", string.Empty).Trim();

			if (pokemonSdk.Weaknesses != null)
			{
				pokemon.Weakness = convertFullTypeToType(pokemonSdk.Weaknesses.First().Type);
			}
			else
			{
				pokemon.Weakness = EnergyTypes.None;
			}
			if (pokemonSdk.Resistances != null)
			{
				pokemon.Resistance = convertFullTypeToType(pokemonSdk.Resistances.First().Type);
			}
			else
			{
				pokemon.Resistance = EnergyTypes.None;
			}

			bool isComplete = true;

			if (pokemonSdk.Attacks == null)
            {
				pokemonSdk.Attacks = new List<PokemonTcgSdk.Models.Attack>();
			}

			foreach (var atk in pokemonSdk.Attacks)
			{
				var attack = new Attack
				{
					Name = atk.Name,
					Description = atk.Text,
					Cost = new ObservableCollection<Energy>(GenerateCost(atk.Cost)),
				};

				if (int.TryParse(atk.Damage, out int dmg))
                {
					attack.Damage = dmg;
                }
                else
                {
					isComplete = false;
                }

				if (!string.IsNullOrEmpty(attack.Description))
				{
					var attackOk = AttackCreator.TryAddEffects(ref attack, atk);

					isComplete = isComplete && attackOk;

					if (attackOk)
                    {
						pokemon.Attacks.Add(attack);
					}
				}
				else
				{
					pokemon.Attacks.Add(attack);
				}
			}

			if (pokemonSdk.Ability != null)
			{
				isComplete = false;
			}

			pokemon.Completed = isComplete;

			return pokemon;
		}

		private static List<Energy> GenerateCost(List<string> cost)
		{
			var costs = new List<Energy>();

			foreach (var typeCodeGroup in cost.GroupBy(x => x))
			{
				var type = getEnergyType(typeCodeGroup.Key.ToString());
				int count = typeCodeGroup.Count();

				costs.Add(new Energy(type, count));
			}

			return costs;
		}

		private static EnergyTypes getEnergyType(string type)
		{
			switch (type)
			{
				case "Psychic":
					return EnergyTypes.Psychic;
				case "Grass":
					return EnergyTypes.Grass;
				case "Fire":
					return EnergyTypes.Fire;
				case "Water":
					return EnergyTypes.Water;
				case "Colorless":
					return EnergyTypes.Colorless;
				case "Fighting":
					return EnergyTypes.Fighting;
				case "Lightning":
					return EnergyTypes.Electric;
				case "":
				case "none":
				case null:
					return EnergyTypes.None;
				default:
					throw new InvalidOperationException(type);
			}
		}

		private static EnergyTypes convertFullTypeToType(string type)
		{
			switch (type)
			{
				case "Psychic":
					return EnergyTypes.Psychic;
				case "Grass":
					return EnergyTypes.Grass;
				case "Fire":
					return EnergyTypes.Fire;
				case "Water":
					return EnergyTypes.Water;
				case "Colorless":
					return EnergyTypes.Colorless;
				case "Fighting":
					return EnergyTypes.Fighting;
				case "Lightning":
					return EnergyTypes.Electric;
				case "":
				case "none":
				case null:
					return EnergyTypes.None;
				default:
					throw new InvalidOperationException(type);
			}
		}

		private static int SubTypToStage(string subtype)
		{
			switch (subtype.ToLower())
			{
				case "basic":
					return 0;
				case "stage 1":
					return 1;
				case "stage 2":
					return 2;
				default:
					return 0;
			}
		}
	}
}
