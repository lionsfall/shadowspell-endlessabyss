using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace Dogabeey
{
    public abstract class Entity : SerializedMonoBehaviour
    {

        public static List<Entity> entities = new List<Entity>();
        public enum EntityState
        {
            Idle,
            Run,
            Dead
        }

        internal Rigidbody rb;
        internal Collider cd;


        // Start is called before the first frame update
        protected virtual void Start()
        {
            rb = GetComponent<Rigidbody>();
            cd = GetComponent<Collider>();

            entities.Add(this);
        }
        protected virtual void OnDestroy()
        {
            entities.Remove(this);
        }

    }
    public abstract class Creature : Entity
    {
        [BoxGroup("Base Stats")]
        public float baseMaxHealth;
        [BoxGroup("Base Stats")]
        public float baseDamage;
        [BoxGroup("Base Stats")]
        public float baseAttackRate;
        [BoxGroup("Base Stats")]
        public float baseRange;
        [BoxGroup("Base Stats")]
        public float baseSpeed;
        [BoxGroup("Base Stats")]
        public float baseProjectileSpeed;
        [Space]
        [BoxGroup("Events")]
        public UnityEvent onHurt;
        [BoxGroup("Events")]
        public UnityEvent onDeath;
        [BoxGroup("Events")]
        public UnityEvent onDamage;
        [BoxGroup("Events")]
        public UnityEvent onAttack;
        [BoxGroup("Events")]
        public UnityEvent onKill;
        [BoxGroup("Misc")]
        public CreaturePicker targetCreature;
        [BoxGroup("Projectile Settings")]
        public Projectile projectilePrefab;
        internal Projectile ProjectileInstance
        {
            get;
            set;
        }

        protected EntityState state;

        private Entity lastDamager;
        private Entity lastAttacked;
        private Entity lastVictim;

        public abstract bool IsPlayer { get; }
        public float CurrentHealth
        {
            get =>
                    PlayerPrefs.GetFloat("Health_" + GetHashCode(), MaxHealth);
            set
            {
                if (value <= 0)
                {
                    State = EntityState.Dead;
                    PlayerPrefs.SetFloat("Health_" + GetHashCode(), 0);
                }
                if (value > MaxHealth)
                {
                    PlayerPrefs.SetFloat("Health_" + GetHashCode(), MaxHealth);
                }
                else
                {
                    PlayerPrefs.SetFloat("Health_" + GetHashCode(), value);
                }
            }
        }
        public EntityState State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
                switch (state)
                {
                    case EntityState.Idle:
                        break;
                    case EntityState.Run:
                        break;
                    case EntityState.Dead:
                        OnDeath(lastDamager);
                        break;
                    default:
                        break;
                }   
            }
        }
        public abstract float MaxHealth { get; }
        public abstract float Damage { get; }
        public abstract float AttackRate { get; }
        public abstract float Range { get; }
        public abstract float Speed { get; }
        public abstract float ProjectileSpeed { get; }

        protected virtual float InvincibilityDuration => 0.1f;
        
        internal bool IsInvincible => Time.time - lastHurtTime < InvincibilityDuration;

        private float lastHurtTime;

        protected override void Start()
        {
            base.Start();
            CurrentHealth = MaxHealth;

            ProjectileInstance = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        }

        /// <summary>
        /// When the entity is hurt by any source of damage.
        /// </summary>
        /// <param name="damageSource"></param>
        /// <param name="damage"></param>
        public virtual void OnHurt(Entity damageSource, float damage)
        {
            onHurt.Invoke();
            lastDamager = damageSource;
        }
        /// <summary>
        /// When the entity dies.
        /// </summary>
        /// <param name="killer"></param>
        public virtual void OnDeath(Entity killer)
        {
            onDeath.Invoke();
        }
        /// <summary>
        /// When this entity DEALS damage to another entity.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="damage"></param>
        public virtual void OnDamage(Entity target, float damage)
        {
            onDamage.Invoke();
            lastAttacked = target;
        }
        /// <summary>
        /// When this entity ATTACKS another entity, regardless of attack connects or not.
        /// </summary>
        /// <param name="target"></param>
        public virtual void OnAttack(Entity target)
        {
            onAttack.Invoke();
        }
        public virtual void OnKill(Entity target)
        {
            lastVictim = target;
        }

        public abstract void Attack(Entity target);
        public virtual void Hurt(Entity damageSource, float damage, DamageType damageType)
        {
            OnHurt(damageSource, damage);
            CurrentHealth -= damage;
            lastHurtTime = Time.time;
        }

        internal virtual void Heal(float healAmount)
        {
            CurrentHealth += healAmount;
        }
        protected void ThrowProjectile(Entity target)
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
    }

    public enum DamageType
    {
        None,
        Contact,
        Projectile,
        Magic
    }
}