using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;
using UnityEngine.UI;
using System;

public class Information : WindowRoot
{
    public GameObject additionInfo;
    public bool IsAdditionOpen = true;
    public bool IsOpen = false;
    public Text txtName;
    public Text txtCouple;
    public Text txtJob;
    public Text txtLevel;
    public Text txtGrade;
    public Text txtTitle;
    public Text txtHP;
    public Text txtMP;
    public Text txtEXP;
    public Text txtPoint;
    public Text txtAtt;
    public Text txtStrength;
    public Text txtAgility;
    public Text txtIntellect;
    public Text tempAtt;
    public Text tempStrength;
    public Text tempAgility;
    public Text tempIntellect;
    public Text txtDamage;
    public Text txtDefense;
    public Text txtAccuracy;
    public Text txtCriticalRate;
    public Text txtAvoidRate;
    public Text txtMagicDefense;
    public Button AttUpBtn;
    public Button StrengthUpBtn;
    public Button AgilityUpBtn;
    public Button IntellectUpBtn;
    public Button AttDownBtn;
    public Button StrengthDownBtn;
    public Button AgilityDownBtn;
    public Button IntellectDownBtn;
    public Button SummonBtn;
    public Button ApplyBtn;
    public Button ReverseBtn;
    public Button AdditionInfoBtn;
    public Toggle ShowAppearance;
    public Image HpImg;
    public Image MpImg;
    public Image HpImg2;
    public Image MpImg2;
    public Image ExpImage;
    public Text LevelText;
    public Image JobImg;
    public Illustration illustration;
    protected override void InitWnd()
    {
        Debug.Log("初始化InfoWnd");
        base.InitWnd();
        //SetActive(additionInfo, false);
        illustration.InitIllustration();
        RefreshIInfoUI();
    }


    public void RefreshIInfoUI() //根據GameRoot.Instance.ActivePlayer更新角色屬性相關UI(沒有回傳)
    {
        Player player = GameRoot.Instance.ActivePlayer;
        txtPoint.text = player.RestPoint.ToString();
        tempAtt.text = "0";
        tempStrength.text = "0";
        tempAgility.text = "0";
        tempIntellect.text = "0";
        BattleSys.Instance.InitAllAtribute();
        PlayerAttribute attr = BattleSys.Instance.FinalAttribute;
        //UI更新
        JobImg.sprite = ResSvc.Instance.GetJobImgByID(GameRoot.Instance.ActivePlayer.Job);
        LevelText.text = "LV." + player.Level;
        txtName.text = player.Name;
        txtCouple.text = player.CoupleName;
        txtJob.text = Constants.SetJobName(player.Job);
        txtLevel.text = player.Level.ToString();
        txtGrade.text = player.Grade.ToString();
        txtTitle.text = player.Title;
        txtHP.text = player.HP + " / " + attr.MAXHP;
        txtMP.text = player.MP + " / " + attr.MAXMP;
        HpImg.fillAmount = (float)(((double)player.HP) / attr.MAXHP);
        MpImg.fillAmount = (float)(((double)player.MP) / attr.MAXMP);
        HpImg2.fillAmount = (float)(((double)player.HP) / attr.MAXHP);
        MpImg2.fillAmount = (float)(((double)player.MP) / attr.MAXMP);
        GameRoot.Instance.UpdatePlayerHp((int)attr.MAXHP); //角色頭上血條
        txtEXP.text = Convert.ToString(player.Exp) + " / " + Tools.GetExpMax(player.Level);
        ExpImage.fillAmount = (float)(((double)player.Exp) / ((double)Tools.GetExpMax(player.Level)));

        txtAtt.text = attr.Att.ToString();
        txtStrength.text = attr.Strength.ToString();
        txtAgility.text = attr.Agility.ToString();
        txtIntellect.text = attr.Intellect.ToString();
        txtDamage.text = (int)attr.MinDamage + "~" + (int)attr.MaxDamage;
        txtDefense.text = attr.Defense.ToString();
        txtAccuracy.text = attr.Accuracy * 100 + "%";
        txtAvoidRate.text = attr.Avoid * 100 + "%";
        txtCriticalRate.text = attr.Critical * 100 + "%";
        txtMagicDefense.text = attr.MagicDefense * 100 + "%";
    }

