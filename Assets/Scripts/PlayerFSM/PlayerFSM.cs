using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFSM : MonoBehaviour {
    // Start is called before the first frame update
    public IdleState IdleState;
    public JumpState JumpState;
    public RunState RunState;

    public PlayerState currentState;
    public Rigidbody2D rb;
    void Start () {
        currentState = IdleState;
    }

    // Update is called once per frame
    void Update () {
    }

    public void SetState (PlayerState newState){
        currentState = newState;
        currentState.onTrigger(this);
    }

    void FixedUpdate () {
        currentState.onUpdate (this);
    }
}