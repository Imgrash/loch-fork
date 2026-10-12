using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractLoadScene : InteractBase
{
    public InteractLoadScene() : base("SceneScript") { }
    public override void Interact(GameObject interactedObject)
    {
        interactedObject.GetComponent<ScenesManager>().LoadScene();
        Debug.Log("загрузка сцены");
    }
}
