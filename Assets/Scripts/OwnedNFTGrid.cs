using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.Networking;
using Unity.VisualScripting;

public class OwnedNFTGrid : MonoBehaviour
{
    [SerializeField] private RectTransform grid;
    [SerializeField] private float factor = 50;
    [SerializeField] private GameObject gridItemContent, ownedNFTPanel;

	// Testing string for Editor purposes
	private string json = "{\"nfts\":[{\"collectionId\":0,\"nftId\":0,\"owner\":{\"AccountId\":\"5D7sGJu7iCXMNbTdHBDt3irFydbPaaSM5HUPqjiV1RtPSNfx\"},\"metadata\":\"https://ipfs.io/ipfs/QmQK5R4j7rM8bcNsJnJE6VQWcNTSQmV6kR1EcwTxodGDHe\",\"cost\":320000,\"nftName\":\"BoredApeYachtClub #3329\",\"description\":\"BoredApeYachtClub #3329\"},{\"collectionId\":0,\"nftId\":1,\"owner\":{\"AccountId\":\"5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY\"},\"metadata\":\"https://ipfs.io/ipfs/QmRZA3Bf1nJAYEGJYGsxZEWPfjpX2hpeS4fmdGaK5SdB5b\",\"cost\":2100000,\"nftName\":\"BoredApeYachtClub #3634\",\"description\":\"BoredApeYachtClub #3634\"},{\"collectionId\":0,\"nftId\":7,\"owner\":{\"AccountId\":\"5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY\"},\"metadata\":\"https://ipfs.io/ipfs/QmbRmz6c2a4FUF7qr8jJ1QBvjwnTFNYkM6mSyGTf1PHx3R\",\"cost\":6500000,\"nftName\":\"BoredApeYachtClub #4651\",\"description\":\"BoredApeYachtClub #4651\"},{\"collectionId\":0,\"nftId\":8,\"owner\":{\"AccountId\":\"5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY\"},\"metadata\":\"https://ipfs.io/ipfs/QmQYLq7cCtM9xy37eAiwJhJG25CfxDTunkuxV5Zf7cJzHR\",\"cost\":430000,\"nftName\":\"BoredApeYachtClub #527\",\"description\":\"BoredApeYachtClub #527\"},{\"collectionId\":0,\"nftId\":9,\"owner\":{\"AccountId\":\"5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY\"},\"metadata\":\"https://ipfs.io/ipfs/QmZwfgoWzTqo7c13A63XjcWcw3tJUEksfHiUbikgeUDvEt\",\"cost\":540000,\"nftName\":\"BoredApeYachtClub #8914\",\"description\":\"BoredApeYachtClub #8914\"},{\"collectionId\":0,\"nftId\":10,\"owner\":{\"AccountId\":\"5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY\"},\"metadata\":\"https://ipfs.io/ipfs/QmVersEsEXuANWjgWKL2UVex9AJp1Yzh6CsLYuiQuLbWjp\",\"cost\":650000,\"nftName\":\"BoredApeYachtClub #2193\",\"description\":\"BoredApeYachtClub #2193\"},{\"collectionId\":0,\"nftId\":11,\"owner\":{\"AccountId\":\"5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY\"},\"metadata\":\"https://ipfs.io/ipfs/QmXpW2aZtGgYWxVJF3XtvKsYMFPzffZBrVckhkBsPudFTp\",\"cost\":760000,\"nftName\":\"BoredApeYachtClub #9712\",\"description\":\"BoredApeYachtClub #9712\"}]}";

    private List<NFTData> nftDatas = new List<NFTData>();
    private List<GameObject> nftList = new List<GameObject>();
    // private int selectedItemIndex;
    // private GameObject nftMaskParent;

	private void OnEnable()
	{
		// Subscribe to events
		ReactDataManager.OnGetUserOwnedNFTCollectionDataCallback += RefreshData;

		// Load the data and popuulate it in the UI Canvas
		PopulateNFTCollection();
		UpdatePanel();
	}

	private void OnDisable()
	{
		ReactDataManager.OnGetUserOwnedNFTCollectionDataCallback -= RefreshData;
	}

	private void RefreshData()
	{
		PopulateNFTCollection();

		UpdatePanel();
	}

	private void CloseAllOpenedCanvas()
	{
		ownedNFTPanel.SetActive(false);
	}

