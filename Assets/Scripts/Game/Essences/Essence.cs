using Dogabeey;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Dogabeey
{
    [CreateAssetMenu(fileName = "New Essence", menuName = "Scriptable Objects/New Essence...")]
    public class Essence : SerializedScriptableObject
    {
        public string essenceName;
        public string essenceDescription;
        public Sprite essenceIcon;
        public List<PlayerAction> onAcquired;
        public UnityEvent onTick;

        public void OnEssenceAcquired(Player creature)
        {
            for (int i = 0; i < onAcquired.Count; i++)
            {
                onAcquired[i].Invoke(creature);
            }
        }
        public void OnEssenceTick()
        {

        }

        public void AcquireByCreature(Player creature)
        {
            OnEssenceAcquired(creature);
        }
    }
}