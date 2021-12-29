using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PEProtocal;

public class QuestWnd : WindowRoot, IStackWnd
{
    public static QuestWnd Instance;
    public GameObject itemPrefab;

    public TabView Tabs;
    public ListView QuestList;
    public bool IsOpen = false;
    public QuestWndInfo questInfo;

    private bool showAvailableList = false;

    public void Init()
    {
        Instance = this;
    }
    private void Start()
    {
        this.QuestList.onItemSelected += this.OnQuestSelected;
        this.Tabs.OnTabSelect += OnSelectTab;
        RefreshUI();
    }

    private void OnEnable()
    {
        this.questInfo.CloseInfo();
        RefreshUI();
    }

    void OnSelectTab(int idx)
    {
        showAvailableList = idx == 1;
        RefreshUI();
    }

    private void OnDestroy()
    {
        //QuestManager.Instance.OnQuestChanged -= RefreshUI;
    }

    void RefreshUI()
    {
        ClearAllQuestList();
        InitAllQuestItems();
    }


    void InitAllQuestItems()
    {
        foreach (var kv in QuestManager.Instance.allQuests)
        {
            if (showAvailableList)
            {
                if (kv.Value.Info != null)
                    continue;
            }
            else
            {
                if (kv.Value.Info == null || kv.Value.Info.status == QuestStatus.Finished)
                    continue;
            }

            GameObject go = Instantiate(itemPrefab, this.QuestList.transform);
            UIQuestItem ui = go.GetComponent<UIQuestItem>();
            ui.SetQuestInfo(kv.Value);
            this.QuestList.AddItem(ui);
        }
    }

    void ClearAllQuestList()
    {
        this.QuestList.RemoveAll();
    }

    public void OnQuestSelected(ListView.ListViewItem item)
    {
        UIQuestItem questItem = item as UIQuestItem;
        this.questInfo.SetQuestInfo(questItem.quest);
    }


    public void OpenAndPush()
    {

        AudioSvc.Instance.PlayUIAudio(Constants.WindowOpen);
        SetWndState();
        IsOpen = true;
        UISystem.Instance.Push(this);

    }
    public void CloseAndPop()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.WindowClose);
        SetWndState(false);
        IsOpen = false;
        UISystem.Instance.ForcePop(this);
    }

    public void KeyBoardCommand()
    {
        if (IsOpen)
        {
            CloseAndPop();
            IsOpen = false;
        }
        else
        {
            OpenAndPush();
            IsOpen = true;
        }
    }
}
