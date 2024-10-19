using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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

        private IEnumerator Start()
        {
            yield return new WaitUntil(() => Player.Instance != null);
            player = Player.Instance;

            InitUI();
            DrawUI();
        }
        private void OnEnable()
        {
            EventManager.StartListening(Const.GameEvents.PLAYER_HEALTH_CHANGED, OnPlayerHeartChanged);
        }
        private void OnDisable()
        {
            EventManager.StopListening(Const.GameEvents.PLAYER_HEALTH_CHANGED, OnPlayerHeartChanged);
        }
        private void OnPlayerHeartChanged(EventParam e)
        {
            int oldHealth = hearts.Count;
            int newHealth = (int) player.CurrentHealth;
            int difference = newHealth - oldHealth;
            if (difference < 0)
            {
                for (int i = hearts.Count - 1; i >= hearts.Count - difference; i--)
                {
                    DestroyEffect(hearts[i]);
                }
            }
            Invoke(nameof(DrawUI), 0.1f);
        }

        private void DestroyEffect(Image heart)
        {
            hearts.Remove(heart);
            heart.transform.DOScale(2, 0.5f).OnComplete(() => Destroy(heart.gameObject));
            heart.DOFade(0, 0.5f);
        }

        private void Update()
        {
            //DrawUI();
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