using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class __Combat_Actor_Script : MonoBehaviour {

    [Header ("DeterministicStats")]
    [SerializeField]
    private int level = 1;

    [Tooltip ("Stat at level 100")]
    [SerializeField]
    private int maxEn = 250;
    [Tooltip("Stat at level 100")]
    [SerializeField]
    private int maxSt = 250;
    [Tooltip("Stat at level 100")]
    [SerializeField]
    private int maxMa = 250;
    [Tooltip("Stat at level 100")]
    [SerializeField]
    private int maxAg = 250;
    [Tooltip("Stat at level 100")]
    [SerializeField]
    private int maxLu = 250;
    [Tooltip("Skills known by this character")]
    [SerializeField]
    private List<int> skillSet = new List<int>();

    [Header("DisplayInfo")]
    [SerializeField]
    private Vector3 cameraOffset = new Vector3(0,0,0);

    [Header("External References")]
    [SerializeField]
    private Animation anim;

    // Runtime Stats // 
    private float currentHP;

    // Buff/Debuffs
    private float AgilityBuff = 0.0f;
    private float AttackBuff = 0.0f;
    private float DefenceBuff = 0.0f;
    private float MagicBuff = 0.0f;

    // Misc
    public int teamIndex = 0; // 0 = Player, 1 = Enemy
    private Vector3 Home;

    private void Start()
    {
        Home = this.transform.position;
        setPosToHome();

        // Set Runtime Stats
        currentHP = getMaxHP(); // TODO: Load player character's HP and MP from the player's data instead. Be sure to save it out at the end too!
        // TODO: MP Too
    }

    /*  Stat Getters  */
    public float getMaxHP()
    {
        return ((float)maxEn * ((float)level/100.0f)) * 6;
    }

    public float getCurrentHP()
    {
        return currentHP;
    }

    public float getAgility()
    {
        return ((float)maxAg * ((float)level / 100.0f)) * AgilityBuff;
    }

    public float getAttack()
    {
        return ((float)maxSt * ((float)level / 100.0f)) * AttackBuff;
    }

    public float getDefence()
    {
        return ((float)maxEn * ((float)level / 100.0f)) * DefenceBuff;
    }

    public float getMagic()
    {
        return ((float)maxMa * ((float)level / 100.0f)) * MagicBuff;
    }

    /* Stat Alterations */
    public void adjustHP(float amnt)
    {
        currentHP -= amnt;

        if (currentHP > getMaxHP())
            currentHP = getMaxHP();
        else if (currentHP <= 0)
            OnDeath();
    }

    // Called when HP reaches 0
    public void OnDeath()
    {
        currentHP = 0;
        playAnim("Death");

        // Remove from Array
        __Combat_Manager.Instance.RemoveCharFromArray(this.gameObject, teamIndex);

        // Destroy if it's an enemy
    }

    // Gets the cam offset
    public Vector3 getCamOffset()
    {
        return cameraOffset;
    }

    // Plays an animation at a set index
    public void playAnim(string name)
    {
        anim.Play(name);
    }

    // Moves the character back to their home.
    public void setPosToHome()
    {
        this.transform.position = Home;
        anim.Play("Idle");
    }

    // Get Skill List
    public int getSkillAtIndex(int index)
    {
        if (index >= skillSet.Count)
        {
            return -1;
        }
        return skillSet[index];
    }
}
