using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragSystem : SystemRoot
{
    public static DragSystem Instance = null;
    public override void InitSys()
    {
        base.InitSys();
        state = DragState.UnDrag;
        Instance = this;
        Debug.Log("Init DragSys...");
    }

    public DragState state = DragState.UnDrag;
    public GameObject DragContainer;
    public DragObject CurrentDragObject;

}
public enum DragState
{
    Dragging,
    UnDrag
}
public enum DragObjectType
{
    Consumable,
    Skill,
    DiaryItem
}
public class DragBaseData
{
    public Dictionary<string, double> data;
    public string Str = "";

}