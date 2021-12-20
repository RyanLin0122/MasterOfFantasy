using UnityEngine;
using UnityEngine.UI;
using PEProtocal;


public class CreateWnd : WindowRoot
{
    #region Properties
    public Text iptName;
    public Text Gender;
    public Text Face;
    public Text Downwear;
    public Text Hair;
    public Text Upwear;
    public Text Shoes;
    public Text RestPoint;
    public Text AttPoint;
    public Text StrengthPoint;
    public Text AgilityPoint;
    public Text IntellectPoint;

    public Image JobImg;
    public Sprite WarriorImg;
    public Sprite ArcherImg;
    public Sprite MagicianImg;
    public Sprite MonkImg;
    public Text JobIntro;

    public int restpoint = 10;
    public int Attpoint = 4;
    public int Strengthpoint = 4;
    public int Agilitypoint = 4;
    public int Intellectpoint = 4;
    public CharacterDemo Demo;
    public int SelectedJob = 1;
    public bool isNameExist = true;
    public Illustration illustration;
    public Player TempData;

    #endregion

    //初始化
    protected override void InitWnd()
    {
        base.InitWnd();
        TempData = new Player
        {
            Name = "",
            Gender = 0,
            Job = 0,
            Level = 0,           
            Att = 0,
            Strength = 0,
            Agility = 0,
            Intellect = 0,
            IsNew = true,
            MapID = 0,
            Grade = 1,
            RestPoint = 0,            
            playerEquipments = new PlayerEquipments {
                F_Chest = (Equipment)InventorySys.Instance.GetNewItemByID(3005),
                F_Pants = (Equipment)InventorySys.Instance.GetNewItemByID(3013),
                F_Shoes = (Equipment)InventorySys.Instance.GetNewItemByID(3021)
            },
        };
        Demo.SetAllEquipment(TempData);
        illustration.SetGenderAge(true, false, TempData);
        ClickMagicianBtn();
    }

