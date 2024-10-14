using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private Animator anim;
    [SerializeField] private NavMeshAgent navMeshAgent;

    private Transform target;

    [Header(" Settings ")]
    private bool isRunning;

    void Start()
    {
        target = GameManager.instance.GetTargetPoint();
    }

    void LateUpdate()
    {
        if (isRunning)
            navMeshAgent.SetDestination(target.position);

        else navMeshAgent.SetDestination(transform.position);
    }

    public void RunTowardsTarget()
    {
        anim.SetBool("Running", true);
        isRunning = true;
    }

    public void IdleAnim()
    {
        anim.SetBool("Running", false);
        isRunning = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RunnerFL"))
        {
            GameManager.instance.RemoveEnemies();
            GameManager.instance.ActiveDeadEffects(transform);

            gameObject.SetActive(false);
        }
    }
}
