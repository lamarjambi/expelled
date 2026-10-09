using UnityEngine;
using UnityEngine.UI;

public class FadeAfterDelay : MonoBehaviour
{
    public float timeBeforeFade = 9f;
    public float fadeDuration = 2f; 
    
    float timer = 0f;
    RawImage image;
    void Awake()
    {
        image = GetComponent<RawImage>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeBeforeFade)
        {
            float alpha = Mathf.Clamp01(1 - (timer - timeBeforeFade) / fadeDuration);
            Color color = image.color;
            color.a = alpha;
            image.color = color;
        }
    }
}
