using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PEProtocal;
using UnityEngine.EventSystems;
public class CartItemUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Item CurrentItem;
    public Image image;
    public Text NameText;
    public Text PriceText;
    public Button BuyBtn;
    public Button GiftBtn;
    public Button DeleteBtn;
    public CartItem cartItem;
    public void SetItem(CartItem cartItem)
    {
        this.CurrentItem = InventorySys.Instance.GetItemById(cartItem.itemID);
        if (CurrentItem != null)
        {
            this.cartItem = cartItem;
            this.image.sprite = Resources.Load<Sprite>(CurrentItem.Sprite);
            this.NameText.text = CurrentItem.Name;
            if (cartItem.quantity != 1)
            {
                NameText.text = NameText.text + " " + cartItem.quantity.ToString() + " 個";
            }
            this.PriceText.text = cartItem.sellPrice.ToString();
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
            string toolTipText = KnapsackSlot.GetToolTipText(CurrentItem);
        }
    }
    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (CurrentItem != null)
            InventorySys.Instance.HideToolTip();
    }
}
