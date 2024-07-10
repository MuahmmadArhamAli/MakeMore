using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour{
    //reference for the camera
    [SerializeField] private Camera lookCamera;

    //event for trading all objects
    public event EventHandler OnTradeStationClick;

    //event for all Making Stations to reduce Time
    public event EventHandler OnNonInteractableObjectClick;

    public static Player Instance { get; private set; }

    private void Awake() {
        Instance = this;    
    }

    private void Start() {
        InputManager.Instance.OnClickOrTouch += InputManager_OnClickOrTouch;
    }

    private void InputManager_OnClickOrTouch(object sender, System.EventArgs e) {
        //see what did the player click
        if (Physics.Raycast(lookCamera.ScreenPointToRay(InputManager.Instance.GetTapPosition()), out RaycastHit hitObject)) {
            if (hitObject.transform.TryGetComponent(out TradeStation tradeStation)) {
                OnTradeStationClick?.Invoke(this, EventArgs.Empty); 
            }
            else {
                OnNonInteractableObjectClick?.Invoke(this, EventArgs.Empty);
            }
        }
        else {
            OnNonInteractableObjectClick?.Invoke(this, EventArgs.Empty);
        }
    }
}
