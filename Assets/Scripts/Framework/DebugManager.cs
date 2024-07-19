using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dogabeey
{
    public class DebugManager : MonoBehaviour
    {
        public GameObject coinSource;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                EventManager.TriggerEvent(Const.GameEvents.LEVEL_COMPLETED);
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                EventManager.TriggerEvent(Const.GameEvents.LEVEL_FAILED);
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                PlayerPrefs.DeleteAll();
            }
            if(Input.GetKeyDown(KeyCode.L))
            {
                WorldManager.Instance.ResetCurrentLevel();
            }
            if(Input.GetKeyDown(KeyCode.A))
            {
                if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    UnityAdsManager.Instance.ShowRewardedAd();
                }
                else
                {
                    UnityAdsManager.Instance.ShowAd();
                }
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                if (coinSource != null)
                    CurrencyManager.Instance.AddCoin(100, coinSource);
                else
                    CurrencyManager.Instance.AddCoin(100);
            }
        }
    }
}
