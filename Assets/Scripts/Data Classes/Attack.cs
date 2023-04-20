using System;
using UnityEngine;

[Serializable]
public class Attack
{
    [field: SerializeField] public string AnimationName { get; private set; } // the name of the attack animation
    [field: SerializeField] public float TransitionDuration { get; private set; } // how fast we'll transit into an attack's anim
    [field: SerializeField] public int NextAttackStateIndex { get; private set; } = -1; // this will determine what the attack's index/position is in a combo. Is it the first hit, the second hit, etc.
    [field: SerializeField] public float AttackAnimationCompletionThresh { get; private set; } // how much will the anim play before you transition into the next anim. For example, play one anim 80% of the way and then transit into the next
    [field: SerializeField] public float Force { get; private set; } // the force this attack will apply to the player's body
    [field: SerializeField] public float ApplyForceThresh { get; private set; } // the anim time thresh when force will be applied
    [field: SerializeField] public int Damage { get; private set; } // the anim time thresh when force will be applied
    [field: SerializeField] public float Knockback { get; private set; } // the anim time thresh when force will be applied
}
