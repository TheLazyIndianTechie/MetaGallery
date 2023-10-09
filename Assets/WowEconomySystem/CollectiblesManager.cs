using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace WowEconomySystem
{
    public class CollectiblesManager : MonoBehaviour
    {
        public static event Action<string, string, int> OnCollectiblePickup;

        [FormerlySerializedAs("_pickupMessage")] [SerializeField] private string pickupMessage;

        private string _collectibleType;


        [FormerlySerializedAs("_collectibleValue")] [SerializeField] private int collectibleValue = 1;

        private void Awake()
        {
            _collectibleType = gameObject.tag;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            OnCollectiblePickup?.Invoke(pickupMessage, _collectibleType, collectibleValue);
            Destroy(gameObject);
        }
    }
}
