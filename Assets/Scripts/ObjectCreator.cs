using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ObjectCreator : MonoBehaviour
{
    public GameObject Sphere;
    public GameObject Capsule;
    public GameObject Cube;

    public LevelManager levelManager;
    public List<GameObject> ObjectList = new List<GameObject>();

    public void StartGame()
    {
        StartCoroutine(EatableCreator());
        StartCoroutine(CubeCreator());
    }
    public IEnumerator EatableCreator()
    {
        int randomNumObject = Random.Range(1, levelManager.Level+3);
        for (int i = 0; i < randomNumObject; i++)
        {
            GameObject eatable;
            int randomNumber = Random.Range(0, levelManager.GridList.Count);
            int randomObject = Random.Range(0, 2);
            yield return new WaitForSeconds(randomObject + 1 * 0.5f);
            Grid choosenGrid = levelManager.GridList[randomNumber].GetComponent<Grid>();
            if (randomObject == 0)
            {
                eatable = Sphere;
            }
            else
            {
                eatable = Capsule;
            }

            if (!choosenGrid.isEmpty)
            {
                choosenGrid.isEmpty = true;
                GameObject created = Instantiate(eatable, new Vector3(choosenGrid.transform.position.x, choosenGrid.transform.position.y + 1.5f, choosenGrid.transform.position.z), Quaternion.identity);
                created.GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
                created.transform.DOMoveY(choosenGrid.transform.position.y + 0.5f, 0.3f);
                created.transform.SetParent(choosenGrid.transform);
                ObjectList.Add(created);
            }
        }

        yield return new WaitForSeconds(levelManager.Level + 1 * 0.5f);
    }

   public IEnumerator CubeCreator()
    {
        int randomNumObject = Random.Range(1, 10);
        yield return new WaitForSeconds(6);
        if (randomNumObject>4)
        {
            int randomNumber = Random.Range(0, levelManager.GridList.Count);
            Grid choosenGrid = levelManager.GridList[randomNumber].GetComponent<Grid>();
            if (!choosenGrid.isEmpty)
            {
                choosenGrid.isEmpty = true;
                GameObject createdBlock = Instantiate(Cube, new Vector3(choosenGrid.transform.position.x, choosenGrid.transform.position.y +1f, choosenGrid.transform.position.z), Quaternion.identity);
                createdBlock.transform.DOMoveY(choosenGrid.transform.position.y + 0.45f, 0.3f);
                createdBlock.transform.SetParent(choosenGrid.transform);
                ObjectList.Add(createdBlock);
            }
        }
        StartCoroutine(CubeCreator());
    }

    public void DestroyAll()
    {
        StopCoroutine(CubeCreator());
        StopCoroutine(EatableCreator());
        for (int i = 0; i < ObjectList.Count; i++)
        {
            Destroy(ObjectList[i].gameObject);
        }
        ObjectList.Clear();
    }
}
