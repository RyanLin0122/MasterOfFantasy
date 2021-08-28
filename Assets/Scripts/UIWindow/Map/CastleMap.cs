using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleMap : AbstractMap
{
    public override void GoBack()
    {
        transform.parent.parent.GetComponent<MapWnd>().CloseMap();
        Destroy(this.gameObject);
    }

    public override void ShowPosition()
    {
        print("hah");
    }
}
