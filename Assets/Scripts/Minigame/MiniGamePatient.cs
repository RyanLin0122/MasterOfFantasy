using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using System;

public class MiniGamePatient : MonoBehaviour
{
    CureGameManager manager;
    public Sprite[] FeverSprites = new Sprite[7];
    public Sprite[] CrazySprites = new Sprite[7];
    public Sprite[] PoisoningSprites = new Sprite[7];
    public Sprite[] InjuredSprites = new Sprite[7];
    public Sprite DeathSprite;
    public Sprite RehabilitationSprite;
    public Sprite VacancySprite;

    public GameObject ChoosedPatientEffect;
    public Image PatientImg;
    public Image DialogueImg;
    public Image SyringeImg;
    public Image BandageImg;
    public Image AntidoteImg;
    public Image CrossImg;

    public Sprite[] SyringeSprites = new Sprite[2];
    public Sprite[] BandageSprites = new Sprite[2];
    public Sprite[] AntidoteSprites = new Sprite[2];
    public Sprite[] CrossSprites = new Sprite[2];
    public Sprite[] DialogueSprites = new Sprite[4];

    public PatientSituation patientSituation = PatientSituation.Vacancy;
    public PatientSymptom patientSymptom = PatientSymptom.none;
    public PatientOperation patientOperation = PatientOperation.none;

    public float TotalResponseTime;
    public float RestResponseTime;
    public bool IsStartAni = false;

    public float AniSpeed = 2;
    public float AnimTimeInterval = 0;
    public int FrameIndex = 0;
    public float AniTimer = 0f;
    public Sprite[] CurrentAniSprites = null;
    public int GameIndex = 0;
    public int ChooseLevel = 0; //0未選中 1選中病人 2打開藥箱 3治療 

    #region GameFunction
    public void InitPatient(float ResponseTime, CureGameManager cureGameManager)
    {
        TotalResponseTime = ResponseTime;
        RestResponseTime = ResponseTime;
        manager = cureGameManager;
    }
    public void StartPatient()
    {        
        Random.InitState(Guid.NewGuid().GetHashCode());
        int randomnum = Random.Range(0, 4);
        patientSituation = PatientSituation.Waiting;
        switch (randomnum)
        {
            case 0:
                patientOperation = PatientOperation.none;
                patientSymptom = PatientSymptom.Fever;
                PlayFeverAni();
                break;
            case 1:
                patientOperation = PatientOperation.none;
                patientSymptom = PatientSymptom.Injured;
                PlayInjuredAni();
                break;
            case 2:
                patientOperation = PatientOperation.none;
                patientSymptom = PatientSymptom.Crazy;
                PlayCrazyAni();
                break;
            case 3:
                patientOperation = PatientOperation.none;
                patientSymptom = PatientSymptom.Poisoning;
                PlayPoisoningAni();
                break;
            default:
                break;
        }
        StartCoroutine(ForceEndTimer());
        if (manager.Patients[manager.CurrentRowIndex, manager.CurrentColumnIndex] == this)
        {
            ChoosePatient();
        }
    }

