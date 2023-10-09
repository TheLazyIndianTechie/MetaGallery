using Unity.VisualScripting;
using UnityEngine;

public class CallCompleteQuest : MonoBehaviour
{
    private QuestData _currentlyActiveQuest;

    private void OnEnable()
    {
        ReactDataManager.OnGetUserDataCallback += DestroyCurrentObject;
    }

    private void OnDisable()
    {
        ReactDataManager.OnGetUserDataCallback -= DestroyCurrentObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        _currentlyActiveQuest = (QuestData)Variables.Application.Get("ActiveQuestData");
        
        
#if !UNITY_EDITOR
        CompleteActiveQuest(_currentlyActiveQuest.questId);
#else
        CompleteQuestLocally(_currentlyActiveQuest.questId);
        Destroy(gameObject);
#endif

    }

    private void CompleteQuestLocally(int activeQuestId)
    {
        Debug.Log("Called complete quest locally on " + activeQuestId);
    }
    private void CompleteActiveQuest(int activeQuestId)
    {
        ReactDataManager.Instance.CallCompleteQuest(activeQuestId, gameObject.name,
            nameof(UpdateUserReward));
    }

    public void UpdateUserReward(string userDataJSON)
    {
        Debug.Log("Update user reward method being called");

        ReactDataManager.Instance.GetUserData(userDataJSON);


        Debug.Log(userDataJSON);
    }

    private void DestroyCurrentObject()
    {
        Destroy(gameObject);
    }
}
