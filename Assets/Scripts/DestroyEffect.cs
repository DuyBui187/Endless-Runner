using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEffect : MonoBehaviour
{
    [Header(" Settings ")]
    [SerializeField] private float destroyTime;

    void Start()
    {
        StartCoroutine(DeactiveEffectCo());
    }

    IEnumerator DeactiveEffectCo()
    {
        yield return new WaitForSeconds(destroyTime);

        gameObject.SetActive(false);
    }
}
