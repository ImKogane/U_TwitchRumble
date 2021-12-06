using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableManager : SingletonMonobehaviour<ScriptableManager>
{
    public List<ListSoChoiceClass> _turnChoiceList = new List<ListSoChoiceClass>();

    [System.Serializable]
    public class ListSoChoiceClass
    {
        public List<SO_Choice> choiceList;
    }

    private int _choiceIndexCompteur = 0;

    public override bool DestroyOnLoad => true;

    public void IncreaseChoiceIndexCompteur() //A appeler a la fin d'un tour de choice
    {
        _choiceIndexCompteur++;
    }

    public int GetChoiceIndexCompteur()
    {
        return _choiceIndexCompteur;
    }

    public int GetRandomIndexChoice()
    {
        if (_turnChoiceList.Count > _choiceIndexCompteur)
        {
            int rand = Random.Range(0, _turnChoiceList[_choiceIndexCompteur].choiceList.Count);
            return rand;
        }
        return 0;
    }
}
