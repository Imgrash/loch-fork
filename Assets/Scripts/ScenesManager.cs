using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ScenesManager : MonoBehaviour, IPointerClickHandler
{
    public string SceneName;
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("гружу сцену");
        SceneManager.LoadScene(SceneName);
    }
    public void LoadScene()
    {
        Debug.Log("гружу сцену");
        SceneManager.LoadScene(SceneName);
    }
}
