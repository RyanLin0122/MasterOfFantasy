using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PEProtocal;
using System;
using PolyNav;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

public class OtherPeopleCtrl : Controllable
{

    public new Transform transform;
    private new Rigidbody2D rigidbody;
    public GameObject NameBox;
    public Transform Shadow;
    public bool IsRun = false;
    public Text ChatBoxTxt;
    public GameObject ChatBox;
    public Image HpBar;
    public int IdleCounter = 0;

    void Awake()
    {
        rigidbody = this.GetComponent<Rigidbody2D>();
        rigidbody.freezeRotation = true;
        Actions = new Queue<OtherPlayerTask>();
    }

    #region Player animation
    public EquipmentAnimator ShoesCtrl;
    public EquipmentAnimator FaceCtrl;
    public EquipmentAnimator UpwearCtrl;
    public EquipmentAnimator DownwearCtrl;
    public EquipmentAnimator HairFrontCtrl;
    public EquipmentAnimator HairBackCtrl;
    public EquipmentAnimator HandBackCtrl;
    public EquipmentAnimator HandFrontCtrl;
    public EquipmentAnimator SuitCtrl;
    public void SetAllEquipment(TrimedPlayer playerData)
    {
        SetEquipment(playerData, EquipmentType.Shoes);
        SetEquipment(playerData, EquipmentType.Chest);
        SetEquipment(playerData, EquipmentType.Pant);
        SetEquipment(playerData, EquipmentType.Gloves);
        SetEquipment(playerData, EquipmentType.HairStyle);
        SetFace(playerData);
    }
    public void SetEquipment(TrimedPlayer pd, EquipmentType Type)
    {

        switch (Type)
        {
            case EquipmentType.Shoes:
                ShoesCtrl.gameObject.SetActive(true);
                if (pd.playerEquipments.F_Chest.ItemID <= 7000)
                {
                    if (pd.playerEquipments.F_Shoes != null)
                    {
                        //顯示鞋子點裝
                        ChangeEquipment(pd.playerEquipments.F_Shoes.ItemID, Type);
                        return;
                    }
                    else
                    {
                        if (pd.playerEquipments.B_Shoes != null)
                        {
                            //顯示鞋子
                            ChangeEquipment(pd.playerEquipments.B_Shoes.ItemID, Type);
                            return;
                        }
                        else
                        {
                            ChangeDefaultEquipment(pd.Gender, EquipmentType.Shoes);
                            //顯示無鞋子
                        }
                    }
                }
                else
                {
                    //有穿套裝，關閉鞋子
                    ShoesCtrl.gameObject.SetActive(false);
                }
                break;
            case EquipmentType.Chest:
                UpwearCtrl.gameObject.SetActive(true);
                SuitCtrl.gameObject.SetActive(false);
                if (pd.playerEquipments.F_Chest.ItemID <= 7000)
                {
                    if (pd.playerEquipments.F_Chest != null)
                    {
                        //顯示衣服點裝
                        ChangeEquipment(pd.playerEquipments.F_Chest.ItemID, Type);
                        return;
                    }
                    else
                    {
                        if (pd.playerEquipments.B_Chest != null)
                        {
                            //顯示衣服
                            ChangeEquipment(pd.playerEquipments.B_Chest.ItemID, Type);
                            return;
                        }
                        else
                        {
                            ChangeDefaultEquipment(pd.Gender, EquipmentType.Chest);
                            //顯示無衣服
                        }
                    }
                }
                else
                {
                    //開套裝
                    ChangeEquipment(pd.playerEquipments.F_Chest.ItemID, Type);
                }
                break;
            case EquipmentType.Pant:
                DownwearCtrl.gameObject.SetActive(true);
                if (pd.playerEquipments.F_Chest.ItemID <= 7000)
                {
                    if (pd.playerEquipments.F_Pants != null)
                    {
                        //顯示褲子點裝
                        ChangeEquipment(pd.playerEquipments.F_Pants.ItemID, Type);
                        return;
                    }
                    else
                    {
                        if (pd.playerEquipments.B_Pants != null)
                        {
                            //顯示褲子
                            ChangeEquipment(pd.playerEquipments.B_Pants.ItemID, Type);
                            return;
                        }
                        else
                        {
                            ChangeDefaultEquipment(pd.Gender, EquipmentType.Pant);
                            //顯示無褲子
                        }
                    }
                }
                else
                {
                    //有穿套裝，關閉褲子
                    DownwearCtrl.gameObject.SetActive(false);
                }
                break;
            case EquipmentType.Gloves:
                HandBackCtrl.gameObject.SetActive(true);
                HandFrontCtrl.gameObject.SetActive(true);
                if (pd.playerEquipments.F_Chest.ItemID <= 7000)
                {
                    if (pd.playerEquipments.F_Glove != null)
                    {
                        //顯示手套點裝
                        ChangeEquipment(pd.playerEquipments.F_Glove.ItemID, Type);
                        return;
                    }
                    else
                    {
                        if (pd.playerEquipments.B_Glove != null)
                        {
                            //顯示手套
                            ChangeEquipment(pd.playerEquipments.B_Glove.ItemID, Type);
                            return;
                        }
                        else
                        {
                            ChangeDefaultEquipment(pd.Gender, EquipmentType.Gloves);
                            //顯示無手套
                        }
                    }
                }
                else
                {
                    //有穿套裝，關閉手套
                    HandBackCtrl.gameObject.SetActive(false);
                    HandFrontCtrl.gameObject.SetActive(false);
                }
                break;
            case EquipmentType.HairStyle:
                HairBackCtrl.gameObject.SetActive(true);
                HairFrontCtrl.gameObject.SetActive(true);

                if (pd.playerEquipments.F_HairStyle != null)
                {

                    ChangeEquipment(pd.playerEquipments.F_HairStyle.ItemID, Type);
                    return;
                }
                else
                {
                    ChangeDefaultEquipment(pd.Gender, EquipmentType.HairStyle);
                    //顯示默認髮型
                }
                break;
        }
    }
    public void SetFace(TrimedPlayer pd)
    {
        if (pd.Gender == 0)
        {
            if (pd.playerEquipments.F_FaceType == null)
            {
                ChangeDefaultEquipment(0, EquipmentType.FaceType);
            }
            else
            {
                ChangeEquipment(pd.playerEquipments.F_FaceType.ItemID, EquipmentType.FaceType);
            }
        }
        else
        {
            if (pd.playerEquipments.F_FaceType == null)
            {
                ChangeDefaultEquipment(1, EquipmentType.FaceType);
            }
            else
            {
                ChangeEquipment(pd.playerEquipments.F_FaceType.ItemID, EquipmentType.FaceType);
            }
        }

    }
    public void SetWeaponAnimation(int weaponID, ItemQuality quality)
    {

    }
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
                    PECommon.Log("Chest ID: " + id);
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
                    PECommon.Log("Chest ID: " + id);
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
        PlayIdle();
    }
    public void PlayDeath()
    {
        IsDeath = true;
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
    public Vector3 UpdatePos;
    public Vector3 NextPos;
    public void SetUpdatePos(float[] pos)
    {
        UpdatePos = new Vector3(pos[0], pos[1], transform.localPosition.z);
    }
    public Queue<OtherPlayerTask> Actions;

    #endregion

    #region Number
    public GameObject DamageContainer;
    public void GenerateDamageNum(int damage, int mode)
    {
        DamageContainer.transform.localScale = new Vector3(Mathf.Abs(DamageContainer.transform.localScale.x), DamageContainer.transform.localScale.y, DamageContainer.transform.localScale.z);
        GameObject obj = Instantiate(Resources.Load("Prefabs/Damage") as GameObject);
        obj.transform.SetParent(DamageContainer.transform);
        obj.transform.localScale = new Vector3(0.01f, 0.01f, 1);
        obj.transform.localPosition = Vector3.zero;
        obj.GetComponent<DamageController>().SetNumber(damage, mode);
    }
    public void GenerateDamageNum(long damage, int mode)
    {
        DamageContainer.transform.localScale = new Vector3(Mathf.Abs(DamageContainer.transform.localScale.x), DamageContainer.transform.localScale.y, DamageContainer.transform.localScale.z);
        GameObject obj = Instantiate(Resources.Load("Prefabs/Damage") as GameObject);
        obj.transform.SetParent(DamageContainer.transform);
        obj.transform.localScale = new Vector3(0.01f, 0.01f, 1);
        obj.transform.localPosition = Vector3.zero;
        obj.GetComponent<DamageController>().SetNumber(damage, mode);
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
        StartCoroutine(CloseChatBox());

    }
    #endregion
    public void SetFaceDir(bool Dir)
    {
        if (Dir) //往右
        {
            transform.localScale = new Vector3(1, 1, 1);
            DamageContainer.transform.localScale = new Vector3(Mathf.Abs(DamageContainer.transform.localScale.x), DamageContainer.transform.localScale.y, DamageContainer.transform.localScale.z);
            NameBox.transform.localScale = new Vector3(transform.localScale.x, 1, 1);
            ChatBox.transform.localScale = new Vector3(transform.localScale.x, 1, 1);
            Shadow.localScale = new Vector3(100 * transform.localScale.x, 100, 1);
            HpBar.transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
        }
        else //往左
        {
            transform.localScale = new Vector3(-1, 1, 1);
            DamageContainer.transform.localScale = new Vector3(-Mathf.Abs(DamageContainer.transform.localScale.x), DamageContainer.transform.localScale.y, DamageContainer.transform.localScale.z);
            NameBox.transform.localScale = new Vector3(transform.localScale.x, 1, 1);
            ChatBox.transform.localScale = new Vector3(transform.localScale.x, 1, 1);
            Shadow.localScale = new Vector3(100 * transform.localScale.x, 100, 1);
            HpBar.transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
        }
    }
    #region GetHurt or Death
    public bool IsHurt = false;
    public bool IsDeath = false;
    public void ProcessGetHurt(int damage, HurtType hurtType, int MonsterID)
    {
        int UpdateHP = GameRoot.Instance.ActivePlayer.HP - damage;
        Player pd = GameRoot.Instance.ActivePlayer;
        //人物頭上數字
        if (pd.HP > UpdateHP)
        {
            //扣血
            print("別人扣血");
            GenerateDamageNum(pd.HP - UpdateHP, 2);
        }
        else if (pd.HP < UpdateHP)
        {
            //補血
            print("別人補血");
            GenerateDamageNum(pd.HP - UpdateHP, 2);
        }

        //GameRoot.Instance.UpdatePlayerHp(RealMaxHp);

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
        //MainCitySys.Instance.InfoWnd.SetDeathHP();
    }
    public void SetHpBar(int realHp)
    {
        HpBar.fillAmount = (float)(((double)GameRoot.Instance.ActivePlayer.HP) / realHp);
    }
    #endregion
    public void DeleteThisChr()
    {
        Destroy(this.gameObject);
    }

    public IEnumerator _Wait()
    {
        yield return new WaitForSeconds(0.5f);
        NodeCanvas.BehaviourTrees.BehaviourTreeOwner tree = GetComponent<NodeCanvas.BehaviourTrees.BehaviourTreeOwner>();
        tree.enabled = true;
    }
    public void WaitAMoment()
    {
        StartCoroutine("_Wait");
    }
}
public class OtherPlayerTask
{
    public OtherPeopleCtrl Ctrl;
    public int ActionID;
    public bool FaceDir;

