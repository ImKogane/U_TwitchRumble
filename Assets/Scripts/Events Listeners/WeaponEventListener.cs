using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponEventListener : GameEventListener
{
    public UnityEvent<AudioClip> _unitySFXEvent;

    protected void OnEventRaised(SO_Weapon weapon)
    {
        _unitySFXEvent.Invoke(weapon._weaponSFX);
    }
}
