using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public abstract class NPC : MonoBehaviour ,INPC
{
    public int NPCID;
    public int[] Functions;   //任務1 , 學習課程2 , 學習技能3 , 考試4 , 購買5 , 販賣6 , 倉庫7 , 信箱8 
    public Image QuestStart;
    public Image QuestEnd;
    GameObject Buttons;
    
    public void LoadDialogue()
    {
        MainCitySys.Instance.baseUI.OpenNpcDialogue(NPCID);
    }
}

