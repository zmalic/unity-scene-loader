using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SLoader
{
    public class SceneLoader : MonoBehaviour
    {
        public string firstSceneToLoad = "";
        private int _loadSceneIndex;

        Canvas _canvas;
        LazyFollow _lazyFollow;

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


            // Get floating canvas component
            Transform worldCanvas = transform.Find("WorldCanvas");
            _canvas = worldCanvas.GetComponent<Canvas>();
            _lazyFollow = worldCanvas.GetComponent<LazyFollow>();

            // When the loading scene starts, we need to load scene defined in the firstSceneToLoad

            // NOTE: Test
            Invoke("LoadFirstScene", 5);
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
            SceneManager.LoadSceneAsync(_loadSceneIndex);
        }

        public void OnSceneWasSwitched(Scene oldScene, Scene newScene)
        {
            print("Scene was switched");
            _lazyFollow.Reset();
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
