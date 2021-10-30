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
                        int index = 0;
                        for (int i = 0; i < HotKeyDataList.Count; i++)
                        {
                            if(req.NewHotKeyData.PageIndex == HotKeyDataList[i].PageIndex && req.NewHotKeyData.KeyCode == HotKeyDataList[i].KeyCode)
                            {
                                index = i;
                            }
                        }
                        if(index == 0)
                        {
                            HotKeyDataList.Add(req.NewHotKeyData);
                        }
                        else
                        {
                            HotKeyDataList[index] = req.NewHotKeyData;
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
