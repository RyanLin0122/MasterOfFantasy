using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LearnSkillWnd : WindowRoot
{
    public Text TxtSwordPoint;
    public Text TxtArcheryPoint;
    public Text TxtMagicPoint;
    public Text TxtTheologyPoint;
    public Image JobImg;

    public void Init()
    {
        TxtSwordPoint.text = GameRoot.Instance.ActivePlayer.SwordPoint.ToString();
        TxtArcheryPoint.text = GameRoot.Instance.ActivePlayer.ArcheryPoint.ToString();
        TxtMagicPoint.text = GameRoot.Instance.ActivePlayer.MagicPoint.ToString();
        TxtTheologyPoint.text = GameRoot.Instance.ActivePlayer.TheologyPoint.ToString();
        JobImg.sprite = ResSvc.Instance.GetJobImgByID(GameRoot.Instance.ActivePlayer.Job);
    }

    public bool IsOpen;
    public void openCloseWnd()
    {
        if (IsOpen == true)
        {
            MainCitySys.Instance.CloseLearnSkillUI();
            IsOpen = false;
        }
        else
        {
            MainCitySys.Instance.OpenLearnSkillUI();
            IsOpen = true;
        }
    }
    public void ClickCloseBtn()
    {
        MainCitySys.Instance.CloseLearnSkillUI();
        IsOpen = false;
    }
}
