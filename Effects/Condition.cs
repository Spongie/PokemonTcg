using Entities.Effects;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Effects
{
	public enum ConditionType
	{
		Equals
	}

    public abstract class Condition : DataModel
    {
		private ConditionType conditionType;

		public ConditionType ConditionType
		{
			get { return conditionType; }
			set
			{
				conditionType = value;
				FirePropertyChanged();
			}
		}

		public abstract bool IsSatisfied(Dictionary<EffectValues, object> effectValues);
	}

    public class EffectValueCondition : Condition
    {
		private EffectValues valueType;
		private int targetValue;

		public int TargetValue
		{
			get { return targetValue; }
			set
			{
				targetValue = value;
				FirePropertyChanged();
			}
		}


		public EffectValues ValueType
		{
			get { return valueType; }
			set
			{
				valueType = value;
				FirePropertyChanged();
			}
		}

		public override bool IsSatisfied(Dictionary<EffectValues, object> effectValues)
		{
			if (!effectValues.ContainsKey(valueType))
			{
				return false;
			}

			var value = effectValues[valueType];

			bool r = (int)value > TargetValue;

			//TODO: Check condtion type
			return value.Equals(TargetValue);
		}
	}
}
