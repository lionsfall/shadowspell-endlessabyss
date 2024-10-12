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
        [Tooltip("Speed multiplier for the projectile. 0 disables the movement entirely and generally should be used together with keepAsOwnerChild")]
        public float speedMultiplier = 1f;
        public bool keepAsOwnerChild = false;
        [Space]
        public ParticleSystem deathParticle;

        public virtual float Uptime => owner.Range / (owner.ProjectileSpeed * speedMultiplier);

        internal Vector3 direction;
        internal Rigidbody rb;

        private Tween movementTween;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();

            InitProjectile();
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
                    entity.Hurt(owner, owner.Damage, DamageType.Projectile);
                    //Destroy the projectile if has no piercing flag
                    if (!projectileFlags.HasFlag(ProjectileFlags.PiercesEnemy))
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }

        public void InitProjectile()
        {
        }

        public void FireStandaloneProjectile(Creature owner, Creature target)
        {
            // TODO: Redo this using location as target.

            OnFire();
            //TODO: Implement fire logic
            if (target != null)
            {   
                // Send projectile towards the target
                Vector3 towards = target.transform.position - transform.position;
                movementTween = transform.DOMove(owner.transform.position + towards.normalized * owner.Range, Uptime / Const.Values.PROJECTILE_SPEED)
                    .SetEase(Ease.Linear)
                    .OnComplete(() => Destroy(gameObject));
            }
            else
            {
                movementTween = transform.DOMove(owner.transform.position + owner.transform.forward * owner.Range, Uptime / Const.Values.PROJECTILE_SPEED)
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
                if(owner)
                {
                    movementTween = transform.DOMove(owner.transform.position + towards.normalized * owner.Range, Uptime / Const.Values.PROJECTILE_SPEED)
                        .SetEase(Ease.Linear)
                        .OnComplete(() => Destroy(gameObject));
                }
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

        internal void KillProjectile()
        {
            Instantiate(deathParticle, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}