using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeStation : MonoBehaviour{
    //singleton pattern
    public static TradeStation Instance {  get; private set; }

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
    }

    private void Start() {
        TotalObjectsMade = 0;

        Player.Instance.OnTradeStationClick += Player_OnTradeStationClick;
        ObjectMakeStation.OnObjectMade += ObjectMakeStation_OnObjectMade;

    }

    private void ObjectMakeStation_OnObjectMade(object sender, System.EventArgs e) {
        TotalObjectsMade++;

    }

    private void Player_OnTradeStationClick(object sender, System.EventArgs e) {
        OnTradeForMoney?.Invoke(this, new OnTradeForMoneyEventArgs { money = TotalObjectsMade * CurrentObject.Value });        
        OnTrade?.Invoke(this, EventArgs.Empty);

        TotalObjectsMade = 0;
    }

    private void OnDestroy() {
        Player.Instance.OnTradeStationClick -= Player_OnTradeStationClick;
        ObjectMakeStation.OnObjectMade -= ObjectMakeStation_OnObjectMade;

    }
}
