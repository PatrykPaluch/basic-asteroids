using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests {
    public class MainTest {

        [UnityTest]
        public IEnumerator GameObjectPoolSizeTest() {
            // prepare
            GameObject template = new GameObject("foo");
            const int poolSize = 10;
            GameObjectPool pool = new GameObjectPool(template, poolSize);

            yield return null;
            
            //cleanup
            Object.Destroy(template);
        }

        [UnityTest]
        public IEnumerator GameObjectPoolGetTest() {
            //prepare
            GameObject template = new GameObject("foo");
            const int poolSize = 10;
            GameObjectPool pool = new GameObjectPool(template, poolSize);
            
            // test
            Assert.AreEqual(pool.PoolSize, poolSize, "Wrong pool Size");

            GameObject[] pooledGameObjects = new GameObject[poolSize];
            for (int i = 0; i < poolSize; i++) {
                pooledGameObjects[i] = pool.Get();
                Assert.NotNull(pooledGameObjects[i], "GameObject from pool is null");
                Assert.True(pooledGameObjects[i].activeSelf, "GameObject from pool is not active");
            }

            for (int i = 0; i < poolSize; i++) {
                for (int j = i + 1; j < poolSize; j++) {
                    Assert.AreNotEqual(pooledGameObjects[i], pooledGameObjects[j], "Pool returned one instance multiple times");
                }
            }

            Assert.Null(pool.Get(), "GameObject from pool is not null when pool has no free object");
            
            
            //cleanup
            for (int i = 0; i < poolSize; i++) {
                Object.Destroy(pooledGameObjects[i]);
            }
            
            Object.Destroy(template);
            
            yield return null;
        }

        [UnityTest]
        public IEnumerator ScoreSaveTest() {
            string playerPrefsKey = ApplicationData.PlayerPrefsKeyScoreString;
            string dataBeforeTest = PlayerPrefs.GetString(playerPrefsKey); // to prevent data lose
            
            //prepare
            PlayerPrefs.DeleteKey(playerPrefsKey);
            
            //test
            ApplicationData.SaveScore("Foo", 10);
            Assert.AreEqual(PlayerPrefs.GetString(playerPrefsKey), ";Foo=10", "Saved data is wrong (Foo)");
            
            
            ApplicationData.SaveScore("Bar", 11);
            Assert.AreEqual(PlayerPrefs.GetString(playerPrefsKey), ";Foo=10;Bar=11", "Saved data is wrong (Bar)");
            
            
            ApplicationData.SaveScore("Baz", 12);
            Assert.AreEqual(PlayerPrefs.GetString(playerPrefsKey), ";Foo=10;Bar=11;Baz=12", "Saved data is wrong (Baz)");
            
            yield return null;
            
            // restore data 
            PlayerPrefs.SetString(ApplicationData.PlayerPrefsKeyScoreString, dataBeforeTest);
            PlayerPrefs.Save();
        }

        [UnityTest]
        public IEnumerator ScoreReadTest() {
            string playerPrefsKey = ApplicationData.PlayerPrefsKeyScoreString;
            string dataBeforeTest = PlayerPrefs.GetString(playerPrefsKey); // to prevent data lose
            
            //test
            PlayerPrefs.SetString(playerPrefsKey, ";Foo=0;Bar=5;Baz=10;Bow=15");

            ApplicationData.Score[] scores = ApplicationData.GetScores();
            Assert.AreEqual(scores.Length, 4, "Wrong number of scores");
            
            Assert.AreEqual(scores[0].Nick, "Foo", "Wrong Name for Score[0]");
            Assert.AreEqual(scores[0].Value, 0, "Wrong Value for Score[0]");
            
            Assert.AreEqual(scores[1].Nick, "Bar", "Wrong Name for Score[1]");
            Assert.AreEqual(scores[1].Value, 5, "Wrong Value for Score[1]");
            
            Assert.AreEqual(scores[2].Nick, "Baz", "Wrong Name for Score[2]");
            Assert.AreEqual(scores[2].Value, 10, "Wrong Value for Score[2]");
            
            Assert.AreEqual(scores[3].Nick, "Bow", "Wrong Name for Score[3]");
            Assert.AreEqual(scores[3].Value, 15, "Wrong Value for Score[3]");
            
            
            
            yield return null;
            
            // restore data 
            PlayerPrefs.SetString(ApplicationData.PlayerPrefsKeyScoreString, dataBeforeTest);
            PlayerPrefs.Save();
        }


        [UnityTest]
        public IEnumerator ObjectFollowTest() {
            GameObject target = new GameObject();
            target.transform.position = new Vector3(1000, 0, 0);
            
            GameObject follower = new GameObject();
            follower.transform.position = new Vector3(1000, 10, 0);

            ObjectFollow followComponent = follower.AddComponent<ObjectFollow>();
            followComponent.objectToFollow = target.transform;
            followComponent.smoothTime = 0.1f;
            
            yield return new WaitForSeconds(2); //Wait for ObjectFollow.Update

            float distance = Vector2.Distance(target.transform.position, follower.transform.position);
            Assert.Less(distance, 0.01f, "follower is not following target");
            
            //cleanup
            Object.Destroy(follower);
            Object.Destroy(target);
        }


        
        
        /*
          [UnityTest]
        public IEnumerator PointOnMeteorDestroyTest() {
            //prepare
            GameObject scoreManager = new GameObject();
            scoreManager.AddComponent<ScoreManager>();
            
            yield return null; //wait for Unity to Setup instances

            GameObject spacecraft = new GameObject();
            spacecraft.transform.position = new Vector3(1000, -1, 0);
            SpacecraftController spacecraftComponent = spacecraft.AddComponent<SpacecraftController>();
            
            GameObject cameraGameObject = new GameObject();
            Camera cameraComponent = cameraGameObject.AddComponent<Camera>();
            cameraComponent.orthographic = true;
            cameraGameObject.transform.position = new Vector3(1000, 0, -10);

            GameManager gameManager = spacecraft.AddComponent<GameManager>();
            SerializedObject gameManagerSerialized = new SerializedObject(gameManager);
            gameManagerSerialized.FindProperty("spacecraft").objectReferenceValue = spacecraftComponent;
            gameManagerSerialized.FindProperty("mainCamera").objectReferenceValue = cameraComponent;
            gameManagerSerialized.Update();

            yield return null;            
            
            ScoreManager.Instance.Score = 0;

            
            GameObject meteor = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Meteor"));

            Meteor meteorComponent = meteor.GetComponent<Meteor>();
            meteorComponent.InitNewMeteor(
                Meteor.MinimalSize * 0.9f, 
                new Vector2(1000, 0), 
                new Vector2(1, 0));

            GameObject bullet = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Bullet"));
            bullet.transform.rotation = Quaternion.Euler(0,0, 90);
            bullet.transform.position = new Vector3(1000, 0, 0); //same as meteor
            
            Bullet bulletComponent = bullet.GetComponent<Bullet>();
            bulletComponent.ResetBullet();
            
            yield return new WaitForFixedUpdate(); //Wait for physics to calculate collision

            Assert.Equals(ScoreManager.Instance.Score, 1);

            
            // cleanup
            Object.Destroy(meteor);
            Object.Destroy(bullet);
        }
         */
    }
}
