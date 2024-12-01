using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class startButton : MonoBehaviour
{

    public TMP_InputField input_label;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClicked()
    {
        int result;

        if (input_label != null && int.TryParse( input_label.text, out result )) {

            SimulationManager.Instance.num_simulations = result;
            SceneManager.LoadScene(0);
        }
    }
}
