using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PEProtocal;
public class SkillDragSource : DragSourceBase
{
    public override DragObject GenerateDragObject(DragBaseData data, DragMode mode)
    {
        DragObject obj = ((GameObject)Instantiate(Resources.Load("DragObjectPrefab1"))).GetComponent<DragObject>();
        
        return obj;
    }
    public override void SetData(object data)
    {
        this.data = new DragItemData((Item)data);
    }
}
