using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "Singleton/MasterManager")]
public class MasterManager : SingletonScriptableObject<MasterManager>
{
    [SerializeField] private GameSettings _gameSettings;

    public static GameSettings GameSettings { get { return Instance._gameSettings; } }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void initializeGame()
    {
        Debug.Log("This method will be called before Scene is loaded");
    }
}
