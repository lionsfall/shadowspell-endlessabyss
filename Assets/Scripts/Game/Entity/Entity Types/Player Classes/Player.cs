using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Dogabeey
{
    public class Player : Creature
    {
        public Projectile projectile;

        public override bool IsPlayer => true;
        public override float MaxHealth => baseMaxHealth;
        public override float Damage => baseDamage;
        public override float AttackRate => baseAttackRate;
        public override float Range => baseRange;
        public override float Speed => baseSpeed;
        public override float ProjectileSpeed => baseProjectileSpeed;

        private void Start()
        {
            StartCoroutine(AttackSequence());
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
                if (distance < minDistance)
                {
                    minDistance = distance;
                    enemy = e;
                }
            }
            return enemy != null;
        }

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

        public override void Attack(Entity target)
        {
            throw new System.NotImplementedException();
        }
    }
}