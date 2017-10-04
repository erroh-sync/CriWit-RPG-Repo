using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class __List_Button : MonoBehaviour {

    [SerializeField]
    private Button btn;
    [SerializeField]
    private Text t;

    public void setSelected()
    {
            btn.Select();
    }

    public virtual void setNameString(string newName)
    {
        t.text = newName;
    }
}
