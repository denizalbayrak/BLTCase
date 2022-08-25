using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    LevelManager levelManager;
    ScoreManager scoreManager;
    GameManager gameManager;
    float speed = 5.0f;
    string lastEated ="";

    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        scoreManager = FindObjectOfType<ScoreManager>();
        gameManager = FindObjectOfType<GameManager>();
        speed = 5.0f;
    }
    public float panSpeed = 20f;
    RaycastHit hit;
    void Update()
    {
        if (!levelManager.isLevelUp)
        {
            Vector3 pos = transform.position;

            if (Input.GetKey("w"))
            {
                pos.z += speed * Time.deltaTime;
            }
            if (Input.GetKey("s"))
            {
                pos.z -= speed * Time.deltaTime;
            }
            if (Input.GetKey("d"))
            {
                pos.x += speed * Time.deltaTime;
            }
            if (Input.GetKey("a"))
            {
                pos.x -= speed * Time.deltaTime;
            }

            transform.position = pos;
        }
        if ((Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 0.7f )) && (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.back), out hit, 0.7f)) && (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hit, 0.7f)) && (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left), out hit, 0.7f)))
        {
            gameManager.GameOver();
        }
    }

    void OnTriggerEnter(Collider other)
    {
   
        if (other.gameObject.tag == "Sphere" || other.gameObject.tag == "Capsule")
        {
            StartCoroutine(FindObjectOfType<ObjectCreator>().EatableCreator());
            other.transform.parent.GetComponent<Grid>().isEmpty = false;
            Destroy(other.gameObject);

            if (other.gameObject.tag.Equals(lastEated))
            {
                scoreManager.LastEatedScore(lastEated);
            }

            else if (other.gameObject.tag == "Sphere")    
            {
                lastEated = other.gameObject.tag;
                scoreManager.SphereScore();
            }
            else if(other.gameObject.tag == "Capsule")
            {
                lastEated = other.gameObject.tag;
                scoreManager.CapsuleScore();
            }          

        }
    }

    
}