using System;
using System.Linq;
using UnityEngine;

namespace Dogabeey
{ 
    public class EssenceController : MonoBehaviour
    {
        private Essence essence;

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
    }
}
