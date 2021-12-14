using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChargingUI : MonoBehaviour
{
    public float durationOfFadeIn = 1.5f;
    public float durationOfFadeOut = 1.5f;

    private float compteur = 0;

    [Header("UI variables")]
    public Slider slider;

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

        while (normalizedTime <= durationOfFadeOut)
        {
            canvasGroup.alpha = (1 - Mathf.Clamp01(normalizedTime / durationOfFadeOut));

            normalizedTime += Time.deltaTime / durationOfFadeOut;
            yield return null;
        }

        normalizedTime = 0;

        //BackGround fade Out
        while (normalizedTime <= durationOfFadeOut)
        {
            var tempColor = _backGroundImage.color;
            tempColor.a = 1 - Mathf.Clamp01(normalizedTime / durationOfFadeOut);
            _backGroundImage.color = tempColor;

            normalizedTime += Time.deltaTime / durationOfFadeOut;
            yield return null;
        }

        canvasGroup.alpha = 0;

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
            var tempColor = _backGroundImage.color;
            tempColor.a = 0;
            _backGroundImage.color = tempColor;

            //BackGround fade In
            while (normalizedTime <= durationOfFadeIn)
            {
                tempColor = _backGroundImage.color;
                tempColor.a = Mathf.Clamp01(normalizedTime / durationOfFadeIn);
                _backGroundImage.color = tempColor;

                normalizedTime += Time.deltaTime / durationOfFadeIn;
                yield return null;
            }

            tempColor = _backGroundImage.color;
            tempColor.a = 1;
            _backGroundImage.color = tempColor;
        }
        else
        {
            var tempColor = _backGroundImage.color;
            tempColor.a = 1;
            _backGroundImage.color = tempColor;
            firstFadeIn = false;
        }



        normalizedTime = 0;

        //Logo fade In
        while (normalizedTime <= durationOfFadeIn)
        {
            canvasGroup.alpha = Mathf.Clamp01(normalizedTime / durationOfFadeIn);

            normalizedTime += Time.deltaTime / durationOfFadeIn;
            Debug.Log(canvasGroup.alpha);
            yield return null;
        }

        canvasGroup.alpha = 1;

        StartCoroutine(ScenesManager.Instance.AfterFadeIn(ScenesOfNextLevel));
    }
    #endregion
}
