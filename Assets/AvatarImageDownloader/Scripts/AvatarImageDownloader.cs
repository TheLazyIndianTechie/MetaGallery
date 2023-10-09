using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AvatarImageDownloader: MonoBehaviour
{
	[SerializeField] private Image avatarImage;

	// Start is called before the first frame update
	void Start()
	{
		UserData userData = (UserData)Variables.Application.Get("UserDataJSON");

		StartCoroutine(SetImage(avatarImage, userData.avatarUrl));
	}


	IEnumerator SetImage(Image avatarImage, string url)
	{
		string currentAvatarUrl = url;

		Debug.Log("Avatar model url is " + url);

		url = currentAvatarUrl.Replace("glb", "png");

		Debug.Log("The new image url is" + url);


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

			if (avatarImage != null)
			{
				avatarImage.overrideSprite = sprite;
			}
		}
	}
}
