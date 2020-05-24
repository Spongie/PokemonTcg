using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

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
            Damage = Damage.Replace("+", string.Empty).Replace("×", string.Empty).Replace("-", string.Empty);

            foreach (var cost in attack.Cost)
            {
                Cost.Add(cost);
            }

            ClassName = generateSafeClassName(Name);
        }

        public List<string> Cost { get; set; } = new List<string>();
        public string Name { get; set; }
        public string Damage { get; set; }
        public bool NeedsMore { get { return !string.IsNullOrEmpty(Description); } }
        public string Description { get; set; }
        public string ClassName { get; set; }

        public string generateCode(string setName)
        {
            var assembly = typeof(Program).GetTypeInfo().Assembly;
            using (Stream resource = assembly.GetManifestResourceStream("CardCreator.Resources.AttackTemplate.txt"))
            using (var reader = new StreamReader(resource))
            {
                string template = reader.ReadToEnd();

                string code = template.Replace(templateClassName, ClassName);
                code = code.Replace(templateDamage, Damage);
                code = code.Replace(templateName, Name);
                code = code.Replace(templateDescription, Description);
                code = code.Replace(templateNeedsMode, NeedsMore ? "//TODO: Special effects" : string.Empty);
                code = code.Replace(templateSetName, setName);
                code = code.Replace(templateCost, generateCostString());

                return code;
            }
        }

        private string generateCostString()
        {
            var costs = new List<string>();

            foreach (var typeCodeGroup in Cost.GroupBy(x => x))
            {
                string typeName = getEnergyType(typeCodeGroup.Key.ToString());
                int count = typeCodeGroup.Count();

                costs.Add($"new Energy({typeName}, {count})");
            }

            return string.Join("," + Environment.NewLine + "\t\t\t\t", costs);
        }

        private string getEnergyType(string type)
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
