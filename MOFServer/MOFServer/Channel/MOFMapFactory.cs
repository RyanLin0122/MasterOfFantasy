using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Collections.Concurrent;

public class MOFMapFactory
{
    private ChannelServer channelServer;
    public Dictionary<int, MOFMap> maps = new Dictionary<int, MOFMap>();
    private Dictionary<int, MOFMap> instanceMap = new Dictionary<int, MOFMap>();
    //private static Dictionary<int, MapleNodes> mapInfos = new Dictionary<int, MapleNodes>();
    //private ReentrantLock lock = new ReentrantLock(true);
    private int channel;

    public MOFMapFactory(int channel, ChannelServer server)
    {
        setChannel(channel);
        this.channelServer = server;
        LoadMaps();
    }

    public void setChannel(int channel)
    {
        this.channel = channel;
    }
    public void LoadMaps()
    {
        XmlDocument doc = new XmlDocument();
        doc.Load("../../Common/MapInfo.Xml");
        XmlNodeList nodLst = doc.SelectSingleNode("root").ChildNodes;
        for (int i = 0; i < nodLst.Count; i++)
        {
            XmlElement ele = nodLst[i] as XmlElement;

            if (ele.GetAttributeNode("ID") == null)
            {
                continue;
            }
            int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
            float[] playerBornPos = new float[] { 0, 0 };
            bool IsVillage = false, Islimited = false;
            string mapName = "", Location = "", SceneName = "";
            int monsternum = 0, recoverytime = 0;
            ConcurrentDictionary<int, MonsterPoint> Points = new ConcurrentDictionary<int, MonsterPoint>();
            foreach (XmlElement e in nodLst[i].ChildNodes)
            {

                switch (e.Name)
                {
                    case "MapName":
                        mapName = e.InnerText;
                        break;
                    case "Location":
                        Location = e.InnerText;
                        break;
                    case "SceneName":
                        SceneName = e.InnerText;
                        break;
                    case "PlayerBornPos":
                        {
                            string[] valArr = e.InnerText.Split(',');

                            playerBornPos = new float[] { float.Parse(valArr[0]), float.Parse(valArr[1]) };
                        }
                        break;
                    case "Islimited":
                        {
                            if (e.InnerText == "0")
                            {
                                Islimited = false;
                            }
                            else
                            {
                                Islimited = true;
                            }
                        }
                        break;
                    case "IsVillage":
                        {
                            if (e.InnerText == "0")
                            {
                                IsVillage = false;
                            }
                            else
                            {
                                IsVillage = true;
                            }
                        }
                        break;
                    case "MonsterMax":
                        {
                            monsternum = Convert.ToInt32(e.InnerText);
                        }
                        break;
                    case "BornTime":
                        {
                            recoverytime = Convert.ToInt32(e.InnerText);

                        }
                        break;
                    case "MonsterPoints":
                        string[] total = e.InnerText.Split(new char[] { ':' });
                        for (int j = 0; j < monsternum; j++)
                        {
                            string[] t1 = total[j].Split(new char[] { '#' });
                            int MonID = Convert.ToInt32(t1[0]);
                            MonsterPoint p = new MonsterPoint { MonsterID = MonID, monster = new AbstractMonster { MonsterID = MonID, Hp = CacheSvc.Instance.MonsterInfoDic[MonID].MaxHp ,status = PEProtocal.MonsterStatus.Death} };
                            string[] t2 = t1[1].Split(new char[] { ',' });
                            p.InitialPos = new float[] { (float)Convert.ToDouble(t2[0]), (float)Convert.ToDouble(t2[1]) };
                            p.Pos = p.InitialPos;
                            Points.TryAdd(j, p);
                        }

                        break;
                }

            }
            MOFMap map = new MOFMap(ID, channel, 1000, recoverytime, mapName, Location, SceneName, playerBornPos, Islimited, IsVillage, monsternum, Points, channelServer);
            maps.Add(ID, map);
        }
    }
}

public class MonsterPoint
{
    public int MonsterID { get; set; }
    public float[] InitialPos { get; set; }
    public float[] Pos { get; set; }
    public AbstractMonster monster { get; set; }
    public int AccumulateDamage { get; set; }
    public Dictionary<string, int> DamageRecords { get; set; } 
    public string TargetPlayerName { get; set; }

    public MonsterPoint()
    {
        TargetPlayerName = "";
        AccumulateDamage = 0;
        Pos = InitialPos;
        DamageRecords = new Dictionary<string, int>();
        if (monster != null)
        {
            monster.status = PEProtocal.MonsterStatus.Death;
        }
    }
    public void ClearAccumulateDamage()
    {
        AccumulateDamage = 0;
        DamageRecords.Clear();
    }
}


