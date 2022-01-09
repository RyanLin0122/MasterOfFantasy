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
    public void Init()
    {
        #region 初始化
        Rabi_Island.gameObject.SetActive(false); //利比島飛龍計程車地圖
       


        #endregion
        if (GameRoot.Instance.ActivePlayer != null)
        {
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
    public void PressPosedinPosition(GameObject temp)
    {
        InventorySys.Instance.HideToolTip();
        Debug.Log("This : " + this.gameObject.name);
        this.gameObject.SetActive(false);
       
        switch(temp.GetComponent<MapElement>().MapID)
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