    public OtherPlayerTask(OtherPeopleCtrl ctrl, int actionID, bool faceDir)
    {
        Ctrl = ctrl;
        ActionID = actionID;
        FaceDir = faceDir;
    }
    public void OnExecute()
    {
        PECommon.Log("播動畫:" + ActionID);
        switch (ActionID)
        {
            case 1: //受傷
                Ctrl.PlayHurt();
                break;
            case 2: //死亡
                Ctrl.PlayDeath();
                break;
            case 3: //弓攻擊
                Ctrl.PlayBow();
                break;
            default:
                break;
        }
    }
}


[Category("OtherCharacter/")]
public class OtherPlayerAction : ActionTask<Transform>
{
    public IEnumerator Timer(int AnimID)
    {
        yield return new WaitForSeconds(Constants.GetAnimTimeByID(AnimID));

        EndAction();
    }

    protected override void OnExecute()
    {
        try
        {
            Queue<OtherPlayerTask> acitions = agent.GetComponent<OtherPeopleCtrl>().Actions;
            if (acitions.Count > 0)
            {

                OtherPlayerTask task = acitions.Dequeue();
                task.OnExecute();
                StartCoroutine(Timer(task.ActionID));
            }
            else
            {
                EndAction();
            }
            base.OnExecute();

        }
        catch (Exception)
        {
            EndAction();
        }
    }
}



