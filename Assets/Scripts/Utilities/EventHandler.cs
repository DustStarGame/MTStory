using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventHandler
{
    public static event Action<InventoryLocationType, List<InventoryItem>> UpdateInventoryUI;

    public static void CallUpdateInventoryUI(InventoryLocationType locationType, List<InventoryItem> list)
    {
        UpdateInventoryUI?.Invoke(locationType, list);
    }

    public static event Action<int, Vector3> InstantiateItemInScene;
    public static void CallInstantiateItemInScene(int itemID, Vector3 position)
    {
        InstantiateItemInScene?.Invoke(itemID, position);
    }

    public static event Action<ItemDetails, bool> ItemSelectedEvent;
    public static void CallItemSelectedEvent(ItemDetails itemDetails, bool isSelected)
    {
        ItemSelectedEvent?.Invoke(itemDetails, isSelected);
    }

    public static event Action<int, int> GameMinuteEvent;
    public static void CallGameMinuteEvent(int Minute, int hour)
    {
        GameMinuteEvent?.Invoke(Minute, hour);
    }

    public static event Action<int, int, int, int, Season> GameDateEvent;
    public static void CallGameDateEvent(int hour, int day, int month, int year, Season season)
    {
        GameDateEvent?.Invoke(hour, day, month, year, season);
    }

    public static event Action<string, Vector3> TransitionEvent;
    public static void CallTransitionEvent(string sceneName, Vector3 position)
    {
        TransitionEvent?.Invoke(sceneName, position);
    }
}
