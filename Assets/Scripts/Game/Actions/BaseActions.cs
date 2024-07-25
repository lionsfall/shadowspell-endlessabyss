
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;

namespace Dogabeey
{
    [System.Serializable]
    public abstract class BaseAction
    {
        public bool active = true;
    }
    [System.Serializable]
    public abstract class CreatureAction : BaseAction
    {
        public abstract void Invoke(Creature entity);
    }
    [System.Serializable]
    public abstract class PlayerAction : BaseAction
    {
        public abstract void Invoke(Player player);
    }
    public class HealPlayerAction : PlayerAction
    {
        public float amount;
        public override void Invoke(Player player)
        {
            player.Heal(amount);
        }
    }
    public class  FullHealthAction : PlayerAction
    {
        public override void Invoke(Player player)
        {
            player.Heal(player.MaxHealth);
        }
    }
    [System.Serializable]
    public abstract class CreatureInteraction : BaseAction
    {
        public abstract void Invoke(Creature source, Creature target);
    }

}