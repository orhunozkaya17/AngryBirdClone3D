using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    bool jump = false;
    Rigidbody rb;
    Transform cameraHolder;
    [SerializeField] float playerSpeed;
    [SerializeField] float jumpForce;

    Vector3 vec = new Vector3();
    [SerializeField] GameObject[] obstacle;
    [SerializeField] float obstacleDistance;
    [SerializeField] float obstacleDPosY;
    [SerializeField] int numberOfObstacles;
    bool isgameOver;
    int legnt;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cameraHolder = Camera.main.transform.parent;
       
    }
    void Start()
    {

        buildObstacle();
    }

    private void buildObstacle()
    {
        legnt = obstacle.Length;
        vec.z = 56.4f;
        for (int i = 0; i < numberOfObstacles; i++)
        {
            vec.z += obstacleDistance;
            vec.y = UnityEngine.Random.Range(-obstacleDPosY, obstacleDPosY);

            Instantiate(obstacle[UnityEngine.Random.Range(0, legnt)], vec, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            jump = true;
        }
        if (!isgameOver)
        {
            float playerY = transform.position.y;
            if (playerY <-32f||playerY>32f)
            {
                isgameOver = true;
                Invoke("ResartGame", .3f);
            }
        }
    }
    private void FixedUpdate()
    {
        rb.AddForce(Vector3.forward * playerSpeed * Time.fixedDeltaTime);
        if (jump)
        {
            rb.AddForce(Vector3.up * jumpForce * 1000 * Time.fixedDeltaTime);
            jump = false;
        }
    }
    private void LateUpdate()
    {
        vec.x = cameraHolder.transform.position.x;
        vec.y = cameraHolder.transform.position.y;
        vec.z = transform.position.z;
        cameraHolder.transform.position = vec;
    }
    private void OnCollisionEnter(Collision collision)
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;
        rb.useGravity = false;
        GetComponent<MeshRenderer>().enabled = false;
        transform.GetChild(1).GetComponent<ParticleSystem>().Play();
        rb.velocity = Vector3.zero;
        Invoke("ResartGame", 1);

    }
    void ResartGame()
    {
        SceneManager.LoadScene(0);
    }
}

