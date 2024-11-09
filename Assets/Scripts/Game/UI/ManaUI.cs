using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dogabeey
{
    public class ManaUI : UIElement
    {
        public int maxPossibleMana;
        public Transform totalManaTransform, currentManaTransform;
        public Image manaContainerPrefab, manaPrefab;

        internal Player player;
        
        private List<Image> manas = new List<Image>();
        private List<Image> manaContainers = new List<Image>();

        public override string FireEvent => Const.GameEvents.PLAYER_MANA_CHANGED;

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
            if (player == null)
                return;

            DrawUI();
        }
        public void InitUI()
        {
            for (int i = 0; i < maxPossibleMana; i++)
            {
                Image manaContainer = Instantiate(manaContainerPrefab, totalManaTransform);
                Image mana = Instantiate(manaPrefab, currentManaTransform.transform);

                manaContainer.gameObject.SetActive(false);
                mana.gameObject.SetActive(false);

                manaContainers.Add(manaContainer);
                manas.Add(mana);
            }
        
        }
        public override void DrawUI()
        {
            if(!player)
                return;

            manaContainers.ForEach(x => x.gameObject.SetActive(false));
            manas.ForEach(x => x.gameObject.SetActive(false));

            for (int i = 0; i < (int) player.MaxMana; i++)
            {
                manaContainers[i].gameObject.SetActive(true);
            }
            for (int i = 0; i < (int) player.CurrentMana; i++)
            {
                manas[i].gameObject.SetActive(true);
            }
        }
    }
}