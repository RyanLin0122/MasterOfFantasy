﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PEProtocal;

public class DialogueWnd : WindowRoot
{
    public Image NpcImage;
    public Text NpcName;
    public Text NpcDialogue;
    public GameObject NPCFunctionBtns;
    public GameObject QuestBtns;
    public int CurrentNPCID;
    protected override void InitWnd()
    {
        QuestBtn.gameObject.SetActive(false);
        ClassBtn.gameObject.SetActive(false);
        Class2Btn.gameObject.SetActive(false);
        QuizBtn.gameObject.SetActive(false);
        BuyBtn.gameObject.SetActive(false);
        SellBtn.gameObject.SetActive(false);
        LockerBtn.gameObject.SetActive(false);
        MailBoxBtn.gameObject.SetActive(false);
        StrenthenBtn.gameObject.SetActive(false);
        QuestBtns.SetActive(false);
        NPCFunctionBtns.SetActive(true);
        AcceptBtn.gameObject.SetActive(false);
        DeclineBtn.gameObject.SetActive(false);
        UIDeliveryIntro.gameObject.SetActive(false);
        UIKillIntro.gameObject.SetActive(false);
        UIItemIntro.gameObject.SetActive(false);
        CloseBtn.gameObject.SetActive(false);
        base.InitWnd();

    }
    public void SetWndState(bool isActive, int NpcID)
    {
        if (gameObject.activeSelf != isActive)
        {
            SetActive(gameObject, isActive);
        }
        if (isActive)
        {
            InitWnd();
            SetNPC(NpcID);
        }
        else
        {
            ClearWnd();
        }
    }
    public void LockAllBtn()
    {
        foreach (var item in NPCFunctionBtns.GetComponentsInChildren<Button>())
        {
            item.enabled = false;
        }
    }
    public void ActivateAllBtn()
    {
        foreach (var item in NPCFunctionBtns.GetComponentsInChildren<Button>())
        {
            item.enabled = true;
        }
    }
    public Button QuestBtn;
    public Button ClassBtn;
    public Button Class2Btn;
    public Button QuizBtn;
    public Button BuyBtn;
    public Button SellBtn;
    public Button LockerBtn;
    public Button MailBoxBtn;
    public Button StrenthenBtn;
    public Button MiniGameBtn;


