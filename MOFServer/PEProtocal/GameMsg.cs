using System;
using PENet;
using System.Collections.Generic;

namespace PEProtocal
{
    
   

    [Serializable]
    public class PlayerData //cmd:15
    {
        public int id;
        public int accountmapping;
        public string name;
        public int gender;
        public int job;
        public int lv;
        public long exp;
        public int hp;
        public int mp;
        public int MaxHp;
        public int MaxMp;
        public long coin;
        public int Att;
        public int Health;
        public int Dex;
        public int Int;
        public int isnew;
        public int map;
        public int grade;
        public int restpoint;
        public string title;
        public string couple;
        public long LockerCoin;
        public long MailBoxCoin;
        public int battlehead;
        public int battlering1;
        public int battleneck;
        public int battlering2;
        public int battleweapon;
        public int battlechest;
        public int battleglove;
        public int battleshield;
        public int battlepant;
        public int battleshoes;
        public int Fashionhairacc;
        public int Fashionnamebox;
        public int Fashionchatbox;
        public int Fashionface;
        public int Fashionglasses;
        public int Fashionhairstyle;
        public int Fashionchest;
        public int Fashionglove;
        public int Fashioncape;
        public int Fashionpant;
        public int Fashionshoes;
        public int FaceType;
        //TOADD
    }

    public enum ErrorCode
    {
        None = 0,//No errors

        AcctIsOnline,//Account has been logined
        WrongPass,//Passwords is wrong
        NameIsExist,//The name has been used
        UpdateDBError,//Update Database error
    }
    public enum CMD
    {
        None = 0,
        //登入相關 100
        ReqLogin = 101,
        RspLogin = 102,
        //命名相關
        ReqRename = 103,
        RspRename = 104,
        //創建相關 
        ReqCreate = 105,
        RspCreate = 106,

        //聊天相關
        
    }

    public class IPCfg
    {
        //public const string srvIP = "1.160.116.114";
        public const string srvIP = "127.0.0.1";
        public const int srvPort = 8763;
        public const short defaultPort = 8787;
    }
}
