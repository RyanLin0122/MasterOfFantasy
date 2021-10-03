using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class ScreenController : MonoBehaviour
{
    [Header("camera")] public new Camera camera;
    [Header("offset")] public Vector3 Offset;

    [Header("相機上界")] public float UpBound;
    [Header("相機下界")] public float DownBound;
    [Header("相機左界")] public float LeftBound;
    [Header("相機右界")] public float RightBound;
    
    public bool canCtrl = true;
 
    private void Awake()
    {
        Scene scene = SceneManager.GetActiveScene();
        GameObject background = GameObject.Find("BG");
        //print(background.name);
        float bound_x = background.GetComponent<Renderer>().bounds.size.x;
        float bound_y = background.GetComponent<Renderer>().bounds.size.y;

        print("Hi");
        //print(bound_x);
        int[] mapBound = Tools.GetMapBoundary(scene.name);

        float cam_x = 535.0f;
        float cam_y = 300.0f;

        

        UpBound = bound_y / 2.0f - cam_y;
        DownBound = -UpBound;
        if (UpBound < DownBound)
        {
            UpBound = 0;
            DownBound = 0;
        }

        RightBound = bound_x / 2.0f - cam_x;
        LeftBound = -RightBound;
        if(RightBound < LeftBound)
        {
            RightBound = 0;
            LeftBound = 0;

        }

    }
    // Use this for initialization
    void Start()
    {
        camera = Camera.main;
        Offset = camera.transform.position;
        //Offset = camera.transform.position - transform.position;
        //相對位移 = 攝影機座標-玩家座標
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        float tempx = transform.position.x; //+ Offset.x;

        float tempy = transform.position.y; //+ Offset.y;
        Vector3 tempPosition = new Vector3(Mathf.Clamp(tempx, LeftBound, RightBound),
                                          Mathf.Clamp(tempy, DownBound, UpBound), camera.transform.position.z);
        camera.transform.position = tempPosition;//+ new Vector3(Offset.x,Offset.y,0);
                                                 //攝影機座標 = 玩家座標+相對位移
    }


    private void Update()
    {
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

            if (Input.GetKeyDown(KeyCode.J))
            {
                print(MessageBox.IsMessageBox);
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                UISystem.Instance.baseUI.AddExp(100000000);
            }

            //if (MainCitySys.Instance.shopWnd.gameObject.activeSelf == false && MainCitySys.Instance.menuUI.gameObject.activeSelf == false&&
            //    MainCitySys.Instance.dialogueWnd.gameObject.activeSelf==false&& MainCitySys.Instance.MailBoxWnd.gameObject.activeSelf == false&&
            //    MainCitySys.Instance.lockerWnd.gameObject.activeSelf == false)
            {

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
                if (Input.GetKeyDown(KeyCode.F1))
                {
                    GuideWnd.Instance.KeyBoardCommand();
                }
                if (Input.GetKeyDown(KeyCode.K))
                {
                    SkillSys.Instance.skillWnd.KeyBoardCommand();
                }
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    GetComponent<MainPlayerCtrl>().PlayHurt();
                }
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    GetComponent<MainPlayerCtrl>().PlayDeath();
                }
                if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    GetComponent<MainPlayerCtrl>().PlayDown1();
                }
                if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    GetComponent<MainPlayerCtrl>().PlayDown2();
                }
                if (Input.GetKeyDown(KeyCode.Alpha5))
                {
                    GetComponent<MainPlayerCtrl>().PlayHorizon1();
                }
                if (Input.GetKeyDown(KeyCode.Alpha6))
                {
                    GetComponent<MainPlayerCtrl>().PlayHorizon2();
                }
                if (Input.GetKeyDown(KeyCode.Alpha7))
                {
                    GetComponent<MainPlayerCtrl>().PlayMagic();
                }
                if (Input.GetKeyDown(KeyCode.Alpha8))
                {
                    GetComponent<MainPlayerCtrl>().PlayCleric();
                }
                if (Input.GetKeyDown(KeyCode.Alpha9))
                {
                    GetComponent<MainPlayerCtrl>().PlayCrossbow();
                }
                if (Input.GetKeyDown(KeyCode.Alpha0))
                {
                    GetComponent<MainPlayerCtrl>().PlayBow();
                }
                if (Input.GetKeyDown(KeyCode.B))
                {
                    GetComponent<MainPlayerCtrl>().PlaySlash();
                }
                if (Input.GetKeyDown(KeyCode.N))
                {
                    GetComponent<MainPlayerCtrl>().PlayUpper();
                }
                if (Input.GetKeyDown(KeyCode.V))
                {
                    UISystem.Instance.AddMessageQueue(Time.realtimeSinceStartup.ToString());
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Home)) //截圖
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
