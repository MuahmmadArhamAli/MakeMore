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

    [SerializeField] private GameObject NormalObject;


    //variable forsetting the ObjectMakeSO
    private ModelChangeResultSO ObjectMakeSO;

    //refernce of upgrade
    [SerializeField] private MakeStationUpgrade makeStationUpgrade;

    //reference of the outputmodel gameobject
    private GameObject outputModel;
    
    //bool to see if an output model has already been spawned
    private bool isOutputModelSpawned;

    //timer to set the total objects made
    private float ObjectMakeTimer;

    //int to see how many things it can hold
    private static int MaxObjectHold;
    private int ObjectHoldCurrently;

    //Timer increase multiplier
    private float TimerIncreaseMultiplier;
    private static float OnClickTimerInceaseMultiplier;

    private void Start() {
        
        isOutputModelSpawned = false;
        ObjectMakeTimer = 0f;
        MaxObjectHold = 4;
        ObjectHoldCurrently = 0;
        TimerIncreaseMultiplier = 1f;
        OnClickTimerInceaseMultiplier = 1f;

        Player.Instance.OnNonInteractableObjectClick += Player_OnNonInteractableObjectClick;
        
        TradeStation.Instance.OnTrade += TradeStation_OnTrade;

        makeStationUpgrade.OnUpgraded += MakeStationUpgrade_OnUpgraded;

        MainCanvasUI.Instance.OnGoToUpgradeClick += MainCanvasUI_OnGoToUpgradeClick;
        MainCanvasUI.Instance.OnGoToNormalGameplayClick += MainCanvasUI_OnGoToNormalGameplayClick;

        ObjectMakeSO = TradeStation.Instance.GetCurrentWorkinObject();

        SpawnNewInputModel();
    }

    private void MainCanvasUI_OnGoToNormalGameplayClick(object sender, EventArgs e) {
        NormalObject.SetActive(true);
    }

    private void MainCanvasUI_OnGoToUpgradeClick(object sender, EventArgs e) {
        NormalObject.SetActive(false);
    }

    private void MakeStationUpgrade_OnUpgraded(object sender, EventArgs e) {
        TimerIncreaseMultiplier += .2f;
    }

    private void TradeStation_OnTrade(object sender, EventArgs e) {
        DestroyOutputModels();
    }


    private void Player_OnNonInteractableObjectClick(object sender, EventArgs e) {
        
        ObjectMakeTimer += 0.2f * OnClickTimerInceaseMultiplier;
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
                    outputModel.SetActive(true);

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

            //set active to false
            outputModel.SetActive(false);
        }

        isOutputModelSpawned = false;
    }
    private void SpawnNewInputModel() {
        //check if there are models on inputPosition and if so destroy all of them
        foreach (Transform child in InputModelSpawnPosition.transform) {
            Destroy(child.gameObject);
        }
        foreach (Transform child in OutputModelSpawnPosition.transform) {
            Destroy(child.gameObject);
        }

        outputModel = null;

        //spawn the new model 
        Instantiate(ObjectMakeSO.InputModel, InputModelSpawnPosition);
        outputModel = Instantiate(ObjectMakeSO.OutputModel,OutputModelSpawnPosition);

        outputModel.SetActive(false);
    }

    private void OnDestroy() {
        Player.Instance.OnNonInteractableObjectClick -= Player_OnNonInteractableObjectClick;
        TradeStation.Instance.OnTrade -= TradeStation_OnTrade;
    }

    public void SetCurrentWorkingObject() {
        ObjectMakeSO = TradeStation.Instance.GetCurrentWorkinObject();

        SpawnNewInputModel();
    }

    public void HideNormalObject() {
        NormalObject.SetActive(false);
    }

}
