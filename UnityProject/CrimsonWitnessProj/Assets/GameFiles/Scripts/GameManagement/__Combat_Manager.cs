using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class __Combat_Manager : MonoBehaviour {

    public enum CombatState { ecs_Init, ecs_Intro, ecs_Dialogue, ecs_Decision, ecs_Action, ecs_Resolution, ecs_Victory, ecs_PlayerDeath };

    public enum ActionType { eat_Attack, eat_Skill, eat_Item, eat_Wait };

    // The current position in the combat we are at.
    private CombatState currentState = CombatState.ecs_Init;

    public static __Combat_Manager Instance;

    [Header("Data Storage")]

    // Array of Combat Actor Prefabs in Indexed Order
    [SerializeField]
    private GameObject[] combatActorLookup = new GameObject[0];

    // Array of Skill Prefabs in Indexed Order
    [SerializeField]
    private GameObject[] skillLookup = new GameObject[0];

    [Header("Camera Animation")]
     
    // The animation that the camera will follow on entry to the battle.
    [SerializeField]
    private string IntroCamAnim = "intro";

    [Header("Menu Prefabs")]

    // The curve that the camera will follow when combat is entered.
    [SerializeField]
    private GameObject CombatMenu;

    [Header("Test Data")]
    public int PlayerID = 0;
    public int DanteID = 1;
    public int turnFrames = 20;

    // Stores the spawned objects making up the player's party.
    private List<GameObject> playerParty = new List<GameObject>();

    // Stores the spawned objects making up the enemy's party.
    private List<GameObject> enemyParty = new List<GameObject>();

    // List used for turn order.
    private List<GameObject> allChars = new List<GameObject>();

    // Denotes who's turn it is
    private int turnIndex = 0;

    // The current action type
    [HideInInspector]
    public ActionType action_Type = ActionType.eat_Wait;

    // The index of the current skill/item
    [HideInInspector]
    public int action_Index = 0;

    // The current target object
    [HideInInspector]
    public List<GameObject> action_Target = new List<GameObject>();

    // Awake
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    // Called on Start
    private void Start()
    {
        initCombat();
    }

    // Called once per Frame
    private void Update()
    {
        stepCombatState();
    }

    // Initializes the combat
    private void initCombat()
    {
        // Player Party
        // TODO: Actually spawn the player's party out. For now just spawn three protag test objects
        for (int i = 0; i < 3; i++)
        {
            if (combatActorLookup.Length >= PlayerID)
                playerParty.Add(Instantiate(combatActorLookup[PlayerID], this.transform.position + new Vector3(-2.5f + (2.5f * i), 0, -5.0f), Quaternion.Euler(0, 0, 0)) as GameObject);
        }

        // Enemy Party
        // TODO: Actually spawn the enemy party. For now just summon 3 Dantes from the devil may cry serieses
        for (int i = 0; i < 5; i++)
        {
            if (combatActorLookup.Length >= DanteID)
            {
                enemyParty.Add(Instantiate(combatActorLookup[DanteID], this.transform.position + new Vector3(-5.0f + (2.5f * i), 0, 5.0f), Quaternion.Euler(0, 180, 0)) as GameObject);
                enemyParty[i].GetComponent<__Combat_Actor_Script>().teamIndex = 1;
            }
        }

        FindObjectOfType<__Combat_Quick_Stats_Manager>().Init();

        sortPartyByAgility();

        // Move to the next state;
        advanceCombatState(CombatState.ecs_Intro);
    }

    void sortPartyByAgility()
    {
        allChars.Clear();

        // Add the Players
        for(int i = 0; i < playerParty.Count; i++)
        {
            allChars.Add(playerParty[i]);
        }

        // Add the Enemies
        for (int i = 0; i < enemyParty.Count; i++)
        {
            allChars.Add(enemyParty[i]);
        }
    }

    // Runs every time we enter a new Combat State
    void advanceCombatState(CombatState newState)
    {
        switch (newState)
        {
            case CombatState.ecs_Intro:
                __Camera_Effects_Controller.Instance.CamPlayAnim(IntroCamAnim);
                StartCoroutine("C_IntroDelay");
                break;
            case CombatState.ecs_Dialogue:
                break;
            case CombatState.ecs_Decision:
                if (allChars[turnIndex].GetComponent<__Combat_Actor_Script>().teamIndex == 0)
                {
                    __Camera_Effects_Controller.Instance.CamPlayAnim("CamAnim_Decision");
                    GameObject.FindGameObjectWithTag("CameraRig").transform.position = allChars[turnIndex].transform.position;
                    GameObject.FindGameObjectWithTag("CameraRig").transform.rotation = allChars[turnIndex].transform.rotation;
                }
                foreach (GameObject go in allChars)
                {
                    go.GetComponent<__Combat_Actor_Script>().setPosToHome();
                }
                break;
            case CombatState.ecs_Action:
                break;
            case CombatState.ecs_Resolution:
                break;
            case CombatState.ecs_Victory:
                break;
            case CombatState.ecs_PlayerDeath:
                break;
            default: break;
        }
        currentState = newState;
    }

    IEnumerator C_IntroDelay()
    {
        yield return new WaitForSeconds(1.4f);
        advanceCombatState(CombatState.ecs_Dialogue);
    }

    // Runs every frame to adjust stuff based on our current Combat State
    void stepCombatState()
    {
        switch (currentState)
        {
            case CombatState.ecs_Intro:
                break;
            case CombatState.ecs_Dialogue:
                // TODO: Some stuff here but for now fuck it off
                advanceCombatState(CombatState.ecs_Decision);
                break;
            case CombatState.ecs_Decision:
                // TODO: Make it so this picks a skill based on actual stuff
                turnFrames -= 1;
                if (allChars[turnIndex].GetComponent<__Combat_Actor_Script>().teamIndex == 0)
                {
                    if (!GameObject.FindGameObjectWithTag("CombatMenu"))
                        Instantiate(CombatMenu, GameObject.FindGameObjectWithTag("Canvas").transform);
                }else
                {
                    action_Type = ActionType.eat_Attack;
                    action_Target.Clear();
                    action_Target.Add(playerParty[Random.Range(0,playerParty.Count)]);
                    action_Index = 0; // TODO: Replace this with a function in the enemy code that determines the attack to use.

                    DoAction();
                }

                if (GameObject.FindGameObjectWithTag("AttackHandle"))
                {
                    advanceCombatState(CombatState.ecs_Action);
                    if (GameObject.FindGameObjectWithTag("CombatMenu"))
                        Destroy(GameObject.FindGameObjectWithTag("CombatMenu"));
                }
                break;
            case CombatState.ecs_Action:
                // TODO: This might actually be all this state has to do
                if (!GameObject.FindGameObjectWithTag("AttackHandle"))
                {
                    advanceCombatState(CombatState.ecs_Resolution);
                }
                break;
            case CombatState.ecs_Resolution:
                // Increment the turn and if we go over reset the list and go again.
                turnIndex += 1;
                if(turnIndex > allChars.Count - 1)
                {
                    sortPartyByAgility();
                    turnIndex = 0;
                }

                // TODO: Some stuff here but for now fuck it off
                advanceCombatState(CombatState.ecs_Decision);
                break;
            case CombatState.ecs_Victory:
                break;
            case CombatState.ecs_PlayerDeath:
                OnPlayerDeath();
                break;
            default: break;
        }
    }

    public void DoAction()
    {
        __Combat_Attack_Script act = null;
        switch (action_Type)
        {
            case ActionType.eat_Attack:
                act = (Instantiate(skillLookup[0], this.transform) as GameObject).GetComponent<__Combat_Attack_Script>();
                break;
            case ActionType.eat_Skill:
                act = (Instantiate(skillLookup[action_Index], this.transform) as GameObject).GetComponent<__Combat_Attack_Script>();
                break;
            default:
                break;
        }
        act.User = allChars[turnIndex];
        act.Targets.AddRange(action_Target);
    }

    // Char getters
    public List<GameObject> GetEnemyParty()
    {
        return enemyParty;
    }

    public List<GameObject> GetPlayerParty()
    {
        return playerParty;
    }

    // Char remover
    public void RemoveCharFromArray(GameObject actorToRemove, int TeamIndex)
    {
        if(TeamIndex == 0)
            playerParty.Remove(actorToRemove);
        else
            enemyParty.Remove(actorToRemove);

        allChars.Remove(actorToRemove);

        if(playerParty.Count == 0)
        {
            advanceCombatState(CombatState.ecs_PlayerDeath);
        }
    }

    void OnPlayerDeath()
    {
        StartCoroutine("C_OnPlayerDeath");
    }

    IEnumerator C_OnPlayerDeath()
    {
        for (int i = 0; i < 30; i++)
        {
            __Camera_Effects_Controller.Instance.PlayerDeathFadeStep();
            yield return new WaitForSeconds(0.1f);
        }

        SceneManager.LoadScene(0);
    }

    public GameObject getCurrentTurnUser()
    {
        return allChars[turnIndex];
    }

    // Skill Interface
    public string getSkillName(int index)
    {
        return skillLookup[index].GetComponent<__Combat_Attack_Script>().skillName;
    } 
}
