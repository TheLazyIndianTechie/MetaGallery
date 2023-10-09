using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[System.Serializable]
public class StringUnityEvent : UnityEvent<string> { }

public enum WowEvents
{
    OnUnityFirstLoad, //When the Unity scene is first loaded and player avatars have to be read. Contains AvatarURL from RPM
    OnPlayerDataRetrieved, // When Player Data is retrieved from backend
    OnRPMAvatarLoaded, // When RPM avatar is loaded
    OnPlayerRequestMenu, // When Player requests a specific menu
    OnPlayerRequestMetaport, // When Player needs to Metaport.
    OnPlayerResumeGame, // When Player resumes game (closes the displayed Canvas UI and gets back to game)
    OnPlayerRequestQuit, // When Player wants to quit
    OnGameError, // When the player or game triggers an error
    OnDisplayInfo, // When the game has any updates to display
    OnDisplayActionInfo, // When the game has any updates to display
}

public enum WowMenus
{
    ErrorDialogCanvas,
    PauseIconCanvas,
    ActionInfoCanvas,
    MainMenuCanvas,
    AvatarSelectionCanvas,
    LoadingCanvas,
}

public class WowEventManager : MonoBehaviour
{
    private Dictionary<string, UnityEvent> eventDictionary;
    private Dictionary<string, UnityEvent<string>> stringEventDictionary;

    private static WowEventManager eventManager;

    public static WowEventManager Instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType(typeof(WowEventManager)) as WowEventManager;

                if (!eventManager)
                {
                    Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                }
                else
                {
                    eventManager.Init();
                }
            }

            return eventManager;
        }
    }

    protected virtual void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, UnityEvent>();
        }

        if (stringEventDictionary == null)
        {
            stringEventDictionary = new Dictionary<string, UnityEvent<string>>();
        }
    }

    /// <summary>
    /// A static interface to subscribe to a default UnityEvent
    /// </summary>
    /// <param name="eventName">The name of the UnityEvent to subscribe</param>
    /// <param name="listener">The UnityAction to be executed on intercepting the UnityEvent</param>
    public static void StartListening(string eventName, UnityAction listener)
    {
        UnityEvent thisEvent = null;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            Instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    /// <summary>
    /// A static interface to subscribe to a UnityEvent having one string parameter
    /// </summary>
    /// <param name="eventName">The name of the UnityEvent to subscribe</param>
    /// <param name="listener">The UnityAction to be executed on intercepting the UnityEvent</param>
    public static void StartListening(string eventName, UnityAction<string> listener)
    {
        UnityEvent<string> thisEvent = null;
        if (Instance.stringEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent<string>();
            thisEvent.AddListener(listener);
            Instance.stringEventDictionary.Add(eventName, thisEvent);
        }
    }

    /// <summary>
    /// A static interface to unsubscribe to a default UnityEvent
    /// </summary>
    /// <param name="eventName">The name of the UnityEvent to unsubsubscribe from</param>
    /// <param name="listener">The listener that needs to be removed</param>
    public static void StopListening(string eventName, UnityAction listener)
    {
        if (eventManager == null) return;
        UnityEvent thisEvent = null;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    /// <summary>
    /// A static interface to unsubscribe to a UnityEvent having single string parameter
    /// </summary>
    /// <param name="eventName">The name of the UnityEvent to unsubsubscribe from</param>
    /// <param name="listener">The listener that needs to be removed</param>
    public static void StopListening(string eventName, UnityAction<string> listener)
    {
        if (eventManager == null) return;
        UnityEvent<string> thisEvent = null;
        if (Instance.stringEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    /// <summary>
    /// A static interface to trigger a UnityEvent
    /// </summary>
    /// <param name="eventName">The name of the UnityEvent that needs to be triggered</param>
    public static void TriggerEvent(string eventName)
    {
        UnityEvent thisEvent = null;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke();
        }
    }

    /// <summary>
    /// A static interface to trigger a UnityEvent having single string parameter
    /// </summary>
    /// <param name="eventName">The name of the UnityEvent that needs to be triggered</param>
    /// <param name="param">The string message that is to be passed to the listener</param>
    public static void TriggerEvent(string eventName, string param)
    {
        UnityEvent<string> thisEvent = null;
        if (Instance.stringEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(param);
        }
    }
}