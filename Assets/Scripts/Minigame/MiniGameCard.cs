using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class MiniGameCard : MonoBehaviour
{
    public CardGameManager GameManager;
    public MiniGameCardState CardState = MiniGameCardState.Default;
    public MiniGameCardPattern CurrentPattern = MiniGameCardPattern.Dragon;
    public Sprite[] CurrentSprites = new Sprite[5];
    public Sprite[] TeacherSprites = new Sprite[5];
    public Sprite[] DragonSprites = new Sprite[5];
    public Sprite[] PrincipalSprites = new Sprite[5];

    public void InitCard(CardGameManager manager)
    {
        this.GameManager = manager;
        CurrentSprites = DragonSprites;
    }

    public void ResetCard()
    {
        CardState = MiniGameCardState.Default;
        Random.InitState(Guid.NewGuid().GetHashCode());
        int rd = 0;
        if (GameManager.Difficalty == 2) //Hard
        {
            rd = Random.Range(0, 3);
        }
        else
        {
            rd = Random.Range(0, 2);
        }
        switch (rd)
        {
            case 0:
                CurrentPattern = MiniGameCardPattern.Dragon;
                CurrentSprites = DragonSprites;
                break;
            case 1:
                CurrentPattern = MiniGameCardPattern.Teacher;
                CurrentSprites = TeacherSprites;
                break;
            case 2:
                CurrentPattern = MiniGameCardPattern.Principal;
                CurrentSprites = PrincipalSprites;
                break;
            default:
                break;
        }
    }
    public void Accessment(bool IsPress)
    {
        CardState = MiniGameCardState.Accessment;
        if (IsPress)
        {
            if(CurrentPattern == MiniGameCardPattern.Teacher)
            {
                MinusScore(2);
            }
            else if(CurrentPattern == MiniGameCardPattern.Principal)
            {
                MinusScore(3);
            }
            else if(CurrentPattern == MiniGameCardPattern.Dragon)
            {
                PlusScore(1);
            }
        }
    }

    public void PlusScore(int score)
    {
        GameManager.PlusScore(score);
        ShowHit();
        ShowOK();
        AudioSvc.Instance.PlayUIAudio_ForMiniGame(Constants.CardHit);
    }
    public void MinusScore(int score)
    {
        GameManager.MinusScore(score);
        ShowMiss();
    }

    #region Animation
    public void ShowCard()
    {
        StartCoroutine(ChangeCardSprite(CurrentSprites[1], 1));
        StartCoroutine(ChangeCardSprite(CurrentSprites[2], 2));
        StartCoroutine(ChangeCardSprite(CurrentSprites[3], 3));
        StartCoroutine(ChangeCardSprite(CurrentSprites[4], 4));
    }
    public void HindCard()
    {
        StartCoroutine(ChangeCardSprite(CurrentSprites[3], 1));
        StartCoroutine(ChangeCardSprite(CurrentSprites[2], 2));
        StartCoroutine(ChangeCardSprite(CurrentSprites[1], 3));
        StartCoroutine(ChangeCardSprite(CurrentSprites[0], 4));
    }
    public void ShowOK()
    {
        GameObject container = Instantiate(Resources.Load<GameObject>("Prefabs/EffectContainer"), new Vector3(0f, 0f, 0f), Quaternion.identity, this.transform);
        container.transform.localPosition = new Vector3(30f, 70f, 0f);
        GameObject ok = Instantiate(Resources.Load<GameObject>("Prefabs/OKEffect"), new Vector3(0f, 0f, 0f), Quaternion.identity, container.transform);
        ok.transform.localPosition = Vector3.zero;
    }
    public void ShowMiss()
    {
        GameObject container = Instantiate(Resources.Load<GameObject>("Prefabs/EffectContainer"), new Vector3(0f, 0f, 0f), Quaternion.identity, this.transform);
        container.transform.localPosition = new Vector3(30f, 70f, 0f);
        GameObject miss = Instantiate(Resources.Load<GameObject>("Prefabs/MissEffect"), new Vector3(0f, 0f, 0f), Quaternion.identity, container.transform);
        miss.transform.localPosition = Vector3.zero;
    }
    public void ShowHit()
    {
        GameObject cure = Instantiate(Resources.Load<GameObject>("Prefabs/CureEffect"), new Vector3(0f, 0f, 0f), Quaternion.identity, this.transform);
        cure.transform.localPosition = new Vector3(0f, 0f, 0f);
    }
    #endregion

    IEnumerator ChangeCardSprite(Sprite sprite, int index) 
    {
        yield return new WaitForSeconds(0.08f*index);
        Image img = this.GetComponent<Image>();
        img.sprite = null;
        img.sprite = sprite;
        img.SetNativeSize();
    }
}
public enum MiniGameCardState
{
    Default, Response, Accessment
}
public enum MiniGameCardPattern
{
    Dragon, Teacher, Principal, None
}

