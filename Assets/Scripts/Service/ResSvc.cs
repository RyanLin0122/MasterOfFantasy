using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;
using PEProtocal;

public class ResSvc : MonoBehaviour
{
    public static ResSvc Instance = null;

    public void InitSvc()
    {
        Instance = this;
        InitStrings();
        InitMapCfg(PathDefine.MapCfg);
        InitNpcCfg();
        InitShopInfo(PathDefine.NpcShop);
        InitEquipmentPath(PathDefine.EquipmentPath);
        InventorySys.Instance.ParseItemJson();
        ParseMonsterJson();
        ParseSkillJson();
        IllustrationPath.Instance.Init();
        LoadJobImgs();
        ParsePortalJson();
        ParseTitle();
        ParseCashShopInfo(PathDefine.CashShopInfoPath);
        NameBoxDic = ParseNameBoxJson();
        Debug.Log("Init ResSvc...");
    }
    private Action prgCB = null;

    //異步加載場景
    public void AsyncLoadScene(string sceneName, Action loaded)
    {
        try
        {
            GameRoot.Instance.loadingWnd.SetWndState();
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
            throw;
        }
        try
        {
            print(sceneName);
            AsyncOperation sceneAsync = SceneManager.LoadSceneAsync(sceneName);
            prgCB = () =>
            {
                float val = sceneAsync.progress;
                GameRoot.Instance.loadingWnd.SetProgress(val);
                if (val == 1)
                {
                    if (loaded != null)
                    {
                        loaded();
                    }
                    prgCB = null;
                    sceneAsync = null;
                    GameRoot.Instance.loadingWnd.Duration();
                }
            };
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
            throw;
        }


    }

    private void Update()
    {
        if (prgCB != null)
        {
            prgCB();
        }
    }
    public GameObject LoadPrefab(string path, bool cache = false)
    {
        GameObject prefab = null;
        if (!goDic.TryGetValue(path, out prefab))
        {
            prefab = Resources.Load<GameObject>(path);
            if (cache)
            {
                goDic.Add(path, prefab);

            }
        }
        Debug.Log(SceneManager.GetActiveScene().name);
        GameObject go = null;
        if (prefab != null)
        {
            go = Instantiate(prefab);
            go.transform.SetParent(GameObject.Find("Canvas2").transform);
            //go.transform.SetAsFirstSibling();
            go.transform.SetAsLastSibling();

        }
        return go;
    }
    public GameObject LoadPrefab(string path, Transform parent, Vector3 position, bool cache = false, bool IsLocal = false)
    {
        GameObject prefab = null;
        if (!goDic.TryGetValue(path, out prefab))
        {
            prefab = Resources.Load<GameObject>(path);
            if (cache)
            {
                goDic.Add(path, prefab);

            }
        }
        Debug.Log(SceneManager.GetActiveScene().name);
        GameObject go = null;
        if (prefab != null)
        {
            if (!IsLocal)
            {
                go = Instantiate(prefab, position, Quaternion.identity, parent);
                go.transform.SetAsFirstSibling();
            }
            else
            {
                go = Instantiate(prefab, position, Quaternion.identity, parent);
                go.transform.SetAsFirstSibling();
            }

        }
        return go;
    }
    public GameObject LoadPrefab_NotFirstSibling(string path, Transform parent, Vector3 position, bool cache = false, bool IsLocal = false)
    {
        GameObject prefab = null;
        if (!goDic.TryGetValue(path, out prefab))
        {
            prefab = Resources.Load<GameObject>(path);
            if (cache)
            {
                goDic.Add(path, prefab);

            }
        }
        GameObject go = null;
        if (prefab != null)
        {
            if (!IsLocal)
            {
                go = Instantiate(prefab, position, Quaternion.identity, parent);
            }
            else
            {
                go = Instantiate(prefab, position, Quaternion.identity, parent);
            }

        }
        return go;
    }
    private Dictionary<string, GameObject> goDic = new Dictionary<string, GameObject>();
    private Dictionary<string, AudioClip> adDic = new Dictionary<string, AudioClip>();
    public AudioClip LoadAudio(string path, bool cache = false)
    {
        AudioClip au = null;
        if (!adDic.TryGetValue(path, out au))
        {
            au = Resources.Load<AudioClip>(path);
            if (cache)
            {
                adDic.Add(path, au);
            }
        }
        return au;
    }
    private Dictionary<string, Sprite> spDic = new Dictionary<string, Sprite>();
    public Sprite LoadSprite(string path, bool cache = false)
    {
        Sprite sp = null;
        if (!spDic.TryGetValue(path, out sp))
        {
            sp = Resources.Load<Sprite>(path);
            if (cache)
            {
                spDic.Add(path, sp);
            }
        }
        return sp;
    }
    public Sprite[] JobImgs;

