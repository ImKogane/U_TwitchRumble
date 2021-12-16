using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Basic Infos")]
    public string _name;
    public int _currentHealth;
    public int _maxHealth = 100;
    public int _attackDamages = 25;
    public bool _isDead = false;
    
    [Header("Choice Scriptable Objects")]
    public SO_Weapon _currentWeapon = null;
    public SO_BuffWeapon _currentWeaponBuff = null;
    public SO_BuffMoving _currentMoveBuff = null;

    [Header("Owned Components")]
    public PlayerMovement _playerMovementComponent;
    public Animator _animatorComponent;
    public SkinnedMeshRenderer _skinnedMeshComponent;
    
    [Header("Custom data to inject")]
    public SO_PlayerData _playerDataSO;
    
    [Header("Core gameplay variables")]
    public Tile _currentTile;
    public GameObject _trapPrefab;
    public List<Debuff> _debuffList = new List<Debuff>();
    public List<int> _choicesMade = new List<int>();
    
    [Header("UI variables")]
    public Canvas _uiPrefab;
    public Vector3 _uiOffset;
    public Canvas _infosCanvas;
    public Image _feedbackTxtPrefab;
    private Slider _healthBar;
    private TMP_Text _nameText;

    private Quaternion _baseRotation;
    bool _alreadyAttackedThisTurn = false;
    public Action _endOfAttackAction;
    public Action _endOfDeathAction;
    private GameObject _weaponGO;
    private ParticleSystem _weaponVFX;

    #region Basic Unity Events
    void Update()
    {
        UpdatePlayerCanvas();
    }
    
    #endregion
    
    #region Setup
    //Setup the player in a new game
    public void SpawnPlayerInGame(Tile startingTile, string nameOfPlayer) 
    {
        if (_playerDataSO != null) InjectDatasFromSO(); //Inject custom data if needed

        _playerMovementComponent.SetUpPlayerMovement(this);

        _currentTile = startingTile;
        _name = nameOfPlayer;
        _currentTile.currentPlayer = this;
        _currentTile.hasPlayer = true;
        transform.position = _currentTile.transform.position;
        
        _animatorComponent = GetComponent<Animator>();
        _baseRotation = transform.rotation;
        _currentHealth = _maxHealth;
        SetPlayerUI();

        //Random player model system
        _skinnedMeshComponent.sharedMesh = PlayerManager.Instance._skinSystem.GetRandomSkin();
        _skinnedMeshComponent.material = PlayerManager.Instance._skinSystem.GetRandomMaterial();

        // -------------------------
    }

    //Set the new player canvas according to the received data
    public void SetPlayerUI()
    {
        _infosCanvas = Instantiate(_uiPrefab);
        _healthBar = _infosCanvas.GetComponentInChildren<Slider>();
        _nameText = _infosCanvas.GetComponentInChildren<TMP_Text>();
        _infosCanvas.worldCamera = Camera.main;
        _infosCanvas.transform.position = transform.position;
        _nameText.text = _name;
        _healthBar.maxValue = _maxHealth;
        _healthBar.minValue = 0;
        _healthBar.value = _currentHealth;
        
    }
    
    //Set some data from SO
    public void InjectDatasFromSO()
    {
        _maxHealth = _playerDataSO._lifeOfPlayer;
        _attackDamages = _playerDataSO._attackPlayer;
        _uiPrefab = _playerDataSO._playerUIPrefab;
        _uiOffset = _playerDataSO._playerUIOffset;
        _trapPrefab = _playerDataSO._trapPrefab;
    }
    
    //Used to place correctly the player on endgame screen
    public void ResetPlayerRotation()
    {
        transform.rotation = _baseRotation;
    }
    
    #endregion

    #region Attack System
    
    //Play the attack animation, which has the Attack animation Event in it
    public void StartAttackAnimation()
    {
        _animatorComponent.SetTrigger("IsAttacking");
    }

    //Method called by an animation event inside all Attack animations
    public void Attack()
    {
        
        if (_currentWeapon == null) //Safety check
        {
            return;
        }
        
        _currentWeapon.RaiseEvent(); //Raise event for additional FX is wanted
        
        //Fetch from the BoardManager the tiles affected by the weapon's attack
        List<Tile> listTileAffect = BoardManager.Instance.GetAffectedTiles(_currentWeapon._listOfCellAffects, _currentTile, _playerMovementComponent.RotationOfPlayer);
        List<Player> PlayersAffectByAttack = new List<Player>();

        foreach (Tile tile in listTileAffect)
        {
            if (!tile.hasObstacle)
            {
                StartCoroutine(tile.ReturnToClassicColor()); //Display the affected tiles
                if (tile.hasPlayer)
                {
                    if (tile.currentPlayer != null && !tile.currentPlayer._alreadyAttackedThisTurn)
                    {
                        tile.currentPlayer.ReceiveDamage(_attackDamages);
                        if (tile.currentPlayer != null)
                        {
                            //Safety check to avoid player beeing hit multiple times in the same attack
                            tile.currentPlayer._alreadyAttackedThisTurn = true; 
                            PlayersAffectByAttack.Add(tile.currentPlayer);
                            tile.currentPlayer.ReceiveWeaponBuffEffect(this); //Add debuff if possible

                        }
                    }
                }
            }
        }

        foreach (Player player in PlayersAffectByAttack)
        {
            player._alreadyAttackedThisTurn = false;
        }

        _endOfAttackAction.Invoke(); //Tells the Attack Command it ended
    }

    //Remove health, play animation and can trigger death system with animation
    public void ReceiveDamage(int damage)
    {
        //Plays the hit animation if the player is not busy with another one
        if (_animatorComponent.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            _animatorComponent.SetTrigger("GetHit");
        }
        
        _currentHealth -= damage;
        _healthBar.value = _currentHealth;
        
        //Add a DeathCommand to the GlobalManager
        if (_currentHealth <= 0)
        {
            CommandManager.Instance.InsertCommandInList(1, new CommandDeath(this));
        }
    }
    
    //Activate the particle system located in the weapon Prefab
    public void PlayWeaponVFX()
    {
        _weaponVFX.Play();
    }

    #endregion

    #region Death System

    //Main Death method : safely remove the player from the game
    public void KillPlayer()
    {

        PlayerManager.Instance._listPlayersNames.Remove(_name);
        PlayerManager.Instance._listPlayers.Remove(this);


        _currentTile.hasPlayer = false;
        _currentTile.currentPlayer = null;

        _isDead = true;


        CommandManager.Instance.DestroyAllCommandsOfDeadPlayer(this);
        
        Destroy(_infosCanvas.gameObject);
        Destroy(gameObject);
    }

    //Trigger the death animation, wait for it to end and then trigger the main Death method
    public IEnumerator DeathCoroutine()
    {
        _animatorComponent.SetBool("IsDead", true);
        
        AnimatorClipInfo currentAnimationInfo = _animatorComponent.GetCurrentAnimatorClipInfo(0)[0];

        yield return new WaitForSeconds(currentAnimationInfo.clip.length);

        _endOfDeathAction.Invoke(); //Tells the action it just ended

        KillPlayer();
    }

    #endregion
    
    #region Debuff System
    
    //At the very beginning of each Action GameState
    public void ManageAllDebuffs()
    {
        for(int i = 0; i< _debuffList.Count; i++)
        {
            if (_debuffList[i]._duration > 0)
            {
                _debuffList[i].ApplyEffect();
                _debuffList[i]._duration--;
            }
            else
            {
                _debuffList[i].RemoveEffect();
            }
        }
    }
    
    //Get debuff from enemy player's attack
    public void ReceiveWeaponBuffEffect(Player attackingPlayer)
    {
        if (attackingPlayer._currentWeaponBuff != null)
        {
            attackingPlayer._currentWeaponBuff.ApplyWeaponBuff(this, attackingPlayer);
        }

        if (_debuffList.Count > 0)
        {
            foreach (var debuff in _debuffList)
            {
                debuff.OnPlayerReceiveDebuff();
            }
        }
    }
    
    #endregion

    #region UI Management

    //Rotate the player UI toward main Camera
    public void UpdatePlayerCanvas()
    {
        if (Camera.main)
        {
            _infosCanvas.transform.LookAt(_infosCanvas.transform.position + Camera.main.transform.rotation * Vector3.back, Camera.main.transform.rotation * Vector3.up);
            _infosCanvas.transform.position = transform.position + _uiOffset;
        }
    }
    
    public void DisplayCommandTxt(string message)
    {
        StartCoroutine(DisplayCommandTxtCorout(message));
    }

    //Display choice indicator
    public IEnumerator DisplayCommandTxtCorout(string message)
    {
        int stepNumber = 100;
        float secondsOfAction = 2;
        float maxPosY = 3;
        float maxPosZ = 3;
        float maxAlpha = 1;

        Image obj = Instantiate(_feedbackTxtPrefab, _infosCanvas.transform);
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
    
    public void CanvasVisibility(bool visibility)
    {
        _infosCanvas.enabled = visibility;
    }
    
    #endregion

    #region Choices System

    //Distribute the choice into its corresponding variable
    public void ReceiveAChoice(SO_Choice choice)
    {
        choice.StartAChoice(this);

        switch (choice)
        {
            case SO_Weapon weapon:
                _currentWeapon = weapon;
                SetupWeapon();
                break;

            case SO_BuffWeapon buffWeapon:
                _currentWeaponBuff = buffWeapon;
                break;

            case SO_BuffMoving buffMove:
                _currentMoveBuff = buffMove;
                break;
        }
    }

    //Display the new weapon in the player's hands and set reference to its VFX
    public void SetupWeapon()
    {
        _animatorComponent.runtimeAnimatorController = _currentWeapon._weaponAnimatorController;
        foreach(Transform childTransform in GetComponentsInChildren<Transform>())
        {
            if (childTransform.name == "Hand_R")
            {
                _weaponGO = Instantiate(_currentWeapon._weaponPrefab, childTransform, false);
            }
        }

        _weaponVFX = _weaponGO.GetComponentInChildren<ParticleSystem>();
    }

    #endregion

    #region Trap System

    //Start the trap placing animation, which contains an Animation Event to set the trap on the current tile
    public IEnumerator SetupTrapCoroutine()
    {
        _animatorComponent.SetTrigger("IsPlacingTrap");
        AnimatorClipInfo currentAnimationInfo = _animatorComponent.GetCurrentAnimatorClipInfo(0)[0];

        yield return new WaitForSeconds(currentAnimationInfo.clip.length);

    }
    
    //Main function called by an animation Event, finish by ending the command
    public void SetATrap()
    {
        GameObject newTrapGO = GameObject.Instantiate(_trapPrefab, _currentTile.transform.position, Quaternion.identity);
        Trap trapComponent = newTrapGO.GetComponent<Trap>();
        if (trapComponent)
        {
            _currentTile.trapList.Add(trapComponent);
            trapComponent.currentTile = _currentTile;
        }
        
        _endOfAttackAction.Invoke();
    }
    
    

    #endregion
    
}
