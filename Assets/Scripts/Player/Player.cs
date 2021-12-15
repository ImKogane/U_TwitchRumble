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

    public Animator _animator;
    
    public SkinnedMeshRenderer playerModel;

    public string namePlayer;

    public SO_Weapon playerWeapon = null;

    public SO_BuffWeapon playerWeaponBuff = null;

    public SO_BuffMoving playerMoveBuff = null;

    public SO_PlayerData _playerData;

    public Action EndOfAttack;

    public Action EndOfDeath;
    
    public Material materialForTryCell;

    public int _currentHealth;
    public int _maxHealth = 100;
    

    public int _playerDamages = 25;

    GameObject _trapPrefab;
    
    [Header("UI variables")]
    public Canvas _playerUIPrefab;

    private Vector3 _playerUIOffset;

    private Quaternion PlayerBaseRotation;
    
    public Canvas playerCanvas;

    private Slider playerHealthBar;

    private TMP_Text playerNameText;

    bool AlreadyAttackThisTurn = false;

    public List<Debuff> debuffList = new List<Debuff>();

    public List<int> _choicesMade = new List<int>();

    public bool isDead = false;

    private GameObject _weaponGO;
    private ParticleSystem _weaponVFX;

    public Image feedbackTxtPrefab;

    public void SpawnPlayerInGame(Tile TileForStart, string nameP)
    {
        InjectDatasFromSO();

        playerMovement.SetUpPlayerMovment(this);

        CurrentTile = TileForStart;
        namePlayer = nameP;
        CurrentTile.currentPlayer = this;
        CurrentTile.hasPlayer = true;
        transform.position = CurrentTile.transform.position;
        
        _animator = GetComponent<Animator>();
        PlayerBaseRotation = transform.rotation;
        _currentHealth = _maxHealth;
        SetPlayerUI();

        //Random player model system
        playerModel.sharedMesh = PlayerManager.Instance.SkinSystem.GetRandomSkin();
        playerModel.material = PlayerManager.Instance.SkinSystem.GetRandomMaterial();
        // -------------------------

        UpdatePlayerCanvas();
    }

    public void SetPlayerUI()
    {
        playerCanvas = Instantiate(_playerUIPrefab);
        playerHealthBar = playerCanvas.GetComponentInChildren<Slider>();
        playerNameText = playerCanvas.GetComponentInChildren<TMP_Text>();
        playerCanvas.worldCamera = Camera.main;
        playerCanvas.transform.position = transform.position;
        playerNameText.text = namePlayer;
        playerHealthBar.maxValue = _currentHealth;
        playerHealthBar.minValue = 0;
        playerHealthBar.value = _currentHealth;
        
    }
    
    public void InjectDatasFromSO()
    {
        _maxHealth = _playerData._lifeOfPlayer;
        _playerDamages = _playerData._attackPlayer;
        _playerUIPrefab = _playerData._playerUIPrefab;
        _playerUIOffset = _playerData._playerUIOffset;
        _trapPrefab = _playerData._trapPrefab;
    }
    
    public IEnumerator StartAttackCoroutine()
    {
        //Play anim in the animator
        _animator.SetTrigger("IsAttacking");
        
        AnimatorClipInfo currentAnimationInfo = _animator.GetCurrentAnimatorClipInfo(0)[0];

        yield return new WaitForSeconds(currentAnimationInfo.clip.length);
        
        //Attack function is launch in animation events inside the animation.
        
        EndOfAttack.Invoke();
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
                StartCoroutine(tile.ReturnToClassicColor());
                if (tile.hasPlayer)
                {
                    if (tile.currentPlayer != null && !tile.currentPlayer.AlreadyAttackThisTurn)
                    {
                        tile.currentPlayer.ReceiveDamage(_playerDamages);
                        if (tile.currentPlayer != null)
                        {
                            tile.currentPlayer.AlreadyAttackThisTurn = true;
                            PlayersAffectByAttack.Add(tile.currentPlayer);
                            tile.currentPlayer.ReceiveWeaponBuffEffect(this);
                        }
                    }
                }
            }
        }

        foreach (Player player in PlayersAffectByAttack)
        {
            player.AlreadyAttackThisTurn = false;
        }
    }

    public void ReceiveDamage(int damage)
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            _animator.SetTrigger("GetHit");
        }
        
        _currentHealth -= damage;
        playerHealthBar.value = _currentHealth;
        
        if (_currentHealth <= 0)
        {
            GlobalManager.Instance.InsertCommandInList(1, new CommandDeath(this));
        }
    }

    public void KillPlayer()
    {
        Debug.Log($"{namePlayer} is dead !");
        
        PlayerManager.Instance.AllPlayersName.Remove(namePlayer);
        PlayerManager.Instance.PlayerList.Remove(this);

        CurrentTile.hasPlayer = false;
        CurrentTile.currentPlayer = null;

        isDead = true;

        GlobalManager.Instance.DestroyAllCommandsOfPlayer(this);
        
        Destroy(playerCanvas.gameObject);
        Destroy(gameObject);
    }

    public IEnumerator DeathCoroutine()
    {

        _animator.SetBool("IsDead", true);
        
        AnimatorClipInfo currentAnimationInfo = _animator.GetCurrentAnimatorClipInfo(0)[0];

        yield return new WaitForSeconds(currentAnimationInfo.clip.length);
        
        EndOfDeath.Invoke();
        KillPlayer();

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
        if (Camera.main)
        {
            playerCanvas.transform.LookAt(playerCanvas.transform.position + Camera.main.transform.rotation * Vector3.back, Camera.main.transform.rotation * Vector3.up);
            playerCanvas.transform.position = transform.position + _playerUIOffset;
        }
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
        _animator.runtimeAnimatorController = playerWeapon._weaponAnimatorController;
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
        _animator.SetTrigger("IsPlacingTrap");
        AnimatorClipInfo currentAnimationInfo = _animator.GetCurrentAnimatorClipInfo(0)[0];

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

    public void DisplayCommandTxt(string message)
    {
        StartCoroutine(DisplayCommandTxtCorout(message));
    }

    public IEnumerator DisplayCommandTxtCorout(string message)
    {
        int stepNumber = 100;
        float secondsOfAction = 2;
        float maxPosY = 3;
        float maxPosZ = 3;
        float maxAlpha = 1;

        Image obj = Instantiate(feedbackTxtPrefab, playerCanvas.transform);
        obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y + 2, obj.transform.position.z);

        CanvasGroup canvasGroup = obj.GetComponent<CanvasGroup>();
        TextMeshProUGUI txt = obj.GetComponentInChildren<TextMeshProUGUI>();
        txt.text = message;

        for (int i = 0; i < stepNumber; i++)
        {
            obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y + (maxPosY/stepNumber), obj.transform.position.z + (maxPosZ / stepNumber));
            canvasGroup.alpha -= maxAlpha / stepNumber;
            yield return new WaitForSeconds(secondsOfAction/stepNumber);
        }

        Destroy(obj.gameObject);
    }
}
