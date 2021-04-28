using UnityEngine;

public class ElementalTextShower : MonoBehaviour
{
    public Camera camera;
    public GameObject bubble;
    Vector2 bubblePos;

    void Update()
    {
        bubblePos = new Vector2(bubble.transform.GetChild(0).gameObject.transform.position.x, bubble.transform.GetChild(0).gameObject.transform.position.y);
        Vector3 screenPos = camera.WorldToScreenPoint(bubblePos);
        transform.position = screenPos;

        if (Time.timeScale == 1f )
        {
            gameObject.SetActive(false);
        }
    }
}