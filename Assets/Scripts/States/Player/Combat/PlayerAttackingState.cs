public class PlayerAttackingState : PlayerBaseState
{
    private Attack currentAttack;

    private bool alreadyAppliedForce = false;

    public PlayerAttackingState(PlayerStateMachine playerStateMachine, int attackIndex) : base(playerStateMachine) 
    {
        currentAttack = playerStateMachine.Attacks[attackIndex];
    }

    public override void Enter()
    {
        playerStateMachine.Animator.CrossFadeInFixedTime(stateName: currentAttack.AnimationName, fixedTransitionDuration: currentAttack.TransitionDuration);
        playerStateMachine.Weapon.SetAttackProperties(currentAttack.Damage, currentAttack.Knockback); // the playerStateMachine has a ref. to the weapon that the player is holding. On entering the attack state, we'll set the weapon's damage to
                                                                         // whatever the current attack's damage is
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime); // move without input (under the influence of gravity/physics)
        FaceTarget(); // rotate to face target

        float normalizedTime = GetNormalizedAnimationTime(animator: playerStateMachine.Animator, animTag: "Attack");

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
            RevertToLocomotion();
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
}
