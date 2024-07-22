
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
    public abstract class CreatureInteraction : BaseAction
    {
        public abstract void Invoke(Creature source, Creature target);
    }

}