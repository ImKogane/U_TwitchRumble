using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "SO/Panel Choice")]

public class SO_PanelChoice : ScriptableObject
{
    public List<SO_Choice> choiceList = new List<SO_Choice>();

    public int turnToTakeEffect;
    
}
