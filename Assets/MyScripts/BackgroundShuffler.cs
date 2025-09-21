using UnityEngine;
using UnityEngine.UI;

public class BackgroundShuffler : MonoBehaviour
{
    [Header("Assign your panel's Image component")]
    public Image panelImage;

    [Header("List of background sprites")]
    public Sprite[] backgroundImages;

    [Header("Shuffle interval in seconds")]
    public float shuffleInterval = 5f;

    private void Start()
    {
        if (panelImage != null && backgroundImages.Length > 0)
        {
            // Start the shuffling
            InvokeRepeating(nameof(ShuffleBackground), 0f, shuffleInterval);
        }
        else
        {
            Debug.LogWarning("BackgroundShuffler is missing an image or sprite list.");
        }
    }

    private void ShuffleBackground()
    {
        int randomIndex = Random.Range(0, backgroundImages.Length);
        panelImage.sprite = backgroundImages[randomIndex];
    }
}
