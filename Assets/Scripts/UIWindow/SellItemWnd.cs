using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;
using UnityEngine.UI;

public class SellItemWnd : MonoBehaviour
{
    public SellItemSlot slot;
    public GameObject ConfirmWnd;
    public GameObject NumberWnd;
    public bool IsSelling = false;
    public Text PriceText;
    public int SellCount = 0;
    public Item CurrentSellItem;
    public void Init()
    {
        this.gameObject.SetActive(true);
        KnapsackWnd.Instance.CloseBtn.interactable = false;
        KnapsackWnd.Instance.CloseBtn2.interactable = false;
    }
    public void ReceiveItem(Item item)
    {
        this.CurrentSellItem = item;
        this.IsSelling = true;
        if (item.Count > 1)
        {
            OpenNumberWnd();
        }
        else
        {
            this.SellCount = 1;
            this.PriceText.text = "�`�@ " + this.CurrentSellItem.SellPrice + " �Q��";
            OpenConfirmWnd();
        }
    }
    public InputField inputNumberField;
    public void OpenNumberWnd()
    {
        inputNumberField.text = this.CurrentSellItem.Count.ToString();
        NumberWnd.gameObject.SetActive(true);
    }

    public void PressNumberConfirmBtn()
    {
        int Count = 0;
        bool IsNumber = int.TryParse(inputNumberField.text, out Count);
        if (IsNumber)
        {
            if (Count <= 0) GameRoot.AddTips("�п�J�j��0���Ʀr");
            else if (Count > this.CurrentSellItem.Count) GameRoot.AddTips("�A�S������h���~");
            else
            {
                this.SellCount = Count;
                this.PriceText.text = "�`�@ " + this.CurrentSellItem.SellPrice * Count + " �Q��";
                NumberWnd.gameObject.SetActive(false);
                OpenConfirmWnd();

            }
        }
        else
        {
            GameRoot.AddTips("�п�J�Ʀr");
        }
    }

    public void OpenConfirmWnd()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        this.ConfirmWnd.gameObject.SetActive(true);
    }

    public void PressConfirmBtn()
    {
        ConfirmWnd.gameObject.SetActive(false);
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        if (this.CurrentSellItem != null)
        {
            new SellItemSender(this.CurrentSellItem.IsCash, this.CurrentSellItem.ItemID, this.SellCount, this.CurrentSellItem.Position);
        }
    }

    public void PressCancelBtn()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        ConfirmWnd.gameObject.SetActive(false);
        if (this.CurrentSellItem != null)
        {
            if (CurrentSellItem.IsCash)
            {
                var slot = KnapsackWnd.Instance.FindCashSlot(CurrentSellItem.Position);
                if (slot.transform.childCount > 0)
                {
                    GameObject ItemObj = slot.transform.GetComponentInChildren<ItemUI>().gameObject;
                    DestroyImmediate(ItemObj);
                    slot.StoreItem(GameRoot.Instance.ActivePlayer.CashKnapsack[CurrentSellItem.Position]);
                }
            }
            else
            {
                var slot = KnapsackWnd.Instance.FindSlot(CurrentSellItem.Position);
                if (slot.transform.childCount > 0)
                {
                    GameObject ItemObj = slot.transform.GetComponentInChildren<ItemUI>().gameObject;
                    DestroyImmediate(ItemObj);
                    slot.StoreItem(GameRoot.Instance.ActivePlayer.NotCashKnapsack[CurrentSellItem.Position]);
                }
            }
            this.CurrentSellItem = null;
            this.IsSelling = false;
        }
    }

    public void PressCloseBtn()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.WindowClose);
        OnClose();
    }
    public void OnClose()
    {
        KnapsackWnd.Instance.CloseBtn.interactable = true;
        KnapsackWnd.Instance.CloseBtn2.interactable = true;
        this.gameObject.SetActive(false);
    }

    public void Update()
    {
        if (IsSelling)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                if(NumberWnd.activeSelf && !ConfirmWnd.activeSelf)
                {
                    PressNumberConfirmBtn();
                }
                else if(!NumberWnd.activeSelf && ConfirmWnd.activeSelf)
                {
                    PressConfirmBtn();
                }
            }
        }           
    }
}
