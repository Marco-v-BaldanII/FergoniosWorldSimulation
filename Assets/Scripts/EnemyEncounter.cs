using System.IO;
using UnityEngine;

public class EnemyEncounter : MonoBehaviour
{
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

    private class Enemy
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
                    hp = 10;
                    currentHP = 10;
                    name = "Goblin";
                    attack = 6;
                    break;
                case Type.STRONG:
                    hp = 20;
                    currentHP = 20;
                    name = "Orc";
                    attack = 10;
                    break;
                case Type.BOSS:
                    hp = 180;
                    currentHP = 180;
                    name = "Dark Knight";
                    attack = 20;
                    break;
            }
        }
    }

    private int turn = 0;
    private Enemy fight;
    private bool fighting = true;
    private string fightCsvPath;

    void Start() { }

    private bool CheckForDeaths()
    {
        return fight.amount <= 0 || player.hp <= 0;
    }

    private void Turn(Player player)
    {
        string plural = fight.amount == 1 ? "" : "s";
        string deaths = ".";
        int enemyDamage = fight.amount * fight.attack;
        int playerDamage = player.attack;
        int kills = 0;

        player.hp -= enemyDamage;
        LogToFightCSV(turn, player.hp, fight.amount, fight.currentHP, $"Player took {enemyDamage} damage");

        if (CheckForDeaths())
        {
            Debug.Log("Player was killed");
            fighting = false;
            return;
        }

        for (int i = playerDamage; i > 0; i--)
        {
            fight.currentHP--;
            if (fight.currentHP == 0)
            {
                fight.amount--;
                fight.currentHP = fight.hp;
                kills++;
                LogToFightCSV(turn, player.hp, fight.amount, fight.currentHP, "Enemy killed");
                if (CheckForDeaths())
                {
                    Debug.Log($"Player dealt {playerDamage - i + 1} damage to the {fight.name}{plural}, killing {kills} of them and winning the fight.");
                    fighting = false;
                    return;
                }
            }
        }

        if (kills > 0)
        {
            deaths = $" and killed {kills} of them.";
        }

        Debug.Log($"Player dealt {playerDamage} damage to the {fight.name}{plural}{deaths}");
        LogToFightCSV(turn, player.hp, fight.amount, fight.currentHP, $"Player dealt {playerDamage} damage");
    }

    public void InitiateFight()
    {
        fight = new Enemy(enemyCount, enemyType);
        fighting = true;

        // Create a new CSV file for this fight
        fightCsvPath = $"fight_{player.num_fight}.csv";
        InitializeFightCSV();

        while (fighting)
        {
            turn++;
            Debug.Log($"Turn {turn} begins:");
            Turn(player);
        }

        Debug.Log("Current Player HP = " + player.hp);
    }

    private void InitializeFightCSV()
    {
        using (StreamWriter writer = new StreamWriter(fightCsvPath, false))
        {
            writer.WriteLine("Turn,PlayerHP,EnemiesRemaining,EnemyCurrentHP,Event");
        }
    }

    private void LogToFightCSV(int turn, int playerHp, int enemiesRemaining, int enemyCurrentHp, string eventDescription)
    {
        using (StreamWriter writer = new StreamWriter(fightCsvPath, true))
        {
            writer.WriteLine($"{turn},{playerHp},{enemiesRemaining},{enemyCurrentHp},{eventDescription}");
        }
    }
}
