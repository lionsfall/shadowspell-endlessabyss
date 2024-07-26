using UnityEngine;

namespace Dogabeey
{
    [System.Serializable]
    public class RegenerationChancePerHit : ProjectileAction
    { 
        [Range(0, 1)]
        public float rate;
        public override void Invoke(Projectile projectile)
        {
            if (Random.value <= rate)
            {
                projectile.owner.Heal(1);
            }
        }
    }
}
