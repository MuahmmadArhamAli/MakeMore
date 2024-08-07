using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeStationAnimation : MonoBehaviour{

    [SerializeField] private ObjectMakeStation makeStation;

    //references for all the different faces
    [SerializeField] private Sprite HappyFace;
    [SerializeField] private Sprite SmileFace;
    [SerializeField] private Sprite WaitFace;
    [SerializeField] private Sprite SleepFace;

    //Face object
    [SerializeField] private GameObject Face;
    private SpriteRenderer spriteRenderer;

    //animator
    private Animator animator;

    private const string SlowedWorkingString = "SlowWork";
    private const string WorkingString = "Work";
    private const string IdleString = "Idle";

    private void Awake() {
        animator = GetComponent<Animator>();
        spriteRenderer = Face.GetComponent<SpriteRenderer>();
    }
    private void Start() {
        makeStation.OnStateChanged += MakeStation_OnStateChanged;
    }

    private void MakeStation_OnStateChanged(object sender, ObjectMakeStation.OnStateChagedEventArgs e) {
        if(e.state == ObjectMakeStation.WorkingStates.Working) {
            spriteRenderer.sprite = SmileFace;
            animator.SetTrigger(WorkingString);
        }
        else if (e.state == ObjectMakeStation.WorkingStates.Sleeping) {
            spriteRenderer.sprite = SleepFace;
            animator.SetTrigger(IdleString);
        }
        else if (e.state == ObjectMakeStation.WorkingStates.Slowed) {
            spriteRenderer.sprite = HappyFace;
            animator.SetTrigger(SlowedWorkingString);
        }
        else if (e.state == ObjectMakeStation.WorkingStates.Waiting) {
            spriteRenderer.sprite = WaitFace;
            animator.SetTrigger(IdleString);
        }

    }

    private void OnDestroy() {
        makeStation.OnStateChanged -= MakeStation_OnStateChanged;
    }
}
