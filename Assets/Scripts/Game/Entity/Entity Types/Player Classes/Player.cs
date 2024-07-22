using Dogabeey.SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Dogabeey
{
    public class Player : Creature
    {
        [Header("Projectile Settings")]
        public Projectile projectilePrefab;

        internal Projectile ProjectileInstance
        {
            get;
            set;
        }

        public override bool IsPlayer => true;
        public override float MaxHealth => baseMaxHealth;
        public override float Damage => baseDamage;
        public override float AttackRate => baseAttackRate;
        public override float Range => baseRange;
        public override float Speed => baseSpeed;
        public override float ProjectileSpeed => baseProjectileSpeed;

        public string SaveId => "Player";

        private void Awake()
        {

        }

        protected override void Start()
        {
            base.Start();

            StartCoroutine(AttackSequence());
            ProjectileInstance = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, Range);
        }

        private IEnumerator AttackSequence()
        {
            while (true)
            {
                yield return new WaitForSeconds(AttackRate);
                if (TryGetClosestEnemy(out EnemyEntity enemy))
                {
                    Attack(enemy);
                }
            }
        }

        private bool TryGetClosestEnemy(out EnemyEntity enemy)
        {
            enemy = null;
            float minDistance = Range;
            List<EnemyEntity> enemies = Physics.OverlapSphere(transform.position, Range).Select(x => x.GetComponent<EnemyEntity>()).Where(x => x != null).ToList();
            foreach (EnemyEntity e in enemies)
            {
                float distance = Vector3.Distance(transform.position, e.transform.position);
                if (distance <= minDistance)
                {
                    minDistance = distance;
                    enemy = e;
                }
            }
            return enemy != null;
        }

        public override void OnHurt(Entity damageSource, float damage)
        {
            base.OnHurt(damageSource, damage);
        }
        public override void OnDeath(Entity killer)
        {
            base.OnDeath(killer);
        }
        public override void OnDamage(Entity target, float damage)
        {
            base.OnDamage(target, damage);
        }
        public override void OnAttack(Entity target)
        {
            base.OnAttack(target);
        }

        public override void Attack(Entity target)
        {
            if (projectilePrefab != null)
            {
                Projectile p = Instantiate(ProjectileInstance, transform.position, Quaternion.identity);
                p.owner = this;
                p.target = target;
                p.transform.LookAt(target.transform);
                p.gameObject.SetActive(true);
            }
        }

        public override void Hurt(Entity damageSource, float damage)
        {
        }
    }
}