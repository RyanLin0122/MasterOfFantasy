using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PEProtocal;

public class DragonTaxiWnd : MonoSingleton<DragonTaxiWnd>
{
    public Button Rabi_Island;
    public Button[] Rabi_BnL;
    public Image[] RibiDisableImgL;
    public void Init()
    {
        foreach(Button R in Rabi_BnL)
        {
            R.interactable = true;
        }
        foreach(Image R in RibiDisableImgL)
        {
            R.gameObject.SetActive(false);
        }
        if (GameRoot.Instance.ActivePlayer != null)
        {
            int mapID = GameRoot.Instance.ActivePlayer.MapID;

            if (mapID < 7000) //在利比島
            {
               switch(mapID)
                {
                    case 1010://旅者之路
                        {
                            Rabi_BnL[0].interactable = false;
                            RibiDisableImgL[0].gameObject.SetActive(true);
                            break;
                        }
                }


                //Rabi_Island.interactable = false;
                //RibiDisableImg.gameObject.SetActive(true);
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

    public void PressPosedinBtn()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.MiddleBtn);
        UISystem.Instance.CloseDialogueWnd();
        this.gameObject.SetActive(false);
        new ShipSender(ShipDestination.Posedin);
    }


}
