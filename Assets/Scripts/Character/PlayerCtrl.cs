using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PEProtocal;
using System;
using PolyNav;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

public class Controllable : MonoBehaviour
{
    public string PlayerName;
}
public class PlayerCtrl : Controllable
{
    public new Transform transform;
    private new Rigidbody2D rigidbody;
    public Transform Shadow;
    public GameObject NameBox;
    public bool IsRun = false;
    public Text ChatBoxTxt;
    public GameObject ChatBox;
    public ScreenController screenCtrl;
    public Image HpBar;
    public Text Title;
    public bool IsMoving = false;
    public Sprite[] DustSprites;
    void Awake()
    {
        rigidbody = this.GetComponent<Rigidbody2D>();
        rigidbody.freezeRotation = true;
        PlayerName = GameRoot.Instance.ActivePlayer.Name;
        DustSprites = Resources.LoadAll<Sprite>("Effect/Dust/Effect Walking Car Dust");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Blackboard blackboard = GetComponent<Blackboard>();
            NodeCanvas.BehaviourTrees.BehaviourTreeOwner tree = GetComponent<NodeCanvas.BehaviourTrees.BehaviourTreeOwner>();
            print(blackboard.GetVariable<bool>("IsRun").value);
            blackboard.SetVariableValue("IsRun", !blackboard.GetVariable<bool>("IsRun").value);
            print(blackboard.GetVariable<bool>("IsRun").value);
            if (blackboard.GetVariable<bool>("IsRun").value)
            {
                blackboard.SetVariableValue("Speed", 300f);
                GetComponent<PolyNavAgent>().maxSpeed = 300f;
            }
            else
            {
                blackboard.SetVariableValue("Speed", 200f);
                GetComponent<PolyNavAgent>().maxSpeed = 200f;
            }
            tree.RestartBehaviour();

        }
        if (!IsMoving && screenCtrl.canCtrl)
        {
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
            {
                IsMoving = true;
                InstantiateDust();
            }
        }
        else
        {
            if (!screenCtrl.canCtrl)
            {
                IsMoving = false;
                return;
            }
            if (!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
            {
                IsMoving = false;
                return;
            }
        }
        
    }
    public void ChangeDustSprite(int CapeID)
    {

    }
    public void InstantiateDust()
    {
        if (IsMoving)
        {
            GameObject go = Instantiate((GameObject)Resources.Load("Prefabs/DustPrefab"));
            go.transform.SetParent(MainCitySys.Instance.MapCanvas.transform);
            int Sign = transform.localScale.x >= 0 ? -1 : 1;
            go.transform.localPosition = new Vector3(transform.localPosition.x + 12 * Sign, transform.localPosition.y - 35f, transform.localPosition.z);
            go.transform.localScale = new Vector3(30, 30, 1);
            DustAnimator ani = go.GetComponent<DustAnimator>();
            int spriteIndex =Tools.RDInt(0, DustSprites.Length-1);
            ani.Initialize(DustSprites[spriteIndex]);
            int t = TimerSvc.Instance.AddTimeTask((a) => {InstantiateDust();}, 0.13f, PETimeUnit.Second, 1);
        }
    }
    #region player animation
    public EquipmentAnimator ShoesCtrl;
    public EquipmentAnimator FaceCtrl;
    public EquipmentAnimator UpwearCtrl;
    public EquipmentAnimator DownwearCtrl;
    public EquipmentAnimator HairFrontCtrl;
    public EquipmentAnimator HairBackCtrl;
    public EquipmentAnimator HandBackCtrl;
    public EquipmentAnimator HandFrontCtrl;
    public EquipmentAnimator SuitCtrl;
    public void ChangeEquipment(int id, EquipmentType equipType)
    {
        switch (equipType)
        {
            case EquipmentType.Shoes:
                ShoesCtrl.LoadSprite(ResSvc.Instance.GetEquipSpritePath(id)[0]);
                break;
            case EquipmentType.Pant:
                DownwearCtrl.LoadSprite(ResSvc.Instance.GetEquipSpritePath(id)[0]);
                break;
            case EquipmentType.Chest:
                if (id < 7000)
                {
                    SuitCtrl.gameObject.SetActive(false);
                    ShoesCtrl.gameObject.SetActive(true);
                    UpwearCtrl.gameObject.SetActive(true);
                    DownwearCtrl.gameObject.SetActive(true);
                    HandBackCtrl.gameObject.SetActive(true);
                    HandFrontCtrl.gameObject.SetActive(true);
                    //SuitCtrl.LoadDefaultSprite();
                    UpwearCtrl.LoadSprite(ResSvc.Instance.GetEquipSpritePath(id)[0]);
                }
                else
                {
                    SuitCtrl.gameObject.SetActive(true);
                    ShoesCtrl.gameObject.SetActive(false);
                    UpwearCtrl.gameObject.SetActive(false);
                    DownwearCtrl.gameObject.SetActive(false);
                    HandBackCtrl.gameObject.SetActive(false);
                    HandFrontCtrl.gameObject.SetActive(false);
                    SuitCtrl.LoadSprite(ResSvc.Instance.GetEquipSpritePath(id)[0]);
                    //UpwearCtrl.LoadDefaultSprite();
                }
                break;
            case EquipmentType.FaceType:
                FaceCtrl.LoadSprite(ResSvc.Instance.GetEquipSpritePath(id)[0]);
                break;
            case EquipmentType.Gloves:
                HandBackCtrl.LoadSprite(ResSvc.Instance.GetEquipSpritePath(id)[0]);
                HandFrontCtrl.LoadSprite(ResSvc.Instance.GetEquipSpritePath(id)[0]);
                break;
            case EquipmentType.HairStyle:
                HairFrontCtrl.LoadSprite(ResSvc.Instance.GetEquipSpritePath(id)[0]);
                HairBackCtrl.LoadSprite(ResSvc.Instance.GetEquipSpritePath(id)[1]);
                break;
        }
        PlayIdle();
    }
    public void ChangeDefaultEquipment(int Gender, EquipmentType equipType)
    {
        switch (equipType)
        {
            case EquipmentType.Shoes:
                ShoesCtrl.LoadDefaultSprite(Gender);
                break;
            case EquipmentType.Pant:
                DownwearCtrl.LoadDefaultSprite(Gender);
                break;
            case EquipmentType.Chest:
                UpwearCtrl.LoadDefaultSprite(Gender);
                SuitCtrl.LoadDefaultSprite(Gender);
                break;
            case EquipmentType.FaceType:
                FaceCtrl.LoadDefaultSprite(Gender);
                break;
            case EquipmentType.Gloves:
                HandBackCtrl.LoadDefaultSprite(Gender);
                HandFrontCtrl.LoadDefaultSprite(Gender);
                break;
            case EquipmentType.HairStyle:
                HairFrontCtrl.LoadDefaultSprite(Gender);
                HairBackCtrl.LoadDefaultSprite(Gender);
                break;

        }
        PlayIdle();
    }
    public void InitCtrls()
    {
        ShoesCtrl.Init();
        FaceCtrl.Init();
        UpwearCtrl.Init();
        DownwearCtrl.Init();
        HairBackCtrl.Init();
        HairFrontCtrl.Init();
        HandBackCtrl.Init();
        HandFrontCtrl.Init();
        SuitCtrl.Init();
    }
    public void PlayIdle()
    {
        screenCtrl.canCtrl = true;
        ShoesCtrl.PlayAni(EquipAnimState.Idle, true);
        FaceCtrl.PlayAni(EquipAnimState.Idle, true);
        UpwearCtrl.PlayAni(EquipAnimState.Idle, true);
        DownwearCtrl.PlayAni(EquipAnimState.Idle, true);
        HairBackCtrl.PlayAni(EquipAnimState.Idle, true);
        HairFrontCtrl.PlayAni(EquipAnimState.Idle, true);
        HandBackCtrl.PlayAni(EquipAnimState.Idle, true);
        HandFrontCtrl.PlayAni(EquipAnimState.Idle, true);
        SuitCtrl.PlayAni(EquipAnimState.Idle, true);
    }
    public void PlayWalk()
    {
        screenCtrl.canCtrl = true;
        ShoesCtrl.PlayAni(EquipAnimState.Walk, true);
        FaceCtrl.PlayAni(EquipAnimState.Walk, true);
        UpwearCtrl.PlayAni(EquipAnimState.Walk, true);
        DownwearCtrl.PlayAni(EquipAnimState.Walk, true);
        HairBackCtrl.PlayAni(EquipAnimState.Walk, true);
        HairFrontCtrl.PlayAni(EquipAnimState.Walk, true);
        HandBackCtrl.PlayAni(EquipAnimState.Walk, true);
        HandFrontCtrl.PlayAni(EquipAnimState.Walk, true);
        SuitCtrl.PlayAni(EquipAnimState.Walk, true);
    }
    public void PlayRun()
    {
        screenCtrl.canCtrl = true;
        ShoesCtrl.PlayAni(EquipAnimState.Run, true);
        FaceCtrl.PlayAni(EquipAnimState.Run, true);
        UpwearCtrl.PlayAni(EquipAnimState.Run, true);
        DownwearCtrl.PlayAni(EquipAnimState.Run, true);
        HairBackCtrl.PlayAni(EquipAnimState.Run, true);
        HairFrontCtrl.PlayAni(EquipAnimState.Run, true);
        HandBackCtrl.PlayAni(EquipAnimState.Run, true);
        HandFrontCtrl.PlayAni(EquipAnimState.Run, true);
        SuitCtrl.PlayAni(EquipAnimState.Run, true);
    }
    public void PlayDagger()
    {
        screenCtrl.canCtrl = false;
        ShoesCtrl.PlayAni(EquipAnimState.DaggerAttack, false);
        FaceCtrl.PlayAni(EquipAnimState.DaggerAttack, false);
        UpwearCtrl.PlayAni(EquipAnimState.DaggerAttack, false);
        DownwearCtrl.PlayAni(EquipAnimState.DaggerAttack, false);
        HairBackCtrl.PlayAni(EquipAnimState.DaggerAttack, false);
        HairFrontCtrl.PlayAni(EquipAnimState.DaggerAttack, false);
        HandBackCtrl.PlayAni(EquipAnimState.DaggerAttack, false);
        HandFrontCtrl.PlayAni(EquipAnimState.DaggerAttack, false);
        SuitCtrl.PlayAni(EquipAnimState.DaggerAttack, false);
    }
    public void PlaySlash()
    {
        screenCtrl.canCtrl = false;
        ShoesCtrl.PlayAni(EquipAnimState.SlashAttack, false);
        FaceCtrl.PlayAni(EquipAnimState.SlashAttack, false);
        UpwearCtrl.PlayAni(EquipAnimState.SlashAttack, false);
        DownwearCtrl.PlayAni(EquipAnimState.SlashAttack, false);
        HairBackCtrl.PlayAni(EquipAnimState.SlashAttack, false);
        HairFrontCtrl.PlayAni(EquipAnimState.SlashAttack, false);
        HandBackCtrl.PlayAni(EquipAnimState.SlashAttack, false);
        HandFrontCtrl.PlayAni(EquipAnimState.SlashAttack, false);
        SuitCtrl.PlayAni(EquipAnimState.SlashAttack, false);
    }
    public void PlayUpper()
    {
        screenCtrl.canCtrl = false;
        ShoesCtrl.PlayAni(EquipAnimState.UpperAttack, false);
        FaceCtrl.PlayAni(EquipAnimState.UpperAttack, false);
        UpwearCtrl.PlayAni(EquipAnimState.UpperAttack, false);
        DownwearCtrl.PlayAni(EquipAnimState.UpperAttack, false);
        HairBackCtrl.PlayAni(EquipAnimState.UpperAttack, false);
        HairFrontCtrl.PlayAni(EquipAnimState.UpperAttack, false);
        HandBackCtrl.PlayAni(EquipAnimState.UpperAttack, false);
        HandFrontCtrl.PlayAni(EquipAnimState.UpperAttack, false);
        SuitCtrl.PlayAni(EquipAnimState.UpperAttack, false);
    }
    public void PlayHurt()
    {
        screenCtrl.canCtrl = false;
        ShoesCtrl.PlayAni(EquipAnimState.Hurt, false);
        FaceCtrl.PlayAni(EquipAnimState.Hurt, false);
        UpwearCtrl.PlayAni(EquipAnimState.Hurt, false);
        DownwearCtrl.PlayAni(EquipAnimState.Hurt, false);
        HairBackCtrl.PlayAni(EquipAnimState.Hurt, false);
        HairFrontCtrl.PlayAni(EquipAnimState.Hurt, false);
        HandBackCtrl.PlayAni(EquipAnimState.Hurt, false);
        HandFrontCtrl.PlayAni(EquipAnimState.Hurt, false);
        SuitCtrl.PlayAni(EquipAnimState.Hurt, false);
    }
    public void ReLive()
    {
        screenCtrl.canCtrl = false;
        PlayIdle();
    }
    public void PlayDeath()
    {
        IsDeath = true;
        screenCtrl.canCtrl = false;
        ShoesCtrl.PlayAni(EquipAnimState.Death, false);
        FaceCtrl.PlayAni(EquipAnimState.Death, false);
        UpwearCtrl.PlayAni(EquipAnimState.Death, false);
        DownwearCtrl.PlayAni(EquipAnimState.Death, false);
        HairBackCtrl.PlayAni(EquipAnimState.Death, false);
        HairFrontCtrl.PlayAni(EquipAnimState.Death, false);
        HandBackCtrl.PlayAni(EquipAnimState.Death, false);
        HandFrontCtrl.PlayAni(EquipAnimState.Death, false);
        SuitCtrl.PlayAni(EquipAnimState.Death, false);
    }
    public void PlayMagic()
    {
        screenCtrl.canCtrl = false;
        ShoesCtrl.PlayAni(EquipAnimState.MagicAttack, false);
        FaceCtrl.PlayAni(EquipAnimState.MagicAttack, false);
        UpwearCtrl.PlayAni(EquipAnimState.MagicAttack, false);
        DownwearCtrl.PlayAni(EquipAnimState.MagicAttack, false);
        HairBackCtrl.PlayAni(EquipAnimState.MagicAttack, false);
        HairFrontCtrl.PlayAni(EquipAnimState.MagicAttack, false);
        HandBackCtrl.PlayAni(EquipAnimState.MagicAttack, false);
        HandFrontCtrl.PlayAni(EquipAnimState.MagicAttack, false);
        SuitCtrl.PlayAni(EquipAnimState.MagicAttack, false);
    }
    public void PlayCleric()
    {
        screenCtrl.canCtrl = false;
        ShoesCtrl.PlayAni(EquipAnimState.ClericAttack, false);
        FaceCtrl.PlayAni(EquipAnimState.ClericAttack, false);
        UpwearCtrl.PlayAni(EquipAnimState.ClericAttack, false);
        DownwearCtrl.PlayAni(EquipAnimState.ClericAttack, false);
        HairBackCtrl.PlayAni(EquipAnimState.ClericAttack, false);
        HairFrontCtrl.PlayAni(EquipAnimState.ClericAttack, false);
        HandBackCtrl.PlayAni(EquipAnimState.ClericAttack, false);
        HandFrontCtrl.PlayAni(EquipAnimState.ClericAttack, false);
        SuitCtrl.PlayAni(EquipAnimState.ClericAttack, false);
    }
    public void PlayBow()
    {
        screenCtrl.canCtrl = false;
        ShoesCtrl.PlayAni(EquipAnimState.BowAttack, false);
        FaceCtrl.PlayAni(EquipAnimState.BowAttack, false);
        UpwearCtrl.PlayAni(EquipAnimState.BowAttack, false);
        DownwearCtrl.PlayAni(EquipAnimState.BowAttack, false);
        HairBackCtrl.PlayAni(EquipAnimState.BowAttack, false);
        HairFrontCtrl.PlayAni(EquipAnimState.BowAttack, false);
        HandBackCtrl.PlayAni(EquipAnimState.BowAttack, false);
        HandFrontCtrl.PlayAni(EquipAnimState.BowAttack, false);
        SuitCtrl.PlayAni(EquipAnimState.BowAttack, false);
    }
    public void PlayCrossbow()
    {
        screenCtrl.canCtrl = false;
        ShoesCtrl.PlayAni(EquipAnimState.CrossbowAttack, false);
        FaceCtrl.PlayAni(EquipAnimState.CrossbowAttack, false);
        UpwearCtrl.PlayAni(EquipAnimState.CrossbowAttack, false);
        DownwearCtrl.PlayAni(EquipAnimState.CrossbowAttack, false);
        HairBackCtrl.PlayAni(EquipAnimState.CrossbowAttack, false);
        HairFrontCtrl.PlayAni(EquipAnimState.CrossbowAttack, false);
        HandBackCtrl.PlayAni(EquipAnimState.CrossbowAttack, false);
        HandFrontCtrl.PlayAni(EquipAnimState.CrossbowAttack, false);
        SuitCtrl.PlayAni(EquipAnimState.CrossbowAttack, false);
    }
    public void PlayDown1()
    {
        screenCtrl.canCtrl = false;
        ShoesCtrl.PlayAni(EquipAnimState.DownAttack1, false);
        FaceCtrl.PlayAni(EquipAnimState.DownAttack1, false);
        UpwearCtrl.PlayAni(EquipAnimState.DownAttack1, false);
        DownwearCtrl.PlayAni(EquipAnimState.DownAttack1, false);
        HairBackCtrl.PlayAni(EquipAnimState.DownAttack1, false);
        HairFrontCtrl.PlayAni(EquipAnimState.DownAttack1, false);
        HandBackCtrl.PlayAni(EquipAnimState.DownAttack1, false);
        HandFrontCtrl.PlayAni(EquipAnimState.DownAttack1, false);
        SuitCtrl.PlayAni(EquipAnimState.DownAttack1, false);
    }
    public void PlayDown2()
    {
        screenCtrl.canCtrl = false;
        ShoesCtrl.PlayAni(EquipAnimState.DownAttack2, false);
        FaceCtrl.PlayAni(EquipAnimState.DownAttack2, false);
        UpwearCtrl.PlayAni(EquipAnimState.DownAttack2, false);
        DownwearCtrl.PlayAni(EquipAnimState.DownAttack2, false);
        HairBackCtrl.PlayAni(EquipAnimState.DownAttack2, false);
        HairFrontCtrl.PlayAni(EquipAnimState.DownAttack2, false);
        HandBackCtrl.PlayAni(EquipAnimState.DownAttack2, false);
        HandFrontCtrl.PlayAni(EquipAnimState.DownAttack2, false);
        SuitCtrl.PlayAni(EquipAnimState.DownAttack2, false);
    }
    public void PlayHorizon1()
    {
        screenCtrl.canCtrl = false;
        ShoesCtrl.PlayAni(EquipAnimState.HorizontalAttack1, false);
        FaceCtrl.PlayAni(EquipAnimState.HorizontalAttack1, false);
        UpwearCtrl.PlayAni(EquipAnimState.HorizontalAttack1, false);
        DownwearCtrl.PlayAni(EquipAnimState.HorizontalAttack1, false);
        HairBackCtrl.PlayAni(EquipAnimState.HorizontalAttack1, false);
        HairFrontCtrl.PlayAni(EquipAnimState.HorizontalAttack1, false);
        HandBackCtrl.PlayAni(EquipAnimState.HorizontalAttack1, false);
        HandFrontCtrl.PlayAni(EquipAnimState.HorizontalAttack1, false);
        SuitCtrl.PlayAni(EquipAnimState.HorizontalAttack1, false);
    }
    public void PlayHorizon2()
    {
        screenCtrl.canCtrl = false;
        ShoesCtrl.PlayAni(EquipAnimState.HorizontalAttack2, false);
        FaceCtrl.PlayAni(EquipAnimState.HorizontalAttack2, false);
        UpwearCtrl.PlayAni(EquipAnimState.HorizontalAttack2, false);
        DownwearCtrl.PlayAni(EquipAnimState.HorizontalAttack2, false);
        HairBackCtrl.PlayAni(EquipAnimState.HorizontalAttack2, false);
        HairFrontCtrl.PlayAni(EquipAnimState.HorizontalAttack2, false);
        HandBackCtrl.PlayAni(EquipAnimState.HorizontalAttack2, false);
        HandFrontCtrl.PlayAni(EquipAnimState.HorizontalAttack2, false);
        SuitCtrl.PlayAni(EquipAnimState.HorizontalAttack2, false);
    }
    #endregion

