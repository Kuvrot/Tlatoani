using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEventListener : MonoBehaviour
{
    [HideInInspector] public UnityEvent footStepEvent = new UnityEvent();
    [HideInInspector] public UnityEvent attackEvent = new UnityEvent();

    public void FootstepEvent()
    {
        footStepEvent.Invoke();
    }
    public void AttackEvent()
    {
        attackEvent.Invoke();
    }
}
