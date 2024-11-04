using DG.Tweening;
using UnityEngine;
using System.Collections;

namespace Dogabeey
{
    public class Walker : EnemyEntity
    {
        public override float MaxHealth => baseMaxHealth;
        public override float Damage => baseDamage;
        public override float AttackRate => baseAttackRate;
        public override float Range => baseRange;
        public override float Speed => baseSpeed * Const.Values.ENEMY_SPEED_MULTIPLIER;
        public override float ProjectileSpeed => baseProjectileSpeed;

        public override EnemyState EnemyState
        {
            get => base.EnemyState; 
            set
            {
                switch (value)
                {
                    case EnemyState.Idle:
                        break;
                    case EnemyState.Flee:
                        break;
                    case EnemyState.Chase:
                        enemyMesh.material.DOColor(Color.black, "_EmissionColor", 0.1f);
                        break;
                    case EnemyState.Attack:
                        enemyMesh.material.DOColor(new Color(5.574994f, 0, 0.1f), "_EmissionColor", 0.1f);
                        break;
                    default:
                        break;
                }
                enemyState = value;
            }
        }

        protected override void Start()
        {
            base.Start();
            StartCoroutine(AttackSequence());
        }
        public override void AIUpdate()
        {
            base.AIUpdate();

            Creature target = targetCreature.GetCreature(this);
            if(target)
            {

                Debug.Log("Target is " + target.name);
                State = EntityState.Run;
                float distance = Vector3.Distance(transform.position, target.transform.position);
                // If target is in range, attack
                if (distance < Range)
                {
                    EnemyState = EnemyState.Attack;
                }
                else
                {
                    EnemyState = EnemyState.Chase;
                }


                agent.isStopped = false;
                agent.SetDestination(target.transform.position);
            }
            else
            {
                Debug.Log("Target is null");
                State = EntityState.Idle;
                EnemyState = EnemyState.Idle;

                agent.isStopped = true;
            }
        }

        public override void Attack(Entity target)
        {
            base.Attack(target);
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
            }
        }
    }
}
