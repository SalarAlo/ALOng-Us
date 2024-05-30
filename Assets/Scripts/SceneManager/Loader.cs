using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : SingletonPersistent<Loader>
{
    public enum Scene {
        MainMenuScene,
        CharacterSelectionScene,
        GameScene,
    }

    public void LoadScene(Scene sceneToLoad) {
        SceneManager.LoadScene(sceneToLoad.ToString());
    }

    public void LoadSceneNetworked(Scene sceneToLoad) {
        NetworkManager.Singleton.SceneManager.LoadScene(sceneToLoad.ToString(), LoadSceneMode.Single);
    }
}
