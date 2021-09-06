using System.Collections.Generic;
namespace PEProtocal
{
    public class BaseData<T>
    {
        public int ID;
    }

    public class MapCfg : BaseData<MapCfg>
    {
        public string mapName;
        public string Location;
        public string SceneName;
        public float[] PlayerBornPos;
        public bool Islimited;
        public bool IsVillage;
        public int MonsterMax;
        public int BornTime;
    }

    public class NpcConfig : BaseData<NpcConfig>
    {
        public string Name;
        public List<int> Functions;
        public string Sprite;
        public string[] FixedText;
    }

    public class TitleData : BaseData<TitleData>
    {
        public string Tra_ChineseName;
        public string Sim_ChineseName;
        public string English;
        public string Korean;
    }

    public class CashShopData : BaseData<CashShopData>
    {
        public int ItemID { get; set; }
        public int SellPrice { get; set; }
    }
}

