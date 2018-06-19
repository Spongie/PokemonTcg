using HtmlAgilityPack;
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

        public string Cost { get; set; }
        public string Name { get; set; }
        public string Damage { get; set; }
        public bool NeedsMore { get { return !string.IsNullOrEmpty(Description); } }
        public string Description { get; set; }

        public static Attack Parse(HtmlNode node)
        {
            string text = node.ChildNodes[0].InnerHtml;
            int costEnding = text.IndexOf(' ');
            string costString = text.Substring(0, costEnding).Replace("[", string.Empty).Replace("]", string.Empty);

            int nameEnding = text.IndexOf(':');
            string name = text.Substring(costEnding + 1, nameEnding - costEnding - 1);

            int damageEnding = text.IndexOf('.') - 1;
            string damage = text.Substring(nameEnding + 1, damageEnding - nameEnding).Replace("damage", string.Empty);

            string description = text.Substring(damageEnding + 2).Trim();

            if (!int.TryParse(damage.Trim(), out int _))
            {
                description = text.Substring(nameEnding + 1);
                damage = string.Empty;
            }

            return new Attack
            {
                Name = name.Trim(),
                Cost = costString.Trim(),
                Damage = damage.Trim(),
                Description = description.Trim()
            };
        }

        public string generateCode(string setName)
        {
            var template = Properties.Resources.AttackTemplate;
            string code = template.Replace(templateClassName, generateSafeClassName(Name));
            code = code.Replace(templateDamage, Damage);
            code = code.Replace(templateName, Name);
            code = code.Replace(templateDescription, Description);
            code = code.Replace(templateNeedsMode, NeedsMore ? "//TODO:" : string.Empty);
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
                    return "EnergyTypes.Lightning";
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
            return name.Replace("é", "e").Replace("’", "").Replace("!", string.Empty).Replace(" ", string.Empty);
        }
    }
}
