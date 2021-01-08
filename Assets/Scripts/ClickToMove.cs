using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClickToMove : MonoBehaviour
{
    [Header("Stats")]
    public float attackDistance;
    public float attackRate; //fast
    private float nextAttack;

    //NavMesh
    private NavMeshAgent navMeshAgent;
    private Animator anim;

    //Enemy
    private Transform targetedEnemy;
    private bool enemyClicked;
    private bool walking;

    //Object
    private Transform clickedObject;
    private bool objectClicked;

    //Double click
    private bool oneClick;
    private bool doubleClick;
    private float timerforDoubleClick;
    private float delay = 0.25f;

    void Awake()
    {
        anim = GetComponent<Animator>();
        //anim = GetComponentInChildren<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //클릭한 곳 저장
        RaycastHit hit;

        CheckDoubleClick();

        if (Input.GetButtonDown("Fire2")) //마우스 우클릭
        {
            navMeshAgent.ResetPath();
            if(Physics.Raycast(ray, out hit, 1000)) //클릭한 거리 이내 물체 충돌 감지 , 물리 사용
            {
                if(hit.collider.tag == "Enemy")
                {
                    targetedEnemy = hit.transform;
                    enemyClicked = true;
                    //print("ENEMY HITTED");
                }
                else if (hit.collider.tag == "Chest")
                {
                    objectClicked = true;
                    clickedObject = hit.transform;
                }
                else if (hit.collider.tag == "Info")
                {
                    objectClicked = true;
                    clickedObject = hit.transform;
                }
                else
                {
                    walking = true;
                    enemyClicked = false;
                    navMeshAgent.isStopped = false; //움직임
                    navMeshAgent.destination = hit.point;
                }
            }
        }
        if (enemyClicked && doubleClick)
        {
            MoveAndAttck();
        }
        else if (enemyClicked)
        {
            //select enemy
        }
        else if (objectClicked && clickedObject.gameObject.tag =="Info")
        {
            ReadInfos(clickedObject);
        }
        else if (objectClicked && clickedObject.gameObject.tag =="Chest")
        {
            OpenChest(clickedObject);
        }
        else
        {
            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance) //도착시 정지
            {
                walking = false;
            }
            else if(!navMeshAgent.pathPending && navMeshAgent.remainingDistance >= navMeshAgent.stoppingDistance)
            {
                walking = true;
            }
        }

        

        anim.SetBool("isWalking", walking);
    }

    void MoveAndAttck()
    {
        if(targetedEnemy == null)
        {
            return;
        }

        navMeshAgent.destination = targetedEnemy.position; //클릭방향으로

        if(!navMeshAgent.pathPending && navMeshAgent.remainingDistance > attackDistance) //물체와의  거리 >= 공격사정거리
        {
            navMeshAgent.isStopped = false;
            walking = true;
        }
        else if(!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= attackDistance)
        {
            anim.SetBool("isAttacking", false);
            transform.LookAt(targetedEnemy);
            Vector3 dirToAttack = targetedEnemy.transform.position - transform.position;

            if(Time.time > nextAttack)
            {
                nextAttack = Time.time + attackRate;
                anim.SetBool("isAttacking", true);
            }
            navMeshAgent.isStopped = true;
            walking = false;
        }
    }

    void ReadInfos(Transform target)
    {
        //set target
        navMeshAgent.destination = target.position;
        //go close
        if(!navMeshAgent.pathPending && navMeshAgent.remainingDistance > attackDistance)
        {
            navMeshAgent.isStopped = false;
            walking = true;
        }
        //then read
        else if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= attackDistance)
        {
            navMeshAgent.isStopped = true;
            transform.LookAt(target);
            walking = false;

            //print on info
            print(target.GetComponent<Infos>().info);
            objectClicked = false;
            navMeshAgent.ResetPath();
        }
    }

    void OpenChest(Transform target)
    {
        //set target
        navMeshAgent.destination = target.position;
        //go close
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance > attackDistance)
        {
            navMeshAgent.isStopped = false;
            walking = true;
        }
        //then read
        else if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= attackDistance)
        {
            navMeshAgent.isStopped = true;
            transform.LookAt(target);
            walking = false;

            //play animation
            target.gameObject.GetComponentInChildren<Animator>().SetTrigger("Play");

            //print(target.GetComponent<Infos>().info);
            objectClicked = false;
            navMeshAgent.ResetPath();
        }
    }

    void CheckDoubleClick()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (!oneClick)
            {
                oneClick = true;
                timerforDoubleClick = Time.time;
            }
            else
            {
                oneClick = false;
                doubleClick = true;
            }
        }

        if (oneClick)
        {
            if((Time.time - timerforDoubleClick) > delay)
            {
                oneClick = false;
                doubleClick = false;
            }
        }
    }
}
