using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

[ExecuteInEditMode]
public class Room : MonoBehaviour
{

    public RoomConnection connection_prefab;

    public GameObject weightPref;

    public EnemyEncounter encounter;

    public bool fight;

    public Treasure treasure;

    public bool hasTreasure;

    public Room room_prefab;

    public bool bonfire;

    private List<Room> rooms;

   // private List<Room> connected_rooms;
    private List<LineRenderer> line_renderers;

    [SerializeField] public List<RoomConnection> paths;

    public int rooms_count;

    public int prev_room_count;

    bool started = false;

    private Canvas canvas;

    private void Start()
    {
        if (!Application.isPlaying)
        {
            print("Start ROOOOOM");
            rooms_count = 0;
            canvas = GetComponentInParent<Canvas>();
            prev_room_count = 0;

            if (!started)
            {
                started = true;
                if (rooms == null) rooms = new List<Room>();
                //connected_rooms = new List<Room>();
                if (line_renderers == null) line_renderers = new List<LineRenderer>();
            }
        }
    }

    private void Update()
    {
        //print(rooms.Count + " rooms count");

        while (rooms_count > prev_room_count)
        {
            prev_room_count++;
            canvas = GetComponentInParent<Canvas>();
            var room = Instantiate(room_prefab, new Vector3(0, 0, 0), Quaternion.identity, canvas.transform);

            foreach (Transform child in room.transform)
            {
                DestroyImmediate(child.gameObject);
            }

            room.paths.Clear();
            rooms.Add( room);
            room.Start();
            //2 way connection

            int weight = Random.Range(0, 9);


            // Both connections share the same weight display
            var text_component = Instantiate(weightPref, this.transform);
            var we = text_component.GetComponent<WeightIcon>();

            we.connection1 = room.AddConnection(this, weight, text_component);
            we.connection2 = this.AddConnection(room, weight, text_component);

        }
        

        while(prev_room_count > rooms_count) {

            print("removing at " + (rooms.Count - 1).ToString());
            Room room = rooms[rooms.Count -1];

            rooms.RemoveAt(rooms.Count - 1);
            prev_room_count--;

            DestroyImmediate(room.gameObject);
        
        }


        prev_room_count = rooms_count;


        /* Update all line connections */
        

        //for(int i = 0; i < connected_rooms.Count; i++) {


        //    line_renderers[i]?.SetPosition(1, connected_rooms[i].transform.position);
        //    line_renderers[i]?.SetPosition(0, transform.position);

        //}

    }


    RoomConnection  AddConnection(Room connected_room, int weight, GameObject weightP)
    {
        //GameObject connection = new GameObject("connection");
        //connection.transform.parent = this.transform;

        //var line_renderer = connection.AddComponent<LineRenderer>();

        //line_renderer.SetPosition(0, this.transform.position);

        //line_renderer.SetPosition(1, connected_room.transform.position);


        //line_renderers.Add(line_renderer);
        //connected_rooms.Add(connected_room);
        //print("Added " + connected_room.name);


        var newConnection = GameObject.Instantiate(connection_prefab, canvas.transform);
        newConnection.Initialize(this, connected_room, weight, weightP);
        //connection.transform.parent = this.transform;
        paths.Add(newConnection);

        return newConnection;

    }

    public Room GetNextRoom()
    {
        // Create a list to store cumulative weights
        List<float> cumulativeWeights = new List<float>();
        int totalWeight = 0;

        for(int i = 0; i < paths.Count; ++i)
        {
            if (i != 0) { paths[i].weight.min = paths[i - 1].weight.max +1; }

            totalWeight += paths[i].weight.max;


        }

        int randID = Random.Range(0, totalWeight);

        for (int i = 0; i < paths.Count; ++i)
        {
            int prev_max = 0;
            if(i != 0) { prev_max = paths[i].weight.max; }

            if (randID >= paths[i].weight.min && randID <= paths[i].weight.max + prev_max) {

                int new_weight =  int.Parse(paths[i].weightIcon.label.text) - 8;
                new_weight = Mathf.Clamp(new_weight, 2, 9);

                paths[i].weightIcon.label.text = new_weight.ToString();
               // print("changing weight to " + paths[i].weightIcon.my_weight.max);

                if (paths[i].roomA == this) { return paths[i].roomB; }
                return paths[i].roomA;
            }

        }

        print("Should not come here it's ILLLLEEEEGAAAALLL");

            // Fallback (shouldn't reach here if weights are correctly calculated)
        int j = 0;
        //paths[j].weight.max -= 5;
        //paths[j].weightIcon.my_weight.max -= 5;
        if (paths[j].roomA == this) { return paths[j].roomB; }
        return paths[j].roomA; 

    }


}

