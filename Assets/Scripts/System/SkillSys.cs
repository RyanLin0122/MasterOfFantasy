using UnityEngine;
using PEProtocal;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

class SkillSys : SystemRoot
{
    public static SkillSys Instance = null;

    public override void InitSys()
    {
        base.InitSys();

        Instance = this;
        Debug.Log("Init SkillSys...");
    }

    public GameObject DragSkill;
    public SkillWnd skillWnd;

    public void OpenSkillWnd()
    {
        skillWnd.SetWndState(true);
        skillWnd.IsOpen = true;
    }

    public void CloseSkillWnd()
    {
        skillWnd.SetWndState(false);
        skillWnd.IsOpen = false;
    }



}

