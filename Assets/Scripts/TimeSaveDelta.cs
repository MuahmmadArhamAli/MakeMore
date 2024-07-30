using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSaveDelta : MonoBehaviour{
    //Singleton
    public static TimeSaveDelta Instance {  get; private set; }

    private const string TimeDeltaSaveString = "TimeToEnd";

    private float LastTimeToStart;
    private float DeltaTime;

    private void Awake() {
        Instance = this;

        LastTimeToStart = PlayerPrefs.GetFloat(TimeDeltaSaveString,Time.time);

        DeltaTime = Time.time - LastTimeToStart;
    }

    public float GetDeltaTime() {
        return DeltaTime;
    }

    private void OnDestroy() {
        PlayerPrefs.SetFloat(TimeDeltaSaveString,Time.time);
        PlayerPrefs.Save();
    }

}
