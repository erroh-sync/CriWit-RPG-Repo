using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class __List_Functionality_Combat_Skills : __List_Functionality {

    [SerializeField]
    private __List_Script CommandMenu;

    [SerializeField]
    private __Combat_Targetting TargMenu;

    private void Start()
    {
        string[] skillnames = new string[8];
        
        for(var i = 0; i < skillnames.Length; i++)
        {
            int skillIndex = __Combat_Manager.Instance.getCurrentTurnUser().GetComponent<__Combat_Actor_Script>().getSkillAtIndex(i);
            if (skillIndex == -1)
                skillnames[i] = "---";
            else
                skillnames[i] = __Combat_Manager.Instance.getSkillName(skillIndex);
        }
        
        __List_Script_PreInst list = this.gameObject.GetComponent<__List_Script_PreInst>();
        list.names = skillnames;
        list.SetButtonNames();
    }

    public override void OnListUsed(int index)
    {
        __Combat_Manager.Instance.action_Type = __Combat_Manager.ActionType.eat_Skill;
        __Combat_Manager.Instance.action_Index = __Combat_Manager.Instance.getCurrentTurnUser().GetComponent<__Combat_Actor_Script>().getSkillAtIndex(this.gameObject.GetComponent<__List_Script>().getPosition());
        TargMenu.enabled = true;
        this.gameObject.GetComponent<__List_Script>().enabled = false;
    }

    public override void OnListBack(int index)
    {
        this.gameObject.GetComponent<__List_Script>().enabled = false;
        CommandMenu.enabled = true;
        // Do Stuff Here
    }
}
