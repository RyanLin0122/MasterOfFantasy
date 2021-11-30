using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PEProtocal;

public class QuestWnd : MonoBehaviour
{
    public Text title;
    public GameObject itemPrefab;

    //public TabView Tabs;
    public ListView listNormal;
    public ListView listTimeLimit;

    public UIQuestInfo questInfo;

    private bool showAvailableList = false;

    private void Start()
    {
        this.listNormal.onItemSelected += this.OnQuestSelected;
        this.listTimeLimit.onItemSelected += this.OnQuestSelected;
        //this.Tabs.OnTabSelect += OnSelectTab;
        RefreshUI();
        //QuestManager.Instance.OnQuestChanged += RefreshUI;
    }

    void OnSelectedTab(int idx)
    {
        showAvailableList = idx == 1;
        RefreshUI();
    }

    private void OnDestrot()
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
                if (kv.Value.Info == null)
                    continue;
            }

            GameObject go = Instantiate(itemPrefab, kv.Value.Define.Type == QuestType.OneTime ? this.listNormal.transform : this.listTimeLimit.transform);
            UIQuestItem ui = go.GetComponent<UIQuestItem>();
            ui.SetQuestInfo(kv.Value);
            if (kv.Value.Define.Type == QuestType.OneTime)
                this.listNormal.AddItem(ui as ListView.ListViewItem);
            else
                this.listTimeLimit.AddItem(ui as ListView.ListViewItem);
        }
    }
    
    void ClearAllQuestList()
    {
        this.listNormal.RemoveAll();
        this.listTimeLimit.RemoveAll();
    }

    public void OnQuestSelected(ListView.ListViewItem item)
    {
        UIQuestItem questItem = item as UIQuestItem;
        this.questInfo.SetQuestInfo(questInfo.quest);
    }
}
