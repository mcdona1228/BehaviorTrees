using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventBus : Singleton<EventBus>
{
    private Dictionary<string, UnityEvent> this_EventDictionary;
    private static int nextEventId = 1; 

    public override void Awake()
    {
        base.Awake();
        Instance.Initil();
    }

    private void Initil()
    {
        if (Instance.this_EventDictionary == null)
        {
            Instance.this_EventDictionary = new Dictionary<string, UnityEvent>();
        }
    }

    public static int GetEventId()
    {
        return nextEventId++;
    }

    public static void StartListening(string eventName, UnityAction listener)
    {
        UnityEvent thisEvent = null;
        if (Instance.this_EventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            Instance.this_EventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, UnityAction listener)
    {
        UnityEvent thisEvent = null;
        if (Instance.this_EventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(string eventName)
    {
        UnityEvent thisEvent = null;
        if (Instance.this_EventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke();
        }
    }

    public static void ScheduleTrigger(string eventName, float secondsFromNow)
    {
        EventBus.Instance.StartCoroutine(EventBus.Instance.DelayTrigger(eventName, secondsFromNow));
    }

    IEnumerator DelayTrigger(string eventName, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        TriggerEvent(eventName);
    }
}
