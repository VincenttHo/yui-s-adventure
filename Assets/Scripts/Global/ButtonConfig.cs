using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * 按键控制器
 * @author VincentHo
 * @date 2020-08-16
 * TODO 先写死，后面再做按键设置
 */
public class ButtonConfig : MonoBehaviour
{

    public static ButtonConfig key;

    // 上
    public KeyCode up = KeyCode.W;
    // 下
    public KeyCode down = KeyCode.S;
    // 左
    public KeyCode left = KeyCode.A;
    // 右
    public KeyCode right = KeyCode.D;
    // 主武器
    public KeyCode mainWeapon = KeyCode.Y;
    // 副武器
    public KeyCode subWeapon = KeyCode.H;
    // 跳跃
    public KeyCode jump = KeyCode.U;
    // 冲刺
    public KeyCode dash = KeyCode.I;

    private void Start()
    {
        key = this;
    }

}
