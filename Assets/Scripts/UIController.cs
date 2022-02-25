using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Контроллер графического интерфейса.
/// </summary>
public class UIController : MonoBehaviour
{
    /// <summary>
    /// Подпись текущего уровня.
    /// </summary>
    [SerializeField] private Text levelText = null;

    /// <summary>
    /// Подпись оставшегося количества выстрелов.
    /// </summary>
    [SerializeField] private Text shotsText = null;

    /// <summary>
    /// Подпись выбранного вида оружия.
    /// </summary>
    [SerializeField] private Text weaponText = null;

    /// <summary>
    /// Подпись количества набранных очков.
    /// </summary>
    [SerializeField] private Text pointsText = null;

    /// <summary>
    /// Всплывающее окно об окончании игры.
    /// </summary>
    [SerializeField] private PopupWindow popupWindow = null;

    /// <summary>
    /// Изображение прицела.
    /// </summary>
    [SerializeField] private Image aim = null;

    /// <summary>
    /// Вывести текущий уровень игры.
    /// </summary>
    /// <param name="level">Уровень игры.</param>
    public void PrintLevel(int level)
    {
        levelText.text = $"Уровень игры: {level.ToString()}";
    }

    /// <summary>
    /// Вывести оставшееся количество выстрелов.
    /// </summary>
    /// <param name="shots">Оставшееся количество выстрелов.</param>
    public void PrintShots(int shots)
    {
        shotsText.text = $"Осталось выстрелов: {shots.ToString()}";
    }

    /// <summary>
    /// Вывести номер выбранного оружия.
    /// </summary>
    /// <param name="weapon"></param>
    public void PrintWeapon(int weapon)
    {
        weaponText.text = $"Оружие #: {weapon.ToString()}";
    }

    /// <summary>
    /// Вывести количество набранных очков.
    /// </summary>
    /// <param name="points"></param>
    public void PrintPoints(float points)
    {
        pointsText.text = $"Очки: {points.ToString()}";
    }

    /// <summary>
    /// Вывести сообщение о результате игры.
    /// </summary>
    /// <param name="message">Сообщение о результате игры.</param>
    public void OpenPopupWindow(string message)
    {
        popupWindow.Open(message);
    }

    /// <summary>
    /// Переключить видимость курсора прицеливания.
    /// </summary>
    /// <param name="isVisible">true - показать курсор, false - скрыть.</param>
    public void ToggleAim(bool isVisible)
    {
        aim.gameObject.SetActive(isVisible);
    }
}