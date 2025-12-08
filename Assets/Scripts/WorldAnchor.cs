using UnityEngine;

public class WorldAnchor : MonoBehaviour
{
    public enum Side { Left, Right }

    [SerializeField] Side anchorSide = Side.Left;
    [SerializeField] float xOffset = 0.1f;
    [SerializeField] float yPosViewport = 0.5f;

    Camera cam;

    void Awake()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        float x = anchorSide == Side.Left ? xOffset : 1f - xOffset;
        float y = yPosViewport;

        float z = Mathf.Abs(cam.transform.position.z - transform.position.z);

        Vector3 viewPos = new Vector3(x, y, z);
        transform.position = cam.ViewportToWorldPoint(viewPos);
    }
}
