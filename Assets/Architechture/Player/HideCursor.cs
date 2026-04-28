using UnityEngine;

public class HideCursor : MonoBehaviour
{
    void Start()
    {
        // Прячет курсор
        Cursor.visible = false;
        
        // Блокирует курсор по центру экрана, чтобы он не выходил за пределы окна игры
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Опционально: если вы хотите возвращать курсор по нажатию на Escape
    void Update()
    {
         /*if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }*/
    }
}
