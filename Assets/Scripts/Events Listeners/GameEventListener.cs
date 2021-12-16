using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    public SO_GameEvent _gameEvent;//The GameEvent we will listen
    public UnityEvent _unityEvent;//The event we will trigger when the _gameEvent will be called

    //Immediately register to the GameEvent
    private void OnEnable()
    {
        if (_gameEvent)
        {
            _gameEvent.RegisterListener(this);
        }
    }

    //For safety : we don't listen to the event anymore if this component is deactivated
    private void OnDisable()
    {
        if (_gameEvent)
        {
            _gameEvent.UnregisterListener();
        }
    }

    //Simply call the UnityEvent when the GameEvent is triggered
    public void OnEventRaised()
    {
        _unityEvent.Invoke();
    }
    
}
