using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using Unity.VisualScripting;

public class InitializeGame : MonoBehaviour
{
    [SerializeField]
    private TMP_Text loadingStatus;

    // Testing string for Editor purposes
    private string json = "{\"nfts\":[{\"collectionId\":0,\"nftId\":0,\"owner\":{\"AccountId\":\"5D7sGJu7iCXMNbTdHBDt3irFydbPaaSM5HUPqjiV1RtPSNfx\"},\"metadata\":\"https://ipfs.io/ipfs/QmQK5R4j7rM8bcNsJnJE6VQWcNTSQmV6kR1EcwTxodGDHe\",\"cost\":320000,\"nftName\":\"BoredApeYachtClub #3329\",\"description\":\"BoredApeYachtClub #3329\"},{\"collectionId\":0,\"nftId\":1,\"owner\":{\"AccountId\":\"5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY\"},\"metadata\":\"https://ipfs.io/ipfs/QmRZA3Bf1nJAYEGJYGsxZEWPfjpX2hpeS4fmdGaK5SdB5b\",\"cost\":2100000,\"nftName\":\"BoredApeYachtClub #3634\",\"description\":\"BoredApeYachtClub #3634\"},{\"collectionId\":0,\"nftId\":7,\"owner\":{\"AccountId\":\"5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY\"},\"metadata\":\"https://ipfs.io/ipfs/QmbRmz6c2a4FUF7qr8jJ1QBvjwnTFNYkM6mSyGTf1PHx3R\",\"cost\":6500000,\"nftName\":\"BoredApeYachtClub #4651\",\"description\":\"BoredApeYachtClub #4651\"},{\"collectionId\":0,\"nftId\":8,\"owner\":{\"AccountId\":\"5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY\"},\"metadata\":\"https://ipfs.io/ipfs/QmQYLq7cCtM9xy37eAiwJhJG25CfxDTunkuxV5Zf7cJzHR\",\"cost\":430000,\"nftName\":\"BoredApeYachtClub #527\",\"description\":\"BoredApeYachtClub #527\"},{\"collectionId\":0,\"nftId\":9,\"owner\":{\"AccountId\":\"5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY\"},\"metadata\":\"https://ipfs.io/ipfs/QmZwfgoWzTqo7c13A63XjcWcw3tJUEksfHiUbikgeUDvEt\",\"cost\":540000,\"nftName\":\"BoredApeYachtClub #8914\",\"description\":\"BoredApeYachtClub #8914\"},{\"collectionId\":0,\"nftId\":10,\"owner\":{\"AccountId\":\"5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY\"},\"metadata\":\"https://ipfs.io/ipfs/QmVersEsEXuANWjgWKL2UVex9AJp1Yzh6CsLYuiQuLbWjp\",\"cost\":650000,\"nftName\":\"BoredApeYachtClub #2193\",\"description\":\"BoredApeYachtClub #2193\"},{\"collectionId\":0,\"nftId\":11,\"owner\":{\"AccountId\":\"5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY\"},\"metadata\":\"https://ipfs.io/ipfs/QmXpW2aZtGgYWxVJF3XtvKsYMFPzffZBrVckhkBsPudFTp\",\"cost\":760000,\"nftName\":\"BoredApeYachtClub #9712\",\"description\":\"BoredApeYachtClub #9712\"}]}";

    private string questjson = "";

    // Lifecycle methods
    public void OnPlayPressed()
    {
        loadingStatus.SetText("User data is being initialized...");

#if !UNITY_EDITOR
        ReactDataManager.Instance.CallGetUserData(gameObject.name, nameof(FetchUserData));
        Debug.Log(("Unity Initialization "+ nameof(FetchUserData) + " completed successfully"));
        ReactDataManager.Instance.CallGetQuestData(gameObject.name, nameof(FetchQuestData));
        Debug.Log("Unity Initialize Log " + nameof(FetchQuestData) + " completed successfully");
#else
        FetchUserData("");
        FetchQuestData("");
#endif
    }

    public void FetchUserData(string userDataJSON)
    {
#if !UNITY_EDITOR
        ReactDataManager.Instance.GetUserData(userDataJSON);
        Debug.Log("Unity Debug Log - Getting User Data " + userDataJSON);
        
        loadingStatus.text = "Art data is being initialized...";

        ReactDataManager.Instance.CallGetUserOwnedNFTCollectionData(gameObject.name, nameof(UpdateUserOwnedNFT));
#else
        UserData userData = new UserData();

        Debug.Log("Debugging User Data Read" + JsonUtility.ToJson(userData));

        ReactDataManager.Instance.GetUserData(JsonUtility.ToJson(userData));
        

        UpdateUserOwnedNFT(json);
#endif
    }
    public void FetchQuestData(string questJSON)
    {

#if !UNITY_EDITOR
        ReactDataManager.Instance.GetUserQuestsData(questJSON);

        Debug.Log("Unity Debug Log - Getting Quest Data " + questJSON);

        loadingStatus.text = "Retrieving list of quests...";

        
#else
        QuestCollectionData questCollectionData = (QuestCollectionData)Variables.Application.Get("QuestCollectionData");
        /*QuestData questData = new();
        QuestData questData1 = new(9);
        questCollectionData.quests.Add(questData);
        questCollectionData.quests.Add(questData1);*/

        Debug.Log("Debugging User Data Read" + JsonUtility.ToJson(questCollectionData));

        ReactDataManager.Instance.GetUserQuestsData(JsonUtility.ToJson(questCollectionData));
#endif

    }


    public void UpdateUserOwnedNFT(string userNftJSON)
    {
        ReactDataManager.Instance.GetUserOwnedNFTCollectionData(userNftJSON);
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