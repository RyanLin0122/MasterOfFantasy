using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using PEProtocal;

public class LearnSkillSlot : MonoBehaviour, IPointerDownHandler
{
    public SkillInfo info;
    public Text SkillName;
    public Image SkillImg;
    public Text TxtSwordPoint;
    public Text TxtArcheryPoint;
    public Text TxtMagicPoint;
    public Text TxtTheologyPoint;
    public Image DisabledImg;
    public Text TxtRequiredLevel;
    public bool IsEnable = false;
    public LearnSkillWnd LearnSkillWnd;
    public int SkillLevel = 1;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (IsEnable)
            {
                Choose();
            }
        }
    }

    public void SetSkillInfo(SkillInfo info, LearnSkillWnd learnSkillWnd, int SkillLevel = 1)
    {
        this.info = info;
        LearnSkillWnd = learnSkillWnd;
        this.SkillLevel = SkillLevel;
        switch (SkillLevel)
        {
            case 1:
                SkillName.text = info.SkillName + " I";
                break;
            case 2:
                SkillName.text = info.SkillName + " II";
                break;
            case 3:
                SkillName.text = info.SkillName + " III";
                break;
            case 4:
                SkillName.text = info.SkillName + " IV";
                break;
            case 5:
                SkillName.text = info.SkillName + " V";
                break;
        }
        SkillImg.sprite = Resources.Load<Sprite>(info.Icon);
        TxtRequiredLevel.text = info.RequiredLevel[SkillLevel - 1].ToString();
        TxtSwordPoint.text = info.SwordPoint[SkillLevel - 1].ToString();
        TxtArcheryPoint.text = info.ArcheryPoint[SkillLevel - 1].ToString();
        TxtMagicPoint.text = info.MagicPoint[SkillLevel - 1].ToString();
        TxtTheologyPoint.text = info.TheologyPoint[SkillLevel - 1].ToString();
        Player player = GameRoot.Instance.ActivePlayer;
        UnChoose();
        if (player.SwordPoint >= info.SwordPoint[SkillLevel - 1] && player.ArcheryPoint >= info.ArcheryPoint[SkillLevel - 1]
            && player.MagicPoint >= info.MagicPoint[SkillLevel - 1] && player.TheologyPoint >= info.TheologyPoint[SkillLevel - 1] && player.Level >= info.RequiredLevel[SkillLevel - 1])
        {
            DisabledImg.gameObject.SetActive(false);
            IsEnable = true;
        }
        else
        {
            DisabledImg.gameObject.SetActive(true);
            IsEnable = false;
        }
    }
    public void Choose()
    {
        if (LearnSkillWnd != null)
        {
            LearnSkillSlot[] learnSkillSlots = LearnSkillWnd.LearnSkillContainer.GetComponentsInChildren<LearnSkillSlot>();
            foreach (var slot in learnSkillSlots)
            {
                slot.UnChoose();
            }
            LearnSkillWnd.ChoosedLearnSkillSlot = this;
            GetComponent<Outline>().enabled = true;
        }

    }
    public void UnChoose()
    {
        GetComponent<Outline>().enabled = false;
    }
}
