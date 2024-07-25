using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Dogabeey
{
    [System.Serializable]
    public class StatModifier
    {
        public float value;
        public ModifierType type;

        public StatModifier(float value, ModifierType type)
        {
            this.value = value;
            this.type = type;
        }

        public static float CalculateValue(float baseValue, List<StatModifier> stats)
        {
            List<StatModifier> percentPreFlat = stats.Where(x => x.type == ModifierType.PercentPreFlat).ToList();
            List<StatModifier> flat = stats.Where(x => x.type == ModifierType.Flat).ToList();
            List<StatModifier> percentPostFlat = stats.Where(x => x.type == ModifierType.PercentPostFlat).ToList();

            float percentPreFlatValue = percentPreFlat.Select(v => v.value).Aggregate((a, x) => a * x);
            float flatValue = flat.Select(v => v.value).Sum();
            float percentPostFlatValue = percentPostFlat.Select(v => v.value).Aggregate((a, x) => a * x);

            baseValue *= percentPreFlatValue;
            baseValue += flatValue;
            baseValue *= percentPostFlatValue;

            return baseValue;
        }
    }
    [System.Serializable]
    public class MaxHealthModifier : StatModifier
    {
        public MaxHealthModifier(float value, ModifierType type) : base(value, type)
        {
        }
    }
    [System.Serializable]
    public class DamageModifier : StatModifier
    {
        public DamageModifier(float value, ModifierType type) : base(value, type)
        {
        }
    }
    [System.Serializable]
    public class AttackRateModifier : StatModifier
    {
        public AttackRateModifier(float value, ModifierType type) : base(value, type)
        {
        }
    }
    [System.Serializable]
    public class RangeModifier : StatModifier
    {
        public RangeModifier(float value, ModifierType type) : base(value, type)
        {
        }
    }
    [System.Serializable]
    public class SpeedModifier : StatModifier
    {
        public SpeedModifier(float value, ModifierType type) : base(value, type)
        {
        }
    }
    public enum ModifierType
    {
        PercentPreFlat,
        Flat,
        PercentPostFlat
    }

}