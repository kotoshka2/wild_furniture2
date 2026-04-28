using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider))]
public class RitualInteractionPoint : MonoBehaviour
{
    [Header("References")]
    public RitualPerformer performer;

    [Header("Input")]
    [Tooltip("Ссылка на экшен взаимодействия из вашего Input Action Asset")]
    public InputActionReference interactAction;

    private bool isPlayerInRange = false;

    private void Reset()
    {
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void OnEnable()
    {
        if (interactAction != null)
        {
            interactAction.action.Enable();
            interactAction.action.performed += OnInteractPerformed;
        }
    }

    private void OnDisable()
    {
        if (interactAction != null)
        {
            interactAction.action.performed -= OnInteractPerformed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Замените "Player" на тег, который висит на вашем персонаже
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("[RitualInteractionPoint] Игрок в зоне. Нажмите кнопку взаимодействия.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        //Debug.Log("Кнопка нажата!");
        if (isPlayerInRange && performer != null)
        {
            // Если панель уже открыта, игнорируем нажатие, чтобы не сбрасывать ввод буквы E
            if (performer.inputUI != null && performer.inputUI.uiPanel.activeSelf)
            {
                return;
            }

            performer.StartRitualInput();
        }
    }
}
