using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class __List_Script_PreInst : __List_Script{
    [SerializeField]
    private List<GameObject> preInstantiatedButtons = new List<GameObject>();

    protected override void Init()
    {
        spawnedButtons = preInstantiatedButtons;
        visibleButtonCount = preInstantiatedButtons.Count;
        spawnedButtons[0].GetComponent<__List_Button>().setSelected();
        SetButtonNames();
    }
}
