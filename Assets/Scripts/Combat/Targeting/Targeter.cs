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
            targetComp.OnDestroyEvent += RemoveTarget;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Target targetComp = other.gameObject.GetComponent<Target>();

        if (targetComp)
        {
            if (targets.Contains(targetComp))
                RemoveTarget(targetComp);
        }
    }

    public int GetTargetCount()
    {
        return targets.Count;
    }

    public bool SelectTarget()
    {
        if (targets.Count > 0)
        {
            Target closestTarget = null;
            float closestTargetDistance = Mathf.Infinity;

            foreach (Target target in targets)
            {
                Vector2 targetPosOnScreen = mainCamera.WorldToViewportPoint(target.transform.position);

                if (targetPosOnScreen.x < 0 || targetPosOnScreen.x > 1 || targetPosOnScreen.y < 0 || targetPosOnScreen.y > 1)
                    continue;

                else
                {
                    Vector2 targetDistanceFromScreenCenter = targetPosOnScreen - /*screen center: */new Vector2(.5f, .5f);

                    if (targetDistanceFromScreenCenter.sqrMagnitude < closestTargetDistance)
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

    public void Cancel()
    {
        if (CurrentTarget)
            targetGroup.RemoveMember(t: CurrentTarget.transform);

        CurrentTarget = null;
    }

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
