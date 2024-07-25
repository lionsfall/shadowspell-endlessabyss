using UnityEngine;

namespace Dogabeey
{
    [System.Serializable]
    public class HealPlayerAction : PlayerAction
    {
        public float amount;
        public override void Invoke(Player player)
        {
            player.Heal(amount);
        }
    }
    [System.Serializable]
    public class FullHealthAction : PlayerAction
    {
        public override void Invoke(Player player)
        {
            player.Heal(player.MaxHealth);
        }
    }
    [System.Serializable]
    public class DamagePlayerAction : PlayerAction
    {
        public float amount;
        public override void Invoke(Player player)
        {
            player.Hurt(player, amount);
        }
    }
    [System.Serializable]
    public class AddDamageModifier : PlayerAction
    {
        public DamageModifier modifier;
        public override void Invoke(Player player)
        {
            player.damageModifiers.Add(modifier);
        }
    }
    [System.Serializable]
    public class AddMaxHealthModifier : PlayerAction
    {
        public MaxHealthModifier modifier;
        public override void Invoke(Player player)
        {
            player.maxHealthModifiers.Add(modifier);
        }
    }
    [System.Serializable]
    public class AddAttackRateModifier : PlayerAction
    {
        public AttackRateModifier modifier;
        public override void Invoke(Player player)
        {
            player.attackRateModifiers.Add(modifier);
        }
    }
    [System.Serializable]
    public class AddRangeModifier : PlayerAction
    {
        public RangeModifier modifier;
        public override void Invoke(Player player)
        {
            player.rangeModifiers.Add(modifier);
        }
    }
    [System.Serializable]
    public class AddSpeedModifier : PlayerAction
    {
        public SpeedModifier modifier;
        public override void Invoke(Player player)
        {
            player.speedModifiers.Add(modifier);
        }
    }
    [System.Serializable]
    public class RemoveDamageModifier : PlayerAction
    {
        public DamageModifier modifier;
        public override void Invoke(Player player)
        {
            player.damageModifiers.Remove(modifier);
        }
    }
    [System.Serializable]
    public class RemoveMaxHealthModifier : PlayerAction
    {
        public MaxHealthModifier modifier;
        public override void Invoke(Player player)
        {
            player.maxHealthModifiers.Remove(modifier);
        }
    }
    [System.Serializable]
    public class RemoveAttackRateModifier : PlayerAction
    {
        public AttackRateModifier modifier;
        public override void Invoke(Player player)
        {
            player.attackRateModifiers.Remove(modifier);
        }
    }
    [System.Serializable]
    public class RemoveRangeModifier : PlayerAction
    {
        public RangeModifier modifier;
        public override void Invoke(Player player)
        {
            player.rangeModifiers.Remove(modifier);
        }
    }
    [System.Serializable]
    public class RemoveSpeedModifier : PlayerAction
    {
        public SpeedModifier modifier;
        public override void Invoke(Player player)
        {
            player.speedModifiers.Remove(modifier);
        }
    }
}
