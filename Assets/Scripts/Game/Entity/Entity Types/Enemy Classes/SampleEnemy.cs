using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Dogabeey
{
    public class SampleEnemy : EnemyEntity
    {
        public override float MaxHealth => baseMaxHealth;
        public override float Damage => baseDamage;
        public override float AttackRate => baseAttackRate;
        public override float Range => baseRange;
        public override float Speed => baseSpeed * Const.Values.ENEMY_SPEED_MULTIPLIER;
        public override float ProjectileSpeed => baseProjectileSpeed;

        public float shootingRange;
        public float playerLookDuration;

        protected override void Start()
        {
            base.Start();
            StartCoroutine(AttackSequence());
        }
        public override void AIUpdate()
        {
            agent.speed = Speed;
            // Chase the player if the player between Range and shootingRange. Shoot the player if below shootingRange.
            if (Vector3.Distance(transform.position, Player.Instance.transform.position) <= Range && Vector3.Distance(transform.position, Player.Instance.transform.position) > shootingRange)
            {
                agent.SetDestination(Player.Instance.transform.position);
                enemyState = EnemyState.Chase;
            }
            else if (Vector3.Distance(transform.position, Player.Instance.transform.position) <= shootingRange)
            {
                agent.SetDestination(transform.position);
                transform.DOLookAt(Player.Instance.transform.position, playerLookDuration);
                enemyState = EnemyState.Attack;
            }

        }
        public override void Attack(Entity target)
        {
            ThrowProjectile(target);
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

        private IEnumerator AttackSequence()
        {
            while (enabled)
            {
                yield return new WaitForSeconds(AttackRate);
                if (enemyState == EnemyState.Attack)
                {
                    Attack(Player.Instance);
                }
            }
        }

    }
}
