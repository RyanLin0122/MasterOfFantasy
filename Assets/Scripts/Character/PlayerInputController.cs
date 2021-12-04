using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;

public class PlayerInputController : MonoSingleton<PlayerInputController>
{
    public Rigidbody2D rb;
    public PlayerController entityController;
    public PlayerStatus state = PlayerStatus.Idle;
    public Character character;
    public bool IsLockMove = false;
    public void Init(NEntity entity, EntityController entityController)
    {
        state = PlayerStatus.Idle;
        this.character = new Character(entity);
        this.entityController = (PlayerController)entityController;
        if (entity.EntityName == GameRoot.Instance.ActivePlayer.Name)
        {
            this.rb = this.entityController.transform.GetComponent<Rigidbody2D>();
            entityController.transform.GetComponent<CapsuleCollider2D>().enabled = true;
            entityController.entity = new Entity();
            entityController.entity.Type = EntityType.Player;
            entityController.entity.nEntity = entity;
            SkillSys.Instance.InitPlayerSkills(GameRoot.Instance.ActivePlayer, this.entityController);
            BattleSys.Instance.InitAllAtribute();
        }
        if (entityController != null) entityController.entity = this.character;
    }

    void FixedUpdate()
    {
        if (character == null) return;
        if (this.rb == null) return;
        if (state == PlayerStatus.Idle && entityController.screenCtrl.canCtrl)
        {
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
            {
                if (!IsLockMove)
                {
                    entityController.IsMoving = true;
                    state = PlayerStatus.Move;
                    entityController.InstantiateDust();
                    this.character.Move();
                    this.rb.velocity = (BattleSys.Instance.FinalAttribute.RunSpeed) * GetRunDirection();
                    if (this.rb.velocity.x != 0)
                    {
                        entityController.SetFaceDirection(this.rb.velocity.x > 0);
                    }
                    this.SendEntityEvent(EntityEvent.Move);
                }
                return;
            }
        }
        else
        {
            if (!entityController.screenCtrl.canCtrl || IsLockMove)
            {
                entityController.IsMoving = false;
                if (state != PlayerStatus.Idle)
                {
                    state = PlayerStatus.Idle;
                    this.rb.velocity = Vector3.zero;
                    this.character.Stop();
                    this.SendEntityEvent(EntityEvent.Idle);
                }
                return;
            }
            if (!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow) || IsLockMove)
            {
                entityController.IsMoving = false;
                if (state != PlayerStatus.Idle)
                {
                    state = PlayerStatus.Idle;
                    this.rb.velocity = Vector3.zero;
                    this.character.Stop();
                    this.SendEntityEvent(EntityEvent.Idle);
                }
                return;
            }
            this.rb.velocity = (BattleSys.Instance.FinalAttribute.RunSpeed) * GetRunDirection();
            if (this.rb.velocity.x != 0)
            {
                entityController.SetFaceDirection(this.rb.velocity.x > 0);
            }
        }
    }
    public Vector3 GetRunDirection()
    {
        Vector3 dir = Vector3.zero;
        if (Input.GetKey(KeyCode.UpArrow)) dir = new Vector3(dir.x, dir.y + 1, 0);
        if (Input.GetKey(KeyCode.DownArrow)) dir = new Vector3(dir.x, dir.y - 1, 0);
        if (Input.GetKey(KeyCode.LeftArrow)) dir = new Vector3(dir.x - 1, dir.y, 0);
        if (Input.GetKey(KeyCode.RightArrow)) dir = new Vector3(dir.x + 1, dir.y, 0);
        return Vector3.Normalize(dir);
    }
    Vector3 lastPos;
    float lastSync = 0;
    private void LateUpdate()
    {
        if (this.character == null) return;
        if (this.rb == null) return;
        if (entityController.RaceEffect == null && entityController.rb.velocity != Vector2.zero)
        {
            if (entityController.buffManager.IsBuffValid(1)) entityController.RaceEffect = entityController.InstantiateRaceEffect();
        }
        Vector3 offset = this.rb.transform.position - lastPos;
        //this.speed = (int)(offset.magnitude * 100f / Time.deltaTime);
        //Vector3Int goLogicPos = GameObjectTool.WorldToLogic(this.rb.transform.position);
        //float logicOffset = (goLogicPos - this.character.position).magnitude;
        if (offset.magnitude > 0 && Time.realtimeSinceStartup - lastSync > 0.07f)
        {
            this.lastPos = this.rb.transform.position;
            //this.character.SetPosition(this.rb.transform.localPosition);
            this.character.SetDirection(GetRunDirection());
            this.character.SetFaceDirection(this.rb.transform.localScale.x > 0);
            this.SendEntityEvent(EntityEvent.None);
        }
    }

    public void SendEntityEvent(EntityEvent entityEvent, int param = 0)
    {
        lastSync = Time.realtimeSinceStartup;
        if (entityController != null)
        {
            entityController.OnEntityEvent(entityEvent);
        }
        this.character.SetFaceDirection(this.rb.transform.localScale.x > 0);
        this.character.SetPosition(this.rb.transform.localPosition);
        this.character.SetSpeed(BattleSys.Instance.FinalAttribute.RunSpeed);
        new MoveSender(entityEvent, character.nEntity);
    }
    private bool PickUpLock = false;
    public void Update()
    {
        HotKeySlot hotKeySlot = null;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            BattleSys.Instance.HotKeyManager.HotKeySlots.TryGetValue(KeyCode.Alpha1, out hotKeySlot);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            BattleSys.Instance.HotKeyManager.HotKeySlots.TryGetValue(KeyCode.Alpha2, out hotKeySlot);
        }

        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            BattleSys.Instance.HotKeyManager.HotKeySlots.TryGetValue(KeyCode.Alpha3, out hotKeySlot);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            BattleSys.Instance.HotKeyManager.HotKeySlots.TryGetValue(KeyCode.Alpha4, out hotKeySlot);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            BattleSys.Instance.HotKeyManager.HotKeySlots.TryGetValue(KeyCode.Alpha5, out hotKeySlot);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            BattleSys.Instance.HotKeyManager.HotKeySlots.TryGetValue(KeyCode.Alpha6, out hotKeySlot);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            BattleSys.Instance.HotKeyManager.HotKeySlots.TryGetValue(KeyCode.Alpha7, out hotKeySlot);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            BattleSys.Instance.HotKeyManager.HotKeySlots.TryGetValue(KeyCode.Alpha8, out hotKeySlot);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            BattleSys.Instance.HotKeyManager.HotKeySlots.TryGetValue(KeyCode.Alpha9, out hotKeySlot);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            BattleSys.Instance.HotKeyManager.HotKeySlots.TryGetValue(KeyCode.Alpha0, out hotKeySlot);
        }
        else if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
        {
            if (BattleSys.Instance.MapNPCs != null && BattleSys.Instance.MapNPCs.Count > 0)
            {
                GeneralNPC npc = null;
                foreach (var item in BattleSys.Instance.MapNPCs)
                {
                    if ((item.transform.localPosition - entityController.transform.localPosition).magnitude < 200)
                    {
                        npc = item;
                    }
                }
                if (npc != null)
                {
                    npc.LoadDialogue();
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            CommonAttack();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            new RunSender();
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            if (!PickUpLock)
            {
                BattleSys.Instance.PickUpRequest();
                PickUpLock = true;
                TimerSvc.Instance.AddTimeTask((L) => { PickUpLock = false; }, 0.15, PETimeUnit.Second, 1);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            UISystem.Instance.OpenCloseQuestWnd();
        }
        if (hotKeySlot != null)
        {
            var dict = Instance.entityController.SkillDict;
            if (hotKeySlot.State == HotKeyState.Skill)
            {
                if (dict != null)
                {
                    if (!PlayerInputController.Instance.IsLockMove)
                    {
                        dict[hotKeySlot.data.ID].Cast();
                    }
                }
            }
            if (hotKeySlot.State == HotKeyState.Consumable)
            {
                bool IsCash = InventorySys.Instance.ItemList[hotKeySlot.data.ID].IsCash;
                if (IsCash)
                {
                    var ck = GameRoot.Instance.ActivePlayer.CashKnapsack;
                    if (ck == null || ck.Count < 1) return;
                    foreach (var kv in ck)
                    {
                        if (kv.Value.ItemID == hotKeySlot.data.ID)
                        {
                            KnapsackWnd.Instance.FindCashSlot(kv.Key).UseItem();
                            return;
                        }
                    }
                }
                else
                {
                    var nk = GameRoot.Instance.ActivePlayer.NotCashKnapsack;
                    if (nk == null || nk.Count < 1) return;
                    foreach (var kv in nk)
                    {
                        if (kv.Value.ItemID == hotKeySlot.data.ID)
                        {
                            KnapsackWnd.Instance.FindSlot(kv.Key).UseItem();
                            return;
                        }
                    }
                }
            }
        }
    }
    public void CommonAttack()
    {
        if (GameRoot.Instance.ActivePlayer.playerEquipments == null)
        {
            return;
        }
        if (entityController == null) return;
        if (entityController.SkillDict == null) return;
        if (GameRoot.Instance.ActivePlayer.playerEquipments.B_Weapon != null)
        {
            Weapon weapon = GameRoot.Instance.ActivePlayer.playerEquipments.B_Weapon;
            Skill CommonAtackSkill = null;
            switch (weapon.WeapType)
            {
                case WeaponType.None:
                    break;
                case WeaponType.Axe:
                    entityController.SkillDict.TryGetValue(-1, out CommonAtackSkill);
                    break;
                case WeaponType.Bow:
                    entityController.SkillDict.TryGetValue(-2, out CommonAtackSkill);
                    break;
                case WeaponType.Dagger:
                    entityController.SkillDict.TryGetValue(-3, out CommonAtackSkill);
                    break;
                case WeaponType.Gun:
                    entityController.SkillDict.TryGetValue(-4, out CommonAtackSkill);
                    break;
                case WeaponType.Hammer:
                    entityController.SkillDict.TryGetValue(-5, out CommonAtackSkill);
                    break;
                case WeaponType.LongSword:
                    entityController.SkillDict.TryGetValue(-6, out CommonAtackSkill);
                    break;
                case WeaponType.Spear:
                    entityController.SkillDict.TryGetValue(-7, out CommonAtackSkill);
                    break;
                case WeaponType.Staff:
                    entityController.SkillDict.TryGetValue(-8, out CommonAtackSkill);
                    break;
                case WeaponType.Sword:
                    entityController.SkillDict.TryGetValue(-9, out CommonAtackSkill);
                    break;
                case WeaponType.Book:
                    entityController.SkillDict.TryGetValue(-10, out CommonAtackSkill);
                    break;
                case WeaponType.Cross:
                    entityController.SkillDict.TryGetValue(-11, out CommonAtackSkill);
                    break;
                case WeaponType.Crossbow:
                    entityController.SkillDict.TryGetValue(-12, out CommonAtackSkill);
                    break;
                case WeaponType.DualSword:
                    entityController.SkillDict.TryGetValue(-13, out CommonAtackSkill);
                    break;
                default:
                    break;
            }
            if (CommonAtackSkill != null)
            {
                CommonAtackSkill.Cast();
            }
        }
    }
    public void LockMove()
    {
        this.IsLockMove = true;
    }

    public void UnlockMove()
    {
        this.IsLockMove = false;
    }
}

