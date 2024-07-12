using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour{
    //reference for the camera
    [SerializeField] private Camera lookCamera;

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
        OnNonInteractableObjectClick?.Invoke(this, EventArgs.Empty);
    }

    private void OnDestroy() {
        InputManager.Instance.OnClickOrTouch -= InputManager_OnClickOrTouch;
    }
}
