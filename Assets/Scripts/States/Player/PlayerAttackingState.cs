using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    private Attack currentAttack;

    private const string attackTag = "Attack";
    private bool alreadyAppliedForce = false;

    public PlayerAttackingState(PlayerStateMachine playerStateMachine, int attackIndex) : base(playerStateMachine) 
    {
        currentAttack = playerStateMachine.Attacks[attackIndex];
    }

    public override void Enter()
    {
        playerStateMachine.Animator.CrossFadeInFixedTime(stateName: currentAttack.AnimationName, fixedTransitionDuration: currentAttack.TransitionDuration);
        playerStateMachine.Weapon.SetAttackDamage(currentAttack.Damage); // the playerStateMachine has a ref. to the weapon that the player is holding. On entering the attack state, we'll set the weapon's damage to
                                                                         // whatever the current attack's damage is
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime); // move without input (under the influence of gravity/physics)
        FaceTarget(); // rotate to face target

        float normalizedTime = GetNormalizedAttackAnimationTime();

        // if the normalized time for the current animation that's playing is greater than the prev. anim's time (meaning we've transitioned into a new state)
        // and if the curren anim. hasn't played fully:
        if (normalizedTime < 1)
        {
            if (normalizedTime >= currentAttack.ApplyForceThresh)
                TryApplyForce();

            if (playerStateMachine.InputReader.IsAttacking)
                TryComboAttack(normalizedTime);
        }

        else
            RevertToPreviousState();
    }

    public override void Exit()
    {

    }

    /// <summary>
    /// This function is responsible for playing attack animations one after another (combo)
    /// </summary>
    /// <param name="normalizedTime"></param>
    private void TryComboAttack(float normalizedTime)
    {
        if(currentAttack.NextAttackStateIndex == -1) { return; } // if the current attack doesn't link to another attack ahead in the combo, do nothing

        if(normalizedTime < currentAttack.AttackAnimationCompletionThresh) { return; } // if the current attack anim hasn't crossed the comp thresh, do nothing

        playerStateMachine.SwitchState(new PlayerAttackingState(playerStateMachine, currentAttack.NextAttackStateIndex)); // else switch to the attack state with the new attack's index (play next attack anim.)
    }

    private void TryApplyForce()
    {
        if (!alreadyAppliedForce)
        {
            playerStateMachine.ForceReceiver.AddImpactForce(playerStateMachine.transform.forward * currentAttack.Force);
            alreadyAppliedForce = true;
        }
    }

    /// <summary>
    /// This function will get us how far we are into an animation
    /// </summary>
    /// <returns></returns>
    private float GetNormalizedAttackAnimationTime()
    {
        AnimatorStateInfo currentStateInfo = playerStateMachine.Animator.GetCurrentAnimatorStateInfo(0); // current anim state info
        AnimatorStateInfo nextStateInfo = playerStateMachine.Animator.GetNextAnimatorStateInfo(0); // next anim state info

        // if we're transtion into another attack anim:
        if (playerStateMachine.Animator.IsInTransition(0) && nextStateInfo.IsTag(attackTag))
            return nextStateInfo.normalizedTime; // then return the normalized anim time for the next anim state

        // else if, we're in a partiular attack anim:
        else if (!playerStateMachine.Animator.IsInTransition(0) && currentStateInfo.IsTag(attackTag))
            return currentStateInfo.normalizedTime; // then return the normalized anim time for the current anim state

        else
            return 0;
    }
}