    public void ChoosePatient()
    {
        switch (ChooseLevel)
        {
            case 0:
                ChoosePatientEffect();
                if (patientSituation == PatientSituation.Waiting || patientSituation == PatientSituation.Vacancy)
                {
                    ChooseLevel = 1;
                }
                else
                {
                    return;
                }
                break;
            case 1:
                if( patientSituation == PatientSituation.Waiting)
                {
                    OpenCureBox();
                    ChooseLevel = 2;
                }
                else
                {
                    return;
                }
                break;
            default:
                break;
        }
    }
    public void CancelChoosePatient()
    {
        ChooseLevel = 0;
        CancelChooseEffect();
        patientOperation = PatientOperation.none;
    }
    IEnumerator ForceEndTimer()
    {
        int currentIndex = GameIndex;
        yield return new WaitForSeconds(TotalResponseTime);
        if (currentIndex == GameIndex && patientOperation==PatientOperation.none)
        {
            Cure();
        }
    }
    public void Cure()
    {
        ChooseLevel = 3;
        CloseCureBox();
        IsStartAni = false;
        Accessment();
    }
    public void ResetPatient()
    {
        patientSymptom = PatientSymptom.none;
        patientSituation = PatientSituation.Vacancy;
        PatientImg.sprite = VacancySprite;
        DialogueImg.gameObject.SetActive(false);
        AniTimer = 0;
        FrameIndex = 0;
        IsStartAni = false;
        CurrentAniSprites = null;
        ChooseLevel = 0;
    }
    public void ChoosePatientEffect()
    {
        ChoosedPatientEffect.SetActive(true);
    }
    public void CancelChooseEffect()
    {
        ChoosedPatientEffect.SetActive(false);
    }
    public void OpenCureBox()
    {
        if (ChooseLevel == 1)
        {
            AudioSvc.Instance.PlayUIAudio_ForMiniGame(Constants.OpenCureBox);
            SyringeImg.gameObject.SetActive(true);
            BandageImg.gameObject.SetActive(true);
            AntidoteImg.gameObject.SetActive(true);
            CrossImg.gameObject.SetActive(true);
            SyringeImg.sprite = SyringeSprites[0];
            BandageImg.sprite = BandageSprites[0];
            AntidoteImg.sprite = AntidoteSprites[0];
            CrossImg.sprite = CrossSprites[0];
            StartCoroutine(MoveTimer(1)); StartCoroutine(MoveTimer(2)); StartCoroutine(MoveTimer(3)); StartCoroutine(MoveTimer(4)); StartCoroutine(MoveTimer(5));
        }        
    }
    public  IEnumerator MoveTimer(int index)
    {
        yield return new WaitForSeconds(0.06f * index);
        MoveOutCureBox();
    }
    public IEnumerator MoveInTimer(int index)
    {
        yield return new WaitForSeconds(0.06f * index);
        MoveInCureBox();
    }
    public void CloseCureBox()
    {
        SyringeImg.sprite = SyringeSprites[0];
        BandageImg.sprite = BandageSprites[0];
        AntidoteImg.sprite = AntidoteSprites[0];
        CrossImg.sprite = CrossSprites[0];
        StartCoroutine(MoveInTimer(1)); StartCoroutine(MoveInTimer(2)); StartCoroutine(MoveInTimer(3)); StartCoroutine(MoveInTimer(4)); StartCoroutine(MoveInTimer(5));
        StartCoroutine(CloseTimer());
       
    }
    IEnumerator CloseTimer()
    {
        yield return new WaitForSeconds(0.3f);
        SyringeImg.gameObject.SetActive(false);
        BandageImg.gameObject.SetActive(false);
        AntidoteImg.gameObject.SetActive(false);
        CrossImg.gameObject.SetActive(false);
        SyringeImg.transform.localPosition = Vector3.zero;
        BandageImg.transform.localPosition = Vector3.zero;
        AntidoteImg.transform.localPosition = Vector3.zero;
        CrossImg.transform.localPosition = Vector3.zero;
    }
    private void MoveOutCureBox()
    {
        SyringeImg.transform.localPosition = new Vector3(SyringeImg.transform.localPosition.x, SyringeImg.transform.localPosition.y+11f, 0f);
        BandageImg.transform.localPosition = new Vector3(BandageImg.transform.localPosition.x, BandageImg.transform.localPosition.y-11f, 0f);
        AntidoteImg.transform.localPosition = new Vector3(AntidoteImg.transform.localPosition.x-11f, AntidoteImg.transform.localPosition.y, 0f);
        CrossImg.transform.localPosition = new Vector3(CrossImg.transform.localPosition.x+11f, CrossImg.transform.localPosition.y, 0f);
    }
    private void MoveInCureBox()
    {
        SyringeImg.transform.localPosition = new Vector3(SyringeImg.transform.localPosition.x, SyringeImg.transform.localPosition.y - 11f, 0f);
        BandageImg.transform.localPosition = new Vector3(BandageImg.transform.localPosition.x, BandageImg.transform.localPosition.y + 11f, 0f);
        AntidoteImg.transform.localPosition = new Vector3(AntidoteImg.transform.localPosition.x + 11f, AntidoteImg.transform.localPosition.y, 0f);
        CrossImg.transform.localPosition = new Vector3(CrossImg.transform.localPosition.x - 11f, CrossImg.transform.localPosition.y, 0f);
    }
    #endregion

