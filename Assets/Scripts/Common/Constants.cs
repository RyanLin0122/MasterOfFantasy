using UnityEngine;
using PEProtocal;
public enum TxtColor
{
    Red,
    Green,
    Blue,
    Yellow
}
public class Constants
{
    #region Server相關
    public const int MagicNumber = 66666;
    public const int Version = 30;
    public const string PUBLIC_KEY = "HaveFun";
    #endregion
    #region 顏色
    private const string ColorRed = "<color=#FF0000FF>";
    private const string ColorGreen = "<color=#00FF00FF>";
    private const string ColorBlue = "<color=#00B4FFFF>";
    private const string ColorYellow = "<color=#FFFF00FF>";
    private const string ColorEnd = "</color>";
    public static Color BrownColor = new Vector4(77,22,2,255);
    public static string Color(string str, TxtColor c)
    {
        string result = "";
        switch (c)
        {
            case TxtColor.Red:
                result = ColorRed + str + ColorEnd;
                break;
            case TxtColor.Green:
                result = ColorGreen + str + ColorEnd;
                break;
            case TxtColor.Blue:
                result = ColorBlue + str + ColorEnd;
                break;
            case TxtColor.Yellow:
                result = ColorYellow + str + ColorEnd;
                break;
        }
        return result;
    }
    #endregion 
    #region
    public static int GetAnimSpeed(PlayerAniType state)
    {
        switch (state)
        {
            case PlayerAniType.Idle:
                return 4;
            case PlayerAniType.Walk:
                return 8;
            case PlayerAniType.Run:
                return 8;
            case PlayerAniType.Hurt:
                return 8;
            case PlayerAniType.Death:
                return 8;
            case PlayerAniType.MagicAttack:
                return 6;
            case PlayerAniType.HorizontalAttack1:
                return 6;
            case PlayerAniType.HorizontalAttack2:
                return 8;
            case PlayerAniType.DownAttack1:
                return 8;
            case PlayerAniType.DownAttack2:
                return 8;
            case PlayerAniType.DaggerAttack:
                return 9;
            case PlayerAniType.BowAttack:
                return 6;
            case PlayerAniType.ClericAttack:
                return 6;
            case PlayerAniType.SlashAttack:
                return 8;
            case PlayerAniType.CrossbowAttack:
                return 6;
            case PlayerAniType.UpperAttack:
                return 6;
        }
        return 0;
    }
    public static string GetDefaultSpritePath(EquipAnimType Type)
    {
        switch (Type)
        {
            //case EquipAnimType.Cape:
            //    return"Character/CharacterSprites";
            case EquipAnimType.Shoes:
                return "Character/CharacterSprites/BaseShoes";
            case EquipAnimType.Suit:
                return "Character/CharacterSprites/Archer set (F)";
            case EquipAnimType.Face:
                return "Character/CharacterSprites/Character Face (F) 1";
            //case EquipAnimType.HairAcc:
            //    return;
            case EquipAnimType.Upwear:
                return "Character/CharacterSprites/coat_01.gi";
            case EquipAnimType.Downwear:
                return "Character/CharacterSprites/0c0000c9_lower01.gi";
            case EquipAnimType.HandBack:
                return "Character/CharacterSprites/0c0000c7_hand_01.gi";
            case EquipAnimType.HandFront:
                return "Character/CharacterSprites/0c0000c7_hand_01.gi";
            case EquipAnimType.HairBack:
                return "Character/CharacterSprites/Default Brown Hair (F) 2";
            case EquipAnimType.HairFront:
                return "Character/CharacterSprites/Default Brown Hair (F) 1";
        }
        return "";
    }
    public static string GetDefaultSpritePath_Female(EquipAnimType Type)
    {
        switch (Type)
        {
            //case EquipAnimType.Cape:
            //    return"Character/CharacterSprites";
            case EquipAnimType.Shoes:
                return "Character/CharacterSprites/BaseShoes";
            case EquipAnimType.Suit:
                return "Character/CharacterSprites/Archer set (F)";
            case EquipAnimType.Face:
                return "Character/CharacterSprites/Character Face (F) 1";
            //case EquipAnimType.HairAcc:
            //    return;
            case EquipAnimType.Upwear:
                return "Character/CharacterSprites/0c0000ca_coat_02.gi";
            case EquipAnimType.Downwear:
                return "Character/CharacterSprites/0c0000ce_lower02.gi";
            case EquipAnimType.HandBack:
                return "Character/CharacterSprites/0c0000c7_hand_01.gi";
            case EquipAnimType.HandFront:
                return "Character/CharacterSprites/0c0000c7_hand_01.gi";
            case EquipAnimType.HairBack:
                return "Character/CharacterSprites/Default Brown Hair (F) 2";
            case EquipAnimType.HairFront:
                return "Character/CharacterSprites/Default Brown Hair (F) 1";
        }
        return "";
    }
    public static string GetDefaultSpritePath_Male(EquipAnimType Type)
    {
        switch (Type)
        {
            //case EquipAnimType.Cape:
            //    return"Character/CharacterSprites";
            case EquipAnimType.Shoes:
                return "Character/CharacterSprites/BaseShoes";
            case EquipAnimType.Suit:
                return "Character/CharacterSprites/Archer set (M)";
            case EquipAnimType.Face:
                return "Character/CharacterSprites/Character Face (M) 1";
            //case EquipAnimType.HairAcc:
            //    return;
            case EquipAnimType.Upwear:
                return "Character/CharacterSprites/coat_01.gi";
            case EquipAnimType.Downwear:
                return "Character/CharacterSprites/0c0000c9_lower01.gi";
            case EquipAnimType.HandBack:
                return "Character/CharacterSprites/0c0000c7_hand_01.gi";
            case EquipAnimType.HandFront:
                return "Character/CharacterSprites/0c0000c7_hand_01.gi";
            case EquipAnimType.HairBack:
                return "Character/CharacterSprites/0c0000ec_hear_01_02.gi";
            case EquipAnimType.HairFront:
                return "Character/CharacterSprites/Default Brown Hair (M)";
        }
        return "";
    }
    public static int GetAnimLength(PlayerAniType state)
    {
        switch (state)
        {
            case PlayerAniType.Idle:
                return 8;
            case PlayerAniType.Walk:
                return 8;
            case PlayerAniType.Run:
                return 8;
            case PlayerAniType.Hurt:
                return 4;
            case PlayerAniType.Death:
                return 8;
            case PlayerAniType.MagicAttack:
                return 4;
            case PlayerAniType.HorizontalAttack1:
                return 4;
            case PlayerAniType.HorizontalAttack2:
                return 4;
            case PlayerAniType.DownAttack1:
                return 4;
            case PlayerAniType.DownAttack2:
                return 4;
            case PlayerAniType.DaggerAttack:
                return 4;
            case PlayerAniType.BowAttack:
                return 4;
            case PlayerAniType.ClericAttack:
                return 4;
            case PlayerAniType.SlashAttack:
                return 4;
            case PlayerAniType.CrossbowAttack:
                return 4;
            case PlayerAniType.UpperAttack:
                return 4;


        }
        return 0;
    }

