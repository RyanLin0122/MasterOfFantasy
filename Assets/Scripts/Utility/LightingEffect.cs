using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingEffect : MonoBehaviour
{
    private void Awake()
    {
        TimerSvc.Instance.AddTimeTask((a) => { InstantiateLightingFront(new Vector3(50, -40, 0), 2); }, 0.05f, PETimeUnit.Second, 1);
        TimerSvc.Instance.AddTimeTask((a) => { InstantiateLightingFront(new Vector3(-30, -10, 0), 0); }, 0.07f, PETimeUnit.Second, 1);
        TimerSvc.Instance.AddTimeTask((a) => { InstantiateLightingBack(new Vector3(0, 30, 0), 0); }, 0.12f, PETimeUnit.Second, 1);
        TimerSvc.Instance.AddTimeTask((a) => { InstantiateLightingFront(new Vector3(-50, -30, 0), 1); }, 0.13f, PETimeUnit.Second, 1);
        TimerSvc.Instance.AddTimeTask((a) => { InstantiateLightingBack(new Vector3(20, 30, 0), 1); }, 0.2f, PETimeUnit.Second, 1);
        TimerSvc.Instance.AddTimeTask((a) => { InstantiateLightingFront(new Vector3(-50, 10, 0), 1); }, 0.3f, PETimeUnit.Second, 1);
        TimerSvc.Instance.AddTimeTask((a) => { InstantiateLightingFront(new Vector3(50, -30, 0), 1); }, 0.4f, PETimeUnit.Second, 1);

        TimerSvc.Instance.AddTimeTask((a) => { InstantiateLightingFront(new Vector3(60, 40, 0), 2); }, 0.5f, PETimeUnit.Second, 1);
        TimerSvc.Instance.AddTimeTask((a) => { InstantiateLightingFront(new Vector3(-80, -30, 0), 2); }, 0.6f, PETimeUnit.Second, 1);
        TimerSvc.Instance.AddTimeTask((a) => { InstantiateLightingFront(new Vector3(20, 10, 0)); }, 0.7f, PETimeUnit.Second, 1);
        TimerSvc.Instance.AddTimeTask((a) => { InstantiateLightingFront(new Vector3(60, 40, 0), 2); }, 0.51f, PETimeUnit.Second, 1);
        TimerSvc.Instance.AddTimeTask((a) => { InstantiateLightingFront(new Vector3(80, -30, 0), 1); }, 0.53f, PETimeUnit.Second, 1);
        TimerSvc.Instance.AddTimeTask((a) => { InstantiateLightingBack(new Vector3(-40, 20, 0), 0); }, 0.7f, PETimeUnit.Second, 1);
        TimerSvc.Instance.AddTimeTask((a) => { InstantiateLightingBack(new Vector3(0, 15, 0), 1); }, 0.75f, PETimeUnit.Second, 1);
        TimerSvc.Instance.AddTimeTask((a) => { InstantiateLightingFront(new Vector3(-70, 60, 0), 2); }, 0.8f, PETimeUnit.Second, 1);
        TimerSvc.Instance.AddTimeTask((a) => { InstantiateLightingFront(new Vector3(10, -30, 0), 0); }, 0.9f, PETimeUnit.Second, 1);
        TimerSvc.Instance.AddTimeTask((a) => { InstantiateLightingBack(new Vector3(25, 25, 0), 0); }, 0.95f, PETimeUnit.Second, 1);
        TimerSvc.Instance.AddTimeTask((a) => { InstantiateLightingFront(new Vector3(80, 20, 0), 2); }, 0.97f, PETimeUnit.Second, 1);
        TimerSvc.Instance.AddTimeTask((a) => { InstantiateLightingFront(new Vector3(-45, 0, 0), 1); }, 1f, PETimeUnit.Second, 1);
        TimerSvc.Instance.AddTimeTask((a) => { InstantiateLightingFront(new Vector3(40, -30, 0), 1); }, 1.1f, PETimeUnit.Second, 1);
        TimerSvc.Instance.AddTimeTask((a) => { InstantiateLightingFront(new Vector3(-40, 20, 0), 0); }, 1.2f, PETimeUnit.Second, 1);

    }
    //0 ºñ¦â 1ÂÅ¦â 2¬õ¦â
    public Transform InstantiateLighting(Vector3 pos, int Color = 0)
    {
        Transform go = null;
        GameObject container = Instantiate(Resources.Load("Prefabs/NumberContainer") as GameObject);
        container.transform.SetParent(BattleSys.Instance.MapCanvas.transform);
        container.transform.localScale = new Vector3(1, 1, 1);
        container.transform.localPosition = transform.parent.localPosition + pos;
        switch (Color)
        {
            case 0:
                go = ((GameObject)Instantiate(Resources.Load("Prefabs/SkillPrefabs/LightingG"))).GetComponent<Transform>();
                break;
            case 1:
                go = ((GameObject)Instantiate(Resources.Load("Prefabs/SkillPrefabs/LightingB"))).GetComponent<Transform>();
                break;
            case 2:
                go = ((GameObject)Instantiate(Resources.Load("Prefabs/SkillPrefabs/LightingR"))).GetComponent<Transform>();
                break;
            default:
                go = ((GameObject)Instantiate(Resources.Load("Prefabs/SkillPrefabs/LightingG"))).GetComponent<Transform>();
                break;
        }
        go.SetParent(container.transform);
        go.localPosition = Vector3.zero;
        return go;
    }
    public void InstantiateLightingBack(Vector3 pos, int Color = 0)
    {
        Transform lighting = InstantiateLighting(pos, Color);
        SpriteRenderer renderer = lighting.GetComponent<SpriteRenderer>();
        renderer.sortingLayerName = "Player";
    }
    public void InstantiateLightingFront(Vector3 pos, int Color = 0)
    {
        Transform lighting = InstantiateLighting(pos, Color);
        SpriteRenderer renderer = lighting.GetComponent<SpriteRenderer>();
        renderer.sortingLayerName = "Effect";
    }
}
