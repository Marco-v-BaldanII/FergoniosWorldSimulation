using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEncounter : MonoBehaviour
{
    // Start is called before the first frame update
    public enum Type
    {
        WEAK,
        STRONG,
        BOSS
    }
    public Type enemyType;
    public int enemyCount;
    [SerializeField]
    private Player player;
    class Enemy
    {
        public int amount;
        public string name;
        public int hp;
        public int attack;
        public int currentHP;

        public Enemy(int amount, Type type)
        {
            this.amount = amount;
            switch (type)
            {
                case Type.WEAK:
                    hp = 5;
                    currentHP = 5;
                    name = "Goblin";
                    attack = 3;
                    break;
                case Type.STRONG:
                    hp = 10;
                    currentHP = 10;
                    name = "Orc";
                    attack = 5;
                    break;
                case Type.BOSS:
                    hp = 100;
                    currentHP = 100;
                    name = "Dark Knight";
                    attack = 20;
                    break;
            }
        }
    };
    int turn = 0;
    Enemy fight;
    bool fighting = true;

    void Start()
    {
    
    }

    bool CheckForDeaths()
    {
        if(fight.amount <= 0 || player.hp <= 0 )
        {
            return true;
        }
        else return false;
    }

    void Turn(Player player)
    {
        string plural = "s";
        string deaths = ".";
        int enemyDamage = fight.amount * fight.attack;
        int playerDamage = player.attack;
        int kills = 0;
        player.hp -= enemyDamage;
        if(fight.amount == 1)
        {
            plural = "";
        }
        Debug.Log(fight.amount + " " + fight.name + plural + " hurt player for " + enemyDamage + " damage.");
        if(CheckForDeaths())
        {
            Debug.Log("Player was killed");
            fighting = false;
            return;
        }
        for(int i = playerDamage; i > 0; i--)
        {
            fight.currentHP--;
            if(fight.currentHP == 0)
            {
                fight.amount--;
                fight.currentHP = fight.hp;
                kills++;
                if(CheckForDeaths())
                {
                    Debug.Log("Player dealt " + (playerDamage - i) + " damage to the " + fight.name + plural + ", killing" + kills + " of them and winning the fight.");
                    fighting = false;
                    return;
                }
            }
        }
        if(kills > 0)
        {
            deaths = (" and killed " + kills + " of them.");
        }
        Debug.Log("Player dealt " + playerDamage + "to the" + fight.name + plural + deaths);
    }


    public void InitiateFight()
    {
        fight = new Enemy(enemyCount, enemyType);
        fighting = true;
        while (fighting)
        {
            turn++;
            Debug.Log("Turn " + turn + " begins:");
            Turn(player);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
}
