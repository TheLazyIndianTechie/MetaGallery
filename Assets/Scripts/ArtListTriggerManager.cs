using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtListTriggerManager : MonoBehaviour
{
    [SerializeField] private int artFrameID;

    public static event Action<int> OnArtListTriggerEnter;

    public static event Action<int> OnArtListTriggerExit;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        OnArtListTriggerEnter?.Invoke(artFrameID);
        Debug.Log("Player has activated the art list trigger on Frame #" + artFrameID);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        OnArtListTriggerExit?.Invoke(artFrameID);
    }
}
