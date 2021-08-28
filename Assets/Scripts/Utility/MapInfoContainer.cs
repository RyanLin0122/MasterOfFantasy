using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapInfoContainer : MonoBehaviour
{
    public SpriteRenderer MapBG;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log(MapBG.size.x + ", " + MapBG.size.y);
            Debug.Log(MapBG.bounds.size.x + ", " + MapBG.bounds.size.y);
        }
    }
}
