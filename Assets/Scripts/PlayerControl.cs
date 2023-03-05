using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerControl : MonoBehaviour
{

    public float speed = 2f;
    public float timer = 20;
    public float distanceLimit = 2f;

    Rigidbody2D myBody;
    Animator myAnim;
    SpriteRenderer myRend;

    public GameObject Speech1;
    public GameObject Speech2;
    public GameObject Speech3;

    public AudioSource mySource;
    public AudioClip walking;
    public AudioClip collectKey;
    public AudioClip doorOpen;
    public AudioClip speech;

    bool GetKey = false;

    //Trial

    public GameObject npcOutside;
    public GameObject npcInside;

    float distanceX;
    float distanceY;
    float distanceZ;

    float DistanceX;
    float DistanceY;
    float DistanceZ;

    public GameObject speechBubble;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = transform.position;

        if (Input.GetKey(KeyCode.A))
        {
            newPos.x -= speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            newPos.x += speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.W))
        {
            newPos.y += speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            newPos.y -= speed * Time.deltaTime;
        }

        transform.position = newPos;

        //Trial

        Vector3 delta = transform.position - npcOutside.transform.position;
        distanceX = delta.x;
        distanceY = delta.y;
        distanceZ = delta.z;

        Debug.Log(DistanceY);

        if ((-distanceLimit <= distanceX && distanceX <= distanceLimit) && (-distanceLimit-0.2 <= distanceY && distanceY <= distanceLimit))
        {
            Debug.Log("Detected");

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Ready Sir");

                Vector3 bubblePos = new Vector3(transform.position.x, transform.position.y, transform.position.z);

                mySource.PlayOneShot(speech);

                if (GetKey)
                {
                    //GameObject bubbleForRemove = Instantiate(speechBubble, bubblePos, Quaternion.identity);
                    Speech2.SetActive(true);
                }
                else
                {
                    //Instantiate(speechBubble, bubblePos, Quaternion.identity);
                    Speech1.SetActive(true);
                }
            }
        }
        else
        {
            Speech1.SetActive(false);
            Speech2.SetActive(false);
            //Destroy(bubbleForRemove);
        }

        //Second

        Vector3 alpha = transform.position - npcInside.transform.position;
        DistanceX = alpha.x;
        DistanceY = alpha.y;

        if ((-distanceLimit <= DistanceX && DistanceX <= distanceLimit) && (-distanceLimit - 0.2 <= DistanceY && DistanceY <= distanceLimit))
        {
            Debug.Log("Detected Again");

            if (Input.GetKeyDown(KeyCode.Space))
            {
                mySource.PlayOneShot(speech);
                Speech3.SetActive(true);
            }
        }
        else
        {
            Speech3.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D somecollision)
    {
        Debug.Log(somecollision.gameObject.name);

        if (somecollision.gameObject.name == "obj_npcOutside")
        {

            if (Input.GetKey(KeyCode.Space))
            {

                Debug.Log("Ready Sir");

                mySource.PlayOneShot(speech);

                if (GetKey)
                {

                    Speech2.SetActive(true);
                }
                else
                {

                    Speech1.SetActive(true);
                }
            }
        }

        if (somecollision.gameObject.name == "obj_key")
        {
            GetKey = true;
            mySource.PlayOneShot(collectKey);
            Destroy(somecollision.gameObject);
        }

        if (somecollision.gameObject.name == "obj_gate" && GetKey)
        {
            mySource.PlayOneShot(doorOpen);
            Destroy(somecollision.gameObject);
        }

        if(somecollision.gameObject.name == "obj_npcInside")
        {
            if (Input.GetKey(KeyCode.Space))
            {
                mySource.PlayOneShot(speech);
                Speech3.SetActive(true);
            }
        }

        if (somecollision.gameObject.name == "obj_exit")
        {
            SceneManager.LoadScene(0);
        }
    }
}
