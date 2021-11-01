using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PEProtocal;
using System.Globalization;
using System.Threading;

public class StrengthenWnd : Inventory
{
    private static StrengthenWnd _instance;
    public static StrengthenWnd Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = UISystem.Instance.strengthenWnd;
            }
            return _instance;
        }
    }

    public bool IsOpen = false;
    public Button CloseBtn1;
    public Button CloseBtn2;
    public Button StrengthenBtn;
    public Button InfoBtn;
    public Item RegisterStrengthenItem = null;
    public Item RegisterStone = null;
    public GameObject ItemSlot;
    public GameObject StoneSlot;
    public GameObject StrengthenedItemSlot;
    public Text EffectText;
    public GameObject img1;
    public GameObject img2;
    public Image imgFG;
    float timeForProgress;

    protected override void InitWnd()
    {
        SetActive(InventorySys.Instance.toolTip.gameObject, true);
        KnapsackWnd.Instance.IsForge = true;
        slotLists.Add(ItemSlot.GetComponentsInChildren<StrengthenItemSlot>());
        slotLists.Add(StoneSlot.GetComponentsInChildren<StrengthenStoneSlot>());
        slotLists.Add(StrengthenedItemSlot.GetComponentsInChildren<ItemSlot>());
        base.InitWnd();
        AnimaInit();
        StrengthenBtn.interactable = false;
        KnapsackWnd.Instance.OpenAndPush();
    }

    public void AnimaInit()
    {
        img1.SetActive(false);
        img2.SetActive(false);
        imgFG.fillAmount = 0;
        timeForProgress = 0;
    }
    public void SetProgress(int tid)
    {
        LockButton();
        timeForProgress += 0.04f;
        imgFG.fillAmount = timeForProgress;
        if (timeForProgress >= 1)
        {
            img2.SetActive(true);
            
        }
        if(timeForProgress > 1)
        {
            new StrengthenSender(6);
        }
    }

    public void LockButton()
    {
        StrengthenBtn.interactable = false;
        CloseBtn1.interactable = false;
        CloseBtn2.interactable = false;
        InfoBtn.interactable = false;
    }

    public void ClearPanel()
    {
        for (int i = 0; i < 3; i++)
        {
            foreach (var slot in slotLists[i])
            {

                if (slot.HasItem())
                {
                    GameObject obj = slot.GetComponentInChildren<ItemUI>().gameObject;
                    Destroy(obj);
                }
            }
        }
    }

    public void ClickCloseBtn()
    {
        new StrengthenSender(4);
    }

    
    public void PressStrengthenBtn()
    {
        MessageBox.Show("確定要強化嗎?", MessageBoxType.Confirm,() => { img1.SetActive(true);TimerSvc.Instance.AddTimeTask(SetProgress, 100, PETimeUnit.Millisecond, 25); });
    }

    public void GetWeapon(Item item)
    {
        Dictionary<int, Item> knapsack = GameRoot.Instance.ActivePlayer.NotCashKnapsack;
        AddItemInKnap(item);
    }

    
    public void AddItemInKnap(Item item)
    {
        Dictionary<int, Item> knapsack = GameRoot.Instance.ActivePlayer.NotCashKnapsack;
        KnapsackWnd.Instance.FindSlot(item.Position).RemoveItemUI();
        if (knapsack.ContainsKey(item.Position))
        {
            knapsack[item.Position].Count += 1;
            KnapsackWnd.Instance.FindSlot(item.Position).StoreItem(item, knapsack[item.Position].Count);
        }
        else
        {
            knapsack[item.Position] = item;
            KnapsackWnd.Instance.FindSlot(item.Position).StoreItem(item, item.Count);
        }
            
    }
    public void ConsumeItem(Item item)
    {
        Dictionary<int, Item> knapsack = GameRoot.Instance.ActivePlayer.NotCashKnapsack;
        KnapsackWnd.Instance.FindSlot(item.Position).RemoveItemUI();
        if (item.Count > 1)
        {
            knapsack[item.Position].Count -= 1;
            KnapsackWnd.Instance.FindSlot(item.Position).StoreItem(item, knapsack[item.Position].Count);
        }
        else
        {
            knapsack.Remove(item.Position);
        }
    }
    public void CancelStrengthen(Item item = null, Item stone = null)
    {
        if(item!=null)
        {
            AddItemInKnap(item);
        }
        if(stone!=null)
        {
            AddItemInKnap(stone);
        }
        EndStrengthen();
        CloseStrengthenWnd();
        KnapsackWnd.Instance.IsForge = false;
        KnapsackWnd.Instance.CloseAndPop();
    }

    public void EndStrengthen()
    {
        AnimaInit();
        ClearPanel();
        EffectText.text = "";
        RegisterStone = null;
        RegisterStrengthenItem = null;
        //StrengthenBtn.interactable()
        CloseBtn1.interactable = true;
        CloseBtn2.interactable = true;
        InfoBtn.interactable = true;
    }

    public void CloseStrengthenWnd()
    {
        UISystem.Instance.CloseStrengthenWnd();
        IsOpen = false;
    }

    public void CostRibi(long ribi)
    {
        GameRoot.Instance.ActivePlayer.Ribi += ribi;
        KnapsackWnd.Instance.RibiTxt.text = GameRoot.Instance.ActivePlayer.Ribi.ToString("N0");
    }

    public void ProessStrengthenReaponse(StrengthenResponse rsp)
    {
        
        switch(rsp.OperationType)
        {
            case 1://強化石放上來要顯示效果和物品
                slotLists[2][0].StoreItem(rsp.strengthenItem, rsp.strengthenItem.Count);
                EffectText.text = rsp.Effect;
                StrengthenBtn.interactable = true;
                break;
            case 2://把原強化石放回背包再顯示效果和物品
                slotLists[2][0].StoreItem(rsp.strengthenItem, rsp.strengthenItem.Count);
                EffectText.text = rsp.Effect;
                AddItemInKnap(rsp.Stone);
                StrengthenBtn.interactable = true;
                break;
            case 3://換一把武器強化
                EffectText.text = "";
                slotLists[1][0].RemoveItemUI();
                slotLists[2][0].RemoveItemUI();
                if(rsp.Stone!=null)
                {
                    AddItemInKnap(rsp.Stone);
                }
                StrengthenBtn.interactable = false;
                break;
            case 4://取消強化
                CancelStrengthen(rsp.item, rsp.Stone);
                break;

            case 5://強化成功
                GetWeapon(rsp.strengthenItem);
                MessageBox.Show("強化成功!!");
                CostRibi(rsp.Ribi);
                EndStrengthen();

                break;
            case 6://強化失敗
                GetWeapon(rsp.item);
                MessageBox.Show("強化失敗QQ");
                CostRibi(rsp.Ribi);
                EndStrengthen();
                break;

        }

    }





}
