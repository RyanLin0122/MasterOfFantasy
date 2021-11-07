﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PEProtocal;
using System;
using PolyNav;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

public class EntityController : MonoBehaviour
{
    public string Name;
    public Rigidbody2D rb;
    public Transform Shadow;
    public GameObject NameBox;
    public GameObject ChatBox;
    public Image ChatBoxImg;
    public Text ChatBoxTxt;
    public Text TitleText;
    public Image HpBar;
    public Text GuildText;
    public Sprite[] DustSprites;
    public bool IsMoving = false;
    public bool IsRun = false;
    public Entity entity;

    public void Init()
    {
        rb = this.GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        DustSprites = Resources.LoadAll<Sprite>("Effect/Dust/Effect Walking Car Dust");
    }

    #region Title and Guild
    public void SetTitle(string s)
    {
        if (s != "")
        {
            GuildText.text = "[" + s + "]";
            GuildText.gameObject.SetActive(true);
        }
        else
        {
            GuildText.gameObject.SetActive(false);
        }
    }
    #endregion

    #region NameBox & ChatBox
    public int ChatBoxSeconds = 0;
    IEnumerator CloseChatBox()
    {
        ChatBoxSeconds += 1;
        yield return new WaitForSeconds(5);
        if (ChatBoxSeconds == 1)
        {
            ChatBoxTxt.text = "";
            ChatBox.SetActive(false);
        }
        ChatBoxSeconds -= 1;

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
    public void SetChatBoxSprite(Sprite sprite)
    {
        ChatBoxImg.sprite = sprite;

    }

    public void SetNameBox(bool IsHighLight = false)
    {
        Namebox namebox = GetComponent<Namebox>();
        PlayerEquipments equipments = GameRoot.Instance.ActivePlayer.playerEquipments;
        if (Name != null)
        {
            if (equipments != null)
            {
                if (equipments.F_NameBox == null)
                {
                    namebox.SetNameBox(Name, 0);
                }
                else
                {
                    namebox.SetNameBox(Name, equipments.F_NameBox.ItemID);
                }
            }
            else
            {
                namebox.SetNameBox(Name, 0);
            }
        }
        else
        {
            print("PlayerName is null");
        }
    }
    #endregion

    #region Number and Hp Bar
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

    #region Dust
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
            int spriteIndex = Tools.RDInt(0, DustSprites.Length - 1);
            ani.Initialize(DustSprites[spriteIndex]);
            int t = TimerSvc.Instance.AddTimeTask((a) => { InstantiateDust(); }, 0.13f, PETimeUnit.Second, 1);
        }
    }
    #endregion

}
public class PlayerController : EntityController
{
    public ScreenController screenCtrl;

    void Awake()
    {
        base.Init();
    }

