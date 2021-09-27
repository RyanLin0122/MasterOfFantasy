using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

public static class MyExtension
{
    public static T[] GetComponentsInRealChildren<T>(this GameObject go)
    {
        List<T> TList = new List<T>();
        TList.AddRange(go.GetComponentsInChildren<T>());
        TList.RemoveAt(0);
        return TList.ToArray();
    }
}
public class Tools
{
    //Safe Invoke
    public static void SafeInvoke(Action action)
    {
        try
        {
            action.Invoke();
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
    public static TOut TransReflection<TIn, TOut>(TIn tIn)
    {
        TOut tOut = Activator.CreateInstance<TOut>();
        var tInType = tIn.GetType();
        foreach (var itemOut in tOut.GetType().GetProperties())
        {
            var itemIn = tInType.GetProperty(itemOut.Name);
            if (itemIn != null)
            {
                itemOut.SetValue(tOut, itemIn.GetValue(tIn));
            }
        }

        return tOut;
    }
    public static void Log(string message)
    {
        Debug.Log(message + "\n" + DateTime.Now.ToString()+": "+DateTime.Now.Millisecond);
    }
    public static void SetAlpha(Image image, float a)
    {
        var tempColor = image.color;
        tempColor.a = a;
        image.color = tempColor;
    }
    public static int RDInt(int min, int max, System.Random rd = null)
    {
        if (rd == null)
        {
            rd = new System.Random();
        }
        int val = rd.Next(min, max + 1);
        return val;
    }

    //分割每個十百千位數字成list
    public static List<long> SeperateNum(long damage)
    {

        List<long> numList = new List<long>();
        long last = 0;

        for (int i = 1; damage / i != 0; i *= 10)
        {
            numList.Add((damage % (i * 10) - last) / i);

            last = damage % (i * 10);
        }
        return numList;
    }

    //分割每個十百千位數字成list
    public static List<int> SeperateNum(int damage)
    {

        List<int> numList = new List<int>();
        int last = 0;

        for (int i = 1; damage / i != 0; i *= 10)
        {
            numList.Add((damage % (i * 10) - last) / i);
            last = damage % (i * 10);
        }
        return numList;
    }

    //計算等級所需最大經驗值
    public static long GetExpMax(int lv)
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
        return expSheet[lv - 1];
    }

    //mode: 0怪物傷害(普通) 1怪物傷害(爆擊) 2玩家受傷 3玩家補魔 4經驗值 5未知
    public static string GetFloatingDamage(int damage, int mode)
    {
        string r = "";
        switch (mode)
        {
            case 0:
                if (damage > 0)
                {
                    r += "<sprite=12>";
                    
                }
                else if(damage == 0)
                {
                    r += "<sprite=26>";
                    return r;
                }
                else
                {
                    r += "<sprite=11>";
                }
                damage = Mathf.Abs(damage);
                string s1 = damage.ToString();
                char[] array = s1.ToCharArray();
                foreach (var ch in array)
                {
                    switch (ch)
                    {
                        case '0':
                            r += "<sprite=1>";
                            break;
                        case '1':
                            r += "<sprite=2>";
                            break;
                        case '2':
                            r += "<sprite=3>";
                            break;
                        case '3':
                            r += "<sprite=4>";
                            break;
                        case '4':
                            r += "<sprite=5>";
                            break;
                        case '5':
                            r += "<sprite=6>";
                            break;
                        case '6':
                            r += "<sprite=7>";
                            break;
                        case '7':
                            r += "<sprite=8>";
                            break;
                        case '8':
                            r += "<sprite=9>";
                            break;
                        case '9':
                            r += "<sprite=10>";
                            break;
                    }
                }
                break;
            case 1:
                if (damage > 0)
                {
                    r += "<sprite=40>";

                }
                else if (damage == 0)
                {
                    r += "<sprite=26>";
                    return r;
                }
                else
                {
                    r += "<sprite=39>";
                }
                damage = Mathf.Abs(damage);
                string s3 = damage.ToString();
                char[] array3 = s3.ToCharArray();
                foreach (var ch in array3)
                {
                    switch (ch)
                    {
                        case '0':
                            r += "<sprite=29>";
                            break;
                        case '1':
                            r += "<sprite=30>";
                            break;
                        case '2':
                            r += "<sprite=31>";
                            break;
                        case '3':
                            r += "<sprite=32>";
                            break;
                        case '4':
                            r += "<sprite=33>";
                            break;
                        case '5':
                            r += "<sprite=34>";
                            break;
                        case '6':
                            r += "<sprite=35>";
                            break;
                        case '7':
                            r += "<sprite=36>";
                            break;
                        case '8':
                            r += "<sprite=37>";
                            break;
                        case '9':
                            r += "<sprite=38>";
                            break;
                    }
                }
                break;
            case 2:
                if (damage > 0)
                {
                    r += "<sprite=54>";

                }
                else if (damage == 0)
                {
                    r += "<sprite=26>";
                    return r;
                }
                else
                {
                    r += "<sprite=53>";
                }
                damage = Mathf.Abs(damage);
                string s2 = damage.ToString();
                char[] array2 = s2.ToCharArray();
                foreach (var ch in array2)
                {
                    switch (ch)
                    {
                        case '0':
                            r += "<sprite=55>";
                            break;
                        case '1':
                            r += "<sprite=56>";
                            break;
                        case '2':
                            r += "<sprite=59>";
                            break;
                        case '3':
                            r += "<sprite=60>";
                            break;
                        case '4':
                            r += "<sprite=61>";
                            break;
                        case '5':
                            r += "<sprite=62>";
                            break;
                        case '6':
                            r += "<sprite=63>";
                            break;
                        case '7':
                            r += "<sprite=64>";
                            break;
                        case '8':
                            r += "<sprite=65>";
                            break;
                        case '9':
                            r += "<sprite=57>";
                            break;
                    }
                }
                break;
            case 3:
                if (damage > 0)
                {
                    r += "<sprite=52>";

                }
                else if (damage == 0)
                {
                    r += "<sprite=26>";
                    return r;
                }
                else
                {
                    r += "<sprite=51>";
                }
                damage = Mathf.Abs(damage);
                string s4 = damage.ToString();
                char[] array4 = s4.ToCharArray();
                foreach (var ch in array4)
                {
                    switch (ch)
                    {
                        case '0':
                            r += "<sprite=41>";
                            break;
                        case '1':
                            r += "<sprite=42>";
                            break;
                        case '2':
                            r += "<sprite=43>";
                            break;
                        case '3':
                            r += "<sprite=44>";
                            break;
                        case '4':
                            r += "<sprite=45>";
                            break;
                        case '5':
                            r += "<sprite=46>";
                            break;
                        case '6':
                            r += "<sprite=47>";
                            break;
                        case '7':
                            r += "<sprite=48>";
                            break;
                        case '8':
                            r += "<sprite=49>";
                            break;
                        case '9':
                            r += "<sprite=50>";
                            break;
                    }
                }
                break;
            case 4:
                if (damage > 0)
                {
                    r += "<sprite=25>";

                }
                else if (damage == 0)
                {
                    r += "<sprite=26>";
                    return r;
                }
                else
                {
                    r += "<sprite=24>";
                }
                damage = Mathf.Abs(damage);
                string s5 = damage.ToString();
                char[] array5 = s5.ToCharArray();
                foreach (var ch in array5)
                {
                    switch (ch)
                    {
                        case '0':
                            r += "<sprite=14>";
                            break;
                        case '1':
                            r += "<sprite=15>";
                            break;
                        case '2':
                            r += "<sprite=16>";
                            break;
                        case '3':
                            r += "<sprite=17>";
                            break;
                        case '4':
                            r += "<sprite=18>";
                            break;
                        case '5':
                            r += "<sprite=19>";
                            break;
                        case '6':
                            r += "<sprite=20>";
                            break;
                        case '7':
                            r += "<sprite=21>";
                            break;
                        case '8':
                            r += "<sprite=22>";
                            break;
                        case '9':
                            r += "<sprite=23>";
                            break;
                    }
                }
                break;
            case 5:
                if (damage > 0)
                {
                    r += "<sprite=66>";

                }
                else if (damage == 0)
                {
                    r += "<sprite=26>";
                    return r;
                }
                else
                {
                    r += "<sprite=77>";
                }
                damage = Mathf.Abs(damage);
                string s6 = damage.ToString();
                char[] array6 = s6.ToCharArray();
                foreach (var ch in array6)
                {
                    switch (ch)
                    {
                        case '0':
                            r += "<sprite=67>";
                            break;
                        case '1':
                            r += "<sprite=68>";
                            break;
                        case '2':
                            r += "<sprite=69>";
                            break;
                        case '3':
                            r += "<sprite=70>";
                            break;
                        case '4':
                            r += "<sprite=71>";
                            break;
                        case '5':
                            r += "<sprite=72>";
                            break;
                        case '6':
                            r += "<sprite=73>";
                            break;
                        case '7':
                            r += "<sprite=74>";
                            break;
                        case '8':
                            r += "<sprite=75>";
                            break;
                        case '9':
                            r += "<sprite=76>";
                            break;
                    }
                }
                break;

        }

        return r;
    }
    public static string GetFloatingDamage(long damage, int mode)
    {
        string r = "";
        switch (mode)
        {
            case 0:
                if (damage > 0)
                {
                    r += "<sprite=12>";

                }
                else if (damage == 0)
                {
                    r += "<sprite=26>";
                    return r;
                }
                else
                {
                    r += "<sprite=11>";
                }
                string s1 = damage.ToString();
                char[] array = s1.ToCharArray();
                foreach (var ch in array)
                {
                    switch (ch)
                    {
                        case '0':
                            r += "<sprite=1>";
                            break;
                        case '1':
                            r += "<sprite=2>";
                            break;
                        case '2':
                            r += "<sprite=3>";
                            break;
                        case '3':
                            r += "<sprite=4>";
                            break;
                        case '4':
                            r += "<sprite=5>";
                            break;
                        case '5':
                            r += "<sprite=6>";
                            break;
                        case '6':
                            r += "<sprite=7>";
                            break;
                        case '7':
                            r += "<sprite=8>";
                            break;
                        case '8':
                            r += "<sprite=9>";
                            break;
                        case '9':
                            r += "<sprite=10>";
                            break;
                    }
                }
                break;
            case 1:
                if (damage > 0)
                {
                    r += "<sprite=40>";

                }
                else if (damage == 0)
                {
                    r += "<sprite=26>";
                    return r;
                }
                else
                {
                    r += "<sprite=39>";
                }
                string s3 = damage.ToString();
                char[] array3 = s3.ToCharArray();
                foreach (var ch in array3)
                {
                    switch (ch)
                    {
                        case '0':
                            r += "<sprite=29>";
                            break;
                        case '1':
                            r += "<sprite=30>";
                            break;
                        case '2':
                            r += "<sprite=31>";
                            break;
                        case '3':
                            r += "<sprite=32>";
                            break;
                        case '4':
                            r += "<sprite=33>";
                            break;
                        case '5':
                            r += "<sprite=34>";
                            break;
                        case '6':
                            r += "<sprite=35>";
                            break;
                        case '7':
                            r += "<sprite=36>";
                            break;
                        case '8':
                            r += "<sprite=37>";
                            break;
                        case '9':
                            r += "<sprite=38>";
                            break;
                    }
                }
                break;
            case 2:
                if (damage > 0)
                {
                    r += "<sprite=54>";

                }
                else if (damage == 0)
                {
                    r += "<sprite=26>";
                    return r;
                }
                else
                {
                    r += "<sprite=53>";
                }
                string s2 = damage.ToString();
                char[] array2 = s2.ToCharArray();
                foreach (var ch in array2)
                {
                    switch (ch)
                    {
                        case '0':
                            r += "<sprite=55>";
                            break;
                        case '1':
                            r += "<sprite=56>";
                            break;
                        case '2':
                            r += "<sprite=59>";
                            break;
                        case '3':
                            r += "<sprite=60>";
                            break;
                        case '4':
                            r += "<sprite=61>";
                            break;
                        case '5':
                            r += "<sprite=62>";
                            break;
                        case '6':
                            r += "<sprite=63>";
                            break;
                        case '7':
                            r += "<sprite=64>";
                            break;
                        case '8':
                            r += "<sprite=65>";
                            break;
                        case '9':
                            r += "<sprite=57>";
                            break;
                    }
                }
                break;
            case 3:
                if (damage > 0)
                {
                    r += "<sprite=52>";

                }
                else if (damage == 0)
                {
                    r += "<sprite=26>";
                    return r;
                }
                else
                {
                    r += "<sprite=51>";
                }
                string s4 = damage.ToString();
                char[] array4 = s4.ToCharArray();
                foreach (var ch in array4)
                {
                    switch (ch)
                    {
                        case '0':
                            r += "<sprite=41>";
                            break;
                        case '1':
                            r += "<sprite=42>";
                            break;
                        case '2':
                            r += "<sprite=43>";
                            break;
                        case '3':
                            r += "<sprite=44>";
                            break;
                        case '4':
                            r += "<sprite=45>";
                            break;
                        case '5':
                            r += "<sprite=46>";
                            break;
                        case '6':
                            r += "<sprite=47>";
                            break;
                        case '7':
                            r += "<sprite=48>";
                            break;
                        case '8':
                            r += "<sprite=49>";
                            break;
                        case '9':
                            r += "<sprite=50>";
                            break;
                    }
                }
                break;
            case 4:
                if (damage < 0)
                {
                    r += "<sprite=25>";

                }
                else if (damage == 0)
                {
                    r += "<sprite=26>";
                    return r;
                }
                else
                {
                    r += "<sprite=24>";
                }
                string s5 = damage.ToString();
                char[] array5 = s5.ToCharArray();
                foreach (var ch in array5)
                {
                    switch (ch)
                    {
                        case '0':
                            r += "<sprite=14>";
                            break;
                        case '1':
                            r += "<sprite=15>";
                            break;
                        case '2':
                            r += "<sprite=16>";
                            break;
                        case '3':
                            r += "<sprite=17>";
                            break;
                        case '4':
                            r += "<sprite=18>";
                            break;
                        case '5':
                            r += "<sprite=19>";
                            break;
                        case '6':
                            r += "<sprite=20>";
                            break;
                        case '7':
                            r += "<sprite=21>";
                            break;
                        case '8':
                            r += "<sprite=22>";
                            break;
                        case '9':
                            r += "<sprite=23>";
                            break;
                    }
                }
                break;
            case 5:
                if (damage > 0)
                {
                    r += "<sprite=66>";

                }
                else if (damage == 0)
                {
                    r += "<sprite=26>";
                    return r;
                }
                else
                {
                    r += "<sprite=77>";
                }
                string s6 = damage.ToString();
                char[] array6 = s6.ToCharArray();
                foreach (var ch in array6)
                {
                    switch (ch)
                    {
                        case '0':
                            r += "<sprite=67>";
                            break;
                        case '1':
                            r += "<sprite=68>";
                            break;
                        case '2':
                            r += "<sprite=69>";
                            break;
                        case '3':
                            r += "<sprite=70>";
                            break;
                        case '4':
                            r += "<sprite=71>";
                            break;
                        case '5':
                            r += "<sprite=72>";
                            break;
                        case '6':
                            r += "<sprite=73>";
                            break;
                        case '7':
                            r += "<sprite=74>";
                            break;
                        case '8':
                            r += "<sprite=75>";
                            break;
                        case '9':
                            r += "<sprite=76>";
                            break;
                    }
                }
                break;

        }

        return r;
    }
    //取得地圖邊界
    public static int[] GetMapBoundary(string scenename)
    {
        //地圖邊界: 上下左右
        int[] trainingArea2 = { 250, -130, -100, 520 };
        int[] studyway = { 20, -150, -1300, 1150 };
        int[] Academy = { 100, -50, -300, 350};
        int[] SwordClassroom = { 161, -4, -18, 209 };
        int[] BowClassroom = {85, -68, 20, 50 };
        int[] MagicClassroom = { 85, -68, 20, 50};
        int[] TheologyClassroom = {85, -68, 20, 50 };
        int[] none = { 0, 0, 0, 0 };
        int[] ArtisamWay = {82,-86, -1812,1998 };
        int[] CentralSquare = {39,-128,-472,262 };
        int[] Harbour = { 311,-9,-2877,1244};
        int[] TravellerStreet = {96,-67,-591,810 };
        int[] Pubs = {72,-93,-128,91 };
        int[] Storage = { 72,-96,-127,91};
        int[] ThiefGuild = { 71, -92, -126, 93 };
        int[] Forge = {70,-95,-42,92 };
        int[] Grocery = { 72, -91, -130, 91 };
        int[] Market = { 119,-43,-332,399};
        int[] ArcherWay = {39 ,-126, -2011, 1797 };
        switch (scenename)
        {
            //持續增加
            case "Studyway":
                return studyway;
            case "TrainingArea2":
                return trainingArea2;
            case "Academy":
                return Academy;
            case "SwordClassroom":
                return SwordClassroom;
            case "BowClassroom":
                return BowClassroom;
            case "MagicClassroom":
                return MagicClassroom;
            case "TheologyClassroom":
                return TheologyClassroom;
            case "ArtisanWay":
                return ArtisamWay;
            case "CentralSquare":
                return CentralSquare;
            case "Harbour":
                return Harbour;
            case "TravellerStreet":
                return TravellerStreet;
            case "Pubs":
                return Pubs;
            case "Storage":
                return Storage;
            case "ThiefGuild":
                return ThiefGuild;
            case "Forge":
                return Forge;
            case "Grocery":
                return Grocery;
            case "Market":
                return Market;
            case "ArcherWay":
                return ArcherWay;
            default:
                return none;
        }
    }

    public static string ProcessFloatStr(float num)
    {
        string Timestr = (num - (num % 0.01f)).ToString();
        if (num < 10)
        {
            if (Timestr.Length == 1)
            {
                Timestr += ".00";
            }
            else if (Timestr.Length == 2)
            {
                Timestr += "00";
            }
            else if (Timestr.Length == 3)
            {
                Timestr += "0";
            }
            Timestr = Timestr.Substring(0, 4);
        }
        else if (num >= 10 & num <= 100)
        {
            if (Timestr.Length == 2)
            {
                Timestr += ".00";
            }
            else if (Timestr.Length == 3)
            {
                Timestr += "00";
            }
            else if (Timestr.Length == 4)
            {
                Timestr += "0";
            }
            Timestr = Timestr.Substring(0, 5);
        }
        else if (num >= 100 & num <= 1000)
        {
            if (Timestr.Length == 3)
            {
                Timestr += ".00";
            }
            else if (Timestr.Length == 4)
            {
                Timestr += "00";
            }
            else if (Timestr.Length == 5)
            {
                Timestr += "0";
            }
            Timestr = Timestr.Substring(0, 6);
        }
        return Timestr;
    }

    public static void SetDictionary<TDicKey, TDicValue>(Dictionary<TDicKey,TDicValue> dic, TDicKey key,TDicValue value )
    {
        if (dic != null)
        {
            if (dic.ContainsKey(key))
            {
                dic[key] = value;
            }
            else
            {
                dic.Add(key, value);
            }
        }
    }
}