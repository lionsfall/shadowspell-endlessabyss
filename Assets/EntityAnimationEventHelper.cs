
using UnityEngine;

namespace Dogabeey
{
    public class EntityAnimationEventHelper : MonoBehaviour
    {
        public Player playerRef;

        public void AttackCurrentTarget()
        {
            playerRef.AttackCurrentTarget();
        }
    }
}

