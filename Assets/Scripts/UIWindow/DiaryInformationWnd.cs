using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PEProtocal;

public class DiaryInformationWnd : MonoBehaviour
{
    public bool IsMonster = false;
    private List<int> IDs = new List<int>();
    public Text PageText;
    public Transform InfoItemsContainer;

    public int CurrentPage;
    public int MaxPage;
    public Button LastBtn;
    public Button NextBtn;

    public Text TitleText;

    public Sprite ChoosedSprite;
    public Sprite UnChoosedSprite;
    public Image NPCBtnImg;
    public Image MonsterBtnImg;

    public bool IsExam = false;
    public void InitInfos()
    {
        if (!IsMonster)
        {
            LoadNPCs(); 
            SetNPCs(0);
        }
        else
        {
            LoadMonsters();
            SetMonsters(0);
        }
        InfoItemsContainer.GetComponentInChildren<UIInfoObject>().SetInfo();
    }

    public void PressNPCBtn()
    {
        IsMonster = false;
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        NPCBtnImg.sprite = ChoosedSprite;
        MonsterBtnImg.sprite = UnChoosedSprite;
        NPCBtnImg.GetComponentInChildren<Text>().color = Color.white;
        MonsterBtnImg.GetComponentInChildren<Text>().color = Color.black;
        LoadNPCs();
        SetNPCs(0);
        InfoItemsContainer.GetComponentInChildren<UIInfoObject>().SetInfo();
    }

    public void LoadNPCs()
    {
        IDs.Clear();
        foreach (var kv in ResSvc.Instance.NpcCfgDataDic)
        {
            IDs.Add(kv.Value.ID);
        }
        SetNPCs(0);
    }

    public void SetNPCs(int Page)
    {
        if (InfoItemsContainer.childCount > 0)
        {
            foreach (var Info in InfoItemsContainer.GetComponentsInChildren<UIInfoObject>())
            {
                Destroy(Info.gameObject);
            }
        }
        CurrentPage = Page;
        MaxPage = Mathf.CeilToInt((float)IDs.Count / 10);
        PageText.text = (CurrentPage + 1) + " / " + MaxPage;

        LastBtn.interactable = true;
        NextBtn.interactable = true;
        if (Page >= MaxPage - 1)
        {
            NextBtn.interactable = false;
        }
        if (Page == 0)
        {
            LastBtn.interactable = false;
        }

        for (int i = Page * 10; i < Page * 10 + 10; i++)
        {
            if (i < IDs.Count)
                InstantiateNPCItem(IDs[i]);
        }
    }

    public void PressMonsterBtn()
    {
        IsMonster = true;
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        NPCBtnImg.sprite = UnChoosedSprite;
        MonsterBtnImg.sprite = ChoosedSprite;
        NPCBtnImg.GetComponentInChildren<Text>().color = Color.black;
        MonsterBtnImg.GetComponentInChildren<Text>().color = Color.white;
        LoadMonsters();
        SetMonsters(0);
        InfoItemsContainer.GetComponentInChildren<UIInfoObject>().SetInfo();
    }

    public void PressNextBtn()
    {
        if (CurrentPage + 1 < MaxPage)
        {
            CurrentPage++;
            AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
            if (!IsMonster) SetNPCs(CurrentPage);
            else SetMonsters(CurrentPage);
        }
    }

    public void PressLastBtn()
    {
        if (CurrentPage > 0)
        {
            CurrentPage--;
            AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
            if (!IsMonster) SetNPCs(CurrentPage);
            else SetMonsters(CurrentPage);
        }
    }

    public void LoadMonsters()
    {
        IDs.Clear();
        foreach (var kv in ResSvc.Instance.MonsterInfoDic)
        {
            IDs.Add(kv.Value.MonsterID);
        }
        SetMonsters(0);
    }

    public void SetMonsters(int Page)
    {
        if (InfoItemsContainer.childCount > 0)
        {
            foreach (var Info in InfoItemsContainer.GetComponentsInChildren<UIInfoObject>())
            {
                Destroy(Info.gameObject);
            }
        }
        CurrentPage = Page;
        MaxPage = Mathf.CeilToInt((float)IDs.Count / 10);
        PageText.text = (CurrentPage + 1) + " / " + MaxPage;

        LastBtn.interactable = true;
        NextBtn.interactable = true;
        if (Page >= MaxPage - 1)
        {
            NextBtn.interactable = false;
        }
        if (Page == 0)
        {
            LastBtn.interactable = false;
        }

        for (int i = Page * 10; i < Page * 10 + 10; i++)
        {
            if (i < IDs.Count)
                InstantiateMonsterItem(IDs[i]);
        }
    }

    public void InstantiateNPCItem(int NPCID)
    {
        UIInfoObject info = (Instantiate(Resources.Load("Prefabs/InfoObject"), InfoItemsContainer) as GameObject).GetComponent<UIInfoObject>();
        info.SetText(false, NPCID);
    }
    public void InstantiateMonsterItem(int MonsterID)
    {
        UIInfoObject info = (Instantiate(Resources.Load("Prefabs/InfoObject"), InfoItemsContainer) as GameObject).GetComponent<UIInfoObject>();
        info.SetText(true, MonsterID);
    }

    public Image InfoImg;
    public Text InfoDescription;
    public void SetInfo(int ID)
    {
        if (IsMonster)
        {
            MonsterInfo monsterInfo;
            if (ResSvc.Instance.MonsterInfoDic.TryGetValue(ID, out monsterInfo))
            {
                TitleText.text = monsterInfo.Name;
                int SpriteNum = monsterInfo.MonsterAniDic[MonsterAniType.Idle].AnimPosition[0];
                InfoImg.sprite = Resources.LoadAll<Sprite>(monsterInfo.Sprites[0])[SpriteNum];
                InfoImg.SetNativeSize();
                InfoImg.transform.localScale = new Vector2(0.4f, 0.4f);
                InfoDescription.text = monsterInfo.Description;
            }
        }
        else
        {
            NpcConfig npcConfig;
            if (ResSvc.Instance.NpcCfgDataDic.TryGetValue(ID, out npcConfig))
            {
                TitleText.text = npcConfig.Name;
                InfoImg.sprite = Resources.Load<Sprite>("NPC/" + npcConfig.Sprite);
                InfoImg.SetNativeSize();
                InfoImg.transform.localScale = new Vector2(0.4f, 0.4f);
                string Des = "";
                Des += "年齡: " + npcConfig.Age;
                Des += " 血型: " + npcConfig.BloodType;
                Des += " 身高: " + npcConfig.Height;
                Des += " 體重: " + npcConfig.Weight;
                Des += "\n職業: " + npcConfig.Job;
                Des += " 興趣: " + npcConfig.Hobby;
                Des += "\n特技: " + npcConfig.Trick;
                Des += "\n個性: " + npcConfig.Personality;
                Des += "\n座右銘: " + npcConfig.Motto;
                InfoDescription.text = Des;
            }
        }
    }
}
