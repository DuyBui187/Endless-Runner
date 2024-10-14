using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipPanelAnimator : MonoBehaviour
{
    [Header(" Elements ")]
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void EquipItem()
    {
        anim.SetBool("Equipped", false);
    }
}