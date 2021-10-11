using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;

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

    /// <summary>
    /// �j��R���즲��������
    /// </summary>
    public void RemoveDragObject()
    {
        if (CurrentDragObject != null)
        {
            Destroy(CurrentDragObject.gameObject);
            
        }
        state = DragState.UnDrag;
    }

    public void FixedUpdate()
    {
        //��󪫫~
        if (IsPickedItem && Input.GetMouseButtonDown(0) && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1) == false
            && !LockerWnd.Instance.IsOpen && !MailBoxWnd.Instance.IsOpen && ((DragItemData)CurrentDragObject.data).Source == 1)
        {
            RemoveDragObject();
            //�g�R�����~�ʥ]
            List<Item> items = new List<Item>();
            items.Add(GetPickedItem());
            new KnapsackSender(5, items, null, null);
        }
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
    public int Source; //1: �I�] 2:�ܮw 
    public virtual void SetData(System.Object data)
    {
        this.Content = data;
    }
}
public class DragItemData : DragBaseData
{
    public int SlotPosition; //Slot�ӷ���SlotPosition
    public bool IsCashOnly = false; //Slot�ӷ��O�_���w�{���D��A�ثe�u���I�]��������
    public ItemSlot SourceSlot; //Slot�ӷ��A�ΨӴ_��즲�ާ@
    public DragItemData(System.Object item)
    {
        if (item.GetType().IsSubclassOf(typeof(Item)))
        {
            base.SetData(item);
        }
    }
}


//public class DragSkillData : DragBaseData<>