    private void Update()
    {
        if (IsStartAni && (patientSituation == PatientSituation.Waiting))
        {
            if (CurrentAniSprites == null)
            {
                return;
            }
            AniTimer += Time.deltaTime;
            if (AniTimer > AnimTimeInterval)
            {
                FrameIndex++;
                AniTimer -= AnimTimeInterval;
                if (FrameIndex < CurrentAniSprites.Length)
                {
                    PatientImg.sprite = CurrentAniSprites[FrameIndex];
                }
                FrameIndex %= CurrentAniSprites.Length + 1;
                if (FrameIndex >= CurrentAniSprites.Length)
                {
                    ResetAni();
                }
            }
        }
    }

    public void ResetAni()
    {
        AniTimer = 0;
        FrameIndex = 0;
        PatientImg.sprite = CurrentAniSprites[FrameIndex];
    }

    public void Accessment()
    {
        switch (patientSymptom)
        {
            case PatientSymptom.Fever:
                if( patientOperation == PatientOperation.Syringe)
                {
                    PlusScore();
                    PlayRehabilitationAni();
                }
                else
                {
                    Death();
                }
                break;
            case PatientSymptom.Crazy:
                if (patientOperation == PatientOperation.Cross)
                {
                    PlusScore();
                    PlayRehabilitationAni();
                }
                else
                {
                    Death();
                }
                break;
            case PatientSymptom.Poisoning:
                if (patientOperation == PatientOperation.Antidote)
                {
                    PlusScore();
                    PlayRehabilitationAni();
                }
                else
                {
                    Death();
                }
                break;
            case PatientSymptom.Injured:
                if (patientOperation == PatientOperation.Bandage)
                {
                    PlusScore();
                    PlayRehabilitationAni();
                }
                else
                {
                    Death();
                }
                break;
            case PatientSymptom.none:
                break;
            default:
                break;
        }
    }
    public void PlayFeverAni()
    {
        if (patientSituation == PatientSituation.Waiting)
        {
            AnimTimeInterval = 1f / AniSpeed;
            CurrentAniSprites = FeverSprites;
            DialogueImg.gameObject.SetActive(true);
            DialogueImg.sprite = DialogueSprites[0];
            ResetAni();
            IsStartAni = true;
        }       
    }
    public void PlayPoisoningAni()
    {
        if (patientSituation == PatientSituation.Waiting)
        {
            AnimTimeInterval = 1f / AniSpeed;
            CurrentAniSprites = PoisoningSprites;
            DialogueImg.gameObject.SetActive(true);
            DialogueImg.sprite = DialogueSprites[1];
            ResetAni();
            IsStartAni = true;
        }
    }
    public void PlayInjuredAni()
    {
        if (patientSituation == PatientSituation.Waiting)
        {
            AnimTimeInterval = 1f / AniSpeed;
            CurrentAniSprites = InjuredSprites;
            DialogueImg.gameObject.SetActive(true);
            DialogueImg.sprite = DialogueSprites[2];
            ResetAni();
            IsStartAni = true;
        }
    }
    public void PlayCrazyAni()
    {
        if (patientSituation == PatientSituation.Waiting)
        {
            AnimTimeInterval = 1f / AniSpeed;
            CurrentAniSprites = CrazySprites;
            DialogueImg.gameObject.SetActive(true);
            DialogueImg.sprite = DialogueSprites[3];
            ResetAni();
            IsStartAni = true;
        }
    }
    public void PlusScore()
    {
        AudioSvc.Instance.PlayMiniGameUIAudio(Constants.Recovery);
        ShowCureEffect();
        ShowThanksEffect();
        manager.PlusScore();
    }
    public void MinusScore()
    {
        AudioSvc.Instance.PlayMiniGameUIAudio(Constants.PatientDie);
        manager.MinusScore();
    }
    public void Death()
    {
        MinusScore();
        patientSituation = PatientSituation.Death;
        SyringeImg.sprite = SyringeSprites[0];
        BandageImg.sprite = BandageSprites[0];
        AntidoteImg.sprite = AntidoteSprites[0];
        CrossImg.sprite = CrossSprites[0];
        SyringeImg.gameObject.SetActive(false);
        BandageImg.gameObject.SetActive(false);
        AntidoteImg.gameObject.SetActive(false);
        CrossImg.gameObject.SetActive(false);
        SyringeImg.transform.localPosition = Vector3.zero;
        BandageImg.transform.localPosition = Vector3.zero;
        AntidoteImg.transform.localPosition = Vector3.zero;
        CrossImg.transform.localPosition = Vector3.zero;
        PlayDeathAni();
    }
    public void PlayDeathAni()
    {
        PatientImg.sprite = DeathSprite;
        StartCoroutine(ResetTimer());
    }

