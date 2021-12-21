using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PEProtocal;

public class UISystem : SystemRoot
{
    public static UISystem Instance = null;

    public override void InitSys()
    {
        Instance = this;
        Knapsack.InitKnapsack();
        lockerWnd.InitLocker();
        MailBoxWnd.InitMailBox();
        UICalender.Init();
        base.InitSys();
    }

    public bool IsChatWndCanOpen = true;
    public Information InfoWnd;
    public KnapsackWnd Knapsack;
    public EquipmentWnd equipmentWnd;
    public LockerWnd lockerWnd;
    public MailBoxWnd MailBoxWnd;
    public ShopWnd shopWnd;
    public BaseUI baseUI;
    public ChatWnd chatWnd;
    public DialogueWnd dialogueWnd;
    public MenuUI menuUI;
    public Button OpenChatBtn;
    public Sprite OpenChat;
    public Sprite CloseChat;
    public LearnSkillWnd learnSkillWnd;
    public MiniMap miniMap;
    public GuideWnd guideWnd;
    public MinigameSettingWnd miniGameSettingWnd;
    public OptionWnd optionWnd;
    public DiaryWnd diaryWnd;
    public CashShopWnd cashShopWnd;
    public StrengthenWnd strengthenWnd;
    public TransationWnd transationWnd;
    public MGFWnd mGFWnd;
    public CommunityWnd communityWnd;
    public PetWnd petWnd;
    public MessageQueue messageQueue;
    public PlayerOption playerOption;
    public OtherPlayerOption otherPlayerOption;
    public Transform BuffIconsContainer;
    public QuestWnd QuestWnd;
    public UICalender UICalender;
    public LearnMajorSkillWnd LearnMajorSkillWnd;
    public ServerAnouncement serverAnouncement;
    public OtherProfileWnd OtherProfileWnd;
    public ShipWnd shipWnd;
    public DeathWnd deathWnd;
    public Transform WindowsContainer;
    private readonly object stackLock = new object();
    public Stack<IStackWnd> stack = new Stack<IStackWnd>();

    public void PutLastLayer(Transform t)
    {
        t.SetParent(WindowsContainer);
        t.SetAsLastSibling();
    }
    public void Push(IStackWnd wnd)
    {
        lock (stackLock)
        {
            stack.Push(wnd);
        }

    }
    public void Pop()
    {
        lock (stackLock)
        {
            if (stack.Count > 0)
            {
                stack.Pop().CloseAndPop();
            }
        }
    }
    public void ForcePop(IStackWnd wnd)
    {
        lock (stackLock)
        {
            Stack<IStackWnd> tmp = new Stack<IStackWnd>();
            while (stack.Count > 0)
            {
                if (stack.Peek() == wnd)
                {
                    stack.Pop();
                }
                else
                {
                    tmp.Push(stack.Pop());
                }
            }
            while (tmp.Count > 0)
            {
                stack.Push(tmp.Pop());
            }
        }
    }
    public void PressEsc()
    {
        Pop();
    }

