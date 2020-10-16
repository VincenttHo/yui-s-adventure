using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : PlayerState
{
    // Start is called before the first frame update
    public float jumpSpeed;
    public Animator anim;
    protected bool isGrounded;
    public Transform groundCheckPointGroup;
    protected Transform[] groundCheckPoints;
    protected Vector2 slopeNormalPerp;
    public LayerMask whatIsGround;
    void Start()
    {
        groundCheckPoints = groundCheckPointGroup.gameObject.GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void onTrigger (PlayerFSM FSM) { 
        anim.Play("Jump");
        FSM.rb.velocity = new Vector2(FSM.rb.velocity.x, jumpSpeed);
    }
    public override void onUpdate (PlayerFSM FSM) { 
        print("Jump");
        CheckGrounded();
        if(isGrounded){
            FSM.SetState(FSM.IdleState);
        }
    }

     protected void CheckGrounded()
    {
        isGrounded = false;
        // isOnSlope = false;
        foreach (Transform groundCheck in groundCheckPoints)
        {
            RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheck.localPosition.y + 0.2f, whatIsGround);
            //RaycastHit2D hit = Physics2D.Linecast(groundCheck.position, transform.position, whatIsGround);
            if (hit)
            {
                slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;
                var slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);
                Debug.DrawRay(hit.point, slopeNormalPerp, Color.blue);
                Debug.DrawRay(hit.point, hit.normal, Color.green);

                // if(slopeDownAngle > 5)
                // {
                //     isOnSlope = true;
                // } else
                // {
                //     isOnSlope = false;
                // }

                isGrounded = true;
                break;
            }
        }

        // 因为上坡的时候有一帧会因为有向上速度导致动画变了一下，所以加上这个限制
        // if(isGrounded && rigi.velocity.y > 0 && !jumpPressing)
        // {
        //     rigi.velocity = new Vector2(rigi.velocity.x, 0);
        // }
    }
}