    #region PlayerMove

    public int Number = 10;
    public int Frequency = 15;
    public int Serialization = 0;
    public void SendMove(int MoveState)
    {
        new MoveSender(GameRoot.Instance.ActivePlayer.MapID, new float[] { transform.localPosition.x, transform.localPosition.y, transform.localPosition.z });
        Serialization++;
    }

    #endregion

    #region ChatBox
    public int ChatBoxNum = 0;
    IEnumerator CloseChatBox()
    {
        ChatBoxNum += 1;
        yield return new WaitForSeconds(5);
        if (ChatBoxNum == 1)
        {
            ChatBoxTxt.text = "";
            ChatBox.SetActive(false);
        }
        ChatBoxNum -= 1;

    }
    public void ShowChatBox(string txt)
    {
        ChatBoxTxt.text = txt;
        ChatBox.SetActive(true);
        try
        {
            StartCoroutine(CloseChatBox());
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }

    }

    #endregion

    #region Number
    public GameObject DamageContainer;
    public void GenerateDamageNum(int damage, int mode)
    {
        //print("產生數字int:" + damage);
        DamageContainer.transform.localScale = new Vector3(Mathf.Abs(DamageContainer.transform.localScale.x), DamageContainer.transform.localScale.y, DamageContainer.transform.localScale.z);
        GameObject obj = Instantiate(Resources.Load("Prefabs/Damage") as GameObject);
        obj.transform.SetParent(DamageContainer.transform);
        obj.transform.localScale = new Vector3(0.01f, 0.01f, 1);
        obj.transform.localPosition = Vector3.zero;
        obj.GetComponent<DamageController>().SetNumber(damage, mode);
    }
    public void GenerateDamageNum(long damage, int mode)
    {
        //print("產生數字long:" + damage);
        DamageContainer.transform.localScale = new Vector3(Mathf.Abs(DamageContainer.transform.localScale.x), DamageContainer.transform.localScale.y, DamageContainer.transform.localScale.z);
        GameObject obj = Instantiate(Resources.Load("Prefabs/Damage") as GameObject);
        obj.transform.SetParent(DamageContainer.transform);
        obj.transform.localScale = new Vector3(0.01f, 0.01f, 1);
        obj.transform.localPosition = Vector3.zero;
        obj.GetComponent<DamageController>().SetNumber(damage, mode);
    }
    public void SetHpBar(int realHp)
    {
        HpBar.fillAmount = (float)(((double)GameRoot.Instance.ActivePlayer.HP) / realHp);
    }
    #endregion

