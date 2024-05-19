using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateCrates : MonoBehaviour
{
    [SerializeField] private GameObject generativeArea;
    [SerializeField] private int numberOfCrates;    
    [SerializeField] private int cratesWithItem;
    [SerializeField] private GameObject[] items;
    [SerializeField] private GameObject[] crateModels;

    [SerializeField] private GameObject[] area = new GameObject[2];

    private void Start()
    {
        generativeArea = this.gameObject;

        if (generativeArea == null)
        {
            Debug.LogError("Generative Area is not set in the inspector");
            return;
        }
        if (numberOfCrates == 0)
        {
            Debug.LogError("Number of crates is not set in the inspector");
            return;
        }
        if (cratesWithItem > numberOfCrates)
        {
            Debug.LogError("Number of crates with item is greater than number of crates");
            return;
        }

        Generate();
    }


    // QUICK AND VERY BAD GENERATOR!!!
    // this doesnt check for collision or if there is already an item there
    // just hope for the best!!!
    private void Generate()
    {
        // for (int i = 0; i < 10; i++)
        // {
        //     GameObject crate = Instantiate(Resources.Load("Prefabs/Crate"), generativeArea.transform) as GameObject;
        //     crate.transform.position = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0);
        // }
        Vector3[] occupied = new Vector3[numberOfCrates]; // add the size specifier before the variable's identifier

        for (int i = 0; i < numberOfCrates; i++)
        {
            GameObject crate = Instantiate(Random.Range(0, 1) > .5 ? crateModels[0] : crateModels[1], generativeArea.transform) as GameObject;
            // crate.transform.position = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0);
            occupied[i] = new Vector3(Random.Range(area[0].transform.position.x, area[1].transform.position.x), Random.Range(area[0].transform.position.y, area[1].transform.position.y), 0);
            if (i > 0 && occupied[i] == occupied[i - 1])
            {
                i--;
                continue;
            }
            crate.transform.position = occupied[i];

            // crate.transform.position = new Vector3(Random.Range(area[0].transform.position.x, area[1].transform.position.x), Random.Range(area[0].transform.position.y, area[1].transform.position.y), 0);
            if (i < cratesWithItem)
            {
                GameObject item = items[Random.Range(0, items.Length)];
                GameObject createdItem = Instantiate(item, crate.transform.position, Quaternion.identity);
                createdItem.AddComponent<BoxCollider2D>().isTrigger = true;
                createdItem.AddComponent<Rigidbody2D>();
                createdItem.AddComponent<Key>();
            }
        }
    }
}
