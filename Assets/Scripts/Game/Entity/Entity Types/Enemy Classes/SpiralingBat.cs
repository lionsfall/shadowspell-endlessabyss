using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;

namespace Dogabeey
{
    public class SpiralingBat : EnemyEntity
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

        [FoldoutGroup("Enemy-Specific Settings")]
        public float targetOrbitRadius = 1;
        [FoldoutGroup("Enemy-Specific Settings")]
        public float targetOrbitSpeed = 1;
        [FoldoutGroup("Enemy-Specific Settings")]
        public float orbitCloseInSpeed = 0.02f;

        Vector3 targetPos;
        float initialOrbitRadius;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(targetPos, 0.33f);
        }

        protected override void Start()
        {
            base.Start();
            StartCoroutine(AttackSequence());

            initialOrbitRadius = targetOrbitRadius;
        }
        public override void AIUpdate()
        {

            Creature target = targetCreature.GetCreature(this);
            if(target)
            {
                Debug.Log("Target is " + target.name);
                State = EntityState.Run;
                float distance = Vector3.Distance(transform.position, target.transform.position);
                // If target is in range, attack
                if (distance < Range)
                {
                    targetOrbitRadius -= orbitCloseInSpeed * Time.deltaTime * 60f;
                    if (targetOrbitRadius < 0)
                    {
                        targetOrbitRadius = 0;
                    }
                    EnemyState = EnemyState.Attack;
                }
                else
                {
                    targetOrbitRadius = initialOrbitRadius;
                    EnemyState = EnemyState.Chase;
                }

                targetPos = target.transform.position + (Mathf.Sin(Time.time * targetOrbitSpeed) * Vector3.right + Mathf.Cos(Time.time * targetOrbitSpeed) * Vector3.forward) * targetOrbitRadius;
                transform.DOMove(targetPos, distance / Speed);
                transform.DOLookAt(targetPos, 0.1f);
            }
            else
            {
                Debug.Log("Target is null");
                State = EntityState.Idle;
                EnemyState = EnemyState.Idle;
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
                if (EnemyState == EnemyState.Attack)
                {
                    Attack(Player.Instance);
                }
            }
        }
    }
}
