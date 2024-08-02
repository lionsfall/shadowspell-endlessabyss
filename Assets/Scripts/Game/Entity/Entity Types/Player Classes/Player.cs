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
        public static Player Instance;


        [Header("Projectile Settings")]
        public Projectile projectilePrefab;

        internal Projectile ProjectileInstance
        {
            get;
            set;
        }

        [Header("Player Stats")]
        public List<MaxHealthModifier> maxHealthModifiers;
        public List<DamageModifier> damageModifiers;
        public List<AttackRateModifier> attackRateModifiers;
        public List<RangeModifier> rangeModifiers;
        public List<SpeedModifier> speedModifiers;
        public List<ProjectileSpeedModifier> projectileSpeedModifiers;

        internal List<Essence> essences = new List<Essence>();


        public override bool IsPlayer => true;
        public override float MaxHealth => MaxHealthModifier.CalculateValue(baseMaxHealth, maxHealthModifiers);
        public override float Damage => DamageModifier.CalculateValue(baseDamage, damageModifiers);
        public override float AttackRate => AttackRateModifier.CalculateValue(baseAttackRate, attackRateModifiers);
        public override float Range => RangeModifier.CalculateValue(baseRange, rangeModifiers);
        public override float Speed => SpeedModifier.CalculateValue(baseSpeed, speedModifiers);
        public override float ProjectileSpeed => ProjectileSpeedModifier.CalculateValue(baseProjectileSpeed, projectileSpeedModifiers);

        public string SaveId => "Player";

        private void Awake()
        {

        }

        protected override void Start()
        {
            base.Start();

            StartCoroutine(AttackSequence());
            ProjectileInstance = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

            Instance = this;

            InvokeRepeating(nameof(TickEssences), 1f, 1f);

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
                Creature enemy = targetCreature.GetCreature(this);
                if (enemy)
                {
                    Attack(enemy);
                }
            }
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

        public void AcquireEssence(Essence essence)
        {
            essences.Add(essence);
            essence.OnEssenceAcquired(this);
        }
        public void TickEssences()
        {
            foreach (Essence essence in essences)
            {
                essence.OnEssenceTick(this);
            }
        }
    }
}