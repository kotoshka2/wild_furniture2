using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RitualInputUI : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject uiPanel;
    public TMP_InputField wordInputField;
    public Button submitButton;

    private System.Action<string> onWordSubmitted;

    private void Awake()
    {
        if (submitButton != null)
        {
            submitButton.onClick.AddListener(OnSubmitClicked);
        }
        Hide();
    }

    public void Show(System.Action<string> onSubmit)
    {
        onWordSubmitted = onSubmit;
        uiPanel.SetActive(true);
        wordInputField.text = "";
        wordInputField.Select();
        wordInputField.ActivateInputField();

        // Освобождаем курсор для UI
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Hide()
    {
        uiPanel.SetActive(false);
        wordInputField.text = "";

        // Прячем курсор обратно (только если игра уже запущена, чтобы не ломать инициализацию)
        if (Application.isPlaying)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void OnSubmitClicked()
    {
        string input = wordInputField.text;
        Hide();
        onWordSubmitted?.Invoke(input);
    }
}
