using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class Projector : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed = 6f;

    [Header("Focus Area")]
    [SerializeField] Transform focusArea;
    [SerializeField] float activeFocusScale = 3f;
    [SerializeField] float inactiveFocusScale = 1.5f;

    [Header("Lights")]
    [SerializeField] Light2D smallLight;
    [SerializeField] Light2D bigLight;

    [Header("Sprite")]
    [SerializeField] SpriteRenderer spriteRenderer;

    Vector2 moveInput;
    bool focusPressed;

    Camera cam;
    Generator generator;

    public bool IsFocusActive => focusPressed;

    void Awake()
    {
        cam = Camera.main;
        generator = FindAnyObjectByType<Generator>();

        var input = GetComponent<PlayerInput>();
        input.actions["Move"].performed += OnMove;
        input.actions["Move"].canceled += OnMove;
        input.actions["Focus"].performed += OnFocus;
        input.actions["Focus"].canceled += OnFocus;
    }

    void Update()
    {
        Move();
        FlipSprite();
        UpdateFocusArea();
        UpdateLights();
    }

    void Move()
    {
        Vector3 delta = (Vector3)(moveInput * (moveSpeed * Time.deltaTime));
        Vector3 newPos = transform.position + delta;

        Vector3 min = cam.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 max = cam.ViewportToWorldPoint(new Vector3(1, 1, 0));

        newPos.x = Mathf.Clamp(newPos.x, min.x, max.x);
        newPos.y = Mathf.Clamp(newPos.y, min.y, max.y);

        transform.position = newPos;
    }

    void FlipSprite()
    {
        if (!spriteRenderer) return;

        if (moveInput.x > 0.01f)
            spriteRenderer.flipX = false;
        else if (moveInput.x < -0.01f)
            spriteRenderer.flipX = true;
    }

    void UpdateFocusArea()
    {
        bool canUse = generator == null || generator.HasEnergy;
        if (!canUse) focusPressed = false;

        float targetScale = focusPressed ? activeFocusScale : inactiveFocusScale;

        focusArea.localScale = new Vector3(targetScale, targetScale, 1f);
    }

    void UpdateLights()
    {
        bool active = focusPressed && (generator == null || generator.HasEnergy);

        bigLight.intensity = active ? 1f : 0f;
    }

    void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    void OnFocus(InputAction.CallbackContext ctx)
    {
        focusPressed = ctx.ReadValueAsButton();
    }
}
