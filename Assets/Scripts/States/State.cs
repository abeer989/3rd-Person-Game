using UnityEngine;

/// <summary>
/// The very base blueprint for a state that contains Enter, Tick and Exit functions.
/// </summary>
public abstract class State
{
    private const string attackTag = "Attack";

    public abstract void Enter();
    public abstract void Tick(float deltaTime);
    public abstract void Exit();

    /// <summary>
    /// This function will get us how far we are into an animation
    /// </summary>
    /// <returns></returns>
    protected float GetNormalizedAttackAnimationTime(Animator animator)
    {
        AnimatorStateInfo currentStateInfo = animator.GetCurrentAnimatorStateInfo(0); // current anim state info
        AnimatorStateInfo nextStateInfo = animator.GetNextAnimatorStateInfo(0); // next anim state info

        // if we're transtion into another attack anim:
        if (animator.IsInTransition(0) && nextStateInfo.IsTag(attackTag))
            return nextStateInfo.normalizedTime; // then return the normalized anim time for the next anim state

        // else if, we're in a partiular attack anim:
        else if (!animator.IsInTransition(0) && currentStateInfo.IsTag(attackTag))
            return currentStateInfo.normalizedTime; // then return the normalized anim time for the current anim state

        else
            return 0;
    }
}
