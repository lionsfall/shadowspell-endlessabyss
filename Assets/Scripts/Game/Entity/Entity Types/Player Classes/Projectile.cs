using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

namespace Dogabeey
{
    public class Projectile : MonoBehaviour
    {
        public enum ProjectileType
        {
            None,
            Fire,
            Ice,
            Poison,
            Lightning
        }
        [Flags]
        public enum ProjectileFlags
        {
            None = 0,
            Piercing = 1,
            Bouncing = 2,
            Homing = 4,
        }

        public Creature owner;
        public Entity target;
        [Space]
        public UnityEvent onFire;
        public UnityEvent onTick;
        public UnityEvent onProjectileDeath;
        public UnityEvent onHit;
        [Space]
        public ProjectileType projectileType;
        public ProjectileFlags projectileFlags;
        public float tickTime = 1f;
        public float uptime = 5f;

        private void Start()
        {
            FireProjectile();
            InvokeRepeating(nameof(OnTick), 0, tickTime);
            Destroy(gameObject, uptime);
        }
        private void OnDestroy()
        {
            OnProjectileDeath();
        }
        private void OnTriggerEnter(Collider collider)
        {
            if(collider.gameObject.TryGetComponent(out Creature entity))
            {
                if((owner is Player && entity is EnemyEntity) || (owner is EnemyEntity && entity is Player))
                {
                    OnHit();
                    entity.Hurt(owner, owner.Damage);
                    //Destroy the projectile if has no piercing flag
                    if (!projectileFlags.HasFlag(ProjectileFlags.Piercing))
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }

        /// <summary>
        /// Fires the projectile agianst the target. If the target is null, the projectile will be fired in the direction of the owner. If projectile has homing flag, it will follow the target.
        /// </summary>
        public void FireProjectile()
        {
            OnFire();
            //TODO: Implement fire logic
            if(target != null)
            {
                // Send projectile towards the target
                Vector3 towards = target.transform.position - transform.position;
                transform.DOMove(owner.transform.position + towards * 100, Vector3.Distance(towards * 100, transform.position))
                    .SetEase(Ease.Linear);
            }
            else
            {
                transform.DOMove(owner.transform.position + owner.transform.forward * 100, 
                    Vector3.Distance(owner.transform.forward * 100, owner.transform.position))
                    .SetEase(Ease.Linear);
            }

        }

        public virtual void OnFire()
        {
            onFire.Invoke();

            // Fire logic;
        }
        public virtual void OnTick()
        {
            onTick.Invoke();
        }
        public virtual void OnProjectileDeath()
        {
            onProjectileDeath.Invoke();
        }
        public virtual void OnHit()
        {
            onHit.Invoke();
        }
    }
}