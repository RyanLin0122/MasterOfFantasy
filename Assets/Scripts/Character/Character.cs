using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;

public class Character : Entity
{
    public Character(NEntity entity)
    {
        this.entityData = entity;
    }

    public bool IsPlayer
    {
        get
        {
            if (this.entityData == null) return false;
            return this.entityData.Type == EntityType.Player;
        }
    }

    public bool IsCurrentPlayer
    {
        get
        {
            if (!IsPlayer) return false;
            return this.entityName == GameRoot.Instance.ActivePlayer.Name;
        }
    }

    public void Move()
    {
        Debug.LogFormat("Move");
        this.speed = (int)BattleSys.Instance.FinalAttribute.RunSpeed;
    }

    public void Stop()
    {
        Debug.LogFormat("Stop");
        this.speed = 0;
    }
    public void SetFaceDirection(bool Dir)
    {
        this.entityData.FaceDirection = Dir;
    }
    public void SetDirection(Vector3 direction)
    {
        this.entityData.Direction = new NVector3(direction.x, direction.y, 0);
    }

    public void SetPosition(Vector3 position)
    {
        this.entityData.Position = new NVector3(position.x, position.y, 200);
    }
}
