using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class __Combat_Targetting : MonoBehaviour {

    // Previous Menus
    [SerializeField]
    private __List_Script Cmdmenu;
    [SerializeField]
    private __List_Script SkillMenu;
    //[SerializeField]
    //private __List_Script ItemMenu;

    [SerializeField]
    private GameObject ReticuleObj;

    private List<GameObject> targets = new List<GameObject>();
    private List<GameObject> reticules = new List<GameObject>();
    private bool targetAll = false;        

    private int currentTarget = 0;

    private int storedInput = 0;
    private float timer = 0.0f;
    [SerializeField]
    private float intervalTime = 0.2f;

    private void Update()
    {
        UpdateReticulePositions();
        Control();
    }

    private void OnEnable()
    {
        switch (__Combat_Manager.Instance.action_Type)
        {
            case __Combat_Manager.ActionType.eat_Skill:
                // TODO: Look up the target type of the skill being used.
                init(__Combat_Attack_Script.TargetType.ett_Enemy);
                break;
            default:
                init(__Combat_Attack_Script.TargetType.ett_Enemy);
                break;
        }
    }

    private void OnDisable()
    {
        // Clear the old reticules
        for (var i = 0; i < reticules.Count; i++)
            Destroy(reticules[i]);
    }

    public void init(__Combat_Attack_Script.TargetType type)
    {
        // Clear the old reticules
        if (reticules.Count > 0)
        {
            for (var i = 0; i < reticules.Count; i++)
                Destroy(reticules[i]);
        }

        reticules.Clear();
        targets.Clear();

        // Set the list
        switch (type)
        {
            case __Combat_Attack_Script.TargetType.ett_AllEnemy:
                targets = __Combat_Manager.Instance.GetEnemyParty();
                targetAll = true;
                break;
            case __Combat_Attack_Script.TargetType.ett_AllParty:
                targets = __Combat_Manager.Instance.GetPlayerParty();
                targetAll = true;
                break;
            case __Combat_Attack_Script.TargetType.ett_Party:
                targets = __Combat_Manager.Instance.GetPlayerParty();
                targetAll = false;
                break;
            case __Combat_Attack_Script.TargetType.ett_Everyone:
                targets = __Combat_Manager.Instance.GetPlayerParty();
                targets.AddRange(__Combat_Manager.Instance.GetEnemyParty());
                targetAll = true;
                break;
            case __Combat_Attack_Script.TargetType.ett_Enemy:
                targets.AddRange(__Combat_Manager.Instance.GetEnemyParty());
                targetAll = false;
                break;
            case __Combat_Attack_Script.TargetType.ett_RandomEnemies:
                targets = __Combat_Manager.Instance.GetEnemyParty();
                targetAll = true;
                break;
            case __Combat_Attack_Script.TargetType.ett_Self:
                // TODO: This
                //targets = __Combat_Manager.Instance.GetEnemyParty();
                //targetAll = true;
                break;
        }

        if (targetAll)
        {
            for (var i = 0; i < targets.Count; i++)
                reticules.Add(Instantiate(ReticuleObj, this.transform) as GameObject);
        }else
            reticules.Add(Instantiate(ReticuleObj, this.transform) as GameObject); // Only add one

        currentTarget = 0;
        UpdateReticulePositions();
    }

    private void Control()
    {
        timer -= Time.deltaTime;

        // Navigation
        int newI = 0;
        if (Input.GetAxis("Horizontal") > 0.5) { newI = 1; } else if (Input.GetAxis("Horizontal") < -0.5) { newI = -1; }
        if (newI != storedInput)
        {
            storedInput = newI;
        }

        if (timer <= 0.0f && storedInput != 0)
        {
            currentTarget += storedInput;

            if (currentTarget < 0)
                currentTarget = targets.Count - 1;
            else if (currentTarget > targets.Count - 1)
                currentTarget = 0;

            timer = intervalTime;
        }
        else if (storedInput == 0)
            timer = 0.0f;

        // Confirming
        if (Input.GetButtonDown("Submit"))
        {
            __Combat_Manager.Instance.action_Target.Clear();
            if (targetAll)
            {
                __Combat_Manager.Instance.action_Target = targets;
            }else
            {
                __Combat_Manager.Instance.action_Target.Add(targets[currentTarget]);
                __Combat_Manager.Instance.DoAction();
            }
        }

        // Exiting Back
        if (Input.GetButtonDown("Cancel"))
        {
            // TODO: Return to the last menu we were in.
            switch(__Combat_Manager.Instance.action_Type)
            {
                case __Combat_Manager.ActionType.eat_Skill:
                    SkillMenu.enabled = true;
                    break;
                default:
                    Cmdmenu.enabled = true;
                    break;
            }
            this.enabled = false;
        }
    }

    private void UpdateReticulePositions()
    {
        if (targets.Count > 0)
        {
            if (targetAll)
            {
                // Idk here
            }
            else
            {
                reticules[0].transform.position = Camera.main.WorldToScreenPoint(targets[currentTarget].transform.Find("Mesh/TargetReticuleSocket").position);
            }
        }
    }
}
