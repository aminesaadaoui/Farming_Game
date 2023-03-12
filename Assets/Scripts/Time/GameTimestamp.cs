﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameTimestamp 
{
    public int year; 
    public enum Season
    {
        Spring,
        Summer,
        Fall,
        Winter
    }
    public Season season; 
    
    public enum DayOfTheWeek
    {
        Saturday,
        Sunday,
        Monday,
        Thuesday,
        Wednesday,
        Thursday,
        Friday
       
    }
    public int day;
    public int hour;
    public int minute;

    public GameTimestamp(int year, Season season, int day, int hour, int minute)
    {
        this.year = year;
        this.season = season;
        this.day = day;
        this.hour = hour;
        this.minute = minute;
    }

    public void UpdateClock()
    {
        minute++;

        if(minute >= 60)
        {
            hour++;
        }

        if(hour >= 24)
        {
            hour = 0;
            day++;
        }

        if(day > 30)
        {
            day = 1;
            if(season == Season.Winter)
            {
                season = Season.Spring;
                year++;
            }
            else
            {
                season++;
            }

           
        }
    }

    public DayOfTheWeek GetDayOfTheWeek()
    {
        int daysPassed = YearsToDays(year) + SeasonsToDay(season) + day;

        int daysIndex = daysPassed % 7;

        return (DayOfTheWeek)daysIndex;
    }

    public static  int HoursToMinute(int hour)
    {
        return hour * 60;
    }

    public static int DaysTHours(int days)
    {
        return days * 24;
    }
    public static int SeasonsToDay(Season season)
    {
        int seasonIndex = (int)season;
        return seasonIndex * 30;
    }

    public static int YearsToDays(int years)
    {
        return years * 4 * 30;
    }
        
}
