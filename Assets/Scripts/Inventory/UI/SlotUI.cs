using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

namespace Farm.Inventory
{
    public class SlotUI : MonoBehaviour,IPointerClickHandler,IBeginDragHandler,IDragHandler,IEndDragHandler
    {
        [Header("组件获取")]
        [SerializeField] private Image slotImage;
        [SerializeField] private TextMeshProUGUI amountText;
        public Image slotHightlight;
        [SerializeField] private Button button;

        [Header("格子类型")]
        public SlotType slotType;
        public bool isSelected;
        public int slotIndex;           //当前格子索引

        public ItemDetails itemDetails;

        public int itemAmount;          //背包当中当前栏位的物品数量

        public InventoryUI inventoryUI => GetComponentInParent<InventoryUI>();

        private void Start()
        {
            isSelected = true;
            if (itemDetails.itemID == 0)
            {
                UpdateEmptySlot();
            }
        }


        /// <summary>
        /// 更新格子UI和信息
        /// </summary>
        /// <param name="item">ItemDetails物品信息</param>
        /// <param name="amount">持有数量</param>
        public void UpdateSlot(ItemDetails item, int amount)
        {
            itemDetails = item;
            slotImage.sprite = item.itemIcon;
            slotImage.enabled = true;
            itemAmount = amount;
            amountText.text = amount > 1 ? amount.ToString() : string.Empty;
            button.interactable = true;
        }

        /// <summary>
        /// 将Slot更新为空
        /// </summary>
        public void UpdateEmptySlot()
        {
            if (isSelected)
            {
                isSelected = false;
            }

            slotImage.enabled = false;
            amountText.text = string.Empty;
            button.interactable = false;

            // 将物品数量置空 解决物品拖拽置空后，能可以拖转出物品的bug
            itemAmount = 0;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (itemAmount == 0) return;

            isSelected = !isSelected;
            inventoryUI.UpdateSlotHightlight(slotIndex);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (itemAmount > 0)         //解决物品拖拽置空后，能可以拖转出物品的bug
            {
                inventoryUI.dragItem.enabled = true;
                inventoryUI.dragItem.sprite = slotImage.sprite;
                inventoryUI.dragItem.SetNativeSize();

                isSelected = true;
                inventoryUI.UpdateSlotHightlight(slotIndex);

            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            inventoryUI.dragItem.transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            inventoryUI.dragItem.enabled = false;
            // Debug.Log(eventData.pointerCurrentRaycast.gameObject);

            if (eventData.pointerCurrentRaycast.gameObject != null)
            {
                if (eventData.pointerCurrentRaycast.gameObject.TryGetComponent<SlotUI>(out SlotUI targetSlot))
                {
                    int targetIndex = targetSlot.slotIndex;

                    if (slotType == SlotType.Bag && targetSlot.slotType == SlotType.Bag)
                    {
                        InventoryManager.Instance.SwapItem(slotIndex, targetIndex);
                    }
                }

                inventoryUI.UpdateSlotHightlight(-1);
            }
            // else    //测试扔到地上
            // {
            //     if (itemDetails.canDropped)
            //     {
            //         //因为摄像机默认z轴是-10，所以这里取负值
            //         //数据对应世界地图坐标
            //         var pos = Camera.main.ScreenToWorldPoint
            //             (new Vector3(Input.mousePosition.x,
            //             Input.mousePosition.y,
            //             -Camera.main.transform.position.z));

            //         EventHandler.CallInstantiateItemInScene(itemDetails.itemID, pos);
            //     }
            // }
        }

    }
}
