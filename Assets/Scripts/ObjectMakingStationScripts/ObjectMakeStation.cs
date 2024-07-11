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

    //variable forsetting the ObjectMakeSO
    [SerializeField] private ModelChangeResultSO ObjectMakeSO;

    //refernce of upgrade
    [SerializeField] private MakeStationUpgrade makeStationUpgrade;

    //bool to see if an output model has already been spawned
    private bool isOutputModelSpawned;

    //timer to set the total objects made
    private float ObjectMakeTimer;

    //int to see how many things it can hold
    private int MaxObjectHold;
    private int ObjectHoldCurrently;

    //Timer increase multiplier
    private float TimerIncreaseMultiplier;

    private void Start() {
        
        isOutputModelSpawned = false;
        ObjectMakeTimer = 0f;
        MaxObjectHold = 4;
        ObjectHoldCurrently = 0;
        TimerIncreaseMultiplier = 1f;

        SpawnInputModel();

        Player.Instance.OnNonInteractableObjectClick += Player_OnNonInteractableObjectClick;
        
        TradeStation.Instance.OnTrade += TradeStation_OnTrade;

        makeStationUpgrade.OnUpgraded += MakeStationUpgrade_OnUpgraded;
    }

    private void MakeStationUpgrade_OnUpgraded(object sender, EventArgs e) {
        TimerIncreaseMultiplier += .2f;
    }

    private void TradeStation_OnTrade(object sender, EventArgs e) {
        DestroyOutputModels();
    }


    private void Player_OnNonInteractableObjectClick(object sender, EventArgs e) {
        ObjectMakeTimer += 0.5f;
    }

    private void Update() {
        if (ObjectHoldCurrently < MaxObjectHold && ObjectMakeSO != null) {
            //if it can hold more objects
            
            ObjectMakeTimer += Time.deltaTime * TimerIncreaseMultiplier;

            if (ObjectMakeTimer > ObjectMakeSO.TimeToChange) {
                OnObjectMade?.Invoke(this, EventArgs.Empty);

                ObjectHoldCurrently++;


                if (!isOutputModelSpawned) {
                    //there is not a model spawned yet

                    //spawn model
                    Instantiate(ObjectMakeSO.OutputModel, OutputModelSpawnPosition);
                    isOutputModelSpawned = true;
                }

                ObjectMakeTimer = 0f;
            }
        }
    }


    private void DestroyOutputModels() {
        //check if there are spwaned objects at the table
        if (ObjectHoldCurrently > 0) {
            //there are some output models on the making station

            ObjectHoldCurrently = 0;

            //Destroy all the children
            foreach (Transform child in OutputModelSpawnPosition.transform) {
                Destroy(child.gameObject);
            }
        }

        isOutputModelSpawned = false;
    }
    private void SpawnInputModel() {
        //check if there are models on inputPosition and if so destroy all of them
        if (InputModelSpawnPosition.childCount > 0) {
            foreach (Transform child in InputModelSpawnPosition.transform) {
                Destroy(child.gameObject);
            }
        }

        //spawn the new model 
        Instantiate(ObjectMakeSO.InputModel, InputModelSpawnPosition);
    }

    private void OnDestroy() {
        Player.Instance.OnNonInteractableObjectClick -= Player_OnNonInteractableObjectClick;
        TradeStation.Instance.OnTrade -= TradeStation_OnTrade;
    }

}
