using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour {
    public enum State {START, INGAME, PAUSE, END}; 
    public static State CurrentState; 
	// Use this for initialization
	void Start () {
        CurrentState = State.START; 
	}
	
	// Update is called once per frame
	void Update () {
        switch (CurrentState)
        {
            case State.START:
                if (Input.GetKey(KeyCode.Space))
                {
                    CurrentState = State.INGAME;
                }
                break;
            case State.INGAME:
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    Cursor.lockState = CursorLockMode.None;
                    CurrentState = State.PAUSE;
                }
                break;
            case State.PAUSE:
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    CurrentState = State.INGAME; 
                }
                break;
            case State.END: 
                break;
        }
	}
}
