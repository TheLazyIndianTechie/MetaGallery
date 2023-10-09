using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Animations.Rigging;
using System;

namespace ReadyPlayerMe
{
    public class RuntimeAvatarLoader : MonoBehaviour
    {
        [SerializeField]
        private RuntimeAnimatorController starterAssetAnimator;

        [SerializeField]
        private GameObject animationRig;

        //Creating user data
        private UserData userData;

        private readonly string defaultAvatarURL = "https://api.readyplayer.me/v1/avatars/638f616e75f8551b54c92356.glb";

        //Vinay custom code for testing
        [SerializeField] private readonly string editorTestingAvatarURL = "https://api.readyplayer.me/v1/avatars/638f616e75f8551b54c92356.glb";


        private void Awake()
        {
            userData = (UserData)Variables.Application.Get("UserDataJSON");
        }

        private void Start()
        {
            // Only for editor testing
            WowEventManager.TriggerEvent(nameof(WowEvents.OnPlayerDataRetrieved), userData.avatarUrl);
        }

        private void OnEnable()
        {
            // Subscribe to the events
            WowEventManager.StartListening(nameof(WowEvents.OnPlayerDataRetrieved), LoadAvatar);
        }

        private void OnDisable()
        {
            // Unsubscribe to the events
            WowEventManager.StopListening(nameof(WowEvents.OnPlayerDataRetrieved), LoadAvatar);
        }

        private void LoadAvatar(string avatarUrl)
        {
            string currentMessage = "";

            if (string.IsNullOrEmpty(avatarUrl))
            {
                Debug.Log("Haven't received Avatar URL");
                WowEventManager.TriggerEvent(nameof(WowEvents.OnDisplayInfo), "Haven't received Avatar URL");
            }
            else
            {
                WowEventManager.TriggerEvent(nameof(WowEvents.OnDisplayInfo), "Loading your Avatar...");

                var avatarLoader = new AvatarLoader();
                Debug.Log($"Started loading avatar. [{Time.timeSinceLevelLoad:F2}]");

                currentMessage = $"Started loading avatar. [{Time.timeSinceLevelLoad:F2}]";

                WowEventManager.TriggerEvent(nameof(WowEvents.OnDisplayInfo), currentMessage);

                avatarLoader.OnCompleted += (sender, args) =>
                {
                    Debug.Log($"Loaded avatar. [{Time.timeSinceLevelLoad:F2}]");

                    currentMessage = $"Loaded avatar. [{Time.timeSinceLevelLoad:F2}]";
                    WowEventManager.TriggerEvent(nameof(WowEvents.OnDisplayInfo), currentMessage);

                    GameObject go = GameObject.Find("Armature").transform.parent.gameObject;
                    if (go != null)
                    {
                        go.transform.parent = transform;
                    }
                    go.transform.localPosition = new Vector3(0, 0, 0);
                    go.transform.localRotation = Quaternion.Euler(0, 0, 0);

                    // Add animator and necessary callbacks to handle walk and land animation audio
                    go.AddComponent<RuntimeAnimationSoundLoader>();

                    currentMessage = "Adding runtime animator...";
                    WowEventManager.TriggerEvent(nameof(WowEvents.OnDisplayInfo), currentMessage);

                    go.GetComponent<Animator>().runtimeAnimatorController = starterAssetAnimator;

                    currentMessage = "Setting up animation controls..";
                    WowEventManager.TriggerEvent(nameof(WowEvents.OnDisplayInfo), currentMessage);

                    // Add animation rigging
                    AddAnimationRigging(go);

                    currentMessage = "Successfully initialized character...";
                    WowEventManager.TriggerEvent(nameof(WowEvents.OnDisplayInfo), currentMessage);

                    // Trigger OnInitialized Event
                    WowEventManager.TriggerEvent(nameof(WowEvents.OnRPMAvatarLoaded));
                };

                avatarLoader.OnFailed += (sender, args) =>
                {
                    WowEventManager.TriggerEvent(nameof(WowEvents.OnRPMAvatarLoaded));
                    WowEventManager.TriggerEvent(WowEvents.OnGameError.ToString(), args.Type.ToString());
                    Debug.LogError(args.Message);
                };

                avatarLoader.LoadAvatar(avatarUrl);
            }
        }

        private void AddAnimationRigging(GameObject go)
        {
            if (animationRig != null)
            {
                // Move the Rig gameobject inside the newly created runtime avatar
                animationRig.transform.parent = go.transform;

                // Add the RigBuilder component to the parent
                go.transform.AddComponent<RigBuilder>();

                // Find the Chest (Spine1) & Head gameobject
                Transform chest = go.transform.Find("Armature/Hips/Spine/Spine1");
                Transform head = go.transform.Find("Armature/Hips/Spine/Spine1/Spine2/Neck/Head");

                // Add constraint to rig for the Chest animation
                Transform bodyLookAt = animationRig.transform.Find("BodyLookAt");
                MultiAimConstraint bodyMultiAimConstraint = bodyLookAt.gameObject.GetComponent<MultiAimConstraint>();
                bodyMultiAimConstraint.data.constrainedObject = chest;

                // Add constraint to rig for the Head animation
                Transform headLookAt = animationRig.transform.Find("HeadLookAt");
                MultiAimConstraint headMultiAimConstraint = headLookAt.gameObject.GetComponent<MultiAimConstraint>();
                headMultiAimConstraint.data.constrainedObject = head;

                // Create the Rig object with the Rig component in it
                Rig rig = animationRig.AddComponent<Rig>();

                // Create the rigBuilder component and add the above Rig to its RigLayer
                RigBuilder rigbuilder = go.transform.GetComponent<RigBuilder>();
                rigbuilder.layers.Clear();
                rigbuilder.layers.Add(new RigLayer(rig, true));
                rigbuilder.enabled = true;
                rigbuilder.Build();
            }
        }
    }
}
