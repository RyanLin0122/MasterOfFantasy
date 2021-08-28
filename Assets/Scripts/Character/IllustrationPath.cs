using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class IllustrationPath : MonoBehaviour
{
    private static IllustrationPath instance = null;
    public static IllustrationPath Instance
    {
        get
        {
            if (instance == null)
            {
                instance = InventoryManager.Instance.GetComponent<IllustrationPath>();
            }
            return instance;
        }
    }
    public void Init()
    {       
        ParseSprite();
    }
    public List<Sprite> GetSpriteByID(int ItemID)
    {

        return IllusCache[ItemID];
    }
    public List<Vector4> GetOffsetByID(int ItemID)
    {
        return IllustrationOffset[ItemID];
    }
    public Dictionary<int, List<Vector4>> IllustrationOffset = new Dictionary<int, List<Vector4>>();
    public Dictionary<int, List<Sprite>> IllusCache = new Dictionary<int, List<Sprite>>();
    private void ParseSprite()
    {
        TextAsset xml = Resources.Load<TextAsset>("ResCfgs/Illustration");
        if (!xml)
        {

        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);

            XmlNodeList nodLst = doc.SelectSingleNode("root").ChildNodes;

            for (int i = 0; i < nodLst.Count; i++)
            {

                XmlElement ele = nodLst[i] as XmlElement;

                if (ele.GetAttributeNode("ID") == null)
                {
                    continue;
                }
                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
                List<Sprite> Sp = new List<Sprite>();
                List<Vector4> OS = new List<Vector4>();
                List<string> TempPath = new List<string>();
                List<int> TempOrder = new List<int>();
                List<float> TempPos = new List<float>();
                List<float> TempScale = new List<float>();
                //PECommon.Log("解析Illustration: " + ID);
                foreach (XmlElement e in nodLst[i].ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "Path":
                            string[] SpriteArr = e.InnerText.Split(',');
                            if (SpriteArr.Length == 3)
                            {
                                //PECommon.Log("aa");
                                TempPath.Add(SpriteArr[0]);
                                TempPath.Add(SpriteArr[1]);
                                TempPath.Add(SpriteArr[2]);
                            }
                            else
                            {
                                TempPath.Add(SpriteArr[0]);
                                TempPath.Add(SpriteArr[1]);
                                TempPath.Add(SpriteArr[2]);
                                TempPath.Add(SpriteArr[3]);
                                TempPath.Add(SpriteArr[4]);
                                TempPath.Add(SpriteArr[5]);
                            }
                            break;
                        case "Order":
                            string[] valArr = e.InnerText.Split(',');
                            if (valArr.Length == 3)
                            {
                                //PECommon.Log("aa");
                                TempOrder.Add(Convert.ToInt32(valArr[0]));
                                TempOrder.Add(Convert.ToInt32(valArr[1]));
                                TempOrder.Add(Convert.ToInt32(valArr[2]));
                            }
                            else
                            {
                                TempOrder.Add(Convert.ToInt32(valArr[0]));
                                TempOrder.Add(Convert.ToInt32(valArr[1]));
                                TempOrder.Add(Convert.ToInt32(valArr[2]));
                                TempOrder.Add(Convert.ToInt32(valArr[3]));
                                TempOrder.Add(Convert.ToInt32(valArr[4]));
                                TempOrder.Add(Convert.ToInt32(valArr[5]));
                            }
                            break;
                        case "Position":
                            string[] PosArr = e.InnerText.Split(',');
                            if (PosArr.Length == 6)
                            {
                                //PECommon.Log("aa");
                                TempPos.Add((float)Convert.ToDouble(PosArr[0]));
                                TempPos.Add((float)Convert.ToDouble(PosArr[1]));
                                TempPos.Add((float)Convert.ToDouble(PosArr[2]));
                                TempPos.Add((float)Convert.ToDouble(PosArr[3]));
                                TempPos.Add((float)Convert.ToDouble(PosArr[4]));
                                TempPos.Add((float)Convert.ToDouble(PosArr[5]));
                            }
                            else //12
                            {
                                TempPos.Add((float)Convert.ToDouble(PosArr[0]));
                                TempPos.Add((float)Convert.ToDouble(PosArr[1]));
                                TempPos.Add((float)Convert.ToDouble(PosArr[2]));
                                TempPos.Add((float)Convert.ToDouble(PosArr[3]));
                                TempPos.Add((float)Convert.ToDouble(PosArr[4]));
                                TempPos.Add((float)Convert.ToDouble(PosArr[5]));
                                TempPos.Add((float)Convert.ToDouble(PosArr[6]));
                                TempPos.Add((float)Convert.ToDouble(PosArr[7]));
                                TempPos.Add((float)Convert.ToDouble(PosArr[8]));
                                TempPos.Add((float)Convert.ToDouble(PosArr[9]));
                                TempPos.Add((float)Convert.ToDouble(PosArr[10]));
                                TempPos.Add((float)Convert.ToDouble(PosArr[11]));
                            }
                            break;
                        case "Scale":
                            string[] ScaArr = e.InnerText.Split(',');
                            if (ScaArr.Length == 6)
                            {
                                //PECommon.Log("aa");
                                TempScale.Add((float)Convert.ToDouble(ScaArr[0]));
                                TempScale.Add((float)Convert.ToDouble(ScaArr[1]));
                                TempScale.Add((float)Convert.ToDouble(ScaArr[2]));
                                TempScale.Add((float)Convert.ToDouble(ScaArr[3]));
                                TempScale.Add((float)Convert.ToDouble(ScaArr[4]));
                                TempScale.Add((float)Convert.ToDouble(ScaArr[5]));
                            }
                            else //12
                            {
                                TempScale.Add((float)Convert.ToDouble(ScaArr[0]));
                                TempScale.Add((float)Convert.ToDouble(ScaArr[1]));
                                TempScale.Add((float)Convert.ToDouble(ScaArr[2]));
                                TempScale.Add((float)Convert.ToDouble(ScaArr[3]));
                                TempScale.Add((float)Convert.ToDouble(ScaArr[4]));
                                TempScale.Add((float)Convert.ToDouble(ScaArr[5]));
                                TempScale.Add((float)Convert.ToDouble(ScaArr[6]));
                                TempScale.Add((float)Convert.ToDouble(ScaArr[7]));
                                TempScale.Add((float)Convert.ToDouble(ScaArr[8]));
                                TempScale.Add((float)Convert.ToDouble(ScaArr[9]));
                                TempScale.Add((float)Convert.ToDouble(ScaArr[10]));
                                TempScale.Add((float)Convert.ToDouble(ScaArr[11]));
                            }
                            break;
                    }
                }
                if (TempOrder.Count == 3)
                {
                    //PECommon.Log("aa");
                    Sp.Add(GetSprite(TempPath[0], TempOrder[0]));
                    Sp.Add(GetSprite(TempPath[1], TempOrder[1]));
                    Sp.Add(GetSprite(TempPath[2], TempOrder[2]));
                    OS.Add(new Vector4(TempPos[0], TempPos[1], TempScale[0], TempScale[1]));
                    OS.Add(new Vector4(TempPos[2], TempPos[3], TempScale[2], TempScale[3]));
                    OS.Add(new Vector4(TempPos[4], TempPos[5], TempScale[4], TempScale[5]));
                    //PECommon.Log("aa");
                }
                else
                {
                    Sp.Add(GetSprite(TempPath[0], TempOrder[0]));
                    Sp.Add(GetSprite(TempPath[1], TempOrder[1]));
                    Sp.Add(GetSprite(TempPath[2], TempOrder[2]));
                    Sp.Add(GetSprite(TempPath[3], TempOrder[3]));
                    Sp.Add(GetSprite(TempPath[4], TempOrder[4]));
                    Sp.Add(GetSprite(TempPath[5], TempOrder[5]));
                    OS.Add(new Vector4(TempPos[0], TempPos[1], TempScale[0], TempScale[1]));
                    OS.Add(new Vector4(TempPos[2], TempPos[3], TempScale[2], TempScale[3]));
                    OS.Add(new Vector4(TempPos[4], TempPos[5], TempScale[4], TempScale[5]));
                    OS.Add(new Vector4(TempPos[6], TempPos[7], TempScale[6], TempScale[7]));
                    OS.Add(new Vector4(TempPos[8], TempPos[9], TempScale[8], TempScale[9]));
                    OS.Add(new Vector4(TempPos[10], TempPos[11], TempScale[10], TempScale[11]));
                }
                IllusCache.Add(ID, Sp);
                IllustrationOffset.Add(ID, OS);
                //PECommon.Log(IllusCache[3001].Count.ToString());
            }
        }

    }
    private Sprite GetSprite(string path, int index)
    {
        Sprite s = Resources.LoadAll<Sprite>("Character/Illustration/"+path)[index];
        return s;
    }
}
