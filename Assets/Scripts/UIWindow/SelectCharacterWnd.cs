using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;
using UnityEngine.UI;

public class SelectCharacterWnd : WindowRoot {
    public Text LevelText1;
    public Text LevelText2;
    public Text LevelText3;
    public Text JobText1;
    public Text JobText2;
    public Text JobText3;
    public Text NameText1;
    public Text NameText2;
    public Text NameText3;
    public PlayerController Demo1;
    public PlayerController Demo2;
    public PlayerController Demo3;
    public GameObject Effect1;
    public GameObject Effect2;
    public GameObject Effect3;
    public GameObject Guide;
    public GameObject IfDelete;
    public InputField inputPass;
    public Illustration illustration;
    public List<Player> data = null;
    public int ChoosedCharacter = 0;

    //初始化
    protected override void InitWnd()
    {       
        base.InitWnd();
    }

    //設定角色資料
    public void SetCharacterProperty(List<Player> players)
    {
        GameRoot.Instance.GetComponentInChildren<Canvas>().worldCamera = Camera.main;
        GameRoot.Instance.GetComponentInChildren<Canvas>().renderMode = RenderMode.WorldSpace;
        illustration.gameObject.SetActive(true);
        foreach (var item in players)
        {
            Debug.Log(item.Name);
        }
        Demo1.gameObject.SetActive(true);
        Demo2.gameObject.SetActive(true);
        Demo3.gameObject.SetActive(true);
        data = players;
        switch (players.Count)
        {
            case 0:
                SetText(LevelText1, "");
                SetText(JobText1, "");
                SetText(NameText1, "");
                SetText(LevelText2, "");
                SetText(JobText2, "");
                SetText(NameText2, "");
                SetText(LevelText3, "");
                SetText(JobText3, "");
                SetText(NameText3, "");
                illustration.gameObject.SetActive(false);
                Demo1.gameObject.SetActive(false);
                Demo2.gameObject.SetActive(false);
                Demo3.gameObject.SetActive(false);
                break;
            case 1:
                SetText(LevelText1, players[0].Level);
                SetText(JobText1, Constants.SetJobName(players[0].Job));
                SetText(NameText1, players[0].Name);
                SetText(LevelText2, "");
                SetText(JobText2, "");
                SetText(NameText2, "");
                SetText(LevelText3, "");
                SetText(JobText3, "");
                SetText(NameText3, "");
                illustration.SetGenderAge(true, false, players[0]);
                Demo1.gameObject.SetActive(true);
                Demo2.gameObject.SetActive(false);
                Demo3.gameObject.SetActive(false);
                Demo1.SetAllEquipment(players[0].playerEquipments, players[0].Gender);
                break;
            case 2:
                SetText(LevelText1, players[0].Level);
                SetText(JobText1, Constants.SetJobName(players[0].Job));
                SetText(NameText1, players[0].Name);
                SetText(LevelText2, players[1].Level);
                SetText(JobText2, Constants.SetJobName(players[1].Job));
                SetText(NameText2, players[1].Name);
                SetText(LevelText3, "");
                SetText(JobText3, "");
                SetText(NameText3, "");
                illustration.SetGenderAge(true, false, players[0]);
                Demo1.gameObject.SetActive(true);
                Demo2.gameObject.SetActive(true);
                Demo3.gameObject.SetActive(false);
                Demo1.SetAllEquipment(players[0].playerEquipments, players[0].Gender);
                Demo2.SetAllEquipment(players[1].playerEquipments, players[1].Gender);
                break;
            case 3:
                SetText(LevelText1, players[0].Level);
                SetText(JobText1, Constants.SetJobName(players[0].Job));
                SetText(NameText1, players[0].Name);
                SetText(LevelText2, players[1].Level);
                SetText(JobText2, Constants.SetJobName(players[1].Job));
                SetText(NameText2, players[1].Name);
                SetText(LevelText3, players[2].Level);
                SetText(JobText3, Constants.SetJobName(players[2].Job));
                SetText(NameText3, players[2].Name);
                illustration.SetGenderAge(true, false, players[0]);
                Demo1.gameObject.SetActive(true);
                Demo2.gameObject.SetActive(true);
                Demo3.gameObject.SetActive(true);
                Demo1.SetAllEquipment(players[0].playerEquipments, players[0].Gender);
                Demo2.SetAllEquipment(players[1].playerEquipments, players[1].Gender);
                Demo3.SetAllEquipment(players[2].playerEquipments, players[2].Gender);
                break;
        }

        

    }

