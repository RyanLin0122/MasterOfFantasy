using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;

public class Character : Entity
{
    public Character(NEntity entity)
    {
        this.nEntity = entity;
    }

    public bool IsPlayer
    {
        get
        {
            if (this.nEntity == null) return false;
            return this.nEntity.Type == EntityType.Player;
        }
    }

    public bool IsCurrentPlayer
    {
        get
        {
            if (!IsPlayer) return false;
            return this.nEntity.EntityName == GameRoot.Instance.ActivePlayer.Name;
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
        this.nEntity.FaceDirection = Dir;
    }
    public void SetDirection(Vector3 direction)
    {
        this.nEntity.Direction = new NVector3(direction.x, direction.y, 0);
    }

    public void SetPosition(Vector3 position)
    {
        this.nEntity.Position = new NVector3(position.x, position.y, 200);
    }

    public void SetSpeed(float Speed)
    {
        this.nEntity.Speed = speed;
    }
}
