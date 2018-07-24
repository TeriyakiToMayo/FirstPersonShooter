using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour {
    public Text HPText;
    public Text ArmerText;
    public Text WeaponText;
    public Text InformationText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        HPText.text = "HEALTH " + PlayerState.PlayerHP;
        ArmerText.text = "ARMOR " + PlayerState.PlayerArmor;
        if(PlayerState.CurrentWeapon == 0)
        {
            WeaponText.text = "PISTOL INFINITY";  
        }
        else
        {
            WeaponText.text = "ROCKET " + PlayerState.RocketNum; 
        }

        switch (GameState.CurrentState)
        {
            case GameState.State.START:
                InformationText.text = "PRESS SPACE TO START"; 
                break;
            case GameState.State.INGAME:
                InformationText.text = ""; 
                break;
            case GameState.State.PAUSE:
                InformationText.text = "                PAUSE              ";
                break;
            case GameState.State.END:
                InformationText.text = "       YOU SURVIVED!       ";
                break;
        }

        if(PlayerState.playerCurrentState == PlayerState.PlayerInnerState.DEAD)
        {
            InformationText.text = "         YOU DIED!         ";
        }


    }
}
