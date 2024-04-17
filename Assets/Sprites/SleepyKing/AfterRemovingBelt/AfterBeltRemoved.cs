using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Yarn.Unity;

public class AfterBeltRemoved : MonoBehaviour
{
    [SerializeField] Animator sleepyAnimator;
    [YarnCommand("RemoveBeltFromSprite")]
    public void RemoveBeltFromSprite() {
        sleepyAnimator.SetBool("RemovedBelt", true);
    }
}
