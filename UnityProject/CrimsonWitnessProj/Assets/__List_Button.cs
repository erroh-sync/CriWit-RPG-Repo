using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class __List_Button : MonoBehaviour {

    public Button btn;

    public void setSelected()
    {
            btn.Select();
    }

    public void setNameString(string newName)
    {
        btn.GetComponentInChildren<Text>().text = newName;
    }
}
