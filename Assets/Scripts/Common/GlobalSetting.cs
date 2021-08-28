using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSetting : MonoBehaviour
{
    public Language language = Language.TraChinese;

}

public enum Language
{
    TraChinese,
    SimChinese,
    English,
    Korean
}
