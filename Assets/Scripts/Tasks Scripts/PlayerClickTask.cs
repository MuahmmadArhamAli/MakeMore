using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerClickTask : MonoBehaviour,IHasProgress{

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    [SerializeField] private Tasks tasks;

    //reference of Text to update
    [SerializeField] private TextMeshProUGUI ShowingText;

    private void Start() {
        ShowingText.text = tasks.GetCurrentAndMaxValue(Tasks.TasksComplete.TapTask);

        tasks.OnPlayerClickTaskUpdate += Tasks_OnPlayerClickTaskUpdate;
    }

    private void Tasks_OnPlayerClickTaskUpdate(object sender, Tasks.OnTaskUpdateEventArgs e) {
        ShowingText.text = tasks.GetCurrentAndMaxValue(Tasks.TasksComplete.TapTask);

        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progress = e.CurrentValue / e.MaxValue });
    }
}