    #region GetHurt or Death
    public bool IsHurt = false;
    public bool IsDeath = false;
    public void GetHurt(int damage, HurtType hurtType, int MonsterID)
    {
        bool FaceDir = false;
        if (transform.localScale.x > 0)
        {
            FaceDir = true;
        }
        new PlayerGetHurtSender(damage, hurtType, MonsterID, FaceDir);
    }
    public void ProcessGetHurt(int damage, HurtType hurtType, int MonsterID)
    {
        MainCitySys.Instance.InfoWnd.UpdateHp(GameRoot.Instance.ActivePlayer.HP - damage);
        if (!IsHurt)
        {
            //播受傷動畫
            Blackboard blackboard = GetComponent<Blackboard>();
            NodeCanvas.BehaviourTrees.BehaviourTreeOwner tree = GetComponent<NodeCanvas.BehaviourTrees.BehaviourTreeOwner>();
            blackboard.SetVariableValue("CanMove", false);
            blackboard.SetVariableValue("IsHurt", true);
            blackboard.SetVariableValue("IsIdle", false);
            blackboard.SetVariableValue("IsDeath", false);
            tree.RestartBehaviour();
        }
        else
        {
            return;
        }
    }
    public void Death()
    {
        //播死亡動畫
        Blackboard blackboard = GetComponent<Blackboard>();
        NodeCanvas.BehaviourTrees.BehaviourTreeOwner tree = GetComponent<NodeCanvas.BehaviourTrees.BehaviourTreeOwner>();
        blackboard.SetVariableValue("CanMove", false);
        blackboard.SetVariableValue("IsHurt", false);
        blackboard.SetVariableValue("IsIdle", false);
        blackboard.SetVariableValue("IsDeath", true);
        tree.RestartBehaviour();
        MainCitySys.Instance.InfoWnd.SetDeathHP();
    }
    public void Enable()
    {
        Blackboard blackboard = GetComponent<Blackboard>();
        NodeCanvas.BehaviourTrees.BehaviourTreeOwner tree = GetComponent<NodeCanvas.BehaviourTrees.BehaviourTreeOwner>();
        blackboard.SetVariableValue("CanMove", true);
        tree.RestartBehaviour();

    }
    public void Disable()
    {
        Blackboard blackboard = GetComponent<Blackboard>();
        NodeCanvas.BehaviourTrees.BehaviourTreeOwner tree = GetComponent<NodeCanvas.BehaviourTrees.BehaviourTreeOwner>();
        blackboard.SetVariableValue("CanMove", false);
        tree.RestartBehaviour();
    }
    #endregion
    public void Print(string s)
    {
        print(s);
    }

