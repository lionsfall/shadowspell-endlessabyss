using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Events;

namespace Dogabeey
{
    public abstract class Entity : MonoBehaviour
    {

        public static List<Entity> entities;
        public enum EntityState
        {
            Idle,
            Run,
            Dead
        }

        internal Rigidbody2D rb;
        internal Collider2D cd;


        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            cd = GetComponent<Collider2D>();

            entities.Add(this);
        }
        private void OnDestroy()
        {
            entities.Remove(this);
        }
    }
    public abstract class Creature : Entity
    {
        public float baseMaxHealth;
        public float baseDamage;
        public float baseAttackRate;
        public float baseRange;
        public float baseSpeed;
        public float baseProjectileSpeed;
        [Space]
        public UnityEvent onHurt;
        public UnityEvent onDeath;
        public UnityEvent onDamage;
        public UnityEvent onAttack;

        protected EntityState state;

        private Entity lastDamager;
        private Entity lastAttacked;
        private Entity lastVictim;

        public abstract bool IsPlayer { get; }
        public float CurrentHealth
        {
            get => CurrentHealth;
            set
            {
                if (value <= 0)
                {
                    state = EntityState.Dead;
                    CurrentHealth = 0;
                }
                if (value > MaxHealth)
                {
                    CurrentHealth = MaxHealth;
                }
                else
                {
                    CurrentHealth = value;
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
                        OnDeath(null);
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
    }

    public abstract class EnemyEntity : Creature
    {
        public override bool IsPlayer => false;
    }
}