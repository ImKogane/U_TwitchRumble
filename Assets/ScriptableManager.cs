using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableManager : MonoBehaviour
{
    public static ScriptableManager Instance;

    public List<ListSoChoiceClass> _turnChoiceList = new List<ListSoChoiceClass>();

    [System.Serializable]
    public class ListSoChoiceClass
    {
        public List<SO_Choice> choiceList;
    }


    private int _choiceIndexCompteur = 0;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void IncreaseChoiceIndexCompteur() //A appeler a la fin d'un tour de choice
    {
        _choiceIndexCompteur++;
    }

    public int GetChoiceIndexCompteur()
    {
        return _choiceIndexCompteur;
    }
}
