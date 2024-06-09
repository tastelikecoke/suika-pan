using UnityEngine;
using UnityEngine.UI;

namespace tastelikecoke.PanMachine
{
    /// <summary>
    /// Handles BGM enabler disabler.
    /// </summary>
    public class SoundFiend : MonoBehaviour
    {
        [SerializeField]
        private Toggle toggle;
        private void Update()
        {
            if (GameSystem.Instance)
            {
                toggle.SetIsOnWithoutNotify(GameSystem.Instance.bgm.mute);
            }
        }

        public void Flick()
        {
            if (GameSystem.Instance)
            {
                GameSystem.Instance.bgm.mute = !GameSystem.Instance.bgm.mute;
            }
        }
    }
}