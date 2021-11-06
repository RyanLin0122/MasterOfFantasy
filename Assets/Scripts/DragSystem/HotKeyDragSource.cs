using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;
using UnityEngine.UI;
public class HotKeyDragSource : DragSourceBase
{
    public HotKeySlot slot;
    public override DragObject GenerateDragObject(DragBaseData data, DragMode mode)
    {
        if (Enabled)
        {
            HotkeyData hotkey = slot.data;
            if (hotkey == null)
            {
                DragSystem.Instance.state = DragState.UnDrag;
                return null;
            }
            else
            {
                SetData(hotkey);
            }
            AudioSvc.Instance.PlayUIAudio(Constants.LargeBtn);
            DragObject obj = ((GameObject)Instantiate(Resources.Load("Prefabs/ItemDragObject"))).GetComponent<DragObject>();
            obj.transform.SetParent(DragSystem.Instance.DragContainer.transform);
            obj.transform.position = transform.position;
            obj.transform.localScale = Vector3.one;
            HotkeyData content = (HotkeyData)(this.data.Content);
            if(content.HotKeyState == 2)
            {
                obj.transform.GetComponent<Image>().sprite = Resources.Load<Sprite>(ResSvc.Instance.SkillDic[content.ID].Icon);
                obj.transform.GetComponent<Image>().SetNativeSize();
            }
            else if(content.HotKeyState == 1)
            {
                obj.transform.GetComponent<Image>().sprite = Resources.Load<Sprite>(InventorySys.Instance.itemList[content.ID].Sprite);
                obj.transform.GetComponent<Image>().SetNativeSize();
            }           
            obj.data = this.data;
            obj.mode = mode;
            return obj;
        }
        else
        {
            DragSystem.Instance.state = DragState.UnDrag;
            return null;
        }

    }
    public override void SetData(object data)
    {
        this.data = new DragHotKeyData(data);
    }

}
