using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PEProtocal;
using System.Globalization;

public class PetWnd : Inventory
{
    private static PetWnd _instance;
    public static PetWnd Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = MainCitySys.Instance.petWnd;
            }
            return _instance;
        }
    }

    public bool IsOpen = false;
    public Button CloseBtn;

    public Button InfoBtn;
    public Button SkillBtn;
    public Button ItemsBtn;

    public Sprite BtnSprite1;
    public Sprite BtnSprite2;

    public Text panel1Text;
    public Text panel2Text;
    public Text panel3Text;

    public GameObject panel1;
    public GameObject panel2;
    public GameObject panel3;


    protected override void InitWnd()
    {
        PressInfoBtn();
        base.InitWnd();
    }



    public void ClickCloseBtn()
    {
        MainCitySys.Instance.ClosePetWnd();
        IsOpen = false;
    }

    //public void openCloseWnd()
    //{
    //    if (IsOpen == true)
    //    {
    //        MainCitySys.Instance.CloseCommunityWnd();
    //        IsOpen = false;
    //    }
    //    else
    //    {
    //        MainCitySys.Instance.OpenCommunityWnd();
    //        IsOpen = true;
    //    }
    //}



    #region PressBtn

    public void PressInfoBtn()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);

        InfoBtn.GetComponent<Image>().sprite = BtnSprite2;
        SkillBtn.GetComponent<Image>().sprite = BtnSprite1;
        ItemsBtn.GetComponent<Image>().sprite = BtnSprite1;

        panel1Text.text = "<color=#323232>  情報</color>";
        panel2Text.text = "<color=#4F0D0D>  技能</color>";
        panel3Text.text = "<color=#4F0D0D>  道具</color>";

        panel1.SetActive(true);
        panel2.SetActive(false);
        panel3.SetActive(false);


        InfoBtn.transform.SetAsLastSibling();

        InfoBtn.GetComponent<Image>().raycastTarget = false;
        SkillBtn.GetComponent<Image>().raycastTarget = true;
        ItemsBtn.GetComponent<Image>().raycastTarget = true;

    }
    public void PressSkillBtn()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);

        InfoBtn.GetComponent<Image>().sprite = BtnSprite1;
        SkillBtn.GetComponent<Image>().sprite = BtnSprite2;
        ItemsBtn.GetComponent<Image>().sprite = BtnSprite1;

        panel1Text.text = "<color=#4F0D0D>  情報</color>";
        panel2Text.text = "<color=#323232>  技能</color>";
        panel3Text.text = "<color=#4F0D0D>  道具</color>";
        panel1.SetActive(false);
        panel2.SetActive(true);
        panel3.SetActive(false);

        SkillBtn.transform.SetAsLastSibling();

        InfoBtn.GetComponent<Image>().raycastTarget = true;
        SkillBtn.GetComponent<Image>().raycastTarget = false;
        ItemsBtn.GetComponent<Image>().raycastTarget = true;
    }
    public void PressItemsBtn()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);

        InfoBtn.GetComponent<Image>().sprite = BtnSprite1;
        SkillBtn.GetComponent<Image>().sprite = BtnSprite1;
        ItemsBtn.GetComponent<Image>().sprite = BtnSprite2;

        panel1Text.text = "<color=#4F0D0D>  情報</color>";
        panel2Text.text = "<color=#4F0D0D>  技能</color>";
        panel3Text.text = "<color=#323232>  道具</color>";

        panel1.SetActive(false);
        panel2.SetActive(false);
        panel3.SetActive(true);

        ItemsBtn.transform.SetAsLastSibling();

        InfoBtn.GetComponent<Image>().raycastTarget = true;
        SkillBtn.GetComponent<Image>().raycastTarget = true;
        ItemsBtn.GetComponent<Image>().raycastTarget = false;
    }

    #endregion



}
