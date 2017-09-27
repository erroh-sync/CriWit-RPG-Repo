using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class __List_Animation_Controller : MonoBehaviour {

    public __List_Script myListObj;
    public Animator myAnim;
	
	// Update is called once per frame
	void Update () {
        myAnim.SetBool("Active", myListObj.isActiveAndEnabled);
	}
}
