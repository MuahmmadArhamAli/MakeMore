using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    // Button for trading
    [SerializeField] private Button TradeButton;

    //integer to see how many objects have been made
    private int TotalObjectsMade;

    private void Awake() {
        //set instance
        Instance = this;

        TradeButton.onClick.AddListener(() => {
            OnTrade?.Invoke(this, EventArgs.Empty);
            OnTradeForMoney?.Invoke(this,new OnTradeForMoneyEventArgs { money = TotalObjectsMade * CurrentObject.Value});

            TotalObjectsMade = 0;
        });

    }

    private void Start() {
        TotalObjectsMade = 0;

        ObjectMakeStation.OnObjectMade += ObjectMakeStation_OnObjectMade;

    }

    private void ObjectMakeStation_OnObjectMade(object sender, System.EventArgs e) {
        TotalObjectsMade++;

    }


    private void OnDestroy() {
        ObjectMakeStation.OnObjectMade -= ObjectMakeStation_OnObjectMade;
    }
}
