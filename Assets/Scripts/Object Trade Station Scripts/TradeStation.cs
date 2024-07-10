using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeStation : MonoBehaviour{
    //reference for the ObjectMakeSO
    [SerializeField] private ModelChangeResultSO CurrentObject;

    //integer to see how many objects have been made
    private int TotalObjectsMade;
    private int Money;

    private void Start() {
        TotalObjectsMade = 0;
        Money = 0;

        Player.Instance.OnTradeStationClick += Player_OnTradeStationClick;
        ObjectMakeStation.OnObjectMade += ObjectMakeStation_OnObjectMade;
    }

    private void ObjectMakeStation_OnObjectMade(object sender, System.EventArgs e) {
        Debug.Log(TotalObjectsMade);

        TotalObjectsMade++;
    }

    private void Player_OnTradeStationClick(object sender, System.EventArgs e) {
        Money += (TotalObjectsMade * CurrentObject.Value);

        TotalObjectsMade = 0;
    }

    private void OnDestroy() {
        
    }
}