    #region Title
    public void SetTitle(string s)
    {
        if (s != "")
        {
            Title.text = "[" + s + "]";
            Title.gameObject.SetActive(true);
        }
        else
        {
            Title.gameObject.SetActive(false);
        }
    }
    #endregion
}

#region ActionTask
[Category("Character/")]
public class ChangeSpeed : ActionTask<Transform>
{
    protected override void OnExecute()
    {
        agent.GetComponent<PlayerCtrl>().ShoesCtrl.IsAniPause = false;
        agent.GetComponent<PlayerCtrl>().FaceCtrl.IsAniPause = false;
        agent.GetComponent<PlayerCtrl>().UpwearCtrl.IsAniPause = false;
        agent.GetComponent<PlayerCtrl>().DownwearCtrl.IsAniPause = false;
        agent.GetComponent<PlayerCtrl>().HairFrontCtrl.IsAniPause = false;
        agent.GetComponent<PlayerCtrl>().HairBackCtrl.IsAniPause = false;
        agent.GetComponent<PlayerCtrl>().HandBackCtrl.IsAniPause = false;
        agent.GetComponent<PlayerCtrl>().HandFrontCtrl.IsAniPause = false;
        agent.GetComponent<PlayerCtrl>().SuitCtrl.IsAniPause = false;
        agent.GetComponent<PlayerCtrl>().PlayIdle();
        base.OnExecute();
        EndAction();
    }
}

