using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Farm.Inventory
{
    public class InventoryUI : MonoBehaviour
    {
        [Header("玩家背包UI")]
        [SerializeField] private GameObject bagUI;
        private bool bagOpened;

        [SerializeField] private SlotUI[] playerSlots;

        private void Start()
        {
            for (int i = 0; i < playerSlots.Length; i++)
            {
                playerSlots[i].slotIndex = i;
            }
            bagOpened = bagUI.activeInHierarchy;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                OpenBagUI();
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


        /// <summary>
        /// 打开关闭背包UI，Button调用事件
        /// </summary>
        public void OpenBagUI()
        {
            bagOpened = !bagOpened;
            bagUI.SetActive(bagOpened);
        }

        /// <summary>
        /// 更新高亮显示
        /// </summary>
        /// <param name="index">序号</param>
        public void UpdateSlotHightlight(int index)
        {
            foreach (var slot in playerSlots)
            {
                if (slot.isSelected && slot.slotIndex == index)
                {
                    slot.slotHightlight.gameObject.SetActive(true);
                }
                else
                {
                    slot.isSelected = false;
                    slot.slotHightlight.gameObject.SetActive(false);
                }
            }
        } 
    }
}
