using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Manager/GameSetting")]
public class GameSettings : ScriptableObject
{
    [SerializeField] private string _gameVersion = "0.0.0";

    [SerializeField] private string _nickName = "User_";

    [SerializeField] private string _roomName = "art_gallery_0";

    public string GameVersion { get { return _gameVersion; } }

    public string NickName
    {
        get
        {
            int value = Random.Range(1000, 9999);
            return _nickName + value.ToString();
        }
    }

    public string RoomName { get { return _roomName; } }
}