[Category("Character/")]
public class StartAni : ActionTask<Transform>
{
    protected override void OnExecute()
    {
        agent.GetComponent<PlayerCtrl>().ShoesCtrl.IsAniPause = false;
        agent.GetComponent<PlayerCtrl>().FaceCtrl.IsAniPause = false;
        agent.GetComponent<PlayerCtrl>().UpwearCtrl.IsAniPause = false;
        agent.GetComponent<PlayerCtrl>().DownwearCtrl.IsAniPause = false;
        agent.GetComponent<PlayerCtrl>().HairFrontCtrl.IsAniPause = false;
        agent.GetComponent<PlayerCtrl>().HairBackCtrl.IsAniPause = false;
        agent.GetComponent<PlayerCtrl>().HandBackCtrl.IsAniPause = false;
        agent.GetComponent<PlayerCtrl>().HandFrontCtrl.IsAniPause = false;
        agent.GetComponent<PlayerCtrl>().SuitCtrl.IsAniPause = false;
        agent.GetComponent<PlayerCtrl>().PlayIdle();
        base.OnExecute();
        EndAction();
    }
}

[Category("Character/")]
public class PauseAni : ActionTask<Transform>
{
    protected override void OnExecute()
    {
        agent.GetComponent<PlayerCtrl>().ShoesCtrl.IsAniPause = true;
        agent.GetComponent<PlayerCtrl>().FaceCtrl.IsAniPause = true;
        agent.GetComponent<PlayerCtrl>().UpwearCtrl.IsAniPause = true;
        agent.GetComponent<PlayerCtrl>().DownwearCtrl.IsAniPause = true;
        agent.GetComponent<PlayerCtrl>().HairFrontCtrl.IsAniPause = true;
        agent.GetComponent<PlayerCtrl>().HairBackCtrl.IsAniPause = true;
        agent.GetComponent<PlayerCtrl>().HandBackCtrl.IsAniPause = true;
        agent.GetComponent<PlayerCtrl>().HandFrontCtrl.IsAniPause = true;
        agent.GetComponent<PlayerCtrl>().SuitCtrl.IsAniPause = true;
        base.OnExecute();
        EndAction();
    }
}

