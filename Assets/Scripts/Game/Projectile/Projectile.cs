using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using Dogabeey.SimpleJSON;
using Sirenix.OdinInspector;

namespace Dogabeey
{

    public class Projectile : SerializedMonoBehaviour
    {
        [Flags]
        public enum ProjectileFlags
        {
            None = 0,
            PiercesEnemy = 1, // not implemented
            PiercesWalls = 2, // not implemented
            Bouncing = 4, // not implemented
            Homing = 8, // not implemented
            Aura = 16, // not implemented
        }

        public Creature owner;
        public Entity target;
        [Space]
        public List<ProjectileAction> onFire;
        public List<ProjectileAction> onTick;
        public List<ProjectileAction> onProjectileDeath;
        public List<ProjectileAction> onHit;
        [Space]
        public ProjectileFlags projectileFlags;
        public float tickTime = 1f;
        public float maxRange;
        public float uptime => owner.Range / owner.ProjectileSpeed;

        private Tween movementTween;

        private void Start()
        {
            FireProjectile();
            InvokeRepeating(nameof(OnTick), 0, tickTime);
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
                    if (!projectileFlags.HasFlag(ProjectileFlags.PiercesEnemy))
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
            if (target != null)
            {
                // Send projectile towards the target
                Vector3 towards = target.transform.position - transform.position;
                movementTween = transform.DOMove(owner.transform.position + towards.normalized * owner.Range, uptime / Const.Values.PROJECTILE_SPEED)
                    .SetEase(Ease.Linear)
                    .OnComplete(() => Destroy(gameObject));
            }
            else
            {
                movementTween = transform.DOMove(owner.transform.position + owner.transform.forward * owner.Range, uptime / Const.Values.PROJECTILE_SPEED)
                    .SetEase(Ease.Linear)
                    .OnComplete(() => Destroy(gameObject));
            }

        }
        public void FireStandaloneProjectile(Creature owner, Creature target)
        {
            OnFire();
            //TODO: Implement fire logic
            if (target != null)
            {   
                // Send projectile towards the target
                Vector3 towards = target.transform.position - transform.position;
                movementTween = transform.DOMove(owner.transform.position + towards.normalized * owner.Range, uptime / Const.Values.PROJECTILE_SPEED)
                    .SetEase(Ease.Linear)
                    .OnComplete(() => Destroy(gameObject));
            }
            else
            {
                movementTween = transform.DOMove(owner.transform.position + owner.transform.forward * owner.Range, uptime / Const.Values.PROJECTILE_SPEED)
                    .SetEase(Ease.Linear)
                    .OnComplete(() => Destroy(gameObject));
            }

        }

        public virtual void OnFire()
        {
            onFire.ForEach(o => o.Invoke(this));

            // Fire logic;
        }
        public virtual void OnTick()
        {
            onTick.ForEach(o => o.Invoke(this));
            // Update moevement target
            if (target != null && projectileFlags.HasFlag(ProjectileFlags.Homing))
            {
                Vector3 towards = target.transform.position - transform.position;
                movementTween = transform.DOMove(owner.transform.position + towards.normalized * owner.Range, uptime / Const.Values.PROJECTILE_SPEED)
                    .SetEase(Ease.Linear)
                    .OnComplete(() => Destroy(gameObject));
            }
        }
        public virtual void OnProjectileDeath()
        {
            onProjectileDeath.ForEach(o => o.Invoke(this));
        }
        public virtual void OnHit()
        {
            onHit.ForEach(o => o.Invoke(this));
        }

        public static explicit operator Projectile(JSONNode v)
        {
            var projectile = new Projectile
            {
                projectileFlags = (ProjectileFlags)(int)v["ProjectileFlags"],
                tickTime = (float)v["TickTime"],
                maxRange = (float)v["MaxRange"]
            };
            return projectile;
        }
    }
}