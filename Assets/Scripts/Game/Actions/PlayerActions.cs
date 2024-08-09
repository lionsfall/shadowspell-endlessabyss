using UnityEngine;

namespace Dogabeey
{
    [System.Serializable]
    public class DebugMessageAction : PlayerAction
    {
        public string message;
        public override void Invoke(Player player)
        {
            Debug.Log(message);
        }
    }
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
    public class RestoreManaAction : PlayerAction
    {
        public float amount;
        public override void Invoke(Player player)
        {
            player.CurrentMana += amount;
        }
    }
    [System.Serializable]
    public class FullManaAction : PlayerAction
    {
        public override void Invoke(Player player)
        {
            player.CurrentMana = player.MaxMana;
        }
    }
    [System.Serializable]
    public class DamagePlayerAction : PlayerAction
    {
        public float amount;
        public DamageType damageType;
        public override void Invoke(Player player)
        {
            player.Hurt(player, amount, damageType);
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
    public class AddProjectileSpeedModifier : PlayerAction
    {
        public ProjectileSpeedModifier modifier;
        public override void Invoke(Player player)
        {
            player.projectileSpeedModifiers.Add(modifier);
        }
    }
    [System.Serializable]
    public class AddMaxManaModifier : PlayerAction
    {
        public MaxManaModifier modifier;
        public override void Invoke(Player player)
        {
            player.maxManaModifiers.Add(modifier);
        }
    }
    [System.Serializable]
    public class AddManaRegenModifier : PlayerAction
    {
        public ManaRegenModifier modifier;
        public override void Invoke(Player player)
        {
            player.manaRegenModifiers.Add(modifier);
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
    [System.Serializable]
    public class RemoveProjectileSpeedModifier : PlayerAction
    {
        public ProjectileSpeedModifier modifier;
        public override void Invoke(Player player)
        {
            player.projectileSpeedModifiers.Remove(modifier);
        }
    }
    [System.Serializable]
    public class AddProjectileActionOnFire : PlayerAction
    {
        public ProjectileAction action;
        public override void Invoke(Player player)
        {
            player.ProjectileInstance.onFire.Add(action);
        }
    }
    [System.Serializable]
    public class AddProjectileActionOnHit : PlayerAction
    {
        public ProjectileAction action;
        public override void Invoke(Player player)
        {
            player.ProjectileInstance.onHit.Add(action);
        }
    }
    [System.Serializable]
    public class AddProjectileActionOnProjectileDeath : PlayerAction
    {
        public ProjectileAction action;
        public override void Invoke(Player player)
        {
            player.ProjectileInstance.onProjectileDeath.Add(action);
        }
    }
    [System.Serializable]
    public class AddProjectileActionOnTick : PlayerAction
    {
        public ProjectileAction action;
        public override void Invoke(Player player)
        {
            player.ProjectileInstance.onTick.Add(action);
        }
    }
    [System.Serializable]
    public class FireProjectileAction : PlayerAction
    {
        public Projectile projectilePrefab;
        public CreaturePicker targetPicker;
        public override void Invoke(Player player)
        {
            player.projectilePrefab.FireStandaloneProjectile(player, targetPicker.GetCreature(player));
        }
    }
    [System.Serializable]
    public class AddEssenceAction : PlayerAction
    {
        public Essence essence;
        public override void Invoke(Player player)
        {
            player.AcquireEssence(essence);
        }
    }
    [System.Serializable]
    public class RemoveEssenceAction : PlayerAction
    {
        public Essence essence;
        public override void Invoke(Player player)
        {
            player.RemoveEssence(essence.essenceID);
        }
    }
    [System.Serializable]
    public class AddCoin : PlayerAction
    {
        public int amount;
        public override void Invoke(Player player)
        {
            CurrencyManager.Instance.AddCoin(amount);
        }
    }
    [System.Serializable]
    public class AddCurrencyAction : PlayerAction
    {
        public CurrencyModel currency;
        public int amount;
        public override void Invoke(Player player)
        {
            CurrencyManager.Instance.AddCurrency(currency.currencyID, amount);
        }
    }
    [System.Serializable]
    public class MassDamageAction : PlayerAction
    {
        public float damage;
        public float radius;
        public override void Invoke(Player player)
        {
            player.MassDamage(damage, radius);
        }
    }
    [System.Serializable]
    public class AddOnHurtAction : PlayerAction
    {
        public PlayerAction action;
        public override void Invoke(Player player)
        {
            player.onHurt.AddListener(() => action.Invoke(player));
        }
    }
    [System.Serializable]
    public class AddOnDeathAction : PlayerAction
    {
        public PlayerAction action;
        public override void Invoke(Player player)
        {
            player.onDeath.AddListener(() => action.Invoke(player));
        }
    }
    [System.Serializable]
    public class AddOnDamageAction : PlayerAction
    {
        public PlayerAction action;
        public override void Invoke(Player player)
        {
            player.onDamage.AddListener(() => action.Invoke(player));
        }
    }
    [System.Serializable]
    public class AddOnAttackAction : PlayerAction
    {
        public PlayerAction action;
        public override void Invoke(Player player)
        {
            player.onAttack.AddListener(() => action.Invoke(player));
        }
    }
    [System.Serializable]
    public class AddOnKillAction : PlayerAction
    {
        public PlayerAction action;
        public override void Invoke(Player player)
        {
            player.onKill.AddListener(() => action.Invoke(player));
        }
    }
    [System.Serializable]
    public class RemoveOnHurtAction : PlayerAction
    {
        public PlayerAction action;
        public override void Invoke(Player player)
        {
            player.onHurt.RemoveListener(() => action.Invoke(player));
        }
    }
    [System.Serializable]
    public class RemoveOnDeathAction : PlayerAction
    {
        public PlayerAction action;
        public override void Invoke(Player player)
        {
            player.onDeath.RemoveListener(() => action.Invoke(player));
        }
    }
    [System.Serializable]
    public class RemoveOnDamageAction : PlayerAction
    {
        public PlayerAction action;
        public override void Invoke(Player player)
        {
            player.onDamage.RemoveListener(() => action.Invoke(player));
        }
    }
    [System.Serializable]
    public class RemoveOnAttackAction : PlayerAction
    {
        public PlayerAction action;
        public override void Invoke(Player player)
        {
            player.onAttack.RemoveListener(() => action.Invoke(player));
        }
    }
    [System.Serializable]
    public class RemoveOnKillAction : PlayerAction
    {
        public PlayerAction action;
        public override void Invoke(Player player)
        {
            player.onKill.RemoveListener(() => action.Invoke(player));
        }
    }
    [System.Serializable]
    public class AddImmunityCount : PlayerAction
    {
        public DamageType damageType;
        public int count;
        public override void Invoke(Player player)
        {
            player.AddImmunity(damageType, count);
        }
    }
    [System.Serializable]
    public class RemoveImmunityCount : PlayerAction
    {
        public DamageType damageType;
        public int count;
        public override void Invoke(Player player)
        {
            player.RemoveImmunityCount(damageType, count);
        }
    }
    [System.Serializable]
    public class RemoveImmunity : PlayerAction
    {
        public DamageType damageType;
        public override void Invoke(Player player)
        {
            player.RemoveImmunity(damageType);
        }
    }

}
