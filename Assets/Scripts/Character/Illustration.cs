using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;
using UnityEngine.UI;

public class Illustration : MonoBehaviour
{
    public GameObject Child_M;
    public GameObject Teen_M;
    public GameObject Adult_M;
    public GameObject Child_F;
    public GameObject Teen_F;
    public GameObject Adult_F;
    #region Image
    public Image Child_Suit;
    public Image Child_Cape;
    public Image Child_CapeFront;
    public Image Child_Chest;
    public Image Child_Face;
    public Image Child_FaceAcc;
    public Image Child_Glasses;
    public Image Child_GloveFront;
    public Image Child_GloveBack;
    public Image Child_HairAcc;
    public Image Child_HairFront;
    public Image Child_HairBack;
    public Image Child_Pants;
    public Image Child_Shoes;

    public Image Teen_Suit;
    public Image Teen_Cape;
    public Image Teen_CapeFront;
    public Image Teen_Chest;
    public Image Teen_Face;
    public Image Teen_FaceAcc;
    public Image Teen_Glasses;
    public Image Teen_GloveFront;
    public Image Teen_GloveBack;
    public Image Teen_HairAcc;
    public Image Teen_HairFront;
    public Image Teen_HairBack;
    public Image Teen_Pants;
    public Image Teen_Shoes;

    public Image Adult_Suit;
    public Image Adult_Cape;
    public Image Adult_CapeFront;
    public Image Adult_Chest;
    public Image Adult_Face;
    public Image Adult_FaceAcc;
    public Image Adult_Glasses;
    public Image Adult_GloveFront;
    public Image Adult_GloveBack;
    public Image Adult_HairAcc;
    public Image Adult_HairFront;
    public Image Adult_HairBack;
    public Image Adult_Pants;
    public Image Adult_Shoes;

    public Image FChild_Suit;
    public Image FChild_Cape;
    public Image FChild_CapeFront;
    public Image FChild_Chest;
    public Image FChild_Face;
    public Image FChild_FaceAcc;
    public Image FChild_Glasses;
    public Image FChild_GloveFront;
    public Image FChild_GloveBack;
    public Image FChild_HairAcc;
    public Image FChild_HairFront;
    public Image FChild_HairBack;
    public Image FChild_Pants;
    public Image FChild_Shoes;

    public Image FTeen_Suit;
    public Image FTeen_Cape;
    public Image FTeen_CapeFront;
    public Image FTeen_Chest;
    public Image FTeen_Face;
    public Image FTeen_FaceAcc;
    public Image FTeen_Glasses;
    public Image FTeen_GloveFront;
    public Image FTeen_GloveBack;
    public Image FTeen_HairAcc;
    public Image FTeen_HairFront;
    public Image FTeen_HairBack;
    public Image FTeen_Pants;
    public Image FTeen_Shoes;

    public Image FAdult_Suit;
    public Image FAdult_Cape;
    public Image FAdult_CapeFront;
    public Image FAdult_Chest;
    public Image FAdult_Face;
    public Image FAdult_FaceAcc;
    public Image FAdult_Glasses;
    public Image FAdult_GloveFront;
    public Image FAdult_GloveBack;
    public Image FAdult_HairAcc;
    public Image FAdult_HairFront;
    public Image FAdult_HairBack;
    public Image FAdult_Pants;
    public Image FAdult_Shoes;
    public List<Image> Images = new List<Image>();
    public List<Sprite> DefaultSprites = new List<Sprite>();
    List<Vector3> DefaultScale = new List<Vector3>();
    List<Vector3> DefaultPosition = new List<Vector3>();
    #endregion


