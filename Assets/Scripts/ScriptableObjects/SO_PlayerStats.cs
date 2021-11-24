using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stats/Player")]
public class SO_PlayerStats : ScriptableObject
{
    //Tile sur laquelle se trouve le joueur

    [SerializeField]
    private int PlayerHealth;
}
