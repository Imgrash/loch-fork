using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHi : MonoBehaviour
{
    public GameObject Food1, Food2;
    public void Pick()
    {
        GameObject Pick = Food1;
        if (Food1.activeInHierarchy != false)
            Pick = Food2;
        if (Food2.activeInHierarchy != false)
            Pick = Food1;
        Food1.SetActive(false);
        Food2.SetActive(false);
        Pick.SetActive(true);
        Debug.Log("вуаля");
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.F))
            Pick();
    }
}
//    public void OnPointerClick(PointerEventData eventData)
//    {
//        Pick();
//    }
//}
