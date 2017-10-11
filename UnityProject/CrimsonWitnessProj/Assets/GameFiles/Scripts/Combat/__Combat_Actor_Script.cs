using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class __Combat_Actor_Script : MonoBehaviour {

    [Header ("DeterministicStats")]
    [SerializeField]
    private int level = 1;
    private float baseEn = 10.0f;
    private float baseAg = 10.0f;

    [Header("DisplayInfo")]
    [SerializeField]
    private Vector3 cameraOffset = new Vector3(0,0,0);

    [Header("External References")]
    [SerializeField]
    private Animation anim;

    // Runtime Stats // 
    private float currentHP;

    // Buff/Debuffs
    private float AgBuff = 0.0f;

    // Misc
    public int teamIndex = 0; // 0 = Player, 1 = Enemy
    private Vector3 Home;

    private void Start()
    {
        Home = this.transform.position;
        setPosToHome();

        // Set Runtime Stats
        currentHP = getHP(); // TODO: Load player character's HP and MP from the player's data instead. Be sure to save it out at the end too!
        // TODO: MP Too
    }

    /*  Stat Getters  */
    public float getHP()
    {
        return (level + baseEn) * 6;
    }

    public float getAg()
    {
        return baseAg * AgBuff;
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
}
