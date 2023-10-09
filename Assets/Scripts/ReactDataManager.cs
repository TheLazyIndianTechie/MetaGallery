using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

enum VisualScriptingVariables
{
    UserDataJSON,
    UserOwnedNFTCollectionData,
    ListedNFTCollectionData,
    QuestCollectionData,
}

public class ReactDataManager : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void GetUserData(string gameObjectName, string functionName);

    [DllImport("__Internal")]
    private static extern void GetUserOwnedNFTCollectionData(string gameObjectName, string functionName);

    [DllImport("__Internal")]
    private static extern void GetListedNFTData(string gameObjectName, string functionName);


    [DllImport("__Internal")]
    private static extern void DeListNFT(string gameObjectName, string functionName, int frameID, int collectionID, int nftID);

    [DllImport("__Internal")]
    private static extern void ListNFT(string gameObjectName, string functionName, int frameID, int collectionID, int nftID, ulong nftCost);

    [DllImport("__Internal")]
    private static extern void BuyListedNFT(string gameObjectName, string functionName, int frameID, int collectionID, int nftID, ulong nftCost);

    [DllImport("__Internal")]
    private static extern void GetQuestData(string gameObjectName, string functionName);

    [DllImport("__Internal")]
    private static extern void CompleteQuest(int questId, string gameObjectName, string functionName);


    public static ReactDataManager Instance { get; private set; }

    // Delegates & Events
    public delegate void GetUserDataCallback();
    public static event GetUserDataCallback OnGetUserDataCallback;

    public delegate void GetUserOwnedNFTCollectionDataCallback();
    public static event GetUserOwnedNFTCollectionDataCallback OnGetUserOwnedNFTCollectionDataCallback;

    public delegate void GetListedNFTDataCallback();
    public static event GetListedNFTDataCallback OnGetListedNFTDataCallback;

    public delegate void DeListNFTCallback();
    public static event DeListNFTCallback OnDeListNFTCallback;

    public delegate void ListNFTCallback();
    public static event ListNFTCallback OnListNFTCallback;

    public delegate void BuyListedNFTCallback();
    public static event BuyListedNFTCallback OnBuyListedNFTCallback;

    public delegate void GetQuestDataCallback();
    public static event GetQuestDataCallback OnGetQuestDataCallback;

    // Lifecycle Methods
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Dispatch events from Unity to React app
    public void CallGetUserData(string gameObjectName, string functionName)
    {
        GetUserData(gameObjectName, functionName);
    }

    public void CallGetUserOwnedNFTCollectionData(string gameObjectName, string functionName)
    {
        GetUserOwnedNFTCollectionData(gameObjectName, functionName);
    }

    public void CallGetListedNFTCollectionData(string gameObjectName, string functionName)
    {
        GetListedNFTData(gameObjectName, functionName);
    }

    public void CallDeListNFT(string gameObjectName, string functionName, int frameID, int collectionID, int nftID)
    {
        Debug.Log("ReactDataManager: CallDeListNFT(" + gameObjectName + ", " + functionName + ", " + frameID + ", " + collectionID + ", " + nftID + ")");
        DeListNFT(gameObjectName, functionName, frameID, collectionID, nftID);
    }

    public void CallListNFT(string gameObjectName, string functionName, int frameID, int collectionID, int nftID, ulong nftCost)
    {
        Debug.Log("ReactDataManager: CallListNFT(" + gameObjectName + ", " + functionName + ", " + frameID + ", " + collectionID + ", " + nftID + ", " + nftCost + ")");
        ListNFT(gameObjectName, functionName, frameID, collectionID, nftID, nftCost);
    }

    public void CallBuyListedNFT(string gameObjectName, string functionName, int frameID, int collectionID, int nftID, ulong nftCost)
    {
        Debug.Log("ReactDataManager: CallBuyListedNFT(" + gameObjectName + ", " + functionName + ", " + frameID + ", " + collectionID + ", " + nftID + ", " + nftCost + ")");
        BuyListedNFT(gameObjectName, functionName, frameID, collectionID, nftID, nftCost);
    }

    public void CallGetQuestData(string gameObjectName, string functionName)
    {
        GetQuestData(gameObjectName, functionName);
    }

    public void CallCompleteQuest(int questId, string gameObjectName, string functionName)
    {
        CompleteQuest(questId, gameObjectName, functionName);
    }

    // Handle React callbacks
    public void GetUserData(string dataJSON)
    {
        // Deserialize the JSON data
        UserData userData = JsonUtility.FromJson<UserData>(dataJSON);

        // Save the data into visual scripting object
        Variables.Application.Set(VisualScriptingVariables.UserDataJSON.ToString(), userData);

        // Dispatch Unity delegate event
        OnGetUserDataCallback?.Invoke();
    }


    public void GetUserOwnedNFTCollectionData(string dataJSON)
    {
        // Deserialize the JSON data
        NFTCollectionData collection = JsonUtility.FromJson<NFTCollectionData>(dataJSON);

        // Save the data into visual scripting object
        if (collection != null)
        {
            Variables.Application.Set(VisualScriptingVariables.UserOwnedNFTCollectionData.ToString(), collection);
        }

        // Dispatch Unity delegate event
        OnGetUserOwnedNFTCollectionDataCallback?.Invoke();
    }

    public void GetListedNFTCollectionData(string dataJSON)
    {
        // Deserialize the JSON data
        ListedNFTCollectionData collection = JsonUtility.FromJson<ListedNFTCollectionData>(dataJSON);

        // Save the data into visual scripting object
        if (collection != null)
        {
            Variables.Application.Set(VisualScriptingVariables.ListedNFTCollectionData.ToString(), collection);
        }

        // Dispatch Unity delegate event
        OnGetListedNFTDataCallback?.Invoke();
    }

    public void GetUserQuestsData(string questJSON)
    {
        //Deserialize the JSON quest data
        QuestCollectionData quests = JsonUtility.FromJson<QuestCollectionData>(questJSON);

        if (quests != null)
        {
            Variables.Application.Set(VisualScriptingVariables.QuestCollectionData.ToString(), quests);
        }

        // Dispatch Unity delegate event
        OnGetQuestDataCallback?.Invoke();
    }
}