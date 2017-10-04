using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class __List_Functionality_Combat_Command : __List_Functionality {

    [SerializeField]
    private __List_Script SkillMenu;

    [SerializeField]
    private __Combat_Targetting  TargMenu;

    public override void OnListUsed(int index)
    {
        switch (index)
        {
            case 0:
                __Combat_Manager.Instance.action_Type = __Combat_Manager.ActionType.eat_Attack;
                TargMenu.enabled = true;
                this.gameObject.GetComponent<__List_Script>().enabled = false;
                break;
            case 1:
                this.gameObject.GetComponent<__List_Script>().enabled = false;
                SkillMenu.enabled = true;
                break;
            default:
                break;
        }
    }
}
