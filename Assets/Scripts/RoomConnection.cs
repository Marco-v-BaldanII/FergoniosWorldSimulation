using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class RoomConnection : MonoBehaviour
{

    Room roomA;
    Room roomB;

    LineRenderer lineRenderer;

    public Text label;

    [Range(0, 9)]
    public int weight;

    public RoomConnection(Room rA, Room rB) { Initialize(rA, rB); }

    public void Initialize(Room rA, Room rb)
    {
        roomA = rA; roomB = rb;
        lineRenderer = gameObject.AddComponent<LineRenderer>();

        lineRenderer.SetPositions(new Vector3[] { rA.transform.position, rb.transform.position });

    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer?.SetPosition(1, roomA.transform.position);
        lineRenderer?.SetPosition(0, roomB.transform.position);
    }
}
