using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour {
    public enum PlayerInnerState { ALIVE, DEAD};
    public static PlayerInnerState playerCurrentState;
    public static int PlayerHP;
    public static int PlayerArmor;
    public static int RocketNum;
    public static int CurrentWeapon;
    public static int maxHP = 100;
    public static int maxArmor = 100;  

    // Use this for initialization
    void Start () {
        playerCurrentState = PlayerInnerState.ALIVE;
        PlayerHP = maxHP;
        PlayerArmor = 0; 
        RocketNum = 10;
    }
	
	// Update is called once per frame
	void Update () {
        if (GameState.CurrentState == GameState.State.END)
        {
            Rigidbody rb = GetComponent<Rigidbody>(); 
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        if (PlayerHP <= 0)
        {
            PlayerHP = 0;
            playerCurrentState = PlayerInnerState.DEAD; 
            
            Rigidbody rb = GetComponent<Rigidbody>(); 
            rb.constraints = RigidbodyConstraints.None;

            Renderer ren = GetComponent<Renderer>();
            ren.material.color = Color.red; 
        }
    }

    public static void TakeDamage(int damage)
    {
        if (PlayerArmor >= damage)
        {
            PlayerArmor -= damage;
        }
        else
        {
            damage -= PlayerArmor;
            PlayerArmor = 0;
            PlayerHP -= damage; 
        }
    }

    static int healthAid = 50;
    public static void HealthUP()
    {
        PlayerHP += healthAid;
        if(PlayerHP > maxHP)
        {
            PlayerHP = maxHP;
        }
    }

    static int armorAid = 50;
    public static void ArmorUP()
    {
        PlayerArmor += armorAid;
        if (PlayerArmor > maxArmor)
        {
            PlayerArmor = maxArmor;
        }
    }

    public static void RocketUP()
    {
        RocketNum += 5; 
    }

    private void OnCollisionStay(Collision collision)
    {
        GameObject colObj = collision.collider.gameObject;
        string name = colObj.name;

        if (name.Equals("Plane")) 
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.velocity = new Vector3(0, rb.velocity.y, 0); 
        }
    }

}
