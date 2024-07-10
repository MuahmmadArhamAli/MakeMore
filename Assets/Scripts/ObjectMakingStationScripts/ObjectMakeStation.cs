using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ObjectMakeStation : MonoBehaviour{
    //static event for sending TradeStation to increase Total Numbers
    public static event EventHandler OnObjectMade;


    [SerializeField] private ModelChangeResultSO ObjectMakeSO;

    //timer to set the total objects made
    private float ObjectMakeTimer;

    private void Start() {
        ObjectMakeTimer = 0f;

        Player.Instance.OnNonInteractableObjectClick += Player_OnNonInteractableObjectClick;
    }

    private void Player_OnNonInteractableObjectClick(object sender, EventArgs e) {
        ObjectMakeTimer += 0.5f;
    }

    private void Update() {
        ObjectMakeTimer += Time.deltaTime;

        if(ObjectMakeTimer > ObjectMakeSO.TimeToChange) {
            OnObjectMade?.Invoke(this, EventArgs.Empty);
            ObjectMakeTimer = 0f;
        }
    }


}
