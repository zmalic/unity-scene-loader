using UnityEngine;
using UnityEngine.UI;
namespace SLoader
{
    public class LoadingScreen : MonoBehaviour
    {
        public Text tipTitle;
        public Text tipDescription;
        public Slider loadingBar;

        public void ShowTip(Tip tip)
        {
            tipTitle.text = tip.title;
            tipDescription.text = tip.description;
        }

        public void UpdateLoadingBar(float val)
        {
            val = Mathf.Clamp(val, 0, 1f);
            loadingBar.value = val;
        }
    }
}