    public void RefreshHPMP()
    {
        Player player = GameRoot.Instance.ActivePlayer;
        PlayerAttribute attr = BattleSys.Instance.FinalAttribute;
        txtHP.text = player.HP + " / " + attr.MAXHP;
        txtMP.text = player.MP + " / " + attr.MAXMP;
        HpImg.fillAmount = (float)(((double)player.HP) / attr.MAXHP);
        MpImg.fillAmount = (float)(((double)player.MP) / attr.MAXMP);
        HpImg2.fillAmount = (float)(((double)player.HP) / attr.MAXHP);
        MpImg2.fillAmount = (float)(((double)player.MP) / attr.MAXMP);
        GameRoot.Instance.UpdatePlayerHp((int)attr.MAXHP); //角色頭上血條
    }
    public void openCloseWnd()
    {
        if (IsOpen == true)
        {
            UISystem.Instance.CloseInfoWnd();
            IsOpen = false;
        }
        else
        {
            UISystem.Instance.OpenInfoWnd();
            SetIllustration();
            IsOpen = true;
        }
    }
    public void ClickCloseBtn()
    {
        UISystem.Instance.CloseInfoWnd();
        IsOpen = false;
    }
    public void OpenCloseAddition()
    {

        if (IsAdditionOpen)
        {
            additionInfo.SetActive(false);
            IsAdditionOpen = false;
            AudioSvc.Instance.PlayUIAudio(Constants.WindowClose);
        }
        else
        {
            additionInfo.SetActive(true);
            IsAdditionOpen = true;
            AudioSvc.Instance.PlayUIAudio(Constants.WindowOpen);
        }
    }
    public void AttUp()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        int _Att = GameRoot.Instance.ActivePlayer.Att + Convert.ToInt32(tempAtt.text);
        if (_Att < 20)
        {
            if (Convert.ToInt32(txtPoint.text) > 0)
            {
                txtPoint.text = (Convert.ToUInt32(txtPoint.text) - 1).ToString();
                tempAtt.text = (Convert.ToInt32(tempAtt.text) + 1).ToString();
            }
        }
        else if (_Att >= 20 && _Att < 40)
        {
            if (Convert.ToInt32(txtPoint.text) > 1)
            {
                txtPoint.text = (Convert.ToUInt32(txtPoint.text) - 2).ToString();
                tempAtt.text = (Convert.ToInt32(tempAtt.text) + 1).ToString();
            }
        }
        else if (_Att >= 40 && _Att < 60)
        {
            if (Convert.ToInt32(txtPoint.text) > 2)
            {
                txtPoint.text = (Convert.ToUInt32(txtPoint.text) - 3).ToString();
                tempAtt.text = (Convert.ToInt32(tempAtt.text) + 1).ToString();
            }
        }
        else if (_Att >= 60 && _Att < 80)
        {
            if (Convert.ToInt32(txtPoint.text) > 3)
            {
                txtPoint.text = (Convert.ToUInt32(txtPoint.text) - 4).ToString();
                tempAtt.text = (Convert.ToInt32(tempAtt.text) + 1).ToString();
            }
        }
        else if (_Att >= 80)
        {
            if (Convert.ToInt32(txtPoint.text) > 4)
            {
                txtPoint.text = (Convert.ToUInt32(txtPoint.text) - 5).ToString();
                tempAtt.text = (Convert.ToInt32(tempAtt.text) + 1).ToString();
            }
        }
    }
    public void AttDown()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        int num = 0;
        int value = GameRoot.Instance.ActivePlayer.Att + Convert.ToInt32(tempAtt.text);
        if (value <= 20)
        {
            num = 1;
        }
        else if (value > 20 && value <= 40)
        {
            num = 2;
        }
        else if (value > 40 && value <= 60)
        {
            num = 3;
        }
        else if (value > 60 && value <= 80)
        {
            num = 4;
        }
        else if (value > 80)
        {
            num = 5;
        }
        if (Convert.ToInt32(tempAtt.text) > 0)
        {
            txtPoint.text = (Convert.ToUInt32(txtPoint.text) + num).ToString();
            tempAtt.text = (Convert.ToInt32(tempAtt.text) - 1).ToString();
        }
    }
    public void StrengthUp()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        int _Health = GameRoot.Instance.ActivePlayer.Strength + Convert.ToInt32(tempStrength.text);
        if (_Health < 20)
        {
            if (Convert.ToInt32(txtPoint.text) > 0)
            {
                txtPoint.text = (Convert.ToUInt32(txtPoint.text) - 1).ToString();
                tempStrength.text = (Convert.ToInt32(tempStrength.text) + 1).ToString();
            }
        }
        else if (_Health >= 20 && _Health < 40)
        {
            if (Convert.ToInt32(txtPoint.text) > 1)
            {
                txtPoint.text = (Convert.ToUInt32(txtPoint.text) - 2).ToString();
                tempStrength.text = (Convert.ToInt32(tempStrength.text) + 1).ToString();
            }
        }
        else if (_Health >= 40 && _Health < 60)
        {
            if (Convert.ToInt32(txtPoint.text) > 2)
            {
                txtPoint.text = (Convert.ToUInt32(txtPoint.text) - 3).ToString();
                tempStrength.text = (Convert.ToInt32(tempStrength.text) + 1).ToString();
            }
        }
        else if (_Health >= 60 && _Health < 80)
        {
            if (Convert.ToInt32(txtPoint.text) > 3)
            {
                txtPoint.text = (Convert.ToUInt32(txtPoint.text) - 4).ToString();
                tempStrength.text = (Convert.ToInt32(tempStrength.text) + 1).ToString();
            }
        }
        else if (_Health >= 80)
        {
            if (Convert.ToInt32(txtPoint.text) > 4)
            {
                txtPoint.text = (Convert.ToUInt32(txtPoint.text) - 5).ToString();
                tempStrength.text = (Convert.ToInt32(tempStrength.text) + 1).ToString();
            }
        }
    }
    public void StrengthDown()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        int num = 0;
        int value = GameRoot.Instance.ActivePlayer.Strength + Convert.ToInt32(tempStrength.text);
        if (value <= 20)
        {
            num = 1;
        }
        else if (value > 20 && value <= 40)
        {
            num = 2;
        }
        else if (value > 40 && value <= 60)
        {
            num = 3;
        }
        else if (value > 60 && value <= 80)
        {
            num = 4;
        }
        else if (value > 80)
        {
            num = 5;
        }
        if (Convert.ToInt32(tempStrength.text) > 0)
        {
            txtPoint.text = (Convert.ToUInt32(txtPoint.text) + num).ToString();
            tempStrength.text = (Convert.ToInt32(tempStrength.text) - 1).ToString();
        }
    }
    public void AgilityUp()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        int _Dex = GameRoot.Instance.ActivePlayer.Agility + Convert.ToInt32(tempAgility.text);
        if (_Dex < 20)
        {
            if (Convert.ToInt32(txtPoint.text) > 0)
            {
                txtPoint.text = (Convert.ToUInt32(txtPoint.text) - 1).ToString();
                tempAgility.text = (Convert.ToInt32(tempAgility.text) + 1).ToString();
            }
        }
        else if (_Dex >= 20 && _Dex < 40)
        {
            if (Convert.ToInt32(txtPoint.text) > 1)
            {
                txtPoint.text = (Convert.ToUInt32(txtPoint.text) - 2).ToString();
                tempAgility.text = (Convert.ToInt32(tempAgility.text) + 1).ToString();
            }
        }
        else if (_Dex >= 40 && _Dex < 60)
        {
            if (Convert.ToInt32(txtPoint.text) > 2)
            {
                txtPoint.text = (Convert.ToUInt32(txtPoint.text) - 3).ToString();
                tempAgility.text = (Convert.ToInt32(tempAgility.text) + 1).ToString();
            }
        }
        else if (_Dex >= 60 && _Dex < 80)
        {
            if (Convert.ToInt32(txtPoint.text) > 3)
            {
                txtPoint.text = (Convert.ToUInt32(txtPoint.text) - 4).ToString();
                tempAgility.text = (Convert.ToInt32(tempAgility.text) + 1).ToString();
            }
        }
        else if (_Dex >= 80)
        {
            if (Convert.ToInt32(txtPoint.text) > 4)
            {
                txtPoint.text = (Convert.ToUInt32(txtPoint.text) - 5).ToString();
                tempAgility.text = (Convert.ToInt32(tempAgility.text) + 1).ToString();
            }
        }
    }
    public void AgilityDown()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        int num = 0;
        int value = GameRoot.Instance.ActivePlayer.Agility + Convert.ToInt32(tempAgility.text);
        if (value <= 20)
        {
            num = 1;
        }
        else if (value > 20 && value <= 40)
        {
            num = 2;
        }
        else if (value > 40 && value <= 60)
        {
            num = 3;
        }
        else if (value > 60 && value <= 80)
        {
            num = 4;
        }
        else if (value > 80)
        {
            num = 5;
        }
        if (Convert.ToInt32(tempAgility.text) > 0)
        {
            txtPoint.text = (Convert.ToUInt32(txtPoint.text) + num).ToString();
            tempAgility.text = (Convert.ToInt32(tempAgility.text) - 1).ToString();
        }

    }
    public void IntellectUp()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        int _Int = GameRoot.Instance.ActivePlayer.Intellect + Convert.ToInt32(tempIntellect.text);
        if (_Int < 20)
        {
            if (Convert.ToInt32(txtPoint.text) > 0)
            {
                txtPoint.text = (Convert.ToUInt32(txtPoint.text) - 1).ToString();
                tempIntellect.text = (Convert.ToInt32(tempIntellect.text) + 1).ToString();
            }
        }
        else if (_Int >= 20 && _Int < 40)
        {
            if (Convert.ToInt32(txtPoint.text) > 1)
            {
                txtPoint.text = (Convert.ToUInt32(txtPoint.text) - 2).ToString();
                tempIntellect.text = (Convert.ToInt32(tempIntellect.text) + 1).ToString();
            }
        }
        else if (_Int >= 40 && _Int < 60)
        {
            if (Convert.ToInt32(txtPoint.text) > 2)
            {
                txtPoint.text = (Convert.ToUInt32(txtPoint.text) - 3).ToString();
                tempIntellect.text = (Convert.ToInt32(tempIntellect.text) + 1).ToString();
            }
        }
        else if (_Int >= 60 && _Int < 80)
        {
            if (Convert.ToInt32(txtPoint.text) > 3)
            {
                txtPoint.text = (Convert.ToUInt32(txtPoint.text) - 4).ToString();
                tempIntellect.text = (Convert.ToInt32(tempIntellect.text) + 1).ToString();
            }
        }
        else if (_Int >= 80)
        {
            if (Convert.ToInt32(txtPoint.text) > 4)
            {
                txtPoint.text = (Convert.ToUInt32(txtPoint.text) - 5).ToString();
                tempIntellect.text = (Convert.ToInt32(tempIntellect.text) + 1).ToString();
            }
        }
    }
    public void IntellectDown()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        int num = 0;
        int value = GameRoot.Instance.ActivePlayer.Intellect + Convert.ToInt32(tempIntellect.text);
        if (value <= 20)
        {
            num = 1;
        }
        else if (value > 20 && value <= 40)
        {
            num = 2;
        }
        else if (value > 40 && value <= 60)
        {
            num = 3;
        }
        else if (value > 60 && value <= 80)
        {
            num = 4;
        }
        else if (value > 80)
        {
            num = 5;
        }
        if (Convert.ToInt32(tempIntellect.text) > 0)
        {
            txtPoint.text = (Convert.ToUInt32(txtPoint.text) + num).ToString();
            tempIntellect.text = (Convert.ToInt32(tempIntellect.text) - 1).ToString();
        }

    }
    public void DetermineBtn() //按下確定
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        UpdateProperty();
        tempAtt.text = "0";
        tempAgility.text = "0";
        tempStrength.text = "0";
        tempIntellect.text = "0";
    }
    public void CancelBtn()  //按下取消
    {
        Player pd = GameRoot.Instance.ActivePlayer;
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        txtPoint.text = pd.RestPoint.ToString();
        tempAtt.text = "0";
        tempAgility.text = "0";
        tempStrength.text = "0";
        tempIntellect.text = "0";
    }
    private int CalculateBackPoint(int tempValue, int RealValue)
    {
        int Result = 0;
        while (tempValue > 0)
        {
            if (RealValue + tempValue <= 20)
            {
                Result++;
                tempValue--;
            }
            else if (RealValue + tempValue > 20 && RealValue + tempValue <= 40)
            {
                Result += 2;
                tempValue--;
            }
            else if (RealValue + tempValue > 40 && RealValue + tempValue <= 60)
            {
                Result += 3;
                tempValue--;
            }
            else if (RealValue + tempValue > 60 && RealValue + tempValue <= 80)
            {
                Result += 4;
                tempValue--;
            }
            else if (RealValue + tempValue > 80)
            {
                Result += 5;
                tempValue--;
            }

        }
        return Result;
    }

    //Send Add Property Pkg
    public void UpdateProperty() //點屬性點回傳
    {
        AddPropertyPoint ap = new AddPropertyPoint();
        Player pd = GameRoot.Instance.ActivePlayer;
        ap.RestPoint = Convert.ToInt32(txtPoint.text);
        ap.Att = Convert.ToInt32(tempAtt.text);
        ap.Strength = Convert.ToInt32(tempStrength.text);
        ap.Agility = Convert.ToInt32(tempAgility.text);
        ap.Intellect = Convert.ToInt32(tempIntellect.text);
        new AddPropertySender(ap);
    }

    public void SetIllustration()
    {
        illustration.SetGenderAge(ShowAppearance.isOn, false, GameRoot.Instance.ActivePlayer);
    }
    public void UpdateHp(int UpdateHP)
    {
        Player pd = GameRoot.Instance.ActivePlayer;
        //人物頭上數字
        if (GameRoot.Instance.MainPlayerControl != null)
        {
            if (pd.HP > UpdateHP)
            {
                //扣血
                GameRoot.Instance.MainPlayerControl.GenerateDamageNum(pd.HP - UpdateHP, 2);
            }
            else if (pd.HP < UpdateHP)
            {
                //補血
                GameRoot.Instance.MainPlayerControl.GenerateDamageNum(pd.HP - UpdateHP, 2);
            }
        }
        if (UpdateHP <= BattleSys.Instance.FinalAttribute.MAXHP)
        {
            pd.HP = UpdateHP;
            GameRoot.Instance.MainPlayerControl.entity.nEntity.HP = UpdateHP;
        }
        txtHP.text = pd.HP + " / " + BattleSys.Instance.FinalAttribute.MAXHP;
        HpImg.fillAmount = (float)(((double)pd.HP) / BattleSys.Instance.FinalAttribute.MAXHP);
        HpImg2.fillAmount = (float)(((double)pd.HP) / BattleSys.Instance.FinalAttribute.MAXHP);
        GameRoot.Instance.UpdatePlayerHp((int)BattleSys.Instance.FinalAttribute.MAXHP);

    }
    public void UpdateMp(int UpdateMP)
    {
        Player pd = GameRoot.Instance.ActivePlayer;
        if (GameRoot.Instance.MainPlayerControl != null)
        {
            if (pd.MP > UpdateMP)
            {
                //扣魔
                GameRoot.Instance.MainPlayerControl.GenerateDamageNum(UpdateMP - pd.MP, 3);
            }
            else if (pd.MP < UpdateMP)
            {
                //補魔
                GameRoot.Instance.MainPlayerControl.GenerateDamageNum(pd.MP - UpdateMP, 3);
            }
        }
        if (UpdateMP <= BattleSys.Instance.FinalAttribute.MAXMP)
        {
            pd.MP = UpdateMP;
        }
        txtMP.text = pd.MP + " / " + BattleSys.Instance.FinalAttribute.MAXMP;
        MpImg.fillAmount = (float)(((double)pd.MP) / BattleSys.Instance.FinalAttribute.MAXMP);
        MpImg2.fillAmount = (float)(((double)pd.MP) / BattleSys.Instance.FinalAttribute.MAXMP);
    }
    public void SetDeathHP()
    {
        Player pd = GameRoot.Instance.ActivePlayer;
        pd.HP = 0;
        txtHP.text = pd.HP + " / " + BattleSys.Instance.FinalAttribute.MAXHP;
        HpImg.fillAmount = (float)(((double)pd.HP) / BattleSys.Instance.FinalAttribute.MAXHP);
        HpImg2.fillAmount = (float)(((double)pd.HP) / BattleSys.Instance.FinalAttribute.MAXHP);
        GameRoot.Instance.UpdatePlayerHp((int)BattleSys.Instance.FinalAttribute.MAXHP);
    }
}
