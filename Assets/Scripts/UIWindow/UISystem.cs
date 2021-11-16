using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISystem : SystemRoot
{
    public static UISystem Instance = null;

    public override void InitSys()
    {
        Instance = this;
        Knapsack.InitKnapsack();
        lockerWnd.InitLocker();
        MailBoxWnd.InitMailBox();
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
    public Transform BuffIconsContainer;

    private readonly object stackLock = new object();
    public Stack<IStackWnd> stack = new Stack<IStackWnd>();

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
    }
    public void CloseTransationWnd()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.WindowClose);
        //transationWnd.ClearItem();
        transationWnd.SetWndState(false);
        transationWnd.IsOpen = false;
    }
    public void OpenTransationWnd(string PlayerName , string OtherName)
    {
        AudioSvc.Instance.PlayUIAudio(Constants.WindowOpen);
        transationWnd.SetWndState();
        transationWnd.SetNames(PlayerName,OtherName);
        transationWnd.IsOpen = true;
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
    }
    public void OpenLockerWnd()
    {
        CloseEquipWnd2();
        dialogueWnd.LockAllBtn();
        AudioSvc.Instance.PlayUIAudio(Constants.WindowOpen);
        lockerWnd.SetWndState();
        lockerWnd.IsOpen = true;
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
        CloseEquipWnd2();
        dialogueWnd.LockAllBtn();
        AudioSvc.Instance.PlayUIAudio(Constants.WindowOpen);
        MailBoxWnd.SetWndState();
        MailBoxWnd.IsOpen = true;
        Knapsack.OpenAndPush();
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
        AudioSvc.Instance.PlayUIAudio(Constants.WindowOpen);
        learnSkillWnd.Init();
        learnSkillWnd.SetWndState(true);
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
        dialogueWnd.LockAllBtn();
        miniGameSettingWnd.OpenWnd();
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
    public void ClosePlayOption()
    {
        //AudioSvc.Instance.PlayUIAudio(Constants.WindowClose);
        playerOption.SetWndState(false);
        playerOption.IsOpen = false;
    }
    #endregion

}
