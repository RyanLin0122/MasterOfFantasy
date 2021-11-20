using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class ServerConstants
{
    public const string ip = "127.0.0.1";
    public const short expRate = 1;
    public const short coinRate = 1;
    public const short dropRate = 1;
    public const short cashRate = 1;
    public const MonsterAIMode MonsterMode = MonsterAIMode.ServerControll;
    public const string ServerName = "天空之城";
    public const string ServerMessage = "玩得愉快!";
    public const short DefaultPort = 8787;
    public const int ChannelLimit = 100;
    public const int MaxCharacters = 3000;
    public const int GameServerNum = 3;
    public const int channelNum = 10;
    public const int MagicNumber = 66666;
    public const int Version = 30;
    public const string PublicKey = "HaveFun";
    public const string PrivateKey = "y5(9=8@nV$;G";
    public const string DBconnStr = "mongodb+srv://admin:edei5262@devops-sxe0y.mongodb.net/MOF?authSource=admin&replicaSet=DevOps-shard-0&readPreference=primary&appname=MongoDB%20Compass%20Community&ssl=true";
    public static string[] Channel_Key =
    {
        "a56=-_dcSAgb",
        "y5(9=8@nV$;G",
        "yS5j943GzdUm",
        "G]R8Frg;kx6Y",
        "Z)?7fh*([N6S",
        "p4H8=*sknaEK",
        "A!Z7:mS.2?Kq",
        "M5:!rfv[?mdF",
        "Ee@3-7u5s6xy",
        "p]6L3eS(R;8A",
        "gZ,^k9.npy#F",
        "cG3M,*7%@zgt",
        "t+#@TV^3)hL9",
        "mw4:?sAU7[!6",
        "b6L]HF(2S,aE",
        "H@rAq]#^Y3+J",
        "o2A%wKCuqc7Txk5?#rNZ",
        "d4.Np*B89C6+]y2M^z-7",
        "oTL2jy9^zkH.84u(%b[d",
        "WCSJZj3tGX,[4hu;9s?g"
    };
}

public enum MonsterAIMode
{
    ServerControll,
    ClientControll
}

