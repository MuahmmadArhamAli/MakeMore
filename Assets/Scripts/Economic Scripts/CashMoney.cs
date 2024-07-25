using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashMoney : MonoBehaviour, IMoney {
    //Singleton
    public static CashMoney Instance {  get; private set; }

    public class OnCashChangedEventArgs : EventArgs { public float cash; }
    public event EventHandler<OnCashChangedEventArgs> OnCashChanged;

    private float CashAmount = 2f;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        CashAmount = 2f;
    }

    public void DecreaseMoney(float money) {
        CashAmount -= money;

        OnCashChanged?.Invoke(this, new OnCashChangedEventArgs { cash = CashAmount });
    }

    public float GetMoney() {
        return CashAmount;
    }

    public void IncreaseMoney(float money) {
        CashAmount += money;

        OnCashChanged?.Invoke(this, new OnCashChangedEventArgs { cash = CashAmount });
    }
}