    public void LoadJobImgs()
    {
        JobImgs = Resources.LoadAll<Sprite>("UI/UISprites/JobImage");
    }
    public Sprite GetJobImgByID(int ID)
    {
        switch (ID)
        {
            default:
                break;
            case 1:
                return JobImgs[0];
            case 2:
                return JobImgs[1];
            case 3:
                return JobImgs[2];
            case 4:
                return JobImgs[3];
            case 101:
                return JobImgs[5];
            case 102:
                return JobImgs[6];
            case 103:
                return JobImgs[28];
            case 104:
                return JobImgs[7];
            case 201:
                return JobImgs[9];
            case 202:
                return JobImgs[14];
            case 203:
                return JobImgs[10];
            case 204:
                return JobImgs[15];
            case 205:
                return JobImgs[16];
            case 206:
                return JobImgs[11];
            case 207:
                return JobImgs[12];
            case 208:
                return JobImgs[17];
            case 301:
                return JobImgs[4];
            case 302:
                return JobImgs[8];
            case 303:
                return JobImgs[18];
            case 304:
                return JobImgs[19];
            case 305:
                return JobImgs[13];
            case 306:
                return JobImgs[29];
            case 307:
                return JobImgs[20];
            case 308:
                return JobImgs[21];
            case 401:
                return JobImgs[23];
            case 402:
                return JobImgs[22];
            case 403:
                return JobImgs[31];
            case 404:
                return JobImgs[27];
            case 405:
                return JobImgs[26];
            case 406:
                return JobImgs[25];
            case 407:
                return JobImgs[24];
            case 408:
                return JobImgs[30];
        }
        return null;
    }
    #region Map
    private Dictionary<int, MapCfg> mapCfgDataDic = new Dictionary<int, MapCfg>();
    private void InitMapCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {

        }
        else
        {

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);

            XmlNodeList nodLst = doc.SelectSingleNode("root").ChildNodes;

