using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour{
    //reference for the camera
    [SerializeField] private Camera lookCamera;

    //event for all Making Stations to reduce Time
    public event EventHandler OnNonInteractableObjectClick;

    //event to send the trade station 
    public event EventHandler OnTradeStationClick;

    public static Player Instance { get; private set; }

    private void Awake() {
        Instance = this;    
    }

    private void Start() {
        InputManager.Instance.OnClickOrTouch += InputManager_OnClickOrTouch;
    }

    private void InputManager_OnClickOrTouch(object sender, System.EventArgs e) {
        //get the raycasthit
        var rayCastHit = Physics2D.GetRayIntersection(lookCamera.ScreenPointToRay(InputManager.Instance.GetTapPosition()));

        if (rayCastHit) {
            //player hit something
            if (rayCastHit.transform.TryGetComponent(out TradeStation tradeStation)) {
                //the player hit the trade station

                OnTradeStationClick?.Invoke(this,EventArgs.Empty);
            }
        }
        else {
            OnNonInteractableObjectClick?.Invoke(this, EventArgs.Empty);
        }
    }

    private void OnDestroy() {
        InputManager.Instance.OnClickOrTouch -= InputManager_OnClickOrTouch;
    }
}
