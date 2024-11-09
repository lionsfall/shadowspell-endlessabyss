using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dogabeey
{
    public class HealthUI : UIElement
    {
        public int maxPossibleHealth;
        public Transform totalHeartTransform, currentHealthTransform;
        public Image heartContainerPrefab, heartPrefab;

        internal Player player;
        
        private List<Image> hearts = new List<Image>();
        private List<Image> heartContainers = new List<Image>();

        public override string FireEvent => Const.GameEvents.PLAYER_HEALTH_CHANGED;

        protected override void OnEnable()
        {
            base.OnEnable();
            EventManager.StartListening(Const.GameEvents.LEVEL_STARTED, OnLevelStarted);
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            EventManager.StopListening(Const.GameEvents.LEVEL_STARTED, OnLevelStarted);
        }
        public void OnLevelStarted(EventParam e)
        {
            DOVirtual.DelayedCall(0.1f, () => player = Player.Instance);
        }

        private IEnumerator Start()
        {
            yield return new WaitUntil(() => Player.Instance != null);
            player = Player.Instance;

            InitUI();
        }
        private void Update()
        {
            if(player == null)
                return;

            DrawUI();
        }
        public void InitUI()
        {
            for (int i = 0; i < maxPossibleHealth; i++)
            {
                Image heartContainer = Instantiate(heartContainerPrefab, totalHeartTransform);
                Image heart = Instantiate(heartPrefab, currentHealthTransform.transform);

                heartContainer.gameObject.SetActive(false);
                heart.gameObject.SetActive(false);

                heartContainers.Add(heartContainer);
                hearts.Add(heart);
            }
        
        }
        public override void DrawUI()
        {
            if(player == null)
                return;

            heartContainers.ForEach(x => x.gameObject.SetActive(false));
            hearts.ForEach(x => x.gameObject.SetActive(false));

            for (int i = 0; i < (int) player.MaxHealth; i++)
            {
                heartContainers[i].gameObject.SetActive(true);
            }
            for (int i = 0; i < (int) player.CurrentHealth; i++)
            {
                hearts[i].gameObject.SetActive(true);
            }
        }
    }
}