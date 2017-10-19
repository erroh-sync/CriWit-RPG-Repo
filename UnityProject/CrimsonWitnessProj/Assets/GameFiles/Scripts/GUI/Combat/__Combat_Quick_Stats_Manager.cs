using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class __Combat_Quick_Stats_Manager : MonoBehaviour {

    [Header("Menu Prefabs")]

    // The curve that the camera will follow when combat is entered.
    [SerializeField]
    private GameObject panelItem;

    private int currentNum = 0;

    private List<GameObject> playerparty;

    public void Init()
    {
        StartCoroutine("C_Init");
    }

    IEnumerator C_Init()
    {
        playerparty = __Combat_Manager.Instance.GetPlayerParty();

        for (int i = 0; i < playerparty.Count; i++)
        {
            yield return new WaitForSeconds(0.35f);
            SpawnPanel(i);
        }
    }

    public void SpawnPanel(int partyPosition)
    {
        // TODO: Give the panel a reference to it's active Game Object
        //List<GameObject> playerparty = __Combat_Manager.Instance.GetPlayerParty();
        
        GameObject panel = Instantiate(panelItem, this.transform) as GameObject;
        panel.transform.position -= new Vector3(224 * partyPosition, 0, 0);

        panel.GetComponent<__Combat_Quick_Stats_Controller>().AssignCharacterReference(playerparty[partyPosition].GetComponent<__Combat_Actor_Script>());
    }
}