[Category("OtherCharacter/")]
public class ShiftNextPos : ActionTask<Transform>
{
    protected override void OnExecute()
    {

        OtherPeopleCtrl ctrl = agent.transform.GetComponent<OtherPeopleCtrl>();
        ctrl.NextPos = ctrl.UpdatePos;
        EndAction();
    }
}

[Category("OtherCharacter/")]
public class MoveOtherPlayer : ActionTask<Transform>
{
    [RequiredField]
    public BBParameter<float> speed = 200;
    public BBParameter<float> stopDistance = 30f;
    public bool waitActionFinish;
    Vector3 targetPos = Vector3.zero;
    protected override string OnInit()
    {
        OtherPeopleCtrl ctrl = agent.transform.GetComponent<OtherPeopleCtrl>();
        targetPos = agent.transform.GetComponent<OtherPeopleCtrl>().NextPos;
        return base.OnInit();
    }
    protected override void OnExecute()
    {
        OtherPeopleCtrl ctrl = agent.transform.GetComponent<OtherPeopleCtrl>();
        targetPos = agent.transform.GetComponent<OtherPeopleCtrl>().NextPos;
        if (targetPos.x - agent.transform.localPosition.x > 0) //往右走
        {
            agent.transform.localScale = new Vector3(1, 1, 1);
            ctrl.DamageContainer.transform.localScale = new Vector3(Mathf.Abs(ctrl.DamageContainer.transform.localScale.x), ctrl.DamageContainer.transform.localScale.y, ctrl.DamageContainer.transform.localScale.z);
            ctrl.NameBox.transform.localScale = new Vector3(agent.transform.localScale.x, 1, 1);
            ctrl.ChatBox.transform.localScale = new Vector3(agent.transform.localScale.x, 1, 1);
            ctrl.Shadow.localScale = new Vector3(100 * ctrl.transform.localScale.x, 100, 1);
            ctrl.HpBar.transform.localScale = new Vector3(-agent.transform.localScale.x, 1, 1);
        }
        else if (targetPos.x - agent.transform.localPosition.x == 0) //不變
        {

        }
        else //往左走
        {
            agent.transform.localScale = new Vector3(-1, 1, 1);
            ctrl.DamageContainer.transform.localScale = new Vector3(-Mathf.Abs(ctrl.DamageContainer.transform.localScale.x), ctrl.DamageContainer.transform.localScale.y, ctrl.DamageContainer.transform.localScale.z);
            ctrl.NameBox.transform.localScale = new Vector3(agent.transform.localScale.x, 1, 1);
            ctrl.ChatBox.transform.localScale = new Vector3(agent.transform.localScale.x, 1, 1);
            ctrl.Shadow.localScale = new Vector3(100 * ctrl.transform.localScale.x, 100, 1);
            ctrl.HpBar.transform.localScale = new Vector3(-agent.transform.localScale.x, 1, 1);
        }
    }
    private PolyNavAgent Nav_agent;
    private PolyNavAgent Navagent
    {
        get { return Nav_agent != null ? Nav_agent : Nav_agent = agent.GetComponent<PolyNavAgent>(); }
    }

