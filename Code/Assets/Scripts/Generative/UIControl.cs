using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIControl : MonoBehaviour
{
    
    public Dropdown dropdown;
    public TrainGenerator trainer;
    public TestGenerator testGenerator;

    public InputField initialHealth;
    public InputField healthReward;
    public InputField healthPunishment;
    public InputField waspMax;

    public TMPro.TextMeshProUGUI iterations;
    public TMPro.TextMeshProUGUI wasps;

    public void Train() {
        ShapeGenerator generator;
        switch(dropdown.value){
            case 0:
                generator = new DefaultShape();
                break;
            case 1:
                generator = new SquareShape();
                break;
            case 2:
                generator = new SquareOutline();
                break;
            case 3:
                generator = new CircleOutline();
                break;
            case 4:
                generator = new TreeShape();
                break;
            default:
                generator = new DickShape();
                break;
        }

        trainer.initialWaspsPoints = Int32.Parse(initialHealth.text);
        trainer.waspReward = Int32.Parse(healthReward.text);
        trainer.waspPunishment = Int32.Parse(healthPunishment.text);
        trainer.maxWasps = Int32.Parse(waspMax.text);
        
        trainer.StartTrain(generator);

    }

    public void Test() {
        trainer.StopTrain();
        testGenerator.Generate(trainer.GetWasps());
    }

    int pCounter = 0;

    private void FixedUpdate() {
        if(trainer.IsTraining()) {
            wasps.text = trainer.AmountOfWasps().ToString();
            iterations.text = trainer.IterationNumber().ToString();
        }

        if(Input.GetKey(KeyCode.P)) {
            pCounter++;
            if(pCounter==10) {
                dropdown.AddOptions(new List<string>(){"Dick"});
            }
        }
    }

}
