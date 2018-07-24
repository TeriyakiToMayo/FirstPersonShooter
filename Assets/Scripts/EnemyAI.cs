using System.Collections; 
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    public GameObject Player;
    enum MovementState { SEARCH, CHASE }
    MovementState state;

    double timer = 0;
    double currentMaxTime = 0;
    double maxTime = 3; 
    // Use this for initialization
    void Start()
    {
        state = MovementState.SEARCH;
    }

    Vector3 investigateSpot;
    float playerHeight = .5f;
    float sightDist = 10;
    int rayDensity = 20;
    int maxDegree = 45;

    public float speed = 4f;

    public int attack = 10; 
    public float attackInterval = .5f;
    float attackTimer = 0;
    public float attackDist = 1.5f;

    bool deadFlag = true;
    float deadTimer = 0;
    float deadMaxTime = 2f;
    // Update is called once per frame
    void Update()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        switch (GameState.CurrentState)
        {
            case GameState.State.START:
                rb.velocity = Vector3.zero;
                goto endOfFrame;
            case GameState.State.INGAME:
                break;
            case GameState.State.PAUSE:
                rb.velocity = Vector3.zero;
                goto endOfFrame;
            case GameState.State.END:
                rb.velocity = Vector3.zero; 
                goto endOfFrame; 
        }

        if(GetComponent<EnemyState>().enemyCurrentState == EnemyState.EnemyInnerState.DEAD)
        {
            if (deadFlag)
            {
                rb.constraints = RigidbodyConstraints.None;
                deadTimer = 0;
                deadFlag = false; 
            }

            deadTimer += (float)System.Math.Round(Time.deltaTime, 2);
            if(deadTimer >= deadMaxTime)
            {
                Destroy(gameObject); 
            }

            goto endOfFrame;
        }

        switch (state)
        {
            case MovementState.SEARCH:
                if (PlayerIsInSight())
                {
                    timer = currentMaxTime;
                    state = MovementState.CHASE;
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
                }
                break;
            case MovementState.CHASE:
                if (!PlayerIsInSight())
                {
                    state = MovementState.SEARCH;
                    break;
                }

                if((Player.transform.position - transform.position).magnitude <= attackDist )
                {
                    if(attackTimer >= attackInterval)
                    {
                        attackTimer = 0;
                        if(PlayerState.playerCurrentState == PlayerState.PlayerInnerState.ALIVE) 
                        {
                            PlayerState.TakeDamage(attack);
                        }
                        
                    }
                    else
                    {
                        attackTimer += (float)System.Math.Round(Time.deltaTime, 2);
                    }
                }
                else
                {
                    attackTimer = 0;
                }

                transform.LookAt(new Vector3(Player.transform.position.x, transform.position.y, Player.transform.position.z));

                break;
        }

        endOfFrame:
        {

        }

    }

    bool PlayerIsInSight()
    {
        bool canSeePlayer = false;
        for (int i = 0; i < rayDensity + 1; i++)
        {
            int currentAngle = 180 * i / rayDensity;
            Vector3 forward = new Vector3(transform.forward.x, transform.forward.y, transform.forward.z);
            Vector3 left = Quaternion.AngleAxis(-currentAngle, Vector3.up) * forward;
            Vector3 right = Quaternion.AngleAxis(currentAngle, Vector3.up) * forward;
            float currentSightDist = sightDist;
            if(currentAngle >= maxDegree)
            {
                currentSightDist /= 2;
            }
            Debug.DrawRay(transform.position + Vector3.up * playerHeight, left * currentSightDist, Color.blue);
            Debug.DrawRay(transform.position + Vector3.up * playerHeight, right * currentSightDist, Color.blue);

            if (VisionControl(left, currentSightDist) || VisionControl(right, currentSightDist ))
            {
                canSeePlayer = true;
            }

        }

        return canSeePlayer;
    }

    bool VisionControl(Vector3 ray, float sight)
    {
        RaycastHit hit;
        bool canSeePlayer = false;
        if (Physics.Raycast(transform.position + Vector3.up * playerHeight, ray, out hit, sight))
        {
            if (hit.collider.gameObject.name.Equals(Player.name))
            {
                canSeePlayer = true; 
            }
        }

        return canSeePlayer; 
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject colObj = collision.collider.gameObject;
        string name = colObj.name; 
        if (name.Equals("Cube(Clone)")) 
        {
            Vector3 normal = collision.contacts[0].normal;
            Vector3 vertNormal = Vector3.Cross(normal, transform.forward);
            Vector3 horiNormal = Vector3.Cross(vertNormal, normal);
            transform.LookAt(new Vector3(transform.position.x + horiNormal.x, transform.position.y, transform.position.z + horiNormal.z)); 
        }
    }

        float jumpSpeed = 2f;
    private void OnCollisionStay(Collision collision)
    {
        GameObject colObj = collision.collider.gameObject;
        string name = colObj.name; 
        if (name.Equals("Plane"))
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * speed, ForceMode.Impulse); 
            rb.AddForce(Vector3.up, ForceMode.Impulse);
        }
    }
}
