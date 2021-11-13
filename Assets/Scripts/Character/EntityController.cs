using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;

public class EntityController : MonoBehaviour
{
    public string Name;
    public Rigidbody2D rb;
    public Transform Shadow;
    public bool IsMoving = false;
    public Entity entity;
    public bool IsAIControlling = false;
    public Dictionary<int, Skill> SkillDict;
    public Player playerdata;
    public TrimedPlayer trimedPlayerdata;
    public void Init()
    {
        rb = this.GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }
    public virtual void SetFaceDirection(bool FaceDir)
    {
        //�l����{
    }
    public virtual void OnEntityEvent(EntityEvent entityEvent)
    {
        //�l����{
    }
    public virtual void DoDamage(DamageInfo damage)
    {
        //�l����{
        int NumberCount = damage.Damage.Length;
        if (NumberCount > 0)
        {
            for (int i = 0; i < NumberCount; i++)
            {
                if (i > 0) TimerSvc.Instance.AddTimeTask((t) => { GenerateDamageNum(damage.Damage[i], 2); }, 0.3f * i, PETimeUnit.Second);
                else GenerateDamageNum(damage.Damage[i], 2);
            }
        }
    }
    #region Generate Number
    public GameObject DamageContainer;
    public void GenerateDamageNum(int damage, int mode)
    {
        //DamageContainer.transform.localScale = new Vector3(Mathf.Abs(DamageContainer.transform.localScale.x), DamageContainer.transform.localScale.y, DamageContainer.transform.localScale.z);
        GameObject container = Instantiate(Resources.Load("Prefabs/NumberContainer") as GameObject);
        container.transform.SetParent(BattleSys.Instance.MapCanvas.transform);
        container.transform.localScale = new Vector3(100, 100, 1f);
        container.transform.localPosition = transform.localPosition;
        
        GameObject obj = Instantiate(Resources.Load("Prefabs/Damage") as GameObject);
        RectTransform rectTransform = obj.GetComponent<RectTransform>();
        rectTransform.transform.SetParent(container.transform);
        obj.GetComponent<DamageController>().SetNumber(damage, mode);
    }
    #endregion
}