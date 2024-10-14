using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager instance;

    [Header(" Elements ")]
    [SerializeField] private GameObject loadingPanel;

    [Header(" UI ")]
    [SerializeField] private Slider loadingSlider;
    [SerializeField] private TextMeshProUGUI progressText;

    void Awake()
    {
        if (instance == null)
            instance = this;

        else Destroy(gameObject);
    }

    public void LoadScene(int sceneIndex)
    {
        loadingPanel.SetActive(true);

        StartCoroutine(LoadAsync(sceneIndex));
    }

    IEnumerator LoadAsync(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        yield return new WaitForSeconds(3);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            loadingSlider.value = progress;
            progressText.text = (progress * 100f).ToString("F0") + "%";

            yield return new WaitForEndOfFrame();
        }
    }
}