using Dogabeey;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Dogabeey
{
    [CreateAssetMenu(fileName = "New Essence", menuName = "Scriptable Objects/New Essence...")]
    public class Essence : SerializedScriptableObject
    {
        [System.Serializable]
        public class EssenceDrop
        {
            public Essence essence;
            public int dropWeight;
        }

        public int essenceID;
        [Tooltip("If true, the essence only execute on acquire effect and won't be added to player's essence list. Used for basic essences like heart pickups.")]
        public bool executeAcquireEffectOnly;
        public string essenceName;
        public string essenceDescription;
        public Sprite essenceIcon;
        public MeshRenderer essenceMesh;
        [Space]
        public List<PlayerAction> onAcquired;
        public List<PlayerAction> onTick;


        public void OnEssenceAcquired(Player creature)
        {
            onAcquired.ForEach(action => action?.Invoke(creature));
        }
        public void OnEssenceTick(Player creature)
        {
            onTick.ForEach(action => action?.Invoke(creature));
        }

        internal void DropEssence(EssenceController controller, Vector3 position)
        {
            EssenceController essenceInstance = Instantiate(controller, position, Quaternion.identity);
            essenceInstance.Essence = this;
            essenceInstance.transform.parent = LevelScene.Instance.transform;
        }
    }
}