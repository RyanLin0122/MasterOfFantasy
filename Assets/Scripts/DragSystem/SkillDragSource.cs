using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PEProtocal;
public class SkillDragSource : DragSourceBase
{
    public SkillSlot slot;
    public override DragObject GenerateDragObject(DragBaseData data, DragMode mode)
    {
        if (Enabled)
        {
            SkillInfo info = slot.Info;
            if (info == null)
            {
                DragSystem.Instance.state = DragState.UnDrag;
                return null;
            }
            else
            {
                SetData(info);
            }
            AudioSvc.Instance.PlayUIAudio(Constants.LargeBtn);
            DragObject obj = ((GameObject)Instantiate(Resources.Load("Prefabs/ItemDragObject"))).GetComponent<DragObject>();
            obj.transform.SetParent(DragSystem.Instance.DragContainer.transform);
            obj.transform.position = transform.position;
            obj.transform.localScale = Vector3.one;
            obj.transform.GetComponent<Image>().sprite = slot.Info.Icon;
            obj.transform.GetComponent<Image>().SetNativeSize();
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
        this.data = new DragSkillData((SkillInfo)data);
    }
}
