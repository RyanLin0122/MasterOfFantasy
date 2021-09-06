using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PEProtocal;

public class DialogueWnd : WindowRoot
{
    public Image NpcImage;
    public Text NpcName;
    public Text NpcDialogue;
    public GameObject Buttons;
    public int NPCID;
    protected override void InitWnd()
    {
        QuestBtn.gameObject.SetActive(false);
        ClassBtn.gameObject.SetActive(false);
        Class2Btn.gameObject.SetActive(false);
        QuizBtn.gameObject.SetActive(false);
        BuyBtn.gameObject.SetActive(false);
        SellBtn.gameObject.SetActive(false);
        LockerBtn.gameObject.SetActive(false);
        MailBoxBtn.gameObject.SetActive(false);
        StrenthenBtn.gameObject.SetActive(false);
        base.InitWnd();

    }
    public void SetWndState(bool isActive, int NpcID)
    {
        if (gameObject.activeSelf != isActive)
        {
            SetActive(gameObject, isActive);
        }
        if (isActive)
        {
            InitWnd();
            SetNPC(NpcID);
        }
        else
        {
            ClearWnd();
        }
    }
    public void LockAllBtn()
    {
        foreach (var item in Buttons.GetComponentsInChildren<Button>())
        {
            item.enabled = false;
        }
    }
    public void ActivateAllBtn()
    {
        foreach (var item in Buttons.GetComponentsInChildren<Button>())
        {
            item.enabled = true;
        } 
    }
    public Button QuestBtn;
    public Button ClassBtn;
    public Button Class2Btn;
    public Button QuizBtn;
    public Button BuyBtn;
    public Button SellBtn;
    public Button LockerBtn;
    public Button MailBoxBtn;
    public Button StrenthenBtn;
    public Button MiniGameBtn;


    public void SetNPC(int NpcId)
    {
        NpcConfig npcCfg = resSvc.GetNpcCfgData(NpcId);
        NPCID = NpcId;
        NpcName.text = npcCfg.Name;
        NpcDialogue.text = npcCfg.FixedText[0];
        SetSprite(NpcImage, "NPC/" + npcCfg.Sprite);
        NpcImage.SetNativeSize();
        //調位置
        //PECommon.Log((18 + (NpcImage.rectTransform.rect.width * 0.15f)).ToString());
        NpcImage.transform.localPosition = new Vector2(-400 + (NpcImage.rectTransform.rect.width * 0.15f), NpcImage.transform.localPosition.y);

        foreach (var item in npcCfg.Functions)
        {
            //根據NPC擁有的功能加載不同按紐
            //任務1 , 學習課程2 , 學習課程3 , 考試4 , 購買5 , 販賣6 , 倉庫7, 郵箱8, 強化裝備9, 小遊戲設定10, 製作裝備11
            switch (item)
            {
                case 1:
                    QuestBtn.gameObject.SetActive(true);
                    break;
                case 2:
                    ClassBtn.gameObject.SetActive(true);                    
                    break;
                case 3:
                    Class2Btn.gameObject.SetActive(true);

                    break;
                case 4:
                    QuizBtn.gameObject.SetActive(true);
                    break;
                case 5:
                    BuyBtn.gameObject.SetActive(true);
                    break;
                case 6:
                    SellBtn.gameObject.SetActive(true);

                    break;
                case 7:
                    LockerBtn.gameObject.SetActive(true);
                    break;
                case 8:
                    MailBoxBtn.gameObject.SetActive(true);
                    break;
                case 9:
                    StrenthenBtn.gameObject.SetActive(true);
                    break;
                case 10:
                    MiniGameBtn.gameObject.SetActive(true);
                    break;
            }
        }
    }
    public void ImportNpcShopItems()
    {
        ShopWnd.Instance.GetSellItemList(NPCID);
    }
    public void EnterMiniGame()
    {
        SetWndState(false);
        Debug.Log("Minigame request!");
        switch (NPCID)
        {
            case 1004:
                GameRoot.Instance.gameObject.GetComponent<GotoMiniGame>().game = GotoMiniGame.MiniGameEnum.card;
                break;
            case 1005:
                GameRoot.Instance.gameObject.GetComponent<GotoMiniGame>().game = GotoMiniGame.MiniGameEnum.archery;
                break;
            case 1006:
                GameRoot.Instance.gameObject.GetComponent<GotoMiniGame>().game = GotoMiniGame.MiniGameEnum.puzzle;
                break;
            case 1007:
                GameRoot.Instance.gameObject.GetComponent<GotoMiniGame>().game = GotoMiniGame.MiniGameEnum.kungfu;
                break;
        }
        GameRoot.Instance.gameObject.GetComponent<GotoMiniGame>().GoToMiniGame();
    }
    public void EnterMiniGame2()
    {
        SetWndState(false);
        Debug.Log("Minigame request!");
        switch (NPCID)
        {
            case 1004:
                GameRoot.Instance.gameObject.GetComponent<GotoMiniGame>().game = GotoMiniGame.MiniGameEnum.cure;
                break;
            case 1005:
                GameRoot.Instance.gameObject.GetComponent<GotoMiniGame>().game = GotoMiniGame.MiniGameEnum.shuriken;
                break;
            case 1006:
                GameRoot.Instance.gameObject.GetComponent<GotoMiniGame>().game = GotoMiniGame.MiniGameEnum.shoot;
                break;
            case 1007:
                GameRoot.Instance.gameObject.GetComponent<GotoMiniGame>().game = GotoMiniGame.MiniGameEnum.dummy;
                break;
        }
        GameRoot.Instance.gameObject.GetComponent<GotoMiniGame>().GoToMiniGame();
    }

}
