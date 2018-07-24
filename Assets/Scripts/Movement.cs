using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    GameObject character;

    public GameObject Rocket;

    Vector2 mouseLook;
    Vector2 smthFix;
    float sensitivity = 3f;
    float smthConst = 2.0f;

    float maxSpeed = 8.0f;
    float minSpeed = 5.0f;
    float jumpSpeed = 5.0f;

    Animator anim;
    Animator animRocket;

    // Use this for initialization
    void Start () {
        character = this.transform.parent.gameObject;
        Cursor.lockState = CursorLockMode.Locked;

        anim = this.transform.GetChild(0).GetComponent<Animator>(); 
        anim.SetInteger("State", 0);

        animRocket = this.transform.GetChild(1).GetComponent<Animator>();
        animRocket.SetInteger("State", 0);
    }

    float deadCameraHeight = 10f;
    float cameraMoveSpeed = 3f;
    Transform deadTransform;
    bool deadFlag = true;
    bool aliveFlag = true; 

    // Update is called once per frame
    void Update () {
        
        switch (GameState.CurrentState)
        {
            case GameState.State.START:
                goto endOfFrame;
            case GameState.State.INGAME:
                break;
            case GameState.State.PAUSE:
                goto endOfFrame;
            case GameState.State.END:
                goto endOfFrame;
        }

        if (PlayerState.playerCurrentState == PlayerState.PlayerInnerState.DEAD)
        {
            if (deadFlag)
            {
                deadTransform = transform.parent.transform; 
                transform.SetParent(null);
                deadFlag = false;
                this.transform.GetChild(0).gameObject.SetActive(false);
                this.transform.GetChild(1).gameObject.SetActive(false); 
            }
            
            transform.LookAt(deadTransform.position); 
            if (transform.position.y < deadCameraHeight) 
            {
                transform.position += Vector3.up * cameraMoveSpeed * Time.deltaTime;
            }
            goto endOfFrame;
        }else if(PlayerState.playerCurrentState == PlayerState.PlayerInnerState.ALIVE)
        {
            if (aliveFlag)
            {
                this.transform.GetChild(0).gameObject.SetActive(true);
                aliveFlag = false;
            }
            
        }

        if (Cursor.lockState == CursorLockMode.Locked)
        {
            /*
             *  The first person shooting camera movement is implemented based on the Holistic3d's tutorial 
             *  "How to construct a simple First Person Controller with Camera Mouse Look in Unity 5"
             *  Retrieved from: https://www.youtube.com/watch?v=blO039OzUZc&t=314s
             **/
            Vector2 mouseMovement = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
            mouseMovement = Vector2.Scale(mouseMovement, new Vector2(sensitivity * smthConst, sensitivity * smthConst));
            smthFix.x = Mathf.Lerp(smthFix.x, mouseMovement.x, 1f / smthConst);
            smthFix.y = Mathf.Lerp(smthFix.y, mouseMovement.y, 1f / smthConst);
            mouseLook += smthFix;
            mouseLook.y = Mathf.Clamp(mouseLook.y, -90f, 90f);
            transform.localRotation = Quaternion.AngleAxis(mouseLook.y, Vector3.left);
            if(character != null)
            {
                character.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, Vector3.up);
            }
        }

        if(Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            bool boolean = this.transform.GetChild(0).gameObject.activeSelf; 
            this.transform.GetChild(0).gameObject.SetActive(!boolean);
            this.transform.GetChild(1).gameObject.SetActive(boolean);
            if (!boolean)
            {
                PlayerState.CurrentWeapon = 0;
            }
            else
            {
                PlayerState.CurrentWeapon = 1; 
            }
        }

        if (transform.position.x >= MazeGeneration.EndPoint.x &&
            transform.position.x <= MazeGeneration.EndPoint.x + MazeGeneration.ExpandConst &&
            transform.position.z >= MazeGeneration.EndPoint.z &&
            transform.position.z <= MazeGeneration.EndPoint.z + MazeGeneration.ExpandConst)
        {
            GameState.CurrentState = GameState.State.END;
            anim.SetInteger("State", 0);
            animRocket.SetInteger("State", 0);
            goto endOfFrame ;
        }


        
        float speed = minSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = maxSpeed;
        }

        if (IsInAir())
        {
            speed = minSpeed / 2; 
            goto movement; 
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Rigidbody rb = GetComponentInParent<Rigidbody>();
                rb.velocity = new Vector3(rb.velocity.x, jumpSpeed, rb.velocity.z);
            }
        }

        movement: {

            bool hasMoved = false;

            if (Input.GetKey(KeyCode.W))
            {
                character.transform.position += character.transform.forward * speed * Time.deltaTime;
                hasMoved = true;
            }

            if (Input.GetKey(KeyCode.A))
            {
                character.transform.position += -character.transform.right * speed * Time.deltaTime;
                hasMoved = true;
            }

            if (Input.GetKey(KeyCode.S))
            {
                character.transform.position += -character.transform.forward * speed * Time.deltaTime;
                hasMoved = true;
            }

            if (Input.GetKey(KeyCode.D))
            {
                character.transform.position += character.transform.right * speed * Time.deltaTime;
                hasMoved = true;
            }

            if (hasMoved)
            {
                anim.SetInteger("State", 1);
                animRocket.SetInteger("State", 1);
            }
            else
            {
                anim.SetInteger("State", 0);
                animRocket.SetInteger("State", 0); 
            }

        }
        

        endOfFrame:
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && GameState.CurrentState == GameState.State.INGAME && attackTimer <= 0) 
            {
                attackTimer = maxAttackInterval;

                if (this.transform.GetChild(0).gameObject.activeSelf)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, transform.forward, out hit, pistolDist))
                    {
                        if (hit.collider.gameObject.name.Equals("Potato(Clone)") ||
                            hit.collider.gameObject.name.Equals("Grape(Clone)"))
                        {
                            GameObject enemy = hit.collider.gameObject;
                            EnemyState state = enemy.GetComponent<EnemyState>();
                            state.enemyHP -= pistolDamage;
                            Rigidbody rb = enemy.GetComponent<Rigidbody>();
                            rb.AddForce(transform.forward * 10, ForceMode.Impulse);
                        }
                    }
                }
                else
                {
                    if(PlayerState.RocketNum > 0)
                    {
                        GameObject rocketLauncher = this.transform.GetChild(0).gameObject;
                        GameObject newRocket = Instantiate(Rocket, rocketLauncher.transform.position + rocketLauncher.transform.forward, Quaternion.identity);
                        newRocket.transform.LookAt(newRocket.transform.position + transform.forward);
                        Rigidbody rb = newRocket.GetComponent<Rigidbody>();

                        newRocket.SetActive(true);
                        PlayerState.RocketNum--; 
                    }
                    
                }
                

                anim.SetInteger("State", 2);
                animRocket.SetInteger("State", 2); 
            }

            if(attackTimer > 0)
            {
                attackTimer -= Time.deltaTime;
            }
        }
    }

    float pistolDist = 20f;
    int pistolDamage = 20;
    float attackTimer = 0;
    float maxAttackInterval = 0.3f; 

    bool IsInAir()
    {
        Rigidbody rb = GetComponentInParent<Rigidbody>();
        return !(rb.velocity.y <= 0.001f && rb.velocity.y >= -0.001f); 
    }

}
