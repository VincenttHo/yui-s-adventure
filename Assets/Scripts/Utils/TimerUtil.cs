using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

/**
 * 定时执行工具
 */
public class TimerUtil : MonoBehaviour
{

    private static float timer;

    public static void ExecuteAfterSec(float waitSec, UnityAction call)
    {
        if (timer < waitSec)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0f;
            call.Invoke();
        }
    }

}
