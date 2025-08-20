using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class SlotUI : MonoBehaviour
{
    [Header("组件获取")]
    [SerializeField] private Image slotImage;
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private Image slotHightlight;
    [SerializeField] private Button button;

    [Header("格子类型")]
    public SlotType slotType;

    public bool isSelected;

    public ItemDetails itemDetails;
    
    public int itemAmount;          //背包当中当前栏位的物品数量


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
        itemAmount = amount;
        amountText.text = amount > 1 ? amount.ToString() : string.Empty;
        button.interactable = true;
    }

    /// <summary>
    /// 将Slot更新为空
    /// </summary>
    private void UpdateEmptySlot()
    {
        if (isSelected)
        {
            isSelected = false;
        }

        slotImage.enabled = false;
        amountText.text = string.Empty;
        button.interactable = false;
    }
}
