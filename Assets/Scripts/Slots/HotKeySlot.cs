using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PEProtocal;
public class HotKeySlot : MonoBehaviour
{
    public KeyCode keyCode;
    public Text NumberText;
    public HotKeyDragSource dragSource;
    public HotKeyDragTarget dragTarget;
    public Image ContentImg;
    public Text TxtItemCount;
    public Image ColdTimeImg;
    public Image ItemCountBG;
    public HotKeyState State;
    public HotkeyData data;
    public int ColdTimeTaskID = -1;
    public void SetColdTime(float maxTime, float time)
    {
        ColdTimeImg.fillAmount = time / maxTime;
        float Period = 0.1f;
        int MinusTimes = Mathf.CeilToInt(time / Period);
        ColdTimeTaskID = TimerSvc.Instance.AddTimeTask(
            (t) =>
            {
                float num = ColdTimeImg.fillAmount;
                if ((num - (1f / MinusTimes)) > 0)
                {
                    ColdTimeImg.fillAmount = num - (1f / MinusTimes);
                }
                else
                {
                    ColdTimeImg.fillAmount = 0;
                }
            }
            , Period, PETimeUnit.Second, MinusTimes);
    }
    public void RemoveColdTimeTask()
    {
        TimerSvc.Instance.DeleteTimeTask(ColdTimeTaskID);
        ColdTimeTaskID = -1;
    }
    public void ResetUI()
    {
        data = null;
        State = HotKeyState.Vacancy;
        RemoveColdTimeTask();
        ColdTimeImg.fillAmount = 0;
        ContentImg.sprite = null;
        ContentImg.gameObject.SetActive(false);
        TxtItemCount.gameObject.SetActive(false);
        ItemCountBG.gameObject.SetActive(false);
    }
    public void SetHotKey(DragBaseData data)
    {
        print("SetHotKey");
        if (data is DragItemData)
        {
            Item item = (Item)data.Content;
            if (item is Consumable)
            {
                print("Set Consumable");
                SendHotKeyRequest(item);
            }
            DragSystem.Instance.ReturnDragItem();
        }
        else if (data is DragSkillData)
        {
            SkillInfo info = (SkillInfo)data.Content;
            SendHotKeyRequest(info);
            DragSystem.Instance.RemoveDragObject();
        }
        else if (data is DragHotKeyData)
        {

        }
        else
        {

        }
    }
    public void SendHotKeyRequest(System.Object obj)
    {
        HotkeyData data = null;
        if (GameRoot.Instance.ActivePlayer.Hotkeys != null)
        {
            if(GameRoot.Instance.ActivePlayer.Hotkeys.Count > 0)
            {
                foreach (var hotkeyData in GameRoot.Instance.ActivePlayer.Hotkeys)
                {
                    if (hotkeyData.PageIndex == BattleSys.Instance.HotKeyManager.CurrentPage && hotkeyData.KeyCode == keyCode.ToString())
                    {
                        data = hotkeyData;
                    }
                }
            }
        }
        else
        {
            GameRoot.Instance.ActivePlayer.Hotkeys = new List<HotkeyData>();
        }
        HotkeyData newData = null;
        if(obj is SkillInfo)
        {
            newData = new HotkeyData
            {
                HotKeyState = 2,
                KeyCode = keyCode.ToString(),
                PageIndex = BattleSys.Instance.HotKeyManager.CurrentPage,
                ID = ((SkillInfo)obj).SkillID
            };
        }
        else if(obj is Consumable)
        {
            newData = new HotkeyData
            {
                HotKeyState = 1,
                KeyCode = keyCode.ToString(),
                PageIndex = BattleSys.Instance.HotKeyManager.CurrentPage,
                ID = ((Consumable)obj).ItemID
            };
        }
        if(newData!=null) //新增一個
        {
            new HotKeySender(1, newData);
        }
    }
    public void SetHotKeyUI(HotkeyData data)
    {
        if (data.PageIndex != BattleSys.Instance.HotKeyManager.CurrentPage)
        {
            return;
        }
        if (data.HotKeyState == 1) //消耗
        {
            if (keyCode.ToString() == data.KeyCode)
            {
                this.data = data;
                State = HotKeyState.Consumable;
                ColdTimeImg.fillAmount = 0;
                ContentImg.sprite = Resources.Load<Sprite>(InventorySys.Instance.itemList[data.ID].Sprite);
                ContentImg.gameObject.SetActive(true);
                int Count = 0;
                if (InventorySys.Instance.itemList[data.ID].IsCash)
                {
                    foreach (var item in GameRoot.Instance.ActivePlayer.CashKnapsack.Values)
                    {
                        if (item.ItemID == data.ID)
                        {
                            Count += item.Count;
                        }
                    }
                }
                else
                {
                    foreach (var item in GameRoot.Instance.ActivePlayer.NotCashKnapsack.Values)
                    {
                        if (item.ItemID == data.ID)
                        {
                            Count += item.Count;
                        }
                    }
                }
                TxtItemCount.text = Count.ToString();
                TxtItemCount.gameObject.SetActive(true);
                ItemCountBG.gameObject.SetActive(true);
                SetColdTime(1, 1); //ToDo
            }
        }
        else if (data.HotKeyState == 2) //技能
        {
            if (keyCode.ToString() == data.KeyCode)
            {
                this.data = data;
                State = HotKeyState.Consumable;
                ColdTimeImg.fillAmount = 0;
                ContentImg.sprite = ResSvc.Instance.SkillDic[data.ID].Icon;
                ContentImg.gameObject.SetActive(true);
                TxtItemCount.gameObject.SetActive(false);
                ItemCountBG.gameObject.SetActive(false);
                SetColdTime(1, 1); //ToDo
            }
        }
    }
}
public enum HotKeyState
{
    Vacancy,
    Consumable,
    Skill
}
