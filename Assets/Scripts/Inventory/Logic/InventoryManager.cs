using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Farm.Inventory
{
    public class InventoryManager : Singleton<InventoryManager>
    {
        public ItemDataList_SO itemDataList_SO;

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
        /// 添加物品到Player背包里面
        /// </summary>
        /// <param name="item"></param>
        /// <param name="toDestory">是否要摧毁物品</param>
        public void AddItem(Item item, bool toDestory)
        {
            Debug.Log($"添加物品ID:{item.itemDetails.itemID} + 物品名字:{item.itemDetails.itemName}  到背包");
            if (toDestory)
            {
                // 销毁物品对象
                Destroy(item.gameObject);
            }
        }
    }

}

