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

    //Vector 2 for storing the original hitbox size
    private Vector2 hitboxSize;

    //reference for the ObjectMakeSO
    [SerializeField] private ModelChangeResultSO CurrentObject;

    //event to say to making station to delete all output models
    public event EventHandler OnTrade;

    //event for sending the money script to increase or decerease money
    public class OnTradeForMoneyEventArgs : EventArgs { public float money; }
    public event EventHandler<OnTradeForMoneyEventArgs> OnTradeForMoney;
    
    //integer to see how many objects have been made
    private int TotalObjectsMade;

    //cash multiplier
    private float cashMultiplier;

    private void Awake() {
        //set instance
        Instance = this;

        hitbox = GetComponent<BoxCollider2D>();
    }

    private void Start() {
        TotalObjectsMade = 0;

        cashMultiplier = UpgradeWithCashUI.Instance.GetCashMultiplier();

        hitboxSize.x = hitbox.size.x;
        hitboxSize.y = hitbox.size.y;

        ObjectMakeStation.OnAnyObjectMade += ObjectMakeStation_OnObjectMade;
        Player.Instance.OnTradeStationClick += Player_OnTradeStationClick;
        MainCanvasUI.Instance.OnGoToUpgradeClick += MianCanvasUI_OnGoToUpgradeClick;
        MainCanvasUI.Instance.OnGoToNormalGameplayClick += MainCanvasUI_OnGoToNormalGameplayClick;
        MainCanvasUI.Instance.OnTasksButtonClick += MainCanvas_OnTasksButtonClick;
        MainCanvasUI.Instance.OnUpgradeCashButtonClick += MainCanvas_OnUpgradeCashButtonClick;

        UpgradeWithCashUI.Instance.OnCashMultiplierIncrease += UpgradeCashUI_OnCashMultiplierIncrease;
    }

    private void UpgradeCashUI_OnCashMultiplierIncrease(object sender, UpgradeWithCashUI.OnMultiplierIncreaseEventArgs e) {
        cashMultiplier += e.Multiplier;
    }

    private void MainCanvas_OnUpgradeCashButtonClick(object sender, EventArgs e) {
        Hide();
    }

    private void MainCanvas_OnTasksButtonClick(object sender, EventArgs e) {
        Hide();
    }

    private void MainCanvasUI_OnGoToNormalGameplayClick(object sender, EventArgs e) {
        ShowHitbox();
        Show();
    }

    private void MianCanvasUI_OnGoToUpgradeClick(object sender, EventArgs e) {
        HideHitbox();
    }

    private void Player_OnTradeStationClick(object sender, EventArgs e) {
        OnTrade?.Invoke(this, EventArgs.Empty);
        OnTradeForMoney?.Invoke(this, new OnTradeForMoneyEventArgs { money = (TotalObjectsMade * CurrentObject.Value) * cashMultiplier });

        TotalObjectsMade = 0;
    }

    private void ObjectMakeStation_OnObjectMade(object sender, System.EventArgs e) {
        TotalObjectsMade++;

    }


    private void OnDestroy() {
        ObjectMakeStation.OnAnyObjectMade -= ObjectMakeStation_OnObjectMade;
        Player.Instance.OnTradeStationClick -= Player_OnTradeStationClick;
        MainCanvasUI.Instance.OnGoToUpgradeClick -= MianCanvasUI_OnGoToUpgradeClick;
        MainCanvasUI.Instance.OnGoToNormalGameplayClick -= MainCanvasUI_OnGoToNormalGameplayClick;
        MainCanvasUI.Instance.OnTasksButtonClick -= MainCanvas_OnTasksButtonClick;
        MainCanvasUI.Instance.OnUpgradeCashButtonClick -= MainCanvas_OnUpgradeCashButtonClick;

        UpgradeWithCashUI.Instance.OnCashMultiplierIncrease -= UpgradeCashUI_OnCashMultiplierIncrease;
    }

    public ModelChangeResultSO GetCurrentWorkinObject() {
        return CurrentObject;
    }

    private void HideHitbox() {
        hitbox.size = new Vector2(0f,0f);
    }
    private void ShowHitbox() {
        hitbox.size = new Vector2(hitboxSize.x,hitboxSize.y);
    }

    private void Show() {
        gameObject.SetActive(true);
    }
    private void Hide() {
        gameObject.SetActive(false);
    }

    public float GetCashMultiplier() {
        return cashMultiplier;
    }
}
