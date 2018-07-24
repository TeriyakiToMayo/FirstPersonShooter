﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorAid : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider collider)
    {
        GameObject colObj = collider.gameObject;
        string name = colObj.name;

        if (name.Equals("Player(Clone)") && PlayerState.PlayerArmor < PlayerState.maxArmor)
        {
            Destroy(gameObject); 
            PlayerState.ArmorUP(); 
        }
    }
}
