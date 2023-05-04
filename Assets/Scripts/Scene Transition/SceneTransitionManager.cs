using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
   public static SceneTransitionManager Instance;

    public enum Location { Farm, PlayerHome, Town}

    public Location currentLocation;

    static readonly Location[] indoor = { Location.PlayerHome }; 

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

    public bool CurrentlyIndoor()
    {
        return indoor.Contains(currentLocation);
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
        screenFadeOut = false;
        UIManager.Instance.ResetFadeDefaults();
        SceneManager.LoadScene(locationToSwitch.ToString());
        
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
