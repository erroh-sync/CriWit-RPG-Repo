using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class __Encounter_Volume : __Trigger_Volume {

    [SerializeField]
    [Tooltip("The combat scene used by this trigger volume.")]
    private int combatSceneIndex = -1;

    [SerializeField]
    [Tooltip("Vector offset from the player's root to follow.")]
    private float TransitionWaitTime = 3.0f;

    [SerializeField]
    [Tooltip("Effect spawned when an enemy is encountered")]
    private GameObject TransitionEffect;

    [SerializeField]
    [Tooltip("Effect spawned when an enemy is encountered")]
    private GameObject[] PossibleEncounters = new GameObject[1];

    int timer = 200;

    private void Update()
    {
        timer--;
    }

    // TODO: Should increment an encounter value. For now just auto scene transition
    protected override void OnTriggered()
    {
        if (timer < 0)
        {
            // Mark the selected encounter for keeps
            int selectedEncounter = Random.Range(0, PossibleEncounters.Length);
            DontDestroyOnLoad(PossibleEncounters[selectedEncounter]);
             
            // Minisave so that we return to the same place after combat (Also freeze the player)
            GameObject p = GameObject.FindGameObjectWithTag("Player"); // TODO: Reference the player someow so we don't have to Find them
            if (p)
            {
                __Game_Manager.Instance.playerStoredPosition = p.transform.position;
                __Game_Manager.Instance.playerStoredYaw = p.transform.eulerAngles.y;
                p.GetComponent<__Overworld_Player_Control>().Stopped = true;
            }
            __Game_Manager.Instance.lastScene = SceneManager.GetActiveScene().buildIndex; // Save the current scene so we know which one to come back to

            // Instantiates the particles used during the transition. TODO: Make this way better looking.
            Instantiate(TransitionEffect, __Camera_Effects_Controller.Instance.transform);
            StartCoroutine("C_CombatEntry");
        }
    }

    // Animaes the combat fade
    IEnumerator C_CombatEntry()
    {
        __Camera_Effects_Controller.Instance.EnterCombatFade();
        yield return new WaitForSeconds(TransitionWaitTime);
        SceneManager.LoadScene(combatSceneIndex);
    }
}
