using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RunState : PlayerState
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
        anim.Play("Run");
        var direct = 1;
        if(Input.GetKey(ButtonConfig.key.left)){
            direct = -1;
            transform.localScale = new Vector3(
                -Math.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z);
        }else{
            transform.localScale = new Vector3(
                Math.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z);
        }
        FSM.rb.velocity = new Vector2(8 * direct, FSM.rb.velocity.y);
    }
    public override void onUpdate (PlayerFSM FSM) { 
        print("Run");
        if(Input.GetKey(ButtonConfig.key.left) || Input.GetKey(ButtonConfig.key.right)){
            FSM.SetState(FSM.RunState);
        }

        if(!Input.GetKey(ButtonConfig.key.left) && !Input.GetKey(ButtonConfig.key.right)){
            FSM.SetState(FSM.IdleState);
        }

        if(Input.GetKey(ButtonConfig.key.jump)){
            FSM.SetState(FSM.JumpState);
        }
    }
}
