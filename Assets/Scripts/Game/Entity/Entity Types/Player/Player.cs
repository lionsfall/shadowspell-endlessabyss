namespace Dogabeey
{
    public class Player : Creature
    {
        public override bool IsPlayer => true;
        public override float MaxHealth => baseMaxHealth;
        public override float Damage => baseDamage;
        public override float AttackRate => baseAttackRate;
        public override float Speed => baseSpeed;
        public override float ProjectileSpeed => baseProjectileSpeed;

        public override void OnHurt(Entity damageSource, float damage)
        {
            throw new System.NotImplementedException();
        }
        public override void OnDeath(Entity killer)
        {
            throw new System.NotImplementedException();
        }
        public override void OnDamage(Entity target, float damage)
        {
            throw new System.NotImplementedException();
        }
        public override void OnAttack(Entity target)
        {
            throw new System.NotImplementedException();
        }
    }
}