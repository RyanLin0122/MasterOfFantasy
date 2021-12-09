using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DragInfoSource : DragSourceBase
{
    public override DragObject GenerateDragObject(DragBaseData data, DragMode mode)
    {
        DiaryInformationWnd diaryInformationWnd = transform.parent.parent.GetComponent<DiaryInformationWnd>();
        if (Enabled && diaryInformationWnd != null && diaryInformationWnd.IsExam)
        {
            string[] CurrentData = GetComponent<UIInfoObject>().DragData;
            if (CurrentData == null)
            {
                DragSystem.Instance.state = DragState.UnDrag;
                return null;
            }
            else
            {
                SetData(CurrentData);
            }

            DragInfoObject obj = ((GameObject)Instantiate(Resources.Load("Prefabs/DragInfoObject"))).GetComponent<DragInfoObject>();
            obj.transform.SetParent(DragSystem.Instance.DragContainer.transform);
            obj.transform.position = transform.position;
            obj.transform.localScale = Vector3.one;
            string[] content = (string[])(this.data.Content);
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
        this.data.Content = (string[])data;
    }
}
