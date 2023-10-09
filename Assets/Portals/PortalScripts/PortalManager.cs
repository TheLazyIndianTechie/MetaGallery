using System.Runtime.InteropServices;
using UnityEngine;
using System;
using WowQuestSystem;

public class PortalManager : MonoBehaviour

{
    [DllImport("__Internal")]
    private static extern void LinkBetweenWorlds(string url);

    //Creating an event to inform other systems that player has entered the portal area
    
    public static event Action OnPortalCanvasOpenRequested, OnPortalCanvasCloseRequested, OnMetaPortRequested;

    public static event Action<string, string> OnNotifyPortalOpen;

    

    [SerializeField]
    private GameObject vortex;

    [SerializeField]
    private ParticleSystem vortexParticles;

    
    [SerializeField]
    private bool isPortalActive;


    private void Awake()
    {
        isPortalActive = true;
    }

    private void OnEnable()
    {
        QuestManager.OnAllQuestsCompleted += PortalActivation;
        Debug.Log("Started listening for portal activation");
    }


    private void OnDisable()
    {
        QuestManager.OnAllQuestsCompleted -= PortalActivation;
        Debug.Log("Stopped listening for portal activation");
    }



    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        Debug.Log(other.name = " has entered collider");


        switch (isPortalActive)
        {
            case true:
                Debug.Log("Player has entered an active Portal");

                ActivatePortals();

                OnNotifyPortalOpen?.Invoke(name,
                    name + " Portal is now open. Do you want to head to " + name +
                    " ?. Press (Y) to Confirm and (X) to exit!");
                break;
            case false:
                Debug.Log("Player has entered an inactive portal");

                DeactivatePortals();
                break;
        }



    }


    private void OnTriggerStay(Collider other)
    {
        if (isPortalActive && other.CompareTag("Player") && Input.GetKey(KeyCode.Y))
        {
            Debug.Log("Requesting redirect to " + name);

                OnMetaPortRequested?.Invoke();
            
        }

        if (isPortalActive && other.CompareTag("Player") && Input.GetKey(KeyCode.X))
        {
            OnPortalCanvasCloseRequested?.Invoke();
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnPortalCanvasCloseRequested?.Invoke();
        }
    }

    private void ActivatePortals()
    {
        vortexParticles.Play();
        vortex?.SetActive(true);
    }

    private void DeactivatePortals()
    {
        vortexParticles.Play();
        vortex?.SetActive(false);
    }

    private void PortalActivation(bool activationStatus)
    {
        isPortalActive = activationStatus;
        Debug.Log("Portal activation status: " + isPortalActive);
    }

    

}
