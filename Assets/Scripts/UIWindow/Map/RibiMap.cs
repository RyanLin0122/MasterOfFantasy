using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RibiMap : AbstractMap
{
    public override void GoBack()
    {
        print("Using goback");
        transform.parent.parent.GetComponent<MapWnd>().CloseMap();
        Destroy(this.gameObject);
    }

    public override void ShowPosition()
    {
        print("hah");
    }
}
