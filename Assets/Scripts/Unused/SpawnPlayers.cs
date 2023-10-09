using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SpawnPlayers : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private GameObject playerPrefab;

    [Header("Position range")]
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minZ;
    [SerializeField] private float maxZ;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 position = new Vector3(Random.Range(minX, maxX), 0, Random.Range(minZ, maxZ));
        PhotonNetwork.Instantiate(playerPrefab.name, position, Quaternion.identity);
    }
}
