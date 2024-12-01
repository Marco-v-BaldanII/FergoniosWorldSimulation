using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimulationManager : MonoBehaviour
{

    public static SimulationManager Instance { get; private set; }

    public float enemy_amount_multiplier;

    public float player_stat_multiplier;

    public Player player;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        using (StreamWriter writer = new StreamWriter(Application.dataPath + "run_summary.csv", false))
        {
            writer.WriteLine("Simulation,EnemiesInRun,DeathsInRun,PlayerStartingHP,PlayerStartingAttack,TreasuresFound,TimeSteps,Beatable");
        }
    }

    public int num_simulations = 10;

    private void Start()
    {
        enemy_amount_multiplier = Random.Range(0.2f, 1.1f);
        player_stat_multiplier = Random.Range(0.3f, 2.0f);
        enemies_in_run = 0;
        deaths_in_run = 0;

        treasures_found = 0;
    }

    public int enemies_in_run = 0;

    public int deaths_in_run = 0;

    public int player_starting_hp = 0;

    public int player_starting_attack = 0;

    public int treasures_found = 0;

    public void WriteRunSummaryToCSV(bool beatable = true)
    {
        string filePath = Application.dataPath + "run_summary.csv";

        // Check if the file exists
        bool fileExists = File.Exists(filePath);

        using (StreamWriter writer = new StreamWriter(filePath, true)) // Append = true
        {
            writer.WriteLine($"{num_simulations},{enemies_in_run},{deaths_in_run},{player_starting_hp},{player_starting_attack},{treasures_found},{player.step},{beatable}");
        }

        Debug.Log("Run summary written to CSV!");

        Start();

        SceneManager.LoadScene(0);

        num_simulations--;
    }
}
