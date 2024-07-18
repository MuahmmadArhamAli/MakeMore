using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddNewStation : MonoBehaviour{
    //reference of makeStation 
    [SerializeField] private ObjectMakeStation MakeStationPrefab;

    private ObjectMakeStation makeStation;

    //reference to see if there should be a makeStation at the start
    [SerializeField] private bool ShouldMakeStationSpawn;

    //reference of button and text
    [SerializeField] private Button AddButton;
    [SerializeField] private TextMeshProUGUI PriceText;

    //reference of canvas to show only on upgrade click
    [SerializeField] private GameObject AddCanvas;

    //bool to see if a prefab has already been spawned
    private bool isMakeStationSpawned;

    //price of adding
    private static float PriceOfAdding;
    private static float CostMultiplier;

    //static event to updatevisual on any change
    private static event EventHandler OnPriceChanged;


    private void Awake() {
        AddButton.onClick.AddListener(() => {
            if (!isMakeStationSpawned) {
                //spawn a new station

                if (NormalMoney.Instance.GetMoney() >= PriceOfAdding) {
                    
                    makeStation = Instantiate(MakeStationPrefab, transform.position,Quaternion.identity);

                    isMakeStationSpawned = true;

                    makeStation.HideNormalObject();

                    NormalMoney.Instance.DecreaseMoney(PriceOfAdding);

                    CostMultiplier += 0.2f;
                    PriceOfAdding += (PriceOfAdding * CostMultiplier);

                    UpdateVisual();

                    OnPriceChanged?.Invoke(this, EventArgs.Empty);

                    HideCanvas();
                }
            }
        });
    }

    private void Start() {
        CostMultiplier = 1f;
        PriceOfAdding = 200 * CostMultiplier;

        UpdateVisual();
        HideCanvas();

        if (ShouldMakeStationSpawn) {
            //spawn the make station

            makeStation = Instantiate(MakeStationPrefab, transform.position, Quaternion.identity);

            isMakeStationSpawned = true;
        }

        MainCanvasUI.Instance.OnGoToUpgradeClick += MianCanvas_OnGoToUpgradeClick;
        MainCanvasUI.Instance.OnGoToNormalGameplayClick += MainCanvas_OnGoToNormalGameplayClick;

        OnPriceChanged += AddNewStation_OnPriceChanged;
    }

    private void AddNewStation_OnPriceChanged(object sender, EventArgs e) {
        UpdateVisual();
    }

    private void MainCanvas_OnGoToNormalGameplayClick(object sender, System.EventArgs e) {
        HideCanvas();
    }

    private void MianCanvas_OnGoToUpgradeClick(object sender, System.EventArgs e) {
        ShowCanvas();
    }

    private void ShowCanvas() {
        if (!isMakeStationSpawned) {
            AddCanvas.SetActive(true);
        }
    }
    private void HideCanvas() {
        AddCanvas.SetActive(false);
    }

    private void UpdateVisual() {
        PriceText.text = PriceOfAdding.ToString();
    }
}
