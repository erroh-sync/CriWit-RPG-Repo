using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class __List_Button_Skill : __List_Button{

    [SerializeField]
    private Text spT;

    public override void setNameString(string newName)
    {
        base.setNameString(newName);
    }

    public void setSPString(int cost)
    {
        spT.text = cost.ToString() + "SP";
    }
}
