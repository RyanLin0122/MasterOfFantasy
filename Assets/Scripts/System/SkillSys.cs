using UnityEngine;
using PEProtocal;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

class SkillSys : MonoSingleton<SkillSys>
{
    public void InitSys()
    {
        Debug.Log("Init SkillSys...");
    }
    public SkillWnd skillWnd;

    public void OpenSkillWnd()
    {
        skillWnd.SetWndState(true);
        skillWnd.IsOpen = true;
    }

    public void CloseSkillWnd()
    {
        skillWnd.SetWndState(false);
        skillWnd.IsOpen = false;
    }

    public void ShowSkillToolTip(int skillID)
    {
        //ToDo
        
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            Skill skill = new Skill(ResSvc.Instance.SkillDic[304]);
            skill.BeginCast(new SkillCastInfo());
        }
    }
    public void InstantiateCasterSkillEffect(int SkillID, Transform CasterTransform)
    {
        ActiveSkillInfo info = (ActiveSkillInfo)ResSvc.Instance.SkillDic[SkillID];
        Transform go = ((GameObject)Instantiate(Resources.Load("Prefabs/SkillPrefabs/" + info.AniPath["Self"]))).GetComponent<Transform>();
        go.SetParent(CasterTransform);
        go.localPosition = new Vector3(info.AniOffset["Self"][0], info.AniOffset["Self"][1], info.AniOffset["Self"][2]);
        if (info.Sound["Cast"] != "")
        {
            AudioSvc.Instance.PlaySkillAudio(info.Sound["Cast"]);
        }       
    }
    public void InstantiateTargetSkillEffect(int SkillID, Transform TargetTransform)
    {
        ActiveSkillInfo info = (ActiveSkillInfo)ResSvc.Instance.SkillDic[SkillID];
        Transform go = ((GameObject)Instantiate(Resources.Load("Prefabs/SkillPrefabs/" + info.AniPath["Other"]))).GetComponent<Transform>();
        go.SetParent(TargetTransform);
        go.localScale = Vector3.one;
        go.localPosition = new Vector3(info.AniOffset["Target"][0], info.AniOffset["Target"][1], info.AniOffset["Target"][2]);
        if (info.Sound["Hit"] != "")
        {
            //AudioSvc.Instance.PlaySkillAudio(info.Sound["Hit"]);
        }
    }
    public void InitPlayerSkills(Player player, EntityController controller)
    {
        controller.SkillDict = new Dictionary<int, Skill>();
        if(player.Skills!=null && player.Skills.Count > 0)
        {
            foreach (var kv in player.Skills)
            {
                controller.SkillDict[kv.Key] = new Skill(ResSvc.Instance.SkillDic[kv.Key]);
                controller.SkillDict[kv.Key].CD = 0;
                controller.SkillDict[kv.Key].EntityController = controller;
                controller.SkillDict[kv.Key].SkillLevel = kv.Value.SkillLevel;
            }
        }
    }
    public void InitPlayerSkills(TrimedPlayer player, EntityController controller)
    {
        controller.SkillDict = new Dictionary<int, Skill>();
        if (player.Skills != null && player.Skills.Count > 0)
        {
            foreach (var kv in player.Skills)
            {
                controller.SkillDict[kv.Key] = new Skill(ResSvc.Instance.SkillDic[kv.Key]);
                controller.SkillDict[kv.Key].CD = 0;
                controller.SkillDict[kv.Key].EntityController = controller;
                controller.SkillDict[kv.Key].SkillLevel = kv.Value.SkillLevel;
            }
        }
    }
    
}

