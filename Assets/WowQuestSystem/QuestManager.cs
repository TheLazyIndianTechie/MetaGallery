using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using WowEconomySystem;

namespace WowQuestSystem
{
    public class QuestManager : MonoBehaviour
    {
        private QuestCollectionData _myQuestCollectionData;

        private QuestData _myQuest;

        public Dictionary<int, QuestData> QuestHashMap;

        public static event Action<QuestData> OnQuestRetrieved;

        public static event Action<int> OnQuestCompleted;

        public static event Action<bool> OnAllQuestsCompleted;


        private void OnEnable()
        {
            QuestTriggerManager.OnQuestTriggered += RetrieveQuestDataLocally;

            //Listen to Economy Manager active quest data
            EconomyManager.OnQuestParamsChecked += ActiveQuestProcessor;

        }

        private void OnDisable()
        {
            QuestTriggerManager.OnQuestTriggered -= RetrieveQuestDataLocally;

            EconomyManager.OnQuestParamsChecked -= ActiveQuestProcessor;
        }


        private void RetrieveQuestDataLocally(int questId)
        {
            var questCollectionData = (QuestCollectionData)Variables.Application.Get("QuestCollectionData");

            switch (questCollectionData.quests.Count)
            {
                case > 0:
                {
                    QuestData result = questCollectionData.quests.Find(x => x.questId == questId);

                    // Debug.Log(result.itemId + result.questId + result.questDescription + result.questName);
                    Debug.Log(result.questId);
                    Debug.Log(result.questName);
                    Debug.Log(result.questDescription);
                    Debug.Log(result.itemId);

                    OnQuestRetrieved?.Invoke(result);

                    Variables.Application.Set("ActiveQuestData", result);
                    break;
                }
                case 0:
                    OnAllQuestsCompleted?.Invoke(true);
                    Debug.Log("User has completed all quests");
                    break;
            }
        }

        private void ActiveQuestProcessor(int itemId, int currentValue)
        {
            QuestData activeQuestData = (QuestData)Variables.Application.Get("ActiveQuestData");

            if (itemId == activeQuestData.itemId && currentValue >= activeQuestData.fulfillmentValue)
            {
                Debug.Log("Quest is completed. Calling react method.");
                //Call QuestComplete
                ReactDataManager.Instance.CallCompleteQuest(activeQuestData.questId, gameObject.name, nameof(UpdateUserReward));
                Debug.Log("Complete quest called on Quest ID: " + activeQuestData.questId);

                //Triggering an event signalling the quest that has been completed
                OnQuestCompleted?.Invoke(activeQuestData.questId);

            }

            else
            {
                Debug.Log("Quest is pending");
            }
        }

        public void UpdateUserReward(string userDataJSON)
        {
            Debug.Log("Update user reward method being called");

            ReactDataManager.Instance.GetUserData(userDataJSON);


            Debug.Log(userDataJSON);
        }
    }
}
