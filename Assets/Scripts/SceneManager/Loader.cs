using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : SingletonPersistent<Loader>
{
    public Action<Scene> OnSceneChanged;
    public enum Scene {
        MainMenuScene,
        CharacterSelectionScene,
        GameScene,
    }

    private void Start() {
        SceneManager.activeSceneChanged += SceneManager_ActiveSceneChanged;
    }

    private void SceneManager_ActiveSceneChanged(UnityEngine.SceneManagement.Scene arg0, UnityEngine.SceneManagement.Scene newScene) {
        foreach(Scene scene in Enum.GetValues(typeof(Scene))) {
            if (scene.ToString() == newScene.name) {
                OnSceneChanged?.Invoke(scene);
            }
        }
    }

    public void LoadScene(Scene sceneToLoad) {
        SceneManager.LoadScene(sceneToLoad.ToString());
    }

    public void LoadSceneNetworked(Scene sceneToLoad) {
        NetworkManager.Singleton.SceneManager.LoadScene(sceneToLoad.ToString(), LoadSceneMode.Single);
    }
}
