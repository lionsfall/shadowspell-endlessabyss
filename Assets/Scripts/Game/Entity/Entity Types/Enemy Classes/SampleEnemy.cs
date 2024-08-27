using DG.Tweening;
using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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

        protected override void Start()
        {
            base.Start();
            StartCoroutine(AttackSequence());
        }
        public override void AIUpdate()
        {
            if(Player.Instance == null)
            {
                return;
            }


            agent.speed = Speed;
            // Chase the player if the player between Range and shootingRange. Shoot the player if below shootingRange.
            if (Vector3.Distance(transform.position, Player.Instance.transform.position) > Range)
            {
                agent.isStopped = true;
                EnemyState = EnemyState.Idle;
                State = EntityState.Idle;
            }
            else
            {
                agent.isStopped = false;
                agent.SetDestination(Player.Instance.transform.position);
                transform.DOLookAt(Player.Instance.transform.position, 0);
                EnemyState = EnemyState.Chase;
                State = EntityState.Run;
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
                if (EnemyState == EnemyState.Attack)
                {
                    Attack(Player.Instance);
                }
            }
        }

    }
}
