using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
    public GameObject ExpandMiniMap;
    public GameObject ShrinkMiniMap;
    public Camera MiniMapCamera;
    public Text MapMasterName;
    public Text MapName;
    public Text MapName2;
    public Text LocationName;
    public Text LocationName2;
    public Image Player;
    public int UpdateMiniMapID;


    public void UpdateMiniMap()
    {
        if (GameRoot.Instance.PlayerControl != null && GameRoot.Instance.PlayerControl.gameObject != null)
        {
            Vector3 mainCharacterpos = GameRoot.Instance.PlayerControl.transform.localPosition;
            Player.transform.localPosition = mainCharacterpos / 10.0f;


            //更新怪物位置

            //更新其他人位置


        }
    }

    public void RemoveUpdateMiniMap()
    {
        TimerSvc.Instance.DeleteTimeTask(UpdateMiniMapID);
    }



    public void Init()
    {
        if (GameRoot.Instance.ActivePlayer.MapID < 1000)
        {
            return;
        }
        SetMapName();
        SetMapMasterName();        
        MapInfoContainer MapInfo = GameObject.FindGameObjectWithTag("MapContainer").GetComponent<MapInfoContainer>();
        Vector3 MapPosition = new Vector3(MapInfo.MapBG.transform.position.x, MapInfo.MapBG.transform.position.y, -100f);
        MiniMapCamera.transform.position = MapPosition;
        int TaskID = TimerSvc.Instance.AddTimeTask((id) => { UpdateMiniMap(); }, 100, PETimeUnit.Millisecond, 0);
        UpdateMiniMapID = TaskID;
        //NPC Protal 擺好
    }

    public void ClickDownBtn()
    {
        ExpandMiniMap.SetActive(true);
        ShrinkMiniMap.SetActive(false);
    }
    public void ClickUpBtn()
    {
        ExpandMiniMap.SetActive(false);
        ShrinkMiniMap.SetActive(true);
    }
    public void ClickMapBtn()
    {
        if (!GameRoot.Instance.HasOpenedWnd.ContainsKey("MapWnd"))
        {
            MapWnd wnd = ResSvc.Instance.LoadPrefab(PathDefine.MapWnd, KnapsackWnd.Instance.transform.parent, Vector3.zero, true).GetComponent<MapWnd>();
            wnd.OpenAndPush();
            wnd.transform.localPosition = Vector3.zero;
            GameRoot.Instance.HasOpenedWnd.Add("MapWnd", wnd);
        }
        else
        {
            MapWnd wnd = ((MapWnd)GameRoot.Instance.HasOpenedWnd["MapWnd"]);
            wnd.CloseAndPop();
            GameRoot.Instance.HasOpenedWnd.Remove("MapWnd");
        }
    }
    public void ClickRankBtn()
    {

    }

    public void ClickTopBtn()
    {

    }

    private void SetMapName()
    {
        int MapID = GameRoot.Instance.ActivePlayer.MapID;
        string MapName = ResSvc.Instance.GetMapCfgData(MapID).mapName;
        string Location = ResSvc.Instance.GetMapCfgData(MapID).Location;
        this.MapName.text = MapName;
        this.MapName2.text = MapName;
        this.LocationName.text = Location;
        this.LocationName2.text = Location;
    }

    private void SetMapMasterName()
    {

    }
}
