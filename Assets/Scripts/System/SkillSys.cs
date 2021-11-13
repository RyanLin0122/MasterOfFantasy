using UnityEngine;
using PEProtocal;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

class SkillSys : MonoSingleton<SkillSys>
{
    public Dictionary<int, Skill> MainCharacterSkillDic = null;
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
        if (MainCharacterSkillDic != null && MainCharacterSkillDic.Count > 0)
        {
            foreach (var skill in MainCharacterSkillDic.Values)
            {
                skill.Update(Time.deltaTime);
            }
        }
    }
    public void InstantiateCasterSkillEffect(int SkillID, Transform CasterTransform)
    {
        ActiveSkillInfo info = (ActiveSkillInfo)ResSvc.Instance.SkillDic[SkillID];
        if (info.AniPath["Self"] != "")
        {
            Transform go = ((GameObject)Instantiate(Resources.Load("Prefabs/SkillPrefabs/" + info.AniPath["Self"]))).GetComponent<Transform>();
            go.SetParent(CasterTransform);
            go.localPosition = new Vector3(info.AniOffset["Self"][0], info.AniOffset["Self"][1], info.AniOffset["Self"][2]);
        }
        if (info.Sound["Cast"] != "")
        {
            AudioSvc.Instance.PlaySkillAudio("Sound/Skill/" + info.Sound["Cast"]);
        }
    }
    public void InstantiateTargetSkillEffect(int SkillID, Transform TargetTransform)
    {
        ActiveSkillInfo info = (ActiveSkillInfo)ResSvc.Instance.SkillDic[SkillID];
        if (info.AniPath["Other"] != "")
        {
            Transform go = ((GameObject)Instantiate(Resources.Load("Prefabs/SkillPrefabs/" + info.AniPath["Other"]))).GetComponent<Transform>();
            go.SetParent(TargetTransform);
            go.localScale = Vector3.one;
            go.localPosition = new Vector3(info.AniOffset["Target"][0], info.AniOffset["Target"][1], info.AniOffset["Target"][2]);
        }

        if (info.Sound["Hit"] != "")
        {
            AudioSvc.Instance.PlaySkillAudio("Sound/" + info.Sound["Hit"]);
        }
    }
    public void InitPlayerSkills(Player player, EntityController controller)
    {
        if (MainCharacterSkillDic != null)
        {
            controller.SkillDict = MainCharacterSkillDic;
            if (controller.SkillDict.Count > 0)
            {
                foreach (var skill in controller.SkillDict.Values)
                {
                    skill.EntityController = controller;
                }
            }

            foreach (var slot in BattleSys.Instance.HotKeyManager.HotKeySlots.Values)
            {
                if (slot.State == HotKeyState.Skill)
                {
                    slot.SetColdTime(controller.SkillDict[slot.data.ID]);
                }
            }
            return;
        }
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
        MainCharacterSkillDic = controller.SkillDict;
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

