using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    #region Data
    public Item Item { get; private set; }
    public int Count = 0;
    #endregion

    #region UI Component
    public Image ItemImage;
    public Text AmountText;
    public Image BG;
    #endregion

    public void SetSellItem(Item item)
    {
        this.Item = item;
        this.Count = 1;
        // update ui 
        int itemid = item.ItemID;
        ItemImage.sprite = Resources.Load<Sprite>(InventorySys.Instance.ItemList[itemid].Sprite);
        AmountText.text = "";
        SetTxtBGOff();
    }
    public void SetItem(Item item)
    {
        this.Item = item;
        this.Count = item.Count;
        ItemImage.sprite = Resources.Load<Sprite>(item.Sprite);
        ItemImage.SetNativeSize();
        ItemImage.transform.localScale = new Vector3(0.5f, 0.5f, 1);
        if (Item.Capacity > 1 && Item.Count > 1)
        {
            AmountText.text = Count.ToString();
            SetTxtBGOn(AmountText);
        }
        else
        {
            SetTxtBGOff();
            AmountText.text = "";
        }
    }
    public void SetAmount(int amount)
    {
        this.Item.Count = amount;
        this.Count = amount;
        //update ui 
        if (Item.Capacity > 1 && Item.Count > 1)
        {
            AmountText.text = Count.ToString();
            SetTxtBGOn(AmountText);
        }
        else
        {
            AmountText.text = "";
            SetTxtBGOff();
        }
    }

    private void SetTxtBGOn(Text text)
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(text.GetComponent<RectTransform>());
        print("TextHeight = " + text.rectTransform.rect.height);
        print("TextWidth = " + text.rectTransform.rect.width);
        float TextHeight = text.rectTransform.rect.height;
        float TextWidth = text.rectTransform.rect.width;
        if (TextHeight == 0)
        {
            TextHeight = 14;
        }
        if (TextWidth == 0)
        {
            if(this.Item.Count <10) TextWidth = 8;
            else if(this.Item.Count<100 && this.Item.Count >=10) TextWidth = 15;
            else if (this.Item.Count < 1000 && this.Item.Count >= 100) TextWidth = 22;
        }
        BG.rectTransform.sizeDelta = new Vector2(TextWidth, TextHeight);
        Transform CountObj = BG.rectTransform.parent;
        Transform SlotTransform = transform.parent;
        CountObj.GetComponent<RectTransform>().sizeDelta = new Vector2(SlotTransform.GetComponent<RectTransform>().rect.width > 0 ? SlotTransform.GetComponent<RectTransform>().rect.width : 36, SlotTransform.GetComponent<RectTransform>().rect.height > 0 ? SlotTransform.GetComponent<RectTransform>().rect.height : 36);
        BG.gameObject.SetActive(true);
    }
    private void SetTxtBGOff()
    {
        BG.gameObject.SetActive(false);
    }
}