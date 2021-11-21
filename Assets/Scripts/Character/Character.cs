using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;

public class Character : Entity
{
    public Character(NEntity entity)
    {
        this.nentity = entity;
    }

    public bool IsPlayer
    {
        get
        {
            if (this.nentity == null) return false;
            return this.nentity.Type == EntityType.Player;
        }
    }

    public bool IsCurrentPlayer
    {
        get
        {
            if (!IsPlayer) return false;
            return this.nentity.EntityName == GameRoot.Instance.ActivePlayer.Name;
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
        this.nentity.FaceDirection = Dir;
    }
    public void SetDirection(Vector3 direction)
    {
        this.nentity.Direction = new NVector3(direction.x, direction.y, 0);
    }

    public void SetPosition(Vector3 position)
    {
        this.nentity.Position = new NVector3(position.x, position.y, 200);
    }

    public void SetSpeed(float Speed)
    {
        this.nentity.Speed = speed;
    }
}
