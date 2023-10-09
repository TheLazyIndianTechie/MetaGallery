using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;


public class MetaportManager : MonoBehaviour
{
    //Import the JS lib functions for Metaporting
    [DllImport("__Internal")]
    private static extern void LinkBetweenWorlds(string url);

    [SerializeField]
    private string destinationUrl;

    private void OnEnable()
    {
        PortalManager.OnMetaPortRequested += CallMetaport;
    }

    private void OnDisable()
    {
        PortalManager.OnMetaPortRequested -= CallMetaport;
    }

    public void CallMetaport()
    {
#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_OSX

        Debug.Log("Simulating Metaport Successful - You are inside Unity Editor");
#else
        LinkBetweenWorlds(destinationUrl);

#endif


    }
}
