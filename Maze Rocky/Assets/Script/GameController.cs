using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MazeMaker))]

public class GameController : MonoBehaviour
{
    private MazeMaker maker;
    public GameObject player;
    public GameObject[] enemies;

    public GameObject menu;
    public GameObject en;
    public bool running;
    // Start is called before the first frame update
    void Start()
    {
        running = true;
        maker = GetComponent<MazeMaker>();
        maker.gc = this;
        maker.makeMaze(35, 35);
        enemies = new GameObject[10];
        player.GetComponent<Player>().maker = maker;
        player.GetComponent<Player>().gc = this;
        
        for (int i = 0; i < 10; i++)
        {
            Destroy(enemies[i]);
            int posr = 0;
            int posc = 0;
            do
            {
                posr = Random.Range(2, maker.data.GetUpperBound(1) - 2);
                posc = Random.Range(2, maker.data.GetUpperBound(1) - 2);
            } while (maker.data[posr, posc] == 1);
            Vector3 pos = new Vector3(posc * maker.width, 0.5f, posr * maker.width);
            enemies[i] = (GameObject)Instantiate<GameObject>(en, pos, Quaternion.identity);
            enemies[i].name = "Skel" + i.ToString();
            enemies[i].GetComponent<Enemy>().maker = maker;
            enemies[i].GetComponent<Enemy>().player = player;
            enemies[i].GetComponent<Enemy>().gc = this;
            enemies[i].GetComponent<Enemy>().self = enemies[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            running = !running;
            menu.SetActive(!running);
        }
        if (Input.GetKeyDown("f1"))
        {
            for (int i = 0; i < 10; i++)
            {
                Destroy(enemies[i]);
                int posr = 0;
                int posc = 0;
                do
                {
                    posr = Random.Range(2, maker.data.GetUpperBound(1) - 2);
                    posc = Random.Range(2, maker.data.GetUpperBound(1) - 2);
                } while (maker.data[posr, posc] == 1);
                Vector3 pos = new Vector3(posc * maker.width, 0.5f, posr * maker.width);
                enemies[i] = (GameObject)Instantiate<GameObject>(en, pos, Quaternion.identity);
                enemies[i].name = "Skel" + i.ToString();
                enemies[i].GetComponent<Enemy>().maker = maker;
                enemies[i].GetComponent<Enemy>().player = player;
                enemies[i].GetComponent<Enemy>().gc = this;
                enemies[i].GetComponent<Enemy>().self = enemies[i];
            }
            Destroy(maker.g);
            maker.makeMaze(35, 35);
            
            player.transform.position=new Vector3(3f, 0f, 3f);
            player.transform.rotation = Quaternion.FromToRotation(new Vector3(), Vector3.up);
            
        }
    }
}
