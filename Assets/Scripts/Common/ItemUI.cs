using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    #region Data
    public Item Item { get; private set; }
    public int Amount = 0;
    #endregion

    #region UI Component
    private Image itemImage;
    private Text amountText;
    #endregion
    private Image ItemImage
    {
        get
        {
            if (itemImage == null)
            {
                itemImage = GetComponent<Image>();
            }
            return itemImage;
        }
    }
    private Text AmountText
    {
        get
        {
            if (amountText == null)
            {
                amountText = GetComponentInChildren<Text>();
            }
            return amountText;
        }
    }
    public void SetSellItem(Item item)
    {
        this.Item = item;
        this.Amount = 1;
        // update ui 
        int itemid = item.ItemID;
        ItemImage.sprite = Resources.Load<Sprite>(InventorySys.Instance.ItemList[itemid].Sprite);
        AmountText.text = "";
    }
    public void SetItem(Item item, int amount = 1)
    {
        this.Item = item;
        this.Amount = amount;
        // update ui 
        int itemid = item.ItemID;
        ItemImage.sprite = Resources.Load<Sprite>(item.Sprite);
        if (Item.Capacity > 1)
        {
            AmountText.text = Amount.ToString();
        }
        else
        {
            AmountText.text = "";
        }
    }
    public void AddAmount(int amount = 1)
    {
        this.Amount += amount;
        //update ui 
        if (Item.Capacity > 1)
        {
            AmountText.text = Amount.ToString();
        }
        else
        {
            AmountText.text = "";
        }
    }
    public void ReduceAmount(int amount = 1)
    {
        this.Amount -= amount;
        //update ui 
        if (Item.Capacity > 1)
        {
            AmountText.text = Amount.ToString();
        }
        else
        {
            AmountText.text = "";
        }
    }
    public void SetAmount(int amount)
    {
        this.Item.Count = amount;
        this.Amount = amount;
        //update ui 
        if (Item.Capacity > 1)
        {
            AmountText.text = Amount.ToString();
        }
        else
        {
            AmountText.text = "";
        }
    }
    //当前物品 跟 另一个物品 交换显示
    public void Exchange(ItemUI itemUI)
    {
        Item itemTemp = itemUI.Item;
        int amountTemp = itemUI.Amount;
        itemUI.SetItem(this.Item, this.Amount);
        this.SetItem(itemTemp, amountTemp);
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetLocalPosition(Vector3 position)
    {
        transform.localPosition = position;
    }
}