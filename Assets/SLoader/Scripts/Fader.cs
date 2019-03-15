using System.Collections;
using UnityEngine;
using UnityEngine.UI;
namespace SLoader
{
    public class Fader : MonoBehaviour
    {
        [Range(0.2f, 3f)]
        public float fadingTime = .5f;
        public Image faderImage;
        public bool fading { get; private set; }

        
        bool fadeIn;

        public void FadeIn()
        {
            fadeIn = true;
            gameObject.SetActive(true);
            StartCoroutine(Fade());
        }

        public void FadeOut()
        {
            fadeIn = false;
            gameObject.SetActive(true);
            StartCoroutine(Fade());
        }

        IEnumerator Fade()
        {
            fading = true;
            float currentFadingTime = 0;

            while (currentFadingTime <= fadingTime)
            {
                currentFadingTime += Time.deltaTime;
                yield return null;
                float alpha = fadeIn ? currentFadingTime / fadingTime : 1 - currentFadingTime / fadingTime;
                faderImage.color = new Color(faderImage.color.r, faderImage.color.g, faderImage.color.b, alpha);
            }

            fading = false;
            gameObject.SetActive(fadeIn);
        }
    }
}
