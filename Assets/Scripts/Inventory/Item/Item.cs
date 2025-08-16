using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Farm.Inventory
{
    public class Item : MonoBehaviour
    {
        public int itmeID;

        private SpriteRenderer spriteRenderer;
        private BoxCollider2D coll;
        private ItemDetails itemDetails;

        private void Awake()
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            coll = GetComponent<BoxCollider2D>();
        }
        

        private void Start()
        {
            if (itmeID != 0)
            {
                Init(itmeID);
            }
            else
            {
                Debug.Log("数据未初始化,请在Inspector中设置Item ID或通过Init方法传入ID");
            }
        }


        public void Init(int ID)
        {
            itmeID = ID;

            itemDetails = InventoryManager.Instance.GetItemDetails(itmeID);

            if (itemDetails != null)
            {
                spriteRenderer.sprite = itemDetails.itemOnworldSprite ?? itemDetails.itemIcon;

                // 修改碰撞体尺寸
                Vector2 newSize = new Vector2(spriteRenderer.bounds.size.x, spriteRenderer.bounds.size.y);
                coll.size = newSize;
                // 利用图层重叠视觉效果从而实现物体的举起状态  此时会将锚点设置成物品下方 图像向上偏移
                // 此时需要修改碰撞体的位置   
                coll.offset = new Vector2(0, spriteRenderer.sprite.bounds.center.y);
            }
        }
    }

}