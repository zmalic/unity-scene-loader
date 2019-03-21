using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using SLoader;
using UnityEngine.SceneManagement;

namespace Tests
{
    public class IntegrationTests
    {
        [UnityTest]
        // Test usinfg local tips from Resources
        public IEnumerator UseLocalTip()
        {
            string startSceneName = SceneManager.GetActiveScene().name;
            Test(false);
            yield return new WaitForSeconds(6);

            // if the starting sene changed, the test succeeded
            Assert.AreNotEqual(startSceneName, SceneManager.GetActiveScene().name);
        }

        [UnityTest]
        // Test usinfg tips from web (davand.net)
        public IEnumerator UseRemoteTip()
        {
            string startSceneName = SceneManager.GetActiveScene().name;
            Test(true);
            yield return new WaitForSeconds(6);

            // if the starting sene changed, the test succeeded
            Assert.AreNotEqual(startSceneName, SceneManager.GetActiveScene().name);
        }

        private void Test(bool loadFromWeb)
        {
            // load sceneA, if sceneA is current scene then load sceneB
            string sceneForLoad = SceneManager.GetActiveScene().name.Equals("sceneA") ? "sceneB" : "sceneA";

            // if SceneLoader instance exists, set sceneLoaderPrefab as loader's game object
            GameObject sceneLoaderPrefab = SceneLoader.Instance?.gameObject;

            // if if SceneLoader instance doesn't exists, create main camera and the SceneLoader from prefab
            if (SceneLoader.Instance == null)
            {
                GameObject cam = new GameObject("Main Camera");
                cam.AddComponent<Camera>().tag = "MainCamera";

                sceneLoaderPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/SLoader/Prefabs/SceneLoader.prefab");
            }

            if (loadFromWeb)
            {
                // test for loading a tip from web
                TipLoader tipLoader = sceneLoaderPrefab.GetComponent<TipLoader>();
                tipLoader.loadFromWeb = true;
                tipLoader.url = "davand.net/app.php";
            }

            if(SceneLoader.Instance == null)
            {
                // if loader doesn't exists instantiate it, and set the first scene for loading
                MonoBehaviour.Instantiate(sceneLoaderPrefab);
                SceneLoader.Instance.firstSceneToLoad = sceneForLoad;
            }
            else
            {
                // load scene
                SceneLoader.Instance.LoadScene(sceneForLoad);
            }
        }
    }
}
