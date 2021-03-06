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
        if (CurrentDragObject.data is DragItemData) //?p?G?O?????D??
        {
            if (EventSystem.current.IsPointerOverGameObject(-1) == false && !LockerWnd.Instance.IsOpen && !MailBoxWnd.Instance.IsOpen && ((DragItemData)CurrentDragObject.data).Source == 1)
            {
                DragItemData dragItemData = (DragItemData)CurrentDragObject.data;
                Item item = (Item)dragItemData.Content;
                if (dragItemData.Source == 1)  //?????O?I?]
                {
                    //?P?_???q
                    if (item.Count > 1)
                    {
                        //???n?????h??
                        UISystem.Instance.AddMessageQueue("???n???h???b?a?WToDo");
                    }
                    else
                    {
                        //?O?_?i????
                        if (item.Cantransaction)
                        {
                            UISystem.Instance.AddMessageQueue("???b?a?WToDo");
                        }
                        else
                        {
                            MessageBox.Show("?????~???i?????A?T?w?n???????", MessageBoxType.Confirm, () => InventorySys.Instance.DisposeItem());
                        }
                    }
                    //?????????A???^?I?]
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
    /// ?j???R??????????????
    /// </summary>
    public void RemoveDragObject()
    {
        if (CurrentDragObject != null)
        {
            Destroy(CurrentDragObject.gameObject);

        }
        state = DragState.UnDrag;
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
    public int Source; //1: ?I?] 2:???w 
    public virtual void SetData(System.Object data)
    {
        this.Content = data;
    }
}
public class DragItemData : DragBaseData
{
    public int SlotPosition; //Slot??????SlotPosition
    public bool IsCashOnly = false; //Slot?????O?_???w?{???D???A???e?u???I?]????????
    public ItemSlot SourceSlot; //Slot?????A?????_?????????@
    public DragItemData(System.Object item)
    {
        if (item.GetType().IsSubclassOf(typeof(Item)))
        {
            base.SetData(item);
        }
    }
}


//public class DragSkillData : DragBaseData<>
