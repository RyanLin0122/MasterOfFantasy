using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;
using PEProtocal;
using System.Text;

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
        ParseBuffJson();
        ParseQuestJson();
        IllustrationPath.Instance.Init();
        LoadJobImgs();
        ParsePortalJson();
        ParseManuJson();
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
    public Dictionary<int, MapCfg> mapCfgDataDic = new Dictionary<int, MapCfg>();
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
                    ID = ID,
                    NPC_Positions = new Dictionary<int, float[]>()
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

                                mc.PlayerBornPos = new float[] { float.Parse(valArr[0]), float.Parse(valArr[1]) };
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
                        case "NPC":
                            {
                                if (string.IsNullOrEmpty(e.InnerText)) break;
                                string[] total = e.InnerText.Split(new char[] { ':' });
                                for (int j = 0; j < total.Length; j++)
                                {
                                    string[] t1 = total[j].Split(new char[] { '#' });
                                    int NPCID = Convert.ToInt32(t1[0]);                                  
                                    string[] t2 = t1[1].Split(new char[] { ',' });
                                    float[] pos = new float[] { (float)Convert.ToDouble(t2[0]), (float)Convert.ToDouble(t2[1]) };
                                    mc.NPC_Positions[NPCID] = pos;
                                }
                            }
                            break;
                        case "BGM":
                            mc.BGM = e.InnerText;

                            break;
                        case "Embi":
                            mc.Embi = e.InnerText;

                            break;
                        case "BG2":
                            mc.BG2 = e.InnerText;

                            break;
                        case "BG3":
                            mc.BG3 = e.InnerText;

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

    public Dictionary<int, NpcConfig> NpcCfgDataDic = new Dictionary<int, NpcConfig>();
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
                        case "Dot":
                            NC.DotSprite = e.InnerText;
                            break;
                        case "Age":
                            NC.Age = e.InnerText;
                            break;
                        case "BloodType":
                            NC.BloodType = e.InnerText;
                            break;
                        case "Height":
                            NC.Height = e.InnerText;
                            break;
                        case "Weight":
                            NC.Weight = e.InnerText;
                            break;
                        case "Job":
                            NC.Job = e.InnerText;
                            break;
                        case "Hobby":
                            NC.Hobby = e.InnerText;
                            break;
                        case "Trick":
                            NC.Trick = e.InnerText;
                            break;
                        case "Personality":
                            NC.Personality = e.InnerText;
                            break;
                        case "Motto":
                            NC.Motto = e.InnerText;
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
    #region ManufactureInfo

    public Dictionary<int, ManuInfo> FormulaDict { get; set; }
    private void ParseManuJson()
    {
        Dictionary<int, ManuInfo> formuladict = new Dictionary<int, ManuInfo>();
        TextAsset itemText = Resources.Load<TextAsset>("ResCfgs/FormulaInfo");
        string FormulaJson = itemText.text;
        JSONObject j = new JSONObject(FormulaJson);
        foreach (JSONObject Formula in j.list)
        {
            int FormulaID = (int)Formula["FormulaID"].n;
            int itemID = (int)Formula["ItemID"].n;
            string ItemName = Formula["ItemName"].str;
            int Amount = (int)Formula["Amount"].n;

            int[] RequireItem = new int[6];
            var RequireItemList = Formula["RequireItem"].list;
            for (int i = 0; i < 6; i++)
            {
                RequireItem[i] = (int)RequireItemList[i].n;
            }

            int[] RequireAmount = new int[6];
            var RequireAmountList = Formula["RequireAmount"].list;
            for (int i = 0; i < 6; i++)
            {
                RequireAmount[i] = (int)RequireAmountList[i].n;
            }
            int Probablity = (int)Formula["Probablity"].n;
            int Experience = (int)Formula["Experience"].n;


            ManuInfo manu = new ManuInfo
            {
                FormulaID = FormulaID,
                ItemID = itemID,
                ItemName = ItemName,
                Amount = Amount,
                RequireItem = RequireItem,
                RequireItemAmount = RequireAmount,
                Probablity = Probablity,
                Experience = Experience
            };
            formuladict.Add(FormulaID, manu);
        }
        this.FormulaDict = formuladict;
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
    public Dictionary<int, SkillInfo> SkillDic = new Dictionary<int, SkillInfo>();
    //int: 職業代碼 
    private void ParseSkillJson()
    {
        //Unity裡的文本是TextAsset類
        TextAsset itemText = Resources.Load<TextAsset>("ResCfgs/SkillInfo");
        string SkillJson = itemText.text;//物品信息的格式
        JSONObject j = new JSONObject(SkillJson);
        foreach (JSONObject Skill in j.list)
        {

            string Name = Skill["Name"].str;
            int ID = (int)Skill["ID"].n;
            bool IsActive = Skill["IsActive"].b;
            var WeaponList = Skill["RequiredWeapon"].list;
            List<WeaponType> RequiredWeapon = new List<WeaponType>();
            if (WeaponList.Count > 0)
            {
                foreach (var item in WeaponList)
                {
                    RequiredWeapon.Add((WeaponType)Enum.Parse(typeof(WeaponType), item.str));
                }
            }
            int[] RequiredLevel = new int[5];
            var LevelList = Skill["RequiredLevel"].list;
            for (int i = 0; i < 5; i++)
            {
                RequiredLevel[i] = (int)LevelList[i].n;
            }
            float[] Damage = new float[5];
            var DamageList = Skill["Damage"].list;
            for (int i = 0; i < 5; i++)
            {
                Damage[i] = DamageList[i].n;
            }
            int[] SwordPoint = new int[5];
            var SwordPointList = Skill["SwordPoint"].list;
            for (int i = 0; i < 5; i++)
            {
                SwordPoint[i] = (int)SwordPointList[i].n;
            }
            int[] ArcheryPoint = new int[5];
            var ArcheryPointList = Skill["ArcheryPoint"].list;
            for (int i = 0; i < 5; i++)
            {
                ArcheryPoint[i] = (int)ArcheryPointList[i].n;
            }
            int[] MagicPoint = new int[5];
            var MagicPointList = Skill["MagicPoint"].list;
            for (int i = 0; i < 5; i++)
            {
                MagicPoint[i] = (int)MagicPointList[i].n;
            }
            int[] TheologyPoint = new int[5];
            var TheologyPointList = Skill["TheologyPoint"].list;
            for (int i = 0; i < 5; i++)
            {
                TheologyPoint[i] = (int)TheologyPointList[i].n;
            }
            string Des = Skill["Des"].str;
            string Icon = "Effect/SkillIcon/" + Skill["Icon"].str;
            List<SkillEffect> Effects = new List<SkillEffect>();
            var EffectList = Skill["Effect"].list;
            if (EffectList.Count > 0)
            {
                foreach (var item in EffectList)
                {
                    SkillEffect effect = new SkillEffect();
                    effect.EffectID = (int)item["EffectID"].n;
                    float[] Values = new float[5];
                    var ValuesList = item["Value"].list;
                    for (int i = 0; i < 5; i++)
                    {
                        Values[i] = ValuesList[i].n;
                    }
                    effect.Values = Values;
                    Effects.Add(effect);
                }
            }
            if (!IsActive) //是被動技
            {
                NegativeSkillInfo negativeSkillInfo = new NegativeSkillInfo
                {
                    SkillID = ID,
                    SkillName = Name,
                    IsActive = IsActive,
                    RequiredWeapon = RequiredWeapon,
                    RequiredLevel = RequiredLevel,
                    Damage = Damage,
                    SwordPoint = SwordPoint,
                    ArcheryPoint = ArcheryPoint,
                    MagicPoint = MagicPoint,
                    TheologyPoint = TheologyPoint,
                    Des = Des,
                    Icon = Icon,
                    Effect = Effects
                };
                SkillDic.Add(ID, negativeSkillInfo);
            }
            else //主動技
            {
                bool IsAttack = Skill["IsAttack"].b;
                bool IsAOE = Skill["IsAOE"].b;
                bool IsBuff = Skill["IsBuff"].b;
                bool IsSetup = Skill["IsSetup"].b;
                int[] Hp = new int[5];
                var HpList = Skill["HP"].list;
                for (int i = 0; i < 5; i++)
                {
                    Hp[i] = (int)HpList[i].n;
                }
                int[] MP = new int[5];
                var MPList = Skill["MP"].list;
                for (int i = 0; i < 5; i++)
                {
                    MP[i] = (int)MPList[i].n;
                }
                float[] ColdTime = new float[5];
                var ColdTimeList = Skill["ColdTime"].list;
                for (int i = 0; i < 5; i++)
                {
                    ColdTime[i] = ColdTimeList[i].n;
                }
                int[] Times = new int[5];
                var TimesList = Skill["Times"].list;
                for (int i = 0; i < 5; i++)
                {
                    Times[i] = (int)TimesList[i].n;
                }
                float[] Durations = new float[5];
                var DurationsList = Skill["Duration"].list;
                for (int i = 0; i < 5; i++)
                {
                    Durations[i] = DurationsList[i].n;
                }
                SkillTargetType targetType = (SkillTargetType)Enum.Parse(typeof(SkillTargetType), Skill["TargetType"].str);
                bool IsMultiple = Skill["IsMultiple"].b;
                SkillRangeShape Shape = (SkillRangeShape)Enum.Parse(typeof(SkillRangeShape), Skill["Shape"].str);
                float[] Range = new float[3];
                var RangeList = Skill["Range"].list;
                for (int i = 0; i < 3; i++)
                {
                    Range[i] = RangeList[i].n;
                }
                SkillProperty Property = (SkillProperty)Enum.Parse(typeof(SkillProperty), Skill["Property"].str);
                bool IsStun = Skill["IsStun"].b;
                bool IsStop = Skill["IsStop"].b;
                bool IsShoot = Skill["IsShoot"].b;
                float BulletSpeed = Skill["BulletSpeed"].n;
                bool IsContinue = Skill["IsContinue"].b;
                float[] ContiDurations = new float[5];
                var ContiDurationsList = Skill["ContiDurations"].list;
                for (int i = 0; i < 5; i++)
                {
                    ContiDurations[i] = ContiDurationsList[i].n;
                }
                float ContiInterval = Skill["ContiInterval"].n;
                bool IsDOT = Skill["IsDOT"].b;
                List<float> HitTimes = new List<float>();
                var HitTimesList = Skill["HitTimes"].list;
                if (HitTimesList.Count > 0)
                {
                    for (int i = 0; i < HitTimesList.Count; i++)
                    {
                        HitTimes.Add(HitTimesList[i].n);
                    }
                }
                PlayerAniType Action = (PlayerAniType)Enum.Parse(typeof(PlayerAniType), Skill["Action"].str);
                Dictionary<string, string> AniPath = new Dictionary<string, string>();
                AniPath.Add("Self", Skill["AniPath"]["Self"].str);
                AniPath.Add("Shoot", Skill["AniPath"]["Shoot"].str);
                AniPath.Add("Other", Skill["AniPath"]["Other"].str);
                Dictionary<string, float[]> AniOffset = new Dictionary<string, float[]>();
                var AniOffset_Self = Skill["AniOffset"]["Self"].list;
                var AniOffset_Shoot = Skill["AniOffset"]["Shoot"].list;
                var AniOffset_Target = Skill["AniOffset"]["Target"].list;
                AniOffset.Add("Self", new float[3]);
                AniOffset.Add("Shoot", new float[3]);
                AniOffset.Add("Target", new float[3]);
                for (int i = 0; i < 3; i++)
                {
                    AniOffset["Self"][i] = AniOffset_Self[i].n;
                    AniOffset["Shoot"][i] = AniOffset_Shoot[i].n;
                    AniOffset["Target"][i] = AniOffset_Target[i].n;
                }
                Dictionary<string, float[]> AniScale = new Dictionary<string, float[]>();
                var AniScale_Self = Skill["AniScale"]["Self"].list;
                var AniScale_Shoot = Skill["AniScale"]["Shoot"].list;
                var AniScale_Target = Skill["AniScale"]["Target"].list;
                AniScale.Add("Self", new float[3]);
                AniScale.Add("Shoot", new float[3]);
                AniScale.Add("Target", new float[3]);
                for (int i = 0; i < 3; i++)
                {
                    AniScale["Self"][i] = AniScale_Self[i].n;
                    AniScale["Shoot"][i] = AniScale_Shoot[i].n;
                    AniScale["Target"][i] = AniScale_Target[i].n;
                }
                Dictionary<string, string> Sound = new Dictionary<string, string>();
                Sound.Add("Cast", Skill["Sound"]["Cast"].str);
                Sound.Add("Hit", Skill["Sound"]["Hit"].str);
                float CastTime = Skill["CastTime"].n;
                float ChargeTime = Skill["ChargeTime"].n;
                float LockTime = Skill["LockTime"].n;
                int Buff = (int)Skill["Buff"].n;
                ActiveSkillInfo activeSkillInfo = new ActiveSkillInfo
                {
                    SkillID = ID,
                    SkillName = Name,
                    IsActive = IsActive,
                    RequiredWeapon = RequiredWeapon,
                    RequiredLevel = RequiredLevel,
                    Damage = Damage,
                    SwordPoint = SwordPoint,
                    ArcheryPoint = ArcheryPoint,
                    MagicPoint = MagicPoint,
                    TheologyPoint = TheologyPoint,
                    Des = Des,
                    Effect = Effects,
                    IsAttack = IsAttack,
                    IsAOE = IsAOE,
                    IsBuff = IsBuff,
                    IsSetup = IsSetup,
                    Hp = Hp,
                    MP = MP,
                    ColdTime = ColdTime,
                    Times = Times,
                    Durations = Durations,
                    TargetType = targetType,
                    IsMultiple = IsMultiple,
                    Shape = Shape,
                    Range = Range,
                    Property = Property,
                    IsStun = IsStun,
                    IsStop = IsStop,
                    IsShoot = IsShoot,
                    IsContinue = IsContinue,
                    ContiDurations = ContiDurations,
                    ContiInterval = ContiInterval,
                    IsDOT = IsDOT,
                    HitTimes = HitTimes,
                    Action = Action,
                    AniPath = AniPath,
                    AniOffset = AniOffset,
                    Sound = Sound,
                    CastTime = CastTime,
                    ChargeTime = ChargeTime,
                    LockTime = LockTime,
                    Icon = Icon,
                    BulletSpeed = BulletSpeed,
                    Buff = Buff,
                    AniScale = AniScale
                };
                SkillDic.Add(ID, activeSkillInfo);
            }

        }
    }
    #endregion

    #region Buff
    public Dictionary<int, BuffDefine> BuffDic = new Dictionary<int, BuffDefine>();
    public void ParseBuffJson()
    {
        TextAsset itemText = Resources.Load<TextAsset>("ResCfgs/BuffDefine");
        string SkillJson = itemText.text;//物品信息的格式
        JSONObject j = new JSONObject(SkillJson);
        foreach (JSONObject Buff in j.list)
        {
            int BuffID = (int)Buff["ID"].n;
            string Icon = Buff["Icon"].str;
            string BuffName = Buff["BuffName"].str;
            string Description = Buff["Description"].str;
            float Duration = Buff["Duration"].n;
            float Inteval = Buff["Inteval"].n;
            BUFF_TriggerType TriggerType = (BUFF_TriggerType)System.Enum.Parse(typeof(BUFF_TriggerType), Buff["TriggerType"].str);
            float DamageFactor = Buff["DamageFactor"].n;
            BUFF_TargetType TargetType = (BUFF_TargetType)System.Enum.Parse(typeof(BUFF_TargetType), Buff["TargetType"].str);
            float CD = Buff["CD"].n;
            BUFF_Effect buFF_Effect = (BUFF_Effect)System.Enum.Parse(typeof(BUFF_Effect), Buff["BuffState"].str);
            List<int> ConflictBuffIDs = new List<int>();
            var conlist = Buff["ConflictBuffs"].list;
            if (conlist.Count > 0)
            {
                foreach (var id in conlist)
                {
                    ConflictBuffIDs.Add((int)id.n);
                }
            }
            JSONObject Attr = Buff["Attribute"];
            PlayerAttribute attribute = new PlayerAttribute
            {
                MAXHP = (int)(Attr["MAXHP"].n),
                MAXMP = (int)(Attr["MAXMP"].n),
                Att = (int)(Attr["Att"].n),
                Strength = (int)(Attr["Strength"].n),
                Agility = (int)(Attr["Agility"].n),
                Intellect = (int)(Attr["Intellect"].n),
                MaxDamage = (int)(Attr["MaxDamage"].n),
                MinDamage = (int)(Attr["MinDamage"].n),
                Defense = (int)(Attr["Defense"].n),
                Accuracy = Attr["Accuracy"].n,
                Critical = Attr["Critical"].n,
                Avoid = Attr["Avoid"].n,
                MagicDefense = Attr["MagicDefense"].n,
                RunSpeed = Attr["RunSpeed"].n,
                AttRange = Attr["AttRange"].n,
                AttDelay = Attr["AttDelay"].n,
                ExpRate = Attr["ExpRate"].n,
                DropRate = Attr["DropRate"].n,
                HPRate = Attr["HPRate"].n,
                MPRate = Attr["MPRate"].n,
                MinusHurt = Attr["MinusHurt"].n
            };
            BuffDefine buffDefine = new BuffDefine
            {
                ID = BuffID,
                Icon = Icon,
                BuffName = BuffName,
                Description = Description,
                Duration = Duration,
                Interval = Inteval,
                TriggerType = TriggerType,
                DamageFactor = DamageFactor,
                TargetType = TargetType,
                CD = CD,
                BuffState = buFF_Effect,
                ConflictBuff = ConflictBuffIDs,
                AttributeGain = attribute
            };
            BuffDic.Add(BuffID, buffDefine);
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

    #region 任務區
    public Dictionary<int, QuestDefine> QuestDic = new Dictionary<int, QuestDefine>();
    public void ParseQuestJson()
    {
        TextAsset itemText = Resources.Load<TextAsset>("ResCfgs/QuestDefine");//, Encoding.GetEncoding("utf-8"));
        string QuestJson = itemText.text;//物品信息的格式
        JSONObject j = new JSONObject(QuestJson);
        foreach (JSONObject Quest in j.list)
        {
            int QuestID = (int)Quest["QuestID"].n;
            string QuestName = Quest["QuestName"].str;
            int LimitLevel = (int)Quest["LimitLevel"].n;
            int LimitJob = (int)Quest["LimitJob"].n;
            int PreQuest = (int)Quest["PreQuest"].n;
            int PostQuest = (int)Quest["PostQuest"].n;
            QuestType Type = (QuestType)Enum.Parse(typeof(QuestType), Quest["QuestType"].str);
            int AcceptNPC = (int)Quest["AcceptNPC"].n;
            int SubmitNPC = (int)Quest["SubmitNPC"].n;
            int DeliveryNPC = (int)Quest["DeliveryNPC"].n;

            QuestTarget questTarget = (QuestTarget)Enum.Parse(typeof(QuestTarget), Quest["Target"].str);
            List<int> TargetIDs = new List<int>();
            var TargetIDslist = Quest["TargetIDs"].list;
            if (TargetIDslist.Count > 0)
            {
                foreach (var item in TargetIDslist)
                {
                    TargetIDs.Add((int)item.n);
                }
            }
            List<int> TargetNum = new List<int>();
            var TargetNumlist = Quest["TargetNum"].list;
            if (TargetNumlist.Count > 0)
            {
                foreach (var item in TargetNumlist)
                {
                    TargetNum.Add((int)item.n);
                }
            }
            string Overview = Quest["Overview"].str;
            List<string> DialogDelivery = new List<string>();
            var DialogDeliveryList = Quest["DialogDelivery"].list;
            if (DialogDeliveryList != null && DialogDeliveryList.Count > 0)
            {
                foreach (var dialog in DialogDeliveryList)
                {
                    DialogDelivery.Add(dialog.str);
                }
            }

            List<string> DialogAccept = new List<string>();
            var DialogAcceptList = Quest["DialogAccept"].list;
            if (DialogAcceptList != null && DialogAcceptList.Count > 0)
            {
                foreach (var dialog in DialogAcceptList)
                {
                    DialogAccept.Add(dialog.str);
                }
            }

            List<string> DialogDeny = new List<string>();
            var DialogDenyList = Quest["DialogDeny"].list;
            if (DialogDenyList != null && DialogDenyList.Count > 0)
            {
                foreach (var dialog in DialogDenyList)
                {
                    DialogDeny.Add(dialog.str);
                }
            }

            List<string> DialogInComplete = new List<string>();
            var DialogInCompleteList = Quest["DialogInComplete"].list;
            if (DialogInCompleteList != null && DialogInCompleteList.Count > 0)
            {
                foreach (var dialog in DialogInCompleteList)
                {
                    DialogInComplete.Add(dialog.str);
                }
            }

            List<string> DialogFinish = new List<string>();
            var DialogFinishList = Quest["DialogFinish"].list;
            if (DialogFinishList != null && DialogFinishList.Count > 0)
            {
                foreach (var dialog in DialogFinishList)
                {
                    DialogFinish.Add(dialog.str);
                }
            }

            long RewardRibi = Quest["RewardRibi"].i;
            int RewardExp = (int)Quest["RewardExp"].n;
            List<int> RewardItemIDs = new List<int>();
            var RewardItemIDslist = Quest["RewardItemIDs"].list;
            if (RewardItemIDslist.Count > 0)
            {
                foreach (var item in RewardItemIDslist)
                {
                    RewardItemIDs.Add((int)item.n);
                }
            }
            List<int> RewardItemsCount = new List<int>();
            var RewardItemsCountlist = Quest["RewardItemsCount"].list;
            if (RewardItemsCountlist.Count > 0)
            {
                foreach (var item in RewardItemsCountlist)
                {
                    RewardItemsCount.Add((int)item.n);
                }
            }
            int RewardHonerPoint = (int)Quest["RewardHonerPoint"].n;
            int RewardBadge = (int)Quest["RewardBadge"].n;
            int RewardTitle = (int)Quest["RewardTitle"].n;
            float LimitTime = Quest["LimitTime"].n;
            QuestDefine questDefine = new QuestDefine
            {
                ID = QuestID,
                QuestName = QuestName,
                LimitLevel = LimitLevel,
                LimitJob = LimitJob,
                PreQuest = PreQuest,
                PostQuest = PostQuest,
                Type = Type,
                AcceptNPC = AcceptNPC,
                SubmitNPC = SubmitNPC,
                DeliveryNPC = DeliveryNPC,
                Target = questTarget,
                TargetIDs = TargetIDs,
                TargetNum = TargetNum,
                Overview = Overview,
                DialogDelivery = DialogDelivery,
                DialogAccept = DialogAccept,
                DialogDeny = DialogDeny,
                DialogInComplete = DialogInComplete,
                DialogFinish = DialogFinish,

                RewardRibi = RewardRibi,
                RewardExp = RewardExp,
                RewardItemIDs = RewardItemIDs,
                RewardItemsCount = RewardItemsCount,
                RewardBadge = RewardBadge,
                RewardTitle = RewardTitle,
                RewardHonerPoint = RewardHonerPoint,
                LimitTime = LimitTime
            };
            QuestDic[QuestID] = questDefine;
        }

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
            float speed = jo["Speed"].n;
            float Radius = (float)(jo["FloatRadius"].n);
            bool IsFly = jo["IsFly"].b;
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
                MonsterAniDic = AnimationDic,
                AttackSound = AttackSound,
                DeathSound = DeathSound,
                Speed = speed,
                IsFly = IsFly,
                Radius = Radius
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
