using UnityEngine;
using UnityEngine.UI;


namespace Dogabeey
{
    public class CreatureHealthBar : MonoBehaviour
    {
        public RectTransform healthBarRef;
        public Creature referenceCreature;
        public Vector2 healthBarPositionOffset;
        public float scaleOffset = 1;
        public Image healthImage;
        public string canvasTag;

        private Canvas canvas;

        private void OnEnable()
        {
            EventManager.StartListening(Const.GameEvents.CREATURE_DEATH, OnCreatureDied);
        }
        private void OnDisable()
        {
            EventManager.StopListening(Const.GameEvents.CREATURE_DEATH, OnCreatureDied);
        }
        private void OnCreatureDied(EventParam e)
        {
            if (e.paramObj == referenceCreature.gameObject)
            {
                Destroy(healthBarRef.gameObject);
            }
        }

        private void Start()
        {
            canvas = GameObject.FindGameObjectWithTag(canvasTag).GetComponent<Canvas>();

            if(canvas)
            {
                healthBarRef.transform.parent = canvas.transform;
                healthBarRef.transform.localScale *= scaleOffset;
            }
            else
            {
                Debug.LogError("Canvas not found with tag: " + canvasTag); 
            }
        }

        private void Update()
        {
            if (!referenceCreature)
                return;

            healthBarRef.transform.position = Camera.main.WorldToScreenPoint(referenceCreature.transform.position) + (Vector3)healthBarPositionOffset;

            healthImage.fillAmount = referenceCreature.CurrentHealth / referenceCreature.MaxHealth;
        }
    }
}
