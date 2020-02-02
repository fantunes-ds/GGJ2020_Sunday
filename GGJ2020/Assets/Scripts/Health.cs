﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    // Start is called before the first frame update

    public BrokentObjectManager manager;

    float scaleX;
    void Start()
    {
        scaleX = GetComponent<RectTransform>().localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        if(manager.damage >= 100.0f)
        {
            GetComponent<RectTransform>().localScale = new Vector3(0, GetComponent<RectTransform>().localScale.y, GetComponent<RectTransform>().localScale.z);
        }
        else
        {
            GetComponent<RectTransform>().localScale = new Vector3(scaleX - (manager.damage / scaleX), GetComponent<RectTransform>().localScale.y, GetComponent<RectTransform>().localScale.z);
        }
    }
}