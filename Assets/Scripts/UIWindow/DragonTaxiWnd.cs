using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PEProtocal;
using UnityEngine.EventSystems;

public class DragonTaxiWnd : MonoSingleton<DragonTaxiWnd>
{
    public Button Rabi_Island;
    public Button[] Rabi_BnL;
    public Image[] RibiDisableImgL;
    public Image[] RibiToolTipL;
    public bool IsPortalSelect;
    public void Init()
    {
        #region 初始化
        Rabi_Island.gameObject.SetActive(false); //利比島飛龍計程車地圖



        #endregion
        if (GameRoot.Instance.ActivePlayer != null)
        {
            IsPortalSelect = false;
            int mapID = GameRoot.Instance.ActivePlayer.MapID;

            if (mapID < 7000) //在利比島
            {
                Rabi_Island.gameObject.SetActive(true);
                foreach (Button RB in Rabi_BnL)
                {
                    RB.interactable = true;
                }
                foreach (Image RI in RibiDisableImgL)
                {
                    RI.gameObject.SetActive(false);
                }
                foreach (Image RI in RibiToolTipL)
                {
                    RI.gameObject.SetActive(false);

                }
                switch (mapID)
                {
                    case 1010://旅者之路
                        {
                            Rabi_BnL[0].interactable = false;
                            RibiDisableImgL[0].gameObject.SetActive(true);
                            break;
                        }
                    case 2012://廢礦監視塔
                        {
                            Rabi_BnL[1].interactable = false;
                            RibiDisableImgL[1].gameObject.SetActive(true);
                            break;
                        }
                    case 3015://西部監視塔
                        {
                            Rabi_BnL[2].interactable = false;
                            RibiDisableImgL[2].gameObject.SetActive(true);
                            break;
                        }
                }
            }
            //else if(mapID>7000 && mapID < 8000) //在幽靈船
            //{
            //    PosedinBtn.interactable = false;
            //    PosedinDisableImg.gameObject.SetActive(true);
            //}
        }
    }
    public void PressCloseBtn()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        this.gameObject.SetActive(false);
    }

    //點選飛龍計程車地圖上的傳送地點事件
    private void PressPosedinPosition(GameObject temp)
    {
        InventorySys.Instance.HideToolTip();
        Debug.Log("This : " + this.gameObject.name);
        this.gameObject.SetActive(false);

        string MapName = temp.GetComponent<MapElement>().MapName;
        PostMapID = temp.GetComponent<MapElement>().MapID;
        switch (temp.GetComponent<MapElement>().MapID)
        {
            case 1010://旅者之路
                {
                    ChangeDragonTaxiNPCText(MapName, 500);
                    //new DragonTaxiSender(DragonTaxiDestination.TravellerWay);
                    break;
                }
            case 2012://廢礦監視塔
                {
                    ChangeDragonTaxiNPCText(MapName, 500);
                    //new DragonTaxiSender(DragonTaxiDestination.MinePost);
                    break;
                }
            case 3015://西部監視塔
                {
                    ChangeDragonTaxiNPCText(MapName, 500);
                    //new DragonTaxiSender(DragonTaxiDestination.WestPost);
                    break;
                }
        }


    }

    public void ChangeDragonTaxiNPCText(string MapName, int Money)
    {
        Debug.Log("開始更改NPC文字");
        DialogueWnd NPCWnd = UISystem.Instance.dialogueWnd;
        Debug.Log("原先NPC文字 : " + NPCWnd.NpcDialogue.text);

        NPCWnd.NpcDialogue.text = "要前往["+MapName+"]是嗎?費用是"+Money+"利比";
        Debug.Log("更改後NPC文字 : " + NPCWnd.NpcDialogue.text);
        IsPortalSelect = true;
    }
    
    public int PostMapID;
    public void StartPost()
    {
        UISystem.Instance.CloseDialogueWnd();
        this.gameObject.SetActive(false);
        IsPortalSelect = false;
        switch (PostMapID)
        {
            case 1010://旅者之路
                {
                    new DragonTaxiSender(DragonTaxiDestination.TravellerWay);
                    break;
                }
            case 2012://廢礦監視塔
                {
                    new DragonTaxiSender(DragonTaxiDestination.MinePost);
                    break;
                }
            case 3015://西部監視塔
                {
                    new DragonTaxiSender(DragonTaxiDestination.WestPost);
                    break;
                }
        }
    }
}