    public void AddMessageQueue(string s)
    {
        messageQueue.AddMessage(s);
    }
    #region 開關Wnd
    public void OpenInfoWnd()
    {
        CloseEquipWnd2();
        AudioSvc.Instance.PlayUIAudio(Constants.WindowOpen);
        InfoWnd.SetWndState(true);
        PutLastLayer(InfoWnd.transform);
    }
    public void CloseInfo2()
    {
        InfoWnd.SetWndState(false);
        InfoWnd.IsOpen = false;
    }
    public void CloseInfoWnd()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.WindowClose);
        InfoWnd.SetWndState(false);
        InfoWnd.IsOpen = false;
    }
    public void OpenShopWnd()
    {
        dialogueWnd.LockAllBtn();
        AudioSvc.Instance.PlayUIAudio(Constants.WindowOpen);
        shopWnd.SetWndState();
        shopWnd.IsOpen = true;
        baseUI.SetWndState(false);
        PutLastLayer(shopWnd.transform);
    }
    public void CloseShopWnd()
    {
        dialogueWnd.ActivateAllBtn();
        AudioSvc.Instance.PlayUIAudio(Constants.WindowClose);
        shopWnd.SetWndState(false);
        shopWnd.IsOpen = false;
    }

    public void CloseKnapsack2()
    {
        Knapsack.SetWndState(false);
        Knapsack.IsOpen = false;
    }
    public void CloseCashShopWnd()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.WindowClose);
        cashShopWnd.ClearPanel();
        cashShopWnd.SetWndState(false);
        cashShopWnd.IsOpen = false;
        GameRoot.Instance.InUI = false;
    }
    public void OpenCashShopWnd()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.WindowOpen);
        cashShopWnd.SetWndState();
        cashShopWnd.IsOpen = true;
        GameRoot.Instance.InUI = true;
        PutLastLayer(cashShopWnd.transform);
    }

    public void CloseStrengthenWnd()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.WindowClose);
        strengthenWnd.SetWndState(false);
        strengthenWnd.IsOpen = false;
        
    }
    public void OpenStrengthenWnd()
    {
        new StrengthenSender(5);
        AudioSvc.Instance.PlayUIAudio(Constants.WindowOpen);
        strengthenWnd.SetWndState();
        strengthenWnd.IsOpen = true;
        baseUI.CloseNpcDialogue();
        GameRoot.Instance.InUI = false;
        PutLastLayer(strengthenWnd.transform);
    }
    public void CloseMGFWnd()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.WindowClose);
        mGFWnd.SetWndState(false);
        mGFWnd.IsOpen = false;
    }
    public void OpenMGFWnd()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.WindowOpen);
        mGFWnd.SetWndState();
        mGFWnd.IsOpen = true;
        PutLastLayer(mGFWnd.transform);
    }
    public void CloseTransationWnd()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.WindowClose);
        //transationWnd.ClearItem();
        transationWnd.SetWndState(false);
        transationWnd.IsOpen = false;
    }
    public void OpenTransationWnd(string PlayerName, string OtherName)
    {
        AudioSvc.Instance.PlayUIAudio(Constants.WindowOpen);
        transationWnd.SetWndState();
        transationWnd.SetNames(PlayerName, OtherName);
        transationWnd.IsOpen = true;
        PutLastLayer(transationWnd.transform);
    }
    public void CloseCommunityWnd()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.WindowClose);
        communityWnd.SetWndState(false);
        communityWnd.IsOpen = false;
    }
    public void OpenCommunityWnd()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.WindowOpen);
        communityWnd.SetWndState();
        communityWnd.IsOpen = true;
        PutLastLayer(communityWnd.transform);
    }
    public void ClosePetWnd()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.WindowClose);
        petWnd.SetWndState(false);
        petWnd.IsOpen = false;
    }
    public void OpenPetWnd()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.WindowOpen);
        petWnd.SetWndState();
        petWnd.IsOpen = true;
        PutLastLayer(petWnd.transform);
    }
    public void OpenLockerWnd()
    {
        if (Knapsack.sellItemWnd.gameObject.activeSelf) return;
        CloseEquipWnd2();
        dialogueWnd.LockAllBtn();
        AudioSvc.Instance.PlayUIAudio(Constants.WindowOpen);
        lockerWnd.SetWndState();
        lockerWnd.IsOpen = true;
        PutLastLayer(lockerWnd.transform);
    }
    public void CloseLocker2()
    {
        lockerWnd.SetWndState(false);
        lockerWnd.IsOpen = false;
    }
    public void CloseLockerWnd()
    {
        dialogueWnd.ActivateAllBtn();
        AudioSvc.Instance.PlayUIAudio(Constants.WindowClose);
        lockerWnd.SetWndState(false);
        lockerWnd.IsOpen = false;
    }
    public void OpenMailBoxWnd()
    {
        if (Knapsack.sellItemWnd.gameObject.activeSelf) return;
        CloseEquipWnd2();
        dialogueWnd.LockAllBtn();
        AudioSvc.Instance.PlayUIAudio(Constants.WindowOpen);
        MailBoxWnd.SetWndState();
        MailBoxWnd.IsOpen = true;
        Knapsack.OpenAndPush();
        PutLastLayer(MailBoxWnd.transform);
    }
    public void CloseMailbox2()
    {
        MailBoxWnd.SetWndState(false);
        MailBoxWnd.IsOpen = false;
    }
    public void CloseMailBoxWnd()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.WindowClose);
        dialogueWnd.ActivateAllBtn();
        MailBoxWnd.SetWndState(false);
        MailBoxWnd.IsOpen = false;
    }

    public void CloseEquipWnd2()
    {
        equipmentWnd.SetWndState(false);
        equipmentWnd.IsOpen = false;
    }

    public void OpenMenuUI()
    {
        CloseEquipWnd2();
        CloseInfo2();
        CloseLocker2();
        CloseMailbox2();
        AudioSvc.Instance.PlayUIAudio(Constants.WindowOpen);
        menuUI.SetWndState(true);
        PutLastLayer(menuUI.transform);
    }
    public void CloseMenuUI2()
    {
        menuUI.SetWndState(false);
    }
    public void CloseMenuUI()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.WindowClose);
        menuUI.SetWndState(false);
    }
    public void OpenLearnSkillUI()
    {
        CloseEquipWnd2();
        CloseInfo2();
        CloseLocker2();
        CloseMailbox2();
        CloseDialogueWnd();
        AudioSvc.Instance.PlayUIAudio(Constants.WindowOpen);
        learnSkillWnd.Init();
        learnSkillWnd.SetWndState(true);
        PutLastLayer(learnSkillWnd.transform);
    }
    public void CloseLearnSkillUI()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.WindowClose);
        learnSkillWnd.SetWndState(false);
    }
    public void OpenCloseChatWnd()
    {
        if (IsChatWndCanOpen == true)
        {
            chatWnd.SetWndState(false);
            IsChatWndCanOpen = false;
            OpenChatBtn.image.sprite = OpenChat;
            AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        }
        else
        {
            chatWnd.SetWndState(true);
            IsChatWndCanOpen = true;
            OpenChatBtn.image.sprite = CloseChat;
            AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        }
    }

    public void CloseDialogueWnd()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.WindowClose);
        baseUI.CloseNpcDialogue();
        GameRoot.Instance.InUI = false;
    }

    public void OpenMiniGameSetting()
    {

        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        if (CanSetMiniGameSchedule())
        {
            dialogueWnd.LockAllBtn();
            miniGameSettingWnd.OpenWnd();
        }
        else
        {
            dialogueWnd.ActivateAllBtn();
            GameRoot.AddTips("請先完成剩餘課程喔!");
        }
        PutLastLayer(miniGameSettingWnd.transform);
    }
    public void OpenCloseOptionWnd()
    {
        if (optionWnd.gameObject.activeSelf)
        {
            CloseOption();
        }
        else
        {
            OpenOption();
        }
    }
    public void OpenOption()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.MiddleBtn);
        menuUI.SetWndState(false);
        optionWnd.SetWndState(true);
        PutLastLayer(optionWnd.transform);
    }
    public void CloseOption()
    {
        optionWnd.PressCancel();
    }

    public void OpenPlayerOption()
    {
        //AudioSvc.Instance.PlayUIAudio(Constants.WindowOpen);
        playerOption.SetWndState();
        playerOption.IsOpen = true;
    }
    public void ClosePlayerOption()
    {
        //AudioSvc.Instance.PlayUIAudio(Constants.WindowClose);
        playerOption.SetWndState(false);
        playerOption.IsOpen = false;
    }
    public void OpenOtherPlayerOption()
    {
        otherPlayerOption.SetWndState(true);
        otherPlayerOption.IsOpen = true;
    }
    public void CloseOtherPlayOption()
    {
        otherPlayerOption.SetWndState(false);
        otherPlayerOption.IsOpen = false;
    }

    public void OpenCloseQuestWnd()
    {
        if (QuestWnd.gameObject.activeSelf)
        {
            CloseQuestWnd();
        }
        else
        {
            OpenQuestWnd();
        }
    }
    public void OpenQuestWnd()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        QuestWnd.gameObject.SetActive(true);
        PutLastLayer(QuestWnd.transform);
    }
    public void CloseQuestWnd()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.WindowClose);
        QuestWnd.gameObject.SetActive(false);
    }

    public void OpenLearnMajorWnd()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        LearnMajorSkillWnd.SetWndState(true);
        PutLastLayer(LearnMajorSkillWnd.transform);
    }
    public void CloseLearnMajorWnd()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.WindowClose);
        LearnMajorSkillWnd.SetWndState(false);
    }
    public void OpenShipWnd()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        PutLastLayer(shipWnd.transform);
        shipWnd.gameObject.SetActive(true);
        shipWnd.Init();
    }

    public void OpenDeathWnd()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        PutLastLayer(deathWnd.transform);
        deathWnd.gameObject.SetActive(true);
    }
    #endregion

    private bool CanSetMiniGameSchedule()
    {
        bool result = false;
        if (GameRoot.Instance.ActivePlayer.MiniGameArr == null) result = true;
        else
        {
            if (GameRoot.Instance.ActivePlayer.MiniGameArr.Length > 0)
            {
                bool IsAllZero = true;
                for (int i = 0; i < GameRoot.Instance.ActivePlayer.MiniGameArr.Length; i++)
                {
                    if (GameRoot.Instance.ActivePlayer.MiniGameArr[i] > 0)
                        IsAllZero = false;
                }
                if (IsAllZero)
                    result = true;
            }
        }
        return result;
    }

    public void SetServerAnnouncement(string msg, float time)
    {
        if ((!string.IsNullOrEmpty(msg)) && time > 0)
        {
            this.serverAnouncement.SetAnnouncement(msg, time * 100);
        }
        else
        {
            this.serverAnouncement.gameObject.SetActive(false);
        }
    }

    public void OpenOtherProfileWnd(OtherProfileOperation otherProfile )
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        this.OtherProfileWnd.SetText(otherProfile);
        this.OtherProfileWnd.gameObject.SetActive(true);
        PutLastLayer(OtherProfileWnd.transform);
    }
}
