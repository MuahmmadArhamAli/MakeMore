using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GetMoneyTask : MonoBehaviour,IHasProgress{

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    [SerializeField] private Tasks tasks;

    //reference of Text to update
    [SerializeField] private TextMeshProUGUI ShowingText;
    
    private void Start() {
        ShowingText.text = tasks.GetCurrentValue(Tasks.TasksComplete.GetMoneyTask).ToString() + "/" + tasks.GetMaxValue(Tasks.TasksComplete.GetMoneyTask).ToString();
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progress = tasks.GetCurrentValue(Tasks.TasksComplete.GetMoneyTask) / tasks.GetMaxValue(Tasks.TasksComplete.GetMoneyTask) });

        tasks.OnGetMoneyTaskUpdate += Tasks_OnGetMoneyTaskUpdate;
    }

    private void Tasks_OnGetMoneyTaskUpdate(object sender, Tasks.OnTaskUpdateEventArgs e) {
        ShowingText.text = tasks.GetCurrentValue(Tasks.TasksComplete.GetMoneyTask).ToString() + "/" + tasks.GetMaxValue(Tasks.TasksComplete.GetMoneyTask).ToString();

        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progress = e.CurrentValue/e.MaxValue });
    }

    private void OnDestroy() {
        tasks.OnGetMoneyTaskUpdate -= Tasks_OnGetMoneyTaskUpdate;
    }
}