[Category("Character/")]
public class SetFaceDirection : ActionTask<Transform>
{
    [BlackboardOnly]
    public BBParameter<Vector3> Direction;

    protected override void OnExecute()
    {
        float MoveX = Direction.value.x;
        PlayerCtrl ctrl = agent.transform.GetComponent<PlayerCtrl>();
        //控制人物方向和動畫
        if (MoveX > 0)
        {
            agent.transform.localScale = new Vector3(1, 1, 1);
            ctrl.DamageContainer.transform.localScale = new Vector3(Mathf.Abs(ctrl.DamageContainer.transform.localScale.x), ctrl.DamageContainer.transform.localScale.y, ctrl.DamageContainer.transform.localScale.z);
            ctrl.NameBox.transform.localScale = new Vector3(agent.transform.localScale.x, 1, 1);
            ctrl.ChatBox.transform.localScale = new Vector3(agent.transform.localScale.x, 1, 1);
            ctrl.Shadow.localScale = new Vector3(100 * ctrl.transform.localScale.x, 100, 1);
            ctrl.HpBar.transform.localScale = new Vector3(-agent.transform.localScale.x, 1, 1);
        }
        else if (MoveX < 0)
        {
            agent.transform.localScale = new Vector3(-1, 1, 1);
            ctrl.DamageContainer.transform.localScale = new Vector3(-Mathf.Abs(ctrl.DamageContainer.transform.localScale.x), ctrl.DamageContainer.transform.localScale.y, ctrl.DamageContainer.transform.localScale.z);
            ctrl.NameBox.transform.localScale = new Vector3(agent.transform.localScale.x, 1, 1);
            ctrl.ChatBox.transform.localScale = new Vector3(agent.transform.localScale.x, 1, 1);
            ctrl.Shadow.localScale = new Vector3(100 * ctrl.transform.localScale.x, 100, 1);
            ctrl.HpBar.transform.localScale = new Vector3(-agent.transform.localScale.x, 1, 1);
        }
        base.OnExecute();
        EndAction();
    }
}

