using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class KeyboardCommand
{
    public Action Command_I = () => { };
    public Action Command_E = () => { };
    public Action Command_D = () => { };

    public void SetDefault()
    {
        Command_I = () => KnapsackWnd.Instance.KeyBoardCommand();
        Command_E = () => EquipmentWnd.Instance.KeyBoardCommand();
        Command_D = () => DiaryWnd.Instance.KeyBoardCommand();
    }
    public void LoadLocalCommand()
    {
        //ToDo 讀取本地鍵盤配置

    }
    public void ResetCommand(Action command)
    {
        command = () => { };
    }

}
