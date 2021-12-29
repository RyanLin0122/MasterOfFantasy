using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using PEProtocal;
public class ScreenController : MonoBehaviour
{
    [Header("camera")] public Camera MapCamera;
    [Header("offset")] public Vector3 Offset;

    [Header("相機上界")] public float UpBound;
    [Header("相機下界")] public float DownBound;
    [Header("相機左界")] public float LeftBound;
    [Header("相機右界")] public float RightBound;

    public bool canCtrl = true;
    public float cam_x = -1;
    public float cam_y = -1;
    private void Awake()
    {
        MapCamera = Camera.main;
        Offset = MapCamera.transform.position;
        this.background = GameObject.FindGameObjectWithTag("MapBackground").transform;
        float bound_x = background.GetComponent<Renderer>().bounds.size.x;
        float bound_y = background.GetComponent<Renderer>().bounds.size.y;

        Vector3 vector = MapCamera.ScreenToWorldPoint(new Vector2(MapCamera.pixelWidth, MapCamera.pixelHeight)) - MapCamera.ScreenToWorldPoint(Vector2.zero);
        this.cam_x = vector.x;
        this.cam_y = vector.y;
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
        MapRight = background.transform.position.x + background.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        MapLeft = background.transform.position.x - background.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        MapDown = background.transform.position.y - background.GetComponent<SpriteRenderer>().bounds.size.y / 2;
        MapUp = background.transform.position.y + background.GetComponent<SpriteRenderer>().bounds.size.y / 2;
    }
    private void Update()
    {
        float tempx = transform.position.x;
        float tempy = transform.position.y;
        Vector3 tempPosition = new Vector3(Mathf.Clamp(tempx, LeftBound, RightBound), Mathf.Clamp(tempy, DownBound, UpBound), MapCamera.transform.position.z);
        MapCamera.transform.position = tempPosition; //攝影機座標 = 玩家座標
        UpdateBG();
        if (canCtrl)
        {
            if (Input.GetKeyDown(KeyCode.W))
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
            if (Input.GetKeyDown(KeyCode.D))
            {
                DiaryWnd.Instance.KeyBoardCommand();
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                QuestWnd.Instance.KeyBoardCommand();
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

    }
    float MapRight;
    float MapLeft;
    float MapDown;
    float MapUp;
    public Transform SecondBG;
    public Transform ThirdBG;
    public float BG2RightBound = -1;
    public float BG2LeftBound = -1;
    public float BG2UpBound = -1;
    public float BG2DownBound = -1;

    public float BG3RightBound = -1;
    public float BG3LeftBound = -1;
    public float BG3UpBound = -1;
    public float BG3DownBound = -1;

    public Transform background;
    public void LoadBackGround()
    {
        int MapID = GameRoot.Instance.ActivePlayer.MapID;
        MapCfg cfg = null;
        if (ResSvc.Instance.mapCfgDataDic.TryGetValue(MapID, out cfg))
        {
            if (cfg != null)
            {
                string SecondBG = cfg.BG2;
                string ThirdBG = cfg.BG3;
                if (!string.IsNullOrEmpty(SecondBG))
                {
                    this.UpBound += 300;
                    this.SecondBG = (Instantiate(Resources.Load("Prefabs/SecondBG"), this.background.parent) as GameObject).transform;
                    this.SecondBG.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Map/Map Background/" + SecondBG);
                    this.BG2RightBound = background.position.x + background.GetComponent<SpriteRenderer>().bounds.size.x / 2 - this.SecondBG.GetComponent<SpriteRenderer>().bounds.size.x / 2;
                    this.BG2LeftBound = background.position.x - background.GetComponent<SpriteRenderer>().bounds.size.x / 2 + this.SecondBG.GetComponent<SpriteRenderer>().bounds.size.x / 2;
                    this.BG2DownBound = background.position.y - background.GetComponent<SpriteRenderer>().bounds.size.y / 2 + this.SecondBG.GetComponent<SpriteRenderer>().bounds.size.y / 2;
                    this.BG2UpBound = background.position.y + background.GetComponent<SpriteRenderer>().bounds.size.y / 2 - this.SecondBG.GetComponent<SpriteRenderer>().bounds.size.y / 2;
                    if (BG2RightBound < BG2LeftBound)
                    {
                        BG2LeftBound = 0;
                        BG2RightBound = 0;
                    }
                    if (BG2UpBound < BG2DownBound)
                    {
                        BG2UpBound = 0;
                        BG2DownBound = 0;
                    }
                }
                if (!string.IsNullOrEmpty(ThirdBG))
                {
                    this.ThirdBG = (Instantiate(Resources.Load("Prefabs/ThirdBG"), this.background.parent) as GameObject).transform;
                    this.ThirdBG.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Map/Map Background/" + ThirdBG);
                    this.BG3RightBound = background.position.x + background.GetComponent<SpriteRenderer>().bounds.size.x / 2 - this.ThirdBG.GetComponent<SpriteRenderer>().bounds.size.x / 2;
                    this.BG3LeftBound = background.position.x - background.GetComponent<SpriteRenderer>().bounds.size.x / 2 + this.ThirdBG.GetComponent<SpriteRenderer>().bounds.size.x / 2;
                    this.BG3DownBound = background.position.y - background.GetComponent<SpriteRenderer>().bounds.size.y / 2 + this.ThirdBG.GetComponent<SpriteRenderer>().bounds.size.y / 2;
                    this.BG3UpBound = 500 + background.position.y + background.GetComponent<SpriteRenderer>().bounds.size.y / 2 - this.ThirdBG.GetComponent<SpriteRenderer>().bounds.size.y / 2;
                    if (BG3RightBound < BG3LeftBound)
                    {
                        BG3LeftBound = 0;
                        BG3RightBound = 0;
                    }
                    if (BG3UpBound < BG3DownBound)
                    {
                        BG3UpBound = 0;
                        BG3DownBound = 0;
                    }
                }
                UpdateBG();
            }
        }
    }
    public void UpdateBG()
    {
        if (SecondBG != null || ThirdBG != null)
        {

            float RatioX = (transform.position.x - MapLeft - cam_x / 2) / (MapRight - MapLeft - cam_x);
            float RatioY = (transform.position.y - MapDown) / (MapUp - MapDown);

            if (SecondBG != null)
            {
                float CalX = Mathf.Clamp(BG2LeftBound + RatioX * (BG2RightBound - BG2LeftBound), BG2LeftBound, BG2RightBound);
                float X = Mathf.Clamp(CalX, BG2LeftBound, BG2RightBound);
                float CalY = Mathf.Clamp(BG2DownBound + RatioY * (BG2UpBound - BG2DownBound), BG2DownBound, BG2UpBound);
                float Y = Mathf.Clamp(CalY, BG2DownBound, BG2UpBound) + 150f;
                SecondBG.position = new Vector2(X, Y);
                if (transform.position.x < (MapLeft + cam_x / 2)) SecondBG.position = new Vector2(BG2LeftBound, Y);
                if (transform.position.x > (MapRight - cam_x / 2)) SecondBG.position = new Vector2(BG2RightBound, Y);
            }
            if (ThirdBG != null)
            {
                float CalX = Mathf.Clamp(BG3LeftBound + RatioX * (BG3RightBound - BG3LeftBound), BG3LeftBound, BG3RightBound);
                float X = Mathf.Clamp(CalX, BG3LeftBound, BG3RightBound);
                float CalY = BG3DownBound + ThirdBG.GetComponent<SpriteRenderer>().bounds.size.y / 2 + 0.8f * (transform.position.y - BG3DownBound);
                float Y = Mathf.Clamp(CalY, BG3DownBound, BG3UpBound);
                ThirdBG.position = new Vector2(X, Y);
                if (transform.position.x < (MapLeft + cam_x / 2)) ThirdBG.position = new Vector2(BG3LeftBound, Y);
                if (transform.position.x > (MapRight - cam_x / 2)) ThirdBG.position = new Vector2(BG3RightBound, Y);
            }

        }
    }
}
