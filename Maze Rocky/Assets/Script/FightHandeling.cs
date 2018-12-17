using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightHandeling : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public void rock()
    {
        player.GetComponent<Player>().rock();
    }
    public void paper()
    {
        player.GetComponent<Player>().paper();
    }
    public void scissors()
    {
        player.GetComponent<Player>().scissors();
    }
}
