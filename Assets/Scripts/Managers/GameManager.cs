using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BonusLibrary;
using UnityEngine.SceneManagement;

public enum BonusType { Addition, Difference, Product, Division }

public enum GameState { Waiting, Playing, Finish, Result }

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header(" Elements ")]
    [SerializeField] private Transform runnerFLs;
    [SerializeField] private Transform spawnEffects;
    [SerializeField] private Transform deadEffects;
    [SerializeField] private Transform splashEffects;

    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform targetPoint;

    [SerializeField] private Animator runnerAnim;

    [SerializeField] private GameObject[] hats;
    [SerializeField] private GameObject[] weapons;
    [SerializeField] private Material[] colors;

    [SerializeField] private SkinnedMeshRenderer rendererCharacter;

    [Header(" Enemies ")]
    [SerializeField] private Transform enemies;
    [SerializeField] private int enemyCount;

    [Header(" Settings ")]
    private GameState currentState;

    private int runnerFLCount = 1;

    private const string keyScore = "ScoreKey";
    private const string keyLevel = "LevelKey";

    private const string keyHat = "HatKey";
    private const string keyWeapon = "WeaponKey";
    private const string keyColor = "ColorKey";

    [Header(" Library ")]
    LibraryMath libraryMath = new LibraryMath();
    DataManage dataManage = new DataManage();
    AdvertisingManage advertisingManage = new AdvertisingManage();

    void Awake()
    {
        if (instance == null)
            instance = this;

        else Destroy(gameObject);
    }

    void Start()
    {
        Init();
    }

    private void Init()
    {
        AudioManager.instance.PlayMusic(1);

        ChangeGameState(GameState.Waiting);
        ActiveEnemiesFromCountValue();
        LoadItemData();

        advertisingManage.LoadInterstitialAd();
    }

    private void LoadItemData()
    {
        if (dataManage.GetData_Int(keyHat) != -1)
            hats[dataManage.GetData_Int(keyHat)].SetActive(true);

        if (dataManage.GetData_Int(keyWeapon) != -1)
            hats[dataManage.GetData_Int(keyWeapon)].SetActive(true);

        if (dataManage.GetData_Int(keyColor) != -1)
        {
            Material[] mats = rendererCharacter.materials;
            mats[0] = colors[dataManage.GetData_Int(keyColor)];
            rendererCharacter.materials = mats;
        }
    }

    private void ActiveEnemiesFromCountValue()
    {
        for (int i = 0; i < enemyCount; i++)
            enemies.GetChild(i).gameObject.SetActive(true);
    }

    public void EnemiesRunningTowardsTarget()
    {
        foreach (Transform enemy in enemies)
            if (enemy.gameObject.activeInHierarchy)
                enemy.GetComponent<EnemyController>().RunTowardsTarget();
    }

    public void ApplyBonus(BonusType bonusType, int bonusAmount)
    {
        switch (bonusType)
        {
            case BonusType.Addition:
                libraryMath.Addition(runnerFLs, bonusAmount, spawnPoint);
                break;

            case BonusType.Difference:
                libraryMath.Difference(runnerFLs, bonusAmount);
                break;

            case BonusType.Product:
                libraryMath.Product(runnerFLs, bonusAmount, spawnPoint);
                break;

            case BonusType.Division:
                libraryMath.Division(runnerFLs, bonusAmount);
                break;
        }
    }

    public void CalculateRunnerFLCount(BonusType bonusType, int bonusAmount)
    {
        switch (bonusType)
        {
            case BonusType.Addition:
                runnerFLCount += bonusAmount;
                break;

            case BonusType.Difference:
                runnerFLCount -= bonusAmount;
                CheckToActiveRunnerFLs();
                break;

            case BonusType.Product:
                runnerFLCount *= bonusAmount;
                break;

            case BonusType.Division:
                runnerFLCount -= bonusAmount;
                CheckToActiveRunnerFLs();
                break;
        }
    }

    private void CheckToActiveRunnerFLs()
    {
        if (runnerFLCount <= runnerFLs.childCount + 1) return;

        foreach (Transform runnerFL in runnerFLs)
        {
            if (!runnerFL.gameObject.activeInHierarchy)
            {
                runnerFL.position = spawnPoint.position;
                runnerFL.gameObject.SetActive(true);
            }
        }
    }

    public void ActiveDeadEffects(Transform deadPos, bool isHammer = false)
    {
        foreach (Transform deadEffect in deadEffects)
        {
            if (!deadEffect.gameObject.activeInHierarchy)
            {
                deadEffect.position = deadPos.position;
                deadEffect.gameObject.SetActive(true);
                deadEffect.GetComponent<ParticleSystem>().Play();

                AudioManager.instance.PlaySFX(1);

                break;
            }
        }

        if (isHammer)
            ActiveSplashEffects(deadPos);
    }

    public void ActiveSpawnEffects(Transform spawnPos)
    {
        foreach (Transform spawnEffect in spawnEffects)
        {
            if (!spawnEffect.gameObject.activeInHierarchy)
            {
                spawnEffect.position = spawnPos.position;
                spawnEffect.gameObject.SetActive(true);
                spawnEffect.GetComponent<ParticleSystem>().Play();

                AudioManager.instance.PlaySFX(0);

                break;
            }
        }
    }

    public void ActiveSplashEffects(Transform spawnPos)
    {
        foreach (Transform splash in splashEffects)
        {
            if (!splash.gameObject.activeInHierarchy)
            {
                splash.position = new Vector3(spawnPos.position.x,
                    splash.position.y, spawnPos.position.z);

                splash.gameObject.SetActive(true);

                break;
            }
        }
    }

    public void ChangeGameState(GameState gameState)
    {
        currentState = gameState;

        switch (gameState)
        {
            case GameState.Waiting:
                runnerAnim.SetBool("Running", false);
                break;

            case GameState.Playing:
                runnerAnim.SetBool("Running", true);
                break;

            case GameState.Finish:

                EnemiesRunningTowardsTarget();
                CheckRunnerFLCount();
                break;

            case GameState.Result:
                break;
        }
    }

    public void ResetRunnerFLCount()
    {
        runnerFLCount = 1;
    }

    public void AddRunners()
    {
        runnerFLCount++;
    }

    public void RemoveRunners()
    {
        runnerFLCount--;

        CheckToActiveRunnerFLs();

        if (currentState == GameState.Finish)
            CheckRunnerFLCount();
    }

    private void CheckRunnerFLCount()
    {
        if (currentState == GameState.Result) return;

        if (runnerFLCount <= 1 || enemyCount == 0)
        {
            ChangeGameState(GameState.Result);

            advertisingManage.ShowInterstitialAd();

            foreach (Transform enemy in enemies)
                if (enemy.gameObject.activeInHierarchy)
                    enemy.GetComponent<EnemyController>().IdleAnim();

            foreach (Transform runnerFL in runnerFLs)
                if (runnerFL.gameObject.activeInHierarchy)
                    runnerFL.GetComponent<IRunnerAnim>().IdleAnim();

            runnerAnim.SetBool("Running", false);

            if (runnerFLCount <= enemyCount)
                UIManager.instance.losePanel.SetActive(true);

            else
            {
                if (runnerFLCount > 5)
                    dataManage.SaveData_Int(keyScore, dataManage.GetData_Int(keyScore) + 600);

                else dataManage.SaveData_Int(keyScore, dataManage.GetData_Int(keyScore) + 200);

                int currentLevel = dataManage.GetData_Int(keyLevel) - 4;

                if (SceneManager.GetActiveScene().buildIndex == dataManage.GetData_Int(keyLevel))
                    if (currentLevel < 5)
                        dataManage.SaveData_Int(keyLevel, dataManage.GetData_Int(keyLevel) + 1);

                UIManager.instance.winPanel.SetActive(true);
            }
        }
    }

    public void AddRunnerBonusIntoRunnerFLs(Transform runnerBonus)
    {
        runnerBonus.parent = runnerFLs;
    }

    public void RemoveEnemies()
    {
        enemyCount--;

        if (currentState == GameState.Finish)
            CheckRunnerFLCount();
    }

    public GameState GetGameState()
    {
        return currentState;
    }

    public int GetRunnerFLCount()
    {
        return runnerFLCount;
    }

    public Transform GetTargetPoint()
    {
        return targetPoint;
    }
}