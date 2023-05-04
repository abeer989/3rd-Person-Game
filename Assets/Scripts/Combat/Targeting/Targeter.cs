using Cinemachine;
using UnityEngine;
using System.Collections.Generic;

public class Targeter : MonoBehaviour
{
    public Target CurrentTarget { get; private set; }

    [SerializeField] private CinemachineTargetGroup targetGroup;

    private Camera mainCamera;
    private List<Target> targets = new List<Target>();

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void OnTriggerEnter(Collider other)
    {
        Target targetComp = other.gameObject.GetComponent<Target>();

        if (targetComp)
        {
            targets.Add(targetComp);
            targetComp.OnDestroyEvent += RemoveTarget; // subscribe to the target's OnDestroyEvent so that it gets removed from the target group of the target camera
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Target targetComp = other.gameObject.GetComponent<Target>();

        if (targetComp)
        {
            if (targets.Contains(targetComp))
                RemoveTarget(targetComp); // also removing the target when the player moves out of targeting range
        }
    }

    public int GetTargetCount()
    {
        return targets.Count;
    }

    /// <summary>
    /// This function is responsible for selecting the target that is on screen and is nearest to the center of the screen
    /// </summary>
    /// <returns></returns>
    public bool SelectTarget()
    {
        if (targets.Count > 0)
        {
            Target closestTarget = null;
            float closestTargetDistance = Mathf.Infinity;

            foreach (Target target in targets)
            {
                Vector2 targetPosOnScreen = mainCamera.WorldToViewportPoint(target.transform.position);

                if (!target.GetComponentInChildren<MeshRenderer>().isVisible) // if the target is off screen, ignore it
                    continue;

                else
                {
                    Vector2 targetDistanceFromScreenCenter = targetPosOnScreen - /*screen center: */new Vector2(.5f, .5f); // else calculate its dist. from the center of the screen

                    if (targetDistanceFromScreenCenter.sqrMagnitude < closestTargetDistance) // if it's the target closest to the center of the screen set it to current target
                    {
                        closestTarget = target;
                        closestTargetDistance = targetDistanceFromScreenCenter.sqrMagnitude;
                    }
                }
            }

            if (closestTarget)
            {
                CurrentTarget = closestTarget;
                targetGroup.AddMember(t: CurrentTarget.transform, weight: 1, radius: 2);
                return true;
            }

            else
                return false;
        }

        else
            return false;
    }

    /// <summary>
    /// Function resp. for cancelling targeting
    /// </summary>
    public void Cancel()
    {
        if (CurrentTarget)
            targetGroup.RemoveMember(t: CurrentTarget.transform);

        CurrentTarget = null;
    }

    /// <summary>
    /// Removes target from the target group and unsubscribes to its OnDestroy event
    /// </summary>
    /// <param name="target"></param>
    private void RemoveTarget(Target target)
    {
        if (CurrentTarget == target)
        {
            targetGroup.RemoveMember(CurrentTarget.transform);
            CurrentTarget = null;
        }

        target.OnDestroyEvent -= RemoveTarget;
        targets.Remove(target);
    }
}