    //UI 創角相關
    public void ClickCreateBtn()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        if (data.Count<3)
        {
            LoginSys.Instance.ToCreateWnd();
        }
        else
        {
            GameRoot.AddTips("角色已滿囉!!");
        }
    }

    //UI刪角相關
    public void ClickDeleteBtn()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        if (ChoosedCharacter != 0)
        {
            
            IfDelete.SetActive(true);
        }
        
    }
    public void ClkDeleteYes()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        if (inputPass.text == GameRoot.Instance.Password)
        {
            GameRoot.Instance.WindowLock();
            inputPass.text = "";
            DeleteCharacter();
            IfDelete.SetActive(false);
        }
        else
        {
            IfDelete.SetActive(false);
            GameRoot.AddTips("密碼錯誤!!");
        }
    }
    public void ClkDeleteNo()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        IfDelete.SetActive(false);
    }
    public void DeleteCharacter()
    {
        new DeleteSender(GameRoot.Instance.Account, data[ChoosedCharacter - 1].Name);
    }

    //UI 選角相關
    public void ClickChoosedBtn1()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        if (data.Count >= 1)
        {
            ChoosedCharacter = 1;
            //顯示選擇動畫
            GameRoot.Instance.AssignCurrentPlayer(data[0]);
            SelectCharacter1();
        }
        else
        {
            ChoosedCharacter = 0;
            Effect1.SetActive(false);
            Effect2.SetActive(false);
            Effect3.SetActive(false);
            Demo2.PlayIdle();
            Demo1.PlayIdle();
            Demo3.PlayIdle();
        }
    }
    public void ClickChoosedBtn2()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        if (data.Count >= 2)
        {
            ChoosedCharacter = 2;
            //顯示選擇動畫
            GameRoot.Instance.AssignCurrentPlayer(data[1]);
            SelectCharacter2();
        }
        else
        {
            ChoosedCharacter = 0;
            Effect1.SetActive(false);
            Effect2.SetActive(false);
            Effect3.SetActive(false);
            Demo1.PlayIdle();
            Demo2.PlayIdle();
            Demo3.PlayIdle();
        }
    }
    public void ClickChoosedBtn3()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        if (data.Count >= 3)
        {
            ChoosedCharacter = 3;
            //顯示選擇動畫
            GameRoot.Instance.AssignCurrentPlayer(data[2]);
            SelectCharacter3();
        }
        else
        {
            ChoosedCharacter = 0;
            Effect1.SetActive(false);
            Effect2.SetActive(false);
            Effect3.SetActive(false);
            Demo1.PlayIdle();
            Demo2.PlayIdle();
            Demo3.PlayIdle();
        }
    }
    public void SelectCharacter1()
    {

        Effect1.SetActive(true);
        Effect2.SetActive(false);
        Effect3.SetActive(false);
        illustration.SetGenderAge(true, false, data[0]);
        Demo1.PlayWalk();
        Demo2.PlayIdle();
        Demo3.PlayIdle();
    }
    public void SelectCharacter2()
    {
        Effect1.SetActive(false);
        Effect2.SetActive(true);
        Effect3.SetActive(false);
        illustration.SetGenderAge(true, false, data[1]);
        Demo2.PlayWalk();
        Demo1.PlayIdle();
        Demo3.PlayIdle();
    }
    public void SelectCharacter3()
    {
        Effect1.SetActive(false);
        Effect2.SetActive(false);
        Effect3.SetActive(true);
        illustration.SetGenderAge(true, false, data[2]);
        Demo3.PlayWalk();
        Demo1.PlayIdle();
        Demo2.PlayIdle();
    }

    //UI 介面相關
    public void ClickCloseBtn()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        Guide.SetActive(false);
    }
    public void ClickStartBtn()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.LargeBtn);
        if (ChoosedCharacter == 0)
        {
            GameRoot.AddTips("記得選擇角色喔!!");
        }
        else
        {
            if (GameRoot.Instance.ActivePlayer.IsNew == true)
            {
                Guide.SetActive(true);
            }
            else
            {
                LoginSys.Instance.EnterGame(false,false);
            }
        }
    }
    public void ClickBackBtn()
    {
        Reset();
        Effect1.SetActive(false);
        Effect2.SetActive(false);
        Effect3.SetActive(false);
        Demo1.PlayIdle();
        Demo2.PlayIdle();
        Demo3.PlayIdle();
        LoginSys.Instance.BackToServerWnd();
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);       
    }   
    public void ClickExitBtn()
    {
        LoginSys.Instance.LogoutRequest();
    }
    public void Reset()
    {
        Effect1.SetActive(false);
        Effect2.SetActive(false);
        Effect3.SetActive(false);
        SetText(LevelText1, "");
        SetText(JobText1, "");
        SetText(NameText1, "");
        SetText(LevelText2, "");
        SetText(JobText2, "");
        SetText(NameText2, "");
        SetText(LevelText3, "");
        SetText(JobText3, "");
        SetText(NameText3, "");
        illustration.gameObject.SetActive(false);
        Demo1.gameObject.SetActive(false);
        Demo2.gameObject.SetActive(false);
        Demo3.gameObject.SetActive(false);
        ChoosedCharacter = 0;
        
    }

    //UI 新手教學相關
    public void ClickGuideYes()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        LoginSys.Instance.EnterGame(true, true);
    }
    public void ClickGuideNo()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        LoginSys.Instance.EnterGame(false,true);
    }

    
}
