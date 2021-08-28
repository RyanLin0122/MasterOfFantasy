using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using PEProtocal;
public class DiaryTrancriptWnd : MonoBehaviour, IMultiLanguageWnd
{
    #region MultiLanguage
    public Text PrincipalText;
    public Text FencingTitle;
    public Text ArcheryTitle;
    public Text MagicTitle;
    public Text TheologyTitle;
    public Text EasyText;
    public Text EasyText1;
    public Text EasyText2;
    public Text EasyText3;
    public Text NormalText;
    public Text NormalText1;
    public Text NormalText2;
    public Text NormalText3;
    public Text HardText;
    public Text HardText1;
    public Text HardText2;
    public Text HardText3;
    public Text HighestScoreTxt;
    public Text HighestScoreTxt1;
    public Text HighestScoreTxt2;
    public Text HighestScoreTxt3;
    public Text TotalScoreTxt;
    public Text TotalScoreTxt1;
    public Text TotalScoreTxt2;
    public Text TotalScoreTxt3;
    #endregion

    #region Score Text
    public Text EasyScore_Fencing;
    public Text NormalScore_Fencing;
    public Text HardScore_Fencing;

    public Text EasyScore_Archery;
    public Text NormalScore_Archery;
    public Text HardScore_Archery;

    public Text EasyScore_Magic;
    public Text NormalScore_Magic;
    public Text HardScore_Magic;

    public Text EasyScore_Theology;
    public Text NormalScore_Theology;
    public Text HardScore_Theology;

    public Text HighestScore_Fencing;
    public Text TotalScore_Fencing;

    public Text HighestScore_Archery;
    public Text TotalScore_Archery;

    public Text HighestScore_Magic;
    public Text TotalScore_Magic;

    public Text HighestScore_Theology;
    public Text TotalScore_Theology;

    #endregion


    public void SetLanguage()
    {
        Dictionary<string, string> Dic = null;
        if (GameRoot.Instance.AccountOption != null)
        {
            switch (GameRoot.Instance.AccountOption.Language)
            {
                case 0:
                    Dic = ResSvc.Instance.Tra_ChineseStrings;
                    break;
                case 1:
                    Dic = ResSvc.Instance.Sim_ChineseStrings;
                    break;
                case 2:
                    Dic = ResSvc.Instance.EnglishStrings;
                    break;
                case 3:
                    Dic = ResSvc.Instance.KoreanStrings;
                    break;
                default:
                    Dic = ResSvc.Instance.Tra_ChineseStrings;
                    break;
            }
        }
        else
        {
            Dic = ResSvc.Instance.Tra_ChineseStrings;
        }

        PrincipalText.text = Dic["Transcript_PrincipalText"];
        FencingTitle.text = Dic["Transcript_FencingTitle"];
        ArcheryTitle.text = Dic["Transcript_ArcheryTitle"];
        MagicTitle.text = Dic["Transcript_MagicTitle"];
        TheologyTitle.text = Dic["Transcript_TheologyTitle"];
        EasyText.text = Dic["Transcript_EasyText"];
        EasyText1.text = Dic["Transcript_EasyText"];
        EasyText2.text = Dic["Transcript_EasyText"];
        EasyText3.text = Dic["Transcript_EasyText"];
        NormalText.text = Dic["Transcript_NormalText"];
        NormalText1.text = Dic["Transcript_NormalText"];
        NormalText2.text = Dic["Transcript_NormalText"];
        NormalText3.text = Dic["Transcript_NormalText"];
        HardText.text = Dic["Transcript_HardText"];
        HardText1.text = Dic["Transcript_HardText"];
        HardText2.text = Dic["Transcript_HardText"];
        HardText3.text = Dic["Transcript_HardText"];
        HighestScoreTxt.text = Dic["Transcript_HighestScoreTxt"];
        HighestScoreTxt1.text = Dic["Transcript_HighestScoreTxt"];
        HighestScoreTxt2.text = Dic["Transcript_HighestScoreTxt"];
        HighestScoreTxt3.text = Dic["Transcript_HighestScoreTxt"];
        TotalScoreTxt.text = Dic["Transcript_TotalScoreTxt"];
        TotalScoreTxt1.text = Dic["Transcript_TotalScoreTxt"];
        TotalScoreTxt2.text = Dic["Transcript_TotalScoreTxt"];
        TotalScoreTxt3.text = Dic["Transcript_TotalScoreTxt"];
    }

    public void SetScores()
    {
        Player p = GameRoot.Instance.ActivePlayer;
        if (p != null)
        {
            EasyScore_Fencing.text = p.EasySuccess[0] + " / " + p.EasyFail[0];
            NormalScore_Fencing.text = p.NormalSuccess[0] + " / " + p.NormalFail[0];
            HardScore_Fencing.text = p.HardSuccess[0] + " / " + p.HardFail[0];

            EasyScore_Archery.text = p.EasySuccess[1] + " / " + p.EasyFail[1];
            NormalScore_Archery.text = p.NormalSuccess[1] + " / " + p.NormalFail[1];
            HardScore_Archery.text = p.HardSuccess[1] + " / " + p.HardFail[1];

            EasyScore_Magic.text = p.EasySuccess[2] + " / " + p.EasyFail[2];
            NormalScore_Magic.text = p.NormalSuccess[2] + " / " + p.NormalFail[2];
            HardScore_Magic.text = p.HardSuccess[2] + " / " + p.HardFail[2];

            EasyScore_Theology.text = p.EasySuccess[3] + " / " + p.EasyFail[3];
            NormalScore_Theology.text = p.NormalSuccess[3] + " / " + p.NormalFail[3];
            HardScore_Theology.text = p.HardSuccess[3] + " / " + p.HardFail[3];

            HighestScore_Fencing.text = p.HighestMiniGameScores[0].ToString();
            TotalScore_Fencing.text = p.TotalMiniGameScores[0].ToString();

            HighestScore_Archery.text = p.HighestMiniGameScores[1].ToString();
            TotalScore_Archery.text = p.TotalMiniGameScores[1].ToString();

            HighestScore_Magic.text = p.HighestMiniGameScores[2].ToString();
            TotalScore_Magic.text = p.TotalMiniGameScores[2].ToString();

            HighestScore_Theology.text = p.HighestMiniGameScores[3].ToString();
            TotalScore_Theology.text = p.TotalMiniGameScores[3].ToString();
        }

    }
}
