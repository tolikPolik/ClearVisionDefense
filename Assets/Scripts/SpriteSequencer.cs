using UnityEngine;

public class SpriteSequencer : MonoBehaviour
{
    [SerializeField] Sprite[] frames;
    [SerializeField] float frameTime = 0.15f;

    SpriteRenderer sr;
    int index;
    float timer;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (frames == null || frames.Length == 0) return;

        timer += Time.deltaTime;
        if (timer >= frameTime)
        {
            timer -= frameTime;
            index = (index + 1) % frames.Length;
            sr.sprite = frames[index];
        }
    }
}