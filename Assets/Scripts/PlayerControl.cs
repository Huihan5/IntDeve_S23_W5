using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerControl : MonoBehaviour
{

    public float speed = 2f;
    public float timer = 20;

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

    }

    private void OnCollisionEnter2D(Collision2D somecollision)
    {
        Debug.Log(somecollision.gameObject.name);

        if (somecollision.gameObject.name == "obj_npcOutside")
        {
            if (Input.GetKey(KeyCode.Space))
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
