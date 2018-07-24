using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostFlameAI : MonoBehaviour {
    public GameObject Player;
    enum EnemyState { SEARCH, CHASE, ATTACK, DEAD }
    EnemyState state;

    double timer = 0;
    double currentMaxTime = 0;
    double maxTime = 2;
    // Use this for initialization
    void Start()
    {
        state = EnemyState.SEARCH;
        ver = VertDisDrt.UP;
    }

    Vector3 investigateSpot;
    float playerHeight = .5f;
    float sightDist = 10;
    int rayDensity = 10;
    int maxDegree = 45;

    float speed = 5f;

    float vertSpeed = 0.75f;
    float currentVertDis = 0; 
    float maxVerticalDis = 0.4f;  
    enum VertDisDrt { UP, DOWN};
    VertDisDrt ver; 

    // Update is called once per frame
    void Update()
    {
        if(currentVertDis > maxVerticalDis || currentVertDis < -maxVerticalDis)
        {
            if(vertSpeed > 0)
            {
                currentVertDis = maxVerticalDis;
            }
            else
            {
                currentVertDis = -maxVerticalDis;
            }
            vertSpeed = -vertSpeed;
        }
        else
        {
            float origin = (float)System.Math.Round(transform.position.y - currentVertDis, 2); 
            currentVertDis = (float)System.Math.Round(currentVertDis + vertSpeed * Time.deltaTime, 2);
            transform.position = new Vector3(transform.position.x, (float)System.Math.Round(origin + currentVertDis, 2), transform.position.z); 

        }

        switch (state)
        {
            case EnemyState.SEARCH:
                if (PlayerIsInSight())
                {
                    timer = currentMaxTime;
                    state = EnemyState.CHASE;
                    break;
                }
                if (timer >= currentMaxTime)
                {
                    timer = 0;
                    currentMaxTime = 1 + System.Math.Round(Random.value, 2) * maxTime;
                    transform.LookAt(Quaternion.AngleAxis((float)System.Math.Round(Random.value, 2) * 360, Vector3.up) * (Vector3.forward + new Vector3(0, transform.position.y, 0)));
                }
                else
                {
                    timer += Time.deltaTime;
                    transform.position += transform.forward * speed * Time.deltaTime;
                }
                break;
            case EnemyState.CHASE:
                if (!PlayerIsInSight())
                {
                    state = EnemyState.SEARCH;
                    break;
                }
                transform.LookAt(new Vector3(Player.transform.position.x, transform.position.y, Player.transform.position.z)); 
                transform.position += transform.forward * speed * Time.deltaTime;
                break;
        }


    }

    bool PlayerIsInSight()
    {
        bool canSeePlayer = false;
        for (int i = 0; i < rayDensity + 1; i++)
        {
            Vector3 forward = new Vector3(transform.forward.x, transform.forward.y, transform.forward.z);
            Vector3 left = Quaternion.AngleAxis(-maxDegree * i / rayDensity, Vector3.up) * forward;
            Vector3 right = Quaternion.AngleAxis(maxDegree * i / rayDensity, Vector3.up) * forward;
            Vector3 position = new Vector3(transform.position.x, 0, transform.position.z);
            Debug.DrawRay(position + Vector3.up * playerHeight, left * sightDist, Color.blue);
            Debug.DrawRay(position + Vector3.up * playerHeight, right * sightDist, Color.blue);

            if (VisionControl(left) || VisionControl(right))
            {
                canSeePlayer = true;
            }

        }

        return canSeePlayer;
    }

    bool VisionControl(Vector3 ray)
    {
        RaycastHit hit;
        bool canSeePlayer = false;
        Vector3 position = new Vector3(transform.position.x, 0, transform.position.z);
        if (Physics.Raycast(position + Vector3.up * playerHeight, ray, out hit, sightDist))
        {
            if (hit.collider.gameObject.name.Equals("Player"))
            {
                canSeePlayer = true;
            }
        }

        return canSeePlayer;
    }

    
}
