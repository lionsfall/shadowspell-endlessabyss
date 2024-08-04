using System;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Dogabeey
{ 
    public class EssenceController : MonoBehaviour
    {
        [ReadOnly] public Essence essence;
        public Transform meshRendererParent;

        internal MeshRenderer currentRenderer;

        public Essence Essence
        {
            get
            {
                return essence;
            }
            set
            {
                essence = value;
                OnEssenceChanged();
            }
        }
        private void OnEssenceChanged()
        {
            // Set essence's renderer
            if (essence != null)
            {
                if(currentRenderer != null)
                {
                    Destroy(currentRenderer);
                }
                currentRenderer = Instantiate(essence.essenceMesh, meshRendererParent);
            }
        }
        

        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent(out Player player))
            {
                player.AcquireEssence(essence);
                Destroy(gameObject);
            }
        }
    }
}
