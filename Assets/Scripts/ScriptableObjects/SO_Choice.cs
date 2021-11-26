using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Choice")]

public class SO_Choice : ScriptableObject
{
    public EnumClass.ChoiceType choiceType;

    public Sprite leftChoiceCard;

    public Sprite rightChoiceCard;

    public Sprite middleChoiceCard;

    public int turnToTakeEffect;
    
}
