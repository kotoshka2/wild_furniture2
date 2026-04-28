using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider))]
public class RitualAltar : MonoBehaviour
{
    [Header("References")]
    public RitualPerformer performer;

    [Header("Input")]
    [Tooltip("Ссылка на экшен для жертвоприношения (например, та же кнопка Interact или отдельная кнопка Attack)")]
    public InputActionReference sacrificeAction;

    private bool isPlayerInRange = false;

    private void Reset()
    {
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void OnEnable()
    {
        if (sacrificeAction != null)
        {
            sacrificeAction.action.Enable();
            sacrificeAction.action.performed += OnSacrificePerformed;
        }
    }

    private void OnDisable()
    {
        if (sacrificeAction != null)
        {
            sacrificeAction.action.performed -= OnSacrificePerformed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("[RitualAltar] Игрок у алтаря жертвоприношений.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    private void OnSacrificePerformed(InputAction.CallbackContext context)
    {
        if (isPlayerInRange && performer != null)
        {
            // Убеждаемся, что мы не пытаемся принести жертву, пока печатаем заклинание
            if (performer.inputUI != null && performer.inputUI.uiPanel.activeSelf)
            {
                return;
            }

            Debug.Log("[RitualAltar] Кнопка нажата! Приносим жертву...");
            performer.PerformSacrifice();
        }
    }
}
