using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanController : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private Animator anim;
    [SerializeField] private BoxCollider fanCollider;

    [Header(" Settings ")]
    [SerializeField] private float fanOnTime;
    [SerializeField] private float fanOffTime;

    private bool fanOff;
    private float fanRotateTime;

    void Update()
    {
        fanRotateTime += Time.deltaTime;

        if (fanOff && fanRotateTime > fanOnTime)
            ControlFan(false);

        else if (!fanOff && fanRotateTime > fanOffTime)
            ControlFan(true);
    }

    private void ControlFan(bool isControl)
    {
        anim.SetBool("Rotating", isControl);
        fanCollider.enabled = isControl;

        fanOff = isControl;
        fanRotateTime = 0;
    }
}