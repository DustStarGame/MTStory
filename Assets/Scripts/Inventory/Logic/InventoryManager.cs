using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Farm.Inventory
{
    public class InventoryManager : Singleton<InventoryManager>
    {
        [Header("物品数据")]
        public ItemDataList_SO itemDataList_SO;

        [Header("背包数据")]
        public InventoryBag_SO playerBag;

        private void Start()
        {
            EventHandler.CallUpdateInventoryUI(InventoryLocationType.Player, playerBag.itemList);
        }

        /// <summary>
        /// 通过ID获取物品信息
        /// </summary>
        /// <param name="ID">Item ID</param>
        /// <returns></returns>
        public ItemDetails GetItemDetails(int ID)
        {
            return itemDataList_SO.itemDetailsList.Find(item => item.itemID == ID);
        }

        /// <summary>
        /// 添加单个物品到Player背包里面
        /// </summary>
        /// <param name="item"></param>
        /// <param name="toDestory">是否要摧毁物品</param>
        public void AddItem(Item item, bool toDestory)
        {
            //判断是否已经有该物体
            var index = GetItemIndexInBag(item.itemID);


            AddItemAtIndex(item.itemID, index, 1);


            Debug.Log($"添加物品ID:{item.itemDetails.itemID} + 物品名字:{item.itemDetails.itemName}  到背包");
            if (toDestory)
            {
                // 销毁物品对象
                Destroy(item.gameObject);
            }

            // 更新UI
            EventHandler.CallUpdateInventoryUI(InventoryLocationType.Player, playerBag.itemList);
        }


        /// <summary>
        /// 判断背包是否有空位
        /// </summary>
        /// <returns></returns>
        private bool CheckBagCapacity()
        {
            for (int i = 0; i < playerBag.itemList.Count; i++)
            {
                if (playerBag.itemList[i].itemID == 0)
                {
                    // 找到一个空位
                    return true;
                }
            }
            return true;
        }


        /// <summary>
        /// 通过物品ID找到背包已有的物品位置
        /// </summary>
        /// <param name="ID">物品ID</param>
        /// <returns>-1则没有这个物品否则返回对应序号</returns>
        private int GetItemIndexInBag(int ID)
        {
            for (int i = 0; i < playerBag.itemList.Count; i++)
            {
                if (playerBag.itemList[i].itemID == ID)
                {
                    return i;
                }
            }
            return -1; // 如果没有找到，返回-1
        }


        /// <summary>
        /// 在指定背包序号位置添加物品
        /// </summary>
        /// <param name="ID">物品ID</param>
        /// <param name="index">背包中的序号</param>
        /// <param name="amount">背包中被添加物品的数量</param>
        private void AddItemAtIndex(int ID, int index, int amount)
        {
            if (index == -1 && CheckBagCapacity())     //背包里没这个物品  同时背包中有空位
            {
                InventoryItem item = new InventoryItem
                {
                    itemID = ID,
                    itemAmount = amount
                };
                for (int i = 0; i < playerBag.itemList.Count; i++)
                {
                    if (playerBag.itemList[i].itemID == 0)
                    {
                        playerBag.itemList[i] = item;
                        break;
                    }
                }
            }
            else
            {
                int currrentAmount = playerBag.itemList[index].itemAmount + amount;
                InventoryItem item = new InventoryItem
                {
                    itemID = ID,
                    itemAmount = currrentAmount
                };

                playerBag.itemList[index] = item;
            }
        }

        /// <summary>
        /// Player背包内交换物品
        /// </summary>
        /// <param name="fromIndex">起始序号</param>
        /// <param name="targetSlot">目标数据序号</param>
        public void SwapItem(int fromIndex, int targetSlot)
        {
            InventoryItem currentItem = playerBag.itemList[fromIndex];
            InventoryItem targetItem = playerBag.itemList[targetSlot];

            if (targetItem.itemID != 0)
            {
                playerBag.itemList[fromIndex] = targetItem;
                playerBag.itemList[targetSlot] = currentItem;
            }
            else
            {
                playerBag.itemList[targetSlot] = currentItem;
                playerBag.itemList[fromIndex] = new InventoryItem{ itemID = 0, itemAmount = 0};
            }

            EventHandler.CallUpdateInventoryUI(InventoryLocationType.Player, playerBag.itemList);
        }

    }

}

