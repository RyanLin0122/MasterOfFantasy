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
    public static long GetRequiredExp(int Level)
    {
        long[] expSheet = new long[]
        {
            120, //1
            200, //2
            300, //3
            810, //4
            1215,//5
            1482,//6
            2411,//7
            2942,//8
            4413,//9
            6178,//10
            9807,//11
            12455,//12
            18981,//13
            28471,//14
            28731,//15
            37925,//16
            50061,//17
            66080,//18
            81774,//19
            106306,//20
            136453,//21
            166053,//22
            214445,//23
            273417,//24
            351423,//25
            416637,//26
            524963,//27
            661453,//28
            833431,//29
            1050123,//30
            1338570,//31
            1650092,//32
            2071781,//33
            2638870,//34
            3324976,//35
            4189470,//36
            5580200,//37
            7081081,//38
            9293919,//39
            11756808,//40
            14492938,//41
            17936735,//42
            22251300,//43
            29345279,//44
            33129858,//45
            41081024,//46
            50940470,//47
            63166183,//48
            71798895,//49
            77668351,//50
            85603542,//51
            90397340,//52
            95278797,//53
            111165830,//54
            119317991,//55
            126306616,//56
            142726476,//57
            161280918,//58
            168228404,//59
            176639824,//60
            185733242,//61
            195569675,//62
            206216488,//63
            217748115,//64
            230246856,//65
            243803790,//66
            258519788,//67
            274506651,//68
            291888412,//69
            310802781,//70
            331402790,//71
            353858642,//72
            378355315,//73
            405117422,//74
            434366899,//75
            466371052,//76
            501423500,//77
            539852597,//78
            582025883,//79
            58377196,//80
            585523276,//81
            587282481,//82
            589041685,//83
            590808810,//84
            592581237,//85
            594358980,//86
            596142058,//87
            597930483,//88
            599724275,//89
            601523448,//90
            603330726,//91
            605138003,//92
            606953416,//93
            608774276,//94
            610600600,//95
            612435150,//96
            614269698,//97
            616112508,//98
            617960845,//99
            619814728,//100
            621676961,//101
            623539194,//102
            625409812,//103
            627288856,//104
            629167900,//105
            631055403,//106
            632948569,//107
            634847415,//108
            636751957,//109
            638662213,//110
            640578200,//111
            642499934,//112
            644427434,//113
            646360717,//114
            648299798,//115
            650244698,//116
            652198358,//117
            654152019,//118
            656114474,//119
            1000000000
        };
        return expSheet[Level - 1];
    }
}

public enum MonsterAIMode
{
    ServerControll,
    ClientControll
}

