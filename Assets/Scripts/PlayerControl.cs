using System;
using UnityEngine;

/// <summary>
/// Управление со стороны игрока.
/// </summary>
public class PlayerControl : MonoBehaviour
{
    /// <summary>
    /// Событие нажатия клавиши пробел.
    /// </summary>
    public event Action OnSpaceDown;

    /// <summary>
    /// Событие отпускания клавиши пробел.
    /// </summary>
    public event Action OnSpaceUp;

    /// <summary>
    /// Событие нажатия левой кнопки мыши.
    /// </summary>
    public event Action OnLeftClick;

    /// <summary>
    /// Событие нажатия клавиши 1 на клавиатуре.
    /// </summary>
    public event Action<int> OnOneButtonPress;

    /// <summary>
    /// Событие нажатия клавиши 2 на клавиатуре.
    /// </summary>
    public event Action<int> OnTwoButtonPress;

    /// <summary>
    /// Событие нажатия клавиши 3 на клавиатуре.
    /// </summary>
    public event Action<int> OnThreeButtonPress;

    /// <summary>
    /// Событие нажатия клавиши 4 на клавиатуре. 
    /// </summary>
    public event Action<int> OnFourButtonPress;

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
            OnSpaceDown?.Invoke();
        }

        // Если клавиша пробел отпущена, вызвать событие.
        if (spaceUp)
        {
            OnSpaceUp?.Invoke();
        }

        // Если нажали левую кнопку мыши, вызвать событие.
        if (leftClick)
        {
            OnLeftClick?.Invoke();
        }

        // Если нажали кнопку 1 на клавиатуре, вызвать событие.
        if (oneButton)
        {
            OnOneButtonPress?.Invoke(1);
        }

        // Если нажали кнопку 2 на клавиатуре, вызвать событие.
        if (twoButton)
        {
            OnTwoButtonPress?.Invoke(2);
        }

        // Если нажали кнопку 3 на клавиатуре, вызвать событие.
        if (threeButton)
        {
            OnThreeButtonPress?.Invoke(3);
        }

        // Если нажали кнопку 4 на клавиатуре, вызвать событие.
        if (fourButton)
        {
            OnFourButtonPress?.Invoke(4);
        }
    }
}