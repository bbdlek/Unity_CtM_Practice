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

    private NavMeshAgent navMeshAgent;
    private Animator anim;

    private Transform targetedEnemy;
    private bool enemyClicked;
    private bool walking;
    void Awake()
    {
        anim = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //클릭한 곳 저장
        RaycastHit hit;

        if (Input.GetButtonDown("Fire2")) //마우스 우클릭
        {
            if(Physics.Raycast(ray, out hit, 1000)) //클릭한 거리 이내 물체 충돌 감지 , 물리 사용
            {
                if(hit.collider.tag == "Enemy")
                {
                    targetedEnemy = hit.transform;
                    enemyClicked = true;
                    //print("ENEMY HITTED");
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
        if (enemyClicked)
        {
            MoveAndAttck();
        }

        if(navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance) //도착시 정지
        {
            walking = false;
        }
        else
        {
            walking = true;
        }

        //anim.SetBool("isWalking", walking);
    }

    void MoveAndAttck()
    {
        if(targetedEnemy == null)
        {
            return;
        }

        navMeshAgent.destination = targetedEnemy.position; //클릭방향으로

        if(navMeshAgent.remainingDistance > attackDistance) //물체와의  거리 >= 공격사정거리
        {
            navMeshAgent.isStopped = false;
            walking = true;
        }
        else
        {
            transform.LookAt(targetedEnemy);
            Vector3 dirToAttack = targetedEnemy.transform.position - transform.position;

            if(Time.time > nextAttack)
            {
                nextAttack = Time.time + attackRate;
            }
            navMeshAgent.isStopped = true;
            walking = false;
        }
    }
}
