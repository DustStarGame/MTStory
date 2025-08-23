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
}