    protected override void OnUpdate()
    {
        OtherPeopleCtrl ctrl = agent.transform.GetComponent<OtherPeopleCtrl>();
        Blackboard blackboard = agent.transform.GetComponent<Blackboard>();
        targetPos = agent.transform.GetComponent<OtherPeopleCtrl>().NextPos;
        //Debug.Log("Next:" + ctrl.NextPos.x + ", " + ctrl.NextPos.y + ", " + ctrl.NextPos.z);
        //Debug.Log("Update:" + ctrl.UpdatePos.x + ", " + ctrl.UpdatePos.y + ", " + ctrl.UpdatePos.z);
        //Debug.Log("agent:" + agent.transform.localPosition.x + ", " + agent.transform.localPosition.y + ", " + agent.transform.localPosition.z);
        //Debug.Log("target:" + targetPos.x + ", " + targetPos.y + ", " + targetPos.z);
        //Debug.Log(Vector3.Distance(agent.transform.GetComponent<OtherPeopleCtrl>().NextPos, agent.transform.localPosition));
        if (targetPos.x - agent.transform.localPosition.x != 0) //往右走
        {
            if (ctrl.IdleCounter < 10)
            {
                if (!blackboard.GetVariable<bool>("IsMove").value)
                {
                    if (ctrl.IsRun)
                    {
                        PECommon.Log("Run" + ctrl.IdleCounter);
                        ctrl.PlayRun();
                    }
                    else
                    {
                        PECommon.Log("Walk" + ctrl.IdleCounter);
                        ctrl.PlayWalk();
                    }
                    blackboard.SetVariableValue("IsMove", true);
                }

            }
        }

        if (Vector3.Distance(agent.transform.GetComponent<OtherPeopleCtrl>().NextPos, agent.transform.localPosition) < 5)
        {
            if (ctrl.IdleCounter >= 10)
            {
                if (blackboard.GetVariable<bool>("IsMove").value)
                {
                    PECommon.Log("Idle" + ctrl.IdleCounter);
                    ctrl.PlayIdle();
                    blackboard.SetVariableValue("IsMove", false);
                }
            }
            ctrl.IdleCounter++;
            PECommon.Log("IdleCounter: " + ctrl.IdleCounter);
            EndAction();
        }
        else
        {
            PECommon.Log("IdleCounter: " + 0);
            ctrl.IdleCounter = 0;
        }

        //Navagent.stoppingDistance = stopDistance.value;
        //if ((agent.position - targetPos).magnitude <= stopDistance.value)
        //{

        //    return;
        //}
        Navagent.SetDestination(targetPos);

    }
}
