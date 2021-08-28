using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PEProtocal;

public class OptionWnd : WindowRoot, IMultiLanguageWnd
{
    public static OptionWnd _instance = null;

    #region Functional UI
    public GameObject GeneralPanel;
    public GameObject ImagePanel;
    public GameObject SoundPanel;
    public GameObject SocialPanel;

    public MOFOption TempData;
    public MOFOption CurrentData;
    public MOFOption DefaultData;

    public Toggle BgSoundMute;
    public Slider BgSoundSlider;

    public Toggle EmbiSoundMute;
    public Slider EmbiSoundSlider;

    public Toggle UISoundMute;
    public Slider UISoundSlider;

    public Toggle EffectSoundMute;
    public Slider EffectSoundSlider;

    public Toggle MonsterSoundMute;
    public Slider MonsterSoundSlider;

    public Button GeneralBtn;
    public Button ImageBtn;
    public Button SoundBtn;
    public Button SocialBtn;
    public Sprite BtnSprite1;
    public Sprite BtnSprite2;
    Color BrownColor;

    public Toggle PartyToggle;
    public Toggle TransactionToggle;
    public Toggle GuildToggle;
    public Toggle FriendToggle;
    public Toggle PrivateMsgToggle;
    public Toggle AlumniToggle;
    public Toggle PVPToggle;
    public Toggle BroadcastToggle;
    public Toggle ForiegnToggle;
    public Toggle MiniGameToggle;

    public Dropdown LanguageDropDown;
    public Toggle WindowingModeToggle;
    public Toggle FullWndToggle;

    public Toggle LowQualityToggle;
    public Toggle MidiumQualityToggle;
    public Toggle HighQualityToggle;
    public Toggle AntiAliasingToggle;
    public Toggle VsyncToggle;

    public Text SampleColorText;

    #endregion

    #region MultiLanguage
    public Text Option_Title_Text;
    public Text Option_GeneralBtn_Text;
    public Text Option_ImageBtn_Text;
    public Text Option_SoundBtn_Text;
    public Text Option_SocialBtn_Text;
    public Text Option_DefaultBtn_Text;
    public Text Option_ConfirmBtn_Text;
    public Text Option_CancelBtn_Text;

    public Text Option_ScreenMode_Text;
    public Text Option_WindowMode_Text;
    public Text Option_FullMode_Text;
    public Text Option_Language_Text;

    public Text Option_ImageQuality_Text;
    public Text Option_ImageLow_Text;
    public Text Option_ImageMedium_Text;
    public Text Option_ImageHigh_Text;
    public Text Option_AntiAliasing_Text;
    public Text Option_Vsync_Text;
    public Text Option_Mute1_Text;
    public Text Option_Mute2_Text;
    public Text Option_Mute3_Text;
    public Text Option_Mute4_Text;
    public Text Option_Mute5_Text;
    public Text Option_BgmSound_Text;

    public Text Option_EmbiSound_Text;
    public Text Option_UISound_Text;
    public Text Option_EffectSound_Text;
    public Text Option_MonsterSound_Text;

    public Text Option_AllowParty_Text; 
    public Text Option_AllowTransaction_Text;
    public Text Option_AllowGuild_Text;
    public Text Option_AllowBuddy_Text;
    public Text Option_AllowPrivateMsg_Text;
    public Text Option_AllowAlumni_Text;
    public Text Option_AllowPVP_Text;
    public Text Option_AllowBroadcast_Text;
    public Text Option_ForeignLanguage_Text;
    public Text Option_AllowMinigame_Text;


