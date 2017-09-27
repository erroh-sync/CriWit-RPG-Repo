﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class __List_Script : MonoBehaviour {
    public string[] names = new string[8];
    public int visibleButtonCount = 4;
    private List<GameObject> spawnedButtons = new List<GameObject>();
    public GameObject buttonPrefab;
    public float speration = 32.0f;

    private int currentTop = 0;
    private int position = 0;

    private int storedInput = 0;
    private float timer = 0.0f;
    public float intervalTime = 0.2f;

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        Control();
    }

    private void Init()
    {
        for(int i = 0; i < visibleButtonCount; i++)
        {
            spawnedButtons.Add(Instantiate(buttonPrefab, this.transform) as GameObject);
            spawnedButtons[i].transform.localPosition = new Vector3(0, -i * speration, 0);
        }
        spawnedButtons[0].GetComponent<__List_Button>().setSelected();
        SetButtonNames();
    }

    private void SetButtonNames()
    {
        for (int i = 0; i < visibleButtonCount; i++)
        {
            if (names.Length > currentTop + i)
                spawnedButtons[i].GetComponent<__List_Button>().setNameString(names[currentTop + i]);
            else
                spawnedButtons[i].GetComponent<__List_Button>().setNameString("---");
        }
    }

    private void Control()
    {
        int newI = 0;
        if (Input.GetAxis("Vertical") > 0.5) { newI = 1; } else if (Input.GetAxis("Vertical") < -0.5) { newI = -1; }
        if (newI != storedInput)
        {
            storedInput = newI;
        }

        if (timer <= 0.0f && storedInput != 0)
        {
            position -= storedInput;
            
            if (position < 0)
                position = names.Length - 1;
            else if (position > names.Length - 1)
                position = 0;

            
            if (position > currentTop + (visibleButtonCount - 1))
            {
                currentTop = position - (visibleButtonCount - 1);
                SetButtonNames();
            }
            else if (position < currentTop)
            {
                currentTop = position;
                SetButtonNames();
            }

            spawnedButtons[position - currentTop].GetComponent<__List_Button>().setSelected();
            
            timer = intervalTime;
        }
        else if (storedInput == 0)
            timer = 0.0f;
    }
}