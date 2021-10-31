using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEProtocal;
public class HotKeyHandler : GameHandler
{
    protected override void Process(ProtoMsg msg, ServerSession session)
    {
        HotKeyOperation req = msg.hotKeyOperation;
        if (req == null)
        {
            return;
        }
        var HotKeyDataList = session.ActivePlayer.Hotkeys;
        if(HotKeyDataList == null)
        {
            session.ActivePlayer.Hotkeys = new List<HotkeyData>();
        }
        switch (req.OperationType)
        {
            case 1: //新增快捷鍵
                if (req.NewHotKeyData != null)
                {
                    if (HotKeyDataList.Count > 0)
                    {
                        int index = -1;
                        int OldIndex = -1; 
                        for (int i = 0; i < HotKeyDataList.Count; i++)
                        {
                            if(req.NewHotKeyData.PageIndex == HotKeyDataList[i].PageIndex && req.NewHotKeyData.KeyCode == HotKeyDataList[i].KeyCode)
                            {
                                index = i;
                            }
                            if (req.OldHotKeyData != null)
                            {
                                if (HotKeyDataList[i].PageIndex == req.NewHotKeyData.PageIndex && HotKeyDataList[i].KeyCode != req.NewHotKeyData.KeyCode
                                && HotKeyDataList[i].HotKeyState == req.NewHotKeyData.HotKeyState && HotKeyDataList[i].ID == req.NewHotKeyData.ID)
                                {
                                    OldIndex = i;
                                }
                            }

                        }
                        if(index == -1)
                        {
                            HotKeyDataList.Add(req.NewHotKeyData);
                        }
                        else
                        {
                            HotKeyDataList[index] = req.NewHotKeyData;
                        }
                        if (OldIndex != -1)
                        {
                            HotKeyDataList.RemoveAt(OldIndex);
                        }
                    }
                    else
                    {
                        HotKeyDataList.Add(req.NewHotKeyData);
                    }
                }
                session.WriteAndFlush(msg);
                break;
        }
    }
}