    public Vector4 ImageToRect(Image img)
    {
        return new Vector4(img.transform.localPosition.x, img.transform.localPosition.y, img.transform.localScale.x, img.transform.localScale.y);
    }
    public void InitIllustration() //初始化暫存原始位置，圖片，和大小
    {
        if (Images.Count < 50)
        {
            Images.Add(Child_Suit);
            Images.Add(Child_Cape);
            Images.Add(Child_CapeFront);
            Images.Add(Child_Chest);
            Images.Add(Child_Face);
            Images.Add(Child_FaceAcc);
            Images.Add(Child_Glasses);
            Images.Add(Child_GloveFront);
            Images.Add(Child_GloveBack);
            Images.Add(Child_HairAcc);
            Images.Add(Child_HairFront);
            Images.Add(Child_HairBack);
            Images.Add(Child_Pants);
            Images.Add(Child_Shoes);

            Images.Add(Teen_Suit);
            Images.Add(Teen_Cape);
            Images.Add(Teen_CapeFront);
            Images.Add(Teen_Chest);
            Images.Add(Teen_Face);
            Images.Add(Teen_FaceAcc);
            Images.Add(Teen_Glasses);
            Images.Add(Teen_GloveFront);
            Images.Add(Teen_GloveBack);
            Images.Add(Teen_HairAcc);
            Images.Add(Teen_HairFront);
            Images.Add(Teen_HairBack);
            Images.Add(Teen_Pants);
            Images.Add(Teen_Shoes);

            Images.Add(Adult_Suit);
            Images.Add(Adult_Cape);
            Images.Add(Adult_CapeFront);
            Images.Add(Adult_Chest);
            Images.Add(Adult_Face);
            Images.Add(Adult_FaceAcc);
            Images.Add(Adult_Glasses);
            Images.Add(Adult_GloveFront);
            Images.Add(Adult_GloveBack);
            Images.Add(Adult_HairAcc);
            Images.Add(Adult_HairFront);
            Images.Add(Adult_HairBack);
            Images.Add(Adult_Pants);
            Images.Add(Adult_Shoes);

            Images.Add(FChild_Suit);
            Images.Add(FChild_Cape);
            Images.Add(FChild_CapeFront);
            Images.Add(FChild_Chest);
            Images.Add(FChild_Face);
            Images.Add(FChild_FaceAcc);
            Images.Add(FChild_Glasses);
            Images.Add(FChild_GloveFront);
            Images.Add(FChild_GloveBack);
            Images.Add(FChild_HairAcc);
            Images.Add(FChild_HairFront);
            Images.Add(FChild_HairBack);
            Images.Add(FChild_Pants);
            Images.Add(FChild_Shoes);

            Images.Add(FTeen_Suit);
            Images.Add(FTeen_Cape);
            Images.Add(FTeen_CapeFront);
            Images.Add(FTeen_Chest);
            Images.Add(FTeen_Face);
            Images.Add(FTeen_FaceAcc);
            Images.Add(FTeen_Glasses);
            Images.Add(FTeen_GloveFront);
            Images.Add(FTeen_GloveBack);
            Images.Add(FTeen_HairAcc);
            Images.Add(FTeen_HairFront);
            Images.Add(FTeen_HairBack);
            Images.Add(FTeen_Pants);
            Images.Add(FTeen_Shoes);

            Images.Add(FAdult_Suit);
            Images.Add(FAdult_Cape);
            Images.Add(FAdult_CapeFront);
            Images.Add(FAdult_Chest);
            Images.Add(FAdult_Face);
            Images.Add(FAdult_FaceAcc);
            Images.Add(FAdult_Glasses);
            Images.Add(FAdult_GloveFront);
            Images.Add(FAdult_GloveBack);
            Images.Add(FAdult_HairAcc);
            Images.Add(FAdult_HairFront);
            Images.Add(FAdult_HairBack);
            Images.Add(FAdult_Pants);
            Images.Add(FAdult_Shoes);

            DefaultSprites.Add(Child_Suit.sprite);
            DefaultSprites.Add(Child_Cape.sprite);
            DefaultSprites.Add(Child_CapeFront.sprite);
            DefaultSprites.Add(Child_Chest.sprite);
            DefaultSprites.Add(Child_Face.sprite);
            DefaultSprites.Add(Child_FaceAcc.sprite);
            DefaultSprites.Add(Child_Glasses.sprite);
            DefaultSprites.Add(Child_GloveFront.sprite);
            DefaultSprites.Add(Child_GloveBack.sprite);
            DefaultSprites.Add(Child_HairAcc.sprite);
            DefaultSprites.Add(Child_HairFront.sprite);
            DefaultSprites.Add(Child_HairBack.sprite);
            DefaultSprites.Add(Child_Pants.sprite);
            DefaultSprites.Add(Child_Shoes.sprite);

            DefaultSprites.Add(Teen_Suit.sprite);
            DefaultSprites.Add(Teen_Cape.sprite);
            DefaultSprites.Add(Teen_CapeFront.sprite);
            DefaultSprites.Add(Teen_Chest.sprite);
            DefaultSprites.Add(Teen_Face.sprite);
            DefaultSprites.Add(Teen_FaceAcc.sprite);
            DefaultSprites.Add(Teen_Glasses.sprite);
            DefaultSprites.Add(Teen_GloveFront.sprite);
            DefaultSprites.Add(Teen_GloveBack.sprite);
            DefaultSprites.Add(Teen_HairAcc.sprite);
            DefaultSprites.Add(Teen_HairFront.sprite);
            DefaultSprites.Add(Teen_HairBack.sprite);
            DefaultSprites.Add(Teen_Pants.sprite);
            DefaultSprites.Add(Teen_Shoes.sprite);

            DefaultSprites.Add(Adult_Suit.sprite);
            DefaultSprites.Add(Adult_Cape.sprite);
            DefaultSprites.Add(Adult_CapeFront.sprite);
            DefaultSprites.Add(Adult_Chest.sprite);
            DefaultSprites.Add(Adult_Face.sprite);
            DefaultSprites.Add(Adult_FaceAcc.sprite);
            DefaultSprites.Add(Adult_Glasses.sprite);
            DefaultSprites.Add(Adult_GloveFront.sprite);
            DefaultSprites.Add(Adult_GloveBack.sprite);
            DefaultSprites.Add(Adult_HairAcc.sprite);
            DefaultSprites.Add(Adult_HairFront.sprite);
            DefaultSprites.Add(Adult_HairBack.sprite);
            DefaultSprites.Add(Adult_Pants.sprite);
            DefaultSprites.Add(Adult_Shoes.sprite);

            DefaultSprites.Add(FChild_Suit.sprite);
            DefaultSprites.Add(FChild_Cape.sprite);
            DefaultSprites.Add(FChild_CapeFront.sprite);
            DefaultSprites.Add(FChild_Chest.sprite);
            DefaultSprites.Add(FChild_Face.sprite);
            DefaultSprites.Add(FChild_FaceAcc.sprite);
            DefaultSprites.Add(FChild_Glasses.sprite);
            DefaultSprites.Add(FChild_GloveFront.sprite);
            DefaultSprites.Add(FChild_GloveBack.sprite);
            DefaultSprites.Add(FChild_HairAcc.sprite);
            DefaultSprites.Add(FChild_HairFront.sprite);
            DefaultSprites.Add(FChild_HairBack.sprite);
            DefaultSprites.Add(FChild_Pants.sprite);
            DefaultSprites.Add(FChild_Shoes.sprite);

            DefaultSprites.Add(FTeen_Suit.sprite);
            DefaultSprites.Add(FTeen_Cape.sprite);
            DefaultSprites.Add(FTeen_CapeFront.sprite);
            DefaultSprites.Add(FTeen_Chest.sprite);
            DefaultSprites.Add(FTeen_Face.sprite);
            DefaultSprites.Add(FTeen_FaceAcc.sprite);
            DefaultSprites.Add(FTeen_Glasses.sprite);
            DefaultSprites.Add(FTeen_GloveFront.sprite);
            DefaultSprites.Add(FTeen_GloveBack.sprite);
            DefaultSprites.Add(FTeen_HairAcc.sprite);
            DefaultSprites.Add(FTeen_HairFront.sprite);
            DefaultSprites.Add(FTeen_HairBack.sprite);
            DefaultSprites.Add(FTeen_Pants.sprite);
            DefaultSprites.Add(FTeen_Shoes.sprite);

            DefaultSprites.Add(FAdult_Suit.sprite);
            DefaultSprites.Add(FAdult_Cape.sprite);
            DefaultSprites.Add(FAdult_CapeFront.sprite);
            DefaultSprites.Add(FAdult_Chest.sprite);
            DefaultSprites.Add(FAdult_Face.sprite);
            DefaultSprites.Add(FAdult_FaceAcc.sprite);
            DefaultSprites.Add(FAdult_Glasses.sprite);
            DefaultSprites.Add(FAdult_GloveFront.sprite);
            DefaultSprites.Add(FAdult_GloveBack.sprite);
            DefaultSprites.Add(FAdult_HairAcc.sprite);
            DefaultSprites.Add(FAdult_HairFront.sprite);
            DefaultSprites.Add(FAdult_HairBack.sprite);
            DefaultSprites.Add(FAdult_Pants.sprite);
            DefaultSprites.Add(FAdult_Shoes.sprite);

            DefaultPosition.Add(Child_Suit.transform.localPosition);
            DefaultPosition.Add(Child_Cape.transform.localPosition);
            DefaultPosition.Add(Child_CapeFront.transform.localPosition);
            DefaultPosition.Add(Child_Chest.transform.localPosition);
            DefaultPosition.Add(Child_Face.transform.localPosition);
            DefaultPosition.Add(Child_FaceAcc.transform.localPosition);
            DefaultPosition.Add(Child_Glasses.transform.localPosition);
            DefaultPosition.Add(Child_GloveFront.transform.localPosition);
            DefaultPosition.Add(Child_GloveBack.transform.localPosition);
            DefaultPosition.Add(Child_HairAcc.transform.localPosition);
            DefaultPosition.Add(Child_HairFront.transform.localPosition);
            DefaultPosition.Add(Child_HairBack.transform.localPosition);
            DefaultPosition.Add(Child_Pants.transform.localPosition);
            DefaultPosition.Add(Child_Shoes.transform.localPosition);

            DefaultPosition.Add(Teen_Suit.transform.localPosition);
            DefaultPosition.Add(Teen_Cape.transform.localPosition);
            DefaultPosition.Add(Teen_CapeFront.transform.localPosition);
            DefaultPosition.Add(Teen_Chest.transform.localPosition);
            DefaultPosition.Add(Teen_Face.transform.localPosition);
            DefaultPosition.Add(Teen_FaceAcc.transform.localPosition);
            DefaultPosition.Add(Teen_Glasses.transform.localPosition);
            DefaultPosition.Add(Teen_GloveFront.transform.localPosition);
            DefaultPosition.Add(Teen_GloveBack.transform.localPosition);
            DefaultPosition.Add(Teen_HairAcc.transform.localPosition);
            DefaultPosition.Add(Teen_HairFront.transform.localPosition);
            DefaultPosition.Add(Teen_HairBack.transform.localPosition);
            DefaultPosition.Add(Teen_Pants.transform.localPosition);
            DefaultPosition.Add(Teen_Shoes.transform.localPosition);

            DefaultPosition.Add(Adult_Suit.transform.localPosition);
            DefaultPosition.Add(Adult_Cape.transform.localPosition);
            DefaultPosition.Add(Adult_CapeFront.transform.localPosition);
            DefaultPosition.Add(Adult_Chest.transform.localPosition);
            DefaultPosition.Add(Adult_Face.transform.localPosition);
            DefaultPosition.Add(Adult_FaceAcc.transform.localPosition);
            DefaultPosition.Add(Adult_Glasses.transform.localPosition);
            DefaultPosition.Add(Adult_GloveFront.transform.localPosition);
            DefaultPosition.Add(Adult_GloveBack.transform.localPosition);
            DefaultPosition.Add(Adult_HairAcc.transform.localPosition);
            DefaultPosition.Add(Adult_HairFront.transform.localPosition);
            DefaultPosition.Add(Adult_HairBack.transform.localPosition);
            DefaultPosition.Add(Adult_Pants.transform.localPosition);
            DefaultPosition.Add(Adult_Shoes.transform.localPosition);

            DefaultPosition.Add(FChild_Suit.transform.localPosition);
            DefaultPosition.Add(FChild_Cape.transform.localPosition);
            DefaultPosition.Add(FChild_CapeFront.transform.localPosition);
            DefaultPosition.Add(FChild_Chest.transform.localPosition);
            DefaultPosition.Add(FChild_Face.transform.localPosition);
            DefaultPosition.Add(FChild_FaceAcc.transform.localPosition);
            DefaultPosition.Add(FChild_Glasses.transform.localPosition);
            DefaultPosition.Add(FChild_GloveFront.transform.localPosition);
            DefaultPosition.Add(FChild_GloveBack.transform.localPosition);
            DefaultPosition.Add(FChild_HairAcc.transform.localPosition);
            DefaultPosition.Add(FChild_HairFront.transform.localPosition);
            DefaultPosition.Add(FChild_HairBack.transform.localPosition);
            DefaultPosition.Add(FChild_Pants.transform.localPosition);
            DefaultPosition.Add(FChild_Shoes.transform.localPosition);

            DefaultPosition.Add(FTeen_Suit.transform.localPosition);
            DefaultPosition.Add(FTeen_Cape.transform.localPosition);
            DefaultPosition.Add(FTeen_CapeFront.transform.localPosition);
            DefaultPosition.Add(FTeen_Chest.transform.localPosition);
            DefaultPosition.Add(FTeen_Face.transform.localPosition);
            DefaultPosition.Add(FTeen_FaceAcc.transform.localPosition);
            DefaultPosition.Add(FTeen_Glasses.transform.localPosition);
            DefaultPosition.Add(FTeen_GloveFront.transform.localPosition);
            DefaultPosition.Add(FTeen_GloveBack.transform.localPosition);
            DefaultPosition.Add(FTeen_HairAcc.transform.localPosition);
            DefaultPosition.Add(FTeen_HairFront.transform.localPosition);
            DefaultPosition.Add(FTeen_HairBack.transform.localPosition);
            DefaultPosition.Add(FTeen_Pants.transform.localPosition);
            DefaultPosition.Add(FTeen_Shoes.transform.localPosition);

            DefaultPosition.Add(FAdult_Suit.transform.localPosition);
            DefaultPosition.Add(FAdult_Cape.transform.localPosition);
            DefaultPosition.Add(FAdult_CapeFront.transform.localPosition);
            DefaultPosition.Add(FAdult_Chest.transform.localPosition);
            DefaultPosition.Add(FAdult_Face.transform.localPosition);
            DefaultPosition.Add(FAdult_FaceAcc.transform.localPosition);
            DefaultPosition.Add(FAdult_Glasses.transform.localPosition);
            DefaultPosition.Add(FAdult_GloveFront.transform.localPosition);
            DefaultPosition.Add(FAdult_GloveBack.transform.localPosition);
            DefaultPosition.Add(FAdult_HairAcc.transform.localPosition);
            DefaultPosition.Add(FAdult_HairFront.transform.localPosition);
            DefaultPosition.Add(FAdult_HairBack.transform.localPosition);
            DefaultPosition.Add(FAdult_Pants.transform.localPosition);
            DefaultPosition.Add(FAdult_Shoes.transform.localPosition);

            DefaultScale.Add(Child_Suit.transform.localScale);
            DefaultScale.Add(Child_Cape.transform.localScale);
            DefaultScale.Add(Child_CapeFront.transform.localScale);
            DefaultScale.Add(Child_Chest.transform.localScale);
            DefaultScale.Add(Child_Face.transform.localScale);
            DefaultScale.Add(Child_FaceAcc.transform.localScale);
            DefaultScale.Add(Child_Glasses.transform.localScale);
            DefaultScale.Add(Child_GloveFront.transform.localScale);
            DefaultScale.Add(Child_GloveBack.transform.localScale);
            DefaultScale.Add(Child_HairAcc.transform.localScale);
            DefaultScale.Add(Child_HairFront.transform.localScale);
            DefaultScale.Add(Child_HairBack.transform.localScale);
            DefaultScale.Add(Child_Pants.transform.localScale);
            DefaultScale.Add(Child_Shoes.transform.localScale);

            DefaultScale.Add(Teen_Suit.transform.localScale);
            DefaultScale.Add(Teen_Cape.transform.localScale);
            DefaultScale.Add(Teen_CapeFront.transform.localScale);
            DefaultScale.Add(Teen_Chest.transform.localScale);
            DefaultScale.Add(Teen_Face.transform.localScale);
            DefaultScale.Add(Teen_FaceAcc.transform.localScale);
            DefaultScale.Add(Teen_Glasses.transform.localScale);
            DefaultScale.Add(Teen_GloveFront.transform.localScale);
            DefaultScale.Add(Teen_GloveBack.transform.localScale);
            DefaultScale.Add(Teen_HairAcc.transform.localScale);
            DefaultScale.Add(Teen_HairFront.transform.localScale);
            DefaultScale.Add(Teen_HairBack.transform.localScale);
            DefaultScale.Add(Teen_Pants.transform.localScale);
            DefaultScale.Add(Teen_Shoes.transform.localScale);

            DefaultScale.Add(Adult_Suit.transform.localScale);
            DefaultScale.Add(Adult_Cape.transform.localScale);
            DefaultScale.Add(Adult_CapeFront.transform.localScale);
            DefaultScale.Add(Adult_Chest.transform.localScale);
            DefaultScale.Add(Adult_Face.transform.localScale);
            DefaultScale.Add(Adult_FaceAcc.transform.localScale);
            DefaultScale.Add(Adult_Glasses.transform.localScale);
            DefaultScale.Add(Adult_GloveFront.transform.localScale);
            DefaultScale.Add(Adult_GloveBack.transform.localScale);
            DefaultScale.Add(Adult_HairAcc.transform.localScale);
            DefaultScale.Add(Adult_HairFront.transform.localScale);
            DefaultScale.Add(Adult_HairBack.transform.localScale);
            DefaultScale.Add(Adult_Pants.transform.localScale);
            DefaultScale.Add(Adult_Shoes.transform.localScale);

            DefaultScale.Add(FChild_Suit.transform.localScale);
            DefaultScale.Add(FChild_Cape.transform.localScale);
            DefaultScale.Add(FChild_CapeFront.transform.localScale);
            DefaultScale.Add(FChild_Chest.transform.localScale);
            DefaultScale.Add(FChild_Face.transform.localScale);
            DefaultScale.Add(FChild_FaceAcc.transform.localScale);
            DefaultScale.Add(FChild_Glasses.transform.localScale);
            DefaultScale.Add(FChild_GloveFront.transform.localScale);
            DefaultScale.Add(FChild_GloveBack.transform.localScale);
            DefaultScale.Add(FChild_HairAcc.transform.localScale);
            DefaultScale.Add(FChild_HairFront.transform.localScale);
            DefaultScale.Add(FChild_HairBack.transform.localScale);
            DefaultScale.Add(FChild_Pants.transform.localScale);
            DefaultScale.Add(FChild_Shoes.transform.localScale);

            DefaultScale.Add(FTeen_Suit.transform.localScale);
            DefaultScale.Add(FTeen_Cape.transform.localScale);
            DefaultScale.Add(FTeen_CapeFront.transform.localScale);
            DefaultScale.Add(FTeen_Chest.transform.localScale);
            DefaultScale.Add(FTeen_Face.transform.localScale);
            DefaultScale.Add(FTeen_FaceAcc.transform.localScale);
            DefaultScale.Add(FTeen_Glasses.transform.localScale);
            DefaultScale.Add(FTeen_GloveFront.transform.localScale);
            DefaultScale.Add(FTeen_GloveBack.transform.localScale);
            DefaultScale.Add(FTeen_HairAcc.transform.localScale);
            DefaultScale.Add(FTeen_HairFront.transform.localScale);
            DefaultScale.Add(FTeen_HairBack.transform.localScale);
            DefaultScale.Add(FTeen_Pants.transform.localScale);
            DefaultScale.Add(FTeen_Shoes.transform.localScale);

            DefaultScale.Add(FAdult_Suit.transform.localScale);
            DefaultScale.Add(FAdult_Cape.transform.localScale);
            DefaultScale.Add(FAdult_CapeFront.transform.localScale);
            DefaultScale.Add(FAdult_Chest.transform.localScale);
            DefaultScale.Add(FAdult_Face.transform.localScale);
            DefaultScale.Add(FAdult_FaceAcc.transform.localScale);
            DefaultScale.Add(FAdult_Glasses.transform.localScale);
            DefaultScale.Add(FAdult_GloveFront.transform.localScale);
            DefaultScale.Add(FAdult_GloveBack.transform.localScale);
            DefaultScale.Add(FAdult_HairAcc.transform.localScale);
            DefaultScale.Add(FAdult_HairFront.transform.localScale);
            DefaultScale.Add(FAdult_HairBack.transform.localScale);
            DefaultScale.Add(FAdult_Pants.transform.localScale);
            DefaultScale.Add(FAdult_Shoes.transform.localScale);
        }

        Debug.Log("初始化Illustration");
    }
    public void LoadPrefab() //載入原始位置，圖片，和大小
    {
        for (int i = 0; i < Images.Count; i++)
        {
            Images[i].sprite = null;
            Images[i].sprite = DefaultSprites[i];
            Images[i].SetNativeSize();
            Images[i].transform.localPosition = DefaultPosition[i];
            Images[i].transform.localScale = DefaultScale[i];
        }
    }
    public void SetGenderAge(bool IsOutlook, bool IsPutOff, Player pd) //第一個執行
    {
        int level = pd.Level;
        //判斷性別
        //判斷等級 關閉其他兩個 只顯示一個
        switch (pd.Gender)
        {
            case 1: //男生
                if (level < 20)
                {
                    Child_M.SetActive(true);
                    Teen_M.SetActive(false);
                    Adult_M.SetActive(false);
                    Child_F.SetActive(false);
                    Teen_F.SetActive(false);
                    Adult_F.SetActive(false);
                }
                else if (20 <= level && level < 40)
                {
                    Child_M.SetActive(false);
                    Teen_M.SetActive(true);
                    Adult_M.SetActive(false);
                    Child_F.SetActive(false);
                    Teen_F.SetActive(false);
                    Adult_F.SetActive(false);
                }
                else
                {
                    Child_M.SetActive(false);
                    Teen_M.SetActive(false);
                    Adult_M.SetActive(true);
                    Child_F.SetActive(false);
                    Teen_F.SetActive(false);
                    Adult_F.SetActive(false);
                }

                break;
            case 0: //女生
                if (level < 20)
                {
                    Child_M.SetActive(false);
                    Teen_M.SetActive(false);
                    Adult_M.SetActive(false);
                    Child_F.SetActive(true);
                    Teen_F.SetActive(false);
                    Adult_F.SetActive(false);
                }
                else if (20 <= level && level < 40)
                {
                    Child_M.SetActive(false);
                    Teen_M.SetActive(false);
                    Adult_M.SetActive(false);
                    Child_F.SetActive(false);
                    Teen_F.SetActive(true);
                    Adult_F.SetActive(false);
                }
                else
                {
                    Child_M.SetActive(false);
                    Teen_M.SetActive(false);
                    Adult_M.SetActive(false);
                    Child_F.SetActive(false);
                    Teen_F.SetActive(false);
                    Adult_F.SetActive(true);
                }
                break;
        }
        SetIllustration(IsOutlook, IsPutOff, pd);
    }
    public void SetIllustration(bool IsOutlook, bool IsPutOff, Player pd)
    {
        LoadPrefab();
        IllustrationPath path = IllustrationPath.Instance;
        //狀態4 顯示點裝 不顯示戰鬥裝 
        if (IsOutlook == true && IsPutOff == false)
        {
            if (pd.Gender == 1)
            {
                if (pd.Level < 20)
                {
                    OpenChild_M();
                    Child_Suit.gameObject.SetActive(false);
                    if (pd.playerEquipments.F_Chest != null)
                    {
                        SetImage(Child_Chest, (path.GetSpriteByID(pd.playerEquipments.F_Chest.ItemID))[0], (path.GetOffsetByID(pd.playerEquipments.F_Chest.ItemID))[0]);
                    }
                    else
                    {
                        SetDefault(3);
                    }
                    if (pd.playerEquipments.F_FaceType != null)
                    {
                        SetImage(Child_Face, (path.GetSpriteByID(pd.playerEquipments.F_FaceType.ItemID))[0], (path.GetOffsetByID(pd.playerEquipments.F_FaceType.ItemID))[0]);
                    }
                    else
                    {
                        SetDefault(4);
                    }
                    if (pd.playerEquipments.F_FaceAcc != null)
                    {
                        SetImage(Child_FaceAcc, (path.GetSpriteByID(pd.playerEquipments.F_FaceAcc.ItemID))[0], (path.GetOffsetByID(pd.playerEquipments.F_FaceAcc.ItemID))[0]);
                    }
                    else
                    {
                        Child_FaceAcc.gameObject.SetActive(false);
                    }
                    if (pd.playerEquipments.F_Glasses != null)
                    {
                        SetImage(Child_Glasses, (path.GetSpriteByID(pd.playerEquipments.F_Glasses.ItemID))[0], (path.GetOffsetByID(pd.playerEquipments.F_Glasses.ItemID))[0]);
                    }
                    else
                    {
                        Child_Glasses.gameObject.SetActive(false);
                    }
                    if (pd.playerEquipments.F_Glove != null)
                    {
                        SetImage(Child_GloveBack, (path.GetSpriteByID(pd.playerEquipments.F_Glove.ItemID))[0], (path.GetOffsetByID(pd.playerEquipments.F_Glove.ItemID))[0]);
                        SetImage(Child_GloveFront, (path.GetSpriteByID(pd.playerEquipments.F_Glove.ItemID))[3], (path.GetOffsetByID(pd.playerEquipments.F_Glove.ItemID))[3]);
                    }
                    else
                    {
                        SetDefault(7);
                        SetDefault(8);
                    }
                    if (pd.playerEquipments.F_Cape != null)
                    {
                        SetImage(Child_Cape, (path.GetSpriteByID(pd.playerEquipments.F_Cape.ItemID))[0], (path.GetOffsetByID(pd.playerEquipments.F_Cape.ItemID))[0]);
                        SetImage(Child_CapeFront, (path.GetSpriteByID(pd.playerEquipments.F_Cape.ItemID))[3], (path.GetOffsetByID(pd.playerEquipments.F_Cape.ItemID))[3]);
                    }
                    else
                    {
                        Child_Cape.gameObject.SetActive(false);
                        Child_CapeFront.gameObject.SetActive(false);
                    }
                    if (pd.playerEquipments.F_Hairacc != null)
                    {
                        SetImage(Child_HairAcc, (path.GetSpriteByID(pd.playerEquipments.F_Hairacc.ItemID))[0], (path.GetOffsetByID(pd.playerEquipments.F_Hairacc.ItemID))[0]);
                    }
                    else
                    {
                        Child_HairAcc.gameObject.SetActive(false);
                    }
                    if (pd.playerEquipments.F_HairStyle != null)
                    {
                        SetImage(Child_HairFront, (path.GetSpriteByID(pd.playerEquipments.F_HairStyle.ItemID))[0], (path.GetOffsetByID(pd.playerEquipments.F_HairStyle.ItemID))[0]);
                        SetImage(Child_HairBack, (path.GetSpriteByID(pd.playerEquipments.F_HairStyle.ItemID))[3], (path.GetOffsetByID(pd.playerEquipments.F_HairStyle.ItemID))[3]);
                    }
                    else
                    {
                        SetDefault(10);
                        SetDefault(11);
                    }
                    if (pd.playerEquipments.F_Pants != null)
                    {
                        SetImage(Child_Pants, (path.GetSpriteByID(pd.playerEquipments.F_Pants.ItemID))[0], (path.GetOffsetByID(pd.playerEquipments.F_Pants.ItemID))[0]);
                    }
                    else
                    {
                        SetDefault(12);
                    }
                    if (pd.playerEquipments.F_Shoes != null)
                    {
                        SetImage(Child_Shoes, (path.GetSpriteByID(pd.playerEquipments.F_Shoes.ItemID))[0], (path.GetOffsetByID(pd.playerEquipments.F_Shoes.ItemID))[0]);
                    }
                    else
                    {
                        SetDefault(13);
                    }
                    if (pd.playerEquipments.F_Chest != null)
                    {
                        if (pd.playerEquipments.F_Chest.ItemID > 7000 && pd.playerEquipments.F_Chest.ItemID < 7300)
                        {
                            Child_Cape.gameObject.SetActive(false);
                            Child_CapeFront.gameObject.SetActive(false);
                            Child_GloveBack.gameObject.SetActive(false);
                            Child_GloveFront.gameObject.SetActive(false);
                            Child_Pants.gameObject.SetActive(false);
                            Child_Shoes.gameObject.SetActive(false);
                        }
                    }
                    else
                    {
                        
                    }
                }
                else if (pd.Level < 40 && pd.Level >= 20)
                {
                    OpenTeen_M();
                    Teen_Suit.gameObject.SetActive(false);
                    if (pd.playerEquipments.F_Chest != null)
                    {              
                        SetImage(Teen_Chest, path.GetSpriteByID(pd.playerEquipments.F_Chest.ItemID)[1], path.GetOffsetByID(pd.playerEquipments.F_Chest.ItemID)[1]);
                    }
                    else
                    {
                        SetDefault(17);
                    }
                    if (pd.playerEquipments.F_FaceType != null)
                    {
                        SetImage(Teen_Face, (path.GetSpriteByID(pd.playerEquipments.F_FaceType.ItemID))[1], (path.GetOffsetByID(pd.playerEquipments.F_FaceType.ItemID))[1]);
                    }
                    else
                    {
                        SetDefault(18);
                    }
                    if (pd.playerEquipments.F_FaceAcc != null)
                    {
                        SetImage(Teen_FaceAcc, (path.GetSpriteByID(pd.playerEquipments.F_FaceAcc.ItemID))[1], (path.GetOffsetByID(pd.playerEquipments.F_FaceAcc.ItemID))[1]);
                    }
                    else
                    {
                        Teen_FaceAcc.gameObject.SetActive(false);
                    }
                    if (pd.playerEquipments.F_Glasses != null)
                    {
                        SetImage(Teen_Glasses, (path.GetSpriteByID(pd.playerEquipments.F_Glasses.ItemID))[1], (path.GetOffsetByID(pd.playerEquipments.F_Glasses.ItemID))[1]);
                    }
                    else
                    {
                        Teen_Glasses.gameObject.SetActive(false);
                    }
                    if (pd.playerEquipments.F_Glove != null)
                    {
                        SetImage(Teen_GloveBack, (path.GetSpriteByID(pd.playerEquipments.F_Glove.ItemID))[1], (path.GetOffsetByID(pd.playerEquipments.F_Glove.ItemID))[1]);
                        SetImage(Teen_GloveFront, (path.GetSpriteByID(pd.playerEquipments.F_Glove.ItemID))[4], (path.GetOffsetByID(pd.playerEquipments.F_Glove.ItemID))[4]);
                    }
                    else
                    {
                        SetDefault(21);
                        SetDefault(22);
                    }
                    if (pd.playerEquipments.F_Cape != null)
                    {
                        SetImage(Teen_Cape, (path.GetSpriteByID(pd.playerEquipments.F_Cape.ItemID))[1], (path.GetOffsetByID(pd.playerEquipments.F_Cape.ItemID))[1]);
                        SetImage(Teen_CapeFront, (path.GetSpriteByID(pd.playerEquipments.F_Cape.ItemID))[4], (path.GetOffsetByID(pd.playerEquipments.F_Cape.ItemID))[4]);
                    }
                    else
                    {
                        Teen_Cape.gameObject.SetActive(false);
                        Teen_CapeFront.gameObject.SetActive(false);
                    }
                    if (pd.playerEquipments.F_Hairacc != null)
                    {
                        SetImage(Teen_HairAcc, (path.GetSpriteByID(pd.playerEquipments.F_Hairacc.ItemID))[1], (path.GetOffsetByID(pd.playerEquipments.F_Hairacc.ItemID))[1]);
                    }
                    else
                    {
                        Teen_HairAcc.gameObject.SetActive(false);
                    }
                    if (pd.playerEquipments.F_HairStyle != null)
                    {
                        SetImage(Teen_HairFront, (path.GetSpriteByID(pd.playerEquipments.F_HairStyle.ItemID))[1], (path.GetOffsetByID(pd.playerEquipments.F_HairStyle.ItemID))[1]);
                        SetImage(Teen_HairBack, (path.GetSpriteByID(pd.playerEquipments.F_HairStyle.ItemID))[4], (path.GetOffsetByID(pd.playerEquipments.F_HairStyle.ItemID))[4]);
                    }
                    else
                    {
                        SetDefault(24);
                        SetDefault(25);
                    }
                    if (pd.playerEquipments.F_Pants != null)
                    {
                        SetImage(Teen_Pants, (path.GetSpriteByID(pd.playerEquipments.F_Pants.ItemID))[1], (path.GetOffsetByID(pd.playerEquipments.F_Pants.ItemID))[1]);
                    }
                    else
                    {
                        SetDefault(26);
                    }
                    if (pd.playerEquipments.F_Shoes != null)
                    {
                        SetImage(Teen_Shoes, (path.GetSpriteByID(pd.playerEquipments.F_Shoes.ItemID))[1], (path.GetOffsetByID(pd.playerEquipments.F_Shoes.ItemID))[1]);
                    }
                    else
                    {
                        SetDefault(27);
                    }
                    if (pd.playerEquipments.F_Chest != null)
                    {
                        if (pd.playerEquipments.F_Chest.ItemID > 7000 && pd.playerEquipments.F_Chest.ItemID < 7300)
                        {
                            Teen_Cape.gameObject.SetActive(false);
                            Teen_CapeFront.gameObject.SetActive(false);
                            Teen_GloveBack.gameObject.SetActive(false);
                            Teen_GloveFront.gameObject.SetActive(false);
                            Teen_Pants.gameObject.SetActive(false);
                            Teen_Shoes.gameObject.SetActive(false);
                        }
                    }
                    else
                    {

                    }
                }
                else
                {
                    OpenAdult_M();
                    Adult_Suit.gameObject.SetActive(false);
                    if (pd.playerEquipments.F_Chest != null)
                    {
                        SetImage(Adult_Chest, path.GetSpriteByID(pd.playerEquipments.F_Chest.ItemID)[2], path.GetOffsetByID(pd.playerEquipments.F_Chest.ItemID)[2]);
                    }
                    else
                    {
                        SetDefault(31);
                    }
                    if (pd.playerEquipments.F_FaceType != null)
                    {
                        SetImage(Adult_Face, (path.GetSpriteByID(pd.playerEquipments.F_FaceType.ItemID))[2], (path.GetOffsetByID(pd.playerEquipments.F_FaceType.ItemID))[2]);
                    }
                    else
                    {
                        SetDefault(32);
                    }
                    if (pd.playerEquipments.F_FaceAcc != null)
                    {
                        SetImage(Adult_FaceAcc, (path.GetSpriteByID(pd.playerEquipments.F_FaceAcc.ItemID))[2], (path.GetOffsetByID(pd.playerEquipments.F_FaceAcc.ItemID))[2]);
                    }
                    else
                    {
                        Adult_FaceAcc.gameObject.SetActive(false);
                    }
                    if (pd.playerEquipments.F_Glasses != null)
                    {
                        SetImage(Adult_Glasses, (path.GetSpriteByID(pd.playerEquipments.F_Glasses.ItemID))[2], (path.GetOffsetByID(pd.playerEquipments.F_Glasses.ItemID))[2]);
                    }
                    else
                    {
                        Adult_Glasses.gameObject.SetActive(false);
                    }
                    if (pd.playerEquipments.F_Glove != null)
                    {
                        SetImage(Adult_GloveBack, (path.GetSpriteByID(pd.playerEquipments.F_Glove.ItemID))[2], (path.GetOffsetByID(pd.playerEquipments.F_Glove.ItemID))[2]);
                        SetImage(Adult_GloveFront, (path.GetSpriteByID(pd.playerEquipments.F_Glove.ItemID))[5], (path.GetOffsetByID(pd.playerEquipments.F_Glove.ItemID))[5]);
                    }
                    else
                    {
                        SetDefault(35);
                        SetDefault(36);
                    }
                    if (pd.playerEquipments.F_Cape != null)
                    {
                        SetImage(Adult_Cape, (path.GetSpriteByID(pd.playerEquipments.F_Cape.ItemID))[2], (path.GetOffsetByID(pd.playerEquipments.F_Cape.ItemID))[2]);
                        SetImage(Adult_CapeFront, (path.GetSpriteByID(pd.playerEquipments.F_Cape.ItemID))[5], (path.GetOffsetByID(pd.playerEquipments.F_Cape.ItemID))[5]);
                    }
                    else
                    {
                        Adult_Cape.gameObject.SetActive(false);
                        Adult_CapeFront.gameObject.SetActive(false);
                    }
                    if (pd.playerEquipments.F_Hairacc != null)
                    {
                        SetImage(Adult_HairAcc, (path.GetSpriteByID(pd.playerEquipments.F_Hairacc.ItemID))[2], (path.GetOffsetByID(pd.playerEquipments.F_Hairacc.ItemID))[2]);
                    }
                    else
                    {
                        Adult_HairAcc.gameObject.SetActive(false);
                    }
                    if (pd.playerEquipments.F_HairStyle != null)
                    {
                        SetImage(Adult_HairFront, (path.GetSpriteByID(pd.playerEquipments.F_HairStyle.ItemID))[2], (path.GetOffsetByID(pd.playerEquipments.F_HairStyle.ItemID))[2]);
                        SetImage(Adult_HairBack, (path.GetSpriteByID(pd.playerEquipments.F_HairStyle.ItemID))[5], (path.GetOffsetByID(pd.playerEquipments.F_HairStyle.ItemID))[5]);
                    }
                    else
                    {
                        SetDefault(38);
                        SetDefault(39);
                    }
                    if (pd.playerEquipments.F_Pants != null)
                    {
                        SetImage(Adult_Pants, (path.GetSpriteByID(pd.playerEquipments.F_Pants.ItemID))[2], (path.GetOffsetByID(pd.playerEquipments.F_Pants.ItemID))[2]);
                    }
                    else
                    {
                        SetDefault(40);
                    }
                    if (pd.playerEquipments.F_Shoes != null)
                    {
                        SetImage(Adult_Shoes, (path.GetSpriteByID(pd.playerEquipments.F_Shoes.ItemID))[2], (path.GetOffsetByID(pd.playerEquipments.F_Shoes.ItemID))[2]);
                    }
                    else
                    {
                        SetDefault(41);
                    }
                    if (pd.playerEquipments.F_Chest != null)
                    {
                        if (pd.playerEquipments.F_Chest.ItemID > 7000 && pd.playerEquipments.F_Chest.ItemID < 7300)
                        {
                            Adult_Cape.gameObject.SetActive(false);
                            Adult_CapeFront.gameObject.SetActive(false);
                            Adult_GloveBack.gameObject.SetActive(false);
                            Adult_GloveFront.gameObject.SetActive(false);
                            Adult_Pants.gameObject.SetActive(false);
                            Adult_Shoes.gameObject.SetActive(false);
                        }
                    }
                    else
                    {

                    }
                }
            }
            else //女生
            {
                if (pd.Level < 20)
                {
                    OpenChild_F();
                    FChild_Suit.gameObject.SetActive(false);
                    if (pd.playerEquipments.F_Chest != null)
                    {
                        SetImage(FChild_Chest, path.GetSpriteByID(pd.playerEquipments.F_Chest.ItemID)[0], path.GetOffsetByID(pd.playerEquipments.F_Chest.ItemID)[0]);
                    }
                    else
                    {
                        SetDefault(45);
                    }
                    if (pd.playerEquipments.F_FaceType != null)
                    {
                        SetImage(FChild_Face, (path.GetSpriteByID(pd.playerEquipments.F_FaceType.ItemID))[0], (path.GetOffsetByID(pd.playerEquipments.F_FaceType.ItemID))[0]);
                    }
                    else
                    {
                        SetDefault(46);
                    }
                    if (pd.playerEquipments.F_FaceAcc != null)
                    {
                        SetImage(FChild_FaceAcc, (path.GetSpriteByID(pd.playerEquipments.F_FaceAcc.ItemID))[0], (path.GetOffsetByID(pd.playerEquipments.F_FaceAcc.ItemID))[0]);
                    }
                    else
                    {
                        FChild_FaceAcc.gameObject.SetActive(false);
                    }
                    if (pd.playerEquipments.F_Glasses != null)
                    {
                        SetImage(FChild_Glasses, (path.GetSpriteByID(pd.playerEquipments.F_Glasses.ItemID))[0], (path.GetOffsetByID(pd.playerEquipments.F_Glasses.ItemID))[0]);
                    }
                    else
                    {
                        FChild_Glasses.gameObject.SetActive(false);
                    }
                    if (pd.playerEquipments.F_Glove != null)
                    {
                        SetImage(FChild_GloveBack, (path.GetSpriteByID(pd.playerEquipments.F_Glove.ItemID))[0], (path.GetOffsetByID(pd.playerEquipments.F_Glove.ItemID))[0]);
                        SetImage(FChild_GloveFront, (path.GetSpriteByID(pd.playerEquipments.F_Glove.ItemID))[3], (path.GetOffsetByID(pd.playerEquipments.F_Glove.ItemID))[3]);
                    }
                    else
                    {
                        SetDefault(49);
                        SetDefault(50);
                    }
                    if (pd.playerEquipments.F_Cape != null)
                    {
                        SetImage(FChild_Cape, (path.GetSpriteByID(pd.playerEquipments.F_Cape.ItemID))[0], (path.GetOffsetByID(pd.playerEquipments.F_Cape.ItemID))[0]);
                        SetImage(FChild_Cape, (path.GetSpriteByID(pd.playerEquipments.F_Cape.ItemID))[3], (path.GetOffsetByID(pd.playerEquipments.F_Cape.ItemID))[3]);
                    }
                    else
                    {
                        FChild_Cape.gameObject.SetActive(false);
                        FChild_CapeFront.gameObject.SetActive(false);
                    }
                    if (pd.playerEquipments.F_Hairacc != null)
                    {
                        SetImage(FChild_HairAcc, (path.GetSpriteByID(pd.playerEquipments.F_Hairacc.ItemID))[0], (path.GetOffsetByID(pd.playerEquipments.F_Hairacc.ItemID))[0]);
                    }
                    else
                    {
                        FChild_HairAcc.gameObject.SetActive(false);
                    }
                    if (pd.playerEquipments.F_HairStyle != null)
                    {
                        SetImage(FChild_HairFront, (path.GetSpriteByID(pd.playerEquipments.F_HairStyle.ItemID))[0], (path.GetOffsetByID(pd.playerEquipments.F_HairStyle.ItemID))[0]);
                        SetImage(FChild_HairBack, (path.GetSpriteByID(pd.playerEquipments.F_HairStyle.ItemID))[3], (path.GetOffsetByID(pd.playerEquipments.F_HairStyle.ItemID))[3]);
                    }
                    else
                    {
                        SetDefault(52);
                        SetDefault(53);
                    }
                    if (pd.playerEquipments.F_Pants != null)
                    {
                        SetImage(FChild_Pants, (path.GetSpriteByID(pd.playerEquipments.F_Pants.ItemID))[0], (path.GetOffsetByID(pd.playerEquipments.F_Pants.ItemID))[0]);
                    }
                    else
                    {
                        SetDefault(54);
                    }
                    if (pd.playerEquipments.F_Shoes != null)
                    {
                        SetImage(FChild_Shoes, (path.GetSpriteByID(pd.playerEquipments.F_Shoes.ItemID))[0], (path.GetOffsetByID(pd.playerEquipments.F_Shoes.ItemID))[0]);
                    }
                    else
                    {
                        SetDefault(55);
                    }
                    if (pd.playerEquipments.F_Chest != null)
                    {
                        if (pd.playerEquipments.F_Chest.ItemID > 7000 && pd.playerEquipments.F_Chest.ItemID < 7300)
                        {
                            FChild_Cape.gameObject.SetActive(false);
                            FChild_CapeFront.gameObject.SetActive(false);
                            FChild_GloveBack.gameObject.SetActive(false);
                            FChild_GloveFront.gameObject.SetActive(false);
                            FChild_Pants.gameObject.SetActive(false);
                            FChild_Shoes.gameObject.SetActive(false);
                        }
                    }
                    else
                    {

                    }
                }
                else if (pd.Level < 40 && pd.Level >= 20)
                {
                    OpenTeen_F();
                    FTeen_Suit.gameObject.SetActive(false);
                    if (pd.playerEquipments.F_Chest != null)
                    {
                        SetImage(FTeen_Chest, path.GetSpriteByID(pd.playerEquipments.F_Chest.ItemID)[1], path.GetOffsetByID(pd.playerEquipments.F_Chest.ItemID)[1]);
                    }
                    else
                    {
                        SetDefault(59);
                    }
                    if (pd.playerEquipments.F_FaceType != null)
                    {
                        SetImage(FTeen_Face, (path.GetSpriteByID(pd.playerEquipments.F_FaceType.ItemID))[1], (path.GetOffsetByID(pd.playerEquipments.F_FaceType.ItemID))[1]);
                    }
                    else
                    {
                        SetDefault(60);
                    }
                    if (pd.playerEquipments.F_FaceAcc != null)
                    {
                        SetImage(FTeen_FaceAcc, (path.GetSpriteByID(pd.playerEquipments.F_FaceAcc.ItemID))[1], (path.GetOffsetByID(pd.playerEquipments.F_FaceAcc.ItemID))[1]);
                    }
                    else
                    {
                        FTeen_FaceAcc.gameObject.SetActive(false);
                    }
                    if (pd.playerEquipments.F_Glasses != null)
                    {
                        SetImage(FTeen_Glasses, (path.GetSpriteByID(pd.playerEquipments.F_Glasses.ItemID))[1], (path.GetOffsetByID(pd.playerEquipments.F_Glasses.ItemID))[1]);
                    }
                    else
                    {
                        FTeen_Glasses.gameObject.SetActive(false);
                    }
                    if (pd.playerEquipments.F_Glove != null)
                    {
                        SetImage(FTeen_GloveBack, (path.GetSpriteByID(pd.playerEquipments.F_Glove.ItemID))[1], (path.GetOffsetByID(pd.playerEquipments.F_Glove.ItemID))[1]);
                        SetImage(FTeen_GloveFront, (path.GetSpriteByID(pd.playerEquipments.F_Glove.ItemID))[4], (path.GetOffsetByID(pd.playerEquipments.F_Glove.ItemID))[4]);
                    }
                    else
                    {
                        SetDefault(63);
                        SetDefault(64);
                    }
                    if (pd.playerEquipments.F_Cape != null)
                    {
                        SetImage(FTeen_Cape, (path.GetSpriteByID(pd.playerEquipments.F_Cape.ItemID))[1], (path.GetOffsetByID(pd.playerEquipments.F_Cape.ItemID))[1]);
                        SetImage(FTeen_Cape, (path.GetSpriteByID(pd.playerEquipments.F_Cape.ItemID))[4], (path.GetOffsetByID(pd.playerEquipments.F_Cape.ItemID))[4]);
                    }
                    else
                    {
                        FTeen_Cape.gameObject.SetActive(false);
                        FTeen_CapeFront.gameObject.SetActive(false);
                    }
                    if (pd.playerEquipments.F_Hairacc != null)
                    {
                        SetImage(FTeen_HairAcc, (path.GetSpriteByID(pd.playerEquipments.F_Hairacc.ItemID))[1], (path.GetOffsetByID(pd.playerEquipments.F_Hairacc.ItemID))[1]);
                    }
                    else
                    {
                        FTeen_HairAcc.gameObject.SetActive(false);
                    }
                    if (pd.playerEquipments.F_HairStyle != null)
                    {
                        SetImage(FTeen_HairFront, (path.GetSpriteByID(pd.playerEquipments.F_HairStyle.ItemID))[1], (path.GetOffsetByID(pd.playerEquipments.F_HairStyle.ItemID))[1]);
                        SetImage(FTeen_HairBack, (path.GetSpriteByID(pd.playerEquipments.F_HairStyle.ItemID))[4], (path.GetOffsetByID(pd.playerEquipments.F_HairStyle.ItemID))[4]);
                    }
                    else
                    {
                        SetDefault(66);
                        SetDefault(67);
                    }
                    if (pd.playerEquipments.F_Pants != null)
                    {
                        SetImage(FTeen_Pants, (path.GetSpriteByID(pd.playerEquipments.F_Pants.ItemID))[1], (path.GetOffsetByID(pd.playerEquipments.F_Pants.ItemID))[1]);
                    }
                    else
                    {
                        SetDefault(68);
                    }
                    if (pd.playerEquipments.F_Shoes != null)
                    {
                        SetImage(FTeen_Shoes, (path.GetSpriteByID(pd.playerEquipments.F_Shoes.ItemID))[1], (path.GetOffsetByID(pd.playerEquipments.F_Shoes.ItemID))[1]);
                    }
                    else
                    {
                        SetDefault(69);
                    }
                    if (pd.playerEquipments.F_Chest != null)
                    {
                        if (pd.playerEquipments.F_Chest.ItemID > 7000 && pd.playerEquipments.F_Chest.ItemID < 7300)
                        {
                            FTeen_Cape.gameObject.SetActive(false);
                            FTeen_CapeFront.gameObject.SetActive(false);
                            FTeen_GloveBack.gameObject.SetActive(false);
                            FTeen_GloveFront.gameObject.SetActive(false);
                            FTeen_Pants.gameObject.SetActive(false);
                            FTeen_Shoes.gameObject.SetActive(false);
                        }
                    }
                    else
                    {

                    }
                }
                else
                {
                    OpenAdult_F();
                    FAdult_Suit.gameObject.SetActive(false);
                    if (pd.playerEquipments.F_Chest != null)
                    {

                        SetImage(FAdult_Chest, path.GetSpriteByID(pd.playerEquipments.F_Chest.ItemID)[2], path.GetOffsetByID(pd.playerEquipments.F_Chest.ItemID)[2]);
                    }
                    else
                    {
                        SetDefault(73);
                    }
                    if (pd.playerEquipments.F_FaceType != null)
                    {
                        SetImage(FAdult_Face, (path.GetSpriteByID(pd.playerEquipments.F_FaceType.ItemID))[2], (path.GetOffsetByID(pd.playerEquipments.F_FaceType.ItemID))[2]);
                    }
                    else
                    {
                        SetDefault(74);
                    }
                    if (pd.playerEquipments.F_FaceAcc != null)
                    {
                        SetImage(FAdult_FaceAcc, (path.GetSpriteByID(pd.playerEquipments.F_FaceAcc.ItemID))[2], (path.GetOffsetByID(pd.playerEquipments.F_FaceAcc.ItemID))[2]);
                    }
                    else
                    {
                        FAdult_FaceAcc.gameObject.SetActive(false);
                    }
                    if (pd.playerEquipments.F_Glasses != null)
                    {
                        SetImage(FAdult_Glasses, (path.GetSpriteByID(pd.playerEquipments.F_Glasses.ItemID))[2], (path.GetOffsetByID(pd.playerEquipments.F_Glasses.ItemID))[2]);
                    }
                    else
                    {
                        FAdult_Glasses.gameObject.SetActive(false);
                    }
                    if (pd.playerEquipments.F_Glove != null)
                    {
                        SetImage(FAdult_GloveBack, (path.GetSpriteByID(pd.playerEquipments.F_Glove.ItemID))[2], (path.GetOffsetByID(pd.playerEquipments.F_Glove.ItemID))[2]);
                        SetImage(FAdult_GloveFront, (path.GetSpriteByID(pd.playerEquipments.F_Glove.ItemID))[5], (path.GetOffsetByID(pd.playerEquipments.F_Glove.ItemID))[5]);
                    }
                    else
                    {
                        SetDefault(77);
                        SetDefault(78);
                    }
                    if (pd.playerEquipments.F_Cape != null)
                    {
                        SetImage(FAdult_Cape, (path.GetSpriteByID(pd.playerEquipments.F_Cape.ItemID))[2], (path.GetOffsetByID(pd.playerEquipments.F_Cape.ItemID))[2]);
                        SetImage(FAdult_CapeFront, (path.GetSpriteByID(pd.playerEquipments.F_Cape.ItemID))[5], (path.GetOffsetByID(pd.playerEquipments.F_Cape.ItemID))[5]);
                    }
                    else
                    {
                        FAdult_Cape.gameObject.SetActive(false);
                        FAdult_CapeFront.gameObject.SetActive(false);
                    }
                    if (pd.playerEquipments.F_Hairacc != null)
                    {
                        SetImage(FAdult_HairAcc, (path.GetSpriteByID(pd.playerEquipments.F_Hairacc.ItemID))[2], (path.GetOffsetByID(pd.playerEquipments.F_Hairacc.ItemID))[2]);
                    }
                    else
                    {
                        FAdult_HairAcc.gameObject.SetActive(false);
                    }
                    if (pd.playerEquipments.F_HairStyle != null)
                    {
                        SetImage(FAdult_HairFront, (path.GetSpriteByID(pd.playerEquipments.F_HairStyle.ItemID))[2], (path.GetOffsetByID(pd.playerEquipments.F_HairStyle.ItemID))[2]);
                        SetImage(FAdult_HairBack, (path.GetSpriteByID(pd.playerEquipments.F_HairStyle.ItemID))[5], (path.GetOffsetByID(pd.playerEquipments.F_HairStyle.ItemID))[5]);
                    }
                    else
                    {
                        SetDefault(80);
                        SetDefault(81);
                    }
                    if (pd.playerEquipments.F_Pants != null)
                    {
                        SetImage(FAdult_Pants, (path.GetSpriteByID(pd.playerEquipments.F_Pants.ItemID))[2], (path.GetOffsetByID(pd.playerEquipments.F_Pants.ItemID))[2]);
                    }
                    else
                    {
                        SetDefault(82);
                    }
                    if (pd.playerEquipments.F_Shoes != null)
                    {
                        SetImage(FAdult_Shoes, (path.GetSpriteByID(pd.playerEquipments.F_Shoes.ItemID))[2], (path.GetOffsetByID(pd.playerEquipments.F_Shoes.ItemID))[2]);
                    }
                    else
                    {
                        SetDefault(83);
                    }
                    if (pd.playerEquipments.F_Chest != null)
                    {
                        if (pd.playerEquipments.F_Chest.ItemID > 7000 && pd.playerEquipments.F_Chest.ItemID < 7300)
                        {
                            FAdult_Cape.gameObject.SetActive(false);
                            FAdult_CapeFront.gameObject.SetActive(false);
                            FAdult_GloveBack.gameObject.SetActive(false);
                            FAdult_GloveFront.gameObject.SetActive(false);
                            FAdult_Pants.gameObject.SetActive(false);
                            FAdult_Shoes.gameObject.SetActive(false);
                        }
                    }
                    
                }
            }

        }
        //狀態3 不顯示戰鬥裝 脫光
        else if (IsOutlook == true && IsPutOff == true)
        {
            if (pd.Gender == 1)
            {
                if (pd.Level < 20)
                {
                    OpenChild_M();
                    Child_Suit.gameObject.SetActive(false);
                    SetDefault(3);
                    if (pd.playerEquipments.F_FaceType != null)
                    {
                        SetImage(Child_Face, (path.GetSpriteByID(pd.playerEquipments.F_FaceType.ItemID))[0], (path.GetOffsetByID(pd.playerEquipments.F_FaceType.ItemID))[0]);
                    }
                    else
                    {
                        SetDefault(4);
                    }
                    Child_FaceAcc.gameObject.SetActive(false);
                    Child_Glasses.gameObject.SetActive(false);
                    SetDefault(7);
                    SetDefault(8);
                    Child_Cape.gameObject.SetActive(false);
                    Child_CapeFront.gameObject.SetActive(false);
                    Child_HairAcc.gameObject.SetActive(false);
                    if (pd.playerEquipments.F_HairStyle != null)
                    {
                        SetImage(Child_HairFront, (path.GetSpriteByID(pd.playerEquipments.F_HairStyle.ItemID))[0], (path.GetOffsetByID(pd.playerEquipments.F_HairStyle.ItemID))[0]);
                        SetImage(Child_HairBack, (path.GetSpriteByID(pd.playerEquipments.F_HairStyle.ItemID))[3], (path.GetOffsetByID(pd.playerEquipments.F_HairStyle.ItemID))[3]);
                    }
                    else
                    {
                        SetDefault(10);
                        SetDefault(11);
                    }
                    SetDefault(12);
                    SetDefault(13);
                }
                else if (pd.Level < 40 && pd.Level >= 20)
                {
                    OpenTeen_M();
                    Teen_Suit.gameObject.SetActive(false);
                    SetDefault(17);
                    if (pd.playerEquipments.F_FaceType != null)
                    {
                        SetImage(Teen_Face, (path.GetSpriteByID(pd.playerEquipments.F_FaceType.ItemID))[1], (path.GetOffsetByID(pd.playerEquipments.F_FaceType.ItemID))[1]);
                    }
                    else
                    {
                        SetDefault(18);
                    }
                    Teen_FaceAcc.gameObject.SetActive(false);
                    Teen_Glasses.gameObject.SetActive(false);
                    SetDefault(21);
                    SetDefault(22);
                    Teen_Cape.gameObject.SetActive(false);
                    Teen_CapeFront.gameObject.SetActive(false);
                    Teen_HairAcc.gameObject.SetActive(false);
                    if (pd.playerEquipments.F_HairStyle != null)
                    {
                        SetImage(Teen_HairFront, (path.GetSpriteByID(pd.playerEquipments.F_HairStyle.ItemID))[1], (path.GetOffsetByID(pd.playerEquipments.F_HairStyle.ItemID))[1]);
                        SetImage(Teen_HairBack, (path.GetSpriteByID(pd.playerEquipments.F_HairStyle.ItemID))[4], (path.GetOffsetByID(pd.playerEquipments.F_HairStyle.ItemID))[4]);
                    }
                    else
                    {
                        SetDefault(24);
                        SetDefault(25);
                    }
                    SetDefault(26);
                    SetDefault(27);
                }
                else
                {
                    OpenAdult_M();
                    Adult_Suit.gameObject.SetActive(false);
                    SetDefault(31);

                    if (pd.playerEquipments.F_FaceType != null)
                    {
                        SetImage(Adult_Face, (path.GetSpriteByID(pd.playerEquipments.F_FaceType.ItemID))[2], (path.GetOffsetByID(pd.playerEquipments.F_FaceType.ItemID))[2]);
                    }
                    else
                    {
                        SetDefault(32);
                    }
                    Adult_FaceAcc.gameObject.SetActive(false);
                    Adult_Glasses.gameObject.SetActive(false);
                    SetDefault(35);
                    SetDefault(36);
                    Adult_Cape.gameObject.SetActive(false);
                    Adult_CapeFront.gameObject.SetActive(false);
                    Adult_HairAcc.gameObject.SetActive(false);
                    if (pd.playerEquipments.F_HairStyle != null)
                    {
                        SetImage(Adult_HairFront, (path.GetSpriteByID(pd.playerEquipments.F_HairStyle.ItemID))[2], (path.GetOffsetByID(pd.playerEquipments.F_HairStyle.ItemID))[2]);
                        SetImage(Adult_HairBack, (path.GetSpriteByID(pd.playerEquipments.F_HairStyle.ItemID))[5], (path.GetOffsetByID(pd.playerEquipments.F_HairStyle.ItemID))[5]);
                    }
                    else
                    {
                        SetDefault(38);
                        SetDefault(39);
                    }
                    SetDefault(40);
                    SetDefault(41);
                }
            }
            else //女生
            {
                if (pd.Level < 20)
                {
                    OpenChild_F();
                    FChild_Suit.gameObject.SetActive(false);
                    SetDefault(45);
                    if (pd.playerEquipments.F_FaceType != null)
                    {
                        SetImage(FChild_Face, (path.GetSpriteByID(pd.playerEquipments.F_FaceType.ItemID))[0], (path.GetOffsetByID(pd.playerEquipments.F_FaceType.ItemID))[0]);
                    }
                    else
                    {
                        SetDefault(46);
                    }
                    FChild_FaceAcc.gameObject.SetActive(false);
                    FChild_Glasses.gameObject.SetActive(false);
                    SetDefault(49);
                    SetDefault(50);
                    FChild_Cape.gameObject.SetActive(false);
                    FChild_CapeFront.gameObject.SetActive(false);
                    FChild_HairAcc.gameObject.SetActive(false);
                    if (pd.playerEquipments.F_HairStyle != null)
                    {
                        SetImage(FChild_HairFront, (path.GetSpriteByID(pd.playerEquipments.F_HairStyle.ItemID))[0], (path.GetOffsetByID(pd.playerEquipments.F_HairStyle.ItemID))[0]);
                        SetImage(FChild_HairBack, (path.GetSpriteByID(pd.playerEquipments.F_HairStyle.ItemID))[3], (path.GetOffsetByID(pd.playerEquipments.F_HairStyle.ItemID))[3]);
                    }
                    else
                    {
                        SetDefault(52);
                        SetDefault(53);
                    }
                    SetDefault(54);
                    SetDefault(55);
                }
                else if (pd.Level < 40 && pd.Level >= 20)
                {
                    OpenTeen_F();
                    FTeen_Suit.gameObject.SetActive(false);
                    SetDefault(59);
                    if (pd.playerEquipments.F_FaceType != null)
                    {
                        SetImage(FTeen_Face, (path.GetSpriteByID(pd.playerEquipments.F_FaceType.ItemID))[1], (path.GetOffsetByID(pd.playerEquipments.F_FaceType.ItemID))[1]);
                    }
                    else
                    {
                        SetDefault(60);
                    }
                    FTeen_FaceAcc.gameObject.SetActive(false);
                    FTeen_Glasses.gameObject.SetActive(false);
                    SetDefault(63);
                    SetDefault(64);
                    FTeen_Cape.gameObject.SetActive(false);
                    FTeen_CapeFront.gameObject.SetActive(false);
                    FTeen_HairAcc.gameObject.SetActive(false);
                    if (pd.playerEquipments.F_HairStyle != null)
                    {
                        SetImage(FTeen_HairFront, (path.GetSpriteByID(pd.playerEquipments.F_HairStyle.ItemID))[1], (path.GetOffsetByID(pd.playerEquipments.F_HairStyle.ItemID))[1]);
                        SetImage(FTeen_HairBack, (path.GetSpriteByID(pd.playerEquipments.F_HairStyle.ItemID))[4], (path.GetOffsetByID(pd.playerEquipments.F_HairStyle.ItemID))[4]);
                    }
                    else
                    {
                        SetDefault(66);
                        SetDefault(67);
                    }
                    SetDefault(68);
                    SetDefault(69);
                }
                else
                {
                    OpenAdult_F();
                    FAdult_Suit.gameObject.SetActive(false);
                    SetDefault(73);
                    if (pd.playerEquipments.F_FaceType != null)
                    {
                        SetImage(FAdult_Face, (path.GetSpriteByID(pd.playerEquipments.F_FaceType.ItemID))[2], (path.GetOffsetByID(pd.playerEquipments.F_FaceType.ItemID))[2]);
                    }
                    else
                    {
                        SetDefault(74);
                    }
                    FAdult_FaceAcc.gameObject.SetActive(false);
                    FAdult_Glasses.gameObject.SetActive(false);
                    SetDefault(77);
                    SetDefault(78);
                    FAdult_Cape.gameObject.SetActive(false);
                    FAdult_CapeFront.gameObject.SetActive(false);
                    FAdult_HairAcc.gameObject.SetActive(false);
                    if (pd.playerEquipments.F_HairStyle != null)
                    {
                        SetImage(FAdult_HairFront, (path.GetSpriteByID(pd.playerEquipments.F_HairStyle.ItemID))[2], (path.GetOffsetByID(pd.playerEquipments.F_HairStyle.ItemID))[2]);
                        SetImage(FAdult_HairBack, (path.GetSpriteByID(pd.playerEquipments.F_HairStyle.ItemID))[5], (path.GetOffsetByID(pd.playerEquipments.F_HairStyle.ItemID))[5]);
                    }
                    else
                    {
                        SetDefault(80);
                        SetDefault(81);
                    }
                    SetDefault(82);
                    SetDefault(83);
                }
            }
        }
        //狀態2 顯示戰鬥裝 不顯示點裝
        else if (IsOutlook == false && IsPutOff == false)
        {
            if (pd.Gender == 1)
            {
                if (pd.Level < 20)
                {
                    OpenChild_M();
                    SetImage(Child_Suit, path.GetSpriteByID(JobToSuitID(pd.Job,pd.Gender))[0], path.GetOffsetByID(JobToSuitID(pd.Job,pd.Gender))[0]);
                    Child_Chest.gameObject.SetActive(false);
                    if (pd.playerEquipments.F_FaceType != null)
                    {
                        SetImage(Child_Face, (path.GetSpriteByID(pd.playerEquipments.F_FaceType.ItemID))[0], (path.GetOffsetByID(pd.playerEquipments.F_FaceType.ItemID))[0]);
                    }
                    else
                    {
                        SetDefault(4);
                    }
                    Child_FaceAcc.gameObject.SetActive(false);
                    Child_Glasses.gameObject.SetActive(false);
                    Child_GloveBack.gameObject.SetActive(false);
                    Child_GloveFront.gameObject.SetActive(false);
                    Child_Cape.gameObject.SetActive(false);
                    Child_CapeFront.gameObject.SetActive(false);
                    Child_HairAcc.gameObject.SetActive(false);
                    if (pd.playerEquipments.F_HairStyle != null)
                    {
                        SetImage(Child_HairFront, (path.GetSpriteByID(pd.playerEquipments.F_HairStyle.ItemID))[0], (path.GetOffsetByID(pd.playerEquipments.F_HairStyle.ItemID))[0]);
                        SetImage(Child_HairBack, (path.GetSpriteByID(pd.playerEquipments.F_HairStyle.ItemID))[3], (path.GetOffsetByID(pd.playerEquipments.F_HairStyle.ItemID))[3]);
                    }
                    else
                    {
                        SetDefault(10);
                        SetDefault(11);
                    }
                    Child_Pants.gameObject.SetActive(false);
                    Child_Shoes.gameObject.SetActive(false);
                }
                else if (pd.Level < 40 && pd.Level >= 20)
                {
                    OpenTeen_M();
                    SetImage(Teen_Suit, path.GetSpriteByID(JobToSuitID(pd.Job,pd.Gender))[1], path.GetOffsetByID(JobToSuitID(pd.Job,pd.Gender))[1]);
                    Teen_Chest.gameObject.SetActive(false);
                    if (pd.playerEquipments.F_FaceType != null)
                    {
                        SetImage(Teen_Face, (path.GetSpriteByID(pd.playerEquipments.F_FaceType.ItemID))[1], (path.GetOffsetByID(pd.playerEquipments.F_FaceType.ItemID))[1]);
                    }
                    else
                    {
                        SetDefault(18);
                    }
                    Teen_FaceAcc.gameObject.SetActive(false);
                    Teen_Glasses.gameObject.SetActive(false);
                    Teen_GloveBack.gameObject.SetActive(false);
                    Teen_GloveFront.gameObject.SetActive(false);
                    Teen_Cape.gameObject.SetActive(false);
                    Teen_CapeFront.gameObject.SetActive(false);
                    Teen_HairAcc.gameObject.SetActive(false);
                    if (pd.playerEquipments.F_HairStyle != null)
                    {
                        SetImage(Teen_HairFront, (path.GetSpriteByID(pd.playerEquipments.F_HairStyle.ItemID))[1], (path.GetOffsetByID(pd.playerEquipments.F_HairStyle.ItemID))[1]);
                        SetImage(Teen_HairBack, (path.GetSpriteByID(pd.playerEquipments.F_HairStyle.ItemID))[4], (path.GetOffsetByID(pd.playerEquipments.F_HairStyle.ItemID))[4]);
                    }
                    else
                    {
                        SetDefault(24);
                        SetDefault(25);
                    }
                    Teen_Pants.gameObject.SetActive(false);
                    Teen_Shoes.gameObject.SetActive(false);
                }
                else
                {
                    OpenAdult_M();
                    SetImage(Adult_Suit, path.GetSpriteByID(JobToSuitID(pd.Job,pd.Gender))[2], path.GetOffsetByID(JobToSuitID(pd.Job,pd.Gender))[2]);
                    Adult_Chest.gameObject.SetActive(false);
                    if (pd.playerEquipments.F_FaceType != null)
                    {
                        SetImage(Adult_Face, (path.GetSpriteByID(pd.playerEquipments.F_FaceType.ItemID))[2], (path.GetOffsetByID(pd.playerEquipments.F_FaceType.ItemID))[2]);
                    }
                    else
                    {
                        SetDefault(32);
                    }
                    Adult_FaceAcc.gameObject.SetActive(false);
                    Adult_Glasses.gameObject.SetActive(false);
                    Adult_GloveBack.gameObject.SetActive(false);
                    Adult_GloveFront.gameObject.SetActive(false);
                    Adult_Cape.gameObject.SetActive(false);
                    Adult_CapeFront.gameObject.SetActive(false);
                    Adult_HairAcc.gameObject.SetActive(false);
                    if (pd.playerEquipments.F_HairStyle != null)
                    {
                        SetImage(Adult_HairFront, (path.GetSpriteByID(pd.playerEquipments.F_HairStyle.ItemID))[2], (path.GetOffsetByID(pd.playerEquipments.F_HairStyle.ItemID))[2]);
                        SetImage(Adult_HairBack, (path.GetSpriteByID(pd.playerEquipments.F_HairStyle.ItemID))[5], (path.GetOffsetByID(pd.playerEquipments.F_HairStyle.ItemID))[5]);
                    }
                    else
                    {
                        SetDefault(38);
                        SetDefault(39);
                    }
                    Adult_Pants.gameObject.SetActive(false);
                    Adult_Shoes.gameObject.SetActive(false);
                }
            }
            else //女生
            {
                if (pd.Level < 20)
                {
                    OpenChild_F();
                    SetImage(FChild_Suit, path.GetSpriteByID(JobToSuitID(pd.Job,pd.Gender))[0], path.GetOffsetByID(JobToSuitID(pd.Job,pd.Gender))[0]);
                    FChild_Chest.gameObject.SetActive(false);
                    if (pd.playerEquipments.F_FaceType != null)
                    {
                        SetImage(FChild_Face, (path.GetSpriteByID(pd.playerEquipments.F_FaceType.ItemID))[0], (path.GetOffsetByID(pd.playerEquipments.F_FaceType.ItemID))[0]);
                    }
                    else
                    {
                        SetDefault(46);
                    }
                    FChild_FaceAcc.gameObject.SetActive(false);
                    FChild_Glasses.gameObject.SetActive(false);
                    FChild_GloveBack.gameObject.SetActive(false);
                    FChild_GloveFront.gameObject.SetActive(false);
                    FChild_Cape.gameObject.SetActive(false);
                    FChild_CapeFront.gameObject.SetActive(false);
                    FChild_HairAcc.gameObject.SetActive(false);
                    if (pd.playerEquipments.F_HairStyle != null)
                    {
                        SetImage(FChild_HairFront, (path.GetSpriteByID(pd.playerEquipments.F_HairStyle.ItemID))[0], (path.GetOffsetByID(pd.playerEquipments.F_HairStyle.ItemID))[0]);
                        SetImage(FChild_HairBack, (path.GetSpriteByID(pd.playerEquipments.F_HairStyle.ItemID))[3], (path.GetOffsetByID(pd.playerEquipments.F_HairStyle.ItemID))[3]);
                    }
                    else
                    {
                        SetDefault(52);
                        SetDefault(53);
                    }
                    FChild_Pants.gameObject.SetActive(false);
                    FChild_Shoes.gameObject.SetActive(false);
                }
                else if (pd.Level < 40 && pd.Level >= 20)
                {
                    OpenTeen_F();
                    SetImage(FTeen_Suit, path.GetSpriteByID(JobToSuitID(pd.Job, pd.Gender))[1], path.GetOffsetByID(JobToSuitID(pd.Job, pd.Gender))[1]);
                    FTeen_Chest.gameObject.SetActive(false);
                    if (pd.playerEquipments.F_FaceType != null)
                    {
                        SetImage(FTeen_Face, (path.GetSpriteByID(pd.playerEquipments.F_FaceType.ItemID))[1], (path.GetOffsetByID(pd.playerEquipments.F_FaceType.ItemID))[1]);
                    }
                    else
                    {
                        SetDefault(60);
                    }
                    FTeen_FaceAcc.gameObject.SetActive(false);
                    FTeen_Glasses.gameObject.SetActive(false);
                    FTeen_GloveBack.gameObject.SetActive(false);
                    FTeen_GloveFront.gameObject.SetActive(false);
                    FTeen_Cape.gameObject.SetActive(false);
                    FTeen_CapeFront.gameObject.SetActive(false);
                    FTeen_HairAcc.gameObject.SetActive(false);
                    if (pd.playerEquipments.F_HairStyle != null)
                    {
                        SetImage(FTeen_HairFront, (path.GetSpriteByID(pd.playerEquipments.F_HairStyle.ItemID))[1], (path.GetOffsetByID(pd.playerEquipments.F_HairStyle.ItemID))[1]);
                        SetImage(FTeen_HairBack, (path.GetSpriteByID(pd.playerEquipments.F_HairStyle.ItemID))[4], (path.GetOffsetByID(pd.playerEquipments.F_HairStyle.ItemID))[4]);
                    }
                    else
                    {
                        SetDefault(66);
                        SetDefault(67);
                    }
                    FTeen_Pants.gameObject.SetActive(false);
                    FTeen_Shoes.gameObject.SetActive(false);
                }
                else
                {
                    OpenAdult_F();
                    SetImage(FAdult_Suit, path.GetSpriteByID(JobToSuitID(pd.Job, pd.Gender))[2], path.GetOffsetByID(JobToSuitID(pd.Job, pd.Gender))[2]);
                    FAdult_Chest.gameObject.SetActive(false);
                    if (pd.playerEquipments.F_FaceType != null)
                    {
                        SetImage(FAdult_Face, (path.GetSpriteByID(pd.playerEquipments.F_FaceType.ItemID))[2], (path.GetOffsetByID(pd.playerEquipments.F_FaceType.ItemID))[2]);
                    }
                    else
                    {
                        SetDefault(74);
                    }
                    FAdult_FaceAcc.gameObject.SetActive(false);
                    FAdult_Glasses.gameObject.SetActive(false);
                    FAdult_GloveBack.gameObject.SetActive(false);
                    FAdult_GloveFront.gameObject.SetActive(false);
                    FAdult_Cape.gameObject.SetActive(false);
                    FAdult_CapeFront.gameObject.SetActive(false);
                    FAdult_HairAcc.gameObject.SetActive(false);
                    if (pd.playerEquipments.F_HairStyle != null)
                    {
                        SetImage(FAdult_HairFront, (path.GetSpriteByID(pd.playerEquipments.F_HairStyle.ItemID))[2], (path.GetOffsetByID(pd.playerEquipments.F_HairStyle.ItemID))[2]);
                        SetImage(FAdult_HairBack, (path.GetSpriteByID(pd.playerEquipments.F_HairStyle.ItemID))[5], (path.GetOffsetByID(pd.playerEquipments.F_HairStyle.ItemID))[5]);
                    }
                    else
                    {
                        SetDefault(80);
                        SetDefault(81);
                    }
                    FAdult_Pants.gameObject.SetActive(false);
                    FAdult_Shoes.gameObject.SetActive(false);
                }
            }
            return;
        }
        //狀態1 不顯示點裝 不顯示戰鬥裝 脫光
        else
        {
            if (pd.Gender == 1)
            {
                if (pd.Level < 20)
                {
                    OpenChild_M();
                    Child_Suit.gameObject.SetActive(false);
                    SetDefault(3);
                    if (pd.playerEquipments.F_FaceType != null)
                    {
                        SetImage(Child_Face, (path.GetSpriteByID(pd.playerEquipments.F_FaceType.ItemID))[0], (path.GetOffsetByID(pd.playerEquipments.F_FaceType.ItemID))[0]);
                    }
                    else
                    {
                        SetDefault(4);
                    }
                    Child_FaceAcc.gameObject.SetActive(false);
                    Child_Glasses.gameObject.SetActive(false);
                    SetDefault(7);
                    SetDefault(8);
                    Child_Cape.gameObject.SetActive(false);
                    Child_CapeFront.gameObject.SetActive(false);
                    Child_HairAcc.gameObject.SetActive(false);
                    if (pd.playerEquipments.F_HairStyle != null)
                    {
                        SetImage(Child_HairFront, (path.GetSpriteByID(pd.playerEquipments.F_HairStyle.ItemID))[0], (path.GetOffsetByID(pd.playerEquipments.F_HairStyle.ItemID))[0]);
                        SetImage(Child_HairBack, (path.GetSpriteByID(pd.playerEquipments.F_HairStyle.ItemID))[3], (path.GetOffsetByID(pd.playerEquipments.F_HairStyle.ItemID))[3]);
                    }
                    else
                    {
                        SetDefault(10);
                        SetDefault(11);
                    }
                    SetDefault(12);
                    SetDefault(13);
                }
                else if (pd.Level < 40 && pd.Level >= 20)
                {
                    OpenTeen_M();
                    Teen_Suit.gameObject.SetActive(false);
                    SetDefault(17);
                    if (pd.playerEquipments.F_FaceType != null)
                    {
                        SetImage(Teen_Face, (path.GetSpriteByID(pd.playerEquipments.F_FaceType.ItemID))[1], (path.GetOffsetByID(pd.playerEquipments.F_FaceType.ItemID))[1]);
                    }
                    else
                    {
                        SetDefault(18);
                    }
                    Teen_FaceAcc.gameObject.SetActive(false);
                    Teen_Glasses.gameObject.SetActive(false);
                    SetDefault(21);
                    SetDefault(22);
                    Teen_Cape.gameObject.SetActive(false);
                    Teen_CapeFront.gameObject.SetActive(false);
                    Teen_HairAcc.gameObject.SetActive(false);
                    if (pd.playerEquipments.F_HairStyle != null)
                    {
                        SetImage(Teen_HairFront, (path.GetSpriteByID(pd.playerEquipments.F_HairStyle.ItemID))[1], (path.GetOffsetByID(pd.playerEquipments.F_HairStyle.ItemID))[1]);
                        SetImage(Teen_HairBack, (path.GetSpriteByID(pd.playerEquipments.F_HairStyle.ItemID))[4], (path.GetOffsetByID(pd.playerEquipments.F_HairStyle.ItemID))[4]);
                    }
                    else
                    {
                        SetDefault(24);
                        SetDefault(25);
                    }
                    SetDefault(26);
                    SetDefault(27);
                }
                else
                {
                    OpenAdult_M();
                    Adult_Suit.gameObject.SetActive(false);
                    SetDefault(31);

                    if (pd.playerEquipments.F_FaceType != null)
                    {
                        SetImage(Adult_Face, (path.GetSpriteByID(pd.playerEquipments.F_FaceType.ItemID))[2], (path.GetOffsetByID(pd.playerEquipments.F_FaceType.ItemID))[2]);
                    }
                    else
                    {
                        SetDefault(32);
                    }
                    Adult_FaceAcc.gameObject.SetActive(false);
                    Adult_Glasses.gameObject.SetActive(false);
                    SetDefault(35);
                    SetDefault(36);
                    Adult_Cape.gameObject.SetActive(false);
                    Adult_CapeFront.gameObject.SetActive(false);
                    Adult_HairAcc.gameObject.SetActive(false);
                    if (pd.playerEquipments.F_HairStyle != null)
                    {
                        SetImage(Adult_HairFront, (path.GetSpriteByID(pd.playerEquipments.F_HairStyle.ItemID))[2], (path.GetOffsetByID(pd.playerEquipments.F_HairStyle.ItemID))[2]);
                        SetImage(Adult_HairBack, (path.GetSpriteByID(pd.playerEquipments.F_HairStyle.ItemID))[5], (path.GetOffsetByID(pd.playerEquipments.F_HairStyle.ItemID))[5]);
                    }
                    else
                    {
                        SetDefault(38);
                        SetDefault(39);
                    }
                    SetDefault(40);
                    SetDefault(41);
                }
            }
            else //女生
            {
                if (pd.Level < 20)
                {
                    OpenChild_F();
                    FChild_Suit.gameObject.SetActive(false);
                    SetDefault(45);
                    if (pd.playerEquipments.F_FaceType != null)
                    {
                        SetImage(FChild_Face, (path.GetSpriteByID(pd.playerEquipments.F_FaceType.ItemID))[0], (path.GetOffsetByID(pd.playerEquipments.F_FaceType.ItemID))[0]);
                    }
                    else
                    {
                        SetDefault(46);
                    }
                    FChild_FaceAcc.gameObject.SetActive(false);
                    FChild_Glasses.gameObject.SetActive(false);
                    SetDefault(49);
                    SetDefault(50);
                    FChild_Cape.gameObject.SetActive(false);
                    FChild_CapeFront.gameObject.SetActive(false);
                    FChild_HairAcc.gameObject.SetActive(false);
                    if (pd.playerEquipments.F_HairStyle != null)
                    {
                        SetImage(FChild_HairFront, (path.GetSpriteByID(pd.playerEquipments.F_HairStyle.ItemID))[0], (path.GetOffsetByID(pd.playerEquipments.F_HairStyle.ItemID))[0]);
                        SetImage(FChild_HairBack, (path.GetSpriteByID(pd.playerEquipments.F_HairStyle.ItemID))[3], (path.GetOffsetByID(pd.playerEquipments.F_HairStyle.ItemID))[3]);
                    }
                    else
                    {
                        SetDefault(52);
                        SetDefault(53);
                    }
                    SetDefault(54);
                    SetDefault(55);
                }
                else if (pd.Level < 40 && pd.Level >= 20)
                {
                    OpenTeen_F();
                    FTeen_Suit.gameObject.SetActive(false);
                    SetDefault(59);
                    if (pd.playerEquipments.F_FaceType != null)
                    {
                        SetImage(FTeen_Face, (path.GetSpriteByID(pd.playerEquipments.F_FaceType.ItemID))[1], (path.GetOffsetByID(pd.playerEquipments.F_FaceType.ItemID))[1]);
                    }
                    else
                    {
                        SetDefault(60);
                    }
                    FTeen_FaceAcc.gameObject.SetActive(false);
                    FTeen_Glasses.gameObject.SetActive(false);
                    SetDefault(63);
                    SetDefault(64);
                    FTeen_Cape.gameObject.SetActive(false);
                    FTeen_CapeFront.gameObject.SetActive(false);
                    FTeen_HairAcc.gameObject.SetActive(false);
                    if (pd.playerEquipments.F_HairStyle != null)
                    {
                        SetImage(FTeen_HairFront, (path.GetSpriteByID(pd.playerEquipments.F_HairStyle.ItemID))[1], (path.GetOffsetByID(pd.playerEquipments.F_HairStyle.ItemID))[1]);
                        SetImage(FTeen_HairBack, (path.GetSpriteByID(pd.playerEquipments.F_HairStyle.ItemID))[4], (path.GetOffsetByID(pd.playerEquipments.F_HairStyle.ItemID))[4]);
                    }
                    else
                    {
                        SetDefault(66);
                        SetDefault(67);
                    }
                    SetDefault(68);
                    SetDefault(69);
                }
                else
                {
                    OpenAdult_F();
                    FAdult_Suit.gameObject.SetActive(false);
                    SetDefault(73);
                    if (pd.playerEquipments.F_FaceType != null)
                    {
                        SetImage(FAdult_Face, (path.GetSpriteByID(pd.playerEquipments.F_FaceType.ItemID))[2], (path.GetOffsetByID(pd.playerEquipments.F_FaceType.ItemID))[2]);
                    }
                    else
                    {
                        SetDefault(74);
                    }
                    FAdult_FaceAcc.gameObject.SetActive(false);
                    FAdult_Glasses.gameObject.SetActive(false);
                    SetDefault(77);
                    SetDefault(78);
                    FAdult_Cape.gameObject.SetActive(false);
                    FAdult_CapeFront.gameObject.SetActive(false);
                    FAdult_HairAcc.gameObject.SetActive(false);
                    if (pd.playerEquipments.F_HairStyle != null)
                    {
                        SetImage(FAdult_HairFront, (path.GetSpriteByID(pd.playerEquipments.F_HairStyle.ItemID))[2], (path.GetOffsetByID(pd.playerEquipments.F_HairStyle.ItemID))[2]);
                        SetImage(FAdult_HairBack, (path.GetSpriteByID(pd.playerEquipments.F_HairStyle.ItemID))[5], (path.GetOffsetByID(pd.playerEquipments.F_HairStyle.ItemID))[5]);
                    }
                    else
                    {
                        SetDefault(80);
                        SetDefault(81);
                    }
                    SetDefault(82);
                    SetDefault(83);
                }
            }
        }

        bool IsSuit = false;
        if (pd.playerEquipments.F_Chest != null)
        {
            if (pd.playerEquipments.F_Chest.ItemID > 7300 && pd.playerEquipments.F_Chest.ItemID < 8000)
            { IsSuit = true; }
        }
        
        //如果有穿點裝套裝(不包含職業套裝)，關閉suit,pants,套裝視為上衣
        if (IsSuit)
        {
            Adult_Suit.gameObject.SetActive(false);
            Child_Suit.gameObject.SetActive(false);
            FAdult_Suit.gameObject.SetActive(false);
            FChild_Suit.gameObject.SetActive(false);
            FTeen_Suit.gameObject.SetActive(false);
            Teen_Suit.gameObject.SetActive(false);
            Adult_Pants.gameObject.SetActive(false);
            Child_Pants.gameObject.SetActive(false);
            Teen_Pants.gameObject.SetActive(false);
            FAdult_Pants.gameObject.SetActive(false);
            FChild_Pants.gameObject.SetActive(false);
            FTeen_Pants.gameObject.SetActive(false);
        }
    }

