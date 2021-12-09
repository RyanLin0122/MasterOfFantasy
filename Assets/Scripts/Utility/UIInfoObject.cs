using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class UIInfoObject : MonoBehaviour, IPointerClickHandler
{
    public Text IDText;
    public Text NameText;
    public Text RegionText;
    public string IDStr;
    public string NameStr;
    public string RegionStr;
    public string[] DragData;
    public DragInfoSource DragInfoSource;
    public GameObject SelectedImg;

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        SetInfo();
    }
    public void SetInfo()
    {
        foreach (var obj in transform.parent.GetComponentsInChildren<UIInfoObject>())
        {
            obj.SelectedImg.SetActive(false);
        }
        GetComponent<UIInfoObject>().SelectedImg.SetActive(true);
        DiaryInformationWnd diaryInformationWnd = transform.parent.parent.GetComponent<DiaryInformationWnd>();
        diaryInformationWnd.SetInfo(Convert.ToInt32(GetComponent<UIInfoObject>().DragData[0]));
    }
    public void SetText(bool IsMonster, int ID)
    {
        if (!IsMonster)
        {
            IDStr = ID.ToString();
            NameStr = ResSvc.Instance.GetNpcCfgData(ID).Name;
            RegionStr = Constants.GetRegionName(ID);
            DragData = new string[] { IDStr, NameStr, RegionStr };
            IDText.text = IDStr;
            NameText.text = NameStr;
            RegionText.text = RegionStr;
        }
        else
        {
            IDStr = ID.ToString();
            NameStr = ResSvc.Instance.MonsterInfoDic[ID].Name;
            RegionStr = Constants.GetRegionName(ID);
            DragData = new string[] { IDStr, NameStr, RegionStr };
            IDText.text = IDStr;
            NameText.text = NameStr;
            RegionText.text = RegionStr;
        }
    }

    
}