    public void SetNPC(int NpcId)
    {
        NpcConfig npcCfg = resSvc.GetNpcCfgData(NpcId);
        CurrentNPCID = NpcId;
        NpcName.text = npcCfg.Name;
        NpcDialogue.text = npcCfg.FixedText[0];
        NpcImage.sprite = null;
        SetSprite(NpcImage, "NPC/" + npcCfg.Sprite);
        NpcImage.SetNativeSize();
        //調位置
        //PECommon.Log((18 + (NpcImage.rectTransform.rect.width * 0.15f)).ToString());
        NpcImage.transform.localPosition = new Vector2(-400 + (NpcImage.rectTransform.rect.width * 0.15f), NpcImage.transform.localPosition.y);

        foreach (var item in npcCfg.Functions)
        {
            //根據NPC擁有的功能加載不同按紐
            //任務1 , 學習課程2 , 學習課程3 , 考試4 , 購買5 , 販賣6 , 倉庫7, 郵箱8, 強化裝備9, 小遊戲設定10, 製作裝備11
            switch (item)
            {
                case 1:
                    QuestBtn.gameObject.SetActive(true);
                    break;
                case 2:
                    ClassBtn.gameObject.SetActive(true);
                    break;
                case 3:
                    Class2Btn.gameObject.SetActive(true);

                    break;
                case 4:
                    QuizBtn.gameObject.SetActive(true);
                    break;
                case 5:
                    BuyBtn.gameObject.SetActive(true);
                    break;
                case 6:
                    SellBtn.gameObject.SetActive(true);

                    break;
                case 7:
                    LockerBtn.gameObject.SetActive(true);
                    break;
                case 8:
                    MailBoxBtn.gameObject.SetActive(true);
                    break;
                case 9:
                    StrenthenBtn.gameObject.SetActive(true);
                    break;
                case 10:
                    MiniGameBtn.gameObject.SetActive(true);
                    break;
            }
        }
    }
    public void ImportNpcShopItems()
    {
        ShopWnd.Instance.GetSellItemList(CurrentNPCID);
    }
    public void EnterMiniGame()
    {
        SetWndState(false);
        Debug.Log("Minigame request!");
        switch (CurrentNPCID)
        {
            case 1004:
                GameRoot.Instance.gameObject.GetComponent<GotoMiniGame>().game = GotoMiniGame.MiniGameEnum.card;
                break;
            case 1005:
                GameRoot.Instance.gameObject.GetComponent<GotoMiniGame>().game = GotoMiniGame.MiniGameEnum.archery;
                break;
            case 1006:
                GameRoot.Instance.gameObject.GetComponent<GotoMiniGame>().game = GotoMiniGame.MiniGameEnum.puzzle;
                break;
            case 1007:
                GameRoot.Instance.gameObject.GetComponent<GotoMiniGame>().game = GotoMiniGame.MiniGameEnum.kungfu;
                break;
        }
        GameRoot.Instance.gameObject.GetComponent<GotoMiniGame>().GoToMiniGame();
    }
    public void EnterMiniGame2()
    {
        SetWndState(false);
        Debug.Log("Minigame request!");
        switch (CurrentNPCID)
        {
            case 1004:
                GameRoot.Instance.gameObject.GetComponent<GotoMiniGame>().game = GotoMiniGame.MiniGameEnum.cure;
                break;
            case 1005:
                GameRoot.Instance.gameObject.GetComponent<GotoMiniGame>().game = GotoMiniGame.MiniGameEnum.shuriken;
                break;
            case 1006:
                GameRoot.Instance.gameObject.GetComponent<GotoMiniGame>().game = GotoMiniGame.MiniGameEnum.shoot;
                break;
            case 1007:
                GameRoot.Instance.gameObject.GetComponent<GotoMiniGame>().game = GotoMiniGame.MiniGameEnum.dummy;
                break;
        }
        GameRoot.Instance.gameObject.GetComponent<GotoMiniGame>().GoToMiniGame();
    }

