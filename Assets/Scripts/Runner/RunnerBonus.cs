using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RunnerBonus : MonoBehaviour, IRunnerAnim
{
    [Header(" Elements ")]
    [SerializeField] private Animator anim;
    [SerializeField] private NavMeshAgent navMeshAgent;

    [SerializeField] private SkinnedMeshRenderer rendererRunnerBN;
    [SerializeField] private Material materialRunner;

    private Transform target;

    [Header(" Settings ")]
    private bool isRunning;

    void Start()
    {
        target = GameManager.instance.GetTargetPoint();
    }

    void Update()
    {
        if (isRunning)
            navMeshAgent.SetDestination(target.position);

        else navMeshAgent.SetDestination(transform.position);
    }

    private void ChangeToRunnerFL()
    {
        Material[] mats = rendererRunnerBN.materials;
        mats[0] = materialRunner;

        rendererRunnerBN.materials = mats;

        RunTowardsTarget();
        AddRunnerBonusIntoRunnerFL();
    }

    private void AddRunnerBonusIntoRunnerFL()
    {
        GameManager.instance.AddRunners();
        GameManager.instance.AddRunnerBonusIntoRunnerFLs(transform);

        gameObject.tag = "RunnerFL";
    }

    private void RunTowardsTarget()
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
        if (other.CompareTag("RunnerFL") || other.CompareTag("Player"))
        {
            if (gameObject.CompareTag("RunnerBonus"))
            {
                ChangeToRunnerFL();

                AudioManager.instance.PlaySFX(0);
            }
        }

        else if (other.CompareTag("Obstacle"))
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
