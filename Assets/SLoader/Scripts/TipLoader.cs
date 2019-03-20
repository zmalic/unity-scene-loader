using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace SLoader
{
    public class TipLoader : MonoBehaviour
    {
        [Tooltip("Load tip from web")]
        public bool loadFromWeb = false;

        /// <summary>
        /// For example, we can download tips from web in specific json format
        /// </summary>
        [Tooltip("Web url with tooltips data")]
        public string url = "";


        public delegate void TipLoaded(Tip t);
        public static event TipLoaded OnTipLoaded;


        /// <summary>
        /// Load tip
        /// </summary>
        public void Load()
        {
            if (loadFromWeb)
            {
                StartCoroutine(LoadTipFromWeb());
            }
            else
            {
                LoadTipFromResources();
            }
        }


        /// <summary>
        /// Load random tip from web
        /// There is a specific format, but you can write your own method for this
        /// </summary>
        /// <returns></returns>
        IEnumerator LoadTipFromWeb()
        {
            /// json request body (get one random tip between 0 and 15 from web
            string jsonBody = "[{\"method\":\"tasks.search\",\"params\":[\"\",\"createdAt\",\"desc\"," + Random.Range(0, 15) + ",1]}]";

            var www = new UnityWebRequest(url, "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
            www.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            www.timeout = 3;

            yield return www.SendWebRequest();

            if (www.isHttpError || www.isNetworkError)
            {
                Debug.Log("Error");
            }
            else
            {
                // Prepaire json for JsonUtility
                string result = www.downloadHandler.text.Replace("\"results\":[[", "\"results\":{\"tips\":[").Replace("]]}", "]}}");

                /// create UrlResponse object from json response
                UrlResponse response = JsonUtility.FromJson<UrlResponse>(result);

                // if tip is successful downloaded
                if (response.success && !response.results.IsEmpty())
                {  
                    Tip tip = response.results.GetRandom();
                    // invoke event
                    OnTipLoaded?.Invoke(tip);
                    yield break;
                }
            }

            // if tip is not loaded from web get one from resource
            LoadTipFromResources();
        }


        /// <summary>
        /// Load tip from Resources/tips.json file
        /// </summary>
        void LoadTipFromResources()
        {
            var tipsFile = Resources.Load("tips") as TextAsset;
            TipList tipList = JsonUtility.FromJson<TipList>(tipsFile.text);
            Tip tip = tipList.GetRandom();
            // invoke event
            OnTipLoaded?.Invoke(tip);
        }
    }
}

