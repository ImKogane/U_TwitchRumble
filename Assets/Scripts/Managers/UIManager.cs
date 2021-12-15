using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
public class UIManager : SingletonMonobehaviour<UIManager>
{
    [Header("UI Canvas references")]
    [SerializeField] private GameObject endScreen;
    [SerializeField] private GameObject gameScreen;
    [SerializeField] private GameObject pauseScreen;
    
    [Header("Other")]
    [SerializeField] private Slider timerBar;

    [SerializeField] private TMP_Text phaseTitle;
    [SerializeField] private TMP_Text phaseDescription;

    [SerializeField] private float turnDescriptionDuration;

    [SerializeField] private TMP_Text turnCount;

    [SerializeField] private float textSpeed;
    [SerializeField] private float waitingDurationAfterPhaseDescription;

    public RectTransform choiceScreen;
    public List<Image> choiceImagesList;

    public Image feedbackTxtPrefab;

    public override bool DestroyOnLoad => true;

    public void UpdateTimerBar(float value)
    {
        timerBar.value = Mathf.Clamp(value, timerBar.minValue, timerBar.maxValue);
    }

    public void ActivateTimerBar(bool value)
    {
        timerBar.enabled = value;
    }

    public void DisplayPhaseTitle(string turnName)
    {
        phaseTitle.gameObject.SetActive(true);
        phaseTitle.text = turnName;
    }

    public void DisplayPhaseDescription(string newTurnDescription)
    {
        phaseTitle.gameObject.SetActive(true);
        StartCoroutine(DisplayPhaseDescriptionCoroutine(newTurnDescription));
    }

    IEnumerator DisplayPhaseDescriptionCoroutine(string newTurnDescription)
    {
        phaseDescription.gameObject.SetActive(true);
        phaseDescription.text = newTurnDescription;

        for (int i = 0; i <= phaseDescription.text.Length; i++)
        {
            phaseDescription.maxVisibleCharacters = i;
            yield return new WaitForSeconds(1 / textSpeed);
        }

        yield return new WaitForSeconds(waitingDurationAfterPhaseDescription);
    }

    public void DisplayEndScreen(bool value)
    {
        endScreen.SetActive(value); 
    }

    public void DisplayGameScreen(bool value)
    {
        gameScreen.SetActive(value);
    }
    
    public void DisplayPauseScreen(bool value)
    {
        pauseScreen.SetActive(value);
    }

    public void DisplayAllPlayersUI(List<Player> playerList, bool value)
    {
        foreach (var player in playerList)
        {
            player.playerCanvas.enabled = value;
        }
    }

    public void DisplayPlayerUI(Player player, bool value)
    {
        player.playerCanvas.enabled = value;
    }
    
    public void UpdateTurnCount(int newCount)
    {
        turnCount.enabled = true;
        turnCount.text = newCount.ToString();
    }
    
    public void DisplayChoiceScreen(bool value)
    {
        choiceScreen.gameObject.SetActive(value);
    }
    

    void DestroyChoicesImages()
    {
        foreach (var choiceImage in choiceImagesList)
        {
            Destroy(choiceImage.gameObject);
        }
        
        choiceImagesList.Clear();
        
    }

    public void UpdateChoiceCardsImage()
    {
        DestroyChoicesImages();

        int currentIndexChoice = ScriptableManager.Instance.GetChoiceIndexCompteur();

        if (ScriptableManager.Instance._turnChoiceList.Count <= currentIndexChoice)
        {
            return;
        }

        for (int i = 0; i < ScriptableManager.Instance._turnChoiceList[currentIndexChoice].choiceList.Count; i++)
        {
            Image newChoiceImage = new GameObject().AddComponent<Image>();

            newChoiceImage.transform.parent = choiceScreen;
            newChoiceImage.sprite =
                ScriptableManager.Instance._turnChoiceList[currentIndexChoice].choiceList[i]._cardSprite;
            newChoiceImage.preserveAspect = true;

            choiceImagesList.Add(newChoiceImage);
        }

    }

    public void EndGameUI()
    {
        GetComponent<UI_WinScreen>().MainCameraEnabled(false);
        GetComponent<UI_WinScreen>().WinCameraEnabled(true);
        DisplayEndScreen(true);
        DisplayGameScreen(false);
        GetComponent<UI_WinScreen>().SetPlayerNameText(PlayerManager.Instance.GetLastPlayer().GetPlayerName());
    }

    public void DisplayChoiceTxt(string namePlayer, int indexChoice)
    {
        StartCoroutine(DisplayChoiceTxtCorout(namePlayer, indexChoice));
    }

    private IEnumerator DisplayChoiceTxtCorout(string namePlayer, int indexChoice)
    {
        //Variables of position
        int stepNumber = 100;
        float secondsOfAction = 2;
        float maxPosY = 500;
        float maxPosZ = 3;
        float maxAlpha = 1;


        Image obj = Instantiate(feedbackTxtPrefab, gameScreen.transform);

        //Position of spawn
        List<float> positionXOfMessage = new List<float>() { obj.rectTransform.position.x - 500, obj.rectTransform.position.x , obj.rectTransform.position.x + 500 };
        obj.rectTransform.position = new Vector3(positionXOfMessage[indexChoice], obj.rectTransform.position.y - 625, obj.rectTransform.position.z);

        //Transparence variables
        CanvasGroup canvasGroup = obj.GetComponent<CanvasGroup>();

        //Display text 
        TextMeshProUGUI txt = obj.GetComponentInChildren<TextMeshProUGUI>();
        string messageToDisplay = namePlayer + "\n[Choice" + (indexChoice+1).ToString() + "]";
        txt.text = messageToDisplay;

        //Go up 
        for (int i = 0; i < stepNumber; i++)
        {
            yield return new WaitForSeconds(secondsOfAction / stepNumber);
            obj.rectTransform.position = new Vector3(obj.rectTransform.position.x, obj.rectTransform.position.y + (maxPosY / stepNumber), obj.rectTransform.position.z + (maxPosZ / stepNumber));
        }

        for (int i = 0; i < stepNumber; i++)
        {
            canvasGroup.alpha -= maxAlpha / stepNumber ;
            yield return new WaitForSeconds((secondsOfAction/4) / stepNumber);
        }

        Destroy(obj.gameObject);
    }

}
