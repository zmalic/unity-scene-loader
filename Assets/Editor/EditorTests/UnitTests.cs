using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SLoader;
using System;

namespace Tests
{
    public class UnitTests
    {
        Tip tip;

        [Test]
        // try to load tip from resources
        public void TipLoaderFromResources()
        {
            // unset current tip if exists and create game object with TipLoader
            tip = null;
            GameObject tipLoaderGameObject = new GameObject("Test tip loader");
            TipLoader tipLoader = tipLoaderGameObject.AddComponent<TipLoader>();

            // tip loaded event
            TipLoader.OnTipLoaded += TipLoaded;

            // load tip
            tipLoader.Load();

            // if tip is not null, the test succeeded
            Assert.IsNotNull(tip);


            TipLoader.OnTipLoaded -= TipLoaded;
        }


        // when tip was loaded
        private void TipLoaded(Tip t)
        {
            tip = t;
        }
    }
}
