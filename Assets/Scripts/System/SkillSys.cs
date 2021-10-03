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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            InstantiateSkill();
        }
    }

    public void InstantiateSkill()
    {
        GameObject go = Instantiate((GameObject)Resources.Load("Prefabs/SkillPrefab"));
        go.transform.SetParent(MainCitySys.Instance.MapCanvas.transform);
        go.transform.localPosition = GameRoot.Instance.MainPlayerControl.transform.localPosition;
        go.transform.localScale = new Vector3(100, 100, 1);
        SkillAnimator ani = go.GetComponent<SkillAnimator>();
        ani.Initialized("Effect/Skill Tracing Trap Explosion",8,14);
    }

}

