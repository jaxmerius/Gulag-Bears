using UnityEngine;

public class TrapManager : MonoBehaviour
{
    GameObject trap;
    Trap trapScript;

    void Start()
    {
        trap = GameObject.Find("Trap");
        trapScript = trap.GetComponent<Trap>();
    }

    private void OnBecameInvisible()
    {
        trapScript.playCloseSound();
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        trapScript.playCloseSound();
        Destroy(gameObject);
    }
}
