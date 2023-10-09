using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class OwnedNFTDetailsCanvasManager : MonoBehaviour
{
   
    [SerializeField] private Sprite loadingSprite;
    [SerializeField] private GameObject loadingPanel, imgArt, btnSelectArt, btnPrimaryAction;
    [SerializeField] private TMP_Text tArtName, tArtDescription, tPrimaryButton;

    private Canvas _canvas;
    private ListedNFTCollectionData _listedNFTCollectionData  = new();
    private int _currentFrameID = 0;

    private string nftJSON = "{\"nfts\":[{\"collectionId\":0,\"nftId\":0,\"owner\":{\"AccountId\":\"5D7sGJu7iCXMNbTdHBDt3irFydbPaaSM5HUPqjiV1RtPSNfx\"},\"metadata\":\"https://ipfs.io/ipfs/QmQK5R4j7rM8bcNsJnJE6VQWcNTSQmV6kR1EcwTxodGDHe\",\"cost\":320000,\"nftName\":\"BoredApeYachtClub #3329\",\"description\":\"BoredApeYachtClub #3329\"},{\"collectionId\":0,\"nftId\":1,\"owner\":{\"AccountId\":\"5D7sGJu7iCXMNbTdHBDt3irFydbPaaSM5HUPqjiV1RtPSNfx\"},\"metadata\":\"https://ipfs.io/ipfs/QmRZA3Bf1nJAYEGJYGsxZEWPfjpX2hpeS4fmdGaK5SdB5b\",\"cost\":2100000,\"nftName\":\"BoredApeYachtClub #3634\",\"description\":\"BoredApeYachtClub #3634\"},{\"collectionId\":0,\"nftId\":7,\"owner\":{\"AccountId\":\"5D7sGJu7iCXMNbTdHBDt3irFydbPaaSM5HUPqjiV1RtPSNfx\"},\"metadata\":\"https://ipfs.io/ipfs/QmbRmz6c2a4FUF7qr8jJ1QBvjwnTFNYkM6mSyGTf1PHx3R\",\"cost\":6500000,\"nftName\":\"BoredApeYachtClub #4651\",\"description\":\"BoredApeYachtClub #4651\"},{\"collectionId\":0,\"nftId\":8,\"owner\":{\"AccountId\":\"5D7sGJu7iCXMNbTdHBDt3irFydbPaaSM5HUPqjiV1RtPSNfx\"},\"metadata\":\"https://ipfs.io/ipfs/QmQYLq7cCtM9xy37eAiwJhJG25CfxDTunkuxV5Zf7cJzHR\",\"cost\":430000,\"nftName\":\"BoredApeYachtClub #527\",\"description\":\"BoredApeYachtClub #527\"},{\"collectionId\":0,\"nftId\":9,\"owner\":{\"AccountId\":\"5D7sGJu7iCXMNbTdHBDt3irFydbPaaSM5HUPqjiV1RtPSNfx\"},\"metadata\":\"https://ipfs.io/ipfs/QmZwfgoWzTqo7c13A63XjcWcw3tJUEksfHiUbikgeUDvEt\",\"cost\":540000,\"nftName\":\"BoredApeYachtClub #8914\",\"description\":\"BoredApeYachtClub #8914\"},{\"collectionId\":0,\"nftId\":10,\"owner\":{\"AccountId\":\"5D7sGJu7iCXMNbTdHBDt3irFydbPaaSM5HUPqjiV1RtPSNfx\"},\"metadata\":\"https://ipfs.io/ipfs/QmVersEsEXuANWjgWKL2UVex9AJp1Yzh6CsLYuiQuLbWjp\",\"cost\":650000,\"nftName\":\"BoredApeYachtClub #2193\",\"description\":\"BoredApeYachtClub #2193\"},{\"collectionId\":0,\"nftId\":11,\"owner\":{\"AccountId\":\"5D7sGJu7iCXMNbTdHBDt3irFydbPaaSM5HUPqjiV1RtPSNfx\"},\"metadata\":\"https://ipfs.io/ipfs/QmXpW2aZtGgYWxVJF3XtvKsYMFPzffZBrVckhkBsPudFTp\",\"cost\":760000,\"nftName\":\"BoredApeYachtClub #9712\",\"description\":\"BoredApeYachtClub #9712\"}]}";
    private string listedNftJSON = "{\"listedNfts\":[{\"frameId\":1,\"nft\":{\"collectionId\":0,\"nftId\":0,\"nftName\":\"BoredApeYachtClub#3329\",\"description\":\"BoredApeYachtClub#3329\",\"metadata\":\"https://ipfs.io/ipfs/QmQK5R4j7rM8bcNsJnJE6VQWcNTSQmV6kR1EcwTxodGDHe\",\"cost\":1000200000000,\"owner\":{\"AccountId\":\"5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY\"}}},{\"frameId\":2,\"nft\":{\"collectionId\":0,\"nftId\":1,\"nftName\":\"BoredApeYachtClub#3634\",\"description\":\"BoredApeYachtClub#3634\",\"metadata\":\"https://ipfs.io/ipfs/QmRZA3Bf1nJAYEGJYGsxZEWPfjpX2hpeS4fmdGaK5SdB5b\",\"cost\":2000200000000,\"owner\":{\"AccountId\":\"5D7sGJu7iCXMNbTdHBDt3irFydbPaaSM5HUPqjiV1RtPSNfx\"}}},{\"frameId\":3,\"nft\":{\"collectionId\":0,\"nftId\":2,\"nftName\":\"BoredApeYachtClub#4651\",\"description\":\"BoredApeYachtClub#4651\",\"metadata\":\"https://ipfs.io/ipfs/QmbRmz6c2a4FUF7qr8jJ1QBvjwnTFNYkM6mSyGTf1PHx3R\",\"cost\":3000200000000,\"owner\":{\"AccountId\":\"5D7sGJu7iCXMNbTdHBDt3irFydbPaaSM5HUPqjiV1RtPSNfx\"}}}]}";

    public static event Action<int> OnSelectArtTriggered;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
        UpdateCanvasState(false);
        loadingPanel.SetActive(false);
    }

    private void OnEnable()
    {
        InputDisplayCanvasManager.onArtListtriggered += UpdateArtFrameAt;
        OwnedNFTCanvasManager.OnSelectedNFTListed += OnClosePressed;
        ReactDataManager.OnGetListedNFTDataCallback += UpdateUserOwnedNFTList;
    }

    private void OnDisable()
    {
        InputDisplayCanvasManager.onArtListtriggered -= UpdateArtFrameAt;
        OwnedNFTCanvasManager.OnSelectedNFTListed -= OnClosePressed;
        ReactDataManager.OnGetListedNFTDataCallback -= UpdateUserOwnedNFTList;
    }

    private void UpdateArtFrameAt(int artFrameID)
    {
        Debug.Log("Unity Debug Log - _currentFrameID inside UpdateArtFrameAt() is " + artFrameID);
        if (artFrameID != -1)
        {
            _currentFrameID = artFrameID;

            ResetCanvasContent();

            UpdateCanvasState(true);

            // Search the Dictionary for any value corresponding to artFrameID key
            // Fetch all the Listed NFTs from the Application Variable
            _listedNFTCollectionData = (ListedNFTCollectionData)Variables.Application.Get(VisualScriptingVariables.ListedNFTCollectionData.ToString());
            ListedNFTData listedNFTData = _listedNFTCollectionData.listedNfts.Find(item => item.frameId == _currentFrameID);
            bool isFrameEmpty = listedNFTData == null;

            btnSelectArt.SetActive(isFrameEmpty);

            if (!isFrameEmpty)
            {
                // Check if the art is owned by the current user
                UserData userData = (UserData)Variables.Application.Get(VisualScriptingVariables.UserDataJSON.ToString());
                bool isArtOwnedByUser = listedNFTData.nft.owner.AccountId == userData.accountId;

                btnPrimaryAction.SetActive(true);
                btnPrimaryAction.GetComponentInChildren<TMP_Text>().text = isArtOwnedByUser ? "DELIST ART" : "BUY ART";

                tArtName.text = listedNFTData.nft.nftName;
                tArtDescription.text = listedNFTData.nft.description;
                StartCoroutine(SetImage(imgArt, listedNFTData.nft.metadata));
            }
        }
    }

    private void ResetCanvasContent()
    {
        btnPrimaryAction.SetActive(false);
        tArtName.text = "...";
        tArtDescription.text = "...";
        imgArt.transform.GetComponent<Image>().sprite = loadingSprite;
        imgArt.transform.GetComponent<Image>().color = new Color32(181, 181, 181, 255);
    }

    IEnumerator SetImage(GameObject gameObject, string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.DataProcessingError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(request.result);
        }
        else
        {
            Texture2D tex = ((DownloadHandlerTexture)request.downloadHandler).texture;
            Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));
            gameObject.transform.GetComponent<Image>().sprite = sprite;
        }
    }

    private void UpdateCanvasState(bool isEnabled)
    {
        _canvas.enabled = isEnabled;
    }

    private void BuyNFT(ListedNFTData listedNFTData)
    {
#if !UNITY_EDITOR
        Debug.Log("Unity Debug Log - _currentFrameID inside BuyNFT() is " + _currentFrameID);
        ReactDataManager.Instance.CallBuyListedNFT(gameObject.name, nameof(UpdateListedNFTCollectionData), _currentFrameID, listedNFTData.nft.collectionId, listedNFTData.nft.nftId, listedNFTData.nft.cost);
#else
        Debug.Log("Unity Debug Log - _currentFrameID inside BuyNFT() is " + _currentFrameID);
        UpdateListedNFTCollectionData(listedNftJSON);
#endif
    }

    private void DeListNFT(ListedNFTData listedNFTData)
    {
#if !UNITY_EDITOR
        Debug.Log("Unity Debug Log - _currentFrameID inside DeListNFT() is " + _currentFrameID);
        ReactDataManager.Instance.CallDeListNFT(gameObject.name, nameof(UpdateListedNFTCollectionData), _currentFrameID, listedNFTData.nft.collectionId, listedNFTData.nft.nftId);
#else
        Debug.Log("Unity Debug Log - _currentFrameID inside DeListNFT() is " + _currentFrameID);
        UpdateListedNFTCollectionData("{\"listedNfts\":[]}");
#endif
    }

    private void UpdateUserOwnedNFTList()
    {
        TMP_Text loadingStatus = loadingPanel.GetComponentInChildren<TMP_Text>();
        if(loadingStatus != null)
        {
            loadingStatus.SetText("Updating your NFT collection...");
        }

#if !UNITY_EDITOR
        ReactDataManager.Instance.CallGetUserOwnedNFTCollectionData(gameObject.name, nameof(UpdateUserOwnedNFTCollectionData));
        Debug.Log(("Unity Initialization "+ nameof(SaveUserOwnedNFTCollectionData) + " completed successfully"));
#else
        UpdateUserOwnedNFTCollectionData(nftJSON);
#endif
    }

    public void OnSelectArtPressed()
    {
        OnSelectArtTriggered?.Invoke(_currentFrameID);
    }

    public void OnPrimaryActionPressed()
    {
        // Show loading panel
        loadingPanel.SetActive(true);

        // DeList/Buy NFT based on NFT owner
        ListedNFTData listedNFTData = _listedNFTCollectionData.listedNfts.Find(item => item.frameId == _currentFrameID);
        UserData userData = (UserData)Variables.Application.Get(VisualScriptingVariables.UserDataJSON.ToString());
        bool isArtOwnedByUser = listedNFTData.nft.owner.AccountId == userData.accountId;

        if(isArtOwnedByUser)
        {
            // DELIST NFT
            DeListNFT(listedNFTData);
        }
        else {
            // BUY NFT
            BuyNFT(listedNFTData);
        }
    }

    public void OnClosePressed()
    {
        _currentFrameID = 0;
        WowEventManager.TriggerEvent(nameof(WowEvents.OnPlayerResumeGame));
        loadingPanel.SetActive(false);
        UpdateCanvasState(false);
    }

    public void UpdateListedNFTCollectionData(string listedNFTCollectionJSON)
    {
        ReactDataManager.Instance.GetListedNFTCollectionData(listedNFTCollectionJSON);
        Debug.Log("Unity Debug Log - Getting Listed NFT Collection Data " + listedNFTCollectionJSON);
    }

    private void UpdateUserOwnedNFTCollectionData(string userOwnedNFTDataJSON)
    {
        ReactDataManager.Instance.GetUserOwnedNFTCollectionData(userOwnedNFTDataJSON);
        Debug.Log("Unity Debug Log - Getting User Owned NFT Collection Data " + userOwnedNFTDataJSON);
        
        OnClosePressed();
    }
}
