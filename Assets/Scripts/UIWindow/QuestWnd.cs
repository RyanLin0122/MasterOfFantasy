using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PEProtocal;

public class QuestWnd : MonoBehaviour
{
    public GameObject itemPrefab;

    public TabView Tabs;
    public ListView QuestList;

    public QuestWndInfo questInfo;

    private bool showAvailableList = false;

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
}
