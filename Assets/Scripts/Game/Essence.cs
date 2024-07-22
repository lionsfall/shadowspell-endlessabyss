using Dogabeey;
using UnityEngine;
using UnityEngine.Events;

public abstract class Essence : ScriptableObject
{
    public string essenceName;
    public string essenceDescription;
    public Sprite essenceIcon;
    public CreatureAction onAcquired;
    public UnityEvent onTick;

    public void OnEssenceAcquired()
    {
        onAcquired.Invoke(null);
    }
    public void OnEssenceTick()
    {

    }
}
