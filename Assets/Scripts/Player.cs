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
    bool canMove;

    void Die()
    {
        canMove = false;
        current_room = spawn_room;
        hp = maxhp;
        Debug.Log("Player respawed");
        canMove = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Simulation");
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

    void CheckTreasure()
    {
        string text = " NOTHING LMAO";
        if(current_room.treasure && current_room.hasTreasure)
        {
            switch(current_room.treasure.treasureType)
            {
                case Treasure.Type.ATK:
                    attack += 5;
                    text = " attack being increased by 5.";
                    break;
                case Treasure.Type.HP:
                    maxhp += 20;
                    hp += 20;
                    text = " max HP being increased by 20.";

                    break;
                case Treasure.Type.HEAL:
                    hp += maxhp / 2;
                    if (hp>maxhp)
                    {
                        hp=maxhp;

                    }
                    text = " health being restored by 50%.";
                    break;
            }
            Debug.Log("Player found a treasure which resulted in" + text);
        }
        current_room.hasTreasure = false;
    }

    // Update is called once per frame
    void Update()
    {
        canMove = false;
        if (hp < 0)
        {
            Die();
        }
        CheckFight();
        CheckBonfire();
        CheckTreasure();
        canMove = true;
    }


    private IEnumerator Simulation()
    {
        while(true)
        {
            // move to room with lowest weight
            current_room = current_room.GetNextRoom();

            transform.position = current_room.transform.position;

            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 2);

            yield return new WaitForSecondsRealtime(step_time);



        }


    }
}
