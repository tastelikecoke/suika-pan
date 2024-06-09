using UnityEngine;
using Random = UnityEngine.Random;

namespace tastelikecoke.PanMachine
{
    /// <summary>
    /// Handles spawning the fruits. Also controls the tongs (aka "Cloud")
    /// Represents the cloud in the original Suika Game.
    /// </summary>
    public class CloudController : MonoBehaviour
    {
        [Header("Fruit Settings")]
        [SerializeField]
        private Transform fruitRoot;
        [SerializeField]
        private Transform fruitContainer;
        [SerializeField]
        private Transform constrainedFruit;
        [SerializeField]
        private Transform nextNextFruitRoot;
        [SerializeField]
        private FruitManager fruitManager;

        [Header("Physics Settings")]
        [SerializeField]
        private Vector3 tilt;
        [SerializeField]
        private float forceMultiplier = 3f;
        [SerializeField]
        private bool isDebugOn = false;

        [Header("UI")]
        [SerializeField]
        private AudioSource audioSource;
        [SerializeField]
        private PauseMenu pauseMenu;

        /* Controller values */
        private bool _isPointerHovering = false;
        private bool _isPointerClicked = false;
        private GameObject _equippedFruit = null;
        private GameObject _equippedNextNextFruit = null;

        public void SetPointerHover(bool value)
        {
            _isPointerHovering = value;
        }
        public void SetPointerClick(bool value)
        {
            _isPointerClicked = value;
        }
        private void EquipNextFruit()
        {
            var newFruit = Instantiate(fruitManager.GetNextFruit(), fruitContainer);

            var newFruitScript = newFruit.GetComponent<Fruit>();
            newFruitScript.SetAsNonMoving();
            
            newFruit.transform.rotation = Random.value > 0.5f ? Quaternion.Euler(-tilt) : Quaternion.Euler(tilt);
            _equippedFruit = newFruit;
            fruitManager.CheckRatEquipped();

            Destroy(_equippedNextNextFruit);

            _equippedNextNextFruit = Instantiate(fruitManager.GetNextNextFruit(), nextNextFruitRoot);
            
            var equippedNextNextFruitScript = _equippedNextNextFruit.GetComponent<Fruit>();
            equippedNextNextFruitScript.SetAsNonMoving();
        }

        private void FixedUpdate()
        {
            // do not execute if on retry.
            if (fruitManager.isFailed) return;
            if (fruitManager.dontFallFirst) return;

            var rb = GetComponent<Rigidbody2D>();
            var horizontalInput = Input.GetAxis("Horizontal");
            rb.velocity = forceMultiplier * Time.fixedDeltaTime * new Vector3(horizontalInput, 0f, 0f);

            if (_isPointerHovering)
            {
                UpdateMouse();
            }
        }

        public void UpdateMouse()
        {
            if (fruitManager.dontFallFirst) return;

            var rb = GetComponent<Rigidbody2D>();
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            transform.position = new Vector3(mousePosition.x, transform.position.y, transform.position.z);
        }
        private void Update()
        {
            // do not execute if on retry.
            if (fruitManager.isFailed) return;
            if (fruitManager.dontFallFirst) return;

            if (_equippedFruit == null || _equippedFruit.GetComponent<Fruit>().isTouched)
            {
                EquipNextFruit();
            }

            if (Input.GetButtonDown("Fire2") && GameSystem.Instance != null)
            {
                GameSystem.Instance.bgm.mute = !GameSystem.Instance.bgm.mute;
            }

            if (Input.GetButtonDown("Cancel") && !pauseMenu.GetComponent<Canvas>().enabled && !fruitManager.retryMenu.GetComponent<Canvas>().enabled)
            {
                StartCoroutine(pauseMenu.ShowCR());
            }

            var fireInput = _isPointerClicked || Input.GetButtonDown("Submit");
            if (fireInput && fruitContainer.childCount > 0)
            {
                var equippedRotation = _equippedFruit.transform.rotation;
                Destroy(_equippedFruit);

                var newFruit = Instantiate(fruitManager.GetNextFruit(), fruitRoot);
                /* add jitter */
                newFruit.transform.position = constrainedFruit.position + (Vector3)(Random.insideUnitCircle * 0.01f);
                newFruit.transform.rotation = equippedRotation;
                newFruit.GetComponent<Fruit>().manager = fruitManager;

                if (!audioSource.isPlaying)
                    audioSource.Play();

                //follow velocity. Just don't lol. funny though
                //newFruit.GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity;
                _equippedFruit = newFruit;
                // set this to allow spam
#if UNITY_EDITOR
                if (isDebugOn)
                    _equippedFruit.GetComponent<Fruit>().isTouched = true;
#endif
                fruitManager.AssignNextFruit();
            }

            _isPointerClicked = false;
        }
    }
}