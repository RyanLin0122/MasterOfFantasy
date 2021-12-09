using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragInfoObject : DragObject
{
    public Text IDText;
    public Text NameText;
    public Text RegionText;

    public override void SetDragData(DragBaseData data, DragMode mode)
    {
        base.SetDragData(data, mode);
        string[] Infos = (string[])data.Content;
        this.IDText.text = Infos[0];
        this.NameText.text = Infos[1];
        this.RegionText.text = Infos[2];
    }

}
