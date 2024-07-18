using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHoldObjectUpgrade : MonoBehaviour
{
    //Singleton
    public static MaxHoldObjectUpgrade Instance {  get; private set; }

    private float Cost;

    public event EventHandler OnMaxHoldUpgrade;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        Cost = 2f;

        MainCanvasUI.Instance.OnMaxHoldLimitUpgradeClick += MainCanvas_OnMaxHoldLimitUpgradeClick;
    }

    private void MainCanvas_OnMaxHoldLimitUpgradeClick(object sender, System.EventArgs e) {
        if(CashMoney.Instance.GetMoney() >= Cost) {
            OnMaxHoldUpgrade?.Invoke(this,EventArgs.Empty);

            CashMoney.Instance.DecreaseMoney(Cost);
        }
    }
}
