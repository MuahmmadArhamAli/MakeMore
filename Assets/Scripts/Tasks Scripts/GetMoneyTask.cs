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
        ShowingText.text = tasks.GetCurrentAndMaxValue(Tasks.TasksComplete.GetMoneyTask);

        tasks.OnGetMoneyTaskUpdate += Tasks_OnGetMoneyTaskUpdate;
    }

    private void Tasks_OnGetMoneyTaskUpdate(object sender, Tasks.OnTaskUpdateEventArgs e) {
        ShowingText.text = tasks.GetCurrentAndMaxValue(Tasks.TasksComplete.GetMoneyTask);

        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progress = e.CurrentValue/e.MaxValue });
    }
}
