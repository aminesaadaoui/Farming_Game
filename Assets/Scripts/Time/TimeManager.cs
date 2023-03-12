using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{

    public static TimeManager Instance { get; private set; }

    [Header("Internal Clock")]
    [SerializeField]
    GameTimestamp timestamp;

    public float timeScale = 1.0f;

    [Header("Day and night Cycle")]
    public Transform sunTransfrom;


    List<ITimeTracker> listeners;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        timestamp = new GameTimestamp(0, GameTimestamp.Season.Spring, 1, 6, 0);
        StartCoroutine(TimeUpdate());
    }

    IEnumerator TimeUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(1/timeScale);
            Tick();
        }
        
    }

    public void Tick()
    {
        timestamp.UpdateClock();

        foreach(ITimeTracker listener in listeners)
        {
            listener.ClockUpdate(timestamp);
        }

        UpdateSunMouvement();

    }


    void UpdateSunMouvement()
    {

        int timeInMinutes = GameTimestamp.HoursToMinute(timestamp.hour) + timestamp.minute;

        float sunAngle = .25f * timeInMinutes - 90;

        sunTransfrom.eulerAngles = new Vector3(sunAngle, 0, 0);

    }



    public void RegisterTracker(ITimeTracker listener)
    {
        listeners.Add(listener);    
    } 

    public void UnregisterTracker(ITimeTracker listener)
    {
        listeners.Remove(listener);
    }



}






