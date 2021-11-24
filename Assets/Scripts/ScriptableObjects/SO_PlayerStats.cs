using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stats/Player")]
public class SO_PlayerStats : ScriptableObject
{
    [SerializeField]
    private int PlayerID;

    [SerializeField]
    private int PlayerHealth;
    
    [SerializeField]
    private Tile CurrentTile;
}
