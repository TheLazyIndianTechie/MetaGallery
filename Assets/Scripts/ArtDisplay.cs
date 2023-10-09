using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ArtDisplay : MonoBehaviour
{
    [SerializeField] private int artFrameID;

    [Header("Game objects")]
    [SerializeField] private TMP_Text artName;
    [SerializeField] private GameObject imgArt;

    private void OnEnable()
    {
        WowEventManager.StartListening(nameof(WowEvents.OnRPMAvatarLoaded), UpdateArtInfo);
        ReactDataManager.OnGetListedNFTDataCallback += UpdateArtInfo;
    }

    private void OnDisable()
    {
        WowEventManager.StopListening(nameof(WowEvents.OnRPMAvatarLoaded), UpdateArtInfo);
        ReactDataManager.OnGetListedNFTDataCallback -= UpdateArtInfo;
    }

    private void UpdateArtInfo()
    {
        // Fetch the Art Data from the Application variable;
        ListedNFTCollectionData listedNFTCollectionData = (ListedNFTCollectionData)Variables.Application.Get(VisualScriptingVariables.ListedNFTCollectionData.ToString());
        ListedNFTData listedNFTData = listedNFTCollectionData.listedNfts.Find(item => item.frameId == artFrameID);
        if(listedNFTData != null)
        {
            artName.text = listedNFTData.nft.nftName;
            StartCoroutine(SetImage(imgArt, listedNFTData.nft.metadata));
        } else
        {
            artName.text = "";
            imgArt.GetComponent<Image>().sprite = null;
        }
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
}