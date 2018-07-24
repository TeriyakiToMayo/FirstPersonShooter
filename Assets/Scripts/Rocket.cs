using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    float timer = 0;
    float age = 5f;
    float speed = 30f;
    int rocketDamage = 50;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(timer >= age)
        {
            Destroy(gameObject); 
        }
        this.transform.position += transform.forward * speed * Time.deltaTime;
        timer += (float)System.Math.Round(Time.deltaTime, 2);
	}

    float explodeDist = 5f;
    public void ProcessCollision(Collision collision)
    {
        
        GameObject colObj = collision.collider.gameObject;
        string name = colObj.name;
        if (name.Equals("Plane") ||
            name.Equals("Cube(Clone)") ||
            name.Equals("Potato(Clone)") ||
            name.Equals("Grape(Clone)"))
        {
        //    Debug.Log("Name = " + name); 
            Destroy(gameObject);
            for (int i = 0; i < MazeGeneration.EnemyList.Count; i++)
            {
                GameObject enemy = (GameObject)MazeGeneration.EnemyList[i];
                if(enemy != null)
                {
                    Vector3 dist = enemy.transform.position - transform.position;
                    if (dist.magnitude <= explodeDist)
                    {
                        EnemyState state = enemy.GetComponent<EnemyState>();
                        state.enemyHP -= rocketDamage;

                        enemy.GetComponent<Rigidbody>().AddForce(dist.normalized * 30, ForceMode.Impulse); 
                    }
                }
                
            }
        }
        
    }
}
