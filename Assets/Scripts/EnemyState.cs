using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : MonoBehaviour {
    public enum EnemyInnerState { ALIVE, DEAD };
    public EnemyInnerState enemyCurrentState;
    public int enemyHP;
    int maxHP = 100;

    // Use this for initialization
    void Start () {
        enemyCurrentState = EnemyInnerState.ALIVE; 
        enemyHP = maxHP;
	}
	
	// Update is called once per frame
	void Update () {
		if(enemyHP <= 0)
        {
            enemyHP = 0;
            enemyCurrentState = EnemyInnerState.DEAD;
            Renderer ren = GetComponent<Renderer>();
            ren.material.color = Color.red; 
        }
	}
}
