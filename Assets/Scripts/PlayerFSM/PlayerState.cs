using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour {
    public virtual void onTrigger (PlayerFSM FSM) { }
    public virtual void onUpdate (PlayerFSM FSM) { }
}
