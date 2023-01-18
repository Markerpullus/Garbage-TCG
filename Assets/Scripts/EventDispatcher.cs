using System;
using System.Collections.Generic;
using UnityEngine;

public delegate void EventHandler(short type);

public delegate void EventHandler<in T>(short type, T data);

// https://gist.github.com/yangruihan/4473a409fbe6bb1d007d470046b2fb9b
// Added my own singleton design
/*
 * #4: Hand Change
*/
public class EventDispatcher : MonoBehaviour
{
    public static EventDispatcher Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public class EventHandlerListData
    {
        public short EventType { get; }

        private readonly List<Delegate> _delegates;

        public EventHandlerListData(short type) : this(type, 8)
        {
        }

        public EventHandlerListData(short type, int size)
        {
            EventType = type;
            _delegates = new List<Delegate>(size);
        }

        public void Send()
        {
            var len = _delegates.Count;
            for (var i = 0; i < len; i++)
            {
                if (_delegates[i] == null)
                    continue;
                var tEvent = (EventHandler)_delegates[i];
                tEvent(EventType);
            }
        }

        public void Send<T>(T data)
        {
            var len = _delegates.Count;
            for (var i = 0; i < len; i++)
            {
                if (_delegates[i] == null)
                    continue;
                var tEvent = (EventHandler<T>)_delegates[i];
                tEvent(EventType, data);
            }
        }

        public void Clear()
        {
            _delegates.Clear();
        }

        public void ClearNullEventHandler()
        {
            var len = _delegates.Count;
            var newLen = 0;
            for (var j = 0; j < len; j++)
            {
                if (_delegates[j] == null) continue;

                if (newLen != j)
                    _delegates[newLen] = _delegates[j];
                newLen++;
            }

            _delegates.RemoveRange(newLen, len - newLen);
        }

        public bool TryAdd(Delegate handler)
        {
            var newEvent = true;

#if DEBUG
            if (_delegates.Contains(handler))
            {
                Debug.LogError("[Error] Repeat to add " + handler);
                newEvent = false;
            }
#endif

            if (newEvent)
            {
                _delegates.Add(handler);
            }

            return newEvent;
        }

        public bool TryRemove(Delegate handler)
        {
            var index = _delegates.IndexOf(handler);
            if (index != -1)
            {
                _delegates[index] = null;
                return true;
            }

            return false;
        }
    }

    private readonly Dictionary<short, EventHandlerListData> _eventHandlerDic =
        new Dictionary<short, EventHandlerListData>();

    private readonly List<EventHandlerListData> _tempHandlerList = new List<EventHandlerListData>();

    public void AddEventHandler(short type, EventHandler handler)
    {
        AddEventHandlerInternal(_eventHandlerDic, type, handler);
    }

    public void RemoveEventHandler(short type, EventHandler handler)
    {
        RemoveEventHandlerInternal(_eventHandlerDic, type, handler);
    }

    public void AddEventHandler<T>(short type, EventHandler<T> handler)
    {
        try
        {
            AddEventHandlerInternal(_eventHandlerDic, type, handler);
        }
        catch
        {
            Debug.LogError("trying to add handler error, type:" + type + " handler:" + handler);
        }
    }

    public void RemoveEventHandler<T>(short type, EventHandler<T> handler)
    {
        RemoveEventHandlerInternal(_eventHandlerDic, type, handler);
    }

    public void SendEvent(short type)
    {
        var eventListeners = GetEventHandlerList(_eventHandlerDic, type, false);
        eventListeners?.Send();
    }

    public void SendEvent<T>(short type, T msg)
    {
        var eventListeners = GetEventHandlerList(_eventHandlerDic, type, false);
        eventListeners?.Send(msg);
    }

    public EventHandlerListData GetEventHandlerList(short type)
    {
        return _eventHandlerDic.TryGetValue(type, out var eventList) ? eventList : null;
    }

    public void Clear()
    {
        foreach (var eventData in _eventHandlerDic)
        {
            eventData.Value.Clear();
        }

        _eventHandlerDic.Clear();
    }

    public void PresizeHandler(short type, int size)
    {
        if (!_eventHandlerDic.ContainsKey(type))
        {
            _eventHandlerDic.Add(type, new EventHandlerListData(type, size));
        }
    }

    public void ClearNullEventHandler()
    {
        var count = _tempHandlerList.Count;
        if (count > 0)
        {
            for (var i = 0; i < count; i++)
            {
                var eventListeners = _tempHandlerList[i];
                eventListeners.ClearNullEventHandler();
            }

            _tempHandlerList.Clear();
        }
    }

    private void AddEventHandlerInternal(Dictionary<short, EventHandlerListData> dic, short type, Delegate handler)
    {
        var eventListeners = GetEventHandlerList(dic, type, true);
        eventListeners?.TryAdd(handler);
    }

    private void RemoveEventHandlerInternal(Dictionary<short, EventHandlerListData> dic, short type,
        Delegate handler)
    {
        var eventListeners = GetEventHandlerList(dic, type, false);
        if (eventListeners == null)
            return;

        if (eventListeners.TryRemove(handler))
        {
            if (!_tempHandlerList.Contains(eventListeners))
                _tempHandlerList.Add(eventListeners);
        }
    }

    private EventHandlerListData GetEventHandlerList(Dictionary<short, EventHandlerListData> dic, short type,
        bool autoCreate)
    {
        if (dic.TryGetValue(type, out var eventList))
            return eventList;

        if (autoCreate)
        {
            eventList = new EventHandlerListData(type);
            dic.Add(type, eventList);
            return eventList;
        }

        return null;
    }
}