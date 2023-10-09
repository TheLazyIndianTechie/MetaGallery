using System;
using UnityEngine;

namespace WowQuestSystem
{
    public class QuestTriggerManager : MonoBehaviour
    {
        [SerializeField] private int questId;

        public static event Action<int> OnQuestTriggered;
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            OnQuestTriggered?.Invoke(questId);
            Debug.Log("Player has activated Quest #"+questId);
            Destroy(gameObject);
        }
    }
}
