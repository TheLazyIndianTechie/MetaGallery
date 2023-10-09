using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ArtDisplay : MonoBehaviour
{
    [Header("Scriptable object")]
    [SerializeField] private Art art;

    [Header("Game objects")]
    [SerializeField] private GameObject artistName;
    [SerializeField] private GameObject imageCanvas;
    [SerializeField] private GameObject video;
    [SerializeField] private Image image;

    private TextMeshPro nameText;
    private VideoPlayer videoPlayer;


    // Start is called before the first frame update
    void Start()
    {
        nameText = artistName.GetComponent<TextMeshPro>();
        videoPlayer = video.GetComponent<VideoPlayer>();

        nameText.text = art.artistName;
        if(art.isImageNFT)
        {
            image.sprite = art.imageNFT;

            imageCanvas.SetActive(true);
            video.SetActive(false);
        } else
        {
            videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, art.videoFile);

            imageCanvas.SetActive(false);
            video.SetActive(true);
        }
    }

    public Art GetArt()
    {
        return art;
    }
}
