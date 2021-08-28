using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;
using UnityEngine.UI;
using System;

public class Information : WindowRoot
{
    public GameObject additionInfo;
    public bool IsAdditionOpen = false;
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
        PECommon.Log("初始化InfoWnd");
        base.InitWnd();
        SetActive(additionInfo, false);
        illustration.InitIllustration();
        RefreshIInfoUI();
    }

    public int RealAttack = 0;
    public int RealStrength = 0;
    public int RealAgility = 0;
    public int RealIntellect = 0;
    public int RealMaxHp = 0;
    public int RealMaxMp = 0;
    public int RealMaxDamage = 0;
    public int RealMinDamage = 0;
    public int RealDefense = 0;
    public float RealAccuracy = 0;
    public float RealCritical = 0;
    public float RealAvoid = 0;
    public float RealMagicDefense = 0;
    public int RealPoint = 0;

    public void RefreshIInfoUI() //根據GameRoot.Instance.ActivePlayer更新角色屬性相關UI(沒有回傳)
    {
        Dictionary<string, float> EquipmentProperty = EquipmentWnd.Instance.CalculateEquipProperty();
        Player player = GameRoot.Instance.ActivePlayer;
        txtPoint.text = player.RestPoint.ToString();
        tempAtt.text = "0";
        tempStrength.text = "0";
        tempAgility.text = "0";
        tempIntellect.text = "0";
        RealAttack = player.Att + (int)EquipmentProperty["Attack"];
        RealStrength = player.Strength + (int)EquipmentProperty["Strength"];
        RealAgility = player.Agility + (int)EquipmentProperty["Agility"];
        RealIntellect = player.Intellect + (int)EquipmentProperty["Intellect"];
        RealMaxHp = 36 + (player.Level * 10) + (RealStrength * 16) + (int)EquipmentProperty["HP"];
        RealMaxMp = (player.Level * 10) + (RealIntellect * 12) + (int)EquipmentProperty["MP"];
        RealMaxDamage = player.Att * 2 + (int)EquipmentProperty["MaxDamage"];
        RealMinDamage = player.Att * 1 + (int)EquipmentProperty["MinDamage"];
        RealDefense = player.Strength * 2 + (int)EquipmentProperty["Defense"];
        RealAccuracy = 0.5f + 0.3f * RealAgility + EquipmentProperty["Accuracy"];
        RealCritical = 0.5f + 0.3f * RealAgility + EquipmentProperty["Critical"];
        RealAvoid = 0.5f + 0.3f * RealAgility + EquipmentProperty["Avoid"];
        RealMagicDefense = 0.3f + EquipmentProperty["MagicDefense"];

        //UI更新
        JobImg.sprite = ResSvc.Instance.GetJobImgByID(GameRoot.Instance.ActivePlayer.Job);
        LevelText.text = "LV." + player.Level;
        txtName.text = player.Name;
        txtCouple.text = player.CoupleName;
        txtJob.text = Constants.SetJobName(player.Job);
        txtLevel.text = player.Level.ToString();
        txtGrade.text = player.Grade.ToString();
        txtTitle.text = player.Title;
        txtHP.text = player.HP + " / " + RealMaxHp;
        txtMP.text = player.MP + " / " + RealMaxMp;
        HpImg.fillAmount = (float)(((double)player.HP) / RealMaxHp);
        MpImg.fillAmount = (float)(((double)player.MP) / RealMaxMp);
        HpImg2.fillAmount = (float)(((double)player.HP) / RealMaxHp);
        MpImg2.fillAmount = (float)(((double)player.MP) / RealMaxMp);
        GameRoot.Instance.UpdatePlayerHp(RealMaxHp); //角色頭上血條
        txtEXP.text = Convert.ToString(player.Exp) + " / " + Tools.GetExpMax(player.Level);
        ExpImage.fillAmount = (float)(((double)player.Exp) / ((double)Tools.GetExpMax(player.Level)));

        txtAtt.text = RealAttack.ToString();
        txtStrength.text = RealStrength.ToString();
        txtAgility.text = RealAgility.ToString();
        txtIntellect.text = RealIntellect.ToString();
        txtDamage.text = RealMinDamage + "~" + RealMaxDamage;
        txtDefense.text = RealDefense.ToString();
        txtAccuracy.text = RealAccuracy * 100 + "%";
        txtAvoidRate.text = RealAvoid * 100 + "%";
        txtCriticalRate.text = RealCritical * 100 + "%";
        txtMagicDefense.text = RealMagicDefense * 100 + "%";
        print("RealMP:" + RealMaxMp);
    }
    public void openCloseWnd()
    {
        if (IsOpen == true)
        {
            MainCitySys.Instance.CloseInfoWnd();
            IsOpen = false;
        }
        else
        {
            MainCitySys.Instance.OpenInfoWnd();
            SetIllustration();
            IsOpen = true;
        }
    }
    public void ClickCloseBtn()
    {
        MainCitySys.Instance.CloseInfoWnd();
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
        int _Att = GameRoot.Instance.CurrentPlayerData.Att + Convert.ToInt32(tempAtt.text);
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
        int value = GameRoot.Instance.CurrentPlayerData.Att + Convert.ToInt32(tempAtt.text);
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
        int _Health = GameRoot.Instance.CurrentPlayerData.Health + Convert.ToInt32(tempStrength.text);
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
        int value = GameRoot.Instance.CurrentPlayerData.Health + Convert.ToInt32(tempStrength.text);
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
        int _Dex = GameRoot.Instance.CurrentPlayerData.Dex + Convert.ToInt32(tempAgility.text);
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
        int value = GameRoot.Instance.CurrentPlayerData.Dex + Convert.ToInt32(tempAgility.text);
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
        int _Int = GameRoot.Instance.CurrentPlayerData.Int + Convert.ToInt32(tempIntellect.text);
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
        int value = GameRoot.Instance.CurrentPlayerData.Int + Convert.ToInt32(tempIntellect.text);
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
        if (GameRoot.Instance.PlayerControl != null)
        {
            if (pd.HP > UpdateHP)
            {
                //扣血
                GameRoot.Instance.PlayerControl.GenerateDamageNum(pd.HP - UpdateHP, 2);
            }
            else if (pd.HP < UpdateHP)
            {
                //補血
                GameRoot.Instance.PlayerControl.GenerateDamageNum(pd.HP - UpdateHP, 2);
            }

        }
        if (UpdateHP <= RealMaxHp)
        {
            pd.HP = UpdateHP;
        }
        txtHP.text = pd.HP + " / " + RealMaxHp;
        HpImg.fillAmount = (float)(((double)pd.HP) / RealMaxHp);
        HpImg2.fillAmount = (float)(((double)pd.HP) / RealMaxHp);
        GameRoot.Instance.UpdatePlayerHp(RealMaxHp);

    }
    public void UpdateMp(int UpdateMP)
    {
        Player pd = GameRoot.Instance.ActivePlayer;
        if (GameRoot.Instance.PlayerControl != null)
        {
            if (pd.MP > UpdateMP)
            {
                //扣魔
                GameRoot.Instance.PlayerControl.GenerateDamageNum(UpdateMP - pd.MP, 3);
            }
            else if (pd.MP < UpdateMP)
            {
                //補魔
                GameRoot.Instance.PlayerControl.GenerateDamageNum(pd.MP - UpdateMP, 3);
            }
        }
        if (UpdateMP <= RealMaxMp)
        {
            pd.MP = UpdateMP;
        }
        txtMP.text = pd.MP + " / " + RealMaxMp;
        MpImg.fillAmount = (float)(((double)pd.MP) / RealMaxMp);
        MpImg2.fillAmount = (float)(((double)pd.MP) / RealMaxMp);
    }
    public void SetDeathHP()
    {
        Player pd = GameRoot.Instance.ActivePlayer;
        pd.HP = 0;
        txtHP.text = pd.HP + " / " + RealMaxHp;
        HpImg.fillAmount = (float)(((double)pd.HP) / RealMaxHp);
        HpImg2.fillAmount = (float)(((double)pd.HP) / RealMaxHp);
        GameRoot.Instance.UpdatePlayerHp(RealMaxHp);
    }
}
