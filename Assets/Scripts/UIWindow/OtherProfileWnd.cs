using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;
using UnityEngine.UI;

public class OtherProfileWnd : MonoSingleton<OtherProfileWnd>
{
    public Illustration illustration;
    #region UIText
    public Text NameTxt;
    public Text JobTxt;
    public Text LevelTxt;
    public Text GradeTxt;
    public Text TitleTxt;
    public Text GuildTxt;
    public Text BadgeTxt;
    public Text BattlePlayTimesTxt;
    public Text BattleWinTimesTxt;
    public Text BattleLoseTimesTxt;
    public Text BattleRatioTxt;
    public Text PVPRankingTxt;
    public Text PVPPlayTimesTxt;
    public Text PVPWinTimesTxt;
    public Text PVPLoseTimsTxt;
    public Text PVPRatioTxt;
    public Text PVPPointsTxt;
    #endregion
    public void Init()
    {
        this.illustration.InitIllustration();
    }

    public void SetText(OtherProfileOperation otherProfile)
    {
        if (otherProfile != null)
        {
            NameTxt.text = otherProfile.Name;
            JobTxt.text = Constants.SetJobName(otherProfile.Job);
            LevelTxt.text = otherProfile.Level.ToString();
            GradeTxt.text = otherProfile.Grade.ToString();
            TitleTxt.text = otherProfile.Title;
            GuildTxt.text = otherProfile.Guild;
            BadgeTxt.text = otherProfile.CurrentBadge.ToString(); //To Do
            BattlePlayTimesTxt.text = (otherProfile.BattleWinTimes + otherProfile.BattleLoseTimes).ToString();
            BattleWinTimesTxt.text = otherProfile.BattleWinTimes.ToString();
            BattleLoseTimesTxt.text = otherProfile.BattleLoseTimes.ToString();
            BattleRatioTxt.text = (((float)otherProfile.BattleWinTimes / otherProfile.BattleLoseTimes) * 100 % 0.01f).ToString() + " %";
            PVPRankingTxt.text = Constants.GetPVPRankName(otherProfile.PVPRank);
            PVPPlayTimesTxt.text = (otherProfile.PVPWinTimes + otherProfile.PVPLoseTimes).ToString();
            PVPWinTimesTxt.text = otherProfile.PVPWinTimes.ToString();
            PVPLoseTimsTxt.text = otherProfile.PVPLoseTimes.ToString();
            PVPRatioTxt.text = (((float)otherProfile.PVPWinTimes / otherProfile.PVPLoseTimes) * 100 % 0.01f).ToString() + " %";
            PVPPointsTxt.text = otherProfile.PVPPoints.ToString();
            Player player = new Player
            {
                Name = otherProfile.Name,
                playerEquipments = otherProfile.PlayerEquipments,
                Gender = otherProfile.Gender,
                Level = otherProfile.Level
            };
            SetIllustration(player);
        }
    }

    public void SetIllustration(Player player)
    {
        if (player.playerEquipments != null)
        {
            this.illustration.SetGenderAge(true, false, player);
        }
    }

    public void ClickCloseBtn()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        this.gameObject.SetActive(false);
    }
}
