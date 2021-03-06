using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HotKeyDragTarget : DragTargetBase, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {

        }
    }

    public override void ReceiveObject(DragObject dragObject)
    {
        Debug.Log("HotKeyTarget Receive Object");
        GetComponent<Image>().sprite = dragObject.GetComponent<Image>().sprite;
        data = dragObject.data;
    }
}
