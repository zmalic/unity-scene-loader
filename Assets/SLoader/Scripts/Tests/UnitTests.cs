using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SLoader;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace Tests
{
    public class UnitTests
    {

        Tip tip;

        [UnityTest]
        // try to load tip from web
        public IEnumerator TipLoaderFromWeb()
        {
            // unset current tip if exists and create game object with TipLoader
            tip = null;
            GameObject tipLoaderGameObject = new GameObject("Test tip loader");
            TipLoader tipLoader = tipLoaderGameObject.AddComponent<TipLoader>();

            // load from web
            tipLoader.loadFromWeb = true;
            tipLoader.url = "davand.net/app.php";

            // tip loaded event
            TipLoader.OnTipLoaded += TipLoaded;

            // load tip
            tipLoader.Load();

            // wait few seconds while tip loads
            yield return new WaitForSeconds(3);

            // if tip is not null, the test succeeded
            Assert.IsNotNull(tip);
        }

        // when tip was loaded
        private void TipLoaded(Tip t)
        {
            tip = t;
        }
        

        [Test]
        // load again same scene
        public void TryToLoadCurrentScene()
        {
            // instantiate SceneLoader prefab if doesn't exists
            if(SceneLoader.Instance == null)
            {
                MonoBehaviour.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/SLoader/Prefabs/SceneLoader.prefab"));
            }

            // we expect SceneLoadException
            Assert.Catch(typeof(SceneLoadException), () => SceneLoader.Instance.LoadScene(SceneManager.GetActiveScene().name));
        }
    }
}
