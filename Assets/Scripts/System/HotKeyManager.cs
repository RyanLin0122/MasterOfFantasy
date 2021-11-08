using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PEProtocal;
public class HotKeyManager : MonoBehaviour
{
    public Dictionary<KeyCode, HotKeySlot> HotKeySlots;
    public Transform HotkeyContainer;
    public int CurrentPage = 0;
    public Text CurrentPageTxt;
    public void Init()
    {
        if (HotkeyContainer != null)
        {
            HotKeySlots = new Dictionary<KeyCode, HotKeySlot>();
            var slots = HotkeyContainer.GetComponentsInChildren<HotKeySlot>();
            if (slots.Length > 0)
            {
                foreach (var slot in slots)
                {
                    HotKeySlots.Add(slot.keyCode, slot);
                }
            }
        }
    }
    public void ClearHotKeysUI()
    {
        foreach (var slot in HotKeySlots.Values)
        {
            slot.ResetUI();
        }
    }
    public void ReadHotKey()
    {
        if (GameRoot.Instance.ActivePlayer.Hotkeys != null && GameRoot.Instance.ActivePlayer.Hotkeys.Count > 0)
        {
            SetHotKey(CurrentPage);
        }
    }
    /// <summary>
    /// 設定第N頁的快捷鍵
    /// </summary>
    /// <param name="Page"></param>
    public void SetHotKey(int Page)
    {
        ClearHotKeysUI();
        HashSet<HotkeyData> datas = new HashSet<HotkeyData>();
        if (GameRoot.Instance.ActivePlayer.Hotkeys != null)
        {
            if (GameRoot.Instance.ActivePlayer.Hotkeys.Count > 0)
            {
                foreach (var data in GameRoot.Instance.ActivePlayer.Hotkeys)
                {
                    if (data.HotKeyState == 0)
                    {
                        GameRoot.Instance.ActivePlayer.Hotkeys.Remove(data);
                    }
                    if (data.PageIndex == Page && data.HotKeyState != 0)
                    {
                        datas.Add(data);
                    }
                }
            }
        }
        else
        {
            GameRoot.Instance.ActivePlayer.Hotkeys = new List<HotkeyData>();
        }
        if (datas.Count > 0)
        {
            foreach (var data in datas)
            {
                KeyCode key = (KeyCode)System.Enum.Parse(typeof(KeyCode), data.KeyCode);
                if (HotKeySlots.ContainsKey(key))
                {
                    if(data.HotKeyState == 1) HotKeySlots[key].State = HotKeyState.Consumable;
                    if (data.HotKeyState == 2) HotKeySlots[key].State = HotKeyState.Skill;
                    HotKeySlots[key].SetHotKeyUI(data);
                }
            }
        }
    }
    public void ProcessHotKeyOperation(ProtoMsg msg)
    {
        HotKeyOperation ho = msg.hotKeyOperation;
        var HotKeyDataList = GameRoot.Instance.ActivePlayer.Hotkeys;
        if (HotKeyDataList == null)
        {
            GameRoot.Instance.ActivePlayer.Hotkeys = new List<HotkeyData>();
        }
        if (ho == null)
        {
            return;
        }
        switch (ho.OperationType)
        {
            case 1: //新增快捷鍵
                if (ho.NewHotKeyData != null)
                {
                    if (HotKeyDataList.Count > 0)
                    {
                        int index = -1;
                        int OldIndex = -1;
                        for (int i = 0; i < HotKeyDataList.Count; i++)
                        {
                            if (ho.NewHotKeyData.PageIndex == HotKeyDataList[i].PageIndex && ho.NewHotKeyData.KeyCode == HotKeyDataList[i].KeyCode)
                            {
                                index = i;
                            }
                            if (ho.OldHotKeyData != null)
                            {
                                if (HotKeyDataList[i].PageIndex == ho.NewHotKeyData.PageIndex && HotKeyDataList[i].KeyCode != ho.NewHotKeyData.KeyCode
                                && HotKeyDataList[i].HotKeyState == ho.NewHotKeyData.HotKeyState && HotKeyDataList[i].ID == ho.NewHotKeyData.ID)
                                {
                                    OldIndex = i;
                                }
                            }
                        }
                        if (index == -1)
                        {
                            HotKeyDataList.Add(ho.NewHotKeyData);
                        }
                        else
                        {
                            HotKeyDataList[index] = ho.NewHotKeyData;
                        }
                        if (OldIndex != -1)
                        {
                            HotKeyDataList.RemoveAt(OldIndex);
                        }
                        if (ho.OldHotKeyData != null && OldIndex != -1)
                        {
                            KeyCode OldCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), ho.OldHotKeyData.KeyCode);
                            HotKeySlots[OldCode].ResetUI();
                        }
                    }
                    else
                    {
                        HotKeyDataList.Add(ho.NewHotKeyData);
                    }
                }
                KeyCode code = (KeyCode)System.Enum.Parse(typeof(KeyCode), ho.NewHotKeyData.KeyCode);
                if (HotKeySlots.ContainsKey(code))
                {
                    HotKeySlots[code].ResetUI();
                    HotKeySlots[code].SetHotKeyUI(ho.NewHotKeyData);
                }
                
                break;
        }
    }
    #region UI
    public void PressUpBtn()
    {
        if (CurrentPage < 3)
        {
            CurrentPage++;
        }
        else
        {
            CurrentPage = 0;
        }
        CurrentPageTxt.text = (CurrentPage + 1).ToString();
        SetHotKey(CurrentPage);
    }
    public void PressDownBtn()
    {
        if (CurrentPage > 0)
        {
            CurrentPage--;
        }
        else
        {
            CurrentPage = 3;
        }
        CurrentPageTxt.text = (CurrentPage + 1).ToString();
        SetHotKey(CurrentPage);
    }
    #endregion
}
