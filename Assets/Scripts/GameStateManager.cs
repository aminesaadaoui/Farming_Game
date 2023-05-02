using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour , ITimeTracker
{
    public static GameStateManager Instance { get; private set; }


    bool screenFadeOut;


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
        TimeManager.Instance.RegisterTracker(this);
    }

    public void ClockUpdate(GameTimestamp timestamp)
    {
      if(SceneTransitionManager.Instance.currentLocation != SceneTransitionManager.Location.Farm)
        {
            List<LandSaveState> landData = LandManager.farmData.Item1;
            List<CropSaveState> cropData = LandManager.farmData.Item2;

            if (cropData.Count == 0) return;

            for (int i = 0; i < cropData.Count; i++)
            {
                CropSaveState crop = cropData[i];
                LandSaveState land = landData[crop.landID];

                if (crop.cropState == CropBehaviour.CropState.Wilted) continue;
                
                land.ClockUpdate(timestamp);

                if(land.landStatus == Land.LandStatus.Watered)
                {
                    crop.Grow();
                }else if(crop.cropState != CropBehaviour.CropState.Seed)
                {
                    crop.Wither();
                }

                cropData[i] = crop;
                landData[crop.landID] = land;
            }

            LandManager.farmData.Item2.ForEach((CropSaveState crop) =>
            {
                Debug.Log(crop.seedToGrow + "\n Health: " + crop.health + "\n Growth: " + crop.growth + "\n State: " + crop.cropState.ToString());
            });
        }
    }

    public void Sleep()
    {
        UIManager.Instance.FadeInScreen();
        screenFadeOut = false; 
        StartCoroutine(TransitionTime());
    }


    IEnumerator TransitionTime()
    {

        GameTimestamp timestampOnNextDay = TimeManager.Instance.GetGameTimestamp();
        timestampOnNextDay.day += 1;
        timestampOnNextDay.hour = 6;
        timestampOnNextDay.minute = 0;
        Debug.Log(timestampOnNextDay.day + " " + timestampOnNextDay.hour + " : " + timestampOnNextDay.minute);
        
        TimeManager.Instance.SkipTime(timestampOnNextDay);

        while (!screenFadeOut)
        {
            yield return new WaitForSeconds(1f);
        }

        screenFadeOut = false;
        UIManager.Instance.ResetFadeDefaults();

    }

    public void OnFadeOutComplete()
    {
        screenFadeOut = true;
    }

    public GameSaveState ExportSaveState()
    {
        List<LandSaveState> landData = LandManager.farmData.Item1;
        List<CropSaveState> cropData = LandManager.farmData.Item2;

        ItemSlotData[] toolSlots = InventoryManager.Instance.GetInventorySlots(InventorySlot.InventoryType.Tool);
        ItemSlotData[] itemSlots = InventoryManager.Instance.GetInventorySlots(InventorySlot.InventoryType.Item);

        ItemSlotData equippedToolSlot = InventoryManager.Instance.GetEquippedSlot(InventorySlot.InventoryType.Tool);
        ItemSlotData equippedItemSlot = InventoryManager.Instance.GetEquippedSlot(InventorySlot.InventoryType.Item);

        GameTimestamp timestamp = TimeManager.Instance.GetGameTimestamp();
        return new GameSaveState(landData, cropData, toolSlots, itemSlots, equippedItemSlot, equippedToolSlot, timestamp);
    }

}
