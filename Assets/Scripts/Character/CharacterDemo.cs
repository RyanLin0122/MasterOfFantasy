using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;

public class CharacterDemo : MonoBehaviour
{
    public EquipmentAnimator ShoesCtrl;
    public EquipmentAnimator FaceCtrl;
    public EquipmentAnimator UpwearCtrl;
    public EquipmentAnimator DownwearCtrl;
    public EquipmentAnimator HairFrontCtrl;
    public EquipmentAnimator HairBackCtrl;
    public EquipmentAnimator HandBackCtrl;
    public EquipmentAnimator HandFrontCtrl;
    public EquipmentAnimator SuitCtrl;
    #region playe animation
    private void Awake()
    {
        InitCtrls();
    }

    public void SetAllEquipment(Player player)
    {
        SetEquipment(player, EquipmentType.Shoes);
        SetEquipment(player, EquipmentType.Chest);
        SetEquipment(player, EquipmentType.Pant);
        SetEquipment(player, EquipmentType.Gloves);
        SetEquipment(player, EquipmentType.HairStyle);
        SetFace(player);
        PlayIdle();
    }
    public void SetEquipment(Player pd, EquipmentType Type)
    {
        switch (Type)
        {
            case EquipmentType.Shoes:
                ShoesCtrl.gameObject.SetActive(true);
                if (pd.playerEquipments.F_Chest == null) //沒有時裝衣服
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
                else //有穿時裝衣服
                {
                    if (pd.playerEquipments.F_Chest.ItemID <= 7000) //如果不是套裝
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
                    else //穿套裝
                    {
                        //關閉鞋子
                        ShoesCtrl.gameObject.SetActive(false);
                    }
                }


                break;
            case EquipmentType.Chest:
                UpwearCtrl.gameObject.SetActive(true);
                SuitCtrl.gameObject.SetActive(false);
                if (pd.playerEquipments.F_Chest != null) //有時裝衣服
                {
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
                }
                else
                {
                    ChangeDefaultEquipment(pd.Gender, EquipmentType.Chest);
                }
                break;
            case EquipmentType.Pant:
                DownwearCtrl.gameObject.SetActive(true);
                if (pd.playerEquipments.F_Chest == null) //沒有時裝衣服
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
                    if (pd.playerEquipments.F_Chest.ItemID <= 7000) //如果不是套裝
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
                        }
                    }
                    else //按脫掉
                    {
                        //關閉鞋子
                        ShoesCtrl.gameObject.SetActive(false);
                    }
                }
                break;
            case EquipmentType.Gloves:
                HandBackCtrl.gameObject.SetActive(true);
                HandFrontCtrl.gameObject.SetActive(true);
                if (pd.playerEquipments.F_Chest != null)
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
    public void SetFace(Player pd)
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


    public void ActivateAnimator()
    {
        ShoesCtrl.gameObject.SetActive(true);
        FaceCtrl.gameObject.SetActive(true);
        UpwearCtrl.gameObject.SetActive(true);
        DownwearCtrl.gameObject.SetActive(true);
        HairBackCtrl.gameObject.SetActive(true);
        HairFrontCtrl.gameObject.SetActive(true);
        HandBackCtrl.gameObject.SetActive(true);
        HandFrontCtrl.gameObject.SetActive(true);
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
                    Debug.Log("Chest ID: " + id);
                    SuitCtrl.gameObject.SetActive(false);
                    ShoesCtrl.gameObject.SetActive(true);
                    UpwearCtrl.gameObject.SetActive(true);
                    DownwearCtrl.gameObject.SetActive(true);
                    HandBackCtrl.gameObject.SetActive(true);
                    HandFrontCtrl.gameObject.SetActive(true);
                    SuitCtrl.LoadDefaultSprite();
                    UpwearCtrl.LoadSprite(ResSvc.Instance.GetEquipSpritePath(id)[0]);
                }
                else
                {
                    Debug.Log("Chest ID: " + id);
                    SuitCtrl.gameObject.SetActive(true);
                    ShoesCtrl.gameObject.SetActive(false);
                    UpwearCtrl.gameObject.SetActive(false);
                    DownwearCtrl.gameObject.SetActive(false);
                    HandBackCtrl.gameObject.SetActive(false);
                    HandFrontCtrl.gameObject.SetActive(false);
                    Debug.Log("Path: " + ResSvc.Instance.GetEquipSpritePath(id)[0]);
                    SuitCtrl.LoadSprite(ResSvc.Instance.GetEquipSpritePath(id)[0]);
                    UpwearCtrl.LoadDefaultSprite();
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
        ForceSynchronizedPlayerCtrl();
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
        ForceSynchronizedPlayerCtrl();
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
    public void ForceSynchronizedPlayerCtrl()
    {
        ShoesCtrl.ResetAni();
        FaceCtrl.ResetAni();
        UpwearCtrl.ResetAni();
        DownwearCtrl.ResetAni();
        HairBackCtrl.ResetAni();
        HairFrontCtrl.ResetAni();
        HandBackCtrl.ResetAni();
        HandFrontCtrl.ResetAni();
        SuitCtrl.ResetAni();
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
    #endregion
}
