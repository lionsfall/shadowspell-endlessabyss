using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Dogabeey
{
    public class EssenceUI : UIElement
    {
        public List<Image> essenceImages;
        public List<TMP_Text> essenceLifeTexts;

        internal Player player;

        public override string FireEvent => Const.GameEvents.PLAYER_ESSENCES_CHANGED;

        private IEnumerator Start()
        {
            yield return new WaitUntil(() => Player.Instance != null);
            player = Player.Instance;

            DrawUI();
        }
        private void Update()
        {
            
        }
        public override void DrawUI()
        {
            for (int i = 0; i < essenceImages.Count; i++)
            {
                if (i < player.essences.Count)
                {
                    essenceImages[i].gameObject.SetActive(true);
                    essenceImages[i].sprite = player.essences[i].essence.essenceIcon;
                    essenceLifeTexts[i].text = player.essences[i].remainingLifeSpan.ToString();
                }
                else
                {
                    essenceImages[i].gameObject.SetActive(false);
                    essenceLifeTexts[i].text = "";
                }
            }
        }
    }
}