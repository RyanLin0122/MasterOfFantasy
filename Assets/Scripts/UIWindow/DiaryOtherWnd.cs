using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiaryOtherWnd : MonoBehaviour, IMultiLanguageWnd
{
    #region MultiLanguage
    public Text Other_TitleText;


    public void SetLanguage()
    {
        
    }
    #endregion

    #region UI
    public Dropdown TitleDropDown;

    public void ShowTitleOption()
    {
        TitleDropDown.ClearOptions();
        if (GameRoot.Instance.ActivePlayer.TitleCollection != null)
        {
            List<int> Titles = GameRoot.Instance.ActivePlayer.TitleCollection;
            List<string> TitleStrings = new List<string>();
            if (GameRoot.Instance.AccountOption == null)
            {
                GameRoot.Instance.AccountOption = OptionWnd.Instance.GenerateDefaultOption();
            }
            switch (GameRoot.Instance.AccountOption.Language)
            {
                case 0:
                    foreach (var TitleID in Titles)
                    {
                        TitleStrings.Add(ResSvc.Instance.TitleDic[TitleID].Tra_ChineseName);
                    }
                    break;
                case 1:
                    foreach (var TitleID in Titles)
                    {
                        TitleStrings.Add(ResSvc.Instance.TitleDic[TitleID].Sim_ChineseName);
                    }
                    break;
                case 2:
                    foreach (var TitleID in Titles)
                    {
                        TitleStrings.Add(ResSvc.Instance.TitleDic[TitleID].English);
                    }
                    break;
                case 3:
                    foreach (var TitleID in Titles)
                    {
                        TitleStrings.Add(ResSvc.Instance.TitleDic[TitleID].Korean);
                    }
                    break;
                default:
                    foreach (var TitleID in Titles)
                    {
                        TitleStrings.Add(ResSvc.Instance.TitleDic[TitleID].Tra_ChineseName);
                    }
                    break;
            }
            TitleDropDown.AddOptions(TitleStrings);
        }
        
        
    }

    public void ChangeTitle()
    {
        if (GameRoot.Instance.PlayerControl != null)
        {
            GameRoot.Instance.PlayerControl.SetTitle(TitleDropDown.options[TitleDropDown.value].text);
        }
    }
    public void SetUI()
    {
        ShowTitleOption();
    }
    #endregion

}
