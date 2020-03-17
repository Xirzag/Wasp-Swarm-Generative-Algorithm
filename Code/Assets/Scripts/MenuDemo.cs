using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuDemo : MonoBehaviour
{

    TrainGenerator train;
    TestGenerator test;
    public float velocity = 10.0f;

    public TMPro.TextMeshProUGUI status;

    // Start is called before the first frame update
    void Start()
    {
        train = GetComponent<TrainGenerator>();
        test = GetComponent<TestGenerator>();

        ShapeGenerator generator = new TreeShape();
        // train.render = false;
        train.StartTrain(generator);
        StartCoroutine(TestCallback());
    }

    private IEnumerator TestCallback() {
        yield return new WaitForSeconds(5);
        train.StopTrain();
        status.text = "Generando...";
        while(true) {
            yield return new WaitForSeconds(1);
            test.Generate(train.GetWasps());
        }
    }

    void FixedUpdate() {
        Quaternion rotation = transform.localRotation;
        transform.localRotation = Quaternion.Euler(rotation.eulerAngles.x,
            rotation.eulerAngles.y  + velocity * Time.deltaTime,
            rotation.eulerAngles.z);
    }

}
