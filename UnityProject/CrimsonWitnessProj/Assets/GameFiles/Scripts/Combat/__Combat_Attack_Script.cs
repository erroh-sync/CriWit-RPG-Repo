using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class __Combat_Attack_Script : MonoBehaviour {

    public enum TargetType { ett_Self, ett_Enemy, ett_Party, ett_AllEnemy, ett_AllParty, ett_Everyone, ett_RandomEnemies };

    [Header("UserTarget")]
    public GameObject User;
    public List<GameObject> Targets;

    [Header("GUI")]
    public string skillName = "SKILL_NAME";

    [Header("Attack Variables")]
    [SerializeField]
    private float baseDamage = 1.0f;
    [SerializeField]
    private TargetType myTargetType = TargetType.ett_Enemy;
    [SerializeField]
    private float MultiTargetInterval = 0.2f;
    [SerializeField]
    private int MultiHitCount = 1;
    [SerializeField]
    private float MultiHitInterval = 0.05f;
    [SerializeField]
    private bool UseMagicStat = false;

    [Header("Timing Variables")]
    [SerializeField]
    private float totalDuration = 1.0f;
    [SerializeField]
    private float effectTime = 0.5f;

    // Current time
    private float currentTime = 0.0f;

    // Have we done this skill's effect yet?
    private bool doneEffect = false;

    // Use this for initialization
    void Start () {
        OnStart();
    }
	
	// Update is called once per frame
	void Update () {
        currentTime += Time.deltaTime;
        if (currentTime >= effectTime && !doneEffect)
        {
            doneEffect = true;
            foreach (GameObject t in Targets)
            {
                StartCoroutine("C_OnCauseEffectLoop");
            }
        }

        if (doneEffect && currentTime >= totalDuration)
        {
            Destroy(this.gameObject);
        }
        OnUpdate();
    }

    IEnumerator C_OnCauseEffectLoop()
    {
        foreach (GameObject t in Targets)
        {
            for (int j = 0; j < MultiHitCount; j++)
            {
                OnCauseEffect(t.GetComponent<__Combat_Actor_Script>());
                yield return new WaitForSeconds(MultiHitInterval);
            }
            yield return new WaitForSeconds(MultiTargetInterval);
        }
    }

    protected virtual void OnStart()
    {
        // Called once at the start.
    }

    protected virtual void OnUpdate()
    {
        // Called every frame.
    }

    protected virtual void OnCauseEffect(__Combat_Actor_Script targetactor)
    {
        // Here is where the result of the attack will go. Healing, damage, etc.
    }

    protected virtual float CalculateDamage(__Combat_Actor_Script targetactor)
    {
        float uStat = User.GetComponent<__Combat_Actor_Script>().getAttack();
        if(UseMagicStat)
            uStat = User.GetComponent<__Combat_Actor_Script>().getMagic();

        float tStat = targetactor.GetComponent<__Combat_Actor_Script>().getDefence();

        float dmg = baseDamage;// = (uStat - tStat) + baseDamage;

        if(dmg >= 0.0f)
        {
            dmg += (uStat - tStat);

            if (dmg < 1.0f)
                dmg = 1.0f;
        }

        return dmg;
    }
}
