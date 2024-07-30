using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ObjectMakeStation : MonoBehaviour,IHasProgress{
    //static event for sending TradeStation to increase Total Numbers
    public static event EventHandler OnAnyObjectMade;

    public static event EventHandler OnAnyUpgrade;

    //events to send to save data
    public class OnUpgradeEventArgs:EventArgs { public float cost; public float TimerMultiplier; }
    public event EventHandler<OnUpgradeEventArgs> OnUpgrade;

    public class OnNewObjectMadeEventArgs : EventArgs { public int Objects; };
    public event EventHandler<OnNewObjectMadeEventArgs> OnNewObjectMade;


    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    //reference for Instantiating positions
    [SerializeField] private Transform InputModelSpawnPosition;
    [SerializeField] private Transform OutputModelSpawnPosition;

    [SerializeField] private GameObject NormalObject;

    //text for showing how many objects there are
    [SerializeField] private TextMeshProUGUI ObjectsHoldText;
    //reference of Image to turn red when the MaxHolds
    [SerializeField] private Image fillImage;

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

    //float to see how much time is increased by
    private static float IncreaseTime;

    //int to see how many things it can hold
    private static int MaxObjectHold;
    private int ObjectHoldCurrently;

    //Timer increase multiplier
    private float TimerIncreaseMultiplier;
    private static float OnClickTimerInceaseMultiplier;

    private const float MaxSlowingDownTime = 10f;
    private const float MaxSleepingTime = 20f;

    private float SlowDownSleepTimer;

    private bool isSlowed;
    private bool isSleeping;

    private void Start() {
        fillImage.color = Color.green;

        MaxObjectHold = MaxHoldObjectUpgrade.Instance.GetMaxObjectHold();

        OnClickTimerInceaseMultiplier = UpgradeWithCashUI.Instance.GetClickMultiplier();
        IncreaseTime = UpgradeWithCashUI.Instance.GetTimeMultiplier();

        isOutputModelSpawned = false;
        ObjectMakeTimer = 0f;

        isSlowed = false;
        isSleeping = false;

        SlowDownSleepTimer = 0f;
        SlowDownSleepTimer += TimeSaveDelta.Instance.GetDeltaTime();

        ObjectsHoldText.text = ObjectHoldCurrently.ToString();

        Player.Instance.OnNonInteractableObjectClick += Player_OnNonInteractableObjectClick;
        
        TradeStation.Instance.OnTrade += TradeStation_OnTrade;

        makeStationUpgrade.OnUpgraded += MakeStationUpgrade_OnUpgraded;

        MainCanvasUI.Instance.OnGoToUpgradeClick += MainCanvasUI_OnGoToUpgradeClick;
        MainCanvasUI.Instance.OnGoToNormalGameplayClick += MainCanvasUI_OnGoToNormalGameplayClick;
        MainCanvasUI.Instance.OnTasksButtonClick += MainCanvas_OnTasksButtonClick;
        MainCanvasUI.Instance.OnUpgradeCashButtonClick += MainCanvas_OnUpgradeCashButtonClick;

        MaxHoldObjectUpgrade.Instance.OnMaxHoldUpgrade += MaxHoldUpgrade_OnMaxHoldUpgrade;

        UpgradeWithCashUI.Instance.OnClickMultiplierIncrease += UpgradeCashUI_OnClickMultiplierIncrease;
        UpgradeWithCashUI.Instance.OnMakeTimeIncrease += UpgradeCashUI_OnMakeTimeIncrease;

        ObjectMakeSO = TradeStation.Instance.GetCurrentWorkinObject();

        SpawnNewInputModel();
    }

    private void UpgradeCashUI_OnMakeTimeIncrease(object sender, UpgradeWithCashUI.OnMultiplierIncreaseEventArgs e) {
        IncreaseTime += e.Multiplier;
    }

    private void UpgradeCashUI_OnClickMultiplierIncrease(object sender, UpgradeWithCashUI.OnMultiplierIncreaseEventArgs e) {
        OnClickTimerInceaseMultiplier += e.Multiplier;
    }

    private void MaxHoldUpgrade_OnMaxHoldUpgrade(object sender, MaxHoldObjectUpgrade.OnMaxHoldUpgradeEventArgs e) {
        MaxObjectHold = e.HoldObjects;
    }

    private void MainCanvas_OnUpgradeCashButtonClick(object sender, EventArgs e) {
        NormalObject.SetActive(false);
    }

    private void MainCanvas_OnTasksButtonClick(object sender, EventArgs e) {
        NormalObject.SetActive(false);
    }

    private void MainCanvasUI_OnGoToNormalGameplayClick(object sender, EventArgs e) {
        NormalObject.SetActive(true);
    }

    private void MainCanvasUI_OnGoToUpgradeClick(object sender, EventArgs e) {
        NormalObject.SetActive(false);
    }

    private void MakeStationUpgrade_OnUpgraded(object sender, MakeStationUpgrade.OnUpgradedEventArgs e) {
        TimerIncreaseMultiplier += .2f;

        OnAnyUpgrade?.Invoke(this,EventArgs.Empty);
        OnUpgrade?.Invoke(this, new OnUpgradeEventArgs { TimerMultiplier = TimerIncreaseMultiplier, cost = e.cost});
    }

    private void TradeStation_OnTrade(object sender, EventArgs e) {
        DestroyOutputModels();
    }


    private void Player_OnNonInteractableObjectClick(object sender, EventArgs e) {
        
        ObjectMakeTimer += IncreaseTime * OnClickTimerInceaseMultiplier;

        SlowDownSleepTimer = 0f;
        isSlowed = false;
        isSleeping = false;
    }

    private void Update() {
        SlowDownSleepTimer += Time.deltaTime;

        if(SlowDownSleepTimer >= MaxSlowingDownTime) {
            isSlowed = true;
        }
        if(SlowDownSleepTimer >= MaxSleepingTime) {
            isSleeping = true;
        }

        if (!isSleeping && ObjectHoldCurrently < MaxObjectHold) {
            //if it can hold more objects and is not sleeping

            fillImage.color = Color.green;

            if(!isSlowed)
                ObjectMakeTimer += Time.deltaTime * TimerIncreaseMultiplier;
            else
                ObjectMakeTimer += (Time.deltaTime * TimerIncreaseMultiplier)/4f;

            OnProgressChanged?.Invoke(this,new IHasProgress.OnProgressChangedEventArgs { progress = ObjectMakeTimer / ObjectMakeSO.TimeToChange });

            if (ObjectMakeTimer > ObjectMakeSO.TimeToChange) {
                OnAnyObjectMade?.Invoke(this, EventArgs.Empty);

                ObjectHoldCurrently++;
                OnNewObjectMade?.Invoke(this, new OnNewObjectMadeEventArgs { Objects = ObjectHoldCurrently });

                UpdateVisual();

                if (!isOutputModelSpawned) {
                    //there is not a model spawned yet

                    //spawn model
                    outputModel.SetActive(true);

                    isOutputModelSpawned = true;
                }

                ObjectMakeTimer = 0f;
            }
        }

        else if(!isSleeping) {
            fillImage.fillAmount = 1f;
            fillImage.color = Color.red;
        }
        else if (isSleeping && ObjectHoldCurrently < MaxObjectHold) {
            fillImage.fillAmount = 0f;

            fillImage.color = Color.green;
        }
    }


    private void DestroyOutputModels() {
        //check if there are spwaned objects at the table
        if (ObjectHoldCurrently > 0) {
            //there are some output models on the making station

            ObjectHoldCurrently = 0;

            UpdateVisual();

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

        makeStationUpgrade.OnUpgraded -= MakeStationUpgrade_OnUpgraded;

        MainCanvasUI.Instance.OnGoToUpgradeClick -= MainCanvasUI_OnGoToUpgradeClick;
        MainCanvasUI.Instance.OnGoToNormalGameplayClick -= MainCanvasUI_OnGoToNormalGameplayClick;
        MainCanvasUI.Instance.OnTasksButtonClick -= MainCanvas_OnTasksButtonClick;
        MainCanvasUI.Instance.OnUpgradeCashButtonClick -= MainCanvas_OnUpgradeCashButtonClick;

        MaxHoldObjectUpgrade.Instance.OnMaxHoldUpgrade -= MaxHoldUpgrade_OnMaxHoldUpgrade;

        UpgradeWithCashUI.Instance.OnClickMultiplierIncrease -= UpgradeCashUI_OnClickMultiplierIncrease;
        UpgradeWithCashUI.Instance.OnMakeTimeIncrease -= UpgradeCashUI_OnMakeTimeIncrease;
    }

    public void SetCurrentWorkingObject() {
        ObjectMakeSO = TradeStation.Instance.GetCurrentWorkinObject();

        SpawnNewInputModel();
    }

    public void HideNormalObject() {
        NormalObject.SetActive(false);
    }

    private void UpdateVisual() {
        ObjectsHoldText.text = ObjectHoldCurrently.ToString();
    }

    public static float GetClickMultiplier() {
        return OnClickTimerInceaseMultiplier;
    }

    public static float GetMakeTimeMultiplier() {
        return IncreaseTime;
    }

    public void SetStationData(float TimerIncreaseMultiplier, float CostOfUpgrade, int ObjectHoldCurrently) {
        this.TimerIncreaseMultiplier = TimerIncreaseMultiplier;
        this.ObjectHoldCurrently = ObjectHoldCurrently;

        makeStationUpgrade.SetCost(CostOfUpgrade);

        UpdateUI();

        AddObjectsAcordingToTime();
    }

    private void AddObjectsAcordingToTime() {
        //calculate the remaining objects needed
        int ObjectsLeft = MaxObjectHold - ObjectHoldCurrently;

        StartCoroutine(ChangeObjectHeld(ObjectsLeft));
    }

    private IEnumerator ChangeObjectHeld(int ObjectsLeft) {
        yield return new WaitUntil(()=> ObjectMakeSO != null);

        ObjectHoldCurrently += Mathf.FloorToInt(TimeSaveDelta.Instance.GetDeltaTime() / ObjectMakeSO.TimeToChange);

        ObjectHoldCurrently = Mathf.Clamp(ObjectHoldCurrently,0,MaxObjectHold);

        UpdateUI();
    }

    private void UpdateUI() {
        ObjectsHoldText.text = ObjectHoldCurrently.ToString();

        if(ObjectHoldCurrently >= MaxObjectHold) {
            fillImage.fillAmount = 1f;
            fillImage.color = Color.red;
        }

        if(ObjectHoldCurrently > 0) {
            outputModel.SetActive(true);
            isOutputModelSpawned = true;
        }
    }

}
