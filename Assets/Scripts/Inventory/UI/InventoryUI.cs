using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Farm.Inventory
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private SlotUI[] playerSlots;

        private void Start()
        {
            for (int i = 0; i < playerSlots.Length; i++)
            {
                playerSlots[i].slotIndex = i;
            }
        }



        private void OnEnable()
        {
            EventHandler.UpdateInventoryUI += OnUpdateInventoryUI;
        }

        private void OnDisable()
        {
            EventHandler.UpdateInventoryUI -= OnUpdateInventoryUI;
        }

        private void OnUpdateInventoryUI(InventoryLocationType location, List<InventoryItem> list)
        {
            switch (location)
            {
                case InventoryLocationType.Player:
                    for (int i = 0; i < playerSlots.Length; i++)
                    {
                        if (list[i].itemID > 0)
                        {
                            var item = InventoryManager.Instance.GetItemDetails(list[i].itemID);
                            playerSlots[i].UpdateSlot(item, list[i].itemAmount);
                        }
                        else
                        {
                            playerSlots[i].UpdateEmptySlot();
                        }
                    }
                    break;

                case InventoryLocationType.Box:
                    // Handle box inventory update logic here if needed
                    break;

                default:
                    Debug.LogWarning("Unknown inventory location type: " + location);
                    break;
            }
        }
    }
}