	private void UpdatePanel()
	{
		if (nftList.Count > 0)
		{
			for (int i = 0; i < nftList.Count; i++)
			{
				Destroy(nftList[i]);
			}

			nftList.Clear();
		}

		gridItemContent.SetActive(true);

		GameObject gameObject;

		int count = nftDatas.Count;

		for (int i = 0; i < count; i++)
		{
			gameObject = Instantiate(gridItemContent, transform);
			StartCoroutine(SetImage(gameObject, nftDatas[i].metadata));
			gameObject.transform.Find("t_NFT").GetComponent<TextMeshProUGUI>().text = nftDatas[i].nftName;
			nftList.Add(gameObject);
		}

		if (count > 4)
		{
			grid.localPosition = new Vector3(grid.localPosition.x + (count * factor), grid.localPosition.y, grid.localPosition.z);
		}

		gridItemContent.SetActive(false);
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
			gameObject.transform.Find("img_NFT").GetComponent<Image>().overrideSprite = sprite;
		}
	}

	/*private void ItemClicked(int itemIndex)
	{
		selectedItemIndex = itemIndex;

		StartCoroutine(SetDetailsImage(nftMaskParent, nftDatas[itemIndex].metadata));

		nftDetails.SetActive(true);
		nftMaskParent.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = nftDatas[itemIndex].nftName;
		nftMaskParent.transform.Find("Buy_Panel").transform.Find("t_Price").GetComponent<TextMeshProUGUI>().text = nftDatas[itemIndex].GetFormattedCost();
		nftMaskParent.transform.Find("Buy_Panel").transform.Find("btn_Purchase").transform.Find("t_Purchase").GetComponent<TextMeshProUGUI>().text = isNFTWall ? "Sell" : "Buy";
		nftMaskParent.transform.Find("Buy_Panel").transform.Find("btn_FrameRemove").gameObject.SetActive(isNFTWall);
		nftMaskParent.transform.Find("Buy_Panel").transform.Find("t_Price").gameObject.SetActive(!isNFTWall);
		nftMaskParent.transform.Find("Buy_Panel").transform.Find("img_Coin").gameObject.SetActive(!isNFTWall);
		nftMaskParent.transform.Find("Description Panel").transform.Find("Description").GetComponent<TextMeshProUGUI>().text = nftDatas[itemIndex].description;
		nftMaskParent.transform.Find("Owner").GetComponent<TextMeshProUGUI>().text = nftDatas[itemIndex].owner.AccountId;

		// Fetch the UserData which contains the details about current NFT displayed in the NFT wall
		UserData userData = (UserData)Variables.Application.Get(VisualScriptingVariables.UserDataJSON.ToString());

		// Hide the Buy button in the NFTMarketplace canvas if the current user is the owner of the NFT
		if (!isNFTWall)
		{
			nftMaskParent.transform.Find("Buy_Panel").transform.Find("btn_Purchase").gameObject.SetActive(nftDatas[itemIndex].owner.AccountId != userData.accountId);
			nftMaskParent.transform.Find("Buy_Panel").transform.Find("OwnedListed").gameObject.SetActive(nftDatas[itemIndex].owner.AccountId == userData.accountId);
		}

		if (userData.currentNft.Count == 2)
		{
			// Toggle the text for the FrameRemove button if the canvas is loading a NFTWall
			bool isFramed = userData.currentNft[0] == nftDatas[itemIndex].collectionId && userData.currentNft[1] == nftDatas[itemIndex].nftId;
			nftMaskParent.transform.Find("Buy_Panel").transform.Find("btn_FrameRemove").transform.Find("t_FrameRemove").GetComponent<TextMeshProUGUI>().text = isFramed ? "Remove" : "Frame";
		}
	}*/

	/*IEnumerator SetDetailsImage(GameObject gameObject, string url)
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
			gameObject.transform.GetChild(0).GetComponent<Image>().overrideSprite = sprite;
			gameObject.transform.GetChild(2).GetComponent<Image>().overrideSprite = sprite;
		}
	}*/

	/*public void PurchaseNFT()
	{
#if !(UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_OSX || UNITY_EDITOR_WIN)
		purchaseLoadingPanel?.SetActive(true);		
		
		NFTData currentNFTData = nftDatas[selectedItemIndex];
		Debug.Log("NFT Marketplace calling: " + currentNFTData.collectionId + " , " + currentNFTData.nftId + " , " + currentNFTData.cost + " , " + gameObject.name + " , " + nameof(UpdateNFTCollectionData) + " , " + nameof(FetchUserData));
		if(isNFTWall)
        {
			// Open dialog for setting the price before initiating the Sell flow
			userInputDialog.SetActive(true);
        } else
        {
			// React Buy NFT flow
			ReactDataManager.Instance.CallBuyNFT(gameObject.name, nameof(UpdateNFTCollectionData), nameof(FetchUserData), currentNFTData.collectionId, currentNFTData.nftId, currentNFTData.cost);
		}
#else
		if (isNFTWall)
		{
			// Open dialog for setting the price before initiating the Sell flow
			userInputDialog.SetActive(true);
		}
		else
		{
			StartCoroutine(LocallyUpdateNFTCollectionData());
		}
#endif
	}*/

	/*public void SellNFT()
	{
		// Show loading panel
		purchaseLoadingPanel?.SetActive(true);

		//Read Input from Player
		var inputText = userPriceInput.GetComponent<TMP_InputField>().text;

		if (!string.IsNullOrEmpty(inputText))
		{
			NFTData currentNFTData = nftDatas[selectedItemIndex];
			int userInputPrice = int.Parse(inputText);

#if !(UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_OSX || UNITY_EDITOR_WIN)
		ReactDataManager.Instance.CallSellNFT(currentNFTData.collectionId, currentNFTData.nftId, userInputPrice);
#endif
			// Hide loading panel
			purchaseLoadingPanel?.SetActive(false);

			CloseAllOpenedCanvas();
		}
		else
		{
			// Hide loading panel
			purchaseLoadingPanel?.SetActive(false);

			Debug.LogError("Input price is not provided. Prompt the user to enter a valid input before clicking the Sell button.");
		}
	}*/

	/*public void SetCurrentNFT()
	{
		// Fetch the UserData which contains the details about current NFT displayed in the NFT wall
		UserData userData = (UserData)Variables.Application.Get(VisualScriptingVariables.UserDataJSON.ToString());

		NFTData currentNFTData = nftDatas[selectedItemIndex];
		bool isFramed = false;

		if (userData.currentNft.Count == 2)
		{
			isFramed = userData.currentNft[0] == currentNFTData.collectionId && userData.currentNft[1] == currentNFTData.nftId;
		}

#if !(UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_OSX || UNITY_EDITOR_WIN)
		ReactDataManager.Instance.CallSetCurrentNFT(gameObject.name, nameof(UpdateCurrentNFT), isFramed ? -1 : currentNFTData.collectionId, isFramed ? -1 : currentNFTData.nftId);
#else
		// Update locally the collection and nft id and save it in visual scripting object
		userData.selectedLocalNft = new List<int>() { isFramed ? -1 : currentNFTData.collectionId, isFramed ? -1 : currentNFTData.nftId };
		Variables.Application.Set(VisualScriptingVariables.UserDataJSON.ToString(), userData);

		UpdateCurrentNFT(1);
#endif
	}*/

	/*IEnumerator LocallyUpdateNFTCollectionData()
	{
		purchaseLoadingPanel?.SetActive(true);
		yield return new WaitForSeconds(6);

		UpdateNFTCollectionData(json);

		UserData userData = new UserData();
		userData.balance = 1;
		FetchUserData(JsonUtility.ToJson(userData));
	}*/
	
	private void PopulateNFTCollection()
	{
		nftDatas.Clear();
		Debug.Log("After populate NFT, clearing" + nftDatas.Count);

		NFTCollectionData nftCollection = (NFTCollectionData)Variables.Application.Get(VisualScriptingVariables.UserOwnedNFTCollectionData.ToString());
		nftDatas.AddRange(nftCollection.nfts);
	}

	// Called from React app

	// React callback for Sell NFT event
	/*public void UpdateNFTCollectionData(string nftsInJSON)
	{
		ReactDataManager.Instance.GetNFTCollectionData(nftsInJSON);
	}*/

	// React callback for Buy NFT event
	public void FetchUserData(string userDataJSON)
	{
		ReactDataManager.Instance.GetUserData(userDataJSON);
	}

	// React callback for Sell NFT event
	public void UpdateUserOwnedNNFTCollectionData(string nftJSON)
	{
		ReactDataManager.Instance.GetUserOwnedNFTCollectionData(nftJSON);
	}

	// React callback for Set Current NFT event
	public void UpdateCurrentNFT(int status)
	{
		CloseAllOpenedCanvas();
		ReactDataManager.Instance.GetCurrentNFTModifyStatus(status);
	}
}
