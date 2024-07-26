using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TasksUI : MonoBehaviour{

    //references of all buttons
    [SerializeField] private Button TradeTaskButton;
    [SerializeField] private Button UpgradeWorkingObjectTaskButton;
    [SerializeField] private Button TapTaskButton;
    [SerializeField] private Button UpgradeWorkStationTaskButton;
    [SerializeField] private Button GetMoneyTaskButton;

    //references of texts
    [SerializeField] private TextMeshProUGUI TradeTaskText;
    [SerializeField] private TextMeshProUGUI UpgradeWorkingObjectTaskText;
    [SerializeField] private TextMeshProUGUI TapTaskText;
    [SerializeField] private TextMeshProUGUI UpgradeWorkStationTaskText;
    [SerializeField] private TextMeshProUGUI GetMoneyTaskText;

    [SerializeField] private TextMeshProUGUI TradeTaskAvailabilityText;
    [SerializeField] private TextMeshProUGUI UpgradeWorkingTaskAvailabilityText;
    [SerializeField] private TextMeshProUGUI TapTaskAvailabilityText;
    [SerializeField] private TextMeshProUGUI UpgradeWorkStationTaskAvailabilityText;
    [SerializeField] private TextMeshProUGUI GetMoneyTaskAvailabilityText;
 
    //reference of tasks
    [SerializeField] private Tasks tasks;

    //event to send the tasks script to reset the task
    public event EventHandler<Tasks.OnTaskCompleteEventArgs> OnTaskMoneyReceived;

    private void Awake() {
        TradeTaskButton.onClick.AddListener(() => {
            CashMoney.Instance.IncreaseMoney(2f);

            OnTaskMoneyReceived?.Invoke(this, new Tasks.OnTaskCompleteEventArgs { taskComplete = Tasks.TasksComplete.TradeTask });
        });
        UpgradeWorkingObjectTaskButton.onClick.AddListener(() => {
            CashMoney.Instance.IncreaseMoney(2f);

            OnTaskMoneyReceived?.Invoke(this, new Tasks.OnTaskCompleteEventArgs { taskComplete = Tasks.TasksComplete.UpgradeObjectTask });
        });
        TapTaskButton.onClick.AddListener(() => {
            CashMoney.Instance.IncreaseMoney(2f);

            OnTaskMoneyReceived?.Invoke(this, new Tasks.OnTaskCompleteEventArgs { taskComplete = Tasks.TasksComplete.TapTask });
        });
        UpgradeWorkStationTaskButton.onClick.AddListener(() => {
            CashMoney.Instance.IncreaseMoney(2f);

            OnTaskMoneyReceived?.Invoke(this, new Tasks.OnTaskCompleteEventArgs { taskComplete = Tasks.TasksComplete.UpgradeStationTask });
        });
        GetMoneyTaskButton.onClick.AddListener(() => {
            CashMoney.Instance.IncreaseMoney(2f);

            OnTaskMoneyReceived?.Invoke(this, new Tasks.OnTaskCompleteEventArgs { taskComplete = Tasks.TasksComplete.GetMoneyTask });
        });
    }

    private void Start() {
        StartCoroutine(HideDelay());

        MainCanvasUI.Instance.OnTasksButtonClick += MainCanvas_OnTasksButtonClick;
        MainCanvasUI.Instance.OnGoToNormalGameplayClick += MainCanvas_OnGoToNormalGameplayClick;
        MainCanvasUI.Instance.OnGoToUpgradeClick += MainCanvas_OnGoToUpgradeClick;
        MainCanvasUI.Instance.OnUpgradeCashButtonClick += MainCanvas_OnUpgradeCashButtonClick;
        
        tasks.OnTaskComplete += Tasks_OnTaskComplete;
        tasks.OnTaskCompletionChange += Tasks_OnTaskCompletionChange;

        //enable or disable buttons
        TradeTaskButton.interactable = tasks.GetTaskCompletion(Tasks.TasksComplete.TradeTask);
        GetMoneyTaskButton.interactable = tasks.GetTaskCompletion(Tasks.TasksComplete.GetMoneyTask);
        TapTaskButton.interactable = tasks.GetTaskCompletion(Tasks.TasksComplete.TapTask);
        UpgradeWorkingObjectTaskButton.interactable = tasks.GetTaskCompletion(Tasks.TasksComplete.UpgradeObjectTask);
        UpgradeWorkStationTaskButton.interactable = tasks.GetTaskCompletion(Tasks.TasksComplete.UpgradeStationTask);

        SetAvailabilityTexts();

        //set text
        TradeTaskText.text = tasks.GetTaskText(Tasks.TasksComplete.TradeTask);
        GetMoneyTaskText.text = tasks.GetTaskText(Tasks.TasksComplete.GetMoneyTask);
        TapTaskText.text = tasks.GetTaskText(Tasks.TasksComplete.TapTask);
        UpgradeWorkingObjectTaskText.text = tasks.GetTaskText(Tasks.TasksComplete.UpgradeObjectTask);
        UpgradeWorkStationTaskText.text = tasks.GetTaskText(Tasks.TasksComplete.UpgradeStationTask);
    }

    private void Tasks_OnTaskCompletionChange(object sender, EventArgs e) {

        // set the buttons interactability accordingly

        TradeTaskButton.interactable = tasks.GetTaskCompletion(Tasks.TasksComplete.TradeTask);
        GetMoneyTaskButton.interactable = tasks.GetTaskCompletion(Tasks.TasksComplete.GetMoneyTask);
        TapTaskButton.interactable = tasks.GetTaskCompletion(Tasks.TasksComplete.TapTask);
        UpgradeWorkingObjectTaskButton.interactable = tasks.GetTaskCompletion(Tasks.TasksComplete.UpgradeObjectTask);
        UpgradeWorkStationTaskButton.interactable = tasks.GetTaskCompletion(Tasks.TasksComplete.UpgradeStationTask);

        SetAvailabilityTexts();
    }

    private void Tasks_OnTaskComplete(object sender, Tasks.OnTaskCompleteEventArgs e) {
        // see which task was completed and set the interactable accordingly
        if(e.taskComplete == Tasks.TasksComplete.TradeTask) {
            TradeTaskButton.interactable = true;
        }
        else if (e.taskComplete == Tasks.TasksComplete.UpgradeObjectTask) {
            UpgradeWorkingObjectTaskButton.interactable = true;
        }
        else if(e.taskComplete == Tasks.TasksComplete.TapTask) {
            TapTaskButton.interactable = true;
        }
        else if(e.taskComplete == Tasks.TasksComplete.UpgradeStationTask) {
            UpgradeWorkStationTaskButton.interactable = true;
        }
        else {
            GetMoneyTaskButton.interactable = true;
        }
    }
    private void MainCanvas_OnUpgradeCashButtonClick(object sender, EventArgs e) {
        Hide();
    }

    private void MainCanvas_OnGoToUpgradeClick(object sender, System.EventArgs e) {
        Hide();
    }

    private void MainCanvas_OnGoToNormalGameplayClick(object sender, System.EventArgs e) {
        Hide();
    }

    private void MainCanvas_OnTasksButtonClick(object sender, System.EventArgs e) {
        Show();
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private IEnumerator HideDelay() {
        yield return new WaitForSeconds(0.5f);

        Hide();
    }

    private void OnDestroy() {
        MainCanvasUI.Instance.OnTasksButtonClick -= MainCanvas_OnTasksButtonClick;
        MainCanvasUI.Instance.OnGoToNormalGameplayClick -= MainCanvas_OnGoToNormalGameplayClick;
        MainCanvasUI.Instance.OnGoToUpgradeClick -= MainCanvas_OnGoToUpgradeClick;
        MainCanvasUI.Instance.OnUpgradeCashButtonClick -= MainCanvas_OnUpgradeCashButtonClick;

        tasks.OnTaskComplete -= Tasks_OnTaskComplete;
        tasks.OnTaskCompletionChange -= Tasks_OnTaskCompletionChange;

    }

    private void SetAvailabilityTexts() {
        TradeTaskAvailabilityText.text = TradeTaskButton.interactable ? "Available" : "Unavailable";
        UpgradeWorkingTaskAvailabilityText.text = UpgradeWorkingObjectTaskButton.interactable ? "Available" : "Unavailable";
        TapTaskAvailabilityText.text = TapTaskButton.interactable ? "Available" : "Unavailable";
        UpgradeWorkStationTaskAvailabilityText.text = UpgradeWorkStationTaskButton.interactable ? "Available" : "Unavailable";
        GetMoneyTaskAvailabilityText.text = GetMoneyTaskButton.interactable ? "Available" : "Unavailable";
    }
}
