using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : PlayerState
{
    // Start is called before the first frame update
    public Animator anim;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void onTrigger (PlayerFSM FSM) { 
        anim.Play("Idle");
    }
    public override void onUpdate (PlayerFSM FSM) { 
        print("Idle");
        if(Input.GetKey(ButtonConfig.key.jump)){
            FSM.SetState(FSM.JumpState);
        }
        if(Input.GetKey(ButtonConfig.key.left) || Input.GetKey(ButtonConfig.key.right)){
            FSM.SetState(FSM.RunState);
        }
    }
}
