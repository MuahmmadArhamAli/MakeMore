using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeStation : MonoBehaviour{
    //singleton pattern
    public static TradeStation Instance {  get; private set; }

    //reference for the ObjectMakeSO
    [SerializeField] private ModelChangeResultSO CurrentObject;

    //event to send the woekers the object that is being made
    public class OnObjectMakeChangeEventArgs : EventArgs { public ModelChangeResultSO newInput; }
    public event EventHandler<OnObjectMakeChangeEventArgs> OnObjectMakeChange;

    //event to say to making station to delete all output models
    public event EventHandler OnTrade;

    //integer to see how many objects have been made
    private int TotalObjectsMade;
    private int Money;

    private void Awake() {
        //set instance
        Instance = this;
    }

    private void Start() {
        TotalObjectsMade = 0;
        Money = 0;

        Player.Instance.OnTradeStationClick += Player_OnTradeStationClick;
        ObjectMakeStation.OnObjectMade += ObjectMakeStation_OnObjectMade;

        //send ObjectChange Event
        OnObjectMakeChange?.Invoke(this,new OnObjectMakeChangeEventArgs { newInput = CurrentObject });
    }

    private void ObjectMakeStation_OnObjectMade(object sender, System.EventArgs e) {
        TotalObjectsMade++;
    }

    private void Player_OnTradeStationClick(object sender, System.EventArgs e) {
        Money += (TotalObjectsMade * CurrentObject.Value);

        OnTrade?.Invoke(this, EventArgs.Empty);

        TotalObjectsMade = 0;
    }

    private void OnDestroy() {
        Player.Instance.OnTradeStationClick -= Player_OnTradeStationClick;
        ObjectMakeStation.OnObjectMade -= ObjectMakeStation_OnObjectMade;

    }
}
