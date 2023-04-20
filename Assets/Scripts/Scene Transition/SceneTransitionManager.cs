using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
   public static SceneTransitionManager Instance;

    public enum Location { Farm, PlayerHome, Town}

    public Location currentLocation;


    Transform playerPoint;

    bool screenFadeOut;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;    
        }

        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnLocationLoad;

        playerPoint = FindObjectOfType<PlayerController>().transform;
    }
    public void SwitchLocation(Location locationToSwitch)
    {
        UIManager.Instance.FadeOutScreen();
        screenFadeOut = false;
        StartCoroutine(ChangeScene(locationToSwitch));
    }

    IEnumerator ChangeScene(Location locationToSwitch)
    {
        while (!screenFadeOut)
        {
            yield return new WaitForSeconds(0.1f);
        }
        SceneManager.LoadScene(locationToSwitch.ToString());
        screenFadeOut = false;
    }

    public void OnFadeOutComplete()
    {
        screenFadeOut = true;   
    }

    public void OnLocationLoad(Scene scene, LoadSceneMode mode)
    {

        Location oldLocation = currentLocation;

        Location newLocation = (Location) Enum.Parse(typeof(Location), scene.name);

        if (currentLocation == newLocation) return;

        Transform startPoint = LocationManager.Instance.GetPlayerStartingPosition(oldLocation);

        CharacterController playerCharacter = playerPoint.GetComponent<CharacterController>();
        playerCharacter.enabled = false;

        playerPoint.position = startPoint.position;
        playerPoint.rotation = startPoint.rotation;

        playerCharacter.enabled = true;

        currentLocation = newLocation;
         
    }
}
