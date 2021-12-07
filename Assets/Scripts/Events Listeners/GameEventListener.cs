using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    public SO_GameEvent _gameEvent;

    public UnityEvent _unityEvent;
    
    private void OnEnable()
    {
        if (_gameEvent)
        {
            _gameEvent.RegisterListener(this);
        }
    }

    private void OnDisable()
    {
        if (_gameEvent)
        {
            _gameEvent.UnregisterListener();
        }
    }

    public void OnEventRaised()
    {
        _unityEvent.Invoke();
    }
    
}
