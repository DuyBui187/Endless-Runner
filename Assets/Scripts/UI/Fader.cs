using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fader : MonoBehaviour
{
    [Header(" Elements ")]
    private CanvasGroup canvasGroup;
    private Coroutine currentActionFade;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Start()
    {
        Init();
    }

    private void Init()
    {
        FadeOutImmediate();
        StartCoroutine(FadeInCo(2f));
    }

    private void FadeOutImmediate()
    {
        canvasGroup.alpha = 1;
    }

    public IEnumerator FadeOutCo(float time)
    {
        return Fade(1, time);
    }

    public IEnumerator FadeInCo(float time)
    {
        return Fade(0, time);
    }

    private IEnumerator Fade(float target, float time)
    {
        if (currentActionFade != null)
        {
            StopCoroutine(currentActionFade);
        }

        currentActionFade = StartCoroutine(FadeRoutine(target, time));
        yield return currentActionFade;
    }

    private IEnumerator FadeRoutine(float target, float time)
    {
        while (!Mathf.Approximately(canvasGroup.alpha, target))
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target, Time.deltaTime / time);
            yield return null;
        }

        TouchManager.instance.isTouch = true;
    }
}
