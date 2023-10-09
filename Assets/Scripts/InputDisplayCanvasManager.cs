using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputDisplayCanvasManager : MonoBehaviour
{
    private Canvas _canvas;
    private int? _activeArtFrameID;
    private bool _isPlayerInsideTrigger = false;

    public static event Action<int> onArtListtriggered;

    private void OnEnable()
    {
        ArtListTriggerManager.OnArtListTriggerEnter += OnPlayerEnteredTrigger;
        ArtListTriggerManager.OnArtListTriggerExit += OnPlayerExitTrigger;
    }

    private void OnDisable()
    {
        ArtListTriggerManager.OnArtListTriggerEnter -= OnPlayerEnteredTrigger;
        ArtListTriggerManager.OnArtListTriggerExit -= OnPlayerExitTrigger;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && _isPlayerInsideTrigger)
        {
            onArtListtriggered?.Invoke(_activeArtFrameID.GetValueOrDefault(-1));
            WowEventManager.TriggerEvent(nameof(WowEvents.OnPlayerRequestMenu));
            PerformTriggerExitAction();
        }
    }

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
        UpdateCanvasState(false);
    }

    private void OnPlayerEnteredTrigger(int artFrameID)
    {
        PerformTriggerEnterAction(artFrameID);
    }
    
    private void OnPlayerExitTrigger(int artFrameID)
    {
        PerformTriggerExitAction();
    }

    private void PerformTriggerEnterAction(int artFrameID)
    {
        _activeArtFrameID = artFrameID;
        _isPlayerInsideTrigger = true;
        UpdateCanvasState(true);
    }
    
    private void PerformTriggerExitAction()
    {
        _isPlayerInsideTrigger = false;
        UpdateCanvasState(false);
    }

    private void UpdateCanvasState(bool isEnabled)
    {
        _canvas.enabled = isEnabled;
    }
}