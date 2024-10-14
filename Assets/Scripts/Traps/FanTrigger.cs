using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanTrigger : MonoBehaviour
{
    [Header(" Settings ")]
    [SerializeField] private float forceAmount;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("RunnerFL"))
            other.GetComponent<Rigidbody>().AddForce(forceAmount, 0, 0);
    }
}