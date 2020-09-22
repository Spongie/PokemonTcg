namespace Entities.Models
{
    public class Set : DataModel
    {
		private string setCode;

		public string SetCode
		{
			get { return setCode; }
			set
			{
				setCode = value;
				FirePropertyChanged();
			}
		}

		private string name;

		public string Name
		{
			get { return name; }
			set
			{
				name = value;
				FirePropertyChanged();
			}
		}

	}
}
