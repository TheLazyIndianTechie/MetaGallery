using UnityEngine;
using TMPro;
using Unity.VisualScripting;

namespace ReadyPlayerMe
{
    public class RuntimeLoaderVarient : MonoBehaviour
    {
        [SerializeField]
        private RuntimeAnimatorController starterAssetAnimator;

        [SerializeField]
        private TextMeshProUGUI playerNameText;

        [SerializeField]
        private GameObject loadingPannel;

        [SerializeField]
        private GameObject pauseControls;

        [SerializeField]
        private GameObject hudOverlay;

        private UserData userData;

        private string defaultAvatarURL = "https://api.readyplayer.me/v1/avatars/638f616e75f8551b54c92356.glb";

        private void Awake()
        {
            userData = (UserData)Variables.Application.Get("UserDataJSON");
        }

        private void Start()
        {
            loadingPannel?.SetActive(true);
            hudOverlay?.SetActive(false);
            pauseControls?.SetActive(false);

            // Load Avatar
            LoadAvatar(userData.avatarUrl);
        }


        private void LoadAvatar(string avatarUrl)
        {
            if (string.IsNullOrEmpty(avatarUrl))
            {
                Debug.Log("Haven't received Avatar URL");
            } else
            {
                Debug.Log($"Started loading avatar. [{Time.timeSinceLevelLoad:F2}]");
                var avatarLoader = new AvatarLoader();
                avatarLoader.OnCompleted += (sender, args) =>
                {
                    Debug.Log($"Loaded avatar. [{Time.timeSinceLevelLoad:F2}]");

                    GameObject go = GameObject.Find("Armature").transform.parent.gameObject;
                    if (go != null)
                    {
                        go.transform.parent = transform;
                    }
                    go.transform.localPosition = new Vector3(0, 0, 0);
                    go.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    go.AddComponent<AnimationEventsRPMVarient>();
                    go.GetComponent<Animator>().runtimeAnimatorController = starterAssetAnimator;

                    loadingPannel?.SetActive(false);
                    hudOverlay?.SetActive(true);
                    pauseControls?.SetActive(true);

                    //Method to deactivate mouse lock for menu clickability
                    //LockMode.DeactivateMouseLock();
                };
                avatarLoader.OnFailed += (sender, args) =>
                {
                    avatarLoader.LoadAvatar(defaultAvatarURL);
                    Debug.Log(args.Type);
                };

                avatarLoader.LoadAvatar(avatarUrl);
            }
        }
    }
}
