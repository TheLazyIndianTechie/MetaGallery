using System;
using Unity.VisualScripting;
using UnityEngine;

namespace WowEconomySystem
{
    public class EconomyManager : MonoBehaviour
    {

        public static event Action<int, int> OnQuestParamsChecked;

        private void OnEnable()
        {
            CollectiblesManager.OnCollectiblePickup += HandleCrystalCollectionEconomy;
        }

        private void OnDisable()
        {
            CollectiblesManager.OnCollectiblePickup -= HandleCrystalCollectionEconomy;
        }

        private static void HandleCrystalCollectionEconomy(string collectibleMessage, string collectibleType, int collectibleValue)
        {
            var collectiblesCount = (int)Variables.Application.Get(collectibleType);

            Debug.Log("Currently owned " + collectibleType + ": " + collectiblesCount);

            collectiblesCount += collectibleValue;

            Debug.Log("Collectibles Count is now: " + collectiblesCount);

            Variables.Application.Set(collectibleType, collectiblesCount);

            OnQuestParamsChecked?.Invoke(DefineCollectibleMappings(collectibleType), collectiblesCount);
        }


        private static int DefineCollectibleMappings(string collectibleType)
        {
            var result = collectibleType switch
            {
                "Crystals" => 1,
                "Shards" => 2,
                _ => -1
            };
            return result;
        }
    }
}
