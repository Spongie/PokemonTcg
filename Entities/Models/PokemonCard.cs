using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Entities.Models
{
	public class PokemonCard : DataModel
    {
		private string setCode;
		private string name;
		private int hp;
		private EnergyTypes type;
		private int retreatCost;
		private EnergyTypes weakness;
		private EnergyTypes resistance;
		private string evolvesFrom;
		private int stage;
		private string imageUrl;
		private ObservableCollection<Attack> attacks;

		public ObservableCollection<Attack> Attacks
		{
			get { return attacks; }
			set
			{
				attacks = value;
				FirePropertyChanged();
			}
		}


		public string ImageUrl
		{
			get { return imageUrl; }
			set
			{
				imageUrl = value;
				FirePropertyChanged();
			}
		}

		public string SetCode
		{
			get { return setCode; }
			set
			{
				setCode = value;
				FirePropertyChanged();
			}
		}

		public string Name
		{
			get { return name; }
			set
			{
				name = value;
				FirePropertyChanged();
			}
		}

		public int Hp
		{
			get { return hp; }
			set
			{
				hp = value;
				FirePropertyChanged();
			}
		}

		public EnergyTypes Type
		{
			get { return type; }
			set
			{
				type = value;
				FirePropertyChanged();
			}
		}

		public int RetreatCost
		{
			get { return retreatCost; }
			set
			{
				retreatCost = value;
				FirePropertyChanged();
			}
		}

		public EnergyTypes Weakness
		{
			get { return weakness; }
			set
			{
				weakness = value;
				FirePropertyChanged();
			}
		}

		public EnergyTypes Resistance
		{
			get { return resistance; }
			set
			{
				resistance = value;
				FirePropertyChanged();
			}
		}

		public string EvolvesFrom
		{
			get { return evolvesFrom; }
			set
			{
				evolvesFrom = value;
				FirePropertyChanged();
			}
		}

		public int Stage
		{
			get { return stage; }
			set
			{
				stage = value;
				FirePropertyChanged();
			}
		}

	}
}
