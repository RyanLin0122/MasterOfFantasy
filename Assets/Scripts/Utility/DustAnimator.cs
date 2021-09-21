using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustAnimator : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public float TimeEnd = 3f;
    public int AnimLength = 8;
    public float TimeIntervel = 0f;
    public int FrameIndex = 0;
    public Vector3 StartScale = new Vector3(5f, 5f, 1);
    public Vector3 EndScale = new Vector3(100f, 100f, 1);
    public float DebugTime = 0f;
    public void Initialize(Sprite sprite)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        TimeIntervel = 1f / AnimLength;
        spriteRenderer.sprite = sprite;
        UpdateAnime(FrameIndex);
    }

    public Vector3 GetScale(int frame)
    {
        if (frame == 0) return StartScale;
        float ScaleX = StartScale.x + (EndScale.x - StartScale.x) * (((float)frame) / (AnimLength - 1));
        float ScaleY = StartScale.y + (EndScale.y - StartScale.y) * (((float)frame) / (AnimLength - 1));
        return new Vector3(ScaleX, ScaleY, 1);
    }
    public Quaternion GetRotation(int frame)
    {
        if (frame == 0) return Quaternion.Euler(Vector3.zero);
        return Quaternion.Euler(new Vector3(0, 0, 30 * frame));
    }
    public float GetAlpha(int frame)
    {
        float Threshold = 0.6f;
        if (frame <= (Threshold * AnimLength)) return 1;
        else
        {
            float temp = (1 - Threshold);
            float alpha = (-1f / (temp * AnimLength)) * frame + (AnimLength / (AnimLength * temp)); ;            
            return Mathf.Clamp01(alpha);
        }
    }
    public void UpdateAnime(int frame)
    {
        DebugTime = Time.realtimeSinceStartup;
        if (frame == AnimLength) { Destroy(gameObject); return; }
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, GetAlpha(frame));
        transform.localScale = GetScale(frame);
        transform.localRotation = GetRotation(frame);
        FrameIndex++;
        TimerSvc.Instance.AddTimeTask((a) => { UpdateAnime(FrameIndex); }, TimeIntervel, PETimeUnit.Second);
    }
}
