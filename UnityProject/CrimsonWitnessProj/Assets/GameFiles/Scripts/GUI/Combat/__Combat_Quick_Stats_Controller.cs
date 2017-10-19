using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class __Combat_Quick_Stats_Controller : MonoBehaviour {

    [SerializeField]
    private Text hpText;
    [SerializeField]
    private Text mpText;

    [SerializeField]
    private Animator anim;

    private float hpDisplay = 100;
    private float mpDisplay = 80;

    private bool isKill = false;

    private __Combat_Actor_Script CharRef;
	
	// Update is called once per frame
	void Update () {
        if (CharRef)
        {
            if(hpDisplay != CharRef.getCurrentHP())
                hpDisplay += Mathf.Sign(CharRef.getCurrentHP() - hpDisplay);
            // TODO: MP Here
        }

        hpText.text = ((int)hpDisplay).ToString();
        mpText.text = ((int)mpDisplay).ToString();

        if (hpDisplay < 1 && !isKill)
            OnKill();
    }

    void OnKill()
    {
        isKill = true;
        StartCoroutine("C_OnKill");
    }

    IEnumerator C_OnKill()
    {
        anim.SetBool("IsDead", true);
        yield return new WaitForSeconds(1.4f);
        Destroy(this.gameObject);
    }

    public void AssignCharacterReference(__Combat_Actor_Script newCharacter)
    {
        CharRef = newCharacter;
        hpDisplay = CharRef.getCurrentHP();
        // TODO: MP Here
    }
}
