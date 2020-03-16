using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ArchitectureList architectures = ArchitectureList.ReadFromJson("architectures.json");

        var simulation = GetComponent<Simulation>();
        simulation.SetRules(architectures.architectures[0].rules);
        simulation.StartSimulation();
    }

}
