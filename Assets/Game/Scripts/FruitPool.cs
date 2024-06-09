using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace tastelikecoke.PanMachine
{
    public class FruitPool : MonoBehaviour
    {
        [SerializeField]
        private Transform _fruitRoot;

        [SerializeField]
        private Dictionary<string, List<Fruit>> fruitPool;

        public void ResetAll()
        {
            if (fruitPool == null) return;
            
            foreach (var key in fruitPool.Keys)
            {
                for (int i = 0; i < fruitPool[key].Count; i++)
                {
                    fruitPool[key][i].Hide();
                }
            }
        }
        
        public GameObject GetObject(GameObject fruitObject, Transform fruitRoot)
        {
            if (fruitPool == null)
                fruitPool = new Dictionary<string, List<Fruit>>();
            
            var fruitScript = fruitObject.GetComponent<Fruit>();
            Debug.Log($"Spawning Item{fruitScript.fruitID}");
            if (!fruitPool.ContainsKey(fruitScript.fruitID))
            {
                fruitPool.Add(fruitScript.fruitID, new List<Fruit>());
            }

            var fruitPoolList = fruitPool[fruitScript.fruitID];

            for (int i = 0; i < fruitPoolList.Count; i++)
            {
                if (fruitPoolList[i].isHidden)
                {
                    fruitPoolList[i].transform.SetParent(fruitRoot);
                    fruitPoolList[i].transform.position = fruitRoot.transform.position;
                    fruitPoolList[i].transform.localScale = Vector3.one;
                    fruitPoolList[i].Reset();
                    return fruitPoolList[i].gameObject;
                }
            }
            

            var newFruit = Instantiate(fruitObject, fruitRoot);
            var newFruitScript = newFruit.GetComponent<Fruit>();
            newFruitScript.pool = this;
            newFruit.transform.SetParent(fruitRoot);

            if (newFruitScript != null)
                fruitPoolList.Add(newFruit.GetComponent<Fruit>());

            Debug.Log($"New Item{newFruitScript.fruitID}");
            return newFruit;
        }
    }
}
