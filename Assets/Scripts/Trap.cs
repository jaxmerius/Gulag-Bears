using UnityEngine;


public class Trap : MonoBehaviour
{
    public GameObject trapPrefab;
    public Transform throwPos;
    public AudioSource placeTrap;
    public AudioSource trapSnap;
    public AudioSource bearRoar;
    public AudioSource fleshRip;

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            if (!IsInvoking("throwTrap"))
            {
                Invoke("throwTrap", 0f);
            }
        }
    }

    void throwTrap()
    {
        GameObject trap = Instantiate(trapPrefab) as GameObject;
        trap.transform.position = throwPos.transform.position;
        for (int i = 5; i >= 0; i--)
        {
            trap.GetComponent<Rigidbody>().velocity = transform.parent.forward * i;
        }
        placeTrap.Play();
    }

    public void playCloseSound()
    {
        Debug.Log("palying close sound");
        bearRoar.Play();
        fleshRip.Play();
        trapSnap.Play();
    }
}