    public static float GetAnimTime(PlayerAniType state)
    {
        return ((float)GetAnimLength(state)) / GetAnimSpeed(state);
    }

    public static float GetAnimTimeByID(int AnimID)
    {
        switch (AnimID)
        {
            case 0:
                return GetAnimTime(PlayerAniType.Hurt);
            case 1:
                return GetAnimTime(PlayerAniType.Death);
            case 2:
                return GetAnimTime(PlayerAniType.BowAttack);
            default:
                return 0;
        }
        
    }
    public static float GetMonsterAnimTime(int MonsterID, MonsterAniType aniType)
    {
        return ResSvc.Instance.MonsterInfoDic[MonsterID].MonsterAniDic[aniType].AnimSprite.Count/ ResSvc.Instance.MonsterInfoDic[MonsterID].MonsterAniDic[aniType].AnimSpeed;
    }
    
    public static int[] GetAnimOrder(PlayerAniType state, EquipAnimType Type)
    {
        int[] Orders = new int[GetAnimLength(state)];
        switch (Type)
        {
            case EquipAnimType.Shoes:
                switch (state)
                {
                    case PlayerAniType.Idle:
                        return new int[] { 0, 0, 0, 0, 0, 0, 0, 0 };
                    case PlayerAniType.Walk:
                        return new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
                    case PlayerAniType.Run:
                        return new int[] { 9, 10, 40, 11, 12, 13, 14, 15 };
                    case PlayerAniType.Hurt:
                        return new int[] { 27, 28, 29, 30 };
                    case PlayerAniType.Death:
                        return new int[] { 27, 28, 29, 30, 32, 32, 32, 32 };
                    case PlayerAniType.UpperAttack:
                        return new int[] { 24, 25, 26, 26 };
                    case PlayerAniType.SlashAttack:
                        return new int[] { 16, 17, 18, 18 };
                    case PlayerAniType.DaggerAttack:
                        return new int[] { 16, 17, 18, 18 };
                    case PlayerAniType.DownAttack1:
                        return new int[] { 19, 20, 21, 21 };
                    case PlayerAniType.DownAttack2:
                        return new int[] { 31, 23, 21, 21 };
                    case PlayerAniType.HorizontalAttack1:
                        return new int[] { 22, 23, 21, 21 };
                    case PlayerAniType.HorizontalAttack2:
                        return new int[] { 22, 23, 21, 21 };
                    case PlayerAniType.BowAttack:
                        return new int[] { 35, 35, 36, 36 };
                    case PlayerAniType.CrossbowAttack:
                        return new int[] { 37, 37, 38, 38 };
                    case PlayerAniType.MagicAttack:
                        return new int[] { 33, 33, 34, 34 };
                    case PlayerAniType.ClericAttack:
                        return new int[] { 39, 39, 39, 39 };
                }
                break;
            case EquipAnimType.Upwear:
                switch (state)
                {
                    case PlayerAniType.Idle:
                        return new int[] { 0, 1, 2, 1, 0, 1, 2, 1 };
                    case PlayerAniType.Walk:
                        return new int[] { 3, 4, 5, 6, 7, 8, 9, 10 };
                    case PlayerAniType.Run:
                        return new int[] { 11, 12, 13, 14, 15, 16, 17, 18 };
                    case PlayerAniType.Hurt:
                        return new int[] { 35, 36, 37, 38 };
                    case PlayerAniType.Death:
                        return new int[] { 35, 36, 37, 38, 39, 39, 39, 39 };
                    case PlayerAniType.UpperAttack:
                        return new int[] { 31, 32, 32, 33 };
                    case PlayerAniType.SlashAttack:
                        return new int[] { 19, 20, 22, 23 };
                    case PlayerAniType.DaggerAttack:
                        return new int[] { 19, 20, 21, 21 };
                    case PlayerAniType.DownAttack1:
                        return new int[] { 28, 29, 30, 30 };
                    case PlayerAniType.DownAttack2:
                        return new int[] { 50, 51, 52, 53 };
                    case PlayerAniType.HorizontalAttack1:
                        return new int[] { 24, 25, 26, 27 };
                    case PlayerAniType.HorizontalAttack2:
                        return new int[] { 24, 25, 26, 34 };
                    case PlayerAniType.BowAttack:
                        return new int[] { 54, 55, 56, 56 };
                    case PlayerAniType.CrossbowAttack:
                        return new int[] { 47, 48, 49, 47 };
                    case PlayerAniType.MagicAttack:
                        return new int[] { 43, 44, 45, 46 };
                    case PlayerAniType.ClericAttack:
                        return new int[] { 40, 42, 41, 42 };
                }
                break;
            case EquipAnimType.Downwear:
                switch (state)
                {
                    case PlayerAniType.Idle:
                        return new int[] { 0, 1, 2, 1, 0, 1, 2, 1 };
                    case PlayerAniType.Walk:
                        return new int[] { 3, 4, 5, 6, 7, 8, 9, 10 };
                    case PlayerAniType.Run:
                        return new int[] { 11, 12, 13, 14, 15, 16, 17, 18 };
                    case PlayerAniType.Hurt:
                        return new int[] { 33, 34, 35, 36 };
                    case PlayerAniType.Death:
                        return new int[] { 33, 34, 35, 36, 38, 38, 38, 38 };
                    case PlayerAniType.UpperAttack:
                        return new int[] { 29, 30, 31, 32 };
                    case PlayerAniType.SlashAttack:
                        return new int[] { 23, 24, 25, 26 };
                    case PlayerAniType.DaggerAttack:
                        return new int[] { 19, 20, 21, 22 };
                    case PlayerAniType.DownAttack1:
                        return new int[] { 28, 20, 21, 22 };
                    case PlayerAniType.DownAttack2:
                        return new int[] { 37, 20, 21, 22 };
                    case PlayerAniType.HorizontalAttack1:
                        return new int[] { 27, 20, 21, 22 };
                    case PlayerAniType.HorizontalAttack2:
                        return new int[] { 27, 20, 21, 22 };
                    case PlayerAniType.BowAttack:
                        return new int[] { 47, 48, 49, 49 };
                    case PlayerAniType.CrossbowAttack:
                        return new int[] { 46, 46, 46, 46 };
                    case PlayerAniType.MagicAttack:
                        return new int[] { 42, 43, 44, 45 };
                    case PlayerAniType.ClericAttack:
                        return new int[] { 39, 40, 41, 40 };
                }
                break;
            case EquipAnimType.Face:
                switch (state)
                {
                    case PlayerAniType.Idle:
                        return new int[] { 0, 0, 0, 0, 0, 0, 0, 0, };
                    case PlayerAniType.Walk:
                        return new int[] { 0, 0, 0, 0, 0, 0, 0, 0 };
                    case PlayerAniType.Run:
                        return new int[] { 0, 0, 0, 0, 0, 0, 0, 0 };
                    case PlayerAniType.Hurt:
                        return new int[] { 5, 6, 5, 5 };
                    case PlayerAniType.Death:
                        return new int[] { 5, 6, 5, 5, 4, 4, 4, 4 };
                    case PlayerAniType.DaggerAttack:
                        return new int[] { 0, 1, 1, 1 };
                    case PlayerAniType.SlashAttack:
                        return new int[] { 0, 1, 1, 1 };
                    case PlayerAniType.DownAttack1:
                        return new int[] { 0, 1, 1, 1 };
                    case PlayerAniType.DownAttack2:
                        return new int[] { 0, 1, 1, 1 };
                    case PlayerAniType.HorizontalAttack1:
                        return new int[] { 0, 1, 1, 1 };
                    case PlayerAniType.HorizontalAttack2:
                        return new int[] { 0, 1, 1, 1 };
                    case PlayerAniType.UpperAttack:
                        return new int[] { 0, 1, 1, 1 };
                    case PlayerAniType.MagicAttack:
                        return new int[] { 0, 1, 1, 1 };
                    case PlayerAniType.ClericAttack:
                        return new int[] { 3, 2, 2, 2 };
                    case PlayerAniType.BowAttack:
                        return new int[] { 0, 1, 1, 1 };
                    case PlayerAniType.CrossbowAttack:
                        return new int[] { 0, 1, 1, 1 };
                }
                break;
            case EquipAnimType.HairFront:
                switch (state)
                {
                    case PlayerAniType.Idle:
                        return new int[] { 0, 0, 0, 0, 0, 0, 0, 0 };
                    case PlayerAniType.Walk:
                        return new int[] { 3, 4, 5, 3, 4, 6, 5, 3 };
                    case PlayerAniType.Run:
                        return new int[] { 3, 4, 5, 3, 4, 6, 5, 3 };
                    case PlayerAniType.Hurt:
                        return new int[] { 0, 2, 3, 3 };
                    case PlayerAniType.Death:
                        return new int[] { 0, 2, 3, 3, 9, 9, 9, 9 };
                    case PlayerAniType.DaggerAttack:
                        return new int[] { 0, 2, 7, 5 };
                    case PlayerAniType.SlashAttack:
                        return new int[] { 1, 2, 7, 5 };
                    case PlayerAniType.DownAttack1:
                        return new int[] { 1, 2, 7, 5 };
                    case PlayerAniType.DownAttack2:
                        return new int[] { 1, 2, 7, 7 };
                    case PlayerAniType.HorizontalAttack1:
                        return new int[] { 1, 2, 7, 5 };
                    case PlayerAniType.HorizontalAttack2:
                        return new int[] { 0, 2, 7, 5 };
                    case PlayerAniType.UpperAttack:
                        return new int[] { 0, 2, 1, 7 };
                    case PlayerAniType.MagicAttack:
                        return new int[] { 0, 2, 7, 5 };
                    case PlayerAniType.ClericAttack:
                        return new int[] { 0, 2, 7, 0 };
                    case PlayerAniType.BowAttack:
                        return new int[] { 0, 2, 5, 8 };
                    case PlayerAniType.CrossbowAttack:
                        return new int[] { 0, 2, 5, 8 };
                }
                break;
            case EquipAnimType.HairBack:
                switch (state)
                {
                    case PlayerAniType.Idle:
                        return new int[] { 0, 0, 0, 0, 0, 0, 0, 0 };
                    case PlayerAniType.Walk:
                        return new int[] { 3, 4, 5, 3, 4, 6, 5, 3 };
                    case PlayerAniType.Run:
                        return new int[] { 3, 4, 5, 3, 4, 6, 5, 3 };
                    case PlayerAniType.Hurt:
                        return new int[] { 0, 2, 3, 3 };
                    case PlayerAniType.Death:
                        return new int[] { 0, 2, 3, 3, 9, 9, 9, 9 };
                    case PlayerAniType.DaggerAttack:
                        return new int[] { 0, 2, 7, 5 };
                    case PlayerAniType.SlashAttack:
                        return new int[] { 1, 2, 7, 5 };
                    case PlayerAniType.DownAttack1:
                        return new int[] { 1, 2, 7, 5 };
                    case PlayerAniType.DownAttack2:
                        return new int[] { 1, 2, 7, 7 };
                    case PlayerAniType.HorizontalAttack1:
                        return new int[] { 1, 2, 7, 5 };
                    case PlayerAniType.HorizontalAttack2:
                        return new int[] { 0, 2, 7, 5 };
                    case PlayerAniType.UpperAttack:
                        return new int[] { 0, 2, 1, 7 };
                    case PlayerAniType.MagicAttack:
                        return new int[] { 0, 2, 7, 5 };
                    case PlayerAniType.ClericAttack:
                        return new int[] { 0, 2, 7, 0 };
                    case PlayerAniType.BowAttack:
                        return new int[] { 0, 2, 5, 8 };
                    case PlayerAniType.CrossbowAttack:
                        return new int[] { 0, 2, 5, 8 };
                }
                break;
            case EquipAnimType.HandFront:
                switch (state)
                {
                    case PlayerAniType.Idle:
                        return new int[] { 14, 14, 14, 14, 14, 14, 14, 14 };
                    case PlayerAniType.Walk:
                        return new int[] { 7, 2, 7, 8, 10, 9, 15, 8 };
                    case PlayerAniType.Run:
                        return new int[] { 0, 1, 2, 2, 7, 4, 3, 2 };
                    case PlayerAniType.Hurt:
                        return new int[] { 4, 33, 34, 34 };
                    case PlayerAniType.Death:
                        return new int[] { 4, 33, 34, 34, 27, 27, 27, 27 };
                    case PlayerAniType.DaggerAttack:
                        return new int[] { 25, 25, 25, 25 };
                    case PlayerAniType.SlashAttack:
                        return new int[] { 24, 25, 19, 30 };
                    case PlayerAniType.DownAttack1:
                        return new int[] { -1, 26, 26, 26 };
                    case PlayerAniType.DownAttack2:
                        return new int[] { 21, 22, 22, 22 };
                    case PlayerAniType.HorizontalAttack1:
                        return new int[] { -1, 18, 19, 19 };
                    case PlayerAniType.HorizontalAttack2:
                        return new int[] { -1, 19, 19, 19 };
                    case PlayerAniType.UpperAttack:
                        return new int[] { 17, -1, -1, -1 };
                    case PlayerAniType.MagicAttack:
                        return new int[] { 11, 12, 12, 12 };
                    case PlayerAniType.ClericAttack:
                        return new int[] { 16, 16, 16, 16 };
                    case PlayerAniType.BowAttack:
                        return new int[] { 38, 38, 39, 39 };
                    case PlayerAniType.CrossbowAttack:
                        return new int[] { 35, 35, 35, 35 };
                }
                break;
            case EquipAnimType.HandBack:
                switch (state)
                {
                    case PlayerAniType.Walk:
                        return new int[] { -1, -1, -1, 5, 5, 5, 5, 5 };
                    case PlayerAniType.Idle:
                        return new int[] { 5, 5, 5, 5, 5, 5, 5, 5 };
                    case PlayerAniType.Run:
                        return new int[] { -1, -1, -1, 5, 5, 6, 5, 5 };
                    case PlayerAniType.Hurt:
                        return new int[] { 5, 5, 5, 5 };
                    case PlayerAniType.Death:
                        return new int[] { 5, 5, 5, 5, 28, 28, 28, 28 };
                    case PlayerAniType.DaggerAttack:
                        return new int[] { 5, -1, -1, -1 };
                    case PlayerAniType.SlashAttack:
                        return new int[] { 5, -1, -1, 31 };
                    case PlayerAniType.DownAttack1:
                        return new int[] { 5, 5, 5, 5 };
                    case PlayerAniType.DownAttack2:
                        return new int[] { 23, -1, -1, -1 };
                    case PlayerAniType.HorizontalAttack1:
                        return new int[] { 20, 5, 5, 5 };
                    case PlayerAniType.HorizontalAttack2:
                        return new int[] { 20, 5, 5, 29 };
                    case PlayerAniType.UpperAttack:
                        return new int[] { -1, -1, -1, -1 };
                    case PlayerAniType.MagicAttack:
                        return new int[] { 13, 13, 13, 13 };
                    case PlayerAniType.ClericAttack:
                        return new int[] { -1, -1, -1, -1 };
                    case PlayerAniType.BowAttack:
                        return new int[] { 40, 40, 40, 40 };
                    case PlayerAniType.CrossbowAttack:
                        return new int[] { 36, 36, 37, 37 };
                }
                break;
            case EquipAnimType.Suit:
                switch (state)
                {
                    case PlayerAniType.Walk:
                        return new int[] { 3, 4, 5, 6, 7, 8, 9, 10 };
                    case PlayerAniType.Idle:
                        return new int[] { 0, 1, 2, 1, 0, 1, 2, 1 };
                    case PlayerAniType.Run:
                        return new int[] { 11, 12, 13, 14, 15, 16, 17, 18 };
                    case PlayerAniType.Hurt:
                        return new int[] { 57, 58, 59, 60 };
                    case PlayerAniType.Death:
                        return new int[] { 57, 58, 59, 60, 61, 61, 61, 61 };
                    case PlayerAniType.DaggerAttack:
                        return new int[] { 23, 24, 25, 26 };
                    case PlayerAniType.SlashAttack:
                        return new int[] { 23, 24, 40, 41 };
                    case PlayerAniType.DownAttack1:
                        return new int[] { 19, 20, 21, 22 };
                    case PlayerAniType.DownAttack2:
                        return new int[] { 42, 43, 44, 45 };
                    case PlayerAniType.HorizontalAttack1:
                        return new int[] { 27, 28, 29, 30 };
                    case PlayerAniType.HorizontalAttack2:
                        return new int[] { 27, 28, 29, 39 };
                    case PlayerAniType.UpperAttack:
                        return new int[] { 31, 32, 33, 34 };
                    case PlayerAniType.MagicAttack:
                        return new int[] { 46, 47, 48, 49 };
                    case PlayerAniType.ClericAttack:
                        return new int[] { 50, 51, 52, 51 };
                    case PlayerAniType.BowAttack:
                        return new int[] { 35, 36, 37, 38 };
                    case PlayerAniType.CrossbowAttack:
                        return new int[] { 53, 54, 55, 56 };
                }
                break;
        }
        return Orders;
    }
    public static Vector2[] GetAnimPosition(PlayerAniType state, EquipAnimType Type)
    {
        Vector2[] Pos = new Vector2[GetAnimLength(state)];
        switch (Type)
        {
            case EquipAnimType.Shoes:
                switch (state)
                {
                    case PlayerAniType.Idle:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.Walk:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.Run:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.Hurt:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.Death:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.DaggerAttack:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.SlashAttack:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.UpperAttack:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.DownAttack1:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.DownAttack2:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.HorizontalAttack1:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.HorizontalAttack2:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.BowAttack:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.CrossbowAttack:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.MagicAttack:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.ClericAttack:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };

                }
                break;
            case EquipAnimType.Upwear:
                switch (state)
                {
                    case PlayerAniType.Idle:
                        return new Vector2[] { new Vector2(0, -0.1f), new Vector2(-0.5f, 0.1f), new Vector2(-0.8f, 0.2f), new Vector2(-0.5f, 0.2f), new Vector2(0, 0), new Vector2(-0.5f, 0.2f), new Vector2(-0.8f, 0.2f), new Vector2(-0.5f, 0.2f) };
                    case PlayerAniType.Walk:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.Run:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.Hurt:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.Death:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.DaggerAttack:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.SlashAttack:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.UpperAttack:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.DownAttack1:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.DownAttack2:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.HorizontalAttack1:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.HorizontalAttack2:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.BowAttack:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.CrossbowAttack:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.MagicAttack:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.ClericAttack:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };

                }
                break;
            case EquipAnimType.Downwear:
                switch (state)
                {
                    case PlayerAniType.Idle:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.Walk:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.Run:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.Hurt:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.Death:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.DaggerAttack:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.SlashAttack:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.UpperAttack:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.DownAttack1:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(-4.6f, -0.4f), new Vector2(-4.6f, -0.6f), new Vector2(-4.5f, -0.7f) };
                    case PlayerAniType.DownAttack2:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(-4.5f, -0.4f), new Vector2(-4.5f, -0.7f), new Vector2(-4.5f, -0.4f) };
                    case PlayerAniType.HorizontalAttack1:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(-4.5f, -0.4f), new Vector2(-4.5f, -0.4f), new Vector2(-4.5f, -0.4f) };
                    case PlayerAniType.HorizontalAttack2:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(-4.5f, -0.4f), new Vector2(-4.2f, -0.4f), new Vector2(-4.4f, -0.6f) };
                    case PlayerAniType.BowAttack:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.CrossbowAttack:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.MagicAttack:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.ClericAttack:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                }
                break;
            case EquipAnimType.Face:
                switch (state)
                {
                    case PlayerAniType.Walk:
                        return new Vector2[] { new Vector2(0.5f, 0f), new Vector2(4f, 0f), new Vector2(4f, 0f), new Vector2(2.3f, 0f), new Vector2(2.3f, 0f), new Vector2(4.2f, 0.2f), new Vector2(3.6f, 0f), new Vector2(3.3f, 0.2f) };
                    case PlayerAniType.Idle:
                        return new Vector2[] { new Vector2(1f, 0.9f), new Vector2(1f, 0.9f), new Vector2(1f, 0.9f), new Vector2(1f, 0.9f), new Vector2(1f, 0.9f), new Vector2(1f, 0.9f), new Vector2(1f, 0.9f), new Vector2(1f, 0.9f) };
                    case PlayerAniType.Run:
                        return new Vector2[] { new Vector2(5f, 0.9f), new Vector2(5f, 0.9f), new Vector2(6.1f, -1.9f), new Vector2(8.2f, -5.3f), new Vector2(11f, -5.1f), new Vector2(6.4f, -6f), new Vector2(8f, -3.8f), new Vector2(4.3f, 0.1f) };
                    case PlayerAniType.Hurt:
                        return new Vector2[] { new Vector2(0.5f, 0.2f), new Vector2(-3.1f, -4.6f), new Vector2(-2f, -5.5f), new Vector2(1f, -6.1f) };
                    case PlayerAniType.Death:
                        return new Vector2[] { new Vector2(0.5f, 0.2f), new Vector2(-3.1f, -4.6f), new Vector2(-2f, -5.5f), new Vector2(1f, -6.1f), new Vector2(-25.1f, -26f), new Vector2(-25.1f, -26f), new Vector2(-25.1f, -26f), new Vector2(-25.1f, -26f) };
                    case PlayerAniType.DaggerAttack:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(7.4f, -4.8f), new Vector2(7.78f, -5.55f), new Vector2(7.78f, -5.55f) };
                    case PlayerAniType.SlashAttack:
                        return new Vector2[] { new Vector2(1.8f, -0.7f), new Vector2(8.3f, -5.3f), new Vector2(7.7f, -5f), new Vector2(7f, -6.9f) };
                    case PlayerAniType.UpperAttack:
                        return new Vector2[] { new Vector2(-2f, -4f), new Vector2(5.7f, 2.4f), new Vector2(5.7f, 2.4f), new Vector2(5.7f, 2.4f) };
                    case PlayerAniType.DownAttack1:
                        return new Vector2[] { new Vector2(-3.2f, -2f), new Vector2(0.5f, -5.3f), new Vector2(1.2f, -6f), new Vector2(1.2f, -6f) };
                    case PlayerAniType.DownAttack2:
                        return new Vector2[] { new Vector2(-6.8f, 2.7f), new Vector2(1.8f, -6.3f), new Vector2(2.8f, -7.3f), new Vector2(2.8f, -7.3f) };
                    case PlayerAniType.HorizontalAttack1:
                        return new Vector2[] { new Vector2(2.4f, 0.6f), new Vector2(0.1f, -4.9f), new Vector2(2.1f, -5.8f), new Vector2(2.1f, -5.8f) };
                    case PlayerAniType.HorizontalAttack2:
                        return new Vector2[] { new Vector2(1.8f, 0.7f), new Vector2(-0.1f, -4.9f), new Vector2(2.2f, -5.6f), new Vector2(1.1f, -5.9f) };
                    case PlayerAniType.BowAttack:
                        return new Vector2[] { new Vector2(0f, 0.3f), new Vector2(-1.7f, 0.7f), new Vector2(-1.7f, 0.7f), new Vector2(-1.7f, 0.7f) };
                    case PlayerAniType.CrossbowAttack:
                        return new Vector2[] { new Vector2(-2.5f, 0.3f), new Vector2(-2.5f, 0.3f), new Vector2(-4.8f, -0.8f), new Vector2(-4.8f, -0.8f) };
                    case PlayerAniType.MagicAttack:
                        return new Vector2[] { new Vector2(-0.8f, 0.66f), new Vector2(-3.3f, 0.4f), new Vector2(-4f, -0.5f), new Vector2(-4f, -0.5f) };
                    case PlayerAniType.ClericAttack:
                        return new Vector2[] { new Vector2(0f, 1.5f), new Vector2(0f, 1.5f), new Vector2(0f, 1.5f), new Vector2(0f, 1.5f) };
                }
                break;
            case EquipAnimType.HairFront:
                switch (state)
                {
                    case PlayerAniType.Idle:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.Walk:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.Run:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.Hurt:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.Death:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(-0.026f, 0.204f), new Vector2(-0.026f, 0.204f), new Vector2(-0.026f, 0.204f), new Vector2(-0.026f, 0.204f) };
                    case PlayerAniType.DaggerAttack:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.SlashAttack:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.UpperAttack:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.DownAttack1:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.DownAttack2:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.HorizontalAttack1:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.HorizontalAttack2:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.BowAttack:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.CrossbowAttack:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.MagicAttack:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.ClericAttack:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };

                }
                break;
            case EquipAnimType.HairBack:
                switch (state)
                {
                    case PlayerAniType.Idle:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.Walk:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.Run:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.Hurt:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.Death:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.DaggerAttack:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.SlashAttack:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.UpperAttack:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.DownAttack1:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.DownAttack2:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.HorizontalAttack1:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.HorizontalAttack2:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.BowAttack:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.CrossbowAttack:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.MagicAttack:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.ClericAttack:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };

                }
                break;
            case EquipAnimType.HandFront:
                switch (state)
                {
                    case PlayerAniType.Walk:
                        return new Vector2[] { new Vector2(-0.1f, -12.3f), new Vector2(5.5f, -12.3f), new Vector2(1.5f, -12.7f), new Vector2(-5.4f, -13.6f), new Vector2(-6.8f, -10.9f), new Vector2(-9.5f, -9.5f), new Vector2(-7.7f, -10.4f), new Vector2(-5f, -13.5f) };
                    case PlayerAniType.Run:
                        return new Vector2[] { new Vector2(6.8f, -5.7f), new Vector2(14.8f, -1.5f), new Vector2(8.8f, -11.3f), new Vector2(3.9f, -16.2f), new Vector2(-0.8f, -14.2f), new Vector2(-7.7f, -11.7f), new Vector2(-2.3f, -13.9f), new Vector2(1f, -13.5f) };
                    case PlayerAniType.DaggerAttack:
                        return new Vector2[] { new Vector2(-3.4f, -8.6f), new Vector2(19.6f, -11.7f), new Vector2(20.3f, -11.9f), new Vector2(20.3f, -11.9f) };
                    case PlayerAniType.Idle:
                        return new Vector2[] { new Vector2(-9.1f, -13f), new Vector2(-10f, -13f), new Vector2(-10.9f, -13f), new Vector2(-10.1f, -13f), new Vector2(-9.3f, -12.8f), new Vector2(-10.2f, -12.8f), new Vector2(-10.9f, -12.7f), new Vector2(-10.3f, -12.5f) };
                    case PlayerAniType.Hurt:
                        return new Vector2[] { new Vector2(-2.2f, -11.3f), new Vector2(-15.4f, -10f), new Vector2(-15.9f, -12.9f), new Vector2(-7.6f, -16.5f) };
                    case PlayerAniType.Death:
                        return new Vector2[] { new Vector2(-2.2f, -11.3f), new Vector2(-15.4f, -10f), new Vector2(-15.9f, -12.9f), new Vector2(-7.6f, -16.5f), new Vector2(-28.8f, -29.2f), new Vector2(-28.8f, -29.2f), new Vector2(-28.8f, -29.2f), new Vector2(-28.8f, -29.2f) };
                    case PlayerAniType.MagicAttack:
                        return new Vector2[] { new Vector2(23f, -9.1f), new Vector2(23f, -2.2f), new Vector2(21.9f, -2.9f), new Vector2(21.9f, -2.9f) };
                    case PlayerAniType.UpperAttack:
                        return new Vector2[] { new Vector2(-1.5f, -19.9f), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.HorizontalAttack1:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(-16f, -12.1f), new Vector2(-14.2f, -13.2f), new Vector2(-14.2f, -13.2f) };
                    case PlayerAniType.ClericAttack:
                        return new Vector2[] { new Vector2(4.3f, -6f), new Vector2(4.3f, -6f), new Vector2(4.3f, -6f), new Vector2(4.3f, -6f) };
                    case PlayerAniType.DownAttack2:
                        return new Vector2[] { new Vector2(-9.8f, 1.4f), new Vector2(8.3f, -17.6f), new Vector2(8.8f, -18.3f), new Vector2(8.8f, -18.3f) };
                    case PlayerAniType.SlashAttack:
                        return new Vector2[] { new Vector2(-3.5f, -11.2f), new Vector2(20.6f, -11.7f), new Vector2(-7.7f, -13.1f), new Vector2(6.4f, -21.8f) };
                    case PlayerAniType.HorizontalAttack2:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(-15.3f, -12.7f), new Vector2(-13.9f, -12.7f), new Vector2(-14.4f, -12.9f) };
                    case PlayerAniType.DownAttack1:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(8.4f, -17.6f), new Vector2(9.7f, -18.5f), new Vector2(9.7f, -18.5f) };
                    case PlayerAniType.BowAttack:
                        return new Vector2[] { new Vector2(6.3f, -1.6f), new Vector2(-8.8f, -1.4f), new Vector2(-8.8f, -2.7f), new Vector2(-8.8f, -2.7f) };
                    case PlayerAniType.CrossbowAttack:
                        return new Vector2[] { new Vector2(-6.1f, -4f), new Vector2(-6.1f, -4f), new Vector2(-13.4f, -6f), new Vector2(-9f, -4.9f) };

                }
                break;
            case EquipAnimType.HandBack:
                switch (state)
                {
                    case PlayerAniType.Walk:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(7.8f, -13.4f), new Vector2(14.7f, -12.2f), new Vector2(19.1f, -9.3f), new Vector2(11.8f, -10.3f), new Vector2(5.9f, -13.4f) };
                    case PlayerAniType.Run:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(8.8f, -14.3f), new Vector2(18.2f, -14.2f), new Vector2(20.4f, -4.4f), new Vector2(14.6f, -13.8f), new Vector2(8f, -12.4f) };
                    case PlayerAniType.Idle:
                        return new Vector2[] { new Vector2(11f, -9.6f), new Vector2(10.4f, -9.6f), new Vector2(11.7f, -9.7f), new Vector2(11.7f, -9.7f), new Vector2(12.5f, -9.8f), new Vector2(12.5f, -9.8f), new Vector2(11.5f, -9.8f), new Vector2(11.5f, -9.8f) };
                    case PlayerAniType.Hurt:
                        return new Vector2[] { new Vector2(14.6f, -6.5f), new Vector2(7.1f, -5.7f), new Vector2(4.7f, -10.7f), new Vector2(10.1f, -17.2f) };
                    case PlayerAniType.Death:
                        return new Vector2[] { new Vector2(14.6f, -6.5f), new Vector2(7.1f, -5.7f), new Vector2(4.7f, -10.7f), new Vector2(10.1f, -17.2f), new Vector2(5.6f, -13.4f), new Vector2(5.6f, -13.4f), new Vector2(5.6f, -13.4f), new Vector2(5.6f, -13.4f) };
                    case PlayerAniType.DaggerAttack:
                        return new Vector2[] { new Vector2(9f, -9.4f), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.SlashAttack:
                        return new Vector2[] { new Vector2(9.3f, -10.2f), new Vector2(0, 0), new Vector2(0, 0), new Vector2(-7.6f, -5.7f) };
                    case PlayerAniType.UpperAttack:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.DownAttack1:
                        return new Vector2[] { new Vector2(4.4f, -11.1f), new Vector2(3.2f, -14f), new Vector2(4.1f, -15.1f), new Vector2(4.1f, -14.9f) };
                    case PlayerAniType.DownAttack2:
                        return new Vector2[] { new Vector2(-14.8f, -1.1f), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.HorizontalAttack1:
                        return new Vector2[] { new Vector2(11.9f, -2f), new Vector2(6.6f, -16.3f), new Vector2(5.7f, -15.8f), new Vector2(5.5f, -15.5f) };
                    case PlayerAniType.HorizontalAttack2:
                        return new Vector2[] { new Vector2(11.2f, -1.6f), new Vector2(5.2f, -18.5f), new Vector2(8.1f, -18.2f), new Vector2(21.4f, -7.8f) };
                    case PlayerAniType.BowAttack:
                        return new Vector2[] { new Vector2(16.3f, -1.6f), new Vector2(21.3f, -1.8f), new Vector2(21.1f, -1.6f), new Vector2(20.3f, -1.6f) };
                    case PlayerAniType.CrossbowAttack:
                        return new Vector2[] { new Vector2(15.8f, -4.8f), new Vector2(15.8f, -4.8f), new Vector2(8.4f, -7.1f), new Vector2(12.5f, -6.2f) };
                    case PlayerAniType.MagicAttack:
                        return new Vector2[] { new Vector2(-10.6f, -16.2f), new Vector2(-6.7f, -15.4f), new Vector2(-4.6f, -17.1f), new Vector2(-4.4f, -16.8f) };
                    case PlayerAniType.ClericAttack:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };

                }
                break;
            case EquipAnimType.Suit:
                switch (state)
                {
                    case PlayerAniType.Idle:
                        return new Vector2[] { new Vector2(1f, 1.3f), new Vector2(1.3f, 1.3f), new Vector2(1.3f, 1.3f), new Vector2(1.3f, 1.3f), new Vector2(1f, 1.3f), new Vector2(1.3f, 1.3f), new Vector2(1.3f, 1.3f), new Vector2(1.3f, 1.3f) };
                    case PlayerAniType.Walk:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0f, 1f) };
                    case PlayerAniType.Run:
                        return new Vector2[] { new Vector2(0f, -0.7f), new Vector2(0f, -0.7f), new Vector2(0f, -0.7f), new Vector2(-1f, -2.7f), new Vector2(0.5f, -1.7f), new Vector2(0.5f, -1.7f), new Vector2(0.5f, -1.7f), new Vector2(-2f, -0.5f) };
                    case PlayerAniType.Hurt:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.Death:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.DaggerAttack:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.SlashAttack:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.UpperAttack:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.DownAttack1:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.DownAttack2:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.HorizontalAttack1:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.HorizontalAttack2:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.BowAttack:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.CrossbowAttack:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.MagicAttack:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
                    case PlayerAniType.ClericAttack:
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };

                }
                break;
        }
        return Pos;
    }
    #endregion
    #region 場景ID
    public const string SceneLogin = "Login";
    public const int RibiTownID = 1000;
    public const int AcademyID = 1001;
    public const int BeginnerTraining1 = 1002;
    #endregion

    //背景音效名稱
    public const string BGCard = "bg_minigame_03";
    public const string BGLogin = "bg_opening";
    public const string BGMiddleField = "bg_middlefield2";
    public const string BGribiTown = "bg_ribitown";
    public const string BGribiTown2 = "bg_ribitown2";
    public const string BGGreenLawn = "bg_westfield";

    //背景音效名稱2
    public const string EmbiRibi = "embi_ribi";
    public const string EmbiSchool = "embi_school";
    public const string EmbiShop = "embi_shop";
    public const string EmbiSmith = "embi_smith";
    public const string EmbiPub = "embi_pub";
    public const string EmbiDock = "embi_dock";
    public const string EmbiGuild = "embi_guild";
    public const string EmbiGreenLawn = "embi_west";
    //登入按鈕音效
    public const string UILoginBtn = "ui_se_button_large";
    //升級音效
    public const string LevelUpAudio = "charater_se_levelup";
    //藥水音效
    public const string PotionAudio = "ui_item_potion";
    //穿裝備音效
    public const string ClothAudio = "ui_item_cloth";
    public const string ArmorHighAudio = "ui_item_h_armor";
    public const string ArmorLowAudio = "ui_item_l_armor";
    public const string WeaponAudio = "ui_item_weapon";
    public const string AcceceryAudio = "ui_item_acc";
    public const string EtcAudio = "ui_item_etc";
    //UI點擊音效
    public const string LargeBtn = "ui_se_button_large";
    public const string MiddleBtn = "ui_se_button_middle";
    public const string SmallBtn = "ui_se_button_small";
    public const string Setup = "ui_item_box";
    public const string EnchantSuccess = "ui_se_enchant_success";
    public const string EnchantFail = "ui_se_enchant_fail";
    public const string UIEnchant = "ui_se_enchant";
    public const string BookAudio = "ui_item_book";
    public const string WindowOpen = "ui_se_window_open";
    public const string WindowClose = "ui_se_window_close";
    public const string MoneyAudio = "ui_se_trade";
    public const string PickUpItem = "ui_se_tab_chage";
    //小遊戲UI音效
    public const string Time321 = "minigame_se_minig_time1";
    public const string Time0 = "minigame_se_minig_time2";
    public const string TimeUp = "minigame_se_minig_timeup1";
    public const string OpenCard = "minigame_se_minig_cle_cardopen";
    public const string HitBred = "minigame_se_break";
    public const string VerticalPower = "minigame_se_gage";
    public const string Confirm = "minigame_se_faststep";
    public const string StrikeDummy = "minigame_se_hit_strike";
    public const string MissDummy = "minigame_se_minig_mag_hit";
    public const string OpenCureBox = "minigame_se_curebox";
    public const string Recovery = "minigame_se_recoervery";
    public const string CursorMove = "minigame_se_cursormove";
    public const string PatientDie = "minigame_se_die";
    public const string TimeUp2 = "minigame_se_minig_timeup2";
    public const string CardHit = "minigame_se_minig_cle_suc";
    //傳點音效
    public const string PortalAudio = "charater_se_warp";
    //獲取裝備類型
    public static string GetEquipType(EquipmentType equipmentType)
    {
        switch (equipmentType)
        {
            case EquipmentType.Badge:
                return "勳章";
            case EquipmentType.Cape:
                return "披風";
            case EquipmentType.ChatBox:
                return "聊天框";
            case EquipmentType.Chest:
                return "上衣";
            case EquipmentType.FaceType:
                return "臉型";
            case EquipmentType.Glasses:
                return "眼鏡";
            case EquipmentType.Gloves:
                return "手套";
            case EquipmentType.HairAcc:
                return "髮飾";
            case EquipmentType.HairStyle:
                return "髮型";
            case EquipmentType.Head:
                return "帽子";
            case EquipmentType.NameBox:
                return "名牌";
            case EquipmentType.Neck:
                return "項鍊";
            case EquipmentType.Pant:
                return "褲/裙";
            case EquipmentType.Ring:
                return "戒指";
            case EquipmentType.Shield:
                return "盾牌";
            case EquipmentType.Shoes:
                return "鞋子";
            case EquipmentType.Weapon:
                return "武器" +
                    "";
        }
        return "";
    }
    public static string GetWeaponType(WeaponType weaponType)
    {
        switch (weaponType)
        {
            case WeaponType.Axe:
                return "斧頭";
            case WeaponType.Book:
                return "魔法書";
            case WeaponType.Bow:
                return "弓";
            case WeaponType.Cross:
                return "十字架";
            case WeaponType.Crossbow:
                return "十字弓";
            case WeaponType.Dagger:
                return "匕首";
            case WeaponType.DualSword:
                return "雙刀";
            case WeaponType.Gun:
                return "手槍";
            case WeaponType.Hammer:
                return "槌";
            case WeaponType.LongSword:
                return "長劍";
            case WeaponType.Spear:
                return "矛";
            case WeaponType.Staff:
                return "法杖";
            case WeaponType.Sword:
                return "劍";
        }
        return "";
    }
    //透過地圖id取得BGM
    public static string GetBGMByMap(int MapID)
    {
        switch (MapID)
        {
            case 1000:
                return "";
            case 1001:
                return BGribiTown2;
            case 1002:
                return "";
            case 1006:
                return "";
            case 1007:
                return "";
            case 1008:
                return "";
            case 1009:
                return "";
            case 1004:
                return BGribiTown;
            case 1005:
                return BGribiTown;
            case 1010:
                return BGribiTown2;
            case 1011:
                return BGribiTown2;
            case 1012:
                return "";
            case 1013:
                return BGribiTown;
            case 1014:
                return "";
            case 1015:
                return "";
            case 1016:
                return "";
            case 1017:
                return "";
            case 2001:
                return BGGreenLawn;
        }
        return BGLogin;
    }
    public static string GetEmbiByMap(int MapID)
    {
        switch (MapID)
        {
            case 1000:
                return EmbiRibi;
            case 1001:
                return EmbiSchool;
            case 1002:
                return EmbiRibi;
            case 1006:
                return EmbiSchool;
            case 1007:
                return EmbiSchool;
            case 1008:
                return EmbiSchool;
            case 1009:
                return EmbiSchool;
            case 1004:
                return EmbiRibi;
            case 1005:
                return EmbiDock;
            case 1010:
                return EmbiRibi;
            case 1011:
                return EmbiRibi;
            case 1012:
                return EmbiPub;
            case 1013:
                return "";
            case 1014:
                return EmbiGuild;
            case 1015:
                return EmbiShop;
            case 1016:
                return EmbiSmith;
            case 1017:
                return EmbiShop;
            case 2001:
                return EmbiGreenLawn;
        }
        return "";
    }
    //職業代碼
    public static string SetJobName(int JobID)
    {
        string JobName = "";
        switch (JobID)
        {
            default:
                break;
            case 1:
                JobName = "戰士";
                break;
            case 2:
                JobName = "弓箭手";
                break;
            case 3:
                JobName = "法師";
                break;
            case 4:
                JobName = "聖職者";
                break;
            case 101:
                JobName = "神鬼戰士";
                break;
            case 102:
                JobName = "獵人";
                break;
            case 103:
                JobName = "巫師";
                break;
            case 104:
                JobName = "祭司";
                break;
            case 201:
                JobName = "騎士";
                break;
            case 202:
                JobName = "鐵甲武士";
                break;
            case 203:
                JobName = "狙擊手";
                break;
            case 204:
                JobName = "遊俠";
                break;
            case 205:
                JobName = "法櫃巫師";
                break;
            case 206:
                JobName = "黃教巫師";
                break;
            case 207:
                JobName = "聖徒";
                break;
            case 208:
                JobName = "聖騎士";
                break;
            case 301:
                JobName = "將軍";
                break;
            case 302:
                JobName = "英雄";
                break;
            case 303:
                JobName = "神射手";
                break;
            case 304:
                JobName = "神槍手";
                break;
            case 305:
                JobName = "哲人";
                break;
            case 306:
                JobName = "咒術師";
                break;
            case 307:
                JobName = "聖靈判官";
                break;
            case 308:
                JobName = "主教";
                break;
            case 401:
                JobName = "終極鬥士";
                break;
            case 402:
                JobName = "聖堂武士";
                break;
            case 403:
                JobName = "皇家射手";
                break;
            case 404:
                JobName = "槍神";
                break;
            case 405:
                JobName = "賢者";
                break;
            case 406:
                JobName = "冥術師";
                break;
            case 407:
                JobName = "先知";
                break;
            case 408:
                JobName = "教皇";
                break;
        }
        return JobName;
    }
    public static Sprite GetJobImgByID(int JobID)
    {
        string ImgPath = "";
        switch (JobID)
        {
            default:
                break;
            case 1:
                ImgPath = "UI/UISprites/JobImage1_0";
                break;
            case 2:
                ImgPath = "UI/UISprites/JobImage1_1";
                break;
            case 3:
                ImgPath = "UI/UISprites/JobImage1_2";
                break;
            case 4:
                ImgPath = "UI/UISprites/JobImage1_3";
                break;
            case 101:
                ImgPath = "神鬼戰士";
                break;
            case 102:
                ImgPath = "獵人";
                break;
            case 103:
                ImgPath = "巫師";
                break;
            case 104:
                ImgPath = "祭司";
                break;
            case 201:
                ImgPath = "騎士";
                break;
            case 202:
                ImgPath = "鐵甲武士";
                break;
            case 203:
                ImgPath = "狙擊手";
                break;
            case 204:
                ImgPath = "遊俠";
                break;
            case 205:
                ImgPath = "法櫃巫師";
                break;
            case 206:
                ImgPath = "黃教巫師";
                break;
            case 207:
                ImgPath = "聖徒";
                break;
            case 208:
                ImgPath = "聖騎士";
                break;
            case 301:
                ImgPath = "將軍";
                break;
            case 302:
                ImgPath = "英雄";
                break;
            case 303:
                ImgPath = "神射手";
                break;
            case 304:
                ImgPath = "神槍手";
                break;
            case 305:
                ImgPath = "哲人";
                break;
            case 306:
                ImgPath = "咒術師";
                break;
            case 307:
                ImgPath = "聖靈判官";
                break;
            case 308:
                ImgPath = "主教";
                break;
            case 401:
                ImgPath = "終極鬥士";
                break;
            case 402:
                ImgPath = "聖堂武士";
                break;
            case 403:
                ImgPath = "皇家射手";
                break;
            case 404:
                ImgPath = "槍神";
                break;
            case 405:
                ImgPath = "賢者";
                break;
            case 406:
                ImgPath = "冥術師";
                break;
            case 407:
                ImgPath = "先知";
                break;
            case 408:
                ImgPath = "教皇";
                break;
        }
        return Resources.Load<Sprite>(ImgPath);
    }

    public static float GetAttackDistanceByJobID(int job)
    {

        switch (job)
        {
            case 1:
                return 150;
            case 2:
                return 250;
            case 3:
                return 200;
            case 4:
                return 200;
            
            case 101:
                return 150;
            case 102:
                return 250;
            case 103:
                return 200;
            case 104:
                return 200;
            
            case 201:
                return 170;
            case 202:
                return 170;
            case 203:
                return 270;
            case 204:
                return 270;
            case 205:
                return 220;
            case 206:
                return 220;
            case 207:
                return 220;
            case 208:
                return 220;
            
            case 301:
                return 170;
            case 302:
                return 170;
            case 303:
                return 270;
            case 304:
                return 270;
            case 305:
                return 220;
            case 306:
                return 220;
            case 307:
                return 220;
            case 308:
                return 220;

            case 401:
                return 170;
            case 402:
                return 170;
            case 403:
                return 270;
            case 404:
                return 270;
            case 405:
                return 220;
            case 406:
                return 220;
            case 407:
                return 220;
            case 408:
                return 220;
            default:
                return 150;
        }

    }

    public static int GetCommonAttackAnimID(WeaponType weapon)
    {
        switch (weapon)
        {
            case WeaponType.Bow:
                return 3;
            default:
                return 0;
        }

    }

    public static Sprite GetMapLogoByLocation(string location)
    {
        string ImgPath = "";
        switch (location)
        {
            case "GreenLawn":
                ImgPath = "Map/Lawn Green Logo";
                break;
            case "RibiTown":
                ImgPath = "Map/Ribi Town Logo";
                break;

            
        }
        return Resources.Load<Sprite>(ImgPath);
    }
}