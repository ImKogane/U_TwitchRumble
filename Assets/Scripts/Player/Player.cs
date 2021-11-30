using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using Random = System.Random;

public class Player : MonoBehaviour
{
    public Tile CurrentTile;

    public PlayerMovement playerMovement;

    public string namePlayer;

    public Weapon playerWeapon = null;
    public WeaponBuff playerWeaponBuff = null;
    public MoveBuff playerMoveBuff = null;

    public GameObject trapPrefab;

    public EnumClass.WeaponType typeOfWeapon;

    public Action EndOfAttack;

    public Action LastMoveActionChosen;

    public Action LastAttackActionChosen;
    
    public Material materialForTryCell;

    public int LifeOfPlayer = 100;

    public int AttackPlayer = 25;

    [Header("UI variables")]
    public Canvas playerUIPrefab;
    public Vector3 UIOffset;
    
    public Canvas playerCanvas;
    private Slider playerHealthBar;
    private TMP_Text playerNameText;

    bool AlreadyAttackThisTurn = false;

    public List<Debuff> debuffList = new List<Debuff>();

    public void SpawnPlayerInGame(Tile TileForStart, string nameP)
    {
        CurrentTile = TileForStart;
        namePlayer = nameP;
        CurrentTile.currentPlayer = this;
        CurrentTile.hasPlayer = true;
        transform.position = CurrentTile.transform.position;

        playerCanvas = Instantiate(playerUIPrefab);
        playerHealthBar = playerCanvas.GetComponentInChildren<Slider>();
        playerNameText = playerCanvas.GetComponentInChildren<TMP_Text>();
        
        playerCanvas.worldCamera = Camera.main;
        playerCanvas.transform.position = transform.position;
        playerNameText.text = namePlayer;
        playerHealthBar.maxValue = LifeOfPlayer;
        playerHealthBar.minValue = 0;
        playerHealthBar.value = LifeOfPlayer;
        
        UpdatePlayerCanvas();
    }

    public void Attack()
    {
        List<Tile> listTileAffect = playerWeapon.Attack(CurrentTile.GetCoord(), playerMovement.RotationOfPlayer);
        List<Player> PlayersAffectByAttack = new List<Player>();

        foreach (Tile tile in listTileAffect)
        {
            if (!tile.hasObstacle)
            {
                if (tile.hasPlayer)
                {
                    if (tile.currentPlayer != null && !tile.currentPlayer.AlreadyAttackThisTurn)
                    {
                        tile.currentPlayer.ReceiveDamage(AttackPlayer);
                        tile.currentPlayer.AlreadyAttackThisTurn = true;
                        PlayersAffectByAttack.Add(tile.currentPlayer);
                        tile.currentPlayer.ReceiveWeaponBuffEffect(this);
                    }
                }
                else
                {
                    tile.gameObject.GetComponentInChildren<MeshRenderer>().material = materialForTryCell;
                }
            }
        }

        foreach (Player player in PlayersAffectByAttack)
        {
            player.AlreadyAttackThisTurn = false;
        }

        EndOfAttack.Invoke();
    }

    public void ReceiveDamage(int damage)
    {
        LifeOfPlayer -= damage;

        playerHealthBar.value = LifeOfPlayer;
        
        if (LifeOfPlayer <= 0)
        {
            Debug.Log("Player is dead !");

            PlayerManager.Instance.AllPlayersName.Remove(namePlayer);
            PlayerManager.Instance.PlayerList.Remove(this);

            CurrentTile.hasPlayer = false;
            CurrentTile.currentPlayer = null;

            GlobalManager.Instance.DestroyAllCommandsOfPlayer(this);
            
            Destroy(playerCanvas.gameObject);
            Destroy(gameObject);
        }
    }

    public void ManageAllDebuffs()
    {
        for(int i = 0; i< debuffList.Count; i++)
        {
            if (debuffList[i].duration > 0)
            {
                debuffList[i].ApplyEffect();
                debuffList[i].duration--;
            }
            else
            {
                debuffList[i].RemoveEffect();
            }
        }
    }
    
    public void ReceiveWeaponBuffEffect(Player attackingPlayer)
    {

        if (attackingPlayer.playerWeaponBuff != null)
        {
            attackingPlayer.playerWeaponBuff.ApplyWeaponBuff(this, attackingPlayer);
        }
    }
    
    public void UpdatePlayerCanvas()
    {
        playerCanvas.transform.LookAt(playerCanvas.transform.position + Camera.main.transform.rotation * Vector3.back, Camera.main.transform.rotation * Vector3.up);
        playerCanvas.transform.position = transform.position + UIOffset;
    }
    
}
