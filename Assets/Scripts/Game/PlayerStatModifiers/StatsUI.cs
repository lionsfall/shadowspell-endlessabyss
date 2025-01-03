using UnityEngine;
using TMPro; 

namespace Dogabeey
{
    public class StatsUI : UIElement
    {
        internal Player playerRef;

        public TMP_Text speedStat;
        public TMP_Text damageStat;
        public TMP_Text attackRateStat;
        public TMP_Text projectileSpeedStat;

        public override string FireEvent => "STAT_CHANGED";

        private void Start()
        {
            playerRef = Player.Instance ;
        }

        private void Update()
        {
            if(playerRef == null)
            {
                if(Player.Instance != null)
                {
                    playerRef = Player.Instance;
                }
                else
                {
                    return;
                }
            }

            DrawUI();
        }

        public override void DrawUI()
        {
            if (speedStat) speedStat.text = playerRef.Speed.ToString();
            if (damageStat) damageStat.text = playerRef.Damage.ToString();
            if (attackRateStat) attackRateStat.text = playerRef.AttackRate.ToString();
            if (projectileSpeedStat) projectileSpeedStat.text = playerRef.ProjectileSpeed.ToString();
        }
    }
}

