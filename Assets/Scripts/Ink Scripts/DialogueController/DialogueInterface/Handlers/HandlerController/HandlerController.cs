using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace StorySpace
{
    public class HandlerController : MonoBehaviour
    {
        public Dictionary<string, IHandler> handlers = new();
        public List<HandlerController> handlerControllers = new();
        public bool isRoot = false;
        public void Start()
        {
            if (transform.parent == null)
            {
                isRoot = true;
            }
            else
            {
                if (transform.parent.gameObject.GetComponent<HandlerController>() == null)
                    isRoot = true;
            }
            if (isRoot)
                CollectAll();
        }
        public void CollectAll()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject child = transform.GetChild(i).gameObject;
                if (child.GetComponent<HandlerController>() is HandlerController handlerController)
                {
                    handlerController.CollectAll();
                    handlerControllers.Add(handlerController);
                }
                if (child.GetComponent<IHandler>() is IHandler handler)
                {
                    handlers.Add(handler.getName(), handler);
                }
            }
            if (isRoot)
            {
                Debug.Log("Все добавленные обработчики тегов:");
                foreach (var item in handlers)
                {
                    Debug.Log(item.Key);
                }
            }
        }

        public IHandler getHandler(string nameHandler)
        {
            IHandler handler = null;
            if (handlers.ContainsKey(nameHandler))
            {
                handler = handlers[nameHandler];
            }
            else
            {
                foreach (var item in handlerControllers)
                {
                    handler = item.getHandler(nameHandler);
                    if (handler != null)
                        break;
                }
            }
            return handler;
        }
    }
}