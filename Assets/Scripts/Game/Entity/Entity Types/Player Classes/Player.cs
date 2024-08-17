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
        [System.Serializable]
        public class CountedDamageImmunity
        {
            public DamageType damageType;
            public float count;
        }

        public static Player Instance;

        [BoxGroup("Base Stats")]
        public float baseMaxMana;
        [BoxGroup("Base Stats")]
        public float baseManaRegen;
        [BoxGroup("Misc")]
        public List<CountedDamageImmunity> damageImmunities;


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

        [Header("Player References")]
        public MeshRenderer playerMesh;
        [BoxGroup("Player Stat Modifiers")]
        public List<MaxHealthModifier> maxHealthModifiers;
        [BoxGroup("Player Stat Modifiers")]
        public List<MaxManaModifier> maxManaModifiers;
        [BoxGroup("Player Stat Modifiers")]
        public List<ManaRegenModifier> manaRegenModifiers;
        [BoxGroup("Player Stat Modifiers")]
        public List<DamageModifier> damageModifiers;
        [BoxGroup("Player Stat Modifiers")]
        public List<AttackRateModifier> attackRateModifiers;
        [BoxGroup("Player Stat Modifiers")]
        public List<RangeModifier> rangeModifiers;
        [BoxGroup("Player Stat Modifiers")]
        public List<SpeedModifier> speedModifiers;
        [BoxGroup("Player Stat Modifiers")]
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
                    RemoveEssence(i);
                }
            }
            EventManager.TriggerEvent(Const.GameEvents.PLAYER_ESSENCES_CHANGED);
        }

        public void RemoveEssence(int i)
        {
            essences[i].essence.OnEssenceExpire(this);
            essences.RemoveAt(i);
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
        private void Update()
        {
            // Quickly turn on-off player's mesh renderer based on invincibility duration.
            playerMesh.enabled = !IsInvincible || Time.time % 0.2f > 0.1f;
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
            ThrowProjectile(target);
        }


        public override void Hurt(Entity damageSource, float damage, DamageType damageType)
        {
            // If has counted immunity against this damage type, don't execute the hurt and Remove its stack by 1.
            CountedDamageImmunity immunity = damageImmunities.FirstOrDefault(i => i.damageType == damageType);
            if (immunity != null)
            {
                RemoveImmunityCount(damageType, 1);
                return;
            }
            else
            {
                base.Hurt(damageSource, damage, damageType);
            }
        }
        public void AcquireEssence(Essence essence)
        {

            if (!essence.acquireEffectOnly)
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

        internal void MassDamage(float damage, float radius)
        {
            // Damages every mob around player in a radius by given damage.
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent(out EnemyEntity creature))
                {
                    creature.Hurt(this, damage, DamageType.None);
                }
            }
        }

        internal void AddImmunity(DamageType damageType, float count)
        {
            CountedDamageImmunity immunity = damageImmunities.FirstOrDefault(i => i.damageType == damageType);
            if (immunity == null)
            {
                immunity = new CountedDamageImmunity
                {
                    damageType = damageType
                };
                damageImmunities.Add(immunity);
            }
            immunity.count += count;
        }
        internal void RemoveImmunityCount(DamageType damageType, float count)
        {
            CountedDamageImmunity immunity = damageImmunities.FirstOrDefault(i => i.damageType == damageType);
            if (immunity != null)
            {
                immunity.count -= count;
                if (immunity.count <= 0)
                {
                    damageImmunities.Remove(immunity);
                }
            }
        }

        internal void RemoveImmunity(DamageType damageType)
        {
            CountedDamageImmunity immunity = damageImmunities.FirstOrDefault(i => i.damageType == damageType);
            if (immunity != null)
            {
                damageImmunities.Remove(immunity);
            }
        }
    }
}