using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Farm.Inventory
{
    public class Item : MonoBehaviour
    {
        public int itemID;

        private SpriteRenderer spriteRenderer;
        private BoxCollider2D coll;
        public ItemDetails itemDetails;

        private void Awake()
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            coll = GetComponent<BoxCollider2D>();
        }
        

        private void Start()
        {
            if (itemID != 0)
            {
                Init(itemID);
            }
            else
            {
                Debug.Log("数据未初始化,请在Inspector中设置Item ID或通过Init方法传入ID");
            }
        }


        public void Init(int ID)
        
        {
            itemID = ID;

            itemDetails = InventoryManager.Instance.GetItemDetails(itemID);

            if (itemDetails != null)
            {
                // Unity重写了UnityEngine.Object（如Sprite）的==运算符。
                // 被销毁的对象在C#中不是真正的null，但Unity引擎会将其视为null。
                // 然而??运算符使用的是C#底层的null检查，无法识别Unity的"伪null"状态。
                spriteRenderer.sprite = itemDetails.itemOnworldSprite? itemDetails.itemOnworldSprite : itemDetails.itemIcon;

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