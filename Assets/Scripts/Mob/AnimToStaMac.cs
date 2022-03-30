using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this script passes information between the animator
// and the state machine the two are on seperate game objects
// and so they can't communicate directly
public class AnimToStaMac : MonoBehaviour
{
    public AttackState attackState;

    private void StartAttack()
    {
        attackState.StartAttack();
    }

    private void FinishAttack()
    {
        attackState.FinishAttack();
    }
}
