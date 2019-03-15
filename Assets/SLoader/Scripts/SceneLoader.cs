using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SLoader
{
    public class SceneLoader : MonoBehaviour
    {
        public string firstSceneToLoad = "";

        int _loadSceneIndex;
        Transform _loadingPanel;
        Canvas _canvas;
        LazyFollow _lazyFollow;
        TipLoader _tipLoader;
        LoadingScreen _loadingScreen;

        public static SceneLoader Instance { get; protected set; }


        public void Awake()
        {
            // Make sure that there is only one instance of SceneLoader
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                throw new System.Exception("An instance of the SceneLoader already exists.");
            }
            Instance = this;

            // Don't destroy on load - maybe we need this loader for loading more scenes
            DontDestroyOnLoad(gameObject);


            // Get world canvas transform
            _loadingPanel = transform.Find("LoadingScreen");

            // floating canvas component
            _canvas = _loadingPanel.GetComponent<Canvas>();

            // lazy follow instance
            _lazyFollow = _loadingPanel.GetComponent<LazyFollow>();

            // tip loader instance
            _tipLoader = gameObject.GetComponent<TipLoader>();

            // loading screen instance
            _loadingScreen = _loadingPanel.GetComponent<LoadingScreen>();

            // when tip loaded...
            TipLoader.OnTipLoaded += TipLoaded;

            // When the loading scene starts, we need to load scene defined in the firstSceneToLoad
            LoadFirstScene();
        }

        public void Start()
        {
            // Fade out when the scene switched
            SceneManager.activeSceneChanged += OnSceneWasSwitched;
        }

        /// <summary>
        /// Load scene
        /// </summary>
        /// <param name="sceneName">Scene name</param>
        public void LoadScene(string sceneName)
        {
            LoadScene(GetSceneIndex(sceneName));
        }


        /// <summary>
        /// Load scene
        /// </summary>
        /// <param name="sceneIndex">Scene index (defined in the Build Settings)</param>
        public void LoadScene(int sceneIndex)
        {
            if (!ReadyForLoad(sceneIndex))
            {
                throw new System.Exception("The scene " + sceneIndex + " is unknown or isn't ready for load");
            }
            _canvas.worldCamera = Camera.main;
            _loadSceneIndex = sceneIndex;
            _tipLoader.Load();
        }

        public void OnSceneWasSwitched(Scene oldScene, Scene newScene)
        {
            print("Scene was switched");
        }


        private void TipLoaded(Tip t)
        {
            print(t.title);
            _loadingScreen.ShowTip(t);
            StartCoroutine(LoadSceneAsync());
        }

        IEnumerator LoadSceneAsync()
        {
            _lazyFollow.Follow();
            yield return new WaitForSeconds(5);
            _lazyFollow.Stop();
            SceneManager.LoadSceneAsync(_loadSceneIndex);
        }


        /// <summary>
        /// Loads scene defined in the firstSceneToLoad variable
        /// </summary>
        /// <param name="sceneName"></param>
        private void LoadFirstScene()
        {
            LoadScene(firstSceneToLoad);
        }

        /// <summary>
        /// Get scene index by scene name
        /// If the scene does not exists, returns -1
        /// </summary>
        /// <param name="sceneName">Scene name</param>
        /// <returns>Scene index (defined in the Build Settings)</returns>
        private int GetSceneIndex(string sceneName)
        {
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                string scene = Path.GetFileNameWithoutExtension(scenePath);
                if (scene.Equals(sceneName))
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Checks if scene is ready for loading
        /// </summary>
        /// <param name="id">Scene id</param>
        /// <returns>is ready</returns>
        private bool ReadyForLoad(int id)
        {
            return SceneManager.GetActiveScene().buildIndex != id && id > 0 && id < SceneManager.sceneCountInBuildSettings;
        }
    }
}
