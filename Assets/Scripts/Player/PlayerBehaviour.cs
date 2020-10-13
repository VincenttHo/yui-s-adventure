using System;
using UnityEngine;

/**
 * 主角控制器
 * @author VincentHo
 * @date 2020-08-26
 */
public class PlayerBehaviour : MonoBehaviour
{

    /** 组件 */
    [HideInInspector]
    public Rigidbody2D rigi;
    protected Animator anim;

    /** 水平移动相关参数 */
    // 公开参数
    [Header("跑步速度")]
    public float runSpeed;
    // 私有参数
    [Header("水平方向运动值（1：向右，-1：向左，0：静止）")]
    protected float horizontalInput;

    /** 跳跃相关参数 */
    // 公开参数
    [Header("跳跃速度")]
    public float jumpSpeed;
    [Header("跳跃最大持续时间，用于长按短按跳跃")]
    public float jumpDurationTime;
    [Header("跳跃最大段数")]
    public int jumpMaxTimes;
    // 私有参数
    [Header("跳跃键是否持续按压")]
    protected bool jumpPressing;
    [Header("是否放开跳跃键")]
    protected bool jumpPressExit;
    [Header("是否可跳跃")]
    protected bool canJump;
    [Header("跳跃持续时间")]
    protected float jumpSec;
    [Header("当前跳跃次数")]
    protected int jumpCurrentTimes;

    /** 着地检测相关参数 */
    // 公开参数
    [Header("地面检测点组")]
    public Transform groundCheckPointGroup;
    [Header("检测的地面层")]
    public LayerMask whatIsGround;
    // 私有参数
    [Header("着地判断")]
    protected bool isGrounded;
    [Header("是否在斜坡上")]
    protected bool isOnSlope;
    protected Transform[] groundCheckPoints;
    protected Vector2 slopeNormalPerp;

    /** 物理材质（用于爬斜坡防止下滑） */
    // 公开参数
    [Header("满摩擦力材质")]
    public PhysicsMaterial2D fullFriction;
    [Header("无摩擦力材质")]
    public PhysicsMaterial2D noFriction;

    protected void Start()
    {
        canJump = true;
        jumpSec = 0f;
        rigi = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        groundCheckPoints = groundCheckPointGroup.gameObject.GetComponentsInChildren<Transform>();
    }

    protected void Update()
    {
        ChangeAnimation();
    }

    protected void FixedUpdate()
    {
        if (!GlobalConstant.canControl) return;
        CheckGrounded();
        Movement(runSpeed, horizontalInput);
        Jump();
        Filp();
    }

    /**
     * 移动
     */
    protected void Movement(float moveSpeed, float direction)
    {
        // 站在平地上时正常横向运动
        if (isGrounded && !isOnSlope)
        {
            rigi.velocity = new Vector2(moveSpeed * direction, rigi.velocity.y);
        }
        // 站在斜坡上时根据斜坡限制速度
        else if (isGrounded && isOnSlope)
        {
            var x = Math.Abs(moveSpeed * slopeNormalPerp.x) * direction;
            var y = slopeNormalPerp.x > 0 ? moveSpeed * slopeNormalPerp.y * direction : moveSpeed * slopeNormalPerp.y * -direction;
            rigi.velocity = new Vector2(x, y);
        }
        // 浮空时
        else if (!isGrounded)
        {
            rigi.velocity = new Vector2(moveSpeed * direction, rigi.velocity.y);
        }

        // 如果在斜坡上且不走动时把摩擦力拉满防止下滑
        if (isOnSlope && direction == 0.0f)
        {
            rigi.sharedMaterial = fullFriction;
        }
        else
        {
            rigi.sharedMaterial = noFriction;
        }


    }

    /**
     * 动画处理
     */
    protected void ChangeAnimation()
    {
        // 水平移动
        anim.SetInteger("horizontalInput", (int)horizontalInput);
        // 着地
        anim.SetBool("isGrounded", isGrounded);
        // 跳跃
        anim.SetFloat("verticalSpeed", rigi.velocity.y);

    }

    /**
     * 转身
     */
    protected void Filp()
    {
        if(horizontalInput > 0)
        {
            transform.localScale = new Vector3(
                Math.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z);
        }
        else if(horizontalInput < 0)
        {
            transform.localScale = new Vector3(
                -Math.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z);
        }
    }

    /**
     * 跳跃
     * @description 实现短按跳跃，长按跳跃以及多段跳跃
     * 参数说明：
     *  canJump：用于单次跳跃到了最高点后判断是否仍然可以向上跳跃的开关
     *  jumpSec：计算单次跳跃所用时间
     *  jumpCurrentTimes：计算总共跳跃次数，用于多段跳跃
     */
    protected void Jump()
    {
        if (Input.GetKey(ButtonConfig.key.down)) return;
        // 着地重置跳跃参数，如果仍然按住跳跃键则不重置，避免按住跳跃键会掉到地面上后又跳起来
        if (!jumpPressing && isGrounded)
        {
            canJump = true;
            jumpSec = 0f;
            jumpCurrentTimes = 0;
        }
        // 放开跳跃键参数处理
        if (jumpPressExit)
        {
            jumpPressExit = false;
            //若不是跳跃最高点，则跳跃次数 + 1（因为最高点的时候跳跃次数会 + 1,无此判断会重复增加跳跃次数）
            if (jumpSec < jumpDurationTime)
            {
                jumpCurrentTimes++;
            }
            canJump = true;
            jumpSec = 0f;
        }

        // 跳跃逻辑
        if (jumpCurrentTimes < jumpMaxTimes)
        {
            if (jumpPressing && canJump)
            {
                rigi.velocity = new Vector2(rigi.velocity.x, jumpSpeed);
                if (jumpSec < jumpDurationTime)
                {
                    jumpSec += Time.deltaTime;
                }
                else
                {
                    jumpCurrentTimes++;
                    canJump = false;
                }
            }
        }
    }

    /**
     * 着地检测
     * @description 使用射线的方式检测是否着地
     */
    protected void CheckGrounded()
    {
        isGrounded = false;
        isOnSlope = false;
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

                if(slopeDownAngle > 5)
                {
                    isOnSlope = true;
                } else
                {
                    isOnSlope = false;
                }

                isGrounded = true;
                break;
            }
        }

        // 因为上坡的时候有一帧会因为有向上速度导致动画变了一下，所以加上这个限制
        if(isGrounded && rigi.velocity.y > 0 && !jumpPressing)
        {
            rigi.velocity = new Vector2(rigi.velocity.x, 0);
        }
    }






    /** ==================公开方法============================= */

    /**
     * 获取当前方向
     * @description -1：左，1：右
     */
    public float GetDirection()
    {
        return transform.localScale.x / Math.Abs(transform.localScale.x);
    }

    /**
     * 重设水平速度
     * @description 用于禁止操作时用（如受伤硬直等），y轴保持速度是为了可以下落不导致浮空
     */
    public void Reset()
    {
        rigi.velocity = new Vector2(0, rigi.velocity.y);
    }

    /**
     * 水平移动设置
     */
    public void SetHorizontalInput(int horizontalInput)
    {
        this.horizontalInput = horizontalInput;
    }

    /**
     * 按下跳跃
     */
    public void JumpPressing()
    {
        this.jumpPressing = true;
    }

    /**
     * 释放跳跃
     */
    public void JumpRelease()
    {
        this.jumpPressing = false;
        this.jumpPressExit = true;
    }

}
