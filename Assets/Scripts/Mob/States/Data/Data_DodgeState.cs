using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newDodgeStateData", menuName = "Data/State Data/Dodge State")]

public class Data_DodgeState : ScriptableObject
{
    // vars for adjusting speed
    // jump angle of the dodge
    // cooldown durration and timer
    public float dodgeSpeed = 1.0f;
    public Vector2 dodgeAngle;
    public float dodgeTimer = 0.3f;
    public float dodgeCoolDwn = 2.0f;

    // dodge SFX
    [SerializeField] public AudioClip _dodgeSFX;
}
