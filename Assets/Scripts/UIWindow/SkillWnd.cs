using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PEProtocal;
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

        var MySkills = GameRoot.Instance.ActivePlayer.Skills;
        if (MySkills != null && MySkills.Count > 0)
        {
            foreach (var skill in MySkills.Values)
            {
                if (IsJobTab)
                {
                    if (skill.SkillID >= 100)
                    {
                        SkillInfo info = ResSvc.Instance.SkillDic[skill.SkillID];
                        if (skill.SkillLevel > 0)
                        {
                            GameObject SkillGameObject = Instantiate(SkillPrefab);
                            SkillGameObject.transform.SetParent(SkillGroup.transform);
                            //SkillGameObject.transform.localPosition = new Vector3(SkillGameObject.transform.localPosition.x, SkillGameObject.transform.localPosition.y, 0);
                            SkillGameObject.GetComponent<SkillSlot>().SetInfo(info, skill.SkillLevel);
                            SkillToolTip skillToolTip = SkillGameObject.GetComponentInChildren<SkillToolTip>();
                            skillToolTip.SetSkill(info, skill.SkillLevel);
                        }
                    }
                }
                else
                {
                    if (skill.SkillID < 100)
                    {
                        SkillInfo info = ResSvc.Instance.SkillDic[skill.SkillID];
                        if (skill.SkillLevel > 0)
                        {
                            GameObject SkillGameObject = Instantiate(SkillPrefab);
                            SkillGameObject.transform.SetParent(SkillGroup.transform);
                            //SkillGameObject.transform.localPosition = new Vector3(SkillGameObject.transform.localPosition.x, SkillGameObject.transform.localPosition.y, 0);
                            SkillGameObject.GetComponent<SkillSlot>().SetInfo(info, skill.SkillLevel);
                            SkillToolTip skillToolTip = SkillGameObject.GetComponentInChildren<SkillToolTip>();
                            skillToolTip.SetSkill(info, skill.SkillLevel);
                        }
                    }
                }
            }
        }
    }
    public void ClearPanel() //清空欄位
    {
        if (SkillGroup.transform.childCount > 0)
        {
            SkillSlot[] slots = SkillGroup.transform.GetComponentsInChildren<SkillSlot>();
            foreach (var item in slots)
            {
                DestroyImmediate(item.gameObject);
            }
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
        InitSkillWnd();
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
        InitSkillWnd();
    }

    public void OpenAndPush()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.WindowOpen);
        SetWndState();
        IsOpen = true;
        IsJobTab = true;
        InitSkillWnd();
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
