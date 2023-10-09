using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu (fileName = "New Art", menuName = "Art")]
public class Art: ScriptableObject
{
    public string artistName;
    public string artistBio;
    public Sprite profileImage;
    public bool isImageNFT;
#nullable enable
    public Sprite? imageNFT;
    public string? videoFile;
#nullable disable
}
