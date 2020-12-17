using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CardCreator
{
    public class CreatorPokemonCard
    {
        private const string templateHP = "{HP}";
        private const string templateName = "{NAME}";
        private const string templateType = "{TYPE}";
        private const string templateWeakness = "{WEAKNESS}";
        private const string templateResistence = "{RESISTENCE}";
        private const string templateRetreat = "{RETREAT}";
        private const string templateAttacks = "{ATTACKS}";
        private const string templatePokemonPower = "{POKEMONPOWER}";
        private const string templateEvolvesFrom = "{EVOLVESFROM}";
        private const string templateSetName = "{SETNAME}";
        private const string templateClassName = "{CLASSNAME}";
        private const string templateStage = "{STAGE}";

        public CreatorPokemonCard()
        {
            Attacks = new List<Attack>();
        }

        public CreatorPokemonCard(PokemonTcgSdk.Models.PokemonCard card) : this()
        {
            Hp = int.Parse(card.Hp);
            Type = card.Types.First();
            Name = card.Name;
            WeaknessResistenceInfo = new WeaknessResistenceInfo(card);
            Stage = subTypeToStage(card.SubType);
            if (Stage > 0)
            {
                EvolvesFrom = "//TODO: Evolve";
            }
            foreach (var attack in card.Attacks)
            {
                Attacks.Add(new Attack(attack));
            }
            HasPokemonPower = card.Ability != null;
        }

        public int Hp { get; set; }
        public string Type { get; set; }
        public int Stage { get; set; }
        public string Name { get; set; }
        public string EvolvesFrom { get; set; }
        public List<Attack> Attacks { get; set; }
        public WeaknessResistenceInfo WeaknessResistenceInfo { get; internal set; }
        public bool HasPokemonPower { get; internal set; }
        public string ClassName { get; set; }

        public string buildFromTemplate(string setName)
        {
            ClassName = generateSafeClassName(Name);

            var assembly = typeof(Program).GetTypeInfo().Assembly;
            using (Stream resource = assembly.GetManifestResourceStream("CardCreator.Resources.PokemonTemplate.txt"))
            using (var reader = new StreamReader(resource))
            {
                string template = reader.ReadToEnd();

                string code = template.Replace(templateHP, Hp.ToString());
                code = code.Replace(templateType, convertFullTypeToType(Type));
                code = code.Replace(templateName, Name);
                code = code.Replace(templateClassName, ClassName);
                code = code.Replace(templatePokemonPower, HasPokemonPower ? "//TODO: Pokemon power" : string.Empty);
                code = code.Replace(templateWeakness, convertFullTypeToType(WeaknessResistenceInfo.Weakness));
                code = code.Replace(templateResistence, convertFullTypeToType(WeaknessResistenceInfo.Resistence));
                code = code.Replace(templateRetreat, WeaknessResistenceInfo.RetreatCost.ToString());
                code = code.Replace(templateAttacks, string.Join("," + Environment.NewLine + "\t\t\t\t", Attacks.Select(x => x.generateAttackStringForPokemon())));
                code = code.Replace(templateStage, Stage.ToString());

                if (!string.IsNullOrEmpty(EvolvesFrom))
                {
                    code = code.Replace(templateEvolvesFrom, $"EvolvesFrom = \"{EvolvesFrom}\";");
                }
                else
                {
                    code = code.Replace(templateEvolvesFrom, string.Empty);
                }

                code = code.Replace(templateSetName, setName);

                return code;
            }
        }

        private static int subTypeToStage(string subType)
        {
            switch (subType)
            {
                case "Basic":
                    return 0;
                case "Stage 1":
                    return 1;
                case "Stage 2":
                    return 2;
                default:
                    throw new Exception(subType + " Invalid");
            }
        }

        private static string convertFullTypeToType(string type)
        {
            switch (type)
            {
                case "Psychic":
                    return "EnergyTypes.Psychic";
                case "Grass":
                    return "EnergyTypes.Grass";
                case "Fire":
                    return "EnergyTypes.Fire";
                case "Water":
                    return "EnergyTypes.Water";
                case "Colorless":
                    return "EnergyTypes.Colorless";
                case "Fighting":
                    return "EnergyTypes.Fighting";
                case "Lightning":
                    return "EnergyTypes.Electric";
                case "":
                case "none":
                case null:
                    return "EnergyTypes.None";
                default:
                    throw new InvalidOperationException(type);
            }
        }

        private static string generateSafeClassName(string name)
        {
            return name.Replace("-", string.Empty)
                .Replace("é", "e")
                .Replace("’", "")
                .Replace("!", string.Empty)
                .Replace(" ", string.Empty)
                .Replace("'", string.Empty)
                .Replace("♂", "Male");
        }
    }
}
