using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TradeStation : MonoBehaviour{
    //singleton pattern
    public static TradeStation Instance {  get; private set; }

    //reference for the box collider for not allowing to interact while in upgrade screen

    private BoxCollider2D hitbox;

    //reference for the ObjectMakeSO
    [SerializeField] private ModelChangeResultSO CurrentObject;

    //event to say to making station to delete all output models
    public event EventHandler OnTrade;

    //event for sending the money script to increase or decerease money
    public class OnTradeForMoneyEventArgs : EventArgs { public float money; }
    public event EventHandler<OnTradeForMoneyEventArgs> OnTradeForMoney;
    
    //integer to see how many objects have been made
    private int TotalObjectsMade;

    private void Awake() {
        //set instance
        Instance = this;

        hitbox = GetComponent<BoxCollider2D>();
    }

    private void Start() {
        TotalObjectsMade = 0;

        ObjectMakeStation.OnObjectMade += ObjectMakeStation_OnObjectMade;
        Player.Instance.OnTradeStationClick += Player_OnTradeStationClick;
        MainCanvasUI.Instance.OnGoToUpgradeClick += MianCanvasUI_OnGoToUpgradeClick;
        MainCanvasUI.Instance.OnGoToNormalGameplayClick += MainCanvasUI_OnGoToNormalGameplayClick;
    }

    private void MainCanvasUI_OnGoToNormalGameplayClick(object sender, EventArgs e) {
        ShowHitbox();
    }

    private void MianCanvasUI_OnGoToUpgradeClick(object sender, EventArgs e) {
        HideHitbox();
    }

    private void Player_OnTradeStationClick(object sender, EventArgs e) {
        OnTrade?.Invoke(this, EventArgs.Empty);
        OnTradeForMoney?.Invoke(this, new OnTradeForMoneyEventArgs { money = TotalObjectsMade * CurrentObject.Value });

        TotalObjectsMade = 0;
    }

    private void ObjectMakeStation_OnObjectMade(object sender, System.EventArgs e) {
        TotalObjectsMade++;

    }


    private void OnDestroy() {
        ObjectMakeStation.OnObjectMade -= ObjectMakeStation_OnObjectMade;
        Player.Instance.OnTradeStationClick -= Player_OnTradeStationClick;
        MainCanvasUI.Instance.OnGoToUpgradeClick -= MianCanvasUI_OnGoToUpgradeClick;
        MainCanvasUI.Instance.OnGoToNormalGameplayClick -= MainCanvasUI_OnGoToNormalGameplayClick;
    }

    public ModelChangeResultSO GetCurrentWorkinObject() {
        return CurrentObject;
    }

    private void HideHitbox() {
        hitbox.gameObject.SetActive(false);
    }
    private void ShowHitbox() {
        hitbox.gameObject.SetActive(true);
    }
}
