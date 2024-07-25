using UnityEngine;

namespace Dogabeey
{
    [System.Serializable]
    public class StatModifier
    {
        public float value;
        public ModifierType type;

        public StatModifier(float value, ModifierType type, int order, object source)
        {
            this.value = value;
            this.type = type;
        }
    }
    [System.Serializable]
    public class MaxHealthModifier : StatModifier
    {
        public MaxHealthModifier(float value, ModifierType type, int order, object source) : base(value, type, order, source)
        {
        }
    }
    [System.Serializable]
    public class DamageModifier : StatModifier
    {
        public DamageModifier(float value, ModifierType type, int order, object source) : base(value, type, order, source)
        {
        }
    }
    [System.Serializable]
    public class AttackRateModifier : StatModifier
    {
        public AttackRateModifier(float value, ModifierType type, int order, object source) : base(value, type, order, source)
        {
        }
    }
    [System.Serializable]
    public class RangeModifier : StatModifier
    {
        public RangeModifier(float value, ModifierType type, int order, object source) : base(value, type, order, source)
        {
        }
    }
    [System.Serializable]
    public class SpeedModifier : StatModifier
    {
        public SpeedModifier(float value, ModifierType type, int order, object source) : base(value, type, order, source)
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