    public void SetLanguage()
    {
        Dictionary<string, string> Dic = null;
        switch (GameRoot.Instance.AccountOption.Language)
        {
            case 0:
                Dic = ResSvc.Instance.Tra_ChineseStrings;
                break;
            case 1:
                Dic = ResSvc.Instance.Sim_ChineseStrings;
                break;
            case 2:
                Dic = ResSvc.Instance.EnglishStrings;
                break;
            case 3:
                Dic = ResSvc.Instance.KoreanStrings;
                break;
            default:
                Dic = ResSvc.Instance.Tra_ChineseStrings;
                break;
        }
        Option_Title_Text.text = Dic["Option_Title_Text"];
        Option_GeneralBtn_Text.text = Dic["Option_GeneralBtn_Text"];
        Option_ImageBtn_Text.text = Dic["Option_ImageBtn_Text"];
        Option_SoundBtn_Text.text = Dic["Option_SoundBtn_Text"];
        Option_SocialBtn_Text.text = Dic["Option_SocialBtn_Text"];
        Option_DefaultBtn_Text.text = Dic["Option_DefaultBtn_Text"];
        Option_ConfirmBtn_Text.text = Dic["Option_ConfirmBtn_Text"];
        Option_CancelBtn_Text.text = Dic["Option_CancelBtn_Text"];
        Option_ScreenMode_Text.text = Dic["Option_ScreenMode_Text"];
        Option_WindowMode_Text.text = Dic["Option_WindowMode_Text"];
        Option_FullMode_Text.text = Dic["Option_FullMode_Text"];
        Option_Language_Text.text = Dic["Option_Language_Text"];
        Option_ImageQuality_Text.text = Dic["Option_ImageQuality_Text"];
        Option_ImageLow_Text.text = Dic["Option_ImageLow_Text"];
        Option_ImageMedium_Text.text = Dic["Option_ImageMedium_Text"];
        Option_ImageHigh_Text.text = Dic["Option_ImageHigh_Text"];
        Option_AntiAliasing_Text.text = Dic["Option_AntiAliasing_Text"];
        Option_Vsync_Text.text = Dic["Option_Vsync_Text"];
        Option_Mute1_Text.text = Dic["Option_Mute_Text"];
        Option_Mute2_Text.text = Dic["Option_Mute_Text"];
        Option_Mute3_Text.text = Dic["Option_Mute_Text"];
        Option_Mute4_Text.text = Dic["Option_Mute_Text"];
        Option_Mute5_Text.text = Dic["Option_Mute_Text"];
        Option_BgmSound_Text.text = Dic["Option_BgmSound_Text"];
        Option_EmbiSound_Text.text = Dic["Option_EmbiSound_Text"];
        Option_UISound_Text.text = Dic["Option_UISound_Text"];
        Option_EffectSound_Text.text = Dic["Option_EffectSound_Text"];
        Option_MonsterSound_Text.text = Dic["Option_MonsterSound_Text"];
        Option_AllowParty_Text.text = Dic["Option_AllowParty_Text"];
        Option_AllowTransaction_Text.text = Dic["Option_AllowTransaction_Text"];
        Option_AllowGuild_Text.text = Dic["Option_AllowGuild_Text"];
        Option_AllowBuddy_Text.text = Dic["Option_AllowBuddy_Text"];
        Option_AllowPrivateMsg_Text.text = Dic["Option_AllowPrivateMsg_Text"];
        Option_AllowAlumni_Text.text = Dic["Option_AllowAlumni_Text"];
        Option_AllowPVP_Text.text = Dic["Option_AllowPVP_Text"];
        Option_AllowBroadcast_Text.text = Dic["Option_AllowBroadcast_Text"];
        Option_ForeignLanguage_Text.text = Dic["Option_ForeignLanguage_Text"];
        Option_AllowMinigame_Text.text = Dic["Option_AllowMinigame_Text"];
    }
    #endregion
    public static OptionWnd Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = MainCitySys.Instance.optionWnd;
            }
            return _instance;
        }
    }

    private void Start()
    {
        BrownColor = SampleColorText.color;
        PressGeneral();
    }
    protected override void InitWnd()
    {
        if (GameRoot.Instance.AccountOption != null)
        {
            SetupOptionUI(GameRoot.Instance.AccountOption);
        }
        else
        {
            GameRoot.Instance.AccountOption = GenerateDefaultOption();
            SetupOptionUI(GameRoot.Instance.AccountOption);
            
        }
        GeneralPanel.SetActive(true);
        ImagePanel.SetActive(false);
        SoundPanel.SetActive(false);
        SocialPanel.SetActive(false);
        GeneralBtn.GetComponent<Image>().sprite = BtnSprite1;
        ImageBtn.GetComponent<Image>().sprite = BtnSprite2;
        SoundBtn.GetComponent<Image>().sprite = BtnSprite2;
        SocialBtn.GetComponent<Image>().sprite = BtnSprite2;
        Dictionary<string, string> Dic = null;
        switch (GameRoot.Instance.AccountOption.Language)
        {
            case 0:
                Dic = ResSvc.Instance.Tra_ChineseStrings;
                break;
            case 1:
                Dic = ResSvc.Instance.Sim_ChineseStrings;
                break;
            case 2:
                Dic = ResSvc.Instance.EnglishStrings;
                break;
            case 3:
                Dic = ResSvc.Instance.KoreanStrings;
                break;
            default:
                Dic = ResSvc.Instance.Tra_ChineseStrings;
                break;
        }
        GeneralBtn.GetComponentInChildren<Text>().text = "<color=#ffffff>"+Dic["Option_GeneralBtn_Text"] +"</color>";
        ImageBtn.GetComponentInChildren<Text>().text = Dic["Option_ImageBtn_Text"];
        SoundBtn.GetComponentInChildren<Text>().text = Dic["Option_SoundBtn_Text"];
        SocialBtn.GetComponentInChildren<Text>().text = Dic["Option_SocialBtn_Text"];
        ImageBtn.GetComponentInChildren<Text>().color = BrownColor;
        SoundBtn.GetComponentInChildren<Text>().color = BrownColor;
        SocialBtn.GetComponentInChildren<Text>().color = BrownColor;
        base.InitWnd();
    }
    public void RecordTempData()
    {
        TempData = Tools.TransReflection<MOFOption, MOFOption>(CurrentData);


    }
    public void SetData(MOFOption option)
    {
        if (option.IsBGMMute)
        {
            BgSoundMute.isOn = true;
            BgSoundSlider.value = 0f;
        }
        else
        {
            BgSoundMute.isOn = false;
            BgSoundSlider.value = option.BGMVolume;
        }
        if (option.IsEmbiMute)
        {
            EmbiSoundMute.isOn = true;
            EmbiSoundSlider.value = 0f;
        }
        else
        {
            EmbiSoundMute.isOn = false;
            EmbiSoundSlider.value = option.BGMVolume;
        }
        if (option.IsUIMute)
        {
            UISoundMute.isOn = true;
            UISoundSlider.value = 0f;
        }
        else
        {
            UISoundMute.isOn = false;
            UISoundSlider.value = option.BGMVolume;
        }
        if (option.IsEffectMute)
        {
            EffectSoundMute.isOn = true;
            EffectSoundSlider.value = 0f;
        }
        else
        {
            EffectSoundMute.isOn = false;
            EffectSoundSlider.value = option.BGMVolume;
        }
        if (option.IsMonsterMute)
        {
            MonsterSoundMute.isOn = true;
            MonsterSoundSlider.value = 0f;
        }
        else
        {
            MonsterSoundMute.isOn = false;
            MonsterSoundSlider.value = option.BGMVolume;
        }
    }

    #region UI
    public void PressGeneral()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        GeneralPanel.SetActive(true);
        ImagePanel.SetActive(false);
        SoundPanel.SetActive(false);
        SocialPanel.SetActive(false);
        GeneralBtn.GetComponent<Image>().sprite = BtnSprite1;
        ImageBtn.GetComponent<Image>().sprite = BtnSprite2;
        SoundBtn.GetComponent<Image>().sprite = BtnSprite2;
        SocialBtn.GetComponent<Image>().sprite = BtnSprite2;
        Dictionary<string, string> Dic = null;
        switch (GameRoot.Instance.AccountOption.Language)
        {
            case 0:
                Dic = ResSvc.Instance.Tra_ChineseStrings;
                break;
            case 1:
                Dic = ResSvc.Instance.Sim_ChineseStrings;
                break;
            case 2:
                Dic = ResSvc.Instance.EnglishStrings;
                break;
            case 3:
                Dic = ResSvc.Instance.KoreanStrings;
                break;
            default:
                Dic = ResSvc.Instance.Tra_ChineseStrings;
                break;
        }
        GeneralBtn.GetComponentInChildren<Text>().text = "<color=#ffffff>" + Dic["Option_GeneralBtn_Text"] + "</color>";
        ImageBtn.GetComponentInChildren<Text>().text = Dic["Option_ImageBtn_Text"];
        SoundBtn.GetComponentInChildren<Text>().text = Dic["Option_SoundBtn_Text"];
        SocialBtn.GetComponentInChildren<Text>().text = Dic["Option_SocialBtn_Text"];
        ImageBtn.GetComponentInChildren<Text>().color = BrownColor;
        SoundBtn.GetComponentInChildren<Text>().color = BrownColor;
        SocialBtn.GetComponentInChildren<Text>().color = BrownColor;
    }
    public void PressImage()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        GeneralPanel.SetActive(false);
        ImagePanel.SetActive(true);
        SoundPanel.SetActive(false);
        SocialPanel.SetActive(false);
        GeneralBtn.GetComponent<Image>().sprite = BtnSprite2;
        ImageBtn.GetComponent<Image>().sprite = BtnSprite1;
        SoundBtn.GetComponent<Image>().sprite = BtnSprite2;
        SocialBtn.GetComponent<Image>().sprite = BtnSprite2;
        Dictionary<string, string> Dic = null;
        switch (GameRoot.Instance.AccountOption.Language)
        {
            case 0:
                Dic = ResSvc.Instance.Tra_ChineseStrings;
                break;
            case 1:
                Dic = ResSvc.Instance.Sim_ChineseStrings;
                break;
            case 2:
                Dic = ResSvc.Instance.EnglishStrings;
                break;
            case 3:
                Dic = ResSvc.Instance.KoreanStrings;
                break;
            default:
                Dic = ResSvc.Instance.Tra_ChineseStrings;
                break;
        }
        GeneralBtn.GetComponentInChildren<Text>().text = Dic["Option_GeneralBtn_Text"];
        ImageBtn.GetComponentInChildren<Text>().text = "<color=#ffffff>"+Dic["Option_ImageBtn_Text"]+"</color>";
        SoundBtn.GetComponentInChildren<Text>().text = Dic["Option_SoundBtn_Text"];
        SocialBtn.GetComponentInChildren<Text>().text = Dic["Option_SocialBtn_Text"];
        GeneralBtn.GetComponentInChildren<Text>().color = BrownColor;
        SoundBtn.GetComponentInChildren<Text>().color = BrownColor;
        SocialBtn.GetComponentInChildren<Text>().color = BrownColor;
    }
    public void PressSound()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        GeneralPanel.SetActive(false);
        ImagePanel.SetActive(false);
        SoundPanel.SetActive(true);
        SocialPanel.SetActive(false);
        GeneralBtn.GetComponent<Image>().sprite = BtnSprite2;
        ImageBtn.GetComponent<Image>().sprite = BtnSprite2;
        SoundBtn.GetComponent<Image>().sprite = BtnSprite1;
        SocialBtn.GetComponent<Image>().sprite = BtnSprite2;
        Dictionary<string, string> Dic = null;
        switch (GameRoot.Instance.AccountOption.Language)
        {
            case 0:
                Dic = ResSvc.Instance.Tra_ChineseStrings;
                break;
            case 1:
                Dic = ResSvc.Instance.Sim_ChineseStrings;
                break;
            case 2:
                Dic = ResSvc.Instance.EnglishStrings;
                break;
            case 3:
                Dic = ResSvc.Instance.KoreanStrings;
                break;
            default:
                Dic = ResSvc.Instance.Tra_ChineseStrings;
                break;
        }
        GeneralBtn.GetComponentInChildren<Text>().text = Dic["Option_GeneralBtn_Text"];
        ImageBtn.GetComponentInChildren<Text>().text = Dic["Option_ImageBtn_Text"];
        SoundBtn.GetComponentInChildren<Text>().text = "<color=#ffffff>"+ Dic["Option_SoundBtn_Text"] + "</color>";
        SocialBtn.GetComponentInChildren<Text>().text = Dic["Option_SocialBtn_Text"];
        ImageBtn.GetComponentInChildren<Text>().color = BrownColor;
        GeneralBtn.GetComponentInChildren<Text>().color = BrownColor;
        SocialBtn.GetComponentInChildren<Text>().color = BrownColor;
    }
    public void PressSocial()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        GeneralPanel.SetActive(false);
        ImagePanel.SetActive(false);
        SoundPanel.SetActive(false);
        SocialPanel.SetActive(true);
        GeneralBtn.GetComponent<Image>().sprite = BtnSprite2;
        ImageBtn.GetComponent<Image>().sprite = BtnSprite2;
        SoundBtn.GetComponent<Image>().sprite = BtnSprite2;
        SocialBtn.GetComponent<Image>().sprite = BtnSprite1;
        Dictionary<string, string> Dic = null;
        switch (GameRoot.Instance.AccountOption.Language)
        {
            case 0:
                Dic = ResSvc.Instance.Tra_ChineseStrings;
                break;
            case 1:
                Dic = ResSvc.Instance.Sim_ChineseStrings;
                break;
            case 2:
                Dic = ResSvc.Instance.EnglishStrings;
                break;
            case 3:
                Dic = ResSvc.Instance.KoreanStrings;
                break;
            default:
                Dic = ResSvc.Instance.Tra_ChineseStrings;
                break;
        }
        GeneralBtn.GetComponentInChildren<Text>().text = Dic["Option_GeneralBtn_Text"];
        ImageBtn.GetComponentInChildren<Text>().text = Dic["Option_ImageBtn_Text"];
        SoundBtn.GetComponentInChildren<Text>().text = Dic["Option_SoundBtn_Text"];
        SocialBtn.GetComponentInChildren<Text>().text = "<color=#ffffff>"+ Dic["Option_SocialBtn_Text"] + "</color>";
        ImageBtn.GetComponentInChildren<Text>().color = BrownColor;
        SoundBtn.GetComponentInChildren<Text>().color = BrownColor;
        GeneralBtn.GetComponentInChildren<Text>().color = BrownColor;
    }
    public void PressCancel()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.WindowClose);
        SetWndState(false);
    }
    public void PressConfirm()
    {
        if (FullWndToggle.isOn)
        {
            Screen.fullScreen = true;
        }
        else if (WindowingModeToggle.isOn)
        {
            Screen.fullScreen = false;
        }
        TempData = GenerateTempData();
        if (GameRoot.Instance.AccountOption.Language != TempData.Language)
        {
            ChangeLanguage();
        }       
        GameRoot.Instance.AccountOption = TempData;
        AudioSvc.Instance.PlayUIAudio(Constants.WindowClose);
        SetWndState(false);
    }
    private MOFOption GenerateTempData()
    {
        MOFOption opt = new MOFOption();
        opt.IsBGMMute = BgSoundMute.isOn;
        opt.IsEmbiMute = EmbiSoundMute.isOn;
        opt.IsUIMute = UISoundMute.isOn;
        opt.IsEffectMute = EffectSoundMute.isOn;
        opt.IsMonsterMute = MonsterSoundMute.isOn;
        opt.BGMVolume = BgSoundSlider.value;
        opt.EmbiVolume = EmbiSoundSlider.value;
        opt.UIVolume = UISoundSlider.value;
        opt.EffectVolume = EffectSoundSlider.value;
        opt.MonsterVolume = MonsterSoundSlider.value;
        opt.IsParty = PartyToggle.isOn;
        opt.IsTransaction = TransactionToggle.isOn;
        opt.IsGuild = GuildToggle.isOn;
        opt.IsFriend = FriendToggle.isOn;
        opt.IsPrivateMsg = PrivateMsgToggle.isOn;
        opt.IsAlumni = AlumniToggle.isOn;
        opt.IsPVP = PVPToggle.isOn;
        opt.IsBroadcast = BroadcastToggle.isOn;
        opt.IsForiegn = ForiegnToggle.isOn;
        opt.IsMiniGame = MiniGameToggle.isOn;
        if (LowQualityToggle.isOn)
        {
            opt.ImageQuality = 0;
        }
        else if (MidiumQualityToggle.isOn)
        {
            opt.ImageQuality = 1;
        }
        else if (HighQualityToggle.isOn) 
        {
            opt.ImageQuality = 2;
        } 
        opt.IsAntiAliasing = AntiAliasingToggle.isOn;
        opt.IsVsync = VsyncToggle.isOn;
        opt.Language = LanguageDropDown.value;
        return opt;
    }
    #endregion


    #region Logic Function
    //0.打開先根據currentoption設定
    //1.調東西，只有聲音會立刻調用 全螢幕也是(不儲存)

    public void SettingWhenOpen()
    {
        if (GameRoot.Instance.AccountOption == null)
        {
            GameRoot.Instance.AccountOption = GenerateDefaultOption();
        }

    }
    public void ChangeLanguage() //很可怕!!
    {
        this.SetLanguage();
    }
    public void SetupOptionUI(MOFOption opt)
    {
        BgSoundMute.isOn = opt.IsBGMMute;
        BgSoundSlider.value = opt.BGMVolume;

        EmbiSoundMute.isOn = opt.IsEmbiMute;
        EmbiSoundSlider.value = opt.EmbiVolume;

        UISoundMute.isOn = opt.IsUIMute;
        UISoundSlider.value = opt.UIVolume;

        EffectSoundMute.isOn = opt.IsEffectMute;
        EffectSoundSlider.value = opt.EffectVolume;

        MonsterSoundMute.isOn = opt.IsMonsterMute;
        MonsterSoundSlider.value = opt.MonsterVolume;

        PartyToggle.isOn = opt.IsParty;
        TransactionToggle.isOn = opt.IsTransaction;
        GuildToggle.isOn = opt.IsGuild;
        FriendToggle.isOn = opt.IsFriend;
        PrivateMsgToggle.isOn = opt.IsPrivateMsg;
        AlumniToggle.isOn = opt.IsAlumni;
        PVPToggle.isOn = opt.IsPVP;
        BroadcastToggle.isOn = opt.IsBroadcast;
        ForiegnToggle.isOn = opt.IsForiegn;
        MiniGameToggle.isOn = opt.IsMiniGame;

        LanguageDropDown.value = opt.Language;
        WindowingModeToggle.isOn = !Screen.fullScreen;
        FullWndToggle.isOn = Screen.fullScreen;

        switch (opt.ImageQuality)
        {
            case 0:
                LowQualityToggle.isOn = true;
                MidiumQualityToggle.isOn = false;
                HighQualityToggle.isOn = false;
                break;
            case 1:
                LowQualityToggle.isOn = false;
                MidiumQualityToggle.isOn = true;
                HighQualityToggle.isOn = false;
                break;
            case 2:
                LowQualityToggle.isOn = false;
                MidiumQualityToggle.isOn = false;
                HighQualityToggle.isOn = true;
                break;
            default:
                break;
        }
        
        AntiAliasingToggle.isOn = true;
        VsyncToggle.isOn = false;
    }
    
    public MOFOption GenerateDefaultOption()
    {
        return new MOFOption
        {
            IsBGMMute = false,
            IsEmbiMute = false,
            IsUIMute = false,
            IsEffectMute = false,
            IsMonsterMute = false,
            BGMVolume = 0.5f,
            EmbiVolume = 0.5f,
            UIVolume = 0.5f,
            EffectVolume = 0.5f,
            MonsterVolume = 0.5f,
            IsParty = true,
            IsTransaction = true,
            IsGuild = true,
            IsFriend = true,
            IsPrivateMsg = true,
            IsAlumni = true,
            IsPVP = true,
            IsBroadcast = true,
            IsForiegn = true,
            IsMiniGame = true,
            ImageQuality = 2,
            IsAntiAliasing = true,
            IsVsync = false,
            Language = 0
        };
    }
    public void SetBGMMute()
    {
        if (BgSoundMute.isOn)
        {
            BgSoundSlider.value = 0;
            AudioSvc.Instance.BgAudio.volume = 0;
        }
        else
        {
            BgSoundSlider.value = 0.5f;
            AudioSvc.Instance.BgAudio.volume = 0.5f;
        }
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        UpdateSoundData();
    }
    public void SetUIMute()
    {
        if (UISoundMute.isOn)
        {
            UISoundSlider.value = 0;
            AudioSvc.Instance.UiAudio.volume = 0;
            AudioSvc.Instance.MiniGameUIAudio.volume = 0;
        }
        else
        {
            UISoundSlider.value = 0.5f;
            AudioSvc.Instance.UiAudio.volume = 0.5f;
            AudioSvc.Instance.MiniGameUIAudio.volume = 0.5f;
        }
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        UpdateSoundData();
    }
    public void SetEmbiMute()
    {
        if (EmbiSoundMute.isOn)
        {
            EmbiSoundSlider.value = 0;
            AudioSvc.Instance.EmbiAudio.volume = 0;
        }
        else
        {
            EmbiSoundSlider.value = 0.5f;
            AudioSvc.Instance.EmbiAudio.volume = 0.5f;
        }
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        UpdateSoundData();
    }
    public void SetEffectMute()
    {
        if (EffectSoundMute.isOn)
        {
            EffectSoundSlider.value = 0;
            AudioSvc.Instance.CharacterAudio.volume = 0;
        }
        else
        {
            EffectSoundSlider.value = 0.5f;
            AudioSvc.Instance.CharacterAudio.volume = 0.5f;
        }
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        UpdateSoundData();
    }
    public void SetMonsterMute()
    {
        if (MonsterSoundMute.isOn)
        {
            MonsterSoundSlider.value = 0;
        }
        else
        {
            MonsterSoundSlider.value = 0.5f;
        }
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        UpdateSoundData();
    }
    public void SetBGVolume()
    {
        AudioSvc.Instance.BgAudio.volume = BgSoundSlider.value;
        UpdateSoundData();
    }
    public void SetEmbiVolume()
    {
        AudioSvc.Instance.EmbiAudio.volume = EmbiSoundSlider.value;
        UpdateSoundData();
    }
    public void SetUIVolume()
    {
        AudioSvc.Instance.UiAudio.volume = UISoundSlider.value;
        AudioSvc.Instance.MiniGameUIAudio.volume = UISoundSlider.value;
        UpdateSoundData();
    }
    public void SetEffectVolume()
    {
        AudioSvc.Instance.CharacterAudio.volume = EffectSoundSlider.value;
        UpdateSoundData();
    }
    public void SetMonsterVolume()
    {
        //ToDO
        UpdateSoundData();
    }
    public void UpdateSoundData()
    {
        if (GameRoot.Instance.AccountOption == null)
        {
            GameRoot.Instance.AccountOption = GenerateDefaultOption();
        }
        GameRoot.Instance.AccountOption.BGMVolume = BgSoundSlider.value;
        GameRoot.Instance.AccountOption.EmbiVolume = EmbiSoundSlider.value;
        GameRoot.Instance.AccountOption.UIVolume = UISoundSlider.value;
        GameRoot.Instance.AccountOption.EffectVolume = EffectSoundSlider.value;
        GameRoot.Instance.AccountOption.MonsterVolume = MonsterSoundSlider.value;
    }

    
    #endregion
}


