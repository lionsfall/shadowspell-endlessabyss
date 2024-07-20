using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dogabeey
{
    public abstract class Entity : MonoBehaviour
    {
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

        }
    }
    public abstract class Creature : Entity
    {
        public float baseMaxHealth;
        public float baseDamage;
        public float baseAttackRate;
        public float baseSpeed;
        public float baseProjectileSpeed;

        protected EntityState state;


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
                        break;
                    default:
                        break;
                }   
            }
        }
        public abstract float MaxHealth { get; }
        public abstract float Damage { get; }
        public abstract float AttackRate { get; }
        public abstract float Speed { get; }
        public abstract float ProjectileSpeed { get; }

        /// <summary>
        /// When the entity is hurt by any source of damage.
        /// </summary>
        /// <param name="damageSource"></param>
        /// <param name="damage"></param>
        public abstract void OnHurt(Entity damageSource, float damage);
        /// <summary>
        /// When the entity dies.
        /// </summary>
        /// <param name="killer"></param>
        public abstract void OnDeath(Entity killer);
        /// <summary>
        /// When this entity DEALS damage to another entity.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="damage"></param>
        public abstract void OnDamage(Entity target, float damage);
        /// <summary>
        /// When this entity ATTACKS another entity, regardless of attack connects or not.
        /// </summary>
        /// <param name="target"></param>
        public abstract void OnAttack(Entity target);
    }

    public abstract class EnemyEntity : Creature
    {
        public override bool IsPlayer => false;
    }
}