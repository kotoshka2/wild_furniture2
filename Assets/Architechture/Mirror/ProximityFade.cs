using UnityEngine;

public class ProximityFade : MonoBehaviour
{
    [Header("Настройки дистанции")]
    public Transform playerTransform;   // Ссылка на игрока (или его камеру)
    public float activationDistance = 2f; // Расстояние, на котором доска видна на 100%
    public float fadeSpeed = 5f;        // Скорость появления/исчезновения

    private CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        // Если забыли назначить игрока, попробуем найти его по тегу
        if (playerTransform == null && GameObject.FindGameObjectWithTag("Player") != null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Update()
    {
        if (playerTransform == null) return;

        // Считаем дистанцию между доской и игроком
        float distance = Vector3.Distance(transform.position, playerTransform.position);

        // Определяем целевое значение прозрачности
        // Если ближе 2 метров — 1 (видно), если дальше — 0 (не видно)
        float targetAlpha = (distance <= activationDistance) ? 1f : 0f;

        // Плавно меняем текущую прозрачность на целевую
        canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, targetAlpha, Time.deltaTime * fadeSpeed);

        // Блокируем взаимодействие, если доска не видна (чтобы нельзя было кликнуть издалека)
        canvasGroup.interactable = (canvasGroup.alpha > 0.5f);
        canvasGroup.blocksRaycasts = (canvasGroup.alpha > 0.5f);
    }
}