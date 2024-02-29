using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTags : MonoBehaviour
{
   // public const string MOVEMENT = "Steve_Walk";
    public const string MOVEMENT = "playerIsClose";
    public const string ATTACK_1_TRIGGER = "attack1T";
    public const string ATTACK_2_TRIGGER = "attack2T";

    public const string IDLE_ANIMATION = "Steve_Idle";

    public const string HIT_TRIGGER = "hurt";
    //public const string STUN_TRIGGER = "Stun";

    public const string DEATH_TRIGGER = "isDead";
}

public class Axis
{
    public const string HORIZONTAL_AXIS = "Horizontal";
}