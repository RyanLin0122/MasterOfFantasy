using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageQueue : MonoBehaviour
{
    int MaxMsgNum = 0;
    float TextHeight = 0;
    public GameObject TextPrefab = null;
    private void Start()
    {
        TextPrefab = LoadMessageTextPrefab();
        TextHeight = TextPrefab.GetComponent<RectTransform>().rect.height;
        MaxMsgNum = Mathf.CeilToInt(GetComponent<RectTransform>().rect.height / TextHeight);

    }

    private GameObject LoadMessageTextPrefab()
    {
        return Resources.Load("Prefabs/MessageQueueText") as GameObject;
    }

    public void AddMessage(string s)
    {
        Transform t = Instantiate(TextPrefab).transform;
        Transform[] ts = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            ts[i] = transform.GetChild(i);
            ts[i].localPosition = new Vector2(ts[i].localPosition.x, ts[i].localPosition.y + TextHeight);
        }
        t.GetComponent<Text>().text = s;
        t.SetParent(transform);
        t.SetAsLastSibling();
        t.localScale = Vector3.one;
        Rect rect = GetComponent<RectTransform>().rect;
        t.localPosition = new Vector2(-rect.width / 2, -rect.height / 2);
        if (transform.childCount + 1 > MaxMsgNum)
        {
            Destroy(transform.GetChild(0).gameObject);
        }
    }
}
