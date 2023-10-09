using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class GalleryUIController : MonoBehaviour
{
    private bool _isInfoShown = false;
    private bool _isPreviewShown = false;
    [Header("Controls")]
    [SerializeField] GameObject infoOverlay;
    [Header("Art Preview")]
    [SerializeField] GameObject previewOverlay;
    [SerializeField] Image imageNFT;
    [SerializeField] GameObject videoNFT;
    [SerializeField] Image profileImage;
    [SerializeField] TextMeshProUGUI artistName;
    [SerializeField] GameObject content;

    public delegate void UIOverlayState(bool isEnabled);
    public static event UIOverlayState OnStateChange;

   /* private void OnEnable()
    {
        ObjectClickDetector.OnTap += ShowPreviewOverlay;
    }

    private void OnDisable()
    {
        ObjectClickDetector.OnTap -= ShowPreviewOverlay;
    }*/

    public void OnClickLeave()
    {
        if (!_isInfoShown)
        {
            //PhotonNetwork.Disconnect();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }

    public void OnClickInfo()
    {
        if (!_isInfoShown)
        {
            _isInfoShown = true;
            infoOverlay.SetActive(true);
            NotifyStateChange();
        }
    }

    public void OnClickCloseInfo()
    {
        if (_isInfoShown)
        {
            _isInfoShown = false;
            infoOverlay.SetActive(false);
            NotifyStateChange();
        }
    }

    public void OnClickClosePreview()
    {
        if(_isPreviewShown)
        {
            _isPreviewShown = false;
            previewOverlay.SetActive(false);
            NotifyStateChange();
        }
    }

    private void NotifyStateChange()
    {
        OnStateChange(_isInfoShown || _isPreviewShown);
    }

    private void ShowPreviewOverlay(Art art)
    {
        _isPreviewShown = true;
        previewOverlay.SetActive(true);
        NotifyStateChange();

        profileImage.sprite = art.profileImage;

        artistName.text = art.artistName;

        var artistBio = content.GetComponent<TextMeshProUGUI>();
        var bio = art.artistBio.Replace("\\n", "\n");
        artistBio.text = bio;

        if(art.isImageNFT)
        {
            imageNFT.transform.gameObject.SetActive(true);
            videoNFT.SetActive(false);
            imageNFT.sprite = art.imageNFT;
        } else
        {
            imageNFT.transform.gameObject.SetActive(false);
            videoNFT.SetActive(true);
            var videoPlayer = videoNFT.GetComponent<VideoPlayer>();
            videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, art.videoFile);
        }
    }
}
