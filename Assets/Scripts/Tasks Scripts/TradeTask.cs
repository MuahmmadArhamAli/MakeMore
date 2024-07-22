using System;
using TMPro;
using UnityEngine;

public class TradeTask : MonoBehaviour,IHasProgress{


    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    [SerializeField] private Tasks tasks;

    //reference of Text to update
    [SerializeField] private TextMeshProUGUI ShowingText;

    private void Start() {
        ShowingText.text = tasks.GetCurrentAndMaxValue(Tasks.TasksComplete.TradeTask);

        tasks.OnTradeTaskUpdate += Tasks_OnTradeTaskUpdate;
    }

    private void Tasks_OnTradeTaskUpdate(object sender, Tasks.OnTaskUpdateEventArgs e) {
        ShowingText.text = tasks.GetCurrentAndMaxValue(Tasks.TasksComplete.TradeTask);

        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progress = e.CurrentValue / e.MaxValue });
    }

}
