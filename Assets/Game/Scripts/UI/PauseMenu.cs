using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace tastelikecoke.PanMachine
{
    /// <summary>
    /// For UI of Pause
    /// </summary>
    public class PauseMenu : MonoBehaviour
    {
        /// <summary>
        /// Show the pause menu over the game
        /// </summary>
        public void Show()
        {
            this.GetComponent<Canvas>().enabled = true;
            foreach (var selectable in GetComponentsInChildren<Selectable>())
            {
                selectable.interactable = true;
            }
        }
        
        /// <summary>
        /// Hide the pause menu, back to the game
        /// </summary>
        public void Hide()
        {
            this.GetComponent<Canvas>().enabled = false;
            foreach (var selectable in GetComponentsInChildren<Selectable>())
            {
                selectable.interactable = false;
            }
        }
        public void Update()
        {
            if (GetComponent<Canvas>().enabled)
            {
                if (Input.GetButtonDown("Submit"))
                {
                    StartCoroutine(HideCR());
                }

                if (Input.GetButtonDown("Cancel"))
                {
                    StartCoroutine(HideCR());
                }
            }

        }
        public IEnumerator ShowCR()
        {
            yield return null;
            Show();
        }
        public IEnumerator HideCR()
        {
            yield return null;
            Hide();
        }
    }
}