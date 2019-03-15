using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace SLoader
{
    public class TipLoader : MonoBehaviour
    {
        [Tooltip("Load tip from web")]
        public bool loadFromWeb = true;

        [Tooltip("Web url with tooltips data")]
        public string url = "";


        public delegate void TipLoaded(Tip t);
        public static event TipLoaded OnTipLoaded;

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

        IEnumerator LoadTipFromWeb()
        {
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

                UrlResponse response = JsonUtility.FromJson<UrlResponse>(result);

                if (response.success && !response.results.IsEmpty())
                {
                    Tip tip = response.results.GetRandom();
                    OnTipLoaded?.Invoke(tip);
                    yield break;
                }
            }

            LoadTipFromResources();
        }

        void LoadTipFromResources()
        {
            var tipsFile = Resources.Load("tips") as TextAsset;
            TipList tipList = JsonUtility.FromJson<TipList>(tipsFile.text);
            Tip tip = tipList.GetRandom();
            OnTipLoaded?.Invoke(tip);
        }
    }
}

