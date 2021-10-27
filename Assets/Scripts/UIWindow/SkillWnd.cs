using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillWnd : WindowRoot, IStackWnd
{
    private static SkillWnd _instance;
    public static SkillWnd Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = SkillSys.Instance.skillWnd;
            }
            return _instance;
        }
    }
    public bool IsOpen = false;
    public Button CloseBtn;
    public Button CloseBtn2;
    public Button JobSkillBtn;
    public Button MajorSkillBtn;
    public Text JobSkillText;
    public Text MajorSkillText;
    public Sprite PanelSprite1;
    public Sprite PanelSprite2;
    public Color Txtcolor;
    public Color referenceColor;
    public GameObject SkillPrefab;
    public bool IsJobTab = true;
    public GameObject SkillGroup;
    public Scrollbar scrollbar;

    public void InitSkillWnd()
    {
        ClearPanel();
        if (IsJobTab)
        {
            var MySkills = GameRoot.Instance.ActivePlayer.Skills;
            if (MySkills != null && MySkills.Count > 0)
            {
                foreach (var skill in MySkills)
                {
                    SkillInfo info = ResSvc.Instance.SkillDic[skill.SkillID];
                    GameObject SkillGameObject = Instantiate(SkillPrefab) as GameObject;
                    SkillGameObject.transform.SetParent(SkillGroup.transform);
                    SkillGameObject.transform.localPosition = new Vector3(SkillGameObject.transform.localPosition.x, SkillGameObject.transform.localPosition.y, 0);
                    SkillGameObject.GetComponent<SkillSlot>().SetInfo(info, skill.SkillLevel);
                }
            }
            
        }
        else
        {

        }
    }
    public void ClearPanel() //清空欄位
    {
        SkillSlot[] slots = SkillGroup.transform.GetComponentsInChildren<SkillSlot>();
        foreach (var item in slots)
        {
            DestroyImmediate(item.gameObject);
        }
    }

    public void ClickCloseBtn()
    {
        CloseAndPop();
    }

    public void PressJobSkillBtn()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.PickUpItem);
        //TO-DO Logic
        IsJobTab = true;
        JobSkillBtn.GetComponent<Image>().sprite = PanelSprite1;
        MajorSkillBtn.GetComponent<Image>().sprite = PanelSprite2;
        JobSkillText.text = "<color=#ffffff>職業技能</color>";
        MajorSkillText.text = "專攻技能";
        MajorSkillText.color = referenceColor;
    }

    public void PressMajorSkillBtn()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.PickUpItem);
        //TO-DO Logic
        IsJobTab = false;
        JobSkillBtn.GetComponent<Image>().sprite = PanelSprite2;
        MajorSkillBtn.GetComponent<Image>().sprite = PanelSprite1;
        JobSkillText.text = "職業技能";
        MajorSkillText.text = "<color=#ffffff>專攻技能</color>";
        JobSkillText.color = referenceColor;
    }

    public void OpenAndPush()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.WindowOpen);
        SetWndState();
        IsOpen = true;
        UISystem.Instance.Push(this);
    }

    public void CloseAndPop()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.WindowClose);
        SetWndState(false);
        IsOpen = false;
        InventorySys.Instance.HideToolTip();
        UISystem.Instance.ForcePop(this);
    }

    public void KeyBoardCommand()
    {
        if (IsOpen)
        {
            CloseAndPop();
            IsOpen = false;
        }
        else
        {
            OpenAndPush();
            IsOpen = true;
        }
    }
}
