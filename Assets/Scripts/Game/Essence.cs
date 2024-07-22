using UnityEngine;
using UnityEngine.Events;

public abstract class Essence : ScriptableObject
{
    public string essenceName;

    public UnityEvent onEssenceCollected;
    public UnityEvent onEssenceTick;
}