    //UI事件
    public void ClickCancelBtn()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        AttPoint.text = "4";
        StrengthPoint.text = "4";
        AgilityPoint.text = "4";
        IntellectPoint.text = "4";
        Attpoint = 4;
        Strengthpoint = 4;
        Agilitypoint = 4;
        Intellectpoint = 4;
        RestPoint.text = "10";
        restpoint = 10;
        LoginSys.Instance.BackToSelectCharacterWnd();
    }
    public void ClickDetermineBtn()
    {
        
        AudioSvc.Instance.PlayUIAudio(Constants.LargeBtn);
        //檢查名稱長度
        if (iptName.text!=""&& iptName.text.Length>=1&& iptName.text.Length<=8)
        {
            //如果否或點數沒點完，要換名稱或點完
            if (restpoint == 0)
            {
                GameRoot.Instance.WindowLock();
                //如果是，傳給伺服器創角
                CreateInfo Info = new CreateInfo
                {
                    name = iptName.text,
                    gender = TempData.Gender,
                    job = SelectedJob,
                    att = int.Parse(AttPoint.text),
                    strength = int.Parse(StrengthPoint.text),
                    agility = int.Parse(AgilityPoint.text),
                    intellect = int.Parse(IntellectPoint.text),
                    MaxHp = 50,
                    MaxMp = 50,
                    Server = GameRoot.Instance.ActiveServer,
                    CreateTime = System.DateTime.Now.ToString("MM-dd-HH-mm-yyyy"),
                    LastLoginTime = "",
                    Fashionchest = TempData.playerEquipments.F_Chest.ItemID,
                    Fashionhairstyle = 0,
                    Fashionpant = TempData.playerEquipments.F_Pants.ItemID,
                    Fashionshoes = TempData.playerEquipments.F_Shoes.ItemID
                };
                new CreateSender(Info);
            }
            else
            {
                GameRoot.AddTips("剩餘點數要點完喔!!");
            }
        }
        else
        {           
            GameRoot.AddTips("名字不得空白，以及小於9個字");
        }
        
    }

    #region 設定角色屬性點
    public void ClickAttUpBtn()
    {
        audioSvc.PlayUIAudio(Constants.SmallBtn);
        if (restpoint > 0)
        {
            Attpoint += 1;
            restpoint -= 1;
            AttPoint.text = (int.Parse(AttPoint.text) + 1).ToString();
            RestPoint.text = (int.Parse(RestPoint.text) - 1).ToString();
        }
    }
    public void ClickAttDownBtn()
    {
        audioSvc.PlayUIAudio(Constants.SmallBtn);
        if (Attpoint > 4)
        {
            Attpoint -= 1;
            restpoint += 1;
            AttPoint.text = (int.Parse(AttPoint.text) - 1).ToString();
            RestPoint.text = (int.Parse(RestPoint.text) + 1).ToString();
        }
    }
    public void ClickHealthUpBtn()
    {
        audioSvc.PlayUIAudio(Constants.SmallBtn);
        if (restpoint > 0)
        {
            Strengthpoint += 1;
            restpoint -= 1;
            StrengthPoint.text = (int.Parse(StrengthPoint.text) + 1).ToString();
            RestPoint.text = (int.Parse(RestPoint.text) - 1).ToString();
        }
    }
    public void ClickHealthDownBtn()
    {
        audioSvc.PlayUIAudio(Constants.SmallBtn);
        if (Strengthpoint > 4)
        {
            Strengthpoint -= 1;
            restpoint += 1;
            StrengthPoint.text = (int.Parse(StrengthPoint.text) - 1).ToString();
            RestPoint.text = (int.Parse(RestPoint.text) + 1).ToString();
        }
    }
    public void ClickDexUpBtn()
    {
        audioSvc.PlayUIAudio(Constants.SmallBtn);
        if (restpoint > 0)
        {
            Agilitypoint += 1;
            restpoint -= 1;
            AgilityPoint.text = (int.Parse(AgilityPoint.text) + 1).ToString();
            RestPoint.text = (int.Parse(RestPoint.text) - 1).ToString();
        }
    }
    public void ClickDexDownBtn()
    {
        audioSvc.PlayUIAudio(Constants.SmallBtn);
        if (Agilitypoint > 4)
        {
            Agilitypoint -= 1;
            restpoint += 1;
            AgilityPoint.text = (int.Parse(AgilityPoint.text) - 1).ToString();
            RestPoint.text = (int.Parse(RestPoint.text) + 1).ToString();
        }
    }
    public void ClickIntUpBtn()
    {
        audioSvc.PlayUIAudio(Constants.SmallBtn);
        if (restpoint > 0)
        {
            Intellectpoint += 1;
            restpoint -= 1;
            IntellectPoint.text = (int.Parse(IntellectPoint.text) + 1).ToString();
            RestPoint.text = (int.Parse(RestPoint.text) - 1).ToString();
        }
    }
    public void ClickIntDownBtn()
    {
        audioSvc.PlayUIAudio(Constants.SmallBtn);
        if (Intellectpoint > 4)
        {
            Intellectpoint -= 1;
            restpoint += 1;
            IntellectPoint.text = (int.Parse(IntellectPoint.text) - 1).ToString();
            RestPoint.text = (int.Parse(RestPoint.text) + 1).ToString();
        }
    }
    public void ClickWarriorBtn()
    {
        audioSvc.PlayUIAudio(Constants.SmallBtn);
        if (SelectedJob != 1)
        {
            SelectedJob = 1;
            JobImg.sprite = WarriorImg;
            JobIntro.text = "身為接近能力相當出眾的職業，主要能力值為攻擊與體力。\n\n可以使用的裝備為短劍、刀劍、鈍器、雙手劍、雙手槍、雙手斧頭、盾牌、戰士專用盔甲。";
        }
    }
    public void ClickArcherBtn()
    {
        audioSvc.PlayUIAudio(Constants.SmallBtn);
        if (SelectedJob != 2)
        {
            SelectedJob = 2;
            JobImg.sprite = ArcherImg;
            JobIntro.text = "身為擅長遠距離攻擊的職業，主要能力值為敏捷與攻擊。\n\n可以使用的裝備為短劍、弓、弩、槍枝、弓箭手專用盔甲。";
        }
    }
    public void ClickMagicianBtn()
    {
        audioSvc.PlayUIAudio(Constants.SmallBtn);
        if (SelectedJob != 3)
        {
            SelectedJob = 3;
            JobImg.sprite = MagicianImg;
            JobIntro.text = "身為可施展快速魔法與範圍攻擊的職業，主要能力值為智力與攻擊。\n\n可以使用的裝備為短劍、法杖、魔法書、法師專用盔甲。";
        }
    }
    public void ClickMonkBtn()
    {
        audioSvc.PlayUIAudio(Constants.SmallBtn);
        if (SelectedJob != 4)
        {
            SelectedJob = 4;
            JobImg.sprite = MonkImg;
            JobIntro.text = "身為擅長輔助與恢復的職業，主要能力值為敏捷與攻擊。\n\n可以使用的裝備為法杖、鈍器、十字架、聖職專用盔甲。";
        }
    }

    #endregion

    #region 設定角色外觀
    public void ClkGenderUp()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        if (Gender.text == "女生")
        {
            Gender.text = "男生";
            TempData.Gender = 1;
            Upwear.text = "基本上衣1";
            Downwear.text = "基本下衣1";
            Shoes.text = "基本鞋子1";
            TempData.playerEquipments.F_Chest = (Equipment)InventorySys.Instance.GetNewItemByID(3001);
            TempData.playerEquipments.F_Pants = (Equipment)InventorySys.Instance.GetNewItemByID(3009);
            TempData.playerEquipments.F_Shoes = (Equipment)InventorySys.Instance.GetNewItemByID(3017);
            
        }
        else if (Gender.text == "男生")
        {
            Gender.text = "女生";
            TempData.Gender = 0;
            Upwear.text = "基本上衣1";
            Downwear.text = "基本下衣1";
            Shoes.text = "基本鞋子1";
            TempData.playerEquipments.F_Chest = (Equipment)InventorySys.Instance.GetNewItemByID(3005);
            TempData.playerEquipments.F_Pants = (Equipment)InventorySys.Instance.GetNewItemByID(3013);
            TempData.playerEquipments.F_Shoes = (Equipment)InventorySys.Instance.GetNewItemByID(3021);
        }
        Demo.SetAllEquipment(TempData);
        illustration.SetGenderAge(true, false, TempData);
    }
    public void ClkHairBtn()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
    }
    public void ClkFaceBtn()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
    }
    public void ClkUpwearUp()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        if(Upwear.text == "基本上衣1")
        {
            Upwear.text = "基本上衣2";
            if (TempData.Gender == 0)
            {
                TempData.playerEquipments.F_Chest = (Equipment)InventorySys.Instance.GetNewItemByID(3006);
            }
            else
            {
                TempData.playerEquipments.F_Chest = (Equipment)InventorySys.Instance.GetNewItemByID(3002);
            }
        }
        else if (Upwear.text == "基本上衣2")
        {
            Upwear.text = "基本上衣3";
            if (TempData.Gender == 0)
            {
                TempData.playerEquipments.F_Chest = (Equipment)InventorySys.Instance.GetNewItemByID(3007);
            }
            else
            {
                TempData.playerEquipments.F_Chest = (Equipment)InventorySys.Instance.GetNewItemByID(3003);
            }
        }
        else if (Upwear.text == "基本上衣3")
        {
            Upwear.text = "基本上衣4";
            if (TempData.Gender == 0)
            {
                TempData.playerEquipments.F_Chest = (Equipment)InventorySys.Instance.GetNewItemByID(3008);
            }
            else
            {
                TempData.playerEquipments.F_Chest = (Equipment)InventorySys.Instance.GetNewItemByID(3004);
            }
        }
        else if (Upwear.text == "基本上衣4")
        {
            Upwear.text = "基本上衣1";
            if (TempData.Gender == 0)
            {
                TempData.playerEquipments.F_Chest = (Equipment)InventorySys.Instance.GetNewItemByID(3005);
            }
            else
            {
                TempData.playerEquipments.F_Chest = (Equipment)InventorySys.Instance.GetNewItemByID(3001);
            }
        }
        Demo.SetAllEquipment(TempData);
        illustration.SetGenderAge(true, false, TempData);
    }
    public void ClkUpwearDown()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        if (Upwear.text == "基本上衣1")
        {
            Upwear.text = "基本上衣4";
            if (TempData.Gender == 0)
            {
                TempData.playerEquipments.F_Chest = (Equipment)InventorySys.Instance.GetNewItemByID(3008);
            }
            else
            {
                TempData.playerEquipments.F_Chest = (Equipment)InventorySys.Instance.GetNewItemByID(3004);
            }
        }
        else if (Upwear.text == "基本上衣2")
        {
            Upwear.text = "基本上衣1";
            if (TempData.Gender == 0)
            {
                TempData.playerEquipments.F_Chest = (Equipment)InventorySys.Instance.GetNewItemByID(3005);
            }
            else
            {
                TempData.playerEquipments.F_Chest = (Equipment)InventorySys.Instance.GetNewItemByID(3001);
            }
        }
        else if (Upwear.text == "基本上衣3")
        {
            Upwear.text = "基本上衣2";
            if (TempData.Gender == 0)
            {
                TempData.playerEquipments.F_Chest = (Equipment)InventorySys.Instance.GetNewItemByID(3006);
            }
            else
            {
                TempData.playerEquipments.F_Chest = (Equipment)InventorySys.Instance.GetNewItemByID(3002);
            }
        }
        else if (Upwear.text == "基本上衣4")
        {
            Upwear.text = "基本上衣3";
            if (TempData.Gender == 0)
            {
                TempData.playerEquipments.F_Chest = (Equipment)InventorySys.Instance.GetNewItemByID(3007);
            }
            else
            {
                TempData.playerEquipments.F_Chest = (Equipment)InventorySys.Instance.GetNewItemByID(3003);
            }
        }
        Demo.SetAllEquipment(TempData);
        illustration.SetGenderAge(true, false, TempData);
    }
    public void ClkDownwearUp()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        if (Downwear.text == "基本下衣1")
        {
            Downwear.text = "基本下衣2";
            if (TempData.Gender == 0)
            {
                TempData.playerEquipments.F_Pants = (Equipment)InventorySys.Instance.GetNewItemByID(3014);
            }
            else
            {
                TempData.playerEquipments.F_Pants = (Equipment)InventorySys.Instance.GetNewItemByID(3010);
            }
        }
        else if (Downwear.text == "基本下衣2")
        {
            Downwear.text = "基本下衣3";
            if (TempData.Gender == 0)
            {
                TempData.playerEquipments.F_Pants = (Equipment)InventorySys.Instance.GetNewItemByID(3015);
            }
            else
            {
                TempData.playerEquipments.F_Pants = (Equipment)InventorySys.Instance.GetNewItemByID(3011);
            }
        }
        else if (Downwear.text == "基本下衣3")
        {
            Downwear.text = "基本下衣4";
            if (TempData.Gender == 0)
            {
                TempData.playerEquipments.F_Pants = (Equipment)InventorySys.Instance.GetNewItemByID(3016);
            }
            else
            {
                TempData.playerEquipments.F_Pants = (Equipment)InventorySys.Instance.GetNewItemByID(3012);
            }
        }
        else if (Downwear.text == "基本下衣4")
        {
            Downwear.text = "基本下衣1";
            if (TempData.Gender == 0)
            {
                TempData.playerEquipments.F_Pants = (Equipment)InventorySys.Instance.GetNewItemByID(3013);
            }
            else
            {
                TempData.playerEquipments.F_Pants = (Equipment)InventorySys.Instance.GetNewItemByID(3009);
            }
        }
        Demo.SetAllEquipment(TempData);
        illustration.SetGenderAge(true, false, TempData);
    }
    public void ClkDownwearDown()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        if (Downwear.text == "基本下衣1")
        {
            Downwear.text = "基本下衣4";
            if (TempData.Gender == 0)
            {
                TempData.playerEquipments.F_Pants = (Equipment)InventorySys.Instance.GetNewItemByID(3016);
            }
            else
            {
                TempData.playerEquipments.F_Pants = (Equipment)InventorySys.Instance.GetNewItemByID(3012);
            }
        }
        else if (Downwear.text == "基本下衣2")
        {
            Downwear.text = "基本下衣1";
            if (TempData.Gender == 0)
            {
                TempData.playerEquipments.F_Pants = (Equipment)InventorySys.Instance.GetNewItemByID(3013);
            }
            else
            {
                TempData.playerEquipments.F_Pants = (Equipment)InventorySys.Instance.GetNewItemByID(3009);
            }
        }
        else if (Downwear.text == "基本下衣3")
        {
            Downwear.text = "基本下衣2";
            if (TempData.Gender == 0)
            {
                TempData.playerEquipments.F_Pants = (Equipment)InventorySys.Instance.GetNewItemByID(3014);
            }
            else
            {
                TempData.playerEquipments.F_Pants = (Equipment)InventorySys.Instance.GetNewItemByID(3010);
            }
        }
        else if (Downwear.text == "基本下衣4")
        {
            Downwear.text = "基本下衣3";
            if (TempData.Gender == 0)
            {
                TempData.playerEquipments.F_Pants = (Equipment)InventorySys.Instance.GetNewItemByID(3015);
            }
            else
            {
                TempData.playerEquipments.F_Pants = (Equipment)InventorySys.Instance.GetNewItemByID(3011);
            }
        }
        Demo.SetAllEquipment(TempData);
        illustration.SetGenderAge(true, false, TempData);
    }
    public void ClkShoesUp()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        if (Shoes.text == "基本鞋子1")
        {
            Shoes.text = "基本鞋子2";
            if (TempData.Gender == 0)
            {
                TempData.playerEquipments.F_Shoes = (Equipment)InventorySys.Instance.GetNewItemByID(3022);
            }
            else
            {
                TempData.playerEquipments.F_Shoes = (Equipment)InventorySys.Instance.GetNewItemByID(3018);
            }
        }
        else if (Shoes.text == "基本鞋子2")
        {
            Shoes.text = "基本鞋子3";
            if (TempData.Gender == 0)
            {
                TempData.playerEquipments.F_Shoes = (Equipment)InventorySys.Instance.GetNewItemByID(3023);
            }
            else
            {
                TempData.playerEquipments.F_Shoes = (Equipment)InventorySys.Instance.GetNewItemByID(3019);
            }
        }
        else if (Shoes.text == "基本鞋子3")
        {
            Shoes.text = "基本鞋子4";
            if (TempData.Gender == 0)
            {
                TempData.playerEquipments.F_Shoes = (Equipment)InventorySys.Instance.GetNewItemByID(3024);
            }
            else
            {
                TempData.playerEquipments.F_Shoes = (Equipment)InventorySys.Instance.GetNewItemByID(3020);
            }
        }
        else if (Shoes.text == "基本鞋子4")
        {
            Shoes.text = "基本鞋子1";
            if (TempData.Gender == 0)
            {
                TempData.playerEquipments.F_Shoes = (Equipment)InventorySys.Instance.GetNewItemByID(3021);
            }
            else
            {
                TempData.playerEquipments.F_Shoes = (Equipment)InventorySys.Instance.GetNewItemByID(3017);
            }
        }
        Demo.SetAllEquipment(TempData);
        illustration.SetGenderAge(true, false, TempData);
    }
    public void ClkShoesDown()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        if (Shoes.text == "基本鞋子1")
        {
            Shoes.text = "基本鞋子4";
            if (TempData.Gender == 0)
            {
                TempData.playerEquipments.F_Shoes = (Equipment)InventorySys.Instance.GetNewItemByID(3024);
            }
            else
            {
                TempData.playerEquipments.F_Shoes = (Equipment)InventorySys.Instance.GetNewItemByID(3020);
            }
        }
        else if (Shoes.text == "基本鞋子2")
        {
            Shoes.text = "基本鞋子1";
            if (TempData.Gender == 0)
            {
                TempData.playerEquipments.F_Shoes = (Equipment)InventorySys.Instance.GetNewItemByID(3021);
            }
            else
            {
                TempData.playerEquipments.F_Shoes = (Equipment)InventorySys.Instance.GetNewItemByID(3017);
            }
        }
        else if (Shoes.text == "基本鞋子3")
        {
            Shoes.text = "基本鞋子2";
            if (TempData.Gender == 0)
            {
                TempData.playerEquipments.F_Shoes = (Equipment)InventorySys.Instance.GetNewItemByID(3022);
            }
            else
            {
                TempData.playerEquipments.F_Shoes = (Equipment)InventorySys.Instance.GetNewItemByID(3018);
            }
        }
        else if (Shoes.text == "基本鞋子4")
        {
            Shoes.text = "基本鞋子3";
            if (TempData.Gender == 0)
            {
                TempData.playerEquipments.F_Shoes = (Equipment)InventorySys.Instance.GetNewItemByID(3023);
            }
            else
            {
                TempData.playerEquipments.F_Shoes = (Equipment)InventorySys.Instance.GetNewItemByID(3019);
            }
        }
        Demo.SetAllEquipment(TempData);
        illustration.SetGenderAge(true, false, TempData);
    }
    public void ResetOutlook()
    {
        Upwear.text = "基本上衣1";
        Downwear.text = "基本下衣1";
        Shoes.text = "基本鞋子1";
        Gender.text = "女生";
        TempData = new Player
        {
            Name = "",
            Gender = 0,
            Job = 0,
            Level = 0,
            IsNew = true,
            MapID = 0,
            Grade = 1,
            RestPoint = 0,

            playerEquipments = new PlayerEquipments
            {
                F_Chest = (Equipment)InventorySys.Instance.GetNewItemByID(3005),
                F_Pants = (Equipment)InventorySys.Instance.GetNewItemByID(3013),
                F_Shoes = (Equipment)InventorySys.Instance.GetNewItemByID(3021)
            },
        };
        Demo.SetAllEquipment(TempData);
        illustration.SetGenderAge(true, false, TempData);
    }
    #endregion
}