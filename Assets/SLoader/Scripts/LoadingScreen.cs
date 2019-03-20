using UnityEngine;
using UnityEngine.UI;
namespace SLoader
{
    public class LoadingScreen : MonoBehaviour
    {
        public Text tipTitle;
        public Text tipDescription;
        public Slider loadingBar;

        /// <summary>
        /// Show tip on the panel
        /// </summary>
        /// <param name="tip"></param>
        public void ShowTip(Tip tip)
        {
            tipTitle.text = tip.title;
            tipDescription.text = tip.description;
        }

        /// <summary>
        /// Update loading bar value
        /// </summary>
        /// <param name="val"></param>
        public void UpdateLoadingBar(float val)
        {
            val = Mathf.Clamp(val, 0, 1f);
            loadingBar.value = val;
        }
    }
}
