using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public Room current_room;
    public Room spawn_room;
    public float step_time = 0.4f;

    public int maxhp = 200;
    public int hp;
    public int attack = 20;

    void Die()
    {
        current_room = spawn_room;
        hp = maxhp;
        Debug.Log("Player respawed");
    }

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine("Simulation");
        hp = maxhp;
    }

    void CheckFight()
    {
        if (current_room.encounter != null && current_room.fight)
        {
            current_room.encounter.InitiateFight();
            current_room.fight = false;
        }
    }

    void CheckBonfire()
    {
        if(current_room.bonfire)
        {
            spawn_room = current_room;
            hp = maxhp;
            Debug.Log("Player rested at a bonfire and recovered HP");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (hp < 0)
        {
            Die();
        }
        CheckFight();
        CheckBonfire();
    }


    private IEnumerator Simulation()
    {
        while(true)
        {
            // move to room with lowest weight
            current_room = current_room.GetNextRoom();

            transform.position = current_room.transform.position;

            yield return new WaitForSecondsRealtime(step_time);



        }


    }
}
