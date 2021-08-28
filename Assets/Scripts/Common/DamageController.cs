using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DamageController : MonoBehaviour
{
    public GameObject CritalImg0;
    public GameObject CritalImg1;
    public TMP_Text text;

    public void SetNumber(int num, int mode)
    {
        if (mode == 1)
        {
            CritalImg0.SetActive(true);
            CritalImg1.SetActive(true);
        }
        else
        {
            CritalImg0.SetActive(false);
            CritalImg1.SetActive(false);
        }
        if (transform.parent.parent.transform.localScale.x < 0)
        {
            transform.parent.localScale = new Vector3(-transform.parent.localScale.x, transform.parent.localScale.y, transform.parent.localScale.z);
        }
        text.text = Tools.GetFloatingDamage(num, mode);
    }
    public void SetNumber(long num, int mode)
    {
        if (mode == 1)
        {
            CritalImg0.SetActive(true);
            CritalImg1.SetActive(true);
        }
        else
        {
            CritalImg0.SetActive(false);
            CritalImg1.SetActive(false);
        }
        if (transform.parent.parent.transform.localScale.x < 0)
        {
            transform.parent.localScale = new Vector3(-transform.parent.localScale.x, transform.parent.localScale.y, transform.parent.localScale.z);
        }
        text.text = Tools.GetFloatingDamage(num, mode);
    }
    public void DestroySelf()
    {
        Destroy(transform.gameObject);
    }
}
