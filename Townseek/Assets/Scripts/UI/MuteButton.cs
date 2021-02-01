using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using WhalesAndGames.MapGame.Singletons;

namespace WhalesAndGames.MapGame.UI
{
    /// <summary>
    /// Toggles the mute on the game's audio.
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class MuteButton : MonoBehaviour
    {
        [FoldoutGroup("Mute Off")]
        [SerializeField]
        private Sprite muteOffSprite;
        [FoldoutGroup("Mute Off")]
        [SerializeField]
        private SpriteState muteOffSpriteState;

        [FoldoutGroup("Mute On")]
        [SerializeField]
        private Sprite muteOnSprite;
        [FoldoutGroup("Mute On")]
        [SerializeField]
        private SpriteState muteOnSpriteState;

        private Image muteImage;
        private Button muteButton;
        private AudioManager audioManager;
        
        /// <summary>
        /// Initializes this button.
        /// </summary>
        private void Awake()
        {
            audioManager = GameManager.AudioManager;

            muteImage = GetComponent<Image>();
            muteButton = GetComponent<Button>();
        }
        
        /// <summary>
        /// Toggles the mute button to change the audio settings.
        /// </summary>
        public void ToggleMuteButton()
        {
            var isMuted = audioManager.ToggleMute();

            muteImage.sprite = isMuted ? muteOnSprite : muteOffSprite;
            muteButton.spriteState = isMuted ? muteOnSpriteState : muteOffSpriteState;
        }
    }
}