using System.Collections;
using UnityEngine;
using UnityEngine.UI;
namespace SLoader
{
    public class Fader : MonoBehaviour
    {
        [Tooltip("Duration of fading animation \n- The duration is same for fade in and fade out")]
        [Range(0.2f, 3f)]
        public float fadingTime = .5f;
        public Image faderImage;
        public bool fading { get; private set; }

        
        bool fadeIn;

        /// <summary>
        /// Fade in animation 
        /// Called before starting the next scene
        /// </summary>
        public void FadeIn()
        {
            fadeIn = true;
            gameObject.SetActive(true);
            StartCoroutine(Fade());
        }

        /// <summary>
        /// Fade out animation
        /// Called after next scene starts
        /// </summary>
        public void FadeOut()
        {
            fadeIn = false;
            gameObject.SetActive(true);
            StartCoroutine(Fade());
        }


        /// <summary>
        /// Fade coroutine
        /// Changes alpha channel of overlay image
        /// </summary>
        /// <returns></returns>
        IEnumerator Fade()
        {
            fading = true;
            float currentFadingTime = 0;

            while (currentFadingTime <= fadingTime)
            {
                currentFadingTime += Time.deltaTime;
                yield return null;
                
                // if animation is fade in - go from 0 to 1
                // otherwise - go from 1 to 0
                float alpha = fadeIn ? currentFadingTime / fadingTime : 1 - currentFadingTime / fadingTime;
                faderImage.color = new Color(faderImage.color.r, faderImage.color.g, faderImage.color.b, alpha);
            }

            fading = false;
            gameObject.SetActive(fadeIn);
        }
    }
}
