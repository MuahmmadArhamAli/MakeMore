using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeObjectTask : MonoBehaviour,IHasProgress{


    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    [SerializeField] private Tasks tasks;

    //reference of Text to update
    [SerializeField] private TextMeshProUGUI ShowingText;
    
    private void Start() {
        ShowingText.text = tasks.GetCurrentValue(Tasks.TasksComplete.UpgradeObjectTask).ToString() + "/"  + tasks.GetMaxValue(Tasks.TasksComplete.UpgradeObjectTask).ToString();

        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progress =  tasks.GetCurrentValue(Tasks.TasksComplete.UpgradeObjectTask) / tasks.GetMaxValue(Tasks.TasksComplete.UpgradeObjectTask) });


        tasks.OnUpgradeObjectTaskUpdate += Tasks_OnUpgradeObjectTaskUpdate;
    }

    private void Tasks_OnUpgradeObjectTaskUpdate(object sender, Tasks.OnTaskUpdateEventArgs e) {
        ShowingText.text = tasks.GetCurrentValue(Tasks.TasksComplete.UpgradeObjectTask).ToString() + "/" + tasks.GetMaxValue(Tasks.TasksComplete.UpgradeObjectTask).ToString();

        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progress = e.CurrentValue / e.MaxValue });
    }
}
