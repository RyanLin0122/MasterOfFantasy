using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;
using UnityEngine.UI;

public class DropItemEntity : MonoBehaviour
{
    public bool HasInit = false;
    public DropItem DropItem;
    float flyTime = 0;
    float duration = 0;
    public Vector2 TargetPos;
    public bool Stopped = false;
    public int RotationNum = 1;
    public void Init(DropItem dropItem)
    {
        this.DropItem = dropItem;
        TargetPos = new Vector2(dropItem.From.X + dropItem.FlyTo[1] * Mathf.Cos(dropItem.FlyTo[0]), dropItem.From.Y + dropItem.FlyTo[1] * Mathf.Sin(dropItem.FlyTo[0]));
        float distance = GetDistance(new Vector2(dropItem.From.X, dropItem.From.Y), TargetPos);
        duration = distance / 120;
        HasInit = true;
        if (dropItem.Type == DropItemType.Item) GetComponent<Image>().sprite = Resources.Load<Sprite>(dropItem.Item.Sprite);
        else GetComponent<Image>().sprite = Resources.Load<Sprite>("Items/Other/Money");
        GetComponent<Image>().SetNativeSize();
        GetComponent<Image>().enabled = true;
    }
    public void Setup(DropItem dropItem)
    {
        this.DropItem = dropItem;
        HasInit = true;
        if (dropItem.Type == DropItemType.Item) GetComponent<Image>().sprite = Resources.Load<Sprite>(dropItem.Item.Sprite);
        else GetComponent<Image>().sprite = Resources.Load<Sprite>("Items/Other/Money");
        GetComponent<Image>().SetNativeSize();
        GetComponent<Image>().enabled = true;
        TargetPos = new Vector2(dropItem.From.X + dropItem.FlyTo[1] * Mathf.Cos(dropItem.FlyTo[0]), dropItem.From.Y + dropItem.FlyTo[1] * Mathf.Sin(dropItem.FlyTo[0]));
        Vector2 Distance = TargetPos - (Vector2)transform.localPosition;
        transform.localPosition = TargetPos;
    }
    public bool OnUpdate(float delta)
    {
        if (!HasInit) return true;
        if (!Stopped)
        {
            if (flyTime < duration)
            {
                float OriginalZ = transform.localPosition.z;
                float RestTime = duration - flyTime;
                Vector2 Distance = TargetPos - (Vector2)transform.localPosition;
                transform.localPosition = (Vector2)transform.localPosition + Distance * (Time.deltaTime / RestTime);
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, OriginalZ);
                transform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, RotationNum * 360 * flyTime / duration);
            }
            flyTime += Time.deltaTime;
            if (this.flyTime > duration)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                this.Stopped = true;
            }
        }
        else
        {
            return DropItem.Update(delta);
        }
        return true;
    }

    public float GetDistance(Vector3 From, Vector3 To)
    {
        return Mathf.Sqrt(Mathf.Pow(From.x - To.x, 2) + Mathf.Pow(From.y - To.y, 2));
    }
}
