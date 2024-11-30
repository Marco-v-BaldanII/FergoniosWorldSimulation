using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

[ExecuteInEditMode]
public class Room : MonoBehaviour
{

    public RoomConnection connection_prefab;

    public Room room_prefab;

    private List<Room> rooms;

   // private List<Room> connected_rooms;
    private List<LineRenderer> line_renderers;

    public List<RoomConnection> paths;

    public int rooms_count;

    public int prev_room_count;

    bool started = false;

    private void Start()
    {
        rooms_count = 0;

        prev_room_count = 0;

        if (!started)
        {
            started = true;
            rooms = new List<Room>();
            //connected_rooms = new List<Room>();
            line_renderers = new List<LineRenderer>();
        }
    }

    private void Update()
    {
        //print(rooms.Count + " rooms count");

        while (rooms_count > prev_room_count)
        {
            prev_room_count++;
            var room = Instantiate(room_prefab, new Vector3(0, 0, 0), Quaternion.identity);
            rooms.Add( room);
            room.Start();
            //2 way connection
            room.AddConnection(this);
            this.AddConnection( room );
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


    public void AddConnection(Room connected_room)
    {
        //GameObject connection = new GameObject("connection");
        //connection.transform.parent = this.transform;

        //var line_renderer = connection.AddComponent<LineRenderer>();

        //line_renderer.SetPosition(0, this.transform.position);

        //line_renderer.SetPosition(1, connected_room.transform.position);


        //line_renderers.Add(line_renderer);
        //connected_rooms.Add(connected_room);
        //print("Added " + connected_room.name);


        var newConnection = GameObject.Instantiate(connection_prefab);
        newConnection.Initialize(this, connected_room);
        //connection.transform.parent = this.transform;
        paths.Add(newConnection);

    }


}

