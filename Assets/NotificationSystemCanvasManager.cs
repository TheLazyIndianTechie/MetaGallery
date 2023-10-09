using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NotificationSystemCanvasManager : MonoBehaviour
{
    private Canvas notificationCanvas;

    [SerializeField]
    private Image notificationImage;

    [SerializeField]
    private TMP_Text notificationHeading, notificationText;

    private void OnEnable()
    {
        PortalManager.OnPortalCanvasOpenRequested += DeactivateNotificationCanvas;
        PortalManager.OnPortalCanvasCloseRequested += DeactivateNotificationCanvas;
        PortalManager.OnNotifyPortalOpen += TriggerDialog;
    }

    private void OnDisable()
    {

        PortalManager.OnPortalCanvasCloseRequested -= DeactivateNotificationCanvas;
        PortalManager.OnPortalCanvasOpenRequested -= DeactivateNotificationCanvas;
        PortalManager.OnNotifyPortalOpen -= TriggerDialog;
    }

    private void Awake()
    {
        notificationCanvas = GetComponent<Canvas>();

        DeactivateNotificationCanvas();

        //Clear any text on Awake
        notificationHeading.text = "";
        notificationText.text = "";

        notificationImage.fillAmount = 0;


    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            CallTestTriggerNotification();
        }

        if (Input.GetKey(KeyCode.Y))
        {
            float speed = 0.5f;

            notificationImage.fillAmount += speed *Time.deltaTime;

        }
    }



    public void CallTestTriggerNotification()
    {
        TriggerNotification( "Triggering Notification", "test trigger notification");
    }


    public void TriggerDialog(string heading, string content)
    {
        ActivateNotificationCanvas();

        notificationHeading.text = heading;
        notificationText.text = content;

    }

    public void TriggerNotification(string heading, string content)
    {
        ActivateNotificationCanvas();

        notificationHeading.text = heading;
        notificationText.text = content;

        StartCoroutine(FadeOutNotificationCanvas(5f));

    }

    public void ActivateNotificationCanvas()
    {
        notificationCanvas.enabled = true;
    }

    public void DeactivateNotificationCanvas()
    {
        notificationCanvas.enabled = false;
    }

    IEnumerator FadeOutNotificationCanvas(float fadeDuration)
    {
        yield return new WaitForSeconds(fadeDuration);

        notificationCanvas.enabled = false;
    }
}
