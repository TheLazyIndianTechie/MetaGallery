using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class OwnedNFTCanvasManager : MonoBehaviour
{
    private Canvas _canvas;
    private int _currentFrameID = 0;
    private NFTData _selectedNFT;

    private string listedNftJSON = "{\"listedNfts\":[{\"frameId\":1,\"nft\":{\"collectionId\":0,\"nftId\":0,\"nftName\":\"BoredApeYachtClub#3329\",\"description\":\"BoredApeYachtClub#3329\",\"metadata\":\"https://ipfs.io/ipfs/QmQK5R4j7rM8bcNsJnJE6VQWcNTSQmV6kR1EcwTxodGDHe\",\"cost\":1000200000000,\"owner\":{\"AccountId\":\"5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY\"}}},{\"frameId\":2,\"nft\":{\"collectionId\":0,\"nftId\":1,\"nftName\":\"BoredApeYachtClub#3634\",\"description\":\"BoredApeYachtClub#3634\",\"metadata\":\"https://ipfs.io/ipfs/QmRZA3Bf1nJAYEGJYGsxZEWPfjpX2hpeS4fmdGaK5SdB5b\",\"cost\":2000200000000,\"owner\":{\"AccountId\":\"5D7sGJu7iCXMNbTdHBDt3irFydbPaaSM5HUPqjiV1RtPSNfx\"}}},{\"frameId\":3,\"nft\":{\"collectionId\":0,\"nftId\":2,\"nftName\":\"BoredApeYachtClub#4651\",\"description\":\"BoredApeYachtClub#4651\",\"metadata\":\"https://ipfs.io/ipfs/QmbRmz6c2a4FUF7qr8jJ1QBvjwnTFNYkM6mSyGTf1PHx3R\",\"cost\":3000200000000,\"owner\":{\"AccountId\":\"5D7sGJu7iCXMNbTdHBDt3irFydbPaaSM5HUPqjiV1RtPSNfx\"}}}]}";

    [SerializeField] GameObject loadingPanel, listPriceInputPanel, imgSelectedNFT;
    [SerializeField] TMP_InputField inputListingPrice;

    public static event Action OnSelectedNFTListed;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
        UpdateCanvasState(false);
        loadingPanel.SetActive(false);
        inputListingPrice.text = "";
        imgSelectedNFT.transform.GetComponent<Image>().sprite = null;
        listPriceInputPanel.SetActive(false);
    }

    private void OnEnable()
    {
        OwnedNFTDetailsCanvasManager.OnSelectArtTriggered += SelectArt;
        OwnedNFTGrid.OnNFTSelected += DisplayCostInputPanel;
    }

    private void OnDisable()
    {
        OwnedNFTDetailsCanvasManager.OnSelectArtTriggered -= SelectArt;
        OwnedNFTGrid.OnNFTSelected -= DisplayCostInputPanel;
    }

    private void SelectArt(int currentFrameID)
    {
        _currentFrameID = currentFrameID;
        UpdateCanvasState(true);
    }

    private void UpdateCanvasState(bool isEnabled)
    {
        _canvas.enabled = isEnabled;
    }

    private void DisplayCostInputPanel(NFTData selectedNFT)
    {
        _selectedNFT = selectedNFT;

        // Display the Art/NFT
        imgSelectedNFT.transform.GetComponent<Image>().sprite = null;
        StartCoroutine(SetImage(imgSelectedNFT, selectedNFT.metadata));

        // Enable the UI Panel to input the cost
        listPriceInputPanel.SetActive(true);
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

    public void OnClosePressed()
    {
        _currentFrameID = 0;
        UpdateCanvasState(false);
    }

    public void OnCancelListingInputPanelPressed()
    {
        inputListingPrice.text = "";
        imgSelectedNFT.transform.GetComponent<Image>().sprite = null;
        listPriceInputPanel.SetActive(false);
    }

    public void OnConfirmListingPressed()
    {
        //Read Input from Player
        var inputText = inputListingPrice.text;

        if (!string.IsNullOrEmpty(inputText))
        {
            // Show loading panel
            loadingPanel.SetActive(true);


            ulong userInputPrice = Convert.ToUInt64(inputText);

#if !UNITY_EDITOR
		    ReactDataManager.Instance.CallListNFT(gameObject.name, nameof(UpdateListedNFTData), _currentFrameID, _selectedNFT.collectionId, _selectedNFT.nftId, userInputPrice);
#else
            UpdateListedNFTData(listedNftJSON);
#endif
        }
    }

    public void UpdateListedNFTData(string listedNFTDataJSON)
    {
        inputListingPrice.text = "";
        listPriceInputPanel.SetActive(false);
        loadingPanel.SetActive(false);
        UpdateCanvasState(false);

        ReactDataManager.Instance.GetListedNFTCollectionData(listedNFTDataJSON);
        Debug.Log("Unity Debug Log - Getting Listed NFT Collection Data " + listedNFTDataJSON);

        // Inform the system that the selected NFT was listed in the backend
        OnSelectedNFTListed?.Invoke();
    }
}
