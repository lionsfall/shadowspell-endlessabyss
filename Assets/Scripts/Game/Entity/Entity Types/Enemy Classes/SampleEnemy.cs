using UnityEngine;

namespace Dogabeey
{
    public class SampleEnemy : EnemyEntity
    {
        public override float MaxHealth => baseMaxHealth;
        public override float Damage => baseDamage;
        public override float AttackRate => baseAttackRate;
        public override float Range => baseRange;
        public override float Speed => baseSpeed;
        public override float ProjectileSpeed => baseProjectileSpeed;


        public override void Attack(Entity target)
        {
            throw new System.NotImplementedException();
        }

        public override void Hurt(Entity damageSource, float damage, DamageType damageType)
        {
            base.Hurt(damageSource, damage, damageType);
        }

        public override void OnDeath(Entity killer)
        {
            base.OnDeath(killer);
            Destroy(gameObject);
        }

    }
}
