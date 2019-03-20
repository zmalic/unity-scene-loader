using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SLoader
{
    public class SceneLoader : MonoBehaviour
    {
        [Tooltip("Name of first scene")]
        public string firstSceneToLoad = "";

        [Tooltip("Minimum time to display tip")]
        [Range(1f, 20f)]
        public float tipTime = 5f;

        int _loadSceneIndex;
        Transform _loadingPanel;
        Canvas _canvas;
        LazyFollow _lazyFollow;
        TipLoader _tipLoader;
        LoadingScreen _loadingScreen;
        Fader _fader;

        /// <summary>
        /// Instance of SceneLoader object,
        /// so we can call it from the other scenes using
        /// SceneLoader.Instance?.LoadScene(<SCENE_NAME_OR_INDEX>);
        /// </summary>
        public static SceneLoader Instance { get; protected set; }

        /// <summary>
        /// Init fields and listeners
        /// </summary>
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

            // fader instance
            _fader = transform.Find("Fader").GetComponent<Fader>();

            // when tip loaded...
            TipLoader.OnTipLoaded += TipLoaded;
        }

        /// <summary>
        /// Set activeSceneChanged listener and load the first scene defined in the firstSceneToLoad
        /// </summary>
        public void Start()
        {
            // Fade out when the scene was switched
            SceneManager.activeSceneChanged += OnSceneSwitched;

            // When the loading scene starts, we need to load scene defined in the firstSceneToLoad
            LoadFirstScene();
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
            _loadSceneIndex = sceneIndex;

            /// First step - load tip
            _tipLoader.Load();
        }

        /// <summary>
        /// When sceen has swiched start the fade out animation
        /// </summary>
        /// <param name="oldScene"></param>
        /// <param name="newScene"></param>
        public void OnSceneSwitched(Scene oldScene, Scene newScene)
        {
            _fader.FadeOut();
        }


        /// <summary>
        /// When tip was loaded, show it and load scene
        /// </summary>
        /// <param name="t"></param>
        private void TipLoaded(Tip t)
        {
            _loadingScreen.ShowTip(t);
            StartCoroutine(LoadSceneCoroutine());
        }


        /// <summary>
        /// Load scene async
        /// </summary>
        /// <returns></returns>
        IEnumerator LoadSceneCoroutine()
        {
            // Start panel follow
            _lazyFollow.Follow();
            float loadingTime = 0;

            // create AsyncOperation and deny auto scene activation
            AsyncOperation asincOperation = SceneManager.LoadSceneAsync(_loadSceneIndex);
            asincOperation.allowSceneActivation = false;

            // while scene is loading or until the loadingTime does not expire show the panel and update progres bar value
            while (asincOperation.progress < 0.9f || loadingTime < tipTime)
            {
                loadingTime += Time.deltaTime;
                yield return null;
                float progress = Mathf.Min(asincOperation.progress / 0.9f, loadingTime / tipTime);
                _loadingScreen.UpdateLoadingBar(progress);
            }

            // when loading has finished, show fade in animation 
            _fader.FadeIn();
            while (_fader.fading)
            {
                yield return new WaitForEndOfFrame();
            }

            // stop panel following
            _lazyFollow.Stop();

            // start new scene
            asincOperation.allowSceneActivation = true;
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
