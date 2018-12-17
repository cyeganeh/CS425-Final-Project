using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Animator m_animator;
    public bool walking;
    public MazeMaker maker;
    public GameObject player;
    public GameObject self;
    public Vector3 vel;
    public Vector3 goal;
    public bool chasing=false;
    public GameController gc;
    public int health;
    // Start is called before the first frame update
    void Start()
    {
        health = 30;
        m_animator = GetComponent<Animator>();
        vel = new Vector3(0, 0, .1f);
        m_animator.SetBool("Walking", true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!gc.running) return;
        
        float dist = Vector3.Distance(player.transform.position, transform.position);
        if (dist < maker.width)
        {
            m_animator.SetBool("Walking", false);
            transform.LookAt(player.transform.position);
            player.GetComponent<Player>().startBattle(self);
        }
        else if ( dist< 20)
        {
            m_animator.SetBool("Walking", true);
            chasing = true;
            transform.LookAt(player.transform.position);
            transform.Translate(0, 0, 0.03f);
        }
        else
        {
            m_animator.SetBool("Walking", true);
            if (chasing)
            {
                transform.rotation = Quaternion.Euler(0, 90*((int)Random.Range(0,4)), 0);
                chasing = false;
            }
            int option = (int)(Random.Range(0, 10000));
            if (option < 9995)
                transform.Translate(0, 0, 0.03f);
            else
            {
                transform.Rotate(0, ((int)(Random.Range(1, 3))) * 90, 0);
            }
        }
        

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Plane" || collision.gameObject.name=="Zombie")
            return;
        transform.Translate(0, 0, -0.075f);
        Debug.Log(name + " collision with " + collision.gameObject.name);
        transform.Rotate(0, ((int)(Random.Range(1, 3))) * 90, 0);
    }
}
