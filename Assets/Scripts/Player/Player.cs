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

    public Animator playerAnimator;
    
    [SerializeField] private SkinnedMeshRenderer playerModel;

    public string namePlayer;

    public SO_Weapon playerWeapon = null;

    public SO_BuffWeapon playerWeaponBuff = null;

    public SO_BuffMoving playerMoveBuff = null;

    public SO_PlayerData _playerData;

    public Action EndOfAttack;

    public Action LastMoveActionChosen;

    public Action LastAttackActionChosen;
    
    public Material materialForTryCell;

    public int _playerLife = 100;

    public int _playerDamages = 25;

    GameObject _trapPrefab;
    
    [Header("UI variables")]
    private Canvas _playerUIPrefab;

    private Vector3 _playerUIOffset;

    private Quaternion PlayerBaseRotation;
    
    public Canvas playerCanvas;

    private Slider playerHealthBar;

    private TMP_Text playerNameText;

    bool AlreadyAttackThisTurn = false;

    public List<Debuff> debuffList = new List<Debuff>();

    public bool isDead = false;

    private GameObject _weaponGO;
    private ParticleSystem _weaponVFX;

    public void SpawnPlayerInGame(Tile TileForStart, string nameP)
    {
        LoadPlayerData();
        
        CurrentTile = TileForStart;
        namePlayer = nameP;
        CurrentTile.currentPlayer = this;
        CurrentTile.hasPlayer = true;
        transform.position = CurrentTile.transform.position;

        playerCanvas = Instantiate(_playerUIPrefab);
        playerHealthBar = playerCanvas.GetComponentInChildren<Slider>();
        playerNameText = playerCanvas.GetComponentInChildren<TMP_Text>();

        playerAnimator = GetComponent<Animator>();
        
        playerCanvas.worldCamera = Camera.main;
        playerCanvas.transform.position = transform.position;
        playerNameText.text = namePlayer;
        playerHealthBar.maxValue = _playerLife;
        playerHealthBar.minValue = 0;
        playerHealthBar.value = _playerLife;
        PlayerBaseRotation = this.transform.rotation;
        
        //Random player model system
        playerModel.sharedMesh = PlayerManager.Instance.SkinSystem.GetRandomSkin();
        playerModel.material = PlayerManager.Instance.SkinSystem.GetRandomMaterial();
        // -------------------------

        UpdatePlayerCanvas();
    }

    private void LoadPlayerData()
    {
        _playerLife = _playerData._lifeOfPlayer;
        _playerDamages = _playerData._attackPlayer;
        _playerUIPrefab = _playerData._playerUIPrefab;
        _playerUIOffset = _playerData._playerUIOffset;
        _trapPrefab = _playerData._trapPrefab;
    }
    
    public IEnumerator StartAttackCoroutine()
    {
        //Play anim in the animator
        playerAnimator.SetTrigger("IsAttacking");
        
        AnimatorClipInfo currentAnimationInfo = playerAnimator.GetCurrentAnimatorClipInfo(0)[0];

        yield return new WaitForSeconds(currentAnimationInfo.clip.length);

        //Attack function is launch in animation events inside the animation.
    }

    public void Attack()
    {
        if (playerWeapon == null)
        {
            return;
        }
        
        playerWeapon.RaiseEvent();
        List<Tile> listTileAffect = BoardManager.Instance.GetAffectedTiles(playerWeapon._listOfCellAffects, CurrentTile, playerMovement.RotationOfPlayer);
        List<Player> PlayersAffectByAttack = new List<Player>();

        foreach (Tile tile in listTileAffect)
        {
            if (!tile.hasObstacle)
            {
                if (tile.hasPlayer)
                {
                    if (tile.currentPlayer != null && !tile.currentPlayer.AlreadyAttackThisTurn)
                    {
                        tile.currentPlayer.ReceiveDamage(_playerDamages);
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
        _playerLife -= damage;

        playerHealthBar.value = _playerLife;
        
        if (_playerLife <= 0)
        {
            KillPlayer();
        }
    }

    public void KillPlayer()
    {
        Debug.Log($"{namePlayer} is dead !");

        PlayerManager.Instance.AllPlayersName.Remove(namePlayer);
        PlayerManager.Instance.PlayerList.Remove(this);

        CurrentTile.hasPlayer = false;
        CurrentTile.currentPlayer = null;

        GlobalManager.Instance.DestroyAllCommandsOfPlayer(this);

        Destroy(playerCanvas.gameObject);
        Destroy(gameObject);
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

        if (debuffList.Count > 0)
        {
            foreach (var debuff in debuffList)
            {
                debuff.OnPlayerReceiveDebuff();
            }
        }
    }
    
    public void UpdatePlayerCanvas()
    {
        playerCanvas.transform.LookAt(playerCanvas.transform.position + Camera.main.transform.rotation * Vector3.back, Camera.main.transform.rotation * Vector3.up);
        playerCanvas.transform.position = transform.position + _playerUIOffset;
    }

    public void ReceiveAChoice(SO_Choice choice)
    {
        choice.StartAChoice(this);

        switch (choice)
        {
            case SO_Weapon weapon:
                playerWeapon = weapon;
                SetupWeapon();
                break;

            case SO_BuffWeapon buffWeapon:
                playerWeaponBuff = buffWeapon;
                break;

            case SO_BuffMoving buffMove:
                playerMoveBuff = buffMove;
                break;
        }
    }

    public void SetupWeapon()
    {
        playerAnimator.runtimeAnimatorController = playerWeapon._weaponAnimatorController;
        foreach(Transform childTransform in GetComponentsInChildren<Transform>())
        {
            if (childTransform.name == "Hand_R")
            {
                _weaponGO = Instantiate(playerWeapon._weaponPrefab, childTransform, false);
            }
        }

        _weaponVFX = _weaponGO.GetComponentInChildren<ParticleSystem>();
        Debug.Log(_weaponVFX);
    }

    public void SetATrap()
    {
        GameObject newTrapGO = GameObject.Instantiate(_trapPrefab, CurrentTile.transform.position, Quaternion.identity);
        Trap trapComponent = newTrapGO.GetComponent<Trap>();
        if (trapComponent)
        {
            CurrentTile.trapList.Add(trapComponent);
            trapComponent.currentTile = CurrentTile;
        }
    }

    [ContextMenu("Play Weapon VFX")]
    public void PlayWeaponVFX()
    {
        _weaponVFX.Play();
    }
    public IEnumerator SetupTrapCoroutine()
    {
        playerAnimator.SetTrigger("IsPlacingTrap");
        AnimatorClipInfo currentAnimationInfo = playerAnimator.GetCurrentAnimatorClipInfo(0)[0];

        yield return new WaitForSeconds(currentAnimationInfo.clip.length);
        
        EndOfAttack.Invoke();
        
    }
    
    public String GetPlayerName()
    {
        return namePlayer;
    }
    
    
    public void CanvasVisibility(bool visibility)
    {
        playerCanvas.enabled = visibility;
    }

    public void ResetPlayerRotation()
    {
        this.transform.rotation = PlayerBaseRotation;
    }
    
    
}
