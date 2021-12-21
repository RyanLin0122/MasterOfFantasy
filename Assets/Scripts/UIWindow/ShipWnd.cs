using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PEProtocal;

public class ShipWnd : MonoSingleton<ShipWnd>
{
    public Button RibiBtn;
    public Button PosedinBtn;
    public Button ArnosBtn;

    public Image RibiDisableImg;
    public Image PosedinDisableImg;
    public Image ArnosDisableImg;

    public void Init()
    {
        RibiBtn.interactable = true;
        PosedinBtn.interactable = true;
        ArnosBtn.interactable = true;
        RibiDisableImg.gameObject.SetActive(false);
        PosedinDisableImg.gameObject.SetActive(false);
        ArnosDisableImg.gameObject.SetActive(false);

        if (GameRoot.Instance.ActivePlayer != null)
        {
            int mapID = GameRoot.Instance.ActivePlayer.MapID;
            if (mapID < 7000) //在利比島
            {
                RibiBtn.interactable = false;
                RibiDisableImg.gameObject.SetActive(true);
            }
            else if(mapID>7000 && mapID < 8000) //在幽靈船
            {
                PosedinBtn.interactable = false;
                PosedinDisableImg.gameObject.SetActive(true);
            }
        }
    }
    public void PressCloseBtn()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.MiddleBtn);
        this.gameObject.SetActive(false);
    }

    public void PressPosedinBtn()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.MiddleBtn);
        UISystem.Instance.CloseDialogueWnd();
        this.gameObject.SetActive(false);
        new ShipSender(ShipDestination.Posedin);
    }

    public void PressRibiBtn()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.MiddleBtn);
        UISystem.Instance.CloseDialogueWnd();
        this.gameObject.SetActive(false);
        new ShipSender(ShipDestination.Ribi);
    }

    public void PressArnosBtn()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.MiddleBtn);
        UISystem.Instance.CloseDialogueWnd();
        this.gameObject.SetActive(false);
        new ShipSender(ShipDestination.Arnos);
    }
}
