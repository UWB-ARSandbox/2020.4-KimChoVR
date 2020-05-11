using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsScript : MonoBehaviour
{
    public SimpleDemos.SendFloatArray_Example floatScript;
    public Rigidbody rb;

    // Index 0 = Jumped / Has Not Jumped
    // Index 1 = Health
    // Index 2 = Movement Speed
    // Index 3 = Look Speed
    // Index 4 = Jump Force

    // Start is called before the first frame update
    void Start()
    {
        SetupDefaultStats();
    }

    /**
     * SetupDefaultStats
     * -----------------
     * This method will initialize the player stats
     * with default starting values
     */
    private void SetupDefaultStats()
    {
        floatScript = this.gameObject.GetComponent<SimpleDemos.SendFloatArray_Example>();
        rb = this.gameObject.GetComponent<Rigidbody>();

        floatScript.m_MyFloats[0] = 0.0f;   // Jumped
        floatScript.m_MyFloats[1] = 100;    // Health
        floatScript.m_MyFloats[2] = 1;      // Movement Speed
        floatScript.m_MyFloats[3] = 3;      // Look Speed
        floatScript.m_MyFloats[4] = 3;      // Jump Force

        floatScript.m_SendFloat = true;
    }
}
