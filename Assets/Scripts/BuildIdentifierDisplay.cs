using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildIdentifierDisplay : MonoBehaviour
{
    private string applicationVersion, appOwner, appName;
    [SerializeField] private TMP_Text displayBuildDetails;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        applicationVersion = Application.version.ToString();
        appOwner = Application.companyName;
        appName = Application.productName;
    }

    private void Start()
    {
        displayBuildDetails.SetText("Build Details: " + appOwner + " " + appName + " " + applicationVersion);
        Debug.Log("Build Details: " + appOwner + " " + appName + " " + applicationVersion);
    }


}
