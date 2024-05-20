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
    // Vector3[] occupied = new Vector3[numberOfCrates]; // add the size specifier before the variable's identifier
    List<Vector3> occupied = new List<Vector3>();

    private void Start()
    {
        generativeArea = this.gameObject;

        if (area.Length != 2)
            Debug.LogError("Area is not set in the inspector");

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
    // this doesnt really spread out the items very well
    // which may lead to items being veyr close to each other
    private void Generate()
    {
        // for (int i = 0; i < 10; i++)
        // {
        //     GameObject crate = Instantiate(Resources.Load("Prefabs/Crate"), generativeArea.transform) as GameObject;
        //     crate.transform.position = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0);
        // }
        // Vector3[] occupied = new Vector3[numberOfCrates]; // add the size specifier before the variable's identifier

        for (int i = 0; i < numberOfCrates - cratesWithItem; i++)
        {
            // GameObject crate = Instantiate(Random.Range(0, 1) > .5 ? crateModels[0] : crateModels[1], generativeArea.transform) as GameObject;
            GameObject crate = Instantiate(crateModels[Random.Range(0, crateModels.Length)], generativeArea.transform) as GameObject;
            // crate.transform.position = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0);
            occupied.Add(new Vector3(Random.Range(area[0].transform.position.x, area[1].transform.position.x), Random.Range(area[0].transform.position.y, area[1].transform.position.y), 0));
            if (i > 0 && occupied[i] == occupied[i - 1]) // this doesnt even work it has to loop throug the rest behind it to check iof its being occupied by another crate
            {
                i--;
                continue;
            }
            crate.transform.position = occupied[i];

            // crate.transform.position = new Vector3(Random.Range(area[0].transform.position.x, area[1].transform.position.x), Random.Range(area[0].transform.position.y, area[1].transform.position.y), 0);
            // if (i < cratesWithItem)
            // {
            //     Debug.Log("Creating item");
            //     GameObject item = items[Random.Range(0, items.Length)];
            //     GameObject createdItem = Instantiate(item, crate.transform.position, Quaternion.identity);
            //     createdItem.AddComponent<BoxCollider2D>().isTrigger = true;
            //     createdItem.AddComponent<Rigidbody2D>();
            //     createdItem.AddComponent<Key>();
            // }
        }

        for (int i = 0; i < cratesWithItem; i++)
        {
            Debug.Log("Second loop");
            GameObject crate = Instantiate(crateModels[Random.Range(0, crateModels.Length)], generativeArea.transform) as GameObject;
            occupied.Add(new Vector3(Random.Range(area[0].transform.position.x, area[1].transform.position.x), Random.Range(area[0].transform.position.y, area[1].transform.position.y), 0));
            // this doesnt even work it has to loop throug the rest behind it to check iof its being occupied by another crate
            if (i > 0 && occupied[i] == occupied[i - 1]) // add numberOfCrates to i because we are starting from the second loop
            {
                i--;
                continue;
            }
            crate.transform.position = occupied[i];
            crate.GetComponent<Crate>().item = items[0];

            // crate.transform.position = new Vector3(Random.Range(area[0].transform.position.x, area[1].transform.position.x), Random.Range(area[0].transform.position.y, area[1].transform.position.y), 0);
        }
    }
}
