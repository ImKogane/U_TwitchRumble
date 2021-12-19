using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Player/Player Data")]
public class SO_PlayerData : ScriptableObject
{
    [Header("Gameplay")]
    public int _lifeOfPlayer = 100;
    public int _attackPlayer = 25;
    public GameObject _trapPrefab;
    
    [Header("UI")]
    public Vector3 _playerUIOffset;
    public Canvas _playerUIPrefab;
}