    public void SetFaceDirection(bool FaceDir)
    {
        if (FaceDir)
        {
            transform.localScale = new Vector3(1, 1, 1);
            DamageContainer.transform.localScale = new Vector3(Mathf.Abs(DamageContainer.transform.localScale.x), DamageContainer.transform.localScale.y, DamageContainer.transform.localScale.z);
            NameBox.transform.localScale = new Vector3(transform.localScale.x, 1, 1);
            ChatBox.transform.localScale = new Vector3(transform.localScale.x, 1, 1);
            Shadow.localScale = new Vector3(100 * transform.localScale.x, 100, 1);
            HpBar.transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            TitleText.transform.localScale = new Vector3(transform.localScale.x, 1, 1);
            GuildText.transform.localScale = new Vector3(transform.localScale.x, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
            DamageContainer.transform.localScale = new Vector3(-Mathf.Abs(DamageContainer.transform.localScale.x), DamageContainer.transform.localScale.y, DamageContainer.transform.localScale.z);
            NameBox.transform.localScale = new Vector3(transform.localScale.x, 1, 1);
            ChatBox.transform.localScale = new Vector3(transform.localScale.x, 1, 1);
            Shadow.localScale = new Vector3(100 * transform.localScale.x, 100, 1);
            HpBar.transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            TitleText.transform.localScale = new Vector3(transform.localScale.x, 1, 1);
            GuildText.transform.localScale = new Vector3(transform.localScale.x, 1, 1);
        }
    }
    public void OnEntityEvent(EntityEvent entityEvent)
    {
        switch (entityEvent)
        {
            case EntityEvent.Idle:
                PlayIdle();
                IsMoving = false;
                break;
            case EntityEvent.Move:
                if (IsRun)
                {
                    PlayRun();
                    IsMoving = true;
                }
                else
                {
                    PlayWalk();
                    IsMoving = true;
                }
                break;
        }
    }
    public Vector2 Destination;
    private void LateUpdate()
    {
        if (this.rb != null)
        {
            if (Name != GameRoot.Instance.ActivePlayer.Name)
            {
                if (IsMoving)
                {
                    Vector2 NextPos = new Vector2(entity.entityData.Position.X, entity.entityData.Position.Y);
                    this.rb.velocity = 150 * ((NextPos - new Vector2(transform.localPosition.x, transform.localPosition.y)).normalized);
                    if (this.rb.velocity.x > 0)
                    {
                        SetFaceDirection(true);
                    }
                    else if (this.rb.velocity.x < 0)
                    {
                        SetFaceDirection(false);
                    }
                }
                else
                {
                    float Offset = 0.4f;
                    this.rb.velocity = Vector2.zero;
                    Destination = new Vector2(entity.entityData.Position.X, entity.entityData.Position.Y);
                    Vector2 CurrentPos = new Vector2(transform.localPosition.x, transform.localPosition.y);
                    if ((Destination - CurrentPos).magnitude < Offset)
                    {
                        transform.localPosition = Destination;
                    }
                    else
                    {
                        transform.localPosition = Vector2.Lerp(CurrentPos, Destination, 0.5f);
                    }
                }
                //print("Velocity: " + rb.velocity.x + " ," + rb.velocity.y);
            }
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
        ShoesCtrl.PlayAni(PlayerAniType.Idle, true);
        FaceCtrl.PlayAni(PlayerAniType.Idle, true);
        UpwearCtrl.PlayAni(PlayerAniType.Idle, true);
        DownwearCtrl.PlayAni(PlayerAniType.Idle, true);
        HairBackCtrl.PlayAni(PlayerAniType.Idle, true);
        HairFrontCtrl.PlayAni(PlayerAniType.Idle, true);
        HandBackCtrl.PlayAni(PlayerAniType.Idle, true);
        HandFrontCtrl.PlayAni(PlayerAniType.Idle, true);
        SuitCtrl.PlayAni(PlayerAniType.Idle, true);
    }
    public void PlayWalk()
    {
        screenCtrl.canCtrl = true;
        ShoesCtrl.PlayAni(PlayerAniType.Walk, true);
        FaceCtrl.PlayAni(PlayerAniType.Walk, true);
        UpwearCtrl.PlayAni(PlayerAniType.Walk, true);
        DownwearCtrl.PlayAni(PlayerAniType.Walk, true);
        HairBackCtrl.PlayAni(PlayerAniType.Walk, true);
        HairFrontCtrl.PlayAni(PlayerAniType.Walk, true);
        HandBackCtrl.PlayAni(PlayerAniType.Walk, true);
        HandFrontCtrl.PlayAni(PlayerAniType.Walk, true);
        SuitCtrl.PlayAni(PlayerAniType.Walk, true);
    }
    public void PlayRun()
    {
        screenCtrl.canCtrl = true;
        ShoesCtrl.PlayAni(PlayerAniType.Run, true);
        FaceCtrl.PlayAni(PlayerAniType.Run, true);
        UpwearCtrl.PlayAni(PlayerAniType.Run, true);
        DownwearCtrl.PlayAni(PlayerAniType.Run, true);
        HairBackCtrl.PlayAni(PlayerAniType.Run, true);
        HairFrontCtrl.PlayAni(PlayerAniType.Run, true);
        HandBackCtrl.PlayAni(PlayerAniType.Run, true);
        HandFrontCtrl.PlayAni(PlayerAniType.Run, true);
        SuitCtrl.PlayAni(PlayerAniType.Run, true);
    }
    public void PlayDagger()
    {
        screenCtrl.canCtrl = false;
        ShoesCtrl.PlayAni(PlayerAniType.DaggerAttack, false);
        FaceCtrl.PlayAni(PlayerAniType.DaggerAttack, false);
        UpwearCtrl.PlayAni(PlayerAniType.DaggerAttack, false);
        DownwearCtrl.PlayAni(PlayerAniType.DaggerAttack, false);
        HairBackCtrl.PlayAni(PlayerAniType.DaggerAttack, false);
        HairFrontCtrl.PlayAni(PlayerAniType.DaggerAttack, false);
        HandBackCtrl.PlayAni(PlayerAniType.DaggerAttack, false);
        HandFrontCtrl.PlayAni(PlayerAniType.DaggerAttack, false);
        SuitCtrl.PlayAni(PlayerAniType.DaggerAttack, false);
    }
    public void PlaySlash()
    {
        screenCtrl.canCtrl = false;
        ShoesCtrl.PlayAni(PlayerAniType.SlashAttack, false);
        FaceCtrl.PlayAni(PlayerAniType.SlashAttack, false);
        UpwearCtrl.PlayAni(PlayerAniType.SlashAttack, false);
        DownwearCtrl.PlayAni(PlayerAniType.SlashAttack, false);
        HairBackCtrl.PlayAni(PlayerAniType.SlashAttack, false);
        HairFrontCtrl.PlayAni(PlayerAniType.SlashAttack, false);
        HandBackCtrl.PlayAni(PlayerAniType.SlashAttack, false);
        HandFrontCtrl.PlayAni(PlayerAniType.SlashAttack, false);
        SuitCtrl.PlayAni(PlayerAniType.SlashAttack, false);
    }
    public void PlayUpper()
    {
        screenCtrl.canCtrl = false;
        ShoesCtrl.PlayAni(PlayerAniType.UpperAttack, false);
        FaceCtrl.PlayAni(PlayerAniType.UpperAttack, false);
        UpwearCtrl.PlayAni(PlayerAniType.UpperAttack, false);
        DownwearCtrl.PlayAni(PlayerAniType.UpperAttack, false);
        HairBackCtrl.PlayAni(PlayerAniType.UpperAttack, false);
        HairFrontCtrl.PlayAni(PlayerAniType.UpperAttack, false);
        HandBackCtrl.PlayAni(PlayerAniType.UpperAttack, false);
        HandFrontCtrl.PlayAni(PlayerAniType.UpperAttack, false);
        SuitCtrl.PlayAni(PlayerAniType.UpperAttack, false);
    }
    public void PlayHurt()
    {
        screenCtrl.canCtrl = false;
        ShoesCtrl.PlayAni(PlayerAniType.Hurt, false);
        FaceCtrl.PlayAni(PlayerAniType.Hurt, false);
        UpwearCtrl.PlayAni(PlayerAniType.Hurt, false);
        DownwearCtrl.PlayAni(PlayerAniType.Hurt, false);
        HairBackCtrl.PlayAni(PlayerAniType.Hurt, false);
        HairFrontCtrl.PlayAni(PlayerAniType.Hurt, false);
        HandBackCtrl.PlayAni(PlayerAniType.Hurt, false);
        HandFrontCtrl.PlayAni(PlayerAniType.Hurt, false);
        SuitCtrl.PlayAni(PlayerAniType.Hurt, false);
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
        ShoesCtrl.PlayAni(PlayerAniType.Death, false);
        FaceCtrl.PlayAni(PlayerAniType.Death, false);
        UpwearCtrl.PlayAni(PlayerAniType.Death, false);
        DownwearCtrl.PlayAni(PlayerAniType.Death, false);
        HairBackCtrl.PlayAni(PlayerAniType.Death, false);
        HairFrontCtrl.PlayAni(PlayerAniType.Death, false);
        HandBackCtrl.PlayAni(PlayerAniType.Death, false);
        HandFrontCtrl.PlayAni(PlayerAniType.Death, false);
        SuitCtrl.PlayAni(PlayerAniType.Death, false);
    }
    public void PlayMagic()
    {
        screenCtrl.canCtrl = false;
        ShoesCtrl.PlayAni(PlayerAniType.MagicAttack, false);
        FaceCtrl.PlayAni(PlayerAniType.MagicAttack, false);
        UpwearCtrl.PlayAni(PlayerAniType.MagicAttack, false);
        DownwearCtrl.PlayAni(PlayerAniType.MagicAttack, false);
        HairBackCtrl.PlayAni(PlayerAniType.MagicAttack, false);
        HairFrontCtrl.PlayAni(PlayerAniType.MagicAttack, false);
        HandBackCtrl.PlayAni(PlayerAniType.MagicAttack, false);
        HandFrontCtrl.PlayAni(PlayerAniType.MagicAttack, false);
        SuitCtrl.PlayAni(PlayerAniType.MagicAttack, false);
    }
    public void PlayCleric()
    {
        screenCtrl.canCtrl = false;
        ShoesCtrl.PlayAni(PlayerAniType.ClericAttack, false);
        FaceCtrl.PlayAni(PlayerAniType.ClericAttack, false);
        UpwearCtrl.PlayAni(PlayerAniType.ClericAttack, false);
        DownwearCtrl.PlayAni(PlayerAniType.ClericAttack, false);
        HairBackCtrl.PlayAni(PlayerAniType.ClericAttack, false);
        HairFrontCtrl.PlayAni(PlayerAniType.ClericAttack, false);
        HandBackCtrl.PlayAni(PlayerAniType.ClericAttack, false);
        HandFrontCtrl.PlayAni(PlayerAniType.ClericAttack, false);
        SuitCtrl.PlayAni(PlayerAniType.ClericAttack, false);
    }
    public void PlayBow()
    {
        screenCtrl.canCtrl = false;
        ShoesCtrl.PlayAni(PlayerAniType.BowAttack, false);
        FaceCtrl.PlayAni(PlayerAniType.BowAttack, false);
        UpwearCtrl.PlayAni(PlayerAniType.BowAttack, false);
        DownwearCtrl.PlayAni(PlayerAniType.BowAttack, false);
        HairBackCtrl.PlayAni(PlayerAniType.BowAttack, false);
        HairFrontCtrl.PlayAni(PlayerAniType.BowAttack, false);
        HandBackCtrl.PlayAni(PlayerAniType.BowAttack, false);
        HandFrontCtrl.PlayAni(PlayerAniType.BowAttack, false);
        SuitCtrl.PlayAni(PlayerAniType.BowAttack, false);
    }
    public void PlayCrossbow()
    {
        screenCtrl.canCtrl = false;
        ShoesCtrl.PlayAni(PlayerAniType.CrossbowAttack, false);
        FaceCtrl.PlayAni(PlayerAniType.CrossbowAttack, false);
        UpwearCtrl.PlayAni(PlayerAniType.CrossbowAttack, false);
        DownwearCtrl.PlayAni(PlayerAniType.CrossbowAttack, false);
        HairBackCtrl.PlayAni(PlayerAniType.CrossbowAttack, false);
        HairFrontCtrl.PlayAni(PlayerAniType.CrossbowAttack, false);
        HandBackCtrl.PlayAni(PlayerAniType.CrossbowAttack, false);
        HandFrontCtrl.PlayAni(PlayerAniType.CrossbowAttack, false);
        SuitCtrl.PlayAni(PlayerAniType.CrossbowAttack, false);
    }
    public void PlayDown1()
    {
        screenCtrl.canCtrl = false;
        ShoesCtrl.PlayAni(PlayerAniType.DownAttack1, false);
        FaceCtrl.PlayAni(PlayerAniType.DownAttack1, false);
        UpwearCtrl.PlayAni(PlayerAniType.DownAttack1, false);
        DownwearCtrl.PlayAni(PlayerAniType.DownAttack1, false);
        HairBackCtrl.PlayAni(PlayerAniType.DownAttack1, false);
        HairFrontCtrl.PlayAni(PlayerAniType.DownAttack1, false);
        HandBackCtrl.PlayAni(PlayerAniType.DownAttack1, false);
        HandFrontCtrl.PlayAni(PlayerAniType.DownAttack1, false);
        SuitCtrl.PlayAni(PlayerAniType.DownAttack1, false);
    }
    public void PlayDown2()
    {
        screenCtrl.canCtrl = false;
        ShoesCtrl.PlayAni(PlayerAniType.DownAttack2, false);
        FaceCtrl.PlayAni(PlayerAniType.DownAttack2, false);
        UpwearCtrl.PlayAni(PlayerAniType.DownAttack2, false);
        DownwearCtrl.PlayAni(PlayerAniType.DownAttack2, false);
        HairBackCtrl.PlayAni(PlayerAniType.DownAttack2, false);
        HairFrontCtrl.PlayAni(PlayerAniType.DownAttack2, false);
        HandBackCtrl.PlayAni(PlayerAniType.DownAttack2, false);
        HandFrontCtrl.PlayAni(PlayerAniType.DownAttack2, false);
        SuitCtrl.PlayAni(PlayerAniType.DownAttack2, false);
    }
    public void PlayHorizon1()
    {
        screenCtrl.canCtrl = false;
        ShoesCtrl.PlayAni(PlayerAniType.HorizontalAttack1, false);
        FaceCtrl.PlayAni(PlayerAniType.HorizontalAttack1, false);
        UpwearCtrl.PlayAni(PlayerAniType.HorizontalAttack1, false);
        DownwearCtrl.PlayAni(PlayerAniType.HorizontalAttack1, false);
        HairBackCtrl.PlayAni(PlayerAniType.HorizontalAttack1, false);
        HairFrontCtrl.PlayAni(PlayerAniType.HorizontalAttack1, false);
        HandBackCtrl.PlayAni(PlayerAniType.HorizontalAttack1, false);
        HandFrontCtrl.PlayAni(PlayerAniType.HorizontalAttack1, false);
        SuitCtrl.PlayAni(PlayerAniType.HorizontalAttack1, false);
    }
    public void PlayHorizon2()
    {
        screenCtrl.canCtrl = false;
        ShoesCtrl.PlayAni(PlayerAniType.HorizontalAttack2, false);
        FaceCtrl.PlayAni(PlayerAniType.HorizontalAttack2, false);
        UpwearCtrl.PlayAni(PlayerAniType.HorizontalAttack2, false);
        DownwearCtrl.PlayAni(PlayerAniType.HorizontalAttack2, false);
        HairBackCtrl.PlayAni(PlayerAniType.HorizontalAttack2, false);
        HairFrontCtrl.PlayAni(PlayerAniType.HorizontalAttack2, false);
        HandBackCtrl.PlayAni(PlayerAniType.HorizontalAttack2, false);
        HandFrontCtrl.PlayAni(PlayerAniType.HorizontalAttack2, false);
        SuitCtrl.PlayAni(PlayerAniType.HorizontalAttack2, false);
    }
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
        UISystem.Instance.InfoWnd.UpdateHp(GameRoot.Instance.ActivePlayer.HP - damage);
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
        UISystem.Instance.InfoWnd.SetDeathHP();
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

    public void DeleteThisChr()
    {
        Destroy(this.gameObject);
    }
}

#region ActionTask
[Category("Character/")]
public class ChangeSpeed : ActionTask<Transform>
{
    protected override void OnExecute()
    {
        agent.GetComponent<PlayerController>().ShoesCtrl.IsAniPause = false;
        agent.GetComponent<PlayerController>().FaceCtrl.IsAniPause = false;
        agent.GetComponent<PlayerController>().UpwearCtrl.IsAniPause = false;
        agent.GetComponent<PlayerController>().DownwearCtrl.IsAniPause = false;
        agent.GetComponent<PlayerController>().HairFrontCtrl.IsAniPause = false;
        agent.GetComponent<PlayerController>().HairBackCtrl.IsAniPause = false;
        agent.GetComponent<PlayerController>().HandBackCtrl.IsAniPause = false;
        agent.GetComponent<PlayerController>().HandFrontCtrl.IsAniPause = false;
        agent.GetComponent<PlayerController>().SuitCtrl.IsAniPause = false;
        agent.GetComponent<PlayerController>().PlayIdle();
        base.OnExecute();
        EndAction();
    }
}

[Category("Character/")]
public class StartAni : ActionTask<Transform>
{
    protected override void OnExecute()
    {
        agent.GetComponent<PlayerController>().ShoesCtrl.IsAniPause = false;
        agent.GetComponent<PlayerController>().FaceCtrl.IsAniPause = false;
        agent.GetComponent<PlayerController>().UpwearCtrl.IsAniPause = false;
        agent.GetComponent<PlayerController>().DownwearCtrl.IsAniPause = false;
        agent.GetComponent<PlayerController>().HairFrontCtrl.IsAniPause = false;
        agent.GetComponent<PlayerController>().HairBackCtrl.IsAniPause = false;
        agent.GetComponent<PlayerController>().HandBackCtrl.IsAniPause = false;
        agent.GetComponent<PlayerController>().HandFrontCtrl.IsAniPause = false;
        agent.GetComponent<PlayerController>().SuitCtrl.IsAniPause = false;
        agent.GetComponent<PlayerController>().PlayIdle();
        base.OnExecute();
        EndAction();
    }
}

[Category("Character/")]
public class PauseAni : ActionTask<Transform>
{
    protected override void OnExecute()
    {
        agent.GetComponent<PlayerController>().ShoesCtrl.IsAniPause = true;
        agent.GetComponent<PlayerController>().FaceCtrl.IsAniPause = true;
        agent.GetComponent<PlayerController>().UpwearCtrl.IsAniPause = true;
        agent.GetComponent<PlayerController>().DownwearCtrl.IsAniPause = true;
        agent.GetComponent<PlayerController>().HairFrontCtrl.IsAniPause = true;
        agent.GetComponent<PlayerController>().HairBackCtrl.IsAniPause = true;
        agent.GetComponent<PlayerController>().HandBackCtrl.IsAniPause = true;
        agent.GetComponent<PlayerController>().HandFrontCtrl.IsAniPause = true;
        agent.GetComponent<PlayerController>().SuitCtrl.IsAniPause = true;
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
        PlayerController ctrl = agent.transform.GetComponent<PlayerController>();
        //控制人物方向和動畫
        if (MoveX > 0)
        {
            agent.transform.localScale = new Vector3(1, 1, 1);
            ctrl.DamageContainer.transform.localScale = new Vector3(Mathf.Abs(ctrl.DamageContainer.transform.localScale.x), ctrl.DamageContainer.transform.localScale.y, ctrl.DamageContainer.transform.localScale.z);
            ctrl.NameBox.transform.localScale = new Vector3(agent.transform.localScale.x, 1, 1);
            ctrl.ChatBox.transform.localScale = new Vector3(agent.transform.localScale.x, 1, 1);
            ctrl.Shadow.localScale = new Vector3(100 * ctrl.transform.localScale.x, 100, 1);
            ctrl.HpBar.transform.localScale = new Vector3(-agent.transform.localScale.x, 1, 1);
            ctrl.TitleText.transform.localScale = new Vector3(agent.transform.localScale.x, 1, 1);
            ctrl.GuildText.transform.localScale = new Vector3(agent.transform.localScale.x, 1, 1);
        }
        else if (MoveX < 0)
        {
            agent.transform.localScale = new Vector3(-1, 1, 1);
            ctrl.DamageContainer.transform.localScale = new Vector3(-Mathf.Abs(ctrl.DamageContainer.transform.localScale.x), ctrl.DamageContainer.transform.localScale.y, ctrl.DamageContainer.transform.localScale.z);
            ctrl.NameBox.transform.localScale = new Vector3(agent.transform.localScale.x, 1, 1);
            ctrl.ChatBox.transform.localScale = new Vector3(agent.transform.localScale.x, 1, 1);
            ctrl.Shadow.localScale = new Vector3(100 * ctrl.transform.localScale.x, 100, 1);
            ctrl.HpBar.transform.localScale = new Vector3(-agent.transform.localScale.x, 1, 1);
            ctrl.TitleText.transform.localScale = new Vector3(agent.transform.localScale.x, 1, 1);
            ctrl.GuildText.transform.localScale = new Vector3(agent.transform.localScale.x, 1, 1);
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
        Dictionary<int, MonsterController> Monsters = BattleSys.Instance.Monsters;
        GameObject target = blackboard.GetVariable<GameObject>("EnemyTarget").value;
        if (target != null) //有目標
        {
            if (Vector3.Distance(target.transform.position, agent.transform.position) > DetectionRange) //如果超過偵測範圍
            {
                blackboard.SetVariableValue("EnemyTarget", null);
                target.GetComponent<MonsterController>().HideProfile();//隱藏怪物HP
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
                NewTarget.GetComponent<MonsterController>().ShowProfile();
                blackboard.SetVariableValue("EnemyTarget", NewTarget.gameObject);
                BattleSys.Instance.LockTarget(NewTarget.GetComponent<MonsterController>());
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
            agent.GetComponent<PlayerController>().PlayIdle();
            base.OnExecute();
            EndAction();
        }
        catch (Exception)
        {
            agent.GetComponent<PlayerController>().PlayIdle();
            EndAction();
        }
    }
}

[Category("Character/")]
public class PlayCharacterWalkAni : ActionTask<Transform>
{
    protected override void OnExecute()
    {
        agent.GetComponent<PlayerController>().PlayWalk();
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
        agent.GetComponent<PlayerController>().PlayRun();
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
                agent.GetComponent<PlayerController>().PlayBow();
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
        MonsterController monAi = (MonsterController)BattleSys.Instance.CurrentTarget;
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
        PlayerController ai = agent.transform.GetComponent<PlayerController>();
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
            PlayerController ai = agent.GetComponent<PlayerController>();
            ai.IsHurt = false;
            EndAction();
        }
        catch (Exception)
        {
        }

    }
    protected override void OnExecute()
    {
        PlayerController ai = agent.GetComponent<PlayerController>();
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
        PlayerController ai = agent.GetComponent<PlayerController>();
        EndAction();
    }
    protected override void OnExecute()
    {
        PlayerController ai = agent.GetComponent<PlayerController>();
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