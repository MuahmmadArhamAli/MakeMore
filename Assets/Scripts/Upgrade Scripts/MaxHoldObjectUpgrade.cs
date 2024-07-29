using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHoldObjectUpgrade : MonoBehaviour{
    //Singleton
    public static MaxHoldObjectUpgrade Instance {  get; private set; }

    private float Cost;

    private int MaxHoldObjects;

    public class OnMaxHoldUpgradeEventArgs:EventArgs { public int HoldObjects; public float cost; }
    public event EventHandler<OnMaxHoldUpgradeEventArgs> OnMaxHoldUpgrade;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        Cost = 300f;
        MaxHoldObjects = 4;

        MainCanvasUI.Instance.OnMaxHoldLimitUpgradeClick += MainCanvas_OnMaxHoldLimitUpgradeClick;
    }

    private void MainCanvas_OnMaxHoldLimitUpgradeClick(object sender, System.EventArgs e) {
        if(NormalMoney.Instance.GetMoney() >= Cost) {

            MaxHoldObjects += 2;

            NormalMoney.Instance.DecreaseMoney(Cost);
            
            Cost += 100;
            
            OnMaxHoldUpgrade?.Invoke(this, new OnMaxHoldUpgradeEventArgs { HoldObjects = MaxHoldObjects, cost = Cost + 100 });
        }
    }

    private void OnDestroy() {
        MainCanvasUI.Instance.OnMaxHoldLimitUpgradeClick -= MainCanvas_OnMaxHoldLimitUpgradeClick;
    }

    public float GetCost() {
        return Cost;
    }

    public int GetMaxObjectHold() {
        return MaxHoldObjects;
    }
}
