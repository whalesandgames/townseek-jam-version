using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using WhalesAndGames.MapGame.Singletons;

namespace WhalesAndGames.MapGame.Helpers
{
    /// <summary>
    /// Displays a cursor in-game, and allows the image to be swapped.
    /// </summary>
    public class GameCursor : MonoBehaviour
    {
        [Header("Canvas Render")]
        public Vector2 mousePixelPosition;
        private Image cursorImage;

        [Header("Cursor Images")]
        [SerializeField]
        private Sprite cursorIdle;
        [SerializeField]
        private Sprite cursorClick;

        // Internal References
        private RectTransform cursorRect;

        /// <summary>
        /// Start is called just before any of the Update methods is called the first time.
        /// </summary>
        private void Start()
        {
            cursorRect = transform.GetChild(0).GetComponent<RectTransform>();
            cursorImage = cursorRect.GetComponent<Image>();
            
            UpdateCursorPosition();

#if UNITY_ANDROID
            cursorRect.GetComponentInChildren<Image>().enabled = false;
#endif
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update()
        {
            // Sets the cursor to be invisible.
            if (Cursor.visible)
            {
                Cursor.visible = false;
            }

            // Gets the mouse in pixel size and updates the UI to reflect it.
#if !UNITY_ANDROID
            mousePixelPosition = Mouse.current.position.ReadValue();
#else
            mousePixelPosition = Touchscreen.current.position.ReadValue();
#endif

            // Updates the cursor position.
            if (cursorRect == null)
            {
                return;
            }

            UpdateCursorPosition();
            cursorImage.sprite = !Mouse.current.leftButton.isPressed ? cursorIdle : cursorClick;
        }

        /// <summary>
        /// Updates the cursor's transform position to match that of the mouse.
        /// </summary>
        private void UpdateCursorPosition()
        {
            cursorRect.transform.position = mousePixelPosition;
        }

        /// <summary>
        /// Shows/Hide the cursor image depending on the state.
        /// </summary>
        public void ShowCursor(bool state)
        {
#if !UNITY_ANDROID
            cursorRect.GetComponentInChildren<Image>().enabled = state;
#endif
        }
    }
}
