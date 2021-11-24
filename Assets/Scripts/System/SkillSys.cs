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

    public void InstantiateCasterSkillEffect(int SkillID, Transform CasterTransform)
    {
        ActiveSkillInfo info = (ActiveSkillInfo)ResSvc.Instance.SkillDic[SkillID];
        if (info.AniPath["Self"] != "")
        {
            Transform go = ((GameObject)Instantiate(Resources.Load("Prefabs/SkillPrefabs/" + info.AniPath["Self"]))).GetComponent<Transform>();
            go.SetParent(CasterTransform);
            if (CasterTransform.localScale.x < 0)
            {
                go.localScale = new Vector3(-go.localScale.x, go.localScale.y, go.localScale.z);
            }
            go.localPosition = new Vector3(info.AniOffset["Self"][0], info.AniOffset["Self"][1], info.AniOffset["Self"][2]);
        }
        if (info.Sound["Cast"] != "")
        {
            AudioSvc.Instance.PlaySkillAudio("Sound/Skill/" + info.Sound["Cast"]);
        }
    }
    public void InstantiateTargetSkillEffect(int SkillID, Transform TargetTransform, bool Dir)
    {
        ActiveSkillInfo info = (ActiveSkillInfo)ResSvc.Instance.SkillDic[SkillID];
        if (info.AniPath["Other"] != "")
        {
            Transform go = ((GameObject)Instantiate(Resources.Load("Prefabs/SkillPrefabs/" + info.AniPath["Other"]))).GetComponent<Transform>();
            go.SetParent(TargetTransform);
            go.localScale = Vector3.one;
            go.localScale = new Vector3(info.AniScale["Target"][0], info.AniScale["Target"][1], info.AniScale["Target"][2]);
            //Debug.Log("-------DIR-------- : " + Dir + " TargetTransform " + (TargetTransform.localScale.x > 0));
            if (!Dir)
            {
                if (TargetTransform.localScale.x > 0) go.localScale = new Vector3(-go.localScale.x, go.localScale.y, go.localScale.z);
                else go.localScale = new Vector3(go.localScale.x, go.localScale.y, go.localScale.z);
            }
            else
            {
                if (TargetTransform.localScale.x > 0) go.localScale = new Vector3(go.localScale.x, go.localScale.y, go.localScale.z);
                else go.localScale = new Vector3(-go.localScale.x, go.localScale.y, go.localScale.z);
            }
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
                    skill.Owner = controller;
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
                controller.SkillDict[kv.Key].Owner = controller;
                controller.SkillDict[kv.Key].Level = kv.Value.SkillLevel;
            }
        }
        MainCharacterSkillDic = controller.SkillDict;
        //增加普攻
        controller.SkillDict[-2] = new Skill(ResSvc.Instance.SkillDic[-2]);
        controller.SkillDict[-2].Owner = controller;
        controller.SkillDict[-4] = new Skill(ResSvc.Instance.SkillDic[-4]);
        controller.SkillDict[-4].Owner = controller;
        controller.SkillDict[-8] = new Skill(ResSvc.Instance.SkillDic[-8]);
        controller.SkillDict[-8].Owner = controller;
        controller.SkillDict[-10] = new Skill(ResSvc.Instance.SkillDic[-10]);
        controller.SkillDict[-10].Owner = controller;
        controller.SkillDict[-12] = new Skill(ResSvc.Instance.SkillDic[-12]);
        controller.SkillDict[-12].Owner = controller;
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
                controller.SkillDict[kv.Key].Owner = controller;
                controller.SkillDict[kv.Key].Level = kv.Value.SkillLevel;
            }
        }
        //增加普攻
        controller.SkillDict[-8] = new Skill(ResSvc.Instance.SkillDic[-8]);
        controller.SkillDict[-8].Owner = controller;
        controller.SkillDict[-10] = new Skill(ResSvc.Instance.SkillDic[-10]);
        controller.SkillDict[-10].Owner = controller;
    }

}

