using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[ExecuteInEditMode]
public class WeightIcon : MonoBehaviour
{

    public Weight my_weight;

    private Weight prev_weight;

    public TextMeshProUGUI label;

    public RoomConnection connection1;
    public RoomConnection connection2;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("GainWeight");
    }

    // Update is called once per frame
    void Update()
    {
        if(label == null) { label = GetComponent<TextMeshProUGUI>(); }
        if( int.Parse(label.text ) != my_weight.max)
        {
            print("changed weight to " + label.text);
            my_weight.max = int.Parse(label.text);
        }

        // update weight on connections
        if (prev_weight != my_weight)
        {
            prev_weight = my_weight;
            connection1.weight = my_weight;
            connection2.weight = my_weight;
        }


    }

    IEnumerator GainWeight()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(0.4f);
            if(label != null) { label.text = (int.Parse(label.text) + 1).ToString(); }
        }
    }
}
