using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tasks : MonoBehaviour{
    private int TotalTasks;

    //arrays of tasks
    private string[] TasksTexts = {"Trade 100 times","Upgrade object 5 times","Tap 3000 times","Upgrade a station 3 times","Get 4000 money"};

    //array of times
    private float[] TaskTimes = {0f,0f,0f,0f,0f};

    //array of bools to see if a task is completed
    private bool[] TaskCompleted = {false,false,false,false,false};

    //array of bools to see if the task is available
    private bool[] TaskAvailability = {true,true,true,true,true };

    private float MaxTime;

    //task ui reference
    [SerializeField] private TasksUI taskUI;

    //variables for each task

    //tap task variables
    private int PlayerTapsCount;
    private int PlayerTapCountMax;
    private bool isOnNormalGameplay;

    //trade task variables
    private int TradeCount;
    private int TradeCountMax;

    //upgrad station varoiables
    private int StationUpgradeCount;
    private int StationUpgradeCountMax;

    //Get money task variables
    private float MoneyRequired = 4000;

    //Upgrade Object Tasks
    private int ObjectUpgradeCount;
    private int ObjectUpgradeCountMax;

    //enum for different tasks
    public enum TasksComplete {
        TradeTask,
        UpgradeObjectTask,
        TapTask,
        UpgradeStationTask,
        GetMoneyTask,
    }
    
    public class OnTaskCompleteEventArgs : EventArgs { public TasksComplete taskComplete; }
    public event EventHandler<OnTaskCompleteEventArgs> OnTaskComplete;

    //event to send the tasks ui that it has done its work for task completion
    public event EventHandler OnTaskCompletionChange;


    //events for different tasks to update UI
    public class OnTaskUpdateEventArgs : EventArgs { public float CurrentValue; public float MaxValue; }

    public event EventHandler<OnTaskUpdateEventArgs> OnPlayerClickTaskUpdate;
    public event EventHandler<OnTaskUpdateEventArgs> OnGetMoneyTaskUpdate;
    public event EventHandler<OnTaskUpdateEventArgs> OnUpgradeStationTaskUpdate;
    public event EventHandler<OnTaskUpdateEventArgs> OnTradeTaskUpdate;
    public event EventHandler<OnTaskUpdateEventArgs> OnUpgradeObjectTaskUpdate;


    private void Start() {
        TotalTasks = 5;

        MaxTime = 5f;

        PlayerTapsCount = 0;
        PlayerTapCountMax = 5;

        TradeCount = 0;
        TradeCountMax = 5;
        
        StationUpgradeCount = 0;
        StationUpgradeCountMax = 2;
        
        MoneyRequired = 4000;

        ObjectUpgradeCount = 0;
        ObjectUpgradeCountMax = 4;

        taskUI.OnTaskMoneyReceived += TaskUI_OnTaskMoneyReceived;

        Player.Instance.OnNonInteractableObjectClick += Player_OnNonInteractableObjectClick;
        TradeStation.Instance.OnTradeForMoney += TradeStation_OnTradeForMoney;
        ObjectMakeStation.OnUpgrade += ObjectMakeStation_OnUpgrade;
        NormalMoney.Instance.OnMoneyChanged += NormalMoney_OnMoneyChanged;

        MainCanvasUI.Instance.OnGoToNormalGameplayClick += MainCanvas_OnGoToNormalGameplayClick;
        MainCanvasUI.Instance.OnGoToUpgradeClick += MainCanvas_OnGoToUpgradeClick;
        MainCanvasUI.Instance.OnTasksButtonClick += MainCanvas_OnTasksButtonClick;
        MainCanvasUI.Instance.OnUpgradeCashButtonClick += MainCanvas_OnUpgradeCashButtonClick;
    }

    private void MainCanvas_OnUpgradeCashButtonClick(object sender, EventArgs e) {
        isOnNormalGameplay = false;
    }

    private void MainCanvas_OnTasksButtonClick(object sender, EventArgs e) {
        isOnNormalGameplay = false;
    }

    private void MainCanvas_OnGoToUpgradeClick(object sender, EventArgs e) {
        isOnNormalGameplay = false;
    }

    private void MainCanvas_OnGoToNormalGameplayClick(object sender, EventArgs e) {
        isOnNormalGameplay = true;
    }

    private void NormalMoney_OnMoneyChanged(object sender, NormalMoney.OnMoneyChangedEventArgs e) {
        if (TaskAvailability[GetIndex(TasksComplete.GetMoneyTask)]) {

            OnGetMoneyTaskUpdate?.Invoke(this, new OnTaskUpdateEventArgs { CurrentValue = NormalMoney.Instance.GetMoney(), MaxValue = MoneyRequired });

            if (NormalMoney.Instance.GetMoney() >= MoneyRequired) {
                //send events
                OnTaskComplete?.Invoke(this, new OnTaskCompleteEventArgs { taskComplete = TasksComplete.GetMoneyTask });

                TaskCompleted[GetIndex(TasksComplete.GetMoneyTask)] = true;
            }
        }
    }

    private void ObjectMakeStation_OnUpgrade(object sender, EventArgs e) {
        //check if the task is not alreday done i.e is available
        if (TaskAvailability[GetIndex(TasksComplete.UpgradeStationTask)]) {

            StationUpgradeCount++;

            OnUpgradeStationTaskUpdate?.Invoke(this, new OnTaskUpdateEventArgs { CurrentValue = StationUpgradeCount , MaxValue = StationUpgradeCountMax });

            if(StationUpgradeCount >= StationUpgradeCountMax) {
                OnTaskComplete?.Invoke(this, new OnTaskCompleteEventArgs { taskComplete = TasksComplete.UpgradeStationTask });

                TaskCompleted[GetIndex(TasksComplete.UpgradeStationTask)] = true;
            }
        }
    }

    private void TaskUI_OnTaskMoneyReceived(object sender, OnTaskCompleteEventArgs e) {
        //check which task was completed and set the timer accordingly

        TaskAvailability[GetIndex(e.taskComplete)] = false;

        if(e.taskComplete == TasksComplete.TradeTask) {
            TradeCount -= TradeCountMax;

            //if condition to see whether the player has alread done this task so that he can get money 2 times
            if(TradeCount < TradeCountMax) {
                //set task completed to false

                TaskCompleted[GetIndex(TasksComplete.TradeTask)] = false;
            }

            OnTradeTaskUpdate?.Invoke(this, new OnTaskUpdateEventArgs { CurrentValue = TradeCount, MaxValue = TradeCountMax });
        }
        else if(e.taskComplete == TasksComplete.UpgradeObjectTask) {

            OnUpgradeObjectTaskUpdate?.Invoke(this, new OnTaskUpdateEventArgs { CurrentValue = 0, MaxValue = 1});

        }
        else if(e.taskComplete == TasksComplete.TapTask) {
            PlayerTapsCount -= PlayerTapCountMax;

            //if condition to see whether the player has alread done this task so that he can get money 2 times
            if (PlayerTapsCount < PlayerTapCountMax) {
                TaskCompleted[GetIndex(TasksComplete.TapTask)] = false;
            }

            OnPlayerClickTaskUpdate?.Invoke(this, new OnTaskUpdateEventArgs { CurrentValue = PlayerTapsCount, MaxValue = PlayerTapCountMax });
        }
        else if(e.taskComplete == TasksComplete.UpgradeStationTask) {

            StationUpgradeCount -= StationUpgradeCountMax;
            //if condition to see whether the player has alread done this task so that he can get money 2 times
            if (StationUpgradeCount < StationUpgradeCountMax) {
                TaskCompleted[GetIndex(TasksComplete.UpgradeStationTask)] = false;
            }

            OnUpgradeStationTaskUpdate?.Invoke(this, new OnTaskUpdateEventArgs { CurrentValue = StationUpgradeCount, MaxValue = StationUpgradeCountMax });
        }
        else {
            if(NormalMoney.Instance.GetMoney() < MoneyRequired) {
                TaskCompleted[GetIndex(TasksComplete.GetMoneyTask)] = false;
            }

            OnGetMoneyTaskUpdate?.Invoke(this, new OnTaskUpdateEventArgs { CurrentValue = NormalMoney.Instance.GetMoney(), MaxValue = MoneyRequired });
        }

        OnTaskCompletionChange?.Invoke(this,EventArgs.Empty);
    }

    private void TradeStation_OnTradeForMoney(object sender, TradeStation.OnTradeForMoneyEventArgs e) {
        if (TaskAvailability[GetIndex(TasksComplete.TradeTask)]) {
            //if the task is available then add to the count


            if (e.money > 0) {
                //the trade was for money and not just a simple click

                TradeCount++;

                OnTradeTaskUpdate?.Invoke(this, new OnTaskUpdateEventArgs { CurrentValue = TradeCount, MaxValue = TradeCountMax });

                if (TradeCount >= TradeCountMax) {
                    OnTaskComplete?.Invoke(this, new OnTaskCompleteEventArgs { taskComplete = TasksComplete.TradeTask });

                    TaskCompleted[GetIndex(TasksComplete.TradeTask)] = true;
                }
            }
        }
    }

    private void Player_OnNonInteractableObjectClick(object sender, EventArgs e) {
        if (isOnNormalGameplay) {
            if (TaskAvailability[GetIndex(TasksComplete.TapTask)]) {

                PlayerTapsCount++;

                OnPlayerClickTaskUpdate.Invoke(this, new OnTaskUpdateEventArgs { MaxValue = PlayerTapCountMax, CurrentValue = PlayerTapsCount });

                if (PlayerTapsCount >= PlayerTapCountMax) {
                    //send event
                    OnTaskComplete?.Invoke(this, new OnTaskCompleteEventArgs { taskComplete = TasksComplete.TapTask });

                    TaskCompleted[GetIndex(TasksComplete.TapTask)] = true;
                }
            }
        }
    }

    public string GetTaskText(TasksComplete task) {
        switch(task){
            case TasksComplete.TradeTask:
                return TasksTexts[0];
            case TasksComplete.UpgradeObjectTask:
                return TasksTexts[1];
            case TasksComplete.TapTask:
                return TasksTexts[2];
            case TasksComplete.UpgradeStationTask:
                return TasksTexts[3];
            case TasksComplete.GetMoneyTask:
                return TasksTexts[4];
            default:
                return null;
        }
    }
    
    public bool GetTaskCompletion( TasksComplete task) {
        //see which task is completed and available
        
        switch (task) {
            case TasksComplete.TradeTask:
                return TaskCompleted[0] && TaskAvailability[0];
            case TasksComplete.UpgradeObjectTask:
                return TaskCompleted[1] && TaskAvailability[1];
            case TasksComplete.TapTask:
                return TaskCompleted[2] && TaskAvailability[2];
            case TasksComplete.UpgradeStationTask:
                return TaskCompleted[3] && TaskAvailability[3];
            case TasksComplete.GetMoneyTask:
                return TaskCompleted[4] && TaskAvailability[4];
            default:
                return false;
        }
    }

    private int GetIndex(TasksComplete task) {
        switch (task) {
            case TasksComplete.TradeTask:
                return 0;
            case TasksComplete.UpgradeObjectTask:
                return 1;
            case TasksComplete.TapTask:
                return 2;
            case TasksComplete.UpgradeStationTask:
                return 3;
            case TasksComplete.GetMoneyTask:
                return 4;
            default:
                return 0;
        }
    }


    //function for different tasks to get the values at the start

    public float GetCurrentValue(TasksComplete task) {
        switch (task) {
            case TasksComplete.TradeTask:
                return TradeCount;
            case TasksComplete.TapTask:
                return PlayerTapsCount;
            case TasksComplete.GetMoneyTask:
                return NormalMoney.Instance.GetMoney();
            case TasksComplete.UpgradeStationTask:
                return StationUpgradeCount;
            case TasksComplete.UpgradeObjectTask:
                return ObjectUpgradeCount;
            default:
                return 0f;
        }
    }

    public float GetMaxValue(TasksComplete task) {
        switch (task) {
            case TasksComplete.TradeTask:
                return TradeCountMax;
            case TasksComplete.TapTask:
                return PlayerTapCountMax;
            case TasksComplete.GetMoneyTask:
                return MoneyRequired;
            case TasksComplete.UpgradeStationTask:
                return StationUpgradeCountMax;
            case TasksComplete.UpgradeObjectTask:
                return ObjectUpgradeCountMax;
            default:
                return 0f;
        }
    }

    private void Update() {
        //increase task timers based on if they are available
        for (int i = 0; i < TotalTasks; i++) {
            if (!TaskAvailability[i]) {
                TaskTimes[i] += Time.deltaTime;

                if (TaskTimes[i] > MaxTime) {
                    //task available again
                    TaskTimes[i] = 0f;

                    TaskAvailability[i] = true;
                    OnTaskCompletionChange?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }

    private void OnDestroy() {
        taskUI.OnTaskMoneyReceived -= TaskUI_OnTaskMoneyReceived;

        Player.Instance.OnNonInteractableObjectClick -= Player_OnNonInteractableObjectClick;
        TradeStation.Instance.OnTradeForMoney -= TradeStation_OnTradeForMoney;
        ObjectMakeStation.OnUpgrade -= ObjectMakeStation_OnUpgrade;
        NormalMoney.Instance.OnMoneyChanged -= NormalMoney_OnMoneyChanged;

        MainCanvasUI.Instance.OnGoToNormalGameplayClick -= MainCanvas_OnGoToNormalGameplayClick;
        MainCanvasUI.Instance.OnGoToUpgradeClick -= MainCanvas_OnGoToUpgradeClick;
        MainCanvasUI.Instance.OnTasksButtonClick -= MainCanvas_OnTasksButtonClick;
        MainCanvasUI.Instance.OnUpgradeCashButtonClick -= MainCanvas_OnUpgradeCashButtonClick;
    }

}
