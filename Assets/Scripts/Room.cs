using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

[ExecuteInEditMode]
[System.Serializable]
public class Room : MonoBehaviour
{

    public RoomConnection connection_prefab;

    public Room room_prefab;

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
            rooms.Add( room);
            room.Start();
            //2 way connection

            int weight = Random.Range(0, 9);

            room.AddConnection(this, weight);
            this.AddConnection( room, weight );
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


    public void AddConnection(Room connected_room, int weight)
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
        newConnection.Initialize(this, connected_room, weight);
        //connection.transform.parent = this.transform;
        paths.Add(newConnection);

    }

    public Room GetNextRoom()
    {
        // Create a list to store cumulative weights
        List<float> cumulativeWeights = new List<float>();
        float totalWeight = 0f;

        // Calculate cumulative weights
        foreach (RoomConnection path in paths)
        {
            totalWeight += 1f / path.weight; // Invert weight so lower weight is more favorable
            cumulativeWeights.Add(totalWeight);
        }

        // Generate a random value between 0 and the total weight
        float randomValue = Random.Range(0f, 1);

        // Find the selected path based on the random value
        for (int i = 0; i < paths.Count; i++)
        {
            if (randomValue <= cumulativeWeights[i])
            {
                if (paths[i].roomA == this) { return paths[i].roomB; }
                if (paths[i].roomB == this) { return paths[i].roomA; }
            }
        }

        // Fallback (shouldn't reach here if weights are correctly calculated)
        int j = 0;
        if (paths[j].roomA == this) { return paths[j].roomB; }
        return paths[j].roomA; 

    }


}

