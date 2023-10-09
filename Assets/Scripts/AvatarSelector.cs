using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class AvatarSelector : MonoBehaviour
{
    [Header("Avatar")]
    [SerializeField] private Avatar sophie;
    [SerializeField] private Avatar bryce;
    [SerializeField] private Avatar kate;
    [SerializeField] private Avatar adam;

    [Header("Container")]
    [SerializeField] private GameObject parentObject;

    private Animator _animator;
    private PhotonView _photonView;
    private int _selectedIndex;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _photonView = GetComponent<PhotonView>();
        object[] data = _photonView.InstantiationData;
        _selectedIndex = data != null && data.Length >= 1 ? (int) data[0] : 1;

        ManageCameraOnAvatar();
        SelectUserAvatar();
    }

    private void ManageCameraOnAvatar()
    {
        if (!_photonView.IsMine)
        {
            for (int index = 0; index < transform.childCount; index++)
            {
                GameObject gameObject = transform.GetChild(index).gameObject;
                bool isAvatar = gameObject.tag == parentObject.tag;
                transform.GetChild(index).gameObject.SetActive(isAvatar);
            }
        }
    }

    private void SelectUserAvatar()
    {
        GameObject[] avatars = GetChildrenIn(parentObject);
        for (int index = 0; index < avatars.Length; index++)
        {
            avatars[index] = parentObject.transform.GetChild(index).gameObject;
            avatars[index].SetActive(index == _selectedIndex);
        }

        string selectedTag = avatars[_selectedIndex].tag;
        Avatar selectedAvatar = sophie;
        switch (selectedTag)
        {
            case "Bryce":
                selectedAvatar = bryce;
                break;
            case "Kate":
                selectedAvatar = kate;
                break;
            case "Adam":
                selectedAvatar = adam;
                break;
            default:
                break;
        }
        if (_animator != null)
        {
            _animator.avatar = selectedAvatar;
        }
    }

    private GameObject[] GetChildrenIn(GameObject parent)
    {
        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in parent.transform)
        {
            children.Add(child.gameObject);
        }
        return children.ToArray();
    }
}
