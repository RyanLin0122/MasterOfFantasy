using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class ScreenController : MonoBehaviour
{
    [Header("camera")] public Camera MapCamera;
    [Header("offset")] public Vector3 Offset;

    [Header("相機上界")] public float UpBound;
    [Header("相機下界")] public float DownBound;
    [Header("相機左界")] public float LeftBound;
    [Header("相機右界")] public float RightBound;

    public bool canCtrl = true;

    private void Awake()
    {
        MapCamera = Camera.main;
        Offset = MapCamera.transform.position;
        GameObject background = GameObject.FindGameObjectWithTag("MapBackground");
        float bound_x = background.GetComponent<Renderer>().bounds.size.x;
        float bound_y = background.GetComponent<Renderer>().bounds.size.y;

        Vector3 vector = MapCamera.ScreenToWorldPoint(new Vector2(MapCamera.pixelWidth, MapCamera.pixelHeight)) - MapCamera.ScreenToWorldPoint(Vector2.zero);
        float cam_x = vector.x;
        float cam_y = vector.y;
        Debug.Log(cam_x + ", " + cam_y);

        UpBound = (bound_y - cam_y) / 2.0f;
        DownBound = -UpBound;
        if (UpBound < DownBound)
        {
            UpBound = 0;
            DownBound = 0;
        }

        RightBound = (bound_x - cam_x) / 2.0f;
        LeftBound = -RightBound;
        if (RightBound < LeftBound)
        {
            RightBound = 0;
            LeftBound = 0;

        }

    }
    private void Update()
    {
        float tempx = transform.position.x;
        float tempy = transform.position.y;
        Vector3 tempPosition = new Vector3(Mathf.Clamp(tempx, LeftBound, RightBound), Mathf.Clamp(tempy, DownBound, UpBound), MapCamera.transform.position.z);
        MapCamera.transform.position = tempPosition; //攝影機座標 = 玩家座標
        if (canCtrl)
        {
            if (Input.GetKeyDown(KeyCode.M))
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
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (UISystem.Instance.stack.Count > 0)
                {
                    UISystem.Instance.PressEsc();
                }
                else
                {
                    UISystem.Instance.menuUI.openCloseWnd();
                }

            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                UISystem.Instance.baseUI.AddExp(100000000);
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                UISystem.Instance.InfoWnd.openCloseWnd();
            }
            if (Input.GetKeyDown(KeyCode.F2))
            {
                UISystem.Instance.menuUI.OpenCloseKeyBind();
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                KnapsackWnd.Instance.KeyBoardCommand();
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                UISystem.Instance.OpenCloseOptionWnd();
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                EquipmentWnd.Instance.KeyBoardCommand();
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                UISystem.Instance.OpenLearnSkillUI();
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                DiaryWnd.Instance.KeyBoardCommand();
            }
            if (Input.GetKeyDown(KeyCode.F1))
            {
                GuideWnd.Instance.KeyBoardCommand();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                SkillSys.Instance.skillWnd.KeyBoardCommand();
            }
        }
        if (Input.GetKeyDown(KeyCode.ScrollLock)) //截圖
        {
            Debug.Log("ScreenShot");
            string name = DateTime.Now.ToString("yyyy'_'MM'_'dd'_'HH'_'mm'_'ss");
            string str = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + "/" + name + ".png";
            str = str.Replace("\\", "/");
            try
            {
                Debug.Log(str);
                ScreenCapture.CaptureScreenshot(str);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }

        if (UISystem.Instance.baseUI.Input.InputFieldAvaliable())
        {

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                if (!UISystem.Instance.baseUI.Input.isSelect)
                    UISystem.Instance.baseUI.Input.ActivateChat();
                else UISystem.Instance.baseUI.Input.EndEdit();
            }
        }

        if (Input.GetKeyDown(KeyCode.F11))
        {
            GameRoot.Instance.LogOut();
        }
    }
}
