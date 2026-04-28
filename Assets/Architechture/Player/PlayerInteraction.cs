using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float interactDistance = 3f; // Дистанция взаимодействия
    private PointerEventData pointerData;
    private EventSystem eventSystem;

    void Start()
    {
        eventSystem = EventSystem.current;
        pointerData = new PointerEventData(eventSystem);
    }

    void Update()
    {
        // Кадр за кадром симулируем положение курсора в центре экрана
        pointerData.position = new Vector2(Screen.width / 2, Screen.height / 2);

        List<RaycastResult> results = new List<RaycastResult>();
        eventSystem.RaycastAll(pointerData, results);

        if (results.Count > 0)
        {

            float distanceToObj = Vector3.Distance(transform.position, results[0].worldPosition);

            // Если объект UI слишком далеко, игнорируем его
            if (distanceToObj > interactDistance) return;

            GameObject hoveredObj = results[0].gameObject;

            // Если нажата кнопка взаимодействия (например, ЛКМ или клавиша E)
            if (Input.GetButtonDown("Fire1")) 
            {
                ExecuteEvents.Execute(hoveredObj, pointerData, ExecuteEvents.submitHandler);
                // Можно также использовать pointerClickHandler для имитации клика мыши
                ExecuteEvents.Execute(hoveredObj, pointerData, ExecuteEvents.pointerClickHandler);
            }
        }
    }
}