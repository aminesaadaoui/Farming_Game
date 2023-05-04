using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class MainMenu : MonoBehaviour
{
    public Button loadGameButton;
    public void NewGame()
    {
        StartCoroutine(LoadGameAsync(SceneTransitionManager.Location.PlayerHome, null));
    }
    public void ContinueGame()
    {
        StartCoroutine(LoadGameAsync(SceneTransitionManager.Location.PlayerHome, LoadGame));
    }

    void LoadGame()
    {
        if(GameStateManager.Instance == null)
        {
            Debug.Log("Count find Game Stat Manager !");
            return;

        }
        GameStateManager.Instance.LoadSave();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator LoadGameAsync(SceneTransitionManager.Location scene, Action onFisrtFarmLoad)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene.ToString());

        DontDestroyOnLoad(gameObject);

        while (!asyncLoad.isDone)
        {
            yield return null;
            Debug.Log("Loading");
            
        }
        Debug.Log("Loaded!");

        yield return new WaitForEndOfFrame();
        Debug.Log("First farm is loaded ! ");

        onFisrtFarmLoad?.Invoke();

        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        loadGameButton.interactable = SaveManager.HasSave();
        
    }

   
}
