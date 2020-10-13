using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    protected PlayerBehaviour playerBehaviour;

    protected void Start()
    {
        playerBehaviour = GetComponent<PlayerBehaviour>();
    }

    protected void Update()
    {
        if (ButtonConfig.key == null) return;
        if (!GlobalConstant.canControl)
        {
            playerBehaviour.Reset();
            return;
        }
        // 行走
        if (Input.GetKey(ButtonConfig.key.left))
        {
            playerBehaviour.SetHorizontalInput(-1);
        }
        else if (Input.GetKey(ButtonConfig.key.right))
        {
            playerBehaviour.SetHorizontalInput(1);
        }
        else
        {
            playerBehaviour.SetHorizontalInput(0);
        }

        // 跳跃
        if (!Input.GetKey(ButtonConfig.key.down))
        {
            if (Input.GetKey(ButtonConfig.key.jump)) playerBehaviour.JumpPressing();
            if (Input.GetKeyUp(ButtonConfig.key.jump)) playerBehaviour.JumpRelease();
        }
    }
}
