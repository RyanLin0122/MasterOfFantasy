using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PEProtocal;

public abstract class NPC : MonoBehaviour, INPC
{
    public int NPCID;
    public SpriteRenderer NPCImg;
    public int[] Functions;   //任務1 , 學習課程2 , 學習技能3 , 考試4 , 購買5 , 販賣6 , 倉庫7 , 信箱8 
    public Image QuestStart;
    public Image QuestEnd;
    public Text NameText;
    GameObject Buttons;

    public void SetNPC(int NPCID, Sprite NPCSprite)
    {
        this.NPCID = NPCID;
        NameText.text = ResSvc.Instance.GetNpcCfgData(NPCID).Name;
        this.NPCImg.sprite = NPCSprite;
        SetQuestStatus();
    }
    public void LoadDialogue()
    {
        UISystem.Instance.baseUI.OpenNpcDialogue(NPCID);
    }

    public void SetQuestStatus()
    {
        QuestStart.gameObject.SetActive(false);
        QuestEnd.gameObject.SetActive(false);
        NPCQuestStatus status = QuestManager.Instance.GetQuestStatusByNpc(NPCID);
        switch (status)
        {
            case NPCQuestStatus.None:
                break;
            case NPCQuestStatus.DeliveryTarget:
                QuestEnd.gameObject.SetActive(true);
                break;
            case NPCQuestStatus.Complete:
                QuestEnd.gameObject.SetActive(true);
                break;
            case NPCQuestStatus.Available:
                QuestStart.gameObject.SetActive(true);
                break;
            case NPCQuestStatus.InComplete:
                break;
            default:
                break;
        }
    }
}

