using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Farm.Inventory
{
    public class ItmePickUp : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            Item item = other.GetComponent<Item>();
            
            if(item != null)
            {
                if (item.itemDetails.canPickedUp)
                {
                    //拾取物品添加到背包
                    InventoryManager.Instance.AddItem(item, true);
                }
                // 销毁物品对象
                Destroy(item.gameObject);
            }
        }
    }

}