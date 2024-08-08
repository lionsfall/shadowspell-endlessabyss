using Dogabeey.SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace Dogabeey
{
    public class Player : Creature
    {
        [System.Serializable]
        public class EssenceInstance
        {             
            public Essence essence;
            public int remainingLifeSpan;

            public EssenceInstance(Essence essence)
            {
                this.essence = essence;
                remainingLifeSpan = essence.lifeSpan;
            }
        }

        public static Player Instance;

        [BoxGroup("Base Stats")]
        public float baseMaxMana;
        [BoxGroup("Base Stats")]
        public float baseManaRegen;

        [Header("Projectile Settings")]
        public Projectile projectilePrefab;

        internal Projectile ProjectileInstance
        {
            get;
            set;
        }
        public float CurrentMana
        {
            get =>
                    PlayerPrefs.GetFloat("Mana_" + GetHashCode(), MaxMana);
            set
            {
                if (value <= 0)
                {
                    PlayerPrefs.SetFloat("Mana_" + GetHashCode(), 0);
                }
                if (value > MaxMana)
                {
                    PlayerPrefs.SetFloat("Mana_" + GetHashCode(), MaxMana);
                }
                else
                {
                    PlayerPrefs.SetFloat("Mana_" + GetHashCode(), value);
                }
            }
        }

        [Header("Player Stat Modifiers")]
        public List<MaxHealthModifier> maxHealthModifiers;
        public List<MaxManaModifier> maxManaModifiers;
        public List<ManaRegenModifier> manaRegenModifiers;
        public List<DamageModifier> damageModifiers;
        public List<AttackRateModifier> attackRateModifiers;
        public List<RangeModifier> rangeModifiers;
        public List<SpeedModifier> speedModifiers;
        public List<ProjectileSpeedModifier> projectileSpeedModifiers;

        internal List<EssenceInstance> essences = new List<EssenceInstance>();

        private float currentMana;

        public override bool IsPlayer => true;
        public override float MaxHealth => MaxHealthModifier.CalculateValue(baseMaxHealth, maxHealthModifiers);
        public float MaxMana => MaxManaModifier.CalculateValue(baseMaxMana, maxManaModifiers);
        public float ManaRegen => ManaRegenModifier.CalculateValue(baseManaRegen, manaRegenModifiers);
        public override float Damage => DamageModifier.CalculateValue(baseDamage, damageModifiers);
        public override float AttackRate => AttackRateModifier.CalculateValue(baseAttackRate, attackRateModifiers);
        public override float Range => RangeModifier.CalculateValue(baseRange, rangeModifiers);
        public override float Speed => SpeedModifier.CalculateValue(baseSpeed, speedModifiers);
        public override float ProjectileSpeed => ProjectileSpeedModifier.CalculateValue(baseProjectileSpeed, projectileSpeedModifiers);

        public string SaveId => "Player";

        private void OnEnable()
        {
            EventManager.StartListening(Const.GameEvents.PLAYER_ENTERED_ROOM, OnEnteringNewRoom);
        }
        private void OnDisable()
        {
            EventManager.StopListening(Const.GameEvents.PLAYER_ENTERED_ROOM, OnEnteringNewRoom);
        }
        public void OnEnteringNewRoom(EventParam e)
        {
            CurrentMana += ManaRegen;

            // Reduce life span of essences by 1. If any reaches zero, execute on expire effect and remove from list.
            for (int i = essences.Count - 1; i >= 0; i--)
            {
                essences[i].remainingLifeSpan--;
                if (essences[i].remainingLifeSpan <= 0)
                {
                    essences[i].essence.OnEssenceExpire(this);
                    essences.RemoveAt(i);
                }
            }
            EventManager.TriggerEvent(Const.GameEvents.PLAYER_ESSENCES_CHANGED);
        }

        private void Awake()
        {

        }

        protected override void Start()
        {
            base.Start();
            CurrentMana = MaxMana;
            

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

            if (!essence.executeAcquireEffectOnly)
            {
                essences.Add(new EssenceInstance(essence));
                EventManager.TriggerEvent(Const.GameEvents.PLAYER_ESSENCES_CHANGED);
            }
            essence.OnEssenceAcquired(this);
        }
        public void TickEssences()
        {
            foreach (EssenceInstance essenceInstance in essences)
            {
                essenceInstance.essence.OnEssenceTick(this);
            }
        }
    }
}