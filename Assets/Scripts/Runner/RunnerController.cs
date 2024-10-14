using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunnerController : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private Rigidbody rig;
    [SerializeField] private Transform arenaPos;
    [SerializeField] private Transform finishArena;

    [Header(" Settings ")]
    [SerializeField] private float verticalSpeed;
    [SerializeField] private float horizontalSpeed;

    [Header(" UI ")]
    [SerializeField] private Slider sliderFinish;

    void Start()
    {
        float distance = Vector3.Distance(transform.position, finishArena.position);
        sliderFinish.maxValue = distance;
    }

    void FixedUpdate()
    {
        if (GameManager.instance.GetGameState() != GameState.Playing)
        {
            rig.velocity = Vector3.zero;

            if (GameManager.instance.GetGameState() == GameState.Finish ||
                GameManager.instance.GetGameState() == GameState.Result)
            {
                transform.position = Vector3.Lerp(transform.position, arenaPos.position, .015f);

                if (sliderFinish.value != 0)
                    sliderFinish.value -= 0.005f;
            }

            return;
        }

        float distance = Vector3.Distance(transform.position, finishArena.position);
        sliderFinish.value = distance;

        transform.Translate(Vector3.forward * verticalSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (Input.GetAxis("Mouse X") < 0)
                MoveHorizontal(-horizontalSpeed);

            else if (Input.GetAxis("Mouse X") > 0)
                MoveHorizontal(horizontalSpeed);
        }

        else rig.velocity = Vector3.zero;
    }

    private void MoveHorizontal(float speedValue)
    {
        Vector3 dir = new Vector3(
            transform.position.x + speedValue, transform.position.y, transform.position.z);

        dir *= Time.deltaTime;

        dir.y = rig.velocity.y;
        dir.z = 0;

        rig.velocity = dir;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
            GameManager.instance.ChangeGameState(GameState.Finish);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("StickGlass") ||
            collision.gameObject.CompareTag("Obstacle"))
        {
            if (transform.position.x > 0)
                transform.position = new Vector3(transform.position.x - .2f, transform.position.y,
                    transform.position.z);

            else
                transform.position = new Vector3(transform.position.x + .2f, transform.position.y,
                    transform.position.z);
        }
    }
}