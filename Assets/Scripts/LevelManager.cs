using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    #region Scripts
    public ObjectCreator objectCreator;
    public ScoreManager scoreManager;
    #endregion
    public GameObject Player;
    public GameObject Floor;
    public GameObject Barrier;
    public GameObject MiddlePoint;
    GameObject playerCreated;
    public Camera cam;
    public Vector3 camStartPos;
    public Vector3 NewLevelFirstPos;
    public Grid WhiteGrid;
    public Grid BlackGrid;
    public List<Grid> GridList = new List<Grid>();
    public List<GameObject> BarrierList = new List<GameObject>();
    public int Level;
    public int StartFloorLevel;
    public bool isLevelUp;


    private void Start()
    {
        camStartPos = cam.transform.position;
    }
    public IEnumerator FloorMaker()
    {
        for (int i = 0; i < Level + StartFloorLevel; i++)
        {
            for (int j = 0; j < Level + StartFloorLevel; j++)
            {
                Grid Grid;
                if ((GridList.Count) % 2 == 0)
                {
                    Grid = Instantiate(WhiteGrid, new Vector3(j, 1, i), Quaternion.identity);
                }
                else
                {
                    Grid = Instantiate(BlackGrid, new Vector3(j, 1, i), Quaternion.identity);
                }
                Grid.transform.DOMoveY(0, 0.15f);
                GridList.Add(Grid);
                Grid.transform.SetParent(Floor.transform);
                yield return new WaitForSeconds(0.1f);
            }
        }
        PlayerCreator(GridList[GridList.Count / 2]);
        BarrierCreator();
        objectCreator.StartGame();
        isLevelUp = false;
    }

    public void PlayerCreator(Grid MiddleGrid)
    {
        playerCreated = Instantiate(Player, new Vector3(MiddlePoint.transform.position.x, MiddlePoint.transform.position.y + 1, MiddlePoint.transform.position.z), Quaternion.identity);
        playerCreated.transform.DOMoveY(MiddleGrid.transform.position.y + 0.6f, 0.3f);
    }

    public void BarrierCreator()
    {
        BarrierCleaner();
        GameObject upbarrier = Instantiate(Barrier, new Vector3(MiddlePoint.transform.position.x, MiddlePoint.transform.position.y, MiddlePoint.transform.position.z + ((StartFloorLevel + 1) / 2)), Quaternion.identity);
        GameObject downbarrier = Instantiate(Barrier, new Vector3(MiddlePoint.transform.position.x, MiddlePoint.transform.position.y, MiddlePoint.transform.position.z - ((StartFloorLevel + 1) / 2)), Quaternion.identity);
        GameObject leftbarrier = Instantiate(Barrier, new Vector3(MiddlePoint.transform.position.x + ((StartFloorLevel + 1) / 2), MiddlePoint.transform.position.y, MiddlePoint.transform.position.z), Quaternion.identity);
        GameObject rightbarrier = Instantiate(Barrier, new Vector3(MiddlePoint.transform.position.x - ((StartFloorLevel + 1) / 2), MiddlePoint.transform.position.y, MiddlePoint.transform.position.z), Quaternion.identity);
        upbarrier.transform.DOScaleX(Level + StartFloorLevel+2, 0.1f);
        downbarrier.transform.DOScaleX(Level + StartFloorLevel+2, 0.1f);
        leftbarrier.transform.DOScaleZ(Level + StartFloorLevel+2, 0.1f);
        rightbarrier.transform.DOScaleZ(Level + StartFloorLevel+2, 0.1f);
        BarrierList.Add(upbarrier);
        BarrierList.Add(downbarrier);
        BarrierList.Add(leftbarrier);
        BarrierList.Add(rightbarrier);
    }

    public void StartGame()
    {
        scoreManager.StartGame();
        StartFloorLevel = 7;      
        cam.transform.DOMove(camStartPos,0.25f);
        Level = 0;
        StartCoroutine(FloorMaker());
    }
    public void GameEnding()
    {
        StopCoroutine(objectCreator.EatableCreator());
        StopCoroutine(objectCreator.CubeCreator());
        Destroy(playerCreated);
        foreach (Transform child in Floor.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        GridList.Clear();
        objectCreator.DestroyAll();
    }

  
    public void LevelUp()
    {
        isLevelUp = true;
        Level++;
        StartFloorLevel+=2;
        StartCoroutine(FloorLevelUp());
        cam.transform.DOMove(new Vector3(camStartPos.x, camStartPos.y + Level, camStartPos.z - Level), 0.25f);
    }
       

    void BarrierCleaner()
    {
        for (int i = 0; i < BarrierList.Count; i++)
        {
            Destroy(BarrierList[i].gameObject);
        }
    }

    public IEnumerator FloorLevelUp()
    {
        
        bool temp = true;
        NewLevelFirstPos = new Vector3(GridList[0].transform.position.x- Level, GridList[0].transform.position.y, GridList[0].transform.position.z- Level);
        for (int i = 0; i <StartFloorLevel; i++)
        {    
            for (int j = 0; j <StartFloorLevel; j++)
            {
                Grid Grid;
                if(i == 0 || i == (StartFloorLevel -1))
                {
                    if (temp)
                    {
                        Grid = Instantiate(WhiteGrid, new Vector3(NewLevelFirstPos.x + j, 1, NewLevelFirstPos.z+i), Quaternion.identity);
                       
                    }
                    else
                    {
                        Grid = Instantiate(BlackGrid, new Vector3(NewLevelFirstPos.x + j, 1, NewLevelFirstPos.z+i), Quaternion.identity);
                       
                    }
                    Grid.transform.DOMoveY(0, 0.1f);
                    GridList.Add(Grid);
                    Grid.transform.SetParent(Floor.transform);
                    yield return new WaitForSeconds(0.05f);
                }
                else
                {
                    
                    if (j==0 || j == (StartFloorLevel - 1))
                    {
                        if (temp)
                        {
                            Grid = Instantiate(WhiteGrid, new Vector3(NewLevelFirstPos.x + j, 1, NewLevelFirstPos.z+i), Quaternion.identity);                       
                        }
                        else
                        {
                            Grid = Instantiate(BlackGrid, new Vector3(NewLevelFirstPos.x + j, 1, NewLevelFirstPos.z+i), Quaternion.identity);
                        
                        }
                        Grid.transform.DOMoveY(0, 0.1f);
                        GridList.Add(Grid);
                        Grid.transform.SetParent(Floor.transform);
                        yield return new WaitForSeconds(0.05f);
                    }
                    

                }
                temp = !temp;

            }

        }
        BarrierCreator();
        isLevelUp = false;
    }
}
