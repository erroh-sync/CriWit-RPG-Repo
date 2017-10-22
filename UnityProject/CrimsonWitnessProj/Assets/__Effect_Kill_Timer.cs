using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class __Effect_Kill_Timer : MonoBehaviour {

    public float killTime = 1.0f;

	// Use this for initialization
	void Start () {
        Destroy(this.gameObject, killTime);
	}
}