    public void SetImage(Image img, Sprite sprite, Vector4 rect)
    {
        ///換圖片
        img.sprite = null;
        img.sprite = sprite;
        ///SetNativeSize
        img.SetNativeSize();
        ///Set rect
        img.transform.localScale = new Vector3(rect.z, rect.w, 1);
        img.transform.localPosition = new Vector3(rect.x, rect.y, 1);
    }
    public void OpenChild_M()
    {
        Child_Suit.gameObject.SetActive(true);
        Child_Cape.gameObject.SetActive(true);
        Child_CapeFront.gameObject.SetActive(true);
        Child_Chest.gameObject.SetActive(true);
        Child_Face.gameObject.SetActive(true);
        Child_FaceAcc.gameObject.SetActive(true);
        Child_Glasses.gameObject.SetActive(true);
        Child_GloveFront.gameObject.SetActive(true);
        Child_GloveBack.gameObject.SetActive(true);
        Child_HairAcc.gameObject.SetActive(true);
        Child_HairFront.gameObject.SetActive(true);
        Child_HairBack.gameObject.SetActive(true);
        Child_Pants.gameObject.SetActive(true);
        Child_Shoes.gameObject.SetActive(true);

    }
    public void OpenTeen_M()
    {
        Teen_Suit.gameObject.SetActive(true);
        Teen_Cape.gameObject.SetActive(true);
        Teen_CapeFront.gameObject.SetActive(true);
        Teen_Chest.gameObject.SetActive(true);
        Teen_Face.gameObject.SetActive(true);
        Teen_FaceAcc.gameObject.SetActive(true);
        Teen_Glasses.gameObject.SetActive(true);
        Teen_GloveFront.gameObject.SetActive(true);
        Teen_GloveBack.gameObject.SetActive(true);
        Teen_HairAcc.gameObject.SetActive(true);
        Teen_HairFront.gameObject.SetActive(true);
        Teen_HairBack.gameObject.SetActive(true);
        Teen_Pants.gameObject.SetActive(true);
        Teen_Shoes.gameObject.SetActive(true);
    }
    public void OpenAdult_M()
    {
        Adult_Suit.gameObject.SetActive(true);
        Adult_Cape.gameObject.SetActive(true);
        Adult_CapeFront.gameObject.SetActive(true);
        Adult_Chest.gameObject.SetActive(true);
        Adult_Face.gameObject.SetActive(true);
        Adult_FaceAcc.gameObject.SetActive(true);
        Adult_Glasses.gameObject.SetActive(true);
        Adult_GloveFront.gameObject.SetActive(true);
        Adult_GloveBack.gameObject.SetActive(true);
        Adult_HairAcc.gameObject.SetActive(true);
        Adult_HairFront.gameObject.SetActive(true);
        Adult_HairBack.gameObject.SetActive(true);
        Adult_Pants.gameObject.SetActive(true);
        Adult_Shoes.gameObject.SetActive(true);
    }
    public void OpenChild_F()
    {
        FChild_Suit.gameObject.SetActive(true);
        FChild_Cape.gameObject.SetActive(true);
        FChild_CapeFront.gameObject.SetActive(true);
        FChild_Chest.gameObject.SetActive(true);
        FChild_Face.gameObject.SetActive(true);
        FChild_FaceAcc.gameObject.SetActive(true);
        FChild_Glasses.gameObject.SetActive(true);
        FChild_GloveFront.gameObject.SetActive(true);
        FChild_GloveBack.gameObject.SetActive(true);
        FChild_HairAcc.gameObject.SetActive(true);
        FChild_HairFront.gameObject.SetActive(true);
        FChild_HairBack.gameObject.SetActive(true);
        FChild_Pants.gameObject.SetActive(true);
        FChild_Shoes.gameObject.SetActive(true);
    }
    public void OpenTeen_F()
    {
        FTeen_Suit.gameObject.SetActive(true);
        FTeen_Cape.gameObject.SetActive(true);
        FTeen_CapeFront.gameObject.SetActive(true);
        FTeen_Chest.gameObject.SetActive(true);
        FTeen_Face.gameObject.SetActive(true);
        FTeen_FaceAcc.gameObject.SetActive(true);
        FTeen_Glasses.gameObject.SetActive(true);
        FTeen_GloveFront.gameObject.SetActive(true);
        FTeen_GloveBack.gameObject.SetActive(true);
        FTeen_HairAcc.gameObject.SetActive(true);
        FTeen_HairFront.gameObject.SetActive(true);
        FTeen_HairBack.gameObject.SetActive(true);
        FTeen_Pants.gameObject.SetActive(true);
        FTeen_Shoes.gameObject.SetActive(true);
    }
    public void OpenAdult_F()
    {
        FAdult_Suit.gameObject.SetActive(true);
        FAdult_Cape.gameObject.SetActive(true);
        FAdult_CapeFront.gameObject.SetActive(true);
        FAdult_Chest.gameObject.SetActive(true);
        FAdult_Face.gameObject.SetActive(true);
        FAdult_FaceAcc.gameObject.SetActive(true);
        FAdult_Glasses.gameObject.SetActive(true);
        FAdult_GloveFront.gameObject.SetActive(true);
        FAdult_GloveBack.gameObject.SetActive(true);
        FAdult_HairAcc.gameObject.SetActive(true);
        FAdult_HairFront.gameObject.SetActive(true);
        FAdult_HairBack.gameObject.SetActive(true);
        FAdult_Pants.gameObject.SetActive(true);
        FAdult_Shoes.gameObject.SetActive(true);
    }
    public void SetDefault(int num)
    {
        Images[num].sprite = null;
        Images[num].sprite = DefaultSprites[num];
        Images[num].SetNativeSize();
        Images[num].transform.localPosition = DefaultPosition[num];
        Images[num].transform.localScale = DefaultScale[num];
    }
    public int JobToSuitID(int Job,int Gender)
    {
        if (Gender == 1)
        {
            switch (Job)
            {
                case 1:
                    return 7001;
                case 2:
                    return 7003;
                case 3:
                    return 7005;
                case 4:
                    return 7007;
            }
        }
        else if (Gender == 0)
        {
            switch (Job)
            {
                case 1:
                    return 7002;
                case 2:
                    return 7004;
                case 3:
                    return 7006;
                case 4:
                    return 7008;
            }
        }
        return 0;
    }
}
