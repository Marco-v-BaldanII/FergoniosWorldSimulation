using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

[ExecuteInEditMode]
public class Room : MonoBehaviour
{

    public Room room_prefab;

    private List<Room> rooms;

    private List<Room> connected_rooms;
    private List<LineRenderer> line_renderers;

    public int rooms_count;

    public int prev_room_count;

    private void Update()
    {
        //print(rooms.Count + " rooms count");

        while (rooms_count > prev_room_count)
        {
            prev_room_count++;
            var room = Instantiate(room_prefab, new Vector3(0, 0, 0), Quaternion.identity, this.transform);
            rooms.Add( room);
            room.AddConnection(this);
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
        

        for(int i = 0; i < connected_rooms.Count; i++) {

            line_renderers[i]?.SetPosition(1, connected_rooms[i].transform.position);
        
        }

    }


    public void AddConnection(Room connected_room)
    {
        var line_renderer = this.AddComponent<LineRenderer>();
        line_renderer.positionCount++;
        line_renderer.SetPosition(0, this.transform.position);
        line_renderer.positionCount++;
        line_renderer.SetPosition(1, connected_room.transform.position);

        line_renderers.Add(line_renderer);
        connected_rooms.Add(connected_room);

    }


}