[Category("Character/")]
public class DetectEnemy : ActionTask<Transform>
{
    [BlackboardOnly]
    float DetectionRange = Constants.GetAttackDistanceByJobID(GameRoot.Instance.ActivePlayer.Job);
    float cosx = 0.5f;
    protected override void OnExecute()
    {
        Dictionary<int, MonsterAI> Monsters = BattleSys.Instance.Monsters;
        GameObject target = blackboard.GetVariable<GameObject>("EnemyTarget").value;
        if (target != null) //有目標
        {
            if (Vector3.Distance(target.transform.position, agent.transform.position) > DetectionRange) //如果超過偵測範圍
            {
                blackboard.SetVariableValue("EnemyTarget", null);
                target.GetComponent<MonsterAI>().HideProfile();//隱藏怪物HP
                BattleSys.Instance.ClearTarget(); //目標設空
            }
        }
        //else
        //{
        GameObject NewTarget = null;
        float MinDistance = 99999f;
        Vector3 right = new Vector3(1, 0, 0);
        Vector3 left = new Vector3(-1, 0, 0);
        if (Monsters.Count != 0)
        {
            foreach (var mon in Monsters.Values) //遍歷所有怪物
            {
                if (mon != null)
                {
                    Vector3 _distance = mon.transform.position - agent.position;
                    float distance = _distance.magnitude; //算距離
                    if (distance <= DetectionRange) //如果小於偵測範圍
                    {
                        if (_distance.x >= 0)
                        {
                            if (agent.transform.localScale.x > 0 && Math.Abs(Vector3.Angle(right, _distance)) < 60 && distance <= MinDistance) //如果方向正確 且FOV小於60度 且為最小距離
                            {
                                MinDistance = distance; //更新最小距離
                                NewTarget = mon.gameObject; //暫時用此怪物
                            }
                        }
                        else
                        {
                            if (agent.transform.localScale.x < 0 && Math.Abs(Vector3.Angle(left, _distance)) < 60 && distance <= MinDistance) //如果方向正確 且FOV小於60度 且為最小距離
                            {
                                MinDistance = distance; //更新最小距離
                                NewTarget = mon.gameObject; //暫時用此怪物
                            }
                        }

                    }
                    mon.HideProfile(); //隱藏怪物HPs
                }

            }
            if (NewTarget != null)
            {
                NewTarget.GetComponent<MonsterAI>().ShowProfile();
                blackboard.SetVariableValue("EnemyTarget", NewTarget.gameObject);
                BattleSys.Instance.LockTarget(NewTarget.GetComponent<MonsterAI>());
            }

        }
        //}
        EndAction();
    }
}
[Category("Character/")]
public class PlayCharacterIdleAni : ActionTask<Transform>
{
    protected override void OnExecute()
    {
        try
        {
            agent.GetComponent<PlayerCtrl>().PlayIdle();
            base.OnExecute();
            EndAction();
        }
        catch (Exception)
        {
            agent.GetComponent<PlayerCtrl>().PlayIdle();
            EndAction();
        }
    }
}

