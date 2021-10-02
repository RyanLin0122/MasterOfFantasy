using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IMap
{
    void GoBack();
    void ShowPosition();
}
public class MapWnd : WindowRoot, IStackWnd
{
    Stack<IMap> MapStack = new Stack<IMap>();
    IMap CurrentMap = null;

    public GameObject btns;
    public Transform Maps;

    public void Init()
    {

    }

    public void OpenNewMap(string s)
    {
        AbstractMap map = null;
        switch (s)
        {
            case "Ribi":
                Debug.Log("Press Ribi");
                map = ResSvc.Instance.LoadPrefab(PathDefine.RibiMap, Maps.transform, Vector3.zero, true).GetComponent<RibiMap>();
                break;
            case "Arnos":
                Debug.Log("Press Arnos");
                map = ResSvc.Instance.LoadPrefab(PathDefine.ArnosMap, Maps.transform, Vector3.zero, true).GetComponent<ArnosMap>();
                break;
            case "Aqua":
                Debug.Log("Press Aqua");
                //map = ResSvc.Instance.LoadPrefab(PathDefine.AquaMap, Maps.transform, Vector3.zero, true).GetComponent<RibiMap>();
                break;
            case "Dokkabi":
                Debug.Log("Press Dokkabi");
                //map = ResSvc.Instance.LoadPrefab(PathDefine.DokkabiMap, Maps.transform, Vector3.zero, true).GetComponent<RibiMap>();
                break;
            case "Castle":
                Debug.Log("Press Castle");
                //map = ResSvc.Instance.LoadPrefab(PathDefine.CastleMap, Maps.transform, Vector3.zero, true).GetComponent<RibiMap>();
                break;
            case "Frost":
                Debug.Log("Press Frost");
                //map = ResSvc.Instance.LoadPrefab(PathDefine.FrostMap, Maps.transform, Vector3.zero, true).GetComponent<RibiMap>();
                break;
            case "RundWell":
                Debug.Log("Press RundWell");
                //map = ResSvc.Instance.LoadPrefab(PathDefine.RundWellMap, Maps.transform, Vector3.zero, true).GetComponent<RibiMap>();
                break;
            case "Poseidos":
                Debug.Log("Press Poseidos");
                //map = ResSvc.Instance.LoadPrefab(PathDefine.PoseidoMap, Maps.transform, Vector3.zero, true).GetComponent<RibiMap>();
                break;
            default:
                break;
        }
        btns.SetActive(false);
        PushMap(map);
        map.transform.localPosition = Vector3.zero;
        Button[] AllBtns = map.GetComponentsInChildren<Button>();
        foreach (var btn in AllBtns)
        {
            if (btn.name == "CloseBtn")
            {
                btn.onClick.AddListener(() => { GameObject.DestroyImmediate(map.gameObject); btns.SetActive(true); });
            }
        }
        map.ShowPosition();

    }

    public void CloseMap()
    {
        print("Close a map");
        if (MapStack.Count > 0)
        {
            MapStack.Pop();
        }
        if (MapStack.Count > 0)
        {
            CurrentMap = MapStack.Peek();
        }
        else
        {
            CloseWnd();
        }
    }

    public void ClickCloseBtn()
    {
        MapWnd wnd = ((MapWnd)GameRoot.Instance.HasOpenedWnd["MapWnd"]);
        CloseAndPop();
    }
    public void PushMap(AbstractMap map)
    {
        MapStack.Push(map);
    }

    public void PopMap()
    {
        MapStack.Pop();
    }

    public void CloseWnd()
    {
        Destroy(this.gameObject);
    }

    public void DisableBtns()
    {
        if (btns != null)
        {
            btns.SetActive(false);
        }
    }
    public void EnableBtns()
    {
        if (btns != null)
        {
            btns.SetActive(true);
        }
    }

    public bool IsOpen = true;
    public void OpenAndPush()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.WindowOpen);
        SetWndState();
        IsOpen = true;
        UISystem.Instance.Push(this);
    }

    public void CloseAndPop()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.WindowClose);
        InventorySys.Instance.HideToolTip();
        UISystem.Instance.ForcePop(this);
        MapWnd wnd = ((MapWnd)GameRoot.Instance.HasOpenedWnd["MapWnd"]);
        GameRoot.Instance.HasOpenedWnd.Remove("MapWnd");
        GameObject.DestroyImmediate(wnd.gameObject);
    }
}

public abstract class AbstractMap : MonoBehaviour, IMap
{
    public string MapName { get; set; }
    public abstract void GoBack();
    public abstract void ShowPosition();
}


