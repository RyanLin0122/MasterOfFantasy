using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;
using UnityEngine.EventSystems;
public class DragSystem : SystemRoot
{
    public static DragSystem Instance = null;
    public static HashSet<DragSourceBase> AllDragSource = new HashSet<DragSourceBase>();
    public static HashSet<DragTargetBase> AllDragTarget = new HashSet<DragTargetBase>();
    public static bool IsPickedItem => Instance != null ? Instance.CheckIsPickedItem() : false;
    public override void InitSys()
    {
        base.InitSys();
        state = DragState.UnDrag;
        Instance = this;
        Debug.Log("Init DragSys...");
    }

    public DragState state = DragState.UnDrag;
    public GameObject DragContainer;
    public DragObject CurrentDragObject;

    public static void DisableAllSources()
    {
        foreach (var sr in AllDragSource)
        {
            sr.Enabled = false;
        }
    }
    public static void DisableAllTargets()
    {
        foreach (var tg in AllDragSource)
        {
            tg.Enabled = false;
        }
    }
    public static void EnableAllSources()
    {
        foreach (var sr in AllDragSource)
        {
            sr.Enabled = true;
        }
    }
    public static void EnableAllTargets()
    {
        foreach (var tg in AllDragSource)
        {
            tg.Enabled = true;
        }
    }

    public bool CheckIsPickedItem()
    {
        if (Instance.CurrentDragObject != null && CurrentDragObject.data is DragItemData &&
            CurrentDragObject.data != null && CurrentDragObject.data.Content != null && CurrentDragObject.data.Content.GetType().IsSubclassOf(typeof(Item)))
        {
            return true;
        }
        return false;
    }
    public Item GetPickedItem()
    {
        if (Instance.CurrentDragObject != null && CurrentDragObject.data is DragItemData &&
            CurrentDragObject.data != null && CurrentDragObject.data.Content != null && CurrentDragObject.data.Content.GetType().IsSubclassOf(typeof(Item)))
        {
            return (Item)CurrentDragObject.data.Content;
        }
        return null;
    }

    public void DisposeDragObject()
    {
        if (CurrentDragObject.data is DragItemData) //如果是拖曳道具
        {
            if (EventSystem.current.IsPointerOverGameObject(-1) == false && !LockerWnd.Instance.IsOpen && !MailBoxWnd.Instance.IsOpen && ((DragItemData)CurrentDragObject.data).Source == 1)
            {
                DragItemData dragItemData = (DragItemData)CurrentDragObject.data;
                Item item = (Item)dragItemData.Content;
                if (dragItemData.Source == 1)  //來源是背包
                {
                    //判斷數量
                    if (item.Count > 1)
                    {
                        //問要丟棄多少
                        UISystem.Instance.AddMessageQueue("問要丟多少在地上ToDo");
                    }
                    else
                    {
                        //是否可交易
                        if (item.Cantransaction)
                        {
                            UISystem.Instance.AddMessageQueue("丟在地上ToDo");
                        }
                        else
                        {
                            MessageBox.Show("此物品不可交易，確定要丟棄嗎?", MessageBoxType.Confirm, () => InventorySys.Instance.DisposeItem());
                        }
                    }
                    //取消拖曳，放回背包
                    if (item.IsCash)
                    {
                        KnapsackWnd.Instance.FindCashSlot(item.Position).StoreItem(item, item.Count);
                    }
                    else
                    {
                        KnapsackWnd.Instance.FindCashSlot(item.Position).StoreItem(item, item.Count);
                    }
                    RemoveDragObject();
                }

            }
        }

    }

    /// <summary>
    /// 強制刪除拖曳中的物體
    /// </summary>
    public void RemoveDragObject()
    {
        if (CurrentDragObject != null)
        {
            Destroy(CurrentDragObject.gameObject);

        }
        state = DragState.UnDrag;
    }
    /// <summary>
    /// 把手上物品還原回去原本地方
    /// </summary>
    public void ReturnDragItem()
    {
        print("Return DragObject");
        if (CurrentDragObject != null)
        {
            DragBaseData data = CurrentDragObject.data;
            if(data is DragItemData)
            {
                Item item = (Item)data.Content;
                switch (data.Source)
                {
                    case 1:
                        if (item.IsCash)
                        {
                            KnapsackWnd.Instance.FindCashSlot(item.Position).StoreItem(item, item.Count);
                        }
                        else
                        {
                            KnapsackWnd.Instance.FindSlot(item.Position).StoreItem(item, item.Count);
                        }
                        break;
                    case 2:
                        LockerWnd.Instance.FindSlot(item.Position).StoreItem(item, item.Count);
                        break;
                    case 3:
                        MailBoxWnd.Instance.FindSlot(item.Position).StoreItem(item, item.Count);
                        break;
                }
            }
            else if(data is DragSkillData)
            {

            }
            else if(data is DragHotKeyData)
            {

            }
            else
            {

            }
        }
        RemoveDragObject();
    }
}
public enum DragState
{
    Dragging,
    UnDrag
}
public enum DragObjectType
{
    Consumable,
    Skill,
    DiaryItem
}

public enum DragMode
{
    MustPointerUp,
    DragImmediately
}
public class DragBaseData
{
    public System.Object Content;
    public int Source; //1: 背包 2:倉庫 
    public virtual void SetData(System.Object data)
    {
        this.Content = data;
    }
}
public class DragItemData : DragBaseData
{
    public int SlotPosition; //Slot來源的SlotPosition
    public bool IsCashOnly = false; //Slot來源是否限定現金道具，目前只有背包有此分類
    public ItemSlot SourceSlot; //Slot來源，用來復原拖曳操作
    public DragItemData(System.Object item)
    {
        if (item.GetType().IsSubclassOf(typeof(Item)))
        {
            base.SetData(item);
        }
    }
}


public class DragSkillData : DragBaseData
{
    public DragSkillData(System.Object item)
    {
        if (item.GetType().IsSubclassOf(typeof(SkillInfo)))
        {
            base.SetData(item);
        }
    }
}

public class DragHotKeyData : DragBaseData
{
    public DragHotKeyData(System.Object item)
    {
        if (item.GetType().IsSubclassOf(typeof(HotkeyData)))
        {
            base.SetData(item);
        }
    }
}
