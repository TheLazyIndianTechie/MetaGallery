using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class OwnedNFTGrid : MonoBehaviour
{
    [SerializeField] private RectTransform grid;
    [SerializeField] private float factor = 50;
    [SerializeField] private GameObject gridItemContent, ownedNFTPanel;

	private readonly List<NFTData> nftDatas = new();
    private readonly List<GameObject> nftList = new();

	public static event Action<NFTData> OnNFTSelected;

	private void Awake()
    {
		// Load the data and popuulate it in the UI Canvas
		PopulateNFTCollection();
		UpdatePanel();
	}

	private void PopulateNFTCollection()
	{
		nftDatas.Clear();
		Debug.Log("After populate NFT, clearing" + nftDatas.Count);

		NFTCollectionData nftCollection = (NFTCollectionData)Variables.Application.Get(VisualScriptingVariables.UserOwnedNFTCollectionData.ToString());
		nftDatas.AddRange(nftCollection.nfts);
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
			gameObject.transform.GetComponent<Button>().AddEventListener(i, ItemClicked);
			nftList.Add(gameObject);
		}

		if (count > 1)
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

	private void ItemClicked(int itemIndex)
    {
		OnNFTSelected?.Invoke(nftDatas[itemIndex]);
    }
}

public static class ButtonExtension
{
	public static void AddEventListener<T>(this Button button, T param, Action<T> OnClick)
	{
		button.onClick.AddListener(delegate () {
			OnClick(param);
		});
	}
}