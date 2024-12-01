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

    public Weight weight;
    public WeightIcon weightIcon;

    public bool needs_key = false;

    //public RoomConnection(Room rA, Room rB) { Initialize(rA, rB); }

    public void Initialize(Room rA, Room rb, int weight, GameObject text_component)
    {
        roomA = rA; roomB = rb;
        lineRenderer = GetComponent<LineRenderer>();
        weightIcon = text_component.GetComponent<WeightIcon>();
        lineRenderer.SetPositions(new Vector3[] { rA.transform.position, rb.transform.position });
        this.weight.max = weight;

        // update weight icon
        this.text_component = text_component;
        label = text_component.GetComponent<TextMeshProUGUI>();
        label.text = weight.ToString();


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

        if (text_component)
        {
            var distance = roomA.transform.position + roomB.transform.position;
            text_component.transform.position = distance * 0.5f;
        }

        if ((roomB && roomB.visited) || (roomA && roomA.visited))
        {
            
            if (weightIcon )
            {
                weightIcon.weight_increase = 1;

            }
        }

       // if(label) label.text = weight.max.ToString();

    }
}

public struct Weight
{
    public int min;
    public int max;

    // Define equality operator (==)
    public static bool operator ==(Weight w1, Weight w2)
    {
        return w1.min == w2.min && w1.max == w2.max;
    }

    // Define inequality operator (!=)
    public static bool operator !=(Weight w1, Weight w2)
    {
        return !(w1 == w2);
    }


}
