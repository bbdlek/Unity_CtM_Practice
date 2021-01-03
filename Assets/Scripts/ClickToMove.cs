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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //Ŭ���� �� ����
        RaycastHit hit;

        if (Input.GetButtonDown("Fire2")) //���콺 ��Ŭ��
        {
            if(Physics.Raycast(ray, out hit, 1000)) //Ŭ���� �Ÿ� �̳� ��ü �浹 ���� , ���� ���
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
                    navMeshAgent.isStopped = false; //������
                    navMeshAgent.destination = hit.point;
                }
            }
        }
        if (enemyClicked)
        {
            MoveAndAttck();
        }

        if(navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance) //������ ����
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

        navMeshAgent.destination = targetedEnemy.position; //Ŭ����������

        if(navMeshAgent.remainingDistance > attackDistance) //��ü����  �Ÿ� >= ���ݻ����Ÿ�
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
