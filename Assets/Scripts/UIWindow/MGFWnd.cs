using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PEProtocal;
using System.Globalization;
public class MGFWnd : Inventory
{
    private static MGFWnd _instance;
    public static MGFWnd Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = UISystem.Instance.mGFWnd;
            }
            return _instance;
        }
    }

    public bool IsOpen = false;
    public Button CloseBtn1;
    public Button CloseBtn2;
    public Button MakeBtn;
    public Button UpBtn;
    public Button DownBtn;
    public Button MinBtn;
    public Button MaxBtn;
    public Text AmountTxt;
    public Text Title;
    public GameObject Tags;
    public GameObject ManuItemGroup;
    public ItemSlot ItemSlot;
    public GameObject FormulaSlots;
    int WantMakeAmount;

    List<int> LearnedFormula = new List<int> { 101,102,103,104,105,106,107,108,109,110,111,112,114,116,601,602,604,605,606,607,608,609,610,611,612,614,501,502,503,504,505,506,507,508,509,510,511,512,451,452,453,454,455,456,
    201,202,203,204,205,206,207,208,209,210,211,212,251,252,253,254,255,256,257,258,259,301,302,303,304,305,306,307,401,402,
    701,703,704,706,707,709,711,712,801,802,803,804,805,806,807,
    1,2,3,4,11,12,13,14,21,22,23,24,25,26,27,28,29,30,31,37,41,42,43,44,45,46,47,48,49,50,56,61,62,63,64,65,66,67,68};//學到的所有配方
    //List<int> LearnedFormula = new List<int> {  101,601,602,604,605,606,607,608,609,610,611 ,501 };//,451,452,453,454,455,456502,503,504,505,506,507,508,509,510,511,512
    int level = 1;//技能等級

    Dictionary<string, List<ManuInfo>> FormulaCates = new Dictionary<string, List<ManuInfo>>();
    //把學會的配方List轉成大字典
    Dictionary<string, Dictionary<string, List<ManuInfo>>> CateFormula = new Dictionary<string, Dictionary<string, List<ManuInfo>>>();

    public void CateForFormula()
    {
        Dictionary<int,ManuInfo> FormulaDict = ResSvc.Instance.FormulaDict;
        foreach (int id in LearnedFormula)
        {
            List<ManuInfo> ManuLists = new List<ManuInfo>();
            ManuInfo manuinfo = FormulaDict[id];
            //藥水
            if (id > 0 && id <= 100)
            {
                if (!CateFormula.ContainsKey(MSkill.煉金製造術.ToString()))
                    CateFormula[MSkill.煉金製造術.ToString()] = new Dictionary<string, List<ManuInfo>>();
                if (id > 0 && id <= 80)
                {
                    if (!CateFormula[MSkill.煉金製造術.ToString()].ContainsKey("藥水"))
                        CateFormula[MSkill.煉金製造術.ToString()]["藥水"] = new List<ManuInfo>();
                    CateFormula[MSkill.煉金製造術.ToString()]["藥水"].Add(FormulaDict[id]);
                }
                if (id > 80 && id <= 100)
                {
                    if (!CateFormula[MSkill.煉金製造術.ToString()].ContainsKey("卷軸"))
                        CateFormula[MSkill.煉金製造術.ToString()]["卷軸"] = new List<ManuInfo>();
                    CateFormula[MSkill.煉金製造術.ToString()]["卷軸"].Add(FormulaDict[id]);
                }
            }
            //礦石
            else if (id > 100 && id <= 200)
            {
                if (!CateFormula.ContainsKey(MSkill.礦物學.ToString()))
                {
                    CateFormula[MSkill.礦物學.ToString()] = new Dictionary<string, List<ManuInfo>>();
                    CateFormula[MSkill.礦物學.ToString()]["礦石"] = new List<ManuInfo>();
                }
                CateFormula[MSkill.礦物學.ToString()]["礦石"].Add(FormulaDict[id]);
            }
            //銳利武器
            else if (id > 200 && id <= 450)
            {
                if (!CateFormula.ContainsKey(MSkill.銳利武器製造術.ToString()))
                    CateFormula[MSkill.銳利武器製造術.ToString()] = new Dictionary<string, List<ManuInfo>>();
                if (id > 200 && id <= 250)
                {
                    if (!CateFormula[MSkill.銳利武器製造術.ToString()].ContainsKey("短劍"))
                        CateFormula[MSkill.銳利武器製造術.ToString()]["短劍"] = new List<ManuInfo>();
                    CateFormula[MSkill.銳利武器製造術.ToString()]["短劍"].Add(FormulaDict[id]);
                }

                else if (id > 250 && id < 300)
                {
                    if (!CateFormula[MSkill.銳利武器製造術.ToString()].ContainsKey("刀劍"))
                        CateFormula[MSkill.銳利武器製造術.ToString()]["刀劍"] = new List<ManuInfo>();
                    CateFormula[MSkill.銳利武器製造術.ToString()]["刀劍"].Add(FormulaDict[id]);
                }

                else if (id > 300 && id < 350)
                {
                    if (!CateFormula[MSkill.銳利武器製造術.ToString()].ContainsKey("大劍"))
                        CateFormula[MSkill.銳利武器製造術.ToString()]["大劍"] = new List<ManuInfo>();
                    CateFormula[MSkill.銳利武器製造術.ToString()]["大劍"].Add(FormulaDict[id]);
                }

                else if (id > 350 && id < 400)
                {
                    if (!CateFormula[MSkill.銳利武器製造術.ToString()].ContainsKey("雙劍"))
                        CateFormula[MSkill.銳利武器製造術.ToString()]["雙劍"] = new List<ManuInfo>();
                    CateFormula[MSkill.銳利武器製造術.ToString()]["雙劍"].Add(FormulaDict[id]);
                }

                else if (id > 400 && id < 450)
                {
                    if (!CateFormula[MSkill.銳利武器製造術.ToString()].ContainsKey("槍"))
                        CateFormula[MSkill.銳利武器製造術.ToString()]["槍"] = new List<ManuInfo>();
                    CateFormula[MSkill.銳利武器製造術.ToString()]["槍"].Add(FormulaDict[id]);
                }
            }
            //鈍器
            else if (id > 450 && id < 700)
            {
                if (!CateFormula.ContainsKey(MSkill.鈍器製造術.ToString()))
                    CateFormula[MSkill.鈍器製造術.ToString()] = new Dictionary<string, List<ManuInfo>>();

                if (id > 450 && id < 500)
                {
                    if (!CateFormula[MSkill.鈍器製造術.ToString()].ContainsKey("斧頭"))
                        CateFormula[MSkill.鈍器製造術.ToString()]["斧頭"] = new List<ManuInfo>();
                    CateFormula[MSkill.鈍器製造術.ToString()]["斧頭"].Add(FormulaDict[id]);
                }

                else if (id > 500 && id < 550)
                {
                    if (!CateFormula[MSkill.鈍器製造術.ToString()].ContainsKey("法杖"))
                        CateFormula[MSkill.鈍器製造術.ToString()]["法杖"] = new List<ManuInfo>();
                    CateFormula[MSkill.鈍器製造術.ToString()]["法杖"].Add(FormulaDict[id]);
                }
                else if (id > 550 && id < 600)
                {
                    if (!CateFormula[MSkill.鈍器製造術.ToString()].ContainsKey("書"))
                        CateFormula[MSkill.鈍器製造術.ToString()]["書"] = new List<ManuInfo>();
                    CateFormula[MSkill.鈍器製造術.ToString()]["書"].Add(FormulaDict[id]);
                }
                else if (id > 600 && id < 650)
                {
                    if (!CateFormula[MSkill.鈍器製造術.ToString()].ContainsKey("鈍器"))
                        CateFormula[MSkill.鈍器製造術.ToString()]["鈍器"] = new List<ManuInfo>();
                    CateFormula[MSkill.鈍器製造術.ToString()]["鈍器"].Add(FormulaDict[id]);
                }

                else if (id > 650 && id < 700)
                {
                    if (!CateFormula[MSkill.鈍器製造術.ToString()].ContainsKey("十字架"))
                        CateFormula[MSkill.鈍器製造術.ToString()]["十字架"] = new List<ManuInfo>();
                    CateFormula[MSkill.鈍器製造術.ToString()]["十字架"].Add(FormulaDict[id]);
                }
            }
            //弓槍
            else if (id > 700 && id < 850)
            {
                if (!CateFormula.ContainsKey(MSkill.弓槍製造術.ToString()))
                    CateFormula[MSkill.弓槍製造術.ToString()] = new Dictionary<string, List<ManuInfo>>();

                if (id > 700 && id < 750)
                {
                    if (!CateFormula[MSkill.弓槍製造術.ToString()].ContainsKey("弓"))
                        CateFormula[MSkill.弓槍製造術.ToString()]["弓"] = new List<ManuInfo>();
                    CateFormula[MSkill.弓槍製造術.ToString()]["弓"].Add(FormulaDict[id]);
                }
                else if (id > 750 && id < 800)
                {

                    if (!CateFormula[MSkill.弓槍製造術.ToString()].ContainsKey("弩"))
                        CateFormula[MSkill.弓槍製造術.ToString()]["弩"] = new List<ManuInfo>();
                    CateFormula[MSkill.弓槍製造術.ToString()]["弩"].Add(FormulaDict[id]);
                }
                else if (id > 800 && id < 850)
                {
                    if (!CateFormula[MSkill.弓槍製造術.ToString()].ContainsKey("手槍"))
                        CateFormula[MSkill.弓槍製造術.ToString()]["手槍"] = new List<ManuInfo>();
                    CateFormula[MSkill.弓槍製造術.ToString()]["手槍"].Add(FormulaDict[id]);
                }
            }
            //防盾
            else if (id > 850 && id < 900)
            {
                if (!CateFormula.ContainsKey(MSkill.防盾製造術.ToString()))
                {
                    CateFormula[MSkill.防盾製造術.ToString()] = new Dictionary<string, List<ManuInfo>>();
                    CateFormula[MSkill.防盾製造術.ToString()]["防盾"] = new List<ManuInfo>();
                }
                CateFormula[MSkill.防盾製造術.ToString()]["防盾"].Add(manuinfo);
            }
        }
    }

    protected override void InitWnd()
    {
        slotLists.Add(FormulaSlots.GetComponentsInChildren<ItemSlot>());
        SetActive(InventorySys.Instance.toolTip.gameObject, true);
        base.InitWnd();
    }




    /// <summary>
    /// 鍊金術
    /// </summary>
    public void Alchemy()
    {
        UISystem.Instance.OpenMGFWnd();
        Title.text = ((Rank)level).ToString();
        string skill = MSkill.煉金製造術.ToString();
        Dictionary<string, List<ManuInfo>> dict = CateFormula[skill];
        Title.text += skill;
        SetTags(dict);

    }
    /// <summary>
    /// 礦物學
    /// </summary>
    public void Mineral()
    {
        UISystem.Instance.OpenMGFWnd();
        Title.text = ((Rank)level).ToString();
        string skill = MSkill.礦物學.ToString();
        Dictionary<string, List<ManuInfo>> dict = CateFormula[skill];
        Title.text += skill;
        SetTags(dict);

    }

    /// <summary>
    /// 銳利武器
    /// </summary>
    public void SharpWeapon()
    {
        UISystem.Instance.OpenMGFWnd();
        Title.text = ((Rank)level).ToString();
        string skill = MSkill.銳利武器製造術.ToString();
        Dictionary<string, List<ManuInfo>> dict = CateFormula[skill];
        Title.text += skill;
        SetTags(dict);
    }

    /// <summary>
    /// 鈍器
    /// </summary>
    public void Blunt()
    {
        UISystem.Instance.OpenMGFWnd();
        Title.text = ((Rank)level).ToString();
        string skill = MSkill.鈍器製造術.ToString();
        Dictionary<string, List<ManuInfo>> dict = CateFormula[skill];
        Title.text += skill;
        SetTags(dict);
    }

    /// <summary>
    /// 弓槍
    /// </summary>
    public void BowGun()
    {
        UISystem.Instance.OpenMGFWnd();
        Title.text = ((Rank)level).ToString();
        string skill = MSkill.弓槍製造術.ToString();
        Dictionary<string, List<ManuInfo>> dict = CateFormula[skill];
        Title.text += skill;
        SetTags(dict);
    }
    /// <summary>
    /// 防盾
    /// </summary>
    public void Shield()
    {
        UISystem.Instance.OpenMGFWnd();
        Title.text = ((Rank)level).ToString();
        string skill = MSkill.防盾製造術.ToString();
        Dictionary<string, List<ManuInfo>> dict = CateFormula[skill];
        Title.text += skill;
        SetTags(dict);
    }


    #region UI Operation
    public void ClickCloseBtn()
    {
        UISystem.Instance.CloseMGFWnd();
        IsOpen = false;
        ClearTags();
    }

    public void ClickMaxBtn()
    {
        WantMakeAmount = CanMakeAmount;
        AmountTxt.text = WantMakeAmount.ToString();
        UpBtn.interactable = false;
        if (WantMakeAmount != 0)
        {
            MakeBtn.interactable = true;
            DownBtn.interactable = true;
        }
        else
        {
            DownBtn.interactable = false;
        }
    }

    public void ClickUpBtn()
    {
        if(WantMakeAmount<= CanMakeAmount)
        {
            MakeBtn.interactable = true;
            WantMakeAmount++;
            AmountTxt.text = WantMakeAmount.ToString();
            DownBtn.interactable = true;
            if(WantMakeAmount == CanMakeAmount)
            {
                UpBtn.interactable = false;
            }
        }
    }

    public void ClickDownBtn()
    {
        if (WantMakeAmount > 0)
        {
            WantMakeAmount--;
            AmountTxt.text = WantMakeAmount.ToString();
            UpBtn.interactable = true;
            if (WantMakeAmount == 0)
            {
                DownBtn.interactable = false;
                MakeBtn.interactable = false;
            }
        }
    }

    public void ClickForwardBtn()
    {
        if(Curpage !=0)
        {
            Curpage -= 1;
            BackBtn.interactable = true;
            CurrPageTxt.text = (Curpage + 1).ToString();
            SetManuItem(Curpage, CurrentList);
            if(Curpage == 0)
            {
                ForwardBtn.interactable = false;
            }
        }
    }
    public void ClickBackBtn()
    {
        if(Curpage != TotPage-1)
        {
            Curpage += 1;
            ForwardBtn.interactable = true;
            CurrPageTxt.text = (Curpage + 1).ToString();
            SetManuItem(Curpage, CurrentList);
            if(Curpage == TotPage - 1)
            {
                BackBtn.interactable = false;
            }
        }
    }


    ManuInfo currmanuInfo;    
    int currentID;
    public void ClickMnufactureBtn()
    {
        new ManufactureSender(1,currentID,WantMakeAmount);
    }



    #endregion



    public List<ManuInfo> CurrentList = null;

    //先把不同類別show出來 刀劍、雙刀、大劍....
    public void SetTags(Dictionary<string, List<ManuInfo>> dict)
    {
        List<string> TagList = new List<string>();
        foreach(var s in dict.Keys)
        {
            TagList.Add(s);
        }
        float BtnGroupWidth = Tags.GetComponent<RectTransform>().rect.width;

        foreach (var tag in TagList)
        {
            ManufactureTag ManuTag = ((GameObject)Instantiate(Resources.Load("Prefabs/ManufactureTag"))).transform.GetComponent<ManufactureTag>();
            Button TagBtn = ManuTag.transform.GetComponent<Button>();
            ManuTag.SetText(tag);
            TagBtn.onClick.AddListener(() =>
            {
                AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
                PressedOneTagsReset();
                ManuTag.SetButton(true);
                CurrentList = dict[tag];
                SetPageAndPageBtn(CurrentList);
                SetManuItem(0, CurrentList);

            });
            ManuTag.transform.SetParent(Tags.transform);
            if (tag == TagList[0])
            {
                TagBtn.onClick.Invoke();
            }
        }
    }





   


    public Text CurrPageTxt;
    public Text TotPageTxt;
    public Button ForwardBtn;
    public Button BackBtn;
    int Curpage;
    int TotPage;


    public void PressedOneTagsReset()
    {
        int count = Tags.transform.childCount;
        for (int i=0;i<count;i++)
        {
            Tags.transform.GetChild(i).GetComponent<ManufactureTag>().SetButton(false);
        }
    }

    public void SetPageAndPageBtn(List<ManuInfo> list)
    {
        TotPage = (int)Mathf.Ceil(list.Count / 12.0f);
        TotPageTxt.text = TotPage.ToString();
        Curpage = 0;
        CurrPageTxt.text = (Curpage + 1).ToString();
        ForwardBtn.interactable = false;
        if (TotPage <= 1)
            BackBtn.interactable = false;
        else
            BackBtn.interactable = true;
    }


    public void PressedOneItemReset()
    {
        int count = ManuItemGroup.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            ManuItemGroup.transform.GetChild(i).GetComponent<ManufactureItem>().SetButton(false);

        }
    }

    public int CanMakeAmount;
    public Text PercentTxt;
    public Text ExpTxt;
    public void SetManuItem(int Page,List<ManuInfo> Items)
    {
        ClearManuItems();
        List<ManuInfo> items = Items.GetRange(Page * 12, (Items.Count - Page * 12)>12? 12: (Items.Count - Page * 12));
        foreach (var item in items)
        {
            ManufactureItem ManuItem = ((GameObject)Instantiate(Resources.Load("Prefabs/MFItem"))).transform.GetComponent<ManufactureItem>();
            Button ItemBtn = ManuItem.transform.GetComponent<Button>();
            ManuItem.SetItem(item.ItemID,item.ItemName);
            ItemBtn.onClick.AddListener(() =>
            {
                PressedOneItemReset();
                ManuItem.SetButton(true);
                ItemSlot.RemoveItemUI();            
                ItemSlot.StoreItem(ManuItem.CurrentItem, item.Amount);
                PutFormulas(item.RequireItem, item.RequireItemAmount);
                ExpTxt.text = item.Experience.ToString();
                PercentTxt.text = item.Probablity.ToString();

                CheckAmountAndButton(item);
                currentID = item.FormulaID;
                currmanuInfo = item;

            });
            ManuItem.transform.SetParent(ManuItemGroup.transform);
            if (item == items[ 0])
            {
                ItemBtn.onClick.Invoke();
            }
        }
    }

    public void CheckAmountAndButton(ManuInfo item)
    {
        CanMakeAmount = CheckCanDoAmount(item.RequireItem, item.RequireItemAmount);

        if (CanMakeAmount > 0)
        {
            DownBtn.interactable = false;
            UpBtn.interactable = true;
        }
        else
        {
            DownBtn.interactable = false;
            UpBtn.interactable = false;
        }
        MakeBtn.interactable = false;
        WantMakeAmount = 0;
        AmountTxt.text = WantMakeAmount.ToString();
    }


    public void PutFormulas(int[] requireItem,int[] requireNum)
    {
        ClearPanel();
        int iter = 0;
        foreach( var id in requireItem)
        {

            if( id != 0)
            {
                Item Item = InventorySys.Instance.GetItemById(id);
                slotLists[0][iter].StoreItem(Item, requireNum[iter]);
                iter++;
            }
            else
            {
                break;
            }
        }
    }

    public void ClearPanel()
    {
        foreach (var slot in slotLists[0])
        {

            if (slot.HasItem())
            {
                slot.RemoveItemUI();
            }
        }
    }

    public void ClearTags()
    {
        int childCount = Tags.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Destroy(Tags.transform.GetChild(i).gameObject);
        }
    }

    public void ClearManuItems()
    {
        int childCount = ManuItemGroup.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Destroy(ManuItemGroup.transform.GetChild(i).gameObject);
        }
    }



    public int CheckCanDoAmount(int[] requireItem, int[] requireNum)
    {
        int minAmount = int.MaxValue;
        
        for(int i=0; i<6;i++)
        {
            if(requireItem[i] == 0)
            {
                break;
            }
            else
            {
                minAmount = Mathf.Min(minAmount, KnapsackWnd.Instance.GetAmountofGroup(requireItem[i], requireNum[i]));
                if (minAmount == 0)
                    break;
            }
        }
        return minAmount; 
    }
    public void ConsumItem(Dictionary<int,int> cunsumitem)
    {
        Dictionary<int, Item> knapsack = GameRoot.Instance.ActivePlayer.NotCashKnapsack;
        foreach (var slot in cunsumitem)
        {
            if(slot.Value == -1)
            {
                KnapsackWnd.Instance.FindSlot(slot.Key).RemoveItemUI();
                knapsack.Remove(slot.Key);
            }
            else
            {
                KnapsackWnd.Instance.FindSlot(slot.Key).RemoveItemUI();
                Item item = knapsack[slot.Key];
                if(item.Count>slot.Value)
                {
                    item.Count -= slot.Value;
                    //item.Position = slot.Key;
                    KnapsackWnd.Instance.FindSlot(slot.Key).StoreItem(item, item.Count);
                }
                else
                {
                    knapsack.Remove(slot.Key);
                }

            }

        }


    }
    public void AddItemInKnap(Dictionary<int,Item> items)
    {
        Dictionary<int, Item> knapsack = GameRoot.Instance.ActivePlayer.NotCashKnapsack;

        foreach(var item in items)
        {
            KnapsackWnd.Instance.FindSlot(item.Value.Position).StoreItem(item.Value,item.Value.Count);
            knapsack[item.Value.Position] = item.Value;

        }


        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
    }

    public void ProessManufactureResponse(ManufactureResponse rsp)
    {
        switch (rsp.OperationType)
        {
            case 1:
                MessageBox.Show("檢查一下空間是否充足~");
                break;
            case 2:
                MessageBox.Show("製作成功!!");
                ConsumItem(rsp.RemoveAndConSume);
                AddItemInKnap(rsp.GetItem);
                CheckAmountAndButton(currmanuInfo);
                UISystem.Instance.baseUI.AddExp(currmanuInfo.Experience);
                break;
            case 3:
                MessageBox.Show("製作失敗QQ");
                ConsumItem(rsp.RemoveAndConSume);
                CheckAmountAndButton(currmanuInfo);

                break;
        }
    }
}


public enum Rank
{
    基礎,
    初級,
    中級,
    高級,
    特級
}

public enum MSkill
{
    煉金製造術,
    礦物學,
    銳利武器製造術,
    鈍器製造術,
    弓槍製造術,
    防盾製造術,
}