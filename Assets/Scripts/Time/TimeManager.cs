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

    public float indoorAngle = 40;


    List<ITimeTracker> listeners = new List<ITimeTracker> ();


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

    public void LoadTime(GameTimestamp timestamp)
    {
        this.timestamp = new GameTimestamp(timestamp);
    }

    IEnumerator TimeUpdate()
    {
        while (true)
        {
            Tick();
            yield return new WaitForSeconds(1 / timeScale);
           
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

    public void SkipTime(GameTimestamp timeToSkipTo)
    {
        int timeToSkipInMinutes = GameTimestamp.TimestampInMinutes(timeToSkipTo);
        Debug.Log("time to skip " + timeToSkipInMinutes);
        int timeNowInMinutes = GameTimestamp.TimestampInMinutes(timestamp);
        Debug.Log("time now: " + timeNowInMinutes);

        int differenceInMinutes = timeToSkipInMinutes - timeNowInMinutes;
        Debug.Log(differenceInMinutes + "minutes will be advanced");

        if (differenceInMinutes <= 0) return;
        for(int i = 0; i < differenceInMinutes; i++)
        {
            Tick();
        }

    }

    void UpdateSunMouvement()
    {
        if(SceneTransitionManager.Instance.CurrentlyIndoor())
        {

            sunTransfrom.eulerAngles = new Vector3(indoorAngle, 0, 0);
            return;
        }

        int timeInMinutes = GameTimestamp.HoursToMinutes(timestamp.hour) + timestamp.minute;

        float sunAngle = .25f * timeInMinutes - 90;


    }

    public GameTimestamp GetGameTimestamp()
    {
        return new GameTimestamp(timestamp); 
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






