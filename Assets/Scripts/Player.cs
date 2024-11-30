using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public Room current_room;
    public float step_time = 0.4f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Simulation");
    }

    // Update is called once per frame
    void Update()
    {
        
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