            for (int i = 0; i < nodLst.Count; i++)
            {

                XmlElement ele = nodLst[i] as XmlElement;

                if (ele.GetAttributeNode("ID") == null)
                {

                    continue;
                }
                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
                MapCfg mc = new MapCfg
                {
                    ID = ID
                };

                foreach (XmlElement e in nodLst[i].ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "MapName":
                            mc.mapName = e.InnerText;

                            break;
                        case "Location":
                            mc.Location = e.InnerText;

                            break;
                        case "SceneName":
                            mc.SceneName = e.InnerText;

                            break;
                        case "PlayerBornPos":
                            {
                                string[] valArr = e.InnerText.Split(',');

                                mc.PlayerBornPos = new float[] { float.Parse(valArr[0]), float.Parse(valArr[1])};
                            }
                            break;
                        case "Islimited":
                            {
                                if (e.InnerText == "0")
                                {
                                    mc.Islimited = false;
                                }
                                else
                                {
                                    mc.Islimited = true;
                                }
                            }
                            break;
                        case "IsVillage":
                            {
                                if (e.InnerText == "0")
                                {
                                    mc.Islimited = false;
                                }
                                else
                                {
                                    mc.Islimited = true;
                                }
                            }
                            break;
                        case "MonsterMax":
                            {
                                mc.MonsterMax = Convert.ToInt32(e.InnerText);

                            }
                            break;
                        case "BornTime":
                            {
                                mc.BornTime = Convert.ToInt32(e.InnerText);

                            }
                            break;

                    }
                }
                mapCfgDataDic.Add(ID, mc);
            }
        }
    }
    public MapCfg GetMapCfgData(int id)
    {
        MapCfg data;
        if (mapCfgDataDic.TryGetValue(id, out data))
        {
            return data;
        }
        return null;
    }
    #endregion

    private Dictionary<int, NpcConfig> NpcCfgDataDic = new Dictionary<int, NpcConfig>();
    private void InitNpcCfg()
    {
        TextAsset xml = null;
        if (GameRoot.Instance.AccountOption != null)
        {
            switch (GameRoot.Instance.AccountOption.Language)
            {
                case 0:
                    xml = Resources.Load<TextAsset>("ResCfgs/NpcInfo_TraChinese");
                    break;
                case 1:
                    xml = Resources.Load<TextAsset>("ResCfgs/NpcInfo_SimChinese");
                    break;
                case 2:
                    xml = Resources.Load<TextAsset>("ResCfgs/NpcInfo_English");
                    break;
                case 3:
                    xml = Resources.Load<TextAsset>("ResCfgs/NpcInfo_Korean");
                    break;
                default:
                    break;
            }
        }
        else
        {
            xml = Resources.Load<TextAsset>("ResCfgs/NpcInfo_TraChinese");
        }
        if (!xml)
        {
            return;
        }
        else
        {

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);

            XmlNodeList nodLst = doc.SelectSingleNode("root").ChildNodes;

            for (int i = 0; i < nodLst.Count; i++)
            {

                XmlElement ele = nodLst[i] as XmlElement;

                if (ele.GetAttributeNode("ID") == null)
                {
                    continue;
                }
                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
                NpcConfig NC = new NpcConfig
                {
                    ID = ID
                };

                foreach (XmlElement e in nodLst[i].ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "NpcName":
                            NC.Name = e.InnerText;

                            break;

                        case "Sprite":
                            NC.Sprite = e.InnerText;

                            break;
                        case "Function":

                            string[] valArr = e.InnerText.Split(',');
                            NC.Functions = new List<int>();
                            foreach (var item in valArr)
                            {
                                NC.Functions.Add(Convert.ToInt32(item));
                            }
                            break;
                        case "FixedText":
                            NC.FixedText = e.InnerText.Split(',');
                            break;
                    }
                }
                NpcCfgDataDic.Add(ID, NC);
            }
        }
    }
    public NpcConfig GetNpcCfgData(int id)
    {
        NpcConfig data;
        if (NpcCfgDataDic.TryGetValue(id, out data))
        {
            return data;
        }
        return null;
    }

    #region NPCShopInfo
    public Dictionary<int, NPCShopInfo> NPCShopInfoDic = new Dictionary<int, NPCShopInfo>();
    private void InitShopInfo(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {

        }
        else
        {

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);

            XmlNodeList nodLst = doc.SelectSingleNode("root").ChildNodes;

            for (int i = 0; i < nodLst.Count; i++)
            {

                XmlElement ele = nodLst[i] as XmlElement;

                if (ele.GetAttributeNode("ID") == null)
                {
                    continue;
                }
                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
                NPCShopInfo npcShopInfo = new NPCShopInfo();
                npcShopInfo.NPCID = ID;
                foreach (XmlElement e in nodLst[i].ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "SellItemID":
                            string[] valArr = e.InnerText.Split(',');
                            foreach (var item in valArr)
                            {
                                if (Convert.ToInt32(item) > 1000 && Convert.ToInt32(item) < 3000)
                                {
                                    npcShopInfo.SellConsumables.Add(Convert.ToInt32(item));
                                }
                                else if (Convert.ToInt32(item) > 3000 && Convert.ToInt32(item) < 8000)
                                {
                                    npcShopInfo.SellEquipments.Add(Convert.ToInt32(item));
                                }
                                else if (Convert.ToInt32(item) > 8000 && Convert.ToInt32(item) < 10000)
                                {
                                    npcShopInfo.SellWeapons.Add(Convert.ToInt32(item));
                                }
                                else if (Convert.ToInt32(item) > 10000 && Convert.ToInt32(item) < 12000)
                                {
                                    npcShopInfo.SellMaterials.Add(Convert.ToInt32(item));
                                }
                                else if (Convert.ToInt32(item) > 12000 && Convert.ToInt32(item) < 17000)
                                {
                                    npcShopInfo.SellETCItems.Add(Convert.ToInt32(item));
                                }
                                else if (Convert.ToInt32(item) > 17000 && Convert.ToInt32(item) < 20000)
                                {
                                    npcShopInfo.SellBadges.Add(Convert.ToInt32(item));
                                }
                            }
                            break;
                        case "SellItemType":

                            string[] Arr = e.InnerText.Split(',');

                            foreach (var item in Arr)
                            {
                                npcShopInfo.SellType.Add(Convert.ToInt32(item));
                            }
                            break;

                    }
                }
                NPCShopInfoDic.Add(ID, npcShopInfo);

            }
        }
    }
    private Dictionary<int, string[]> EquipmentPath = new Dictionary<int, string[]>();
    private void InitEquipmentPath(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
        }
        else
        {

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);
            XmlNodeList nodLst = doc.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < nodLst.Count; i++)
            {
                XmlElement ele = nodLst[i] as XmlElement;
                if (ele.GetAttributeNode("ID") == null)
                {
                    continue;
                }
                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
                string[] TempPath = new string[] { };
                foreach (XmlElement e in nodLst[i].ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "Path":
                            TempPath = e.InnerText.Split(',');
                            break;
                    }
                }
                EquipmentPath.Add(ID, TempPath);
            }
        }
    }
    public string[] GetEquipSpritePath(int id)
    {
        if (EquipmentPath.ContainsKey(id))
        {
            if (EquipmentPath[id].Length == 1)
            {
                string[] s = new string[] { "" };
                s[0] = "Character/CharacterSprites/" + (EquipmentPath[id])[0];
                return s;
            }
            else if (EquipmentPath[id].Length == 2)
            {
                string[] s = new string[] { "", "" };
                s[0] = "Character/CharacterSprites/" + (EquipmentPath[id])[0];
                s[1] = "Character/CharacterSprites/" + (EquipmentPath[id])[1];
                return s;
            }


        }
        return new string[] { };
    }
    public NPCShopInfo GetNpcShopInfo(int id)
    {
        NPCShopInfo data;
        if (NPCShopInfoDic.TryGetValue(id, out data))
        {
            return data;
        }
        return null;
    }
    #endregion

    #region CashShopInfo
    public Dictionary<string, Dictionary<string, List<CashShopData>>> CashShopDic { get; set; }
    public void ParseCashShopInfo(string path)
    {
        Dictionary<string, Dictionary<string, List<CashShopData>>> cashShopDic = new Dictionary<string, Dictionary<string, List<CashShopData>>>();
        //Unity裡的文本是TextAsset類
        TextAsset itemText = Resources.Load<TextAsset>("ResCfgs/CashShopInfo");
        string CashShopJson = itemText.text;//物品信息的格式
        JSONObject jo = new JSONObject(CashShopJson);

        foreach (var cata in jo.keys)
        {
            Dictionary<string, List<CashShopData>> Catagories = new Dictionary<string, List<CashShopData>>();
            //第一層 大分類 
            foreach (var key in jo[cata].keys)
            {
                List<CashShopData> ItemList = new List<CashShopData>();
                //第二層 小分類
                var list = jo[cata][key].list;
                for (int i = 0; i < list.Count; i++)
                {
                    //第三層 商品
                    CashShopData data = new CashShopData
                    {
                        ItemID = (int)list[i]["ID"].n,
                        SellPrice = (int)list[i]["SellPrice"].n,
                        Quantity = (int)list[i]["Quantity"].n
                    };
                    ItemList.Add(data);
                }
                Catagories.Add(key, ItemList);
            }
            cashShopDic.Add(cata, Catagories);
        }
        this.CashShopDic = cashShopDic;
    }

    #endregion

    #region 稱號區
    public Dictionary<int, TitleData> TitleDic = new Dictionary<int, TitleData>();
    public void ParseTitle()
    {
        TextAsset itemText = Resources.Load<TextAsset>("ResCfgs/TitleInfo");
        string TitleJson = itemText.text;//物品信息的格式
        JSONObject j = new JSONObject(TitleJson);
        foreach (JSONObject jo in j.list)
        {
            //先解析公有的屬性
            int TitleID = (int)(jo["ID"].n);
            TitleData td = new TitleData
            {
                ID = TitleID,
                Tra_ChineseName = jo["TraChineseName"].str,
                Sim_ChineseName = jo["SimChineseName"].str,
                English = jo["EnglishName"].str,
                Korean = jo["KoreanName"].str
            };
            TitleDic.Add(TitleID, td);
        }
    }
    #endregion

    #region 技能區
    public Dictionary<int, List<SkillInfo>> SkillDic = new Dictionary<int, List<SkillInfo>>();
    //int: 職業代碼 
    private void ParseSkillJson()
    {
        //Unity裡的文本是TextAsset類
        TextAsset itemText = Resources.Load<TextAsset>("ResCfgs/SkillInfo");
        string SkillJson = itemText.text;//物品信息的格式
        JSONObject j = new JSONObject(SkillJson);
        foreach (JSONObject jo in j.list)
        {
            //先解析公有的屬性
            int JobID = (int)(jo["JobID"].n);
            JSONObject Skills = jo["Skills"];

            List<SkillInfo> Infos = new List<SkillInfo>();
            foreach (var key in Skills.keys)
            {
                string Name = (Skills[key])["Name"].str;
                int ID = (int)(Skills[key])["ID"].n;
                bool IsActive = (Skills[key])["IsActive"].b;
                string Des = (Skills[key])["Des"].str;
                bool IsSpecified = false;
                Sprite Icon = Resources.Load<Sprite>("Effect/SkillIcon/" + (Skills[key])["Icon"].str);
                if (IsActive) //是主動技
                {
                    IsSpecified = (Skills[key])["IsSpecified"].b;
                    SkillInfo info = new SkillInfo
                    {
                        SkillName = Name,
                        SkillID = ID,
                        IsActive = IsActive,
                        Des = Des,
                        IsSpecified = IsSpecified,
                        Icon = Icon
                    };
                    Infos.Add(info);
                }
                else //被動技
                {
                    SkillInfo info = new SkillInfo
                    {
                        SkillName = Name,
                        SkillID = ID,
                        IsActive = IsActive,
                        Des = Des,
                        Icon = Icon
                    };
                    Infos.Add(info);
                }

            }
            SkillDic.Add(JobID, Infos);
        }
    }
    #endregion

    #region NameBox & ChatBox
    public Dictionary<int, (string, int[])> NameBoxDic;
    public Dictionary<int, (string, int[])> ParseNameBoxJson()
    {
        Dictionary<int, (string, int[])> result = new Dictionary<int, (string, int[])>();
        TextAsset itemText = Resources.Load<TextAsset>("ResCfgs/NameBox");
        string NameBoxJson = itemText.text;
        JSONObject j = new JSONObject(NameBoxJson);
        foreach (JSONObject jo in j.list)
        {
            int ItemID = (int)jo["ItemID"].n;
            string Path = jo["Path"].str;
            int[] Order = new int[6];
            List<JSONObject> Orders = jo["Order"].list;
            for (int i = 0; i < Order.Length; i++)
            {
                Order[i] = (int)Orders[i].n;
            }
            result.Add(ItemID, (Path, Order));
        }
        return result;
    }
    #endregion


    #region 怪物區
    public Dictionary<int, MonsterInfo> MonsterInfoDic = new Dictionary<int, MonsterInfo>();

    private void ParseMonsterJson()
    {
        //Unity裡的文本是TextAsset類
        TextAsset itemText = Resources.Load<TextAsset>("ResCfgs/MonsterInfo");
        string MonsterJson = itemText.text;//物品信息的格式
        JSONObject j = new JSONObject(MonsterJson);
        foreach (JSONObject jo in j.list)
        {
            //先解析公有的屬性
            int monsterID = (int)(jo["MonsterID"].n);
            string name = jo["Name"].str;
            int maxHp = (int)(jo["MaxHP"].n);
            MonsterAttribute attribute = (MonsterAttribute)System.Enum.Parse(typeof(MonsterAttribute), jo["Attribute"].str);
            bool isActive = jo["IsActive"].b;
            string description = jo["Des"].str;
            int ribi = (int)(jo["Ribi"].n);
            int bossLevel = (int)(jo["BossLevel"].n);
            int level = (int)(jo["Level"].n);
            string spritestr = jo["Sprite"].str;
            string[] sprites = spritestr.Split(new char[] { '_' });
            int exp = (int)(jo["EXP"].n);
            int defense = (int)(jo["Defense"].n);
            int minDamage = (int)(jo["MinDamage"].n);
            int maxDamage = (int)(jo["MaxDamage"].n);
            int attackRange = (int)(jo["AttackRange"].n);
            float accuracy = (float)(jo["Accuracy"].n);
            float avoid = (float)(jo["Avoid"].n);
            float critical = (float)(jo["Critical"].n);
            float magicdefense = (float)(jo["MagicDefense"].n);
            string AttackSound = jo["AttackSound"].str;
            string DeathSound = jo["DeathSound"].str;
            Dictionary<int, float> DropItems = new Dictionary<int, float>();
            string drop = jo["Drop"].str;
            string[] drops = drop.Split(new char[] { '_' });
            foreach (var s in drops)
            {
                string[] r = s.Split(new char[] { '#' });
                DropItems.Add(Convert.ToInt32(r[0]), (float)Convert.ToDouble(r[1]));
            }
            Dictionary<MonsterAniType, MonsterAnimation> AnimationDic = new Dictionary<MonsterAniType, MonsterAnimation>();
            JSONObject ani = jo["Animation"];

            foreach (var key in ani.keys)
            {
                string AnimString = ani[key].str;
                string[] Anim = AnimString.Split(new char[] { ':' });
                int AnimSpeed = Convert.ToInt32(Anim[0]);
                List<int> AnimSprite = new List<int>();
                List<int> AnimPosition = new List<int>();
                string[] AnimPos = Anim[1].Split(new char[] { '_' });
                foreach (var s in AnimPos)
                {
                    string[] r = s.Split(new char[] { '#' });
                    AnimSprite.Add(Convert.ToInt32(r[0]));
                    AnimPosition.Add(Convert.ToInt32(r[1]));
                }
                MonsterAnimation animation = new MonsterAnimation { AnimSpeed = AnimSpeed, AnimSprite = AnimSprite, AnimPosition = AnimPosition };
                AnimationDic.Add((MonsterAniType)System.Enum.Parse(typeof(MonsterAniType), key), animation);
            }


            MonsterInfo info = new MonsterInfo
            {
                MonsterID = monsterID,
                Name = name,
                MaxHp = maxHp,
                monsterAttribute = attribute,
                Description = description,
                IsActive = isActive,
                Ribi = ribi,
                BossLevel = bossLevel,
                Sprites = sprites,
                Level = level,
                Exp = exp,
                Defense = defense,
                MinDamage = minDamage,
                MaxDamage = maxDamage,
                AttackRange = attackRange,
                Accuracy = accuracy,
                Avoid = avoid,
                Critical = critical,
                MagicDefense = magicdefense,
                DropItems = DropItems,
                AnimationDic = AnimationDic,
                AttackSound = AttackSound,
                DeathSound = DeathSound
            };
            MonsterInfoDic.Add(monsterID, info);
        }
    }
    #endregion

    #region 傳點
    public Dictionary<int, PortalData> PortalDic = new Dictionary<int, PortalData>();
    public void ParsePortalJson()
    {
        TextAsset itemText = Resources.Load<TextAsset>("ResCfgs/PortalDefine");
        string PortalJson = itemText.text;
        JSONObject j = new JSONObject(PortalJson);
        foreach (JSONObject jo in j.list)
        {
            //先解析公有的屬性
            int PortalID = (int)(jo["PortalID"].n);
            var PositionData = jo["Position"].list;
            Vector2 Position = new Vector2(PositionData[0].f, PositionData[1].f);
            int Destination = (int)(jo["Goto"].n);
            PortalData data = new PortalData { PortalID = PortalID, Position = Position, Destination = Destination };
            PortalDic.Add(PortalID, data);
        }
    }
    #endregion

    #region Language
    public Dictionary<string, string> Tra_ChineseStrings = new Dictionary<string, string>();
    public Dictionary<string, string> Sim_ChineseStrings = new Dictionary<string, string>();
    public Dictionary<string, string> EnglishStrings = new Dictionary<string, string>();
    public Dictionary<string, string> KoreanStrings = new Dictionary<string, string>();
    private void InitStrings()
    {
        ParseStrings("Tra_Chinese");
        ParseStrings("Sim_Chinese");
        ParseStrings("English");
        ParseStrings("Korean");
    }
    private void ParseStrings(string Path)
    {
        TextAsset itemText = Resources.Load<TextAsset>("ResCfgs/" + Path);
        string PortalJson = itemText.text;
        JSONObject j = new JSONObject(PortalJson);
        foreach (JSONObject jo in j.list)
        {
            List<string> StringNameList = jo.keys;
            Dictionary<string, string> TargetDic = null;
            switch (Path)
            {
                case "Tra_Chinese":
                    TargetDic = Tra_ChineseStrings;
                    break;
                case "Sim_Chinese":
                    TargetDic = Sim_ChineseStrings;
                    break;
                case "English":
                    TargetDic = EnglishStrings;
                    break;
                case "Korean":
                    TargetDic = KoreanStrings;
                    break;
                default:
                    break;
            }
            if (TargetDic != null)
            {
                foreach (var key in StringNameList)
                {
                    if (!TargetDic.ContainsKey(key))
                    {
                        TargetDic.Add(key, jo[key].str);
                    }
                }
            }
        }
    }
    #endregion
}
