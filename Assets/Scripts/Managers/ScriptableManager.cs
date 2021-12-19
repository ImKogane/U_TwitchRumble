using System.Collections.Generic;
using UnityEngine;

public class ScriptableManager : SingletonMonobehaviour<ScriptableManager>
{
    public List<ListSoChoiceClass> _turnChoiceList = new List<ListSoChoiceClass>();

    [System.Serializable]
    public class ListSoChoiceClass
    {
        public List<SO_Choice> _choiceList;
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
            int rand = Random.Range(0, _turnChoiceList[_choiceIndexCompteur]._choiceList.Count);
            return rand;
        }
        return 0;
    }

    public SO_Choice GetChoiceFromIndex(int compteur, int index)
    {
        if (compteur < _turnChoiceList.Count)
        {
            if (index < _turnChoiceList[compteur]._choiceList.Count)
            {
                return _turnChoiceList[compteur]._choiceList[index];
            }
        }

        return null;
    }

    public int FindChoiceIndex(SO_Choice choice)
    {
        int index = 0;
        
        foreach (var listSoChoice in _turnChoiceList)
        {
            int tempIndex = listSoChoice._choiceList.FindIndex((c) => { return c == choice; });

            if (tempIndex >= 0)
            {
                index = tempIndex;
                break;
            }
        }
        
        return index;
    }
}
