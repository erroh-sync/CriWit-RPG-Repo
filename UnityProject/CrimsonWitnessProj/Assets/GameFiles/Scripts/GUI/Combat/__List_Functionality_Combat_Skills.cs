using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class __List_Functionality_Combat_Skills : __List_Functionality {

    [SerializeField]
    private __List_Script CommandMenu;

    public override void OnListUsed(int index)
    {
        // Do Stuff Here
    }

    public override void OnListBack(int index)
    {
        this.gameObject.GetComponent<__List_Script>().enabled = false;
        CommandMenu.enabled = true;
        // Do Stuff Here
    }
}
