using UnityEngine;
using UnityEngine.InputSystem;

public class Projector : MonoBehaviour
{
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] Vector2 minBounds = new Vector2(-8f, -4f);
    [SerializeField] Vector2 maxBounds = new Vector2(8f, 4f);

    [SerializeField] Transform focusArea;
    [SerializeField] float activeFocusScale = 3f;
    [SerializeField] float inactiveFocusScale = 1.5f;

    Vector2 moveInput;
    bool focusPressed;
    Generator generator;

    public bool IsFocusActive => focusPressed;

    void Awake()
    {
        var input = GetComponent<PlayerInput>();
        input.actions["Move"].performed += OnMove;
        input.actions["Move"].canceled += OnMove;
        input.actions["Focus"].performed += OnFocus;
        input.actions["Focus"].canceled += OnFocus;
        generator = FindAnyObjectByType<Generator>();
    }

    void Update()
    {
        Move();
        UpdateFocus();
    }

    void Move()
    {
        if (moveInput.sqrMagnitude == 0f)
            return;

        Vector3 delta = (Vector3)(moveInput.normalized * (moveSpeed * Time.deltaTime));
        Vector3 newPos = transform.position + delta;

        newPos.x = Mathf.Clamp(newPos.x, minBounds.x, maxBounds.x);
        newPos.y = Mathf.Clamp(newPos.y, minBounds.y, maxBounds.y);

        transform.position = newPos;
    }

    void UpdateFocus()
    {
        if (!focusArea)
            return;

        bool canUseFocus = generator == null || generator.HasEnergy;

        if (!canUseFocus)
            focusPressed = false;

        float targetScale = (focusPressed && canUseFocus)
            ? activeFocusScale
            : inactiveFocusScale;

        focusArea.localScale = new Vector3(targetScale, targetScale, 1f);
    }

    void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    void OnFocus(InputAction.CallbackContext context)
    {
        focusPressed = context.ReadValueAsButton();
    }
}
