﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationEntryPoint : MonoBehaviour
{

    [SerializeField]
    SceneTransitionManager.Location locationToSwitch;

   private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            SceneTransitionManager.Instance.SwitchLocation(locationToSwitch);
        }
    }
    
      
    

    
}
