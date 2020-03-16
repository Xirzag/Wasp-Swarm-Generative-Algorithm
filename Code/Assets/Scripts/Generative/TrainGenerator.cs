using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TrainGenerator : MonoBehaviour
{
        
    List<Wasp> wasps = new List<Wasp>();
    
    public int totalNumberOfIterations = 10000;
    public float initialWaspsPoints = 20;
    public float waspReward = 10;
    public float waspPunishment = -10;
    public float healthIterDecay = -1;
    public float healthStepDecay = -.01f;
    public float probOfCreateNewBrickNumber = -.2f;
    public float probOfUseSameBrickNumber = -.2f;
    public float dieThreshold = 0;
    public int waspAmountToStartDying = 15;
    public int maxWasps = 120;

    private ShapeGenerator generator;
    private Iteration currentIteration;

    void Start() {
        // StartTrain(new DefaultShape());
        // StartTrain(new SquareOutline());
        // StartTrain(new SquareShape());
        // StartTrain(new CircleOutline());
    }

    void Update()
    {
        if (IsTraining()) {
            bool finished = currentIteration.Train();
            GetComponent<StructureRenderer>().Render(currentIteration.structure);
            if (finished)
            {
                int nextIteration = currentIteration.iteration + 1;
                if(nextIteration > totalNumberOfIterations)
                    currentIteration = null;
                else
                    currentIteration = new Iteration(this, 
                        generator.GetShape(nextIteration), nextIteration); 
            }
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            string msg = "Count: " + wasps.Count + '\n';
            foreach (var t in wasps)
            {
                msg += t.ToString() + '\n';
            }
            Debug.Log(msg);
        }

    }

    internal List<Wasp> GetWasps()
    {
        return wasps;
    }

    public bool IsTraining()
    {
        return currentIteration != null;
    }

    public int IterationNumber() {
        return currentIteration != null? currentIteration.iteration : 0;
    }

    public int AmountOfWasps() {
        return wasps.Count;
    }

    public void StartTrain(ShapeGenerator generator) {
        this.generator = generator;
        currentIteration = new Iteration(this, generator.GetShape(0), 0);
    }

    public void StopTrain() {
        currentIteration = null;
    }

    public void Reset() {
        this.wasps.Clear();
         currentIteration = null;
    }

    class Iteration{
        public BrickStructure structure;
        public int iteration;
        HashSet<Vector2Int> shape;
        int lastBrickNumber = 1;
        TrainGenerator trainer;

        public Iteration(TrainGenerator trainer, HashSet<Vector2Int> shape, int iteration) {
            this.trainer = trainer;
            this.shape = shape;
            this.structure = new BrickStructure();
            this.iteration = iteration;
        }

        private bool initialized = false;
        public bool Train() {
            if(!initialized) {
                List<Vector2Int> initialBricks = InitStructure();
                structure.RecalculateHollows(initialBricks);
                initialized = true;
            }
            
            while(structure.HaveHollows()) {
                Step();
                if(trainer.ShouldRender())
                    return false;
            }

            foreach (var wasp in trainer.wasps)
                wasp.health += trainer.healthIterDecay;

            RemoveWorstWasps();

            return true;
        }

        private void RemoveWorstWasps()
        {
            int waspsAmount = trainer.wasps.Count;

            if(waspsAmount > trainer.waspAmountToStartDying) {
                trainer.wasps.RemoveAll(w => w.health < trainer.dieThreshold);

                waspsAmount = trainer.wasps.Count;
                if(waspsAmount > trainer.maxWasps) {
                    trainer.wasps.Sort((w1, w2) => w1.health.CompareTo(w2.health));
                    trainer.wasps.RemoveRange(trainer.maxWasps, waspsAmount - trainer.maxWasps);
                }
            }
        }

        private void Step() {
            Vector2Int hollow = structure.PopFirstHollow();

            List<Wasp> waspWhichCanPutTemplate = new List<Wasp>();

            foreach (var wasp in trainer.wasps)
            {
                bool dontFitShape = false;
                if(structure.TemplateFitIn(hollow, wasp, shape, out dontFitShape)) {
                    if(dontFitShape) {
                        wasp.ModifyHealth(trainer.waspPunishment);
                    }else{
                        waspWhichCanPutTemplate.Add(wasp);
                        wasp.ModifyHealth(trainer.waspReward);
                    }
                }

            }

            if(waspWhichCanPutTemplate.Count == 0) {
                if(trainer.probOfUseSameBrickNumber != 0) {
                    waspWhichCanPutTemplate.Add(CreateWaspFor(hollow, lastBrickNumber));
                    waspWhichCanPutTemplate.Last().health *= trainer.probOfUseSameBrickNumber;
                }
                if(trainer.probOfCreateNewBrickNumber != 0) {
                    lastBrickNumber++;
                    waspWhichCanPutTemplate.Add(CreateWaspFor(hollow, lastBrickNumber));
                    waspWhichCanPutTemplate.Last().health *= trainer.probOfCreateNewBrickNumber;
                }

                foreach (var w in waspWhichCanPutTemplate) trainer.AddWasp(w);
            }

            Wasp selectedTemplate = Wasp.Select(waspWhichCanPutTemplate);

            var newBricks = structure.AddBricks(hollow, selectedTemplate);
            if(newBricks.Count > 0)
                structure.RecalculateHollows(newBricks);

            foreach (var wasp in trainer.wasps)
                wasp.health += trainer.healthStepDecay;


        }

        private Wasp CreateWaspFor(Vector2Int center, int newBrickNumber)
        {
            Wasp wasp = new Wasp();
            for(int i = -1; i < 2; i++){
                for(int j = -1; j < 2; j++){
                    Vector2Int pos = new Vector2Int(i,j) + center;
                    int brick = shape.Contains(pos)? newBrickNumber : 0;
                    brick = structure.HaveBrickIn(pos)? structure.GetBrick(pos) : brick;
                    wasp.template[i+1, j+1] = brick;
                }
            }

            wasp.health = trainer.initialWaspsPoints;

            return wasp;
        }

        private List<Vector2Int> InitStructure()
        {
            Vector2Int b = shape.First();
            structure.AddBrick(b, 1);
            return new List<Vector2Int>(){b};
        }

    }

    private float renderTime = 0;
    public float renderInterval = 1.0f / 60.0f;
    
    private bool ShouldRender()
    {
        if(Time.time > renderTime + renderInterval) {
            renderTime = Time.time + renderInterval;
            return true;
        }else{
            return false;
        }
    }

    private void AddWasp(Wasp wasp)
    {
        wasps.Add(wasp);
    }
}
