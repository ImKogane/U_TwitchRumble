using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Game Event")]
public class SO_GameEvent : ScriptableObject
{
    private GameEventListener _GameEventListener;
    
    public void RegisterListener(GameEventListener gameEventListener)
    {
        _GameEventListener = gameEventListener;
    }

    public void UnregisterListener()
    {
        _GameEventListener = null;
    }
    
    public void Raise()
    {
        _GameEventListener.OnEventRaised();
    }
}
