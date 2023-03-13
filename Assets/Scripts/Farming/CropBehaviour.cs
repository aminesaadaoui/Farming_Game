﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropBehaviour : MonoBehaviour
{
    SeedData seedToGrow;

    [Header("Stage of Life")]
    public GameObject seed;
    private GameObject seedling;
    private GameObject harvestable;

    public enum CropState
    {
        Seed, Seedling, Harvestable
    }

    public CropState cropState;
    public void Plant(SeedData seedToGrow)
    {
        this.seedToGrow = seedToGrow;

        seedling = Instantiate(seedToGrow.seedling, transform);
        
        ItemData cropToYield = seedToGrow.corpToYield;

        harvestable = Instantiate(cropToYield.gameModel, transform);

        SwitchState(CropState.Seed);
    }
    public void Grow()
    {

    }

    void SwitchState(CropState stateToSwitch)
    {
        seed.SetActive(false);
        seedling.SetActive(false);
        harvestable.SetActive(false);


        switch (stateToSwitch)
        {
            case CropState.Seed:
                seed.SetActive(true);
                break;
            case CropState.Seedling:
                seedling.SetActive(true);
                break;
            case CropState.Harvestable:
                harvestable.SetActive(true);
                break;
        }



        cropState = stateToSwitch;

    }
}
