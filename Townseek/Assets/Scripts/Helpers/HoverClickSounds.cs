using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace WhalesAndGames.MapGame.Helpers
{
    /// <summary>
    /// Does Hover and Click sounds whenever the player presses this button
    /// and if there are events associated.
    /// </summary>
    public class HoverClickSounds : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
    {
        [Header("Hover Sound")]
        [FMODUnity.EventRef]
        public string hoverSoundEvent;

        [Header("Click Sound")]
        [FMODUnity.EventRef]
        public string clickSoundEvent;

        /// <summary>
        /// Do this when the cursor enters the rect area of this selectable UI object.
        /// </summary>
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (hoverSoundEvent != string.Empty)
            {
                FMODUnity.RuntimeManager.PlayOneShot(hoverSoundEvent);
            }
        }

        /// <summary>
        /// Called by the EventSystem when a Click event occurs.
        /// </summary>
        public void OnPointerClick(PointerEventData eventData)
        {
            if (clickSoundEvent != string.Empty && eventData.button != PointerEventData.InputButton.Right)
            {
                FMODUnity.RuntimeManager.PlayOneShot(clickSoundEvent);
            }
        }
    }
}
