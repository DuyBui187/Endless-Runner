using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private Transform target;
    [SerializeField] private Transform camFinishPos;

    [Header(" Settings ")]
    [SerializeField] private float speedCam;
    [SerializeField] private float speedCamChange;

    private Vector3 targetOffset;

    void Start()
    {
        targetOffset = transform.position - target.position;
    }

    void LateUpdate()
    {
        if (GameManager.instance.GetGameState() == GameState.Waiting ||
            GameManager.instance.GetGameState() == GameState.Playing)
            transform.position = Vector3.Lerp(transform.position, target.position + targetOffset, speedCam);

        else transform.position = Vector3.Lerp(transform.position, camFinishPos.position, speedCamChange);
    }
}