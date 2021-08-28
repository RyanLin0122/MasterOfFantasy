using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PEProtocal;
using UnityEngine.EventSystems;

public class CashShopItemUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Item CurrentItem;
    public Image image;
    public Text NameText;
    public Text PriceText;
    public Button BuyBtn;
    public Button GiftBtn;
    public Button SelectBtn;
    public void SetItem(int ItemID, int SellPrice = 999)
    {
        this.CurrentItem = InventoryManager.Instance.GetItemById(ItemID);
        if (CurrentItem != null)
        {
            this.image.sprite = Resources.Load<Sprite>(CurrentItem.Sprite);
            this.NameText.text = CurrentItem.Name;
            this.PriceText.text = SellPrice.ToString();
        }
        else
        {
            GameRoot.AddTips("無此道具");
        }

    }
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (CurrentItem != null)
        {
            string toolTipText = Slot.GetToolTipText(CurrentItem);
        }
    }
    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (CurrentItem != null)
            InventoryManager.Instance.HideToolTip();
    }
}