    #region 任務相關
    public Button LastPageBtn;
    public Button NextPageBtn;
    public Button AcceptBtn;
    public Button DeclineBtn;
    public Button CloseBtn;
    public Text PageText;
    List<string> CurrentDialogStrings;
    int CurrentPage = 0;
    int MaxPage = 0;
    Quest CurrentQuest = null;
    public NPCQuestStatus CurrentNPCQuestStatus = NPCQuestStatus.None;
    public void PressQuestBtn()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.MiddleBtn);
        Quest quest = QuestManager.Instance.OpenNPCQuest(this.CurrentNPCID);
        if (quest != null)
        {
            if (quest.Info == null) //有可接新任務
            {
                SetNewQuest(quest);
                return;
            }
            if (quest.Info != null && quest.Info.status == QuestStatus.Completed) //可完成
            {
                SetQuestComplete(quest);
                return;
            }
            if (quest.Info != null && quest.Info.status == QuestStatus.InProgress) //未完成
            {
                if (quest.Define.Target == QuestTarget.Delivery && quest.Define.DeliveryNPC == CurrentNPCID)
                {
                    DeliveryItem(quest);
                }
                if (quest.Define.AcceptNPC == CurrentNPCID)
                {
                    SetUnfinish(quest);
                }
                return;
            }
        }
        else //沒任務
        {
            NpcDialogue.text = "現在可以進行的任務喔，請努力提高等級後再來";
        }
    }
    public void PressLastPage()
    {
        if ((CurrentPage - 1) >= 0)
        {
            AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
            CurrentPage--;
            NextPageBtn.interactable = true;
            PageText.text = (CurrentPage + 1) + " / " + MaxPage;
            NpcDialogue.text = CurrentDialogStrings[CurrentPage];
            AcceptBtn.gameObject.SetActive(false);
            DeclineBtn.gameObject.SetActive(false);
            if (CurrentPage == 0)
            {
                LastPageBtn.interactable = false;
            }
        }
    }
    public void PressNextPage()
    {
        if ((CurrentPage + 1) < this.MaxPage)
        {
            CurrentPage++;
            AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
            LastPageBtn.interactable = true;
            PageText.text = (CurrentPage + 1) + " / " + MaxPage;
            NpcDialogue.text = CurrentDialogStrings[CurrentPage];
            if (CurrentPage == this.MaxPage - 1)
            {
                if (CurrentNPCQuestStatus == NPCQuestStatus.Available)
                {
                    AcceptBtn.gameObject.SetActive(true);
                    DeclineBtn.gameObject.SetActive(true);
                }
                else
                {
                    CloseBtn.gameObject.SetActive(true);
                }
                NextPageBtn.interactable = false;
            }
        }

    }
    public void PressAcceptBtn()
    {
        UISystem.Instance.CloseDialogueWnd();
        new AcceptQuestSender(this.CurrentQuest.Define.ID);
    }
    public void PressDeclineBtn()
    {
        UISystem.Instance.CloseDialogueWnd();
    }

    public void PressCloseBtn()
    {
        if (CurrentQuest != null && CurrentQuest.Info != null && CurrentQuest.Info.HasDeliveried == false && CurrentQuest.Define.Target == QuestTarget.Delivery && CurrentQuest.Define.DeliveryNPC == CurrentNPCID)
        {
            new SubmitQuestSender(CurrentQuest.Info.quest_id);
        }
        else if (CurrentQuest != null && CurrentQuest.Info != null && CurrentQuest.Info.status == QuestStatus.Completed)
        {
            new SubmitQuestSender(CurrentQuest.Info.quest_id);
        }
        UISystem.Instance.CloseDialogueWnd();
    }


    private void SetNewQuest(Quest quest)
    {
        CurrentQuest = quest;
        LastPageBtn.interactable = false;
        MaxPage = quest.Define.DialogAccept.Count;
        CurrentPage = 0;
        NpcDialogue.text = quest.Define.DialogAccept[0];
        CurrentDialogStrings = quest.Define.DialogAccept;
        PageText.text = (CurrentPage + 1) + " / " + MaxPage;
        AcceptBtn.gameObject.SetActive(false);
        DeclineBtn.gameObject.SetActive(false);
        QuestBtns.gameObject.SetActive(true);
        NPCFunctionBtns.gameObject.SetActive(false);
        LastPageBtn.gameObject.SetActive(true);
        NextPageBtn.gameObject.SetActive(true);
        CurrentNPCQuestStatus = NPCQuestStatus.Available;
        ShowQuestIntro(quest, false);
        if (quest.Define.DialogAccept.Count == 1)
        {
            LastPageBtn.interactable = false;
            NextPageBtn.interactable = false;
            AcceptBtn.gameObject.SetActive(true);
            DeclineBtn.gameObject.SetActive(true);
        }
        else
        {
            NextPageBtn.interactable = true;
        }
    }
    private void SetUnfinish(Quest quest)
    {
        CurrentQuest = quest;
        LastPageBtn.interactable = false;
        MaxPage = quest.Define.DialogInComplete.Count;
        CurrentPage = 0;
        NpcDialogue.text = quest.Define.DialogInComplete[0];
        CurrentDialogStrings = quest.Define.DialogInComplete;
        PageText.text = (CurrentPage + 1) + " / " + MaxPage;
        AcceptBtn.gameObject.SetActive(false);
        DeclineBtn.gameObject.SetActive(false);
        QuestBtns.gameObject.SetActive(true);
        NPCFunctionBtns.gameObject.SetActive(false);
        LastPageBtn.gameObject.SetActive(true);
        NextPageBtn.gameObject.SetActive(true);
        CurrentNPCQuestStatus = NPCQuestStatus.DeliveryTarget;
        ShowQuestIntro(quest, false);
        if (quest.Define.DialogDelivery.Count == 1)
        {
            LastPageBtn.interactable = false;
            NextPageBtn.interactable = false;
            CloseBtn.gameObject.SetActive(true);
        }
        else
        {
            NextPageBtn.interactable = true;
        }
    }
    private void SetQuestComplete(Quest quest)
    {
        CurrentQuest = quest;
        LastPageBtn.interactable = false;
        MaxPage = quest.Define.DialogFinish.Count;
        CurrentPage = 0;
        NpcDialogue.text = quest.Define.DialogFinish[0];
        CurrentDialogStrings = quest.Define.DialogFinish;
        PageText.text = (CurrentPage + 1) + " / " + MaxPage;
        AcceptBtn.gameObject.SetActive(false);
        DeclineBtn.gameObject.SetActive(false);
        QuestBtns.gameObject.SetActive(true);
        NPCFunctionBtns.gameObject.SetActive(false);
        LastPageBtn.gameObject.SetActive(true);
        NextPageBtn.gameObject.SetActive(true);
        CurrentNPCQuestStatus = NPCQuestStatus.DeliveryTarget;
        ShowQuestIntro(quest, true);
        if (quest.Define.DialogDelivery.Count == 1)
        {
            LastPageBtn.interactable = false;
            NextPageBtn.interactable = false;
            AcceptBtn.gameObject.SetActive(true);
            DeclineBtn.gameObject.SetActive(true);
        }
        else
        {
            NextPageBtn.interactable = true;
        }
    }
    private void DeliveryItem(Quest quest)
    {
        CurrentQuest = quest;
        LastPageBtn.interactable = false;
        MaxPage = quest.Define.DialogDelivery.Count;
        CurrentPage = 0;
        NpcDialogue.text = quest.Define.DialogDelivery[0];
        CurrentDialogStrings = quest.Define.DialogDelivery;
        PageText.text = (CurrentPage + 1) + " / " + MaxPage;
        AcceptBtn.gameObject.SetActive(false);
        DeclineBtn.gameObject.SetActive(false);
        QuestBtns.gameObject.SetActive(true);
        NPCFunctionBtns.gameObject.SetActive(false);
        LastPageBtn.gameObject.SetActive(true);
        NextPageBtn.gameObject.SetActive(true);
        CurrentNPCQuestStatus = NPCQuestStatus.DeliveryTarget;
        ShowQuestIntro(quest, true);
        if (quest.Define.DialogDelivery.Count == 1)
        {
            LastPageBtn.interactable = false;
            NextPageBtn.interactable = false;
            AcceptBtn.gameObject.SetActive(true);
            DeclineBtn.gameObject.SetActive(true);
        }
        else
        {
            NextPageBtn.interactable = true;
        }
    }
    public UIDeliveryIntro UIDeliveryIntro;
    public UIKillIntro UIKillIntro;
    public UIItemIntro UIItemIntro;
    bool ShowQuestIntro(Quest quest, bool IsSuccess = false) //顯示彈出任務簡介
    {
        UIDeliveryIntro.gameObject.SetActive(false);
        UIKillIntro.gameObject.SetActive(false);
        UIItemIntro.gameObject.SetActive(false);
        switch (quest.Define.Target)
        {
            case QuestTarget.None:
                break;
            case QuestTarget.Kill:
                break;
            case QuestTarget.Item:
                break;
            case QuestTarget.Delivery:
                UIDeliveryIntro.gameObject.SetActive(true);
                UIDeliveryIntro.SetQuestIntro(quest, IsSuccess);
                break;
            default:
                break;
        }
        /*
        if (quest.Info == null || quest.Info.status == QuestStatus.Completed)
        {
            
            UIQuestDialog dlg = UISystem.Instance.ShowUIQuestDialog();
            dlg.SetQuest(quest);
            return true;
            //打開DLG 有可能是新接任務或是解任務，DLG要發請求給server
        }
        if (questInfos != null || quest.Info.status == QuestStatus.Completed)
        {
            if (!string.IsNullOrEmpty(quest.Define.DialogInComplete))
                MessageBox.Show(quest.Define.DialogInComplete);
        }
        */
        return true;
    }
    #endregion
}
