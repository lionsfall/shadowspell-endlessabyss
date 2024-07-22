
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dogabeey
{
    [System.Serializable]
    public class DamageSelfAction : CreatureAction
    {
        public int damageAmount = 0;

        public override void Invoke(Creature entity)
        {
            if (!active) return;

            entity.Hurt(entity, damageAmount);
        }
    }
    [System.Serializable]
    public class HealSelfAction : CreatureAction
    {
        public int healAmount = 0;

        public override void Invoke(Creature entity)
        {
            if (!active) return;

            entity.Heal(healAmount);
        }
    }
}
