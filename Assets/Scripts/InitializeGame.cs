using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using Unity.VisualScripting;
using System;

public class InitializeGame : MonoBehaviour
{
    [SerializeField]
    private TMP_Text loadingStatus;

    // Testing string for Editor purposes
    private string nftJSON = "{\"nfts\":[{\"collectionId\":0,\"nftId\":0,\"owner\":{\"AccountId\":\"5D7sGJu7iCXMNbTdHBDt3irFydbPaaSM5HUPqjiV1RtPSNfx\"},\"metadata\":\"https://ipfs.io/ipfs/QmQK5R4j7rM8bcNsJnJE6VQWcNTSQmV6kR1EcwTxodGDHe\",\"cost\":320000,\"nftName\":\"BoredApeYachtClub #3329\",\"description\":\"BoredApeYachtClub #3329\"},{\"collectionId\":0,\"nftId\":1,\"owner\":{\"AccountId\":\"5D7sGJu7iCXMNbTdHBDt3irFydbPaaSM5HUPqjiV1RtPSNfx\"},\"metadata\":\"https://ipfs.io/ipfs/QmRZA3Bf1nJAYEGJYGsxZEWPfjpX2hpeS4fmdGaK5SdB5b\",\"cost\":2100000,\"nftName\":\"BoredApeYachtClub #3634\",\"description\":\"BoredApeYachtClub #3634\"},{\"collectionId\":0,\"nftId\":7,\"owner\":{\"AccountId\":\"5D7sGJu7iCXMNbTdHBDt3irFydbPaaSM5HUPqjiV1RtPSNfx\"},\"metadata\":\"https://ipfs.io/ipfs/QmbRmz6c2a4FUF7qr8jJ1QBvjwnTFNYkM6mSyGTf1PHx3R\",\"cost\":6500000,\"nftName\":\"BoredApeYachtClub #4651\",\"description\":\"BoredApeYachtClub #4651\"},{\"collectionId\":0,\"nftId\":8,\"owner\":{\"AccountId\":\"5D7sGJu7iCXMNbTdHBDt3irFydbPaaSM5HUPqjiV1RtPSNfx\"},\"metadata\":\"https://ipfs.io/ipfs/QmQYLq7cCtM9xy37eAiwJhJG25CfxDTunkuxV5Zf7cJzHR\",\"cost\":430000,\"nftName\":\"BoredApeYachtClub #527\",\"description\":\"BoredApeYachtClub #527\"},{\"collectionId\":0,\"nftId\":9,\"owner\":{\"AccountId\":\"5D7sGJu7iCXMNbTdHBDt3irFydbPaaSM5HUPqjiV1RtPSNfx\"},\"metadata\":\"https://ipfs.io/ipfs/QmZwfgoWzTqo7c13A63XjcWcw3tJUEksfHiUbikgeUDvEt\",\"cost\":540000,\"nftName\":\"BoredApeYachtClub #8914\",\"description\":\"BoredApeYachtClub #8914\"},{\"collectionId\":0,\"nftId\":10,\"owner\":{\"AccountId\":\"5D7sGJu7iCXMNbTdHBDt3irFydbPaaSM5HUPqjiV1RtPSNfx\"},\"metadata\":\"https://ipfs.io/ipfs/QmVersEsEXuANWjgWKL2UVex9AJp1Yzh6CsLYuiQuLbWjp\",\"cost\":650000,\"nftName\":\"BoredApeYachtClub #2193\",\"description\":\"BoredApeYachtClub #2193\"},{\"collectionId\":0,\"nftId\":11,\"owner\":{\"AccountId\":\"5D7sGJu7iCXMNbTdHBDt3irFydbPaaSM5HUPqjiV1RtPSNfx\"},\"metadata\":\"https://ipfs.io/ipfs/QmXpW2aZtGgYWxVJF3XtvKsYMFPzffZBrVckhkBsPudFTp\",\"cost\":760000,\"nftName\":\"BoredApeYachtClub #9712\",\"description\":\"BoredApeYachtClub #9712\"}]}";
    private string listedNftJSON = "{\"listedNfts\":[{\"frameId\":1,\"nft\":{\"collectionId\":0,\"nftId\":0,\"nftName\":\"BoredApeYachtClub#3329\",\"description\":\"BoredApeYachtClub#3329\",\"metadata\":\"https://ipfs.io/ipfs/QmQK5R4j7rM8bcNsJnJE6VQWcNTSQmV6kR1EcwTxodGDHe\",\"cost\":1000200000000,\"owner\":{\"AccountId\":\"5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY\"}}},{\"frameId\":2,\"nft\":{\"collectionId\":0,\"nftId\":1,\"nftName\":\"BoredApeYachtClub#3634\",\"description\":\"BoredApeYachtClub#3634\",\"metadata\":\"https://ipfs.io/ipfs/QmRZA3Bf1nJAYEGJYGsxZEWPfjpX2hpeS4fmdGaK5SdB5b\",\"cost\":2000200000000,\"owner\":{\"AccountId\":\"5D7sGJu7iCXMNbTdHBDt3irFydbPaaSM5HUPqjiV1RtPSNfx\"}}},{\"frameId\":3,\"nft\":{\"collectionId\":0,\"nftId\":2,\"nftName\":\"BoredApeYachtClub#4651\",\"description\":\"BoredApeYachtClub#4651\",\"metadata\":\"https://ipfs.io/ipfs/QmbRmz6c2a4FUF7qr8jJ1QBvjwnTFNYkM6mSyGTf1PHx3R\",\"cost\":3000200000000,\"owner\":{\"AccountId\":\"5D7sGJu7iCXMNbTdHBDt3irFydbPaaSM5HUPqjiV1RtPSNfx\"}}}]}";

