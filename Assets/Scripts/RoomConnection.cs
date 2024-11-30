using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class RoomConnection : MonoBehaviour
{

    public Room roomA;
    public Room roomB;

    LineRenderer lineRenderer;

    public TextMeshProUGUI label;
    private GameObject text_component;

    public GameObject weightPref;

    [Range(0, 9)]
    public int weight;

    //public RoomConnection(Room rA, Room rB) { Initialize(rA, rB); }

    public void Initialize(Room rA, Room rb, int weight)
    {
        roomA = rA; roomB = rb;
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.SetPositions(new Vector3[] { rA.transform.position, rb.transform.position });
        this.weight = weight;

        text_component = Instantiate(weightPref, this.transform);
        label = text_component.GetComponent<TextMeshProUGUI>();
        label.text = weight.ToString();

    }


    // Start is called before the first frame update
    void Start()
    {
        label = GetComponent<TextMeshProUGUI>();   
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer?.SetPosition(1, roomA.transform.position);
        lineRenderer?.SetPosition(0, roomB.transform.position);

        if (text_component)
        {
            var distance = roomA.transform.position + roomB.transform.position;
            text_component.transform.position = distance * 0.5f;
        }

    }
}
