using UnityEngine;
using UnityEngine.UI;
using PEProtocal;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Collections.Generic;
using System.IO;

public class LoginWnd : WindowRoot
{
    public InputField iptAcct;
    public InputField iptPass;
    public Toggle toggle;
    public Button btnEnter;

    protected override void InitWnd()
    {
        base.InitWnd();

        //Get local account and password
        if (PlayerPrefs.HasKey("Acct") && PlayerPrefs.HasKey("Pass"))
        {
            iptAcct.text = PlayerPrefs.GetString("Acct");
            iptPass.text = PlayerPrefs.GetString("Pass");
        }
        else
        {
            iptAcct.text = "";
            iptPass.text = "";
        }
    }

    private string GetPublicIpAddress()
    {
        var request = (HttpWebRequest)WebRequest.Create("http://ifconfig.me");

        request.UserAgent = "curl"; // this simulate curl linux command

        string publicIPAddress;

        request.Method = "GET";
        using (WebResponse response = request.GetResponse())
        {
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                publicIPAddress = reader.ReadToEnd();
            }
        }

        return publicIPAddress.Replace("n", "");
    }

    public void ClickExitBtn()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        Application.Quit();
    }
    public void ClickEnterBtn()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.LargeBtn);
        GameRoot.Instance.WindowLock();
        string acct = iptAcct.text;
        string pass = iptPass.text;
        List<string> macs = (from nic in NetworkInterface.GetAllNetworkInterfaces()
                             where nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet
                             select nic.GetPhysicalAddress().ToString()).ToList<string>();

        if (acct != "" && pass != "")
        {
            if (pass.Length >= 6 && acct.Length >= 6)
            {
                if (toggle.isOn)
                {
                    //Update local account and password
                    PlayerPrefs.SetString("Acct", acct);
                    PlayerPrefs.SetString("Pass", pass);

                    //ask for login request
                    if (macs != null && macs.Count > 0)
                    {
                        LoginSender sender = new LoginSender(acct, pass, macs[0], GetPublicIpAddress());
                    }
                    else
                    {
                        LoginSender sender = new LoginSender(acct, pass, "NoMac", GetPublicIpAddress());
                    }
                    GameRoot.Instance.Account = acct;
                    GameRoot.Instance.Password = pass;
                }
                else
                {
                    PlayerPrefs.SetString("Acct", "");
                    PlayerPrefs.SetString("Pass", "");

                    if (macs != null && macs.Count > 0)
                    {
                        LoginSender sender = new LoginSender(acct, pass, macs[0], GetPublicIpAddress());
                    }
                    else
                    {
                        LoginSender sender = new LoginSender(acct, pass, "NoMac", GetPublicIpAddress());
                    }
                    GameRoot.Instance.Account = acct;
                    GameRoot.Instance.Password = pass;
                }

            }
            else
            {
                GameRoot.AddTips("帳號與密碼不得小於6個字XD");
                GameRoot.Instance.WindowUnlock();
            }
        }
        else
        {
            GameRoot.AddTips("帳號或密碼不可空白");
            GameRoot.Instance.WindowUnlock();
        }

    }

}
