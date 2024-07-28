using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Dogabeey
{
    public class ActionOnClick : ActionBarItem
    {
        [Header("Action Settings")]
        public PlayerAction action;

        public override float CostMultiplier => 1.0f;

        public override bool IsClickable()
        {
            return CurrencyManager.Instance.GetCurrencyAmount(costCurrency) >= GetCost();
        }

        public override bool IsVisible()
        {
            return true;
        }

        public override void OnClick()
        {
            if (IsClickable())
            {
                CurrencyManager.Instance.AddCurrency(costCurrency.currencyID, -GetCost());
                action.Invoke(Player.Instance);
            }
        }
    }
}