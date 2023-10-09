using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public enum Scene
    {
        Intro_v1,
        ArtGallery_v2,
        ArtGallery_v3,
        Loading_v1,
    }

    private static Action OnSceneLoaderCallback;

    public static void Load(Scene scene)
    {
        // Set the callback action to load the target scene
        OnSceneLoaderCallback = () =>
        {
            SceneManager.LoadScene(scene.ToString());
        };

        // Load the loading scene
        SceneManager.LoadScene(Scene.Loading_v1.ToString());
    }

    public static void SceneLoaderCallback()
    {
        // Triggered after the first Update which lets the screen refresh
        // Executet the call back action which wwould load the target scene
        if (OnSceneLoaderCallback != null)
        {
            OnSceneLoaderCallback();
            OnSceneLoaderCallback = null;
        }
    }
}
