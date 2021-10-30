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
        if (CurrentDragObject.data is DragItemData) //�p�G�O�즲�D��
        {
            if (EventSystem.current.IsPointerOverGameObject(-1) == false && !LockerWnd.Instance.IsOpen && !MailBoxWnd.Instance.IsOpen && ((DragItemData)CurrentDragObject.data).Source == 1)
            {
                DragItemData dragItemData = (DragItemData)CurrentDragObject.data;
                Item item = (Item)dragItemData.Content;
                if (dragItemData.Source == 1)  //�ӷ��O�I�]
                {
                    //�P�_�ƶq
                    if (item.Count > 1)
                    {
                        //�ݭn���h��
                        UISystem.Instance.AddMessageQueue("�ݭn��h�֦b�a�WToDo");
                    }
                    else
                    {
                        //�O�_�i���
                        if (item.Cantransaction)
                        {
                            UISystem.Instance.AddMessageQueue("��b�a�WToDo");
                        }
                        else
                        {
                            MessageBox.Show("�����~���i����A�T�w�n����?", MessageBoxType.Confirm, () => InventorySys.Instance.DisposeItem());
                        }
                    }
                    //�����즲�A��^�I�]
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
    /// <summary>
    /// ���W���~�٭�^�h�쥻�a��
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