    public void GameOver()
    {
        SyringeImg.sprite = SyringeSprites[0];
        BandageImg.sprite = BandageSprites[0];
        AntidoteImg.sprite = AntidoteSprites[0];
        CrossImg.sprite = CrossSprites[0];
        SyringeImg.gameObject.SetActive(false);
        BandageImg.gameObject.SetActive(false);
        AntidoteImg.gameObject.SetActive(false);
        CrossImg.gameObject.SetActive(false);
        SyringeImg.transform.localPosition = Vector3.zero;
        BandageImg.transform.localPosition = Vector3.zero;
        AntidoteImg.transform.localPosition = Vector3.zero;
        CrossImg.transform.localPosition = Vector3.zero;
        PatientImg.sprite = VacancySprite;
        patientOperation = PatientOperation.none;
        patientSituation = PatientSituation.Vacancy;
        ChooseLevel = 0;
        IsStartAni = false;
        AniTimer = 0;
        FrameIndex = 0;
        GameIndex++;
    }
    public IEnumerator ResetTimer()
    {
        GameIndex++;
        yield return new WaitForSeconds(0.6f);
        ResetPatient();
    }
    public void PlayRehabilitationAni()
    {
        patientSituation = PatientSituation.Rehabilitation;
        PatientImg.sprite = RehabilitationSprite;
        StartCoroutine(ResetTimer());
    }
    public void ShowThanksEffect()
    {
        GameObject Thanks = Instantiate(Resources.Load<GameObject>("Prefabs/ThanksEffect"), new Vector3(0f, 0f, 0f), Quaternion.identity, this.transform);
        Thanks.transform.localPosition = new Vector3(0f, 35f, 0f);
    }
    public void ShowCureEffect()
    {
        GameObject cure = Instantiate(Resources.Load<GameObject>("Prefabs/CureEffect"), new Vector3(0f, 0f, 0f), Quaternion.identity, this.transform);
        cure.transform.localPosition = new Vector3(0f, 0f, 0f);
    }
    public void ChooseSyringe()
    {
        patientOperation = PatientOperation.Syringe;
        SyringeImg.sprite = SyringeSprites[1];
        BandageImg.sprite = BandageSprites[0];
        AntidoteImg.sprite = AntidoteSprites[0];
        CrossImg.sprite = CrossSprites[0];
    }
    public void ChooseBandage()
    {
        patientOperation = PatientOperation.Bandage;
        BandageImg.sprite = BandageSprites[1];
        SyringeImg.sprite = SyringeSprites[0];
        AntidoteImg.sprite = AntidoteSprites[0];
        CrossImg.sprite = CrossSprites[0];
    }
    public void ChooseAntidote()
    {
        patientOperation = PatientOperation.Antidote;
        AntidoteImg.sprite = AntidoteSprites[1];
        SyringeImg.sprite = SyringeSprites[0];
        BandageImg.sprite = BandageSprites[0];
        CrossImg.sprite = CrossSprites[0];
    }
    public void ChooseCross()
    {
        patientOperation = PatientOperation.Cross;
        CrossImg.sprite = CrossSprites[1];
        SyringeImg.sprite = SyringeSprites[0];
        BandageImg.sprite = BandageSprites[0];
        AntidoteImg.sprite = AntidoteSprites[0];
    }
    
}
public enum PatientOperation { none, Syringe, Bandage, Antidote, Cross}
public enum PatientSituation { Vacancy, Waiting, Rehabilitation, Death}
public enum PatientSymptom { Fever, Crazy, Poisoning, Injured, none}
