using UnityEngine;
using UnityEngine.AI;

public class Bear : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent agent;

    public float navigationUpdate;
    private float navigationTime = 0;

    GameObject manager;
    GameManager gameManager;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        manager = GameObject.Find("GameManager");
        gameManager = manager.GetComponent<GameManager>();

    }

    void Update()
    {
        navigationTime += Time.deltaTime;

        if (navigationTime > navigationUpdate)
        {
            agent.destination = target.position;
            navigationTime = 0;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Trap"))
        {
            gameManager.countRemovedBears();
            gameManager.addScore(1);
            Destroy(gameObject);
        }

        if (other.tag.Equals("Exit"))
        {
            gameManager.countRemovedBears();
            gameManager.lowerHealth(1);
            Destroy(gameObject);
        }
    }
}
