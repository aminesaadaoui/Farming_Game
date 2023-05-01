﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Land;

[System.Serializable]
public struct LandSaveState 
{
    public Land.LandStatus landStatus;
    public GameTimestamp lastWatered;

    public LandSaveState(Land.LandStatus landStatus, GameTimestamp lastWatered)
    {
        this.landStatus = landStatus;
        this.lastWatered = lastWatered;
    }
    public void ClockUpdate(GameTimestamp timestamp)
    {

        if (landStatus == Land.LandStatus.Watered)
        {
            int hoursElapsed = GameTimestamp.CompareTimestamps(lastWatered, timestamp);
            Debug.Log(hoursElapsed + "hours since this was watered");


          


            if (hoursElapsed > 24)
            {
                landStatus = Land.LandStatus.Farmland;
            }
        }

       
    }
}
