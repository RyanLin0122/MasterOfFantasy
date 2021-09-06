using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : WindowRoot
{
    public Image Menu;
    public KeyBind keyBind;
    public ChangeChannelUI changeChannel;
    public Text ChannelNum;
    public bool IsOpen = false;
    public bool IsKeyBind = false;
    protected override void InitWnd()
    {
        PECommon.Log("初始化MenuUI");
        ChannelNum.text = GameRoot.Instance.ActiveChannel.ToString();
        base.InitWnd();
        //TODO
    }
    #region Open or Close Wnd

    public void openCloseWnd()
    {
        
        if (IsOpen == true)
        {
            MainCitySys.Instance.CloseMenuUI();
            IsOpen = false;
        }
        else
        {
            MainCitySys.Instance.OpenMenuUI();
            changeChannel.SetWndState(false);
            keyBind.SetWndState(false);
            IsOpen = true;
        }
    }
    public void ClickCloseBtn()
    {
        MainCitySys.Instance.CloseMenuUI();
        IsOpen = false;
    }

    public void OpenKeyBind()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.MiddleBtn);
        MainCitySys.Instance.CloseMenuUI();
        keyBind.SetWndState();
        IsOpen = false;
        IsKeyBind = true;
    }
    public void CloseKeyBind()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.MiddleBtn);
        keyBind.SetWndState(false);
        IsKeyBind = false;
    }
    public void OpenCloseKeyBind()
    {

        if (IsKeyBind == true)
        {
            CloseKeyBind();
            IsKeyBind = false;
        }
        else
        {
            MainCitySys.Instance.CloseMenuUI();
            OpenKeyBind();
            IsKeyBind = true;
        }
    }
    public void OpenChangeChannelUI()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.MiddleBtn);
        this.SetWndState(false);
        IsOpen = false;
        changeChannel.SetWndState(true);
    }
    #endregion
    public void ClickEndGameBtn()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.MiddleBtn);

        Application.Quit();
    }
}
