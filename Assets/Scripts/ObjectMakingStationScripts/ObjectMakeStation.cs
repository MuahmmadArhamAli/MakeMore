using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ObjectMakeStation : MonoBehaviour{
    //static event for sending TradeStation to increase Total Numbers
    public static event EventHandler OnObjectMade;

    //reference for Instantiating positions
    [SerializeField] private Transform InputModelSpawnPosition;
    [SerializeField] private Transform OutputModelSpawnPosition;

    //bool to see if an output model has already been spawned
    private bool isOutputModelSpawned;

    //variable forsetting the ObjectMakeSO
    private ModelChangeResultSO ObjectMakeSO;

    //timer to set the total objects made
    private float ObjectMakeTimer;

    private void Start() {
        
        isOutputModelSpawned = false;
        ObjectMakeTimer = 0f;

        Player.Instance.OnNonInteractableObjectClick += Player_OnNonInteractableObjectClick;
        TradeStation.Instance.OnObjectMakeChange += TradeStation_OnObjectMakeChange;
        TradeStation.Instance.OnTrade += TradeStation_OnTrade;

    }

    private void TradeStation_OnTrade(object sender, EventArgs e) {
        DestroyOutputModels();
    }

    private void TradeStation_OnObjectMakeChange(object sender, TradeStation.OnObjectMakeChangeEventArgs e) {
        ObjectMakeSO = e.newInput;

        DestroyOutputModels();

        //check if there are models on inputPosition and if so destroy all of them
        if(InputModelSpawnPosition.childCount > 0) {
            foreach(Transform child in InputModelSpawnPosition.transform) {
                Destroy(child.gameObject);
            }
        }

        //spawn the new model 
        Instantiate(ObjectMakeSO.InputModel,InputModelSpawnPosition);
        
        //reset timer
        ObjectMakeTimer = 0f;
    }

    private void Player_OnNonInteractableObjectClick(object sender, EventArgs e) {
        ObjectMakeTimer += 0.5f;
    }

    private void Update() {
        ObjectMakeTimer += Time.deltaTime;

        if(ObjectMakeTimer > ObjectMakeSO.TimeToChange) {
            OnObjectMade?.Invoke(this, EventArgs.Empty);

            if (!isOutputModelSpawned) {
                //there is not a model spawned yet

                //spawn model
                Instantiate(ObjectMakeSO.OutputModel,OutputModelSpawnPosition);
                isOutputModelSpawned = true;
            }

            ObjectMakeTimer = 0f;
        }
    }


    private void DestroyOutputModels() {
        //check if there are spwaned objects at the table
        if (OutputModelSpawnPosition.childCount > 0) {
            //there are some output models on the making station

            //Destroy all the children
            foreach (Transform child in OutputModelSpawnPosition.transform) {
                Destroy(child.gameObject);
            }
        }

        isOutputModelSpawned = false;
    }

    private void OnDestroy() {
        Player.Instance.OnNonInteractableObjectClick -= Player_OnNonInteractableObjectClick;
        TradeStation.Instance.OnObjectMakeChange -= TradeStation_OnObjectMakeChange;
        TradeStation.Instance.OnTrade -= TradeStation_OnTrade;
    }

}
