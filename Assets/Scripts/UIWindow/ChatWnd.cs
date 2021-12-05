using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PEProtocal;
using System;

public class ChatWnd : WindowRoot
{

    public InputField iptChat;
    public Text txtChat;
    public Image imgAll;
    public Image imgParty;
    public Image imgGuild;
    public Image imgSecret;
    public Sprite Choosed;
    public Sprite Unchoosed;
    public Text ContentText;
    private int chatType = 1;
    private List<string> chatLst_All = new List<string>();
    private List<string> chatLst_Party = new List<string>();
    private List<string> chatLst_Guild = new List<string>();
    private List<string> chatLst_Secret = new List<string>();

    protected override void InitWnd()
    {
        base.InitWnd();
        chatType = 1;
        RefreshUI();
    }
    public void AddNotice(string s)
    {
        if (GetWndState())
        {
            RefreshUI();
        }
    }
    public void AddChatMsg(ProtoMsg msg)
    {
        switch (msg.chatResponse.MessageType)
        {
            case 1:
                print("AddChatMsg case 1");
                chatLst_All.Add(msg.chatResponse.CharacterName + " ： " + msg.chatResponse.Contents);
                if (chatLst_All.Count > 18)
                {
                    chatLst_All.RemoveAt(0);
                }
                if (GetWndState())
                {
                    RefreshUI();
                }
                if (msg.chatResponse.CharacterName == GameRoot.Instance.ActivePlayer.Name)
                {
                    GameRoot.Instance.MainPlayerControl.ShowChatBox(msg.chatResponse.Contents);
                }
                else
                {
                    if (BattleSys.Instance.Players.ContainsKey(msg.chatResponse.CharacterName))
                    {
                        if (BattleSys.Instance.Players[msg.chatResponse.CharacterName] != null)
                        {
                            BattleSys.Instance.Players[msg.chatResponse.CharacterName].ShowChatBox(msg.chatResponse.Contents);
                        }
                    }

                }
                break;
        }
    }
    private bool canSend = true;

    public void ClickSendBtn()
    {
        if (!canSend)
        {
            GameRoot.AddTips("聊天消息每1秒才能發送一條");
            return;
        }

        if (iptChat.text != null && iptChat.text != "" && iptChat.text != " ")
        {
            if (iptChat.text.Length > 24)
            {
                GameRoot.AddTips("輸入內容不可超過24個字");
            }
            else
            {

                switch (chatType)
                {
                    case 1:
                        //發出訊息到伺服器
                        CommandFilter(iptChat.text);
                        new ChatSender(1, iptChat.text);
                        break;
                }
                TimerSvc.Instance.AddTimeTask((int tid) => { canSend = true; }, 1, PETimeUnit.Second);
                canSend = false;
            }
        }

    }

    private void RefreshUI()
    {
        if (chatType == 1) //所有人
        {
            string chatMsg = "";
            for (int i = 0; i < chatLst_All.Count; i++)
            {
                chatMsg += chatLst_All[i] + "\n";
            }
            SetText(txtChat, chatMsg);

            imgAll.sprite = Choosed;
            imgParty.sprite = Unchoosed;
            imgGuild.sprite = Unchoosed;
            imgSecret.sprite = Unchoosed;
        }
        else if (chatType == 2)
        {
            imgAll.sprite = Unchoosed;
            imgParty.sprite = Choosed;
            imgGuild.sprite = Unchoosed;
            imgSecret.sprite = Unchoosed;
        }
        else if (chatType == 3)
        {
            imgAll.sprite = Unchoosed;
            imgParty.sprite = Unchoosed;
            imgGuild.sprite = Choosed;
            imgSecret.sprite = Unchoosed;
        }
        else if (chatType == 4)
        {
            imgAll.sprite = Unchoosed;
            imgParty.sprite = Unchoosed;
            imgGuild.sprite = Unchoosed;
            imgSecret.sprite = Choosed;
        }
    }

    public void ClkAllBtn()
    {
        chatType = 1;
        RefreshUI();
    }
    public void ClkPartyBtn()
    {
        chatType = 2;
        RefreshUI();
    }
    public void ClkGuildBtn()
    {
        chatType = 3;
        RefreshUI();
    }
    public void ClkSecretBtn()
    {
        chatType = 4;
        RefreshUI();
    }

    public void CommandFilter(string s)
    {
        if (s.StartsWith("m "))
        {
            string[] mapstring = s.Split(new char[] { ' ' });
            try
            {
                Commands.GoToMapByPortalID(Convert.ToInt32(mapstring[1]));
            }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);
            }
        }
        if (s == "!Cbag")
        {
            if (GameRoot.Instance.ActivePlayer.NotCashKnapsack != null) GameRoot.Instance.ActivePlayer.NotCashKnapsack.Clear();
            if (GameRoot.Instance.ActivePlayer.CashKnapsack != null) GameRoot.Instance.ActivePlayer.CashKnapsack.Clear();
            foreach (var slots in KnapsackWnd.Instance.slotLists)
            {
                foreach (var slot in slots)
                {
                    if (slot.transform.childCount > 0)
                        Destroy(slot.GetComponentInChildren<ItemUI>().gameObject);
                }
            }
        }
    }


}
