using System;
using UnityEngine;

/// <summary>
/// Управление со стороны игрока.
/// </summary>
public class PlayerControl : MonoBehaviour
{
    /// <summary>
    /// Событие удержания кнопки пробел.
    /// </summary>
    public event Action OnSpaceHold;
    /// <summary>
    /// Событие удержания кнопки пробел.
    /// </summary>
    public event Action OnSpaceRelease;
    /// <summary>
    /// Событие нажатия левой кнопки мыши.
    /// </summary>
    public event Action OnLeftClick;
    /// <summary>
    /// Событие нажатия кнопки 1 на клавиатуре.
    /// </summary>
    public event Action OnOneButtonPress;
    /// <summary>
    /// Событие нажатия кнопки 2 на клавиатуре.
    /// </summary>
    public event Action OnTwoButtonPress;
    /// <summary>
    /// Событие нажатия кнопки 3 на клавиатуре.
    /// </summary>
    public event Action OnThreeButtonPress;
    /// <summary>
    /// Событие нажатия кнопки 4 на клавиатуре.
    /// </summary>
    public event Action OnFourButtonPress;
    
    private void Update()
    {
        bool spaceDown = Input.GetKey(KeyCode.Space);
        bool spaceUp = Input.GetKeyUp(KeyCode.Space);
        bool leftClick = Input.GetMouseButtonDown(0);
        bool oneButton = Input.GetKey(KeyCode.Alpha1);
        bool twoButton = Input.GetKey(KeyCode.Alpha2);
        bool threeButton = Input.GetKey(KeyCode.Alpha3);
        bool fourButton = Input.GetKey(KeyCode.Alpha4);
        // Если зажата клавиша пробел, вызвать событие.
        if (spaceDown)
        {
            OnSpaceHold?.Invoke();
        }
        // Если клавишу пробел отпустили, вызвать событие.
        if (spaceUp)
        {
            OnSpaceRelease?.Invoke();
        }
        // Если нажали левую кнопку мыши, вызвать событие.
        if (leftClick)
        {
            OnLeftClick?.Invoke();
        }
        // Если нажали кнопку 1 на клавиатуре, вызвать событие.
        if (oneButton)
        {
            OnOneButtonPress?.Invoke();
        }
        // Если нажали кнопку 2 на клавиатуре, вызвать событие.
        if (twoButton)
        {
            OnTwoButtonPress?.Invoke();
        }
        // Если нажали кнопку 3 на клавиатуре, вызвать событие.
        if (threeButton)
        {
            OnThreeButtonPress?.Invoke();
        }
        // Если нажали кнопку 4 на клавиатуре, вызвать событие.
        if (fourButton)
        {
            OnFourButtonPress?.Invoke();
        }
        
    }
}
