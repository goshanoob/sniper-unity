using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// Всплывающее окно с результатом игры.
/// </summary>
public class PopupWindow : MonoBehaviour
{
    /// <summary>
    /// Подпись с результатом игры.
    /// </summary>
    [SerializeField] private Text resultMessage = null;

    private void Start()
    {
        // Закрыть всплывающее окно при старте.
        Close();
    }

    /// <summary>
    /// Открыть всплывающее окно.
    /// </summary>
    public void Open(string message)
    {
        resultMessage.text = message;
        gameObject.SetActive(true);
        // Показать курсор.
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    /// <summary>
    /// Закрыть всплывающее окно.
    /// </summary>
    public void Close()
    {
        gameObject.SetActive(false);
    }
}