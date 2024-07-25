
using System.Collections;
using System.Collections.Generic;

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
    [System.Serializable]
    public abstract class CreatureInteraction : BaseAction
    {
        public abstract void Invoke(Creature source, Creature target);
    }

}