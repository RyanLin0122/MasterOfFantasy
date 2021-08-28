using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCShopInfo
{
    public int NPCID;
    public List<int> SellConsumables;
    public List<int> SellEquipments;
    public List<int> SellWeapons;
    public List<int> SellETCItems;
    public List<int> SellMaterials;
    public List<int> SellBadges;
    public List<int> SellType; 
    public NPCShopInfo()
    {
        this.SellConsumables = new List<int>();
        this.SellEquipments = new List<int>();
        this.SellWeapons = new List<int>();
        this.SellETCItems = new List<int>();
        this.SellMaterials = new List<int>();
        this.SellBadges = new List<int>();
        this.SellType = new List<int>();
    }
    
}
