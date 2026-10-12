using StorySpace;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class GlobalUniqueObjectsManager
{
    private static GlobalUniqueObjectsManager instance;

    private Dictionary<Guid,UniqueObject> globalObjs = new();
    public static GlobalUniqueObjectsManager getInstance() => instance ??= new GlobalUniqueObjectsManager();

    public void Add(UniqueObject sv)
    {
        if (globalObjs.ContainsKey(sv.Id))
        {
            Debug.Log("А такой объект уже существует");
            return;
        }
        globalObjs.Add(sv.Id, sv);
    }

    public void Remove(UniqueObject sv)
    {
        if(globalObjs.ContainsKey(sv.Id))
            globalObjs.Remove(sv.Id);
        else
        {
            Debug.Log("А нет такого объекта");
        }
    }
    public bool TryGet(Guid Id, out UniqueObject val)
    {
        if (globalObjs.ContainsKey(Id))
        {
            val = globalObjs[Id];
            return true;
        }
        val = null;
        return false;

    }
}

public abstract class UniqueObject : MonoBehaviour
{
    private Guid? id;
    public Guid Id => id ??= CreateNewId();
    private Guid CreateNewId()
    {
        Guid guid = Guid.NewGuid();
        GlobalUniqueObjectsManager instance = GlobalUniqueObjectsManager.getInstance();
        while (instance.TryGet(guid, out UniqueObject uo))
            guid = Guid.NewGuid();
        id = guid;
        instance.Add(this);
        return guid;
    }
    ~UniqueObject()
    {
        GlobalUniqueObjectsManager instance = GlobalUniqueObjectsManager.getInstance();
        instance.Remove(this);
    }
}
