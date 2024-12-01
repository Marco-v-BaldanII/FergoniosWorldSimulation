using System.Collections;
using System.Collections.Generic;
using System.IO; // For file operations
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Room current_room;
    public Room spawn_room;

    int xp = 0;

    public float step_time = 0.001f;

    public int maxhp = 200;
    public int hp;
    public int attack = 20;
    bool canMove;
    public bool has_key = false;

    private string csvPath;

    bool died = false;

    bool firstTime = true;

    public int num_fight = 1;

    void Die()
    {
        canMove = false;
        current_room = spawn_room;
        transform.position = new Vector3(current_room.transform.position.x, current_room.transform.position.y, current_room.transform.position.z - 2);
        hp = maxhp;
        if (! firstTime) died = true; firstTime = false;
        Debug.Log("Player respawed");
        canMove = true;
    }

    void Start()
    {
        // Initialize CSV file
        csvPath = (Application.dataPath + "simulation_data.csv");
        InitializeCSV();

        StartCoroutine("Simulation");
        hp = maxhp;
    }

    void LevelUp()
    {
        while(xp > 30)
        {
            xp -= 30;
            attack += 5;
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

            num_fight++;

            current_room.fight = false;
        }
    }

    void CheckBonfire()
    {
        if (current_room.bonfire)
        {
            spawn_room = current_room;
            hp = maxhp;
            Debug.Log("Player rested at a bonfire and recovered HP");
        }
    }

    void CheckKey()
    {
        if (current_room.hasKey)
        {
            has_key = true;
            print("The player found a key");
        }
    }

    void CheckTreasure()
    {
        string text = " NOTHING LMAO";
        if (current_room.treasure && current_room.hasTreasure)
        {
            switch (current_room.treasure.treasureType)
            {
                case Treasure.Type.ATK:
                    attack += 10;
                    text = " attack being increased by 5.";
                    break;
                case Treasure.Type.HP:
                    maxhp += 40;
                    hp += 40;
                    text = " max HP being increased by 20.";
                    break;
                case Treasure.Type.HEAL:
                    hp += maxhp / 2;
                    if (hp > maxhp)
                    {
                        hp = maxhp;
                    }
                    text = " health being restored by 50%.";
                    break;
            }
            Debug.Log("Player found a treasure which resulted in" + text);
        }
        current_room.hasTreasure = false;
    }

    void Update()
    {
        canMove = false;

        if (hp <= 0)
        {
            Die();
        }

        CheckFight();
        CheckBonfire();
        CheckTreasure();
        CheckKey();
        canMove = true;
    }

    int step = 1;

    private IEnumerator Simulation()
    {
       

        while (true)
        {
            // Move to the room with the lowest weight
            if (hp > 0)
            {
                current_room = current_room.GetNextRoom(has_key);
                current_room.visited = true;
                transform.position = current_room.transform.position;

                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 2);

                // Log data to CSV
                LogToCSV();
                step++;
            }
            else
            {
                Die();
            }
            yield return new WaitForSecondsRealtime(step_time);
        }
    }

    private void InitializeCSV()
    {
        // Overwrite the file by recreating it
        using (StreamWriter writer = new StreamWriter(csvPath, false)) // false ensures the file is overwritten
        {
            writer.WriteLine("Step,HP,MaxHP,Attack,HasKey,CurrentRoom,Death");
        }
    }

    private void LogToCSV()
    {
        using (StreamWriter writer = new StreamWriter(csvPath, true))
        {
            writer.WriteLine($"{step},{hp},{maxhp},{attack},{has_key},{current_room.name},{died}");
            if (died)
            {
                died = false;
            }

        }
    }

}