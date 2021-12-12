using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "LevelData")]
public class SO_level : ScriptableObject
{
    public List<string> LevelsToCharge = new List<string>();
    public AudioClip _levelMusic;
    
    public void ChargeLevel()
    {
        ScenesManager.Instance.ChargeALevel(LevelsToCharge, _levelMusic);
    }
    
    
}
