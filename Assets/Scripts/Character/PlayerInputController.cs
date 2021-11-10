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
    public int speed;

    public void Init(NEntity entity, EntityController entityController)
    {
        state = PlayerStatus.Idle;
        this.character = new Character(entity);
        this.entityController = (PlayerController)entityController;
        if (entity.EntityName == GameRoot.Instance.ActivePlayer.Name)
        {
            this.rb = this.entityController.transform.GetComponent<Rigidbody2D>();
            entityController.transform.GetComponent<CapsuleCollider2D>().enabled = true;
            SkillSys.Instance.InitPlayerSkills(GameRoot.Instance.ActivePlayer, this.entityController);
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
                entityController.IsMoving = true;
                state = PlayerStatus.Move;
                entityController.InstantiateDust();
                this.character.Move();
                this.SendEntityEvent(EntityEvent.Move);
                this.rb.velocity = (BattleSys.Instance.FinalAttribute.RunSpeed) * GetRunDirection();
                if (this.rb.velocity.x > 0)
                {
                    entityController.SetFaceDirection(true);
                }
                else if (this.rb.velocity.x < 0)
                {
                    entityController.SetFaceDirection(false);
                }
                return;
            }
        }
        else
        {
            if (!entityController.screenCtrl.canCtrl)
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
            if (!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
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
            if (this.rb.velocity.x > 0)
            {
                entityController.SetFaceDirection(true);
            }
            else if (this.rb.velocity.x < 0)
            {
                entityController.SetFaceDirection(false);
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
        Vector3 offset = this.rb.transform.position - lastPos;
        //this.speed = (int)(offset.magnitude * 100f / Time.deltaTime);
        //Vector3Int goLogicPos = GameObjectTool.WorldToLogic(this.rb.transform.position);
        //float logicOffset = (goLogicPos - this.character.position).magnitude;
        if (offset.magnitude > 0 && Time.realtimeSinceStartup - lastSync > 0.07f)
        {
            this.lastPos = this.rb.transform.position;
            //this.character.SetPosition(this.rb.transform.localPosition);
            this.character.SetDirection(GetRunDirection());
            if (this.rb.transform.localScale.x > 0)
            {
                this.character.SetFaceDirection(true);
            }
            else
            {
                this.character.SetFaceDirection(false);
            }    
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
        this.character.SetPosition(this.rb.transform.localPosition);
        //print(character.entityData.Position.X + ", "+ character.entityData.Position.Y);
        new MoveSender(entityEvent, character.entityData);
    }

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
        if (hotKeySlot != null)
        {
            var dict = Instance.entityController.SkillDict;
            if (hotKeySlot.State == HotKeyState.Skill)
            {
                if (dict != null)
                {
                    dict[hotKeySlot.data.ID].Cast();
                }
            }
        }
    }
}

