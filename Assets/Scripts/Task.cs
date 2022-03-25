using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;
using System;

public abstract class Task
{
    public abstract void run();
    public bool completed;

    protected int eventID;
    const string EVENT_NAME_PREFIX = "CompletedTask";
    public string TaskCompleted
    {
        get
        {
            return EVENT_NAME_PREFIX + eventID;
        }
    }
    public Task()
    {
        eventID = EventBus.GetEventId();
    }
}

public class IsTrue : Task
{
    bool variableToTest; 

    public IsTrue(bool someBool)
    {
        variableToTest = someBool;
    }

    public override void run()
    {
        Debug.Log("IsTrue");
        completed = variableToTest;
        EventBus.TriggerEvent(TaskCompleted);
    }
}

public class IsFalse : Task
{
    bool variableToTest;
    public IsFalse(bool someBool)
    {
        variableToTest = someBool;
    }

    public override void run()
    {
        Debug.Log("IsFalse");
        completed = variableToTest;
        EventBus.TriggerEvent(TaskCompleted);
    }
}

public class OpenDoor : Task
{
    Door thisDoor;

    public OpenDoor(Door thatDoor)
    {
        thisDoor = thatDoor;
    }

    public override void run()
    {
        Debug.Log("Open Door");
        completed = thisDoor.open();
        EventBus.TriggerEvent(TaskCompleted);
    }
}

public class BreakDoor : Task
{
    Rigidbody thisDoor;

    public BreakDoor(Rigidbody thatDoor)
    {
        thisDoor = thatDoor;
    }

    public override void run()
    {
        Debug.Log("BreakDoor");
        thisDoor.AddForce(-10f, 0, 0, ForceMode.VelocityChange);
        completed = true;
        EventBus.TriggerEvent(TaskCompleted);
    }
}

public class HulkSmash : Task
{
    GameObject thisEntity;

    public HulkSmash(GameObject thatEntity)
    {
        thisEntity = thatEntity;
    }

    public override void run()
    {
        Debug.Log("HulkSmash");
        thisEntity.transform.localScale *= 3;
        //thisEntity.GetComponent<Renderer>().material > SetColor("_Color", Color.green);
        completed = true;
        EventBus.TriggerEvent(TaskCompleted);
    }
}

public class wait : Task
{
    float thisWaitTime;
    public wait(float time)
    {
        thisWaitTime = time;
    }

    public override void run()
    {
        Debug.Log("Wait");
        completed = true;
        EventBus.ScheduleTrigger(TaskCompleted, thisWaitTime);
    }
}

public class MoveObject : Task
{
    Arriver thisMover;
    GameObject thisTarget;

    public MoveObject(Kinematic mover, GameObject target)
    {
        thisMover = mover as Arriver;
        thisTarget = target;
    }

    public override void run()
    {
        Debug.Log("MoveObject");
        thisMover.OnArrived += MoverhasArrived;
        thisMover.myTarget = thisTarget;
    }

    public void MoverhasArrived()
    {
        thisMover.OnArrived -= MoverhasArrived;
        completed = true;
        EventBus.TriggerEvent(TaskCompleted);
    }
}

public class Sequence : Task
{
    List<Task> children;
    Task imediateTask;
    int imediateTaskIndex = 0;

    public Sequence(List<Task> taskList)
    {
        children = taskList;
    }

    public override void run()
    {
        Debug.Log("Sequence");
        imediateTask = children[imediateTaskIndex];
        EventBus.StartListening(imediateTask.TaskCompleted, ChildTaskCompleted);
        imediateTask.run();
    }

    void ChildTaskCompleted()
    {
        if (imediateTask.completed)
        {
            EventBus.StopListening(imediateTask.TaskCompleted, ChildTaskCompleted);
            imediateTaskIndex++;
            if(imediateTaskIndex < children.Count)
            {
                this.run();
            }
            else
            {
                completed = true;
                EventBus.TriggerEvent(TaskCompleted);
            }
        }
        else
        {
            completed = false;
            EventBus.TriggerEvent(TaskCompleted);
        }
    }
}

public class Selector : Task
{
    List<Task> children;
    Task imediateTask;
    int imediateTaskIndex = 0;

    public Selector(List<Task> taskList)
    {
        children = taskList;
    }

    public override void run()
    {
        Debug.Log("Selector");
        imediateTask = children[imediateTaskIndex];
        EventBus.StartListening(imediateTask.TaskCompleted, ChildTaskCompleted);
        imediateTask.run();
    }

    void ChildTaskCompleted()
    {
        if (imediateTask.completed)
        {
            completed = true;
            EventBus.TriggerEvent(TaskCompleted);
        }
        else
        {
            EventBus.StopListening(imediateTask.TaskCompleted, ChildTaskCompleted);
            imediateTaskIndex++;
            if (imediateTaskIndex < children.Count)
            {
                this.run();
            }
            else
            {
                completed = false;
                EventBus.TriggerEvent(TaskCompleted);
            }
        }
    }
}
