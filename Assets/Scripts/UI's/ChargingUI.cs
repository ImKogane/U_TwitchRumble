using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChargingUI : MonoBehaviour
{
    public float durationOfFadeIn = 1.5f;
    public float durationOfFadeOut = 1.5f;
    public float durationOfFadeOutBar = 1.5f;

    private float compteur = 0;

    [Header("UI variables")]
    public Slider slider;

    public CanvasGroup canvasGroupBackGround;
    public CanvasGroup canvasGroup;

    public Image _backGroundImage;

    public Camera cam;

    public bool firstFadeIn = true;

    public void AdaptSlider(float value, float valueMax)
    {
        compteur += value;
        float progress = Mathf.Clamp01(compteur / valueMax);
        slider.value = progress;

        if (slider.value == 1)
        {
            StartCoroutine(FadeOutCoroutine());
        }
    }

    #region Get out of UI
    public IEnumerator FadeOutCoroutine()
    {
        float normalizedTime = 0;

        canvasGroup.alpha = 1;

        cam.gameObject.SetActive(false);

        while (normalizedTime <= durationOfFadeOutBar)
        {
            canvasGroup.alpha = (1 - Mathf.Clamp01(normalizedTime / durationOfFadeOutBar));

            normalizedTime += Time.deltaTime / durationOfFadeOutBar;
            yield return null;
        }

        normalizedTime = 0;

        //BackGround fade Out
        while (normalizedTime <= durationOfFadeOut)
        {
            canvasGroupBackGround.alpha = (1 - Mathf.Clamp01(normalizedTime / durationOfFadeOut));

            normalizedTime += Time.deltaTime / durationOfFadeOut;
            yield return null;
        }

        canvasGroup.alpha = 0;
        canvasGroupBackGround.alpha = 0;

        compteur = 0;
        slider.value = 0;
        gameObject.SetActive(false);

    }
    #endregion

    #region Get In UI
    public IEnumerator FadeInCoroutine(List<string> ScenesOfNextLevel)
    {
        cam.gameObject.SetActive(true);
        gameObject.SetActive(true);

        canvasGroup.alpha = 0;

        float normalizedTime = 0;

        if (!firstFadeIn)
        {
            //BackGround fade In
            while (normalizedTime <= durationOfFadeIn)
            {
                canvasGroupBackGround.alpha = Mathf.Clamp01(normalizedTime / durationOfFadeIn);
                canvasGroup.alpha = Mathf.Clamp01(normalizedTime / durationOfFadeIn);
                normalizedTime += Time.deltaTime / durationOfFadeIn;
                yield return null;
            }
        }
        else
        {
            canvasGroupBackGround.alpha =  1;
            firstFadeIn = false;
        }

        canvasGroupBackGround.alpha = 1;
        canvasGroup.alpha = 1;

        StartCoroutine(BootstrapManager.Instance.ChargeLvl(ScenesOfNextLevel));
    }
    #endregion
}
