using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hulk : MonoBehaviour
{
    public Door door;
    public GameObject treasure;
    public GameObject test;
    bool executingBehavior = false;
    Task imediateTask;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (!executingBehavior)
            {
                executingBehavior = true;
                imediateTask = BuildTask_GetTreasure();

                EventBus.StartListening(imediateTask.TaskCompleted, OnTaskCompleted);
                imediateTask.run();
            }
        }
    }

    void OnTaskCompleted()
    {
        EventBus.StopListening(imediateTask.TaskCompleted, OnTaskCompleted);
        Debug.Log("Behavior complete! Success = " + imediateTask.completed);
        executingBehavior = false;
    }

    Task BuildTask_GetTreasure()
    {
        List<Task> taskList = new List<Task>();

        Task isDoorNotLocked = new IsFalse(door.isLocked);
        Task waitABeat = new wait(0.5f);
        Task openDoor = new OpenDoor(door);
        taskList.Add(isDoorNotLocked);
        //taskList.Add(waitABeat);
        taskList.Add(openDoor);
        Sequence openUnlockedDoor = new Sequence(taskList);

        return openUnlockedDoor;
      
        taskList = new List<Task>();
        Task isDoorClosed = new IsTrue(door.isClosed);
        Task hulkOut = new HulkSmash(this.gameObject);
        Task bargeDoor = new BreakDoor(door.transform.GetChild(0).GetComponent<Rigidbody>());
        taskList.Add(isDoorClosed);
        taskList.Add(waitABeat);
        taskList.Add(hulkOut);
        taskList.Add(waitABeat);
        taskList.Add(bargeDoor);
        Sequence bargeClosedDoor = new Sequence(taskList);
       
        //return bargeClosedDoor;
        
        taskList = new List<Task>();
        taskList.Add(openUnlockedDoor);
        taskList.Add(bargeClosedDoor);
        Selector openTheDoor = new Selector(taskList);

        return openTheDoor;
        /*
        taskList = new List<Task>();
        Task moveToDoor = new MoveObject(this.GetComponent<Kinematic>(), door.gameObject);
        Task moveToTreasure = new MoveObject(this.GetComponent<Kinematic>(), treasure.gameObject);
        taskList.Add(moveToDoor);
        taskList.Add(waitABeat);
        taskList.Add(openTheDoor); 
        taskList.Add(waitABeat);
        taskList.Add(moveToTreasure);
        Sequence getTreasureBehindClosedDoor = new Sequence(taskList);

        taskList = new List<Task>();
        Task isDoorOpen = new IsFalse(door.isClosed);
        taskList.Add(isDoorOpen);
        taskList.Add(moveToTreasure);
        Sequence getTreasureBehindOpenDoor = new Sequence(taskList);

        taskList = new List<Task>();
        taskList.Add(getTreasureBehindOpenDoor);
        taskList.Add(getTreasureBehindClosedDoor);
        Selector getTreasure = new Selector(taskList);
        
        return getTreasure;*/
    }
}
