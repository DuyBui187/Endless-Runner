using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RunnerFollow : MonoBehaviour, IRunnerAnim
{
    [Header(" Elements ")]
    [SerializeField] private Animator anim;
    [SerializeField] private NavMeshAgent navMeshAgent;

    private Transform target;

    [Header(" Settings ")]
    private bool isRunning;

    void Start()
    {
        Init();
    }

    private void Init()
    {
        target = GameManager.instance.GetTargetPoint();
        anim.SetBool("Running", true);

        isRunning = true;
    }

    void Update()
    {
        if (isRunning)
            navMeshAgent.SetDestination(target.position);

        else navMeshAgent.SetDestination(transform.position);
    }

    public void IdleAnim()
    {
        anim.SetBool("Running", false);
        isRunning = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            GameManager.instance.RemoveRunners();

            if (other.transform.parent.name == "Hammer")
                GameManager.instance.ActiveDeadEffects(transform, true);

            else GameManager.instance.ActiveDeadEffects(transform);

            gameObject.SetActive(false);
        }

        else if (other.CompareTag("Enemy"))
        {
            GameManager.instance.RemoveRunners();
            GameManager.instance.ActiveDeadEffects(transform);

            gameObject.SetActive(false);
        }
    }
}
