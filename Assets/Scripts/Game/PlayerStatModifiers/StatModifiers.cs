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

    }
    [System.Serializable]
    public class MaxHealthModifier : StatModifier
    {
        public MaxHealthModifier(float value, ModifierType type) : base(value, type)
        {
        }
        public static float CalculateValue(float baseValue, List<MaxHealthModifier> stats)
        {
            List<MaxHealthModifier> percentPreFlat = stats.Where(x => x.type == ModifierType.PercentPreFlat).ToList();
            List<MaxHealthModifier> flat = stats.Where(x => x.type == ModifierType.Flat).ToList();
            List<MaxHealthModifier> percentPostFlat = stats.Where(x => x.type == ModifierType.PercentPostFlat).ToList();
            float percentPreFlatValue = 1;
            float flatValue = 0;
            float percentPostFlatValue = 1;

            if (percentPreFlat.Count > 0)
            {
                percentPreFlatValue = percentPreFlat.Select(v => v.value).Aggregate((a, x) => a * x);
            }
            if (flat.Count > 0)
            {
                flatValue = flat.Select(v => v.value).Sum();
            }
            if (percentPostFlat.Count > 0)
            {
                percentPostFlatValue = percentPostFlat.Select(v => v.value).Aggregate((a, x) => a * x);
            }
            baseValue *= percentPreFlatValue;
            baseValue += flatValue;
            baseValue *= percentPostFlatValue;

            return baseValue;
        }
    }
    [System.Serializable]
    public class MaxManaModifier : StatModifier
    {
        public MaxManaModifier(float value, ModifierType type) : base(value, type)
        {
        }
        public static float CalculateValue(float baseValue, List<MaxManaModifier> stats)
        {
            List<MaxManaModifier> percentPreFlat = stats.Where(x => x.type == ModifierType.PercentPreFlat).ToList();
            List<MaxManaModifier> flat = stats.Where(x => x.type == ModifierType.Flat).ToList();
            List<MaxManaModifier> percentPostFlat = stats.Where(x => x.type == ModifierType.PercentPostFlat).ToList();
            float percentPreFlatValue = 1;
            float flatValue = 0;
            float percentPostFlatValue = 1;

            if (percentPreFlat.Count > 0)
            {
                percentPreFlatValue = percentPreFlat.Select(v => v.value).Aggregate((a, x) => a * x);
            }
            if (flat.Count > 0)
            {
                flatValue = flat.Select(v => v.value).Sum();
            }
            if (percentPostFlat.Count > 0)
            {
                percentPostFlatValue = percentPostFlat.Select(v => v.value).Aggregate((a, x) => a * x);
            }
            baseValue *= percentPreFlatValue;
            baseValue += flatValue;
            baseValue *= percentPostFlatValue;

            return baseValue;
        }
    }
    [System.Serializable]
    public class DamageModifier : StatModifier
    {
        public DamageModifier(float value, ModifierType type) : base(value, type)
        {
        }
        public static float CalculateValue(float baseValue, List<DamageModifier> stats)
        {
            List<DamageModifier> percentPreFlat = stats.Where(x => x.type == ModifierType.PercentPreFlat).ToList();
            List<DamageModifier> flat = stats.Where(x => x.type == ModifierType.Flat).ToList();
            List<DamageModifier> percentPostFlat = stats.Where(x => x.type == ModifierType.PercentPostFlat).ToList();
            float percentPreFlatValue = 1;
            float flatValue = 0;
            float percentPostFlatValue = 1;

            if (percentPreFlat.Count > 0)
            {
                percentPreFlatValue = percentPreFlat.Select(v => v.value).Aggregate((a, x) => a * x);
            }
            if (flat.Count > 0)
            {
                flatValue = flat.Select(v => v.value).Sum();
            }
            if (percentPostFlat.Count > 0)
            {
                percentPostFlatValue = percentPostFlat.Select(v => v.value).Aggregate((a, x) => a * x);
            }
            baseValue *= percentPreFlatValue;
            baseValue += flatValue;
            baseValue *= percentPostFlatValue;

            return baseValue;
        }
    }
    [System.Serializable]
    public class AttackRateModifier : StatModifier
    {
        public AttackRateModifier(float value, ModifierType type) : base(value, type)
        {
        }

        public static float CalculateValue(float baseValue, List<AttackRateModifier> stats)
        {
            List<AttackRateModifier> percentPreFlat = stats.Where(x => x.type == ModifierType.PercentPreFlat).ToList();
            List<AttackRateModifier> flat = stats.Where(x => x.type == ModifierType.Flat).ToList();
            List<AttackRateModifier> percentPostFlat = stats.Where(x => x.type == ModifierType.PercentPostFlat).ToList();
            float percentPreFlatValue = 1;
            float flatValue = 0;
            float percentPostFlatValue = 1;

            if (percentPreFlat.Count > 0)
            {
                percentPreFlatValue = percentPreFlat.Select(v => v.value).Aggregate((a, x) => a * x);
            }
            if (flat.Count > 0)
            {
                flatValue = flat.Select(v => v.value).Sum();
            }
            if (percentPostFlat.Count > 0)
            {
                percentPostFlatValue = percentPostFlat.Select(v => v.value).Aggregate((a, x) => a * x);
            }
            baseValue *= percentPreFlatValue;
            baseValue += flatValue;
            baseValue *= percentPostFlatValue;

            return baseValue;
        }
    }
    [System.Serializable]
    public class RangeModifier : StatModifier
    {
        public RangeModifier(float value, ModifierType type) : base(value, type)
        {
        }

        public static float CalculateValue(float baseValue, List<RangeModifier> stats)
        {
            List<RangeModifier> percentPreFlat = stats.Where(x => x.type == ModifierType.PercentPreFlat).ToList();
            List<RangeModifier> flat = stats.Where(x => x.type == ModifierType.Flat).ToList();
            List<RangeModifier> percentPostFlat = stats.Where(x => x.type == ModifierType.PercentPostFlat).ToList();
            float percentPreFlatValue = 1;
            float flatValue = 0;
            float percentPostFlatValue = 1;

            if (percentPreFlat.Count > 0)
            {
                percentPreFlatValue = percentPreFlat.Select(v => v.value).Aggregate((a, x) => a * x);
            }
            if (flat.Count > 0)
            {
                flatValue = flat.Select(v => v.value).Sum();
            }
            if (percentPostFlat.Count > 0)
            {
                percentPostFlatValue = percentPostFlat.Select(v => v.value).Aggregate((a, x) => a * x);
            }
            baseValue *= percentPreFlatValue;
            baseValue += flatValue;
            baseValue *= percentPostFlatValue;

            return baseValue;
        }
    }
    [System.Serializable]
    public class SpeedModifier : StatModifier
    {
        public SpeedModifier(float value, ModifierType type) : base(value, type)
        {
        }

        public static float CalculateValue(float baseValue, List<SpeedModifier> stats)
        {
            List<SpeedModifier> percentPreFlat = stats.Where(x => x.type == ModifierType.PercentPreFlat).ToList();
            List<SpeedModifier> flat = stats.Where(x => x.type == ModifierType.Flat).ToList();
            List<SpeedModifier> percentPostFlat = stats.Where(x => x.type == ModifierType.PercentPostFlat).ToList();
            float percentPreFlatValue = 1;
            float flatValue = 0;
            float percentPostFlatValue = 1;

            if (percentPreFlat.Count > 0)
            {
                percentPreFlatValue = percentPreFlat.Select(v => v.value).Aggregate((a, x) => a * x);
            }
            if (flat.Count > 0)
            {
                flatValue = flat.Select(v => v.value).Sum();
            }
            if (percentPostFlat.Count > 0)
            {
                percentPostFlatValue = percentPostFlat.Select(v => v.value).Aggregate((a, x) => a * x);
            }
            baseValue *= percentPreFlatValue;
            baseValue += flatValue;
            baseValue *= percentPostFlatValue;

            return baseValue;
        }
    }
    [System.Serializable]
    public class ProjectileSpeedModifier : StatModifier
    {
        public ProjectileSpeedModifier(float value, ModifierType type) : base(value, type)
        {
        }

        public static float CalculateValue(float baseValue, List<ProjectileSpeedModifier> stats)
        {
            List<ProjectileSpeedModifier> percentPreFlat = stats.Where(x => x.type == ModifierType.PercentPreFlat).ToList();
            List<ProjectileSpeedModifier> flat = stats.Where(x => x.type == ModifierType.Flat).ToList();
            List<ProjectileSpeedModifier> percentPostFlat = stats.Where(x => x.type == ModifierType.PercentPostFlat).ToList();
            float percentPreFlatValue = 1;
            float flatValue = 0;
            float percentPostFlatValue = 1;

            if (percentPreFlat.Count > 0)
            {
                percentPreFlatValue = percentPreFlat.Select(v => v.value).Aggregate((a, x) => a * x);
            }
            if (flat.Count > 0)
            {
                flatValue = flat.Select(v => v.value).Sum();
            }
            if (percentPostFlat.Count > 0)
            {
                percentPostFlatValue = percentPostFlat.Select(v => v.value).Aggregate((a, x) => a * x);
            }
            baseValue *= percentPreFlatValue;
            baseValue += flatValue;
            baseValue *= percentPostFlatValue;

            return baseValue;
        }
    }
    [System.Serializable]
    public class ManaRegenModifier : StatModifier
    {
        public ManaRegenModifier(float value, ModifierType type) : base(value, type)
        {
        }

        public static float CalculateValue(float baseValue, List<ManaRegenModifier> stats)
        {
            List<ManaRegenModifier> percentPreFlat = stats.Where(x => x.type == ModifierType.PercentPreFlat).ToList();
            List<ManaRegenModifier> flat = stats.Where(x => x.type == ModifierType.Flat).ToList();
            List<ManaRegenModifier> percentPostFlat = stats.Where(x => x.type == ModifierType.PercentPostFlat).ToList();
            float percentPreFlatValue = 1;
            float flatValue = 0;
            float percentPostFlatValue = 1;

            if (percentPreFlat.Count > 0)
            {
                percentPreFlatValue = percentPreFlat.Select(v => v.value).Aggregate((a, x) => a * x);
            }
            if (flat.Count > 0)
            {
                flatValue = flat.Select(v => v.value).Sum();
            }
            if (percentPostFlat.Count > 0)
            {
                percentPostFlatValue = percentPostFlat.Select(v => v.value).Aggregate((a, x) => a * x);
            }
            baseValue *= percentPreFlatValue;
            baseValue += flatValue;
            baseValue *= percentPostFlatValue;

            return baseValue;
        }
    }
    public enum ModifierType
    {
        PercentPreFlat,
        Flat,
        PercentPostFlat
    }

}