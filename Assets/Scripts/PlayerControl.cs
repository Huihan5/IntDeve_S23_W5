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

    GameObject bubbleForRemove;

    GameObject anotherBubbleForRemove;

    // Start is called before the first frame update
    void Start()
    {
        myAnim = GetComponent<Animator>();
        myRend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = transform.position;

        if (Input.GetKey(KeyCode.A))
        {
            newPos.x -= speed * Time.deltaTime;
            myAnim.SetBool("Hori", true);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            newPos.x += speed * Time.deltaTime;
            myRend.flipX = true;
            myAnim.SetBool("Hori", true);
        }
        else
        {
            myRend.flipX = false;
            myAnim.SetBool("Hori", false);
        }

        if (Input.GetKey(KeyCode.W))
        {
            newPos.y += speed * Time.deltaTime;
            myAnim.SetBool("Up", true);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            newPos.y -= speed * Time.deltaTime;
            myAnim.SetBool("Down", true);
        }
        else
        {
            myAnim.SetBool("Down", false);
            myAnim.SetBool("Up", false);
        }

        transform.position = newPos;

        //Trial

        Vector3 delta = transform.position - npcOutside.transform.position;
        distanceX = delta.x;
        distanceY = delta.y;
        distanceZ = delta.z;

        //Debug.Log(DistanceY);

        if ((-distanceLimit <= distanceX && distanceX <= distanceLimit) && (-distanceLimit-0.2 <= distanceY && distanceY <= distanceLimit))
        {
            Debug.Log("Detected");

            if (Input.GetKeyDown(KeyCode.Space))
            {
                mySource.PlayOneShot(speech);

                Vector3 bubblePos = new Vector3(transform.position.x, transform.position.y, transform.position.z);

                bubbleForRemove = Instantiate(speechBubble, bubblePos, Quaternion.identity);

                //bubbleForRemove.SetActive(true);

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
        else
        {
            if (bubbleForRemove != null)
            {
                bubbleForRemove.SetActive(false);
            }

            Speech1.SetActive(false);
            Speech2.SetActive(false);
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
                Vector3 bubblePos = new Vector3(transform.position.x, transform.position.y, transform.position.z);

                mySource.PlayOneShot(speech);
                anotherBubbleForRemove = Instantiate(speechBubble, bubblePos, Quaternion.identity);
                Speech3.SetActive(true);
            }
        }
        else
        {
            if (anotherBubbleForRemove != null)
            {
                anotherBubbleForRemove.SetActive(false);
            }

            Speech3.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D somecollision)
    {
        Debug.Log(somecollision.gameObject.name);

        if (somecollision.gameObject.name == "obj_npcOutside")
        {
            if (Input.GetKey(KeyCode.L))
            {
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
            if (Input.GetKey(KeyCode.L))
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
