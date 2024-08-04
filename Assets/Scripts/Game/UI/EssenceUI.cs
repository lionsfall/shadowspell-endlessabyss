using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dogabeey
{
    public class EssenceUI : UIElement
    {
        public List<Image> essenceImages;

        internal Player player;

        public override string FireEvent => Const.GameEvents.PLAYER_HEALTH_CHANGED;

        private IEnumerator Start()
        {
            yield return new WaitUntil(() => Player.Instance != null);
            player = Player.Instance;

            DrawUI();
        }
        private void Update()
        {
            if(Player.Instance)
                DrawUI();
        }
        public override void DrawUI()
        {
            for (int i = 0; i < essenceImages.Count; i++)
            {
                if (i < player.essences.Count)
                {
                    essenceImages[i].gameObject.SetActive(true);
                    essenceImages[i].sprite = player.essences[i].essence.essenceIcon;
                }
                else
                {
                    essenceImages[i].gameObject.SetActive(false);
                }
            }
        }
    }
}