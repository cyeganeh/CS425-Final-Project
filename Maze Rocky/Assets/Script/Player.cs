using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    Animator m_animator;
    [SerializeField] public float speed = 3.2f;
    public GameObject chest;
    public MazeMaker maker;
    public GameController gc;
    public int health;
    public bool fighting;
    private GameObject opponent;
    public GameObject fightmenu;
    public GameObject playerHealth;
    public GameObject oppHealth;
    // Start is called before the first frame update
    void Start()
    {
        health = 100;
        m_animator = GetComponent<Animator>();
        fighting = false;
        fightmenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.eulerAngles.x != 0 || transform.eulerAngles.z != 0)
        {
            transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        }

        
        if (!gc.running) return;
        if (Vector3.Distance(transform.position, chest.transform.position) < 1)
            SceneManager.LoadScene("end");
        float run = (Input.GetKey(KeyCode.R) ? 2 : 1);
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 70.0f * speed;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 1.0f * speed;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);

        m_animator.SetBool("Walking", z != 0);
        m_animator.SetFloat("speed", speed * (z < 0 ? -1 : 1));


    }
    public void startBattle(GameObject opp)
    {

        fighting = true;
        opponent = opp;
        transform.LookAt(opp.transform.position);
        fightmenu.SetActive(true);
        gc.running = false;
        (playerHealth.GetComponent<Text>()).text = "Health: " + health;
        (oppHealth.GetComponent<Text>()).text = "Opponent Health: " + opponent.GetComponent<Enemy>().health;
        m_animator.SetBool("Walking", false);
    }
    //rock=0, paper=1, scissors=2
    public void rock()
    {

        int opponentMove = (int)Random.Range(0, 3);
        if (opponentMove == 1)
        {
            opponent.GetComponent<Animator>().SetTrigger("Attack");
            m_animator.SetTrigger("Hit");
            health -= 3;
            if (health < 0)
                SceneManager.LoadScene("Dead");
        }
        else if (opponentMove == 2)
        {

            opponent.GetComponent<Animator>().SetTrigger("Hit");
            m_animator.SetTrigger("Attacking");
            opponent.GetComponent<Enemy>().health -= 3;
            if (opponent.GetComponent<Enemy>().health <= 0)
            {
                Destroy(opponent);
                fighting = false;
                fightmenu.SetActive(false);
                gc.running = true;
            }
        }
        (playerHealth.GetComponent<Text>()).text = "Health: " + health;
        (oppHealth.GetComponent<Text>()).text = "Opponent Health: " + opponent.GetComponent<Enemy>().health;

    }
    public void paper() { }
    public void scissors() { }

}