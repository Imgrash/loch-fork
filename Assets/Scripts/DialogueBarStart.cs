using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBarStart : MonoBehaviour
{
    public GameObject Camera;
    public void ActivateCamera()
    {
        Debug.Log("загрузка новелльного диалога");
        Camera.SetActive(true);
    }
}
