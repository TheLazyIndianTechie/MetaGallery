using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using WowQuestSystem;

public class QuestCanvasManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text questNameDisplay, questDescriptionDisplay, questStatusDisplay;
    private Canvas _questCanvas;
    private bool _questCompletionStatus = false;


    private void Awake()
    {
        _questCanvas = GetComponent<Canvas>();
        DeactivateQuestCanvas();
    }
    private void OnEnable()
    {
        //Listen to events of coins being picked up
        QuestManager.OnQuestRetrieved += UpdateQuestDisplay;
        // CurrentQuestManager.OnCurrentQuestRetrieved += UpdateQuestDisplay;
        // CurrentQuestManager.OnQuestCompleted += UpdateQuestCompletedDisplay;
        // CollectiblesManager.OnCollectiblePickedUp += UpdateQuestProgressDisplay;
        // QuestManager.OnQuestCompleted += UpdateQuestCompletedDisplay;
    }

    private void OnDisable()
    {
        //Stop listening to coin pickup events
        QuestManager.OnQuestRetrieved -= UpdateQuestDisplay;
        // CurrentQuestManager.OnCurrentQuestRetrieved -= UpdateQuestDisplay;
        // CurrentQuestManager.OnQuestCompleted -= UpdateQuestCompletedDisplay;
        // QuestManager.OnQuestRetrieved -= UpdateQuestDisplay;
        // CollectiblesManager.OnCollectiblePickedUp -= UpdateQuestProgressDisplay;
        // QuestManager.OnQuestCompleted -= UpdateQuestCompletedDisplay;
    }

    public void UpdateQuestDisplay(QuestData questData)
    {
        ActivateQuestCanvas();
        questNameDisplay.SetText(questData.questName);
        questDescriptionDisplay.SetText(questData.questDescription);
    }

    public void UpdateQuestProgressDisplay(string collectibleMessage, string collectibleType, int count)
    {
        //Temporarily clearing text
        questStatusDisplay.SetText("");

        int currentCount = (int)Variables.Application.Get(collectibleType);

        
        string questProgressMessage = collectibleMessage + " " + currentCount + " " + collectibleType;

        if (!_questCompletionStatus)
        {
            questStatusDisplay.SetText(questProgressMessage);
        }

        else
        {
            questStatusDisplay.SetText("<s> " + questProgressMessage + " </s>");
        }
        
    }

    public void UpdateQuestCompletedDisplay(int questId)
    {
        // QuestData questData = (QuestData)Variables.Application.Get("ActiveQuestData");

        questDescriptionDisplay.SetText("Quest: " + questId + " completed!");

        _questCompletionStatus = true;
    }

    public void ActivateQuestCanvas()
    {
        questStatusDisplay.SetText("");
        _questCanvas.enabled = true;
    }

    public void DeactivateQuestCanvas()
    {
        _questCanvas.enabled = false;
    }
}
