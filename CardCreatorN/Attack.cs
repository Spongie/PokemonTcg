using System;
using System.Collections.Generic;
using System.Linq;

namespace CardCreator
{
    public class Attack
    {
        private const string templateCost = "{COST}";
        private const string templateName = "{NAME}";
        private const string templateDescription = "{DESCRIPTION}";
        private const string templateSetName = "{SETNAME}";
        private const string templateClassName = "{CLASSNAME}";
        private const string templateDamage = "{DAMAGE}";
        private const string templateNeedsMode = "{NEEDSMORE}";

        public Attack(PokemonTcgSdk.Models.Attack attack)
        {
            Name = attack.Name;
            Description = attack.Text;
            Damage = string.IsNullOrWhiteSpace(attack.Damage) ? "0" : attack.Damage;

            foreach (var cost in attack.Cost)
            {
                Cost += cost.Substring(0, 1);
            }

            ClassName = generateSafeClassName(Name);
        }

        public string Cost { get; set; }
        public string Name { get; set; }
        public string Damage { get; set; }
        public bool NeedsMore { get { return !string.IsNullOrEmpty(Description); } }
        public string Description { get; set; }
        public string ClassName { get; set; }

        public string generateCode(string setName)
        {
            var template = Properties.Resources.AttackTemplate;
            string code = template.Replace(templateClassName, ClassName);
            code = code.Replace(templateDamage, Damage);
            code = code.Replace(templateName, Name);
            code = code.Replace(templateDescription, Description);
            code = code.Replace(templateNeedsMode, NeedsMore ? "//TODO: Special effects" : string.Empty);
            code = code.Replace(templateSetName, setName);
            code = code.Replace(templateCost, generateCostString());

            return code;
        }

        private string generateCostString()
        {
            var costs = new List<string>();

            foreach (var typeCodeGroup in Cost.ToCharArray().GroupBy(x => x))
            {
                string typeName = getEnergyType(typeCodeGroup.Key.ToString());
                int count = typeCodeGroup.Count();

                costs.Add($"new Energy({typeName}, {count})");
            }

            return string.Join("," + Environment.NewLine + "\t\t\t\t", costs);
        }

        private string getEnergyType(string typeCode)
        {
            switch (typeCode)
            {
                case "P":
                    return "EnergyTypes.Psychic";
                case "G":
                    return "EnergyTypes.Grass";
                case "R":
                    return "EnergyTypes.Fire";
                case "W":
                    return "EnergyTypes.Water";
                case "C":
                    return "EnergyTypes.Colorless";
                case "F":
                    return "EnergyTypes.Fighting";
                case "L":
                    return "EnergyTypes.Electric";
                case "":
                case "none":
                    return "EnergyTypes.None";
                default:
                    throw new InvalidOperationException(typeCode);
            }
        }

        public string generateAttackStringForPokemon()
        {
            return $"new {generateSafeClassName(Name)}()";
        }

        private static string generateSafeClassName(string name)
        {
            return name.Replace("é", "e").Replace("’", "").Replace("-", string.Empty).Replace("!", string.Empty).Replace(" ", string.Empty);
        }
    }
}