    private string questjson = "";

    // Lifecycle methods

    private void OnEnable()
    {
        ReactDataManager.OnGetUserDataCallback += CallGetListedNFTData;
        ReactDataManager.OnGetListedNFTDataCallback += CallGetUserOwnedNFTCollectionData;
        ReactDataManager.OnGetUserOwnedNFTCollectionDataCallback += CallGetQuestsData;
        ReactDataManager.OnGetQuestDataCallback += LoadGame;
    }

    private void OnDisable()
    {
        ReactDataManager.OnGetUserDataCallback -= CallGetListedNFTData;
        ReactDataManager.OnGetListedNFTDataCallback -= CallGetUserOwnedNFTCollectionData;
        ReactDataManager.OnGetUserOwnedNFTCollectionDataCallback -= CallGetQuestsData;
        ReactDataManager.OnGetQuestDataCallback -= LoadGame;
    }

    public void OnPlayPressed()
    {
        CallGetUserData();
    }

    private void CallGetUserData()
    {
        loadingStatus.SetText("User data is being initialized...");

#if !UNITY_EDITOR
        ReactDataManager.Instance.CallGetUserData(gameObject.name, nameof(SaveUserData));
        Debug.Log(("Unity Initialization "+ nameof(SaveUserData) + " completed successfully"));
#else
        SaveUserData(JsonUtility.ToJson(new UserData()));
#endif
    }

    public void SaveUserData(string userDataJSON)
    {
        ReactDataManager.Instance.GetUserData(userDataJSON);
        Debug.Log("Unity Debug Log - Getting User Data " + userDataJSON);
    }

    private void CallGetListedNFTData()
    {
        loadingStatus.SetText("Gallery artwork is being organized...");

#if !UNITY_EDITOR
        ReactDataManager.Instance.CallGetListedNFTCollectionData(gameObject.name, nameof(SaveListedNFTData));
        Debug.Log(("Unity Initialization "+ nameof(SaveListedNFTData) + " completed successfully"));
#else
        SaveListedNFTData(listedNftJSON);
#endif
    }

    private void SaveListedNFTData(string listedNFTDataJSON)
    {
        ReactDataManager.Instance.GetListedNFTCollectionData(listedNFTDataJSON);
        Debug.Log("Unity Debug Log - Getting Listed NFT Collection Data " + listedNFTDataJSON);
    }

    private void CallGetUserOwnedNFTCollectionData()
    {
        loadingStatus.SetText("Gathering all your NFTs...");

#if !UNITY_EDITOR
        ReactDataManager.Instance.CallGetUserOwnedNFTCollectionData(gameObject.name, nameof(SaveUserOwnedNFTCollectionData));
        Debug.Log(("Unity Initialization "+ nameof(SaveUserOwnedNFTCollectionData) + " completed successfully"));
#else
        SaveUserOwnedNFTCollectionData(nftJSON);
#endif
    }

    private void SaveUserOwnedNFTCollectionData(string userOwnedNFTDataJSON)
    {
        ReactDataManager.Instance.GetUserOwnedNFTCollectionData(userOwnedNFTDataJSON);
        Debug.Log("Unity Debug Log - Getting User Owned NFT Collection Data " + userOwnedNFTDataJSON);
    }

    private void CallGetQuestsData()
    {
        loadingStatus.SetText("Retrieving quests and special events...");

#if !UNITY_EDITOR
        ReactDataManager.Instance.CallGetQuestData(gameObject.name, nameof(SaveQuestData));
        Debug.Log(("Unity Initialization "+ nameof(SaveQuestData) + " completed successfully"));
#else
        QuestCollectionData questCollectionData = (QuestCollectionData)Variables.Application.Get("QuestCollectionData");
        SaveQuestData(JsonUtility.ToJson(questCollectionData));
#endif
    }

    public void SaveQuestData(string questJSON)
    {
        ReactDataManager.Instance.GetUserQuestsData(questJSON);
        Debug.Log("Unity Debug Log - Getting Quest Data " + questJSON);
    }

    private void LoadGame()
    {
        StartCoroutine(LoadYourAsyncScene());
    }

    IEnumerator LoadYourAsyncScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            loadingStatus.text = "Loading MetaGallery: " + (asyncLoad.progress * 100) + "%";
            yield return null;
        }
    }
}