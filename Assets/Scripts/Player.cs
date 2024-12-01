using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public Room current_room;
    public Room spawn_room;
    public float step_time = 0.4f;
    int xp = 0;
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

    void LevelUp()
    {
        while(xp > 100)
        {
            xp -= 100;
            attack++;
            maxhp += 5;
            hp += 5;
            Debug.Log("Leveled up! Attack +1, HP +5");
        }
    }

    void CheckFight()
    {
        if (current_room.encounter != null && current_room.fight)
        {
            current_room.encounter.InitiateFight();
            if (hp < 0)
            {
                Die();
                return;
            }
            else
            {
                int _xp = (current_room.encounter.enemyCount * ((int)current_room.encounter.enemyType + 1));
                xp += _xp;
                Debug.Log("Obtained " + _xp + " XP.");
                LevelUp();
            }
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
        }
        Debug.Log("Player found a treasure which resulted in" + text);
        current_room.hasTreasure = false;
    }

    // Update is called once per frame
    void Update()
    {
        canMove = false;
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

            yield return new WaitForSecondsRealtime(step_time);



        }


    }
}