[Category("Character/")]
public class PlayCharacterWalkAni : ActionTask<Transform>
{
    protected override void OnExecute()
    {
        agent.GetComponent<PlayerCtrl>().PlayWalk();
        base.OnExecute();
        EndAction();
    }
}

[Category("Character/")]
public class PlayCharacterRunAni : ActionTask<Transform>
{
    [BlackboardOnly]
    public BBParameter<float> DelayTime;
    public bool repeat;
    IEnumerator timer()
    {
        yield return new WaitForSeconds(DelayTime.value);
        EndAction();
    }
    protected override void OnExecute()
    {
        agent.GetComponent<PlayerCtrl>().PlayRun();
        base.OnExecute();
        StartCoroutine(timer());
    }
}

[Category("Character/")]
public class PlayCommonAttackAni : ActionTask<Transform>
{

    [BlackboardOnly]
    public BBParameter<float> DelayTime;
    public bool repeat;
    IEnumerator timer()
    {
        yield return new WaitForSeconds(DelayTime.value);
        EndAction();
    }
    protected override void OnExecute()
    {
        switch (GameRoot.Instance.ActivePlayer.Job)
        {
            case 2:
                agent.GetComponent<PlayerCtrl>().PlayBow();
                break;
        }
        base.OnExecute();
        StartCoroutine(timer());
    }
}

[Category("Character/")]
public class AttackTarget : ActionTask<Transform>
{
    protected override void OnExecute()
    {
        MonsterAI monAi = BattleSys.Instance.CurrentTarget;
        if (monAi != null && !monAi.IsReadyDeath)
        {
            bool CanSeeEnemy = false;
            Vector3 right = new Vector3(1, 0, 0);
            Vector3 left = new Vector3(-1, 0, 0);
            Vector3 d = monAi.transform.position - agent.transform.position;
            float scalex = agent.transform.localScale.x;
            if (scalex >= 0 && d.x >= 0)
            {
                if (Math.Abs(Vector3.Angle(d, right)) < 60)
                {
                    CanSeeEnemy = true;
                }
            }
            else if (scalex <= 0 && d.x < 0)
            {
                if (Math.Abs(Vector3.Angle(d, left)) < 60)
                {
                    CanSeeEnemy = true;
                }
            }
            if (CanSeeEnemy)
                BattleSys.Instance.CommonAttack(monAi.MapMonsterID);
            base.OnExecute();

        }
        EndAction();
    }
}

[Category("Character/")]
public class PlayAttackSound : ActionTask<Transform>
{
    protected override void OnExecute()
    {
        PlayerCtrl ai = agent.transform.GetComponent<PlayerCtrl>();
        AudioClip audio = ResSvc.Instance.LoadAudio("Sound/Weapon/weapon_se_weapon_bow", true);
        agent.GetComponent<AudioSource>().clip = audio;
        agent.GetComponent<AudioSource>().Play();
        base.OnExecute();
        EndAction();
    }
}

[Category("Character/")]
public class PlayHitSound : ActionTask<Transform>
{
    protected override void OnExecute()
    {

        base.OnExecute();
        EndAction();
    }
}

[Category("Character/")]
public class PlayHurtAni : ActionTask<Transform>
{
    public float DelayTime = 0.55f;
    IEnumerator timer()
    {
        yield return new WaitForSeconds(DelayTime);
        try
        {
            PlayerCtrl ai = agent.GetComponent<PlayerCtrl>();
            ai.IsHurt = false;
            EndAction();
        }
        catch (Exception)
        {
        }

    }
    protected override void OnExecute()
    {
        PlayerCtrl ai = agent.GetComponent<PlayerCtrl>();
        if (!ai.IsHurt)
        {
            ai.IsHurt = true;
            ai.PlayHurt();
        }
        base.OnExecute();
        StartCoroutine(timer());
    }
}

[Category("Character/")]
public class PlayDeathAni : ActionTask<Transform>
{
    public float DelayTime = 2f;
    IEnumerator timer()
    {
        yield return new WaitForSeconds(DelayTime);
        PlayerCtrl ai = agent.GetComponent<PlayerCtrl>();
        EndAction();
    }
    protected override void OnExecute()
    {
        PlayerCtrl ai = agent.GetComponent<PlayerCtrl>();
        if (!ai.IsDeath)
        {
            ai.IsDeath = true;
            ai.PlayDeath();
        }
        base.OnExecute();
        StartCoroutine(timer());
    }
}
#endregion
