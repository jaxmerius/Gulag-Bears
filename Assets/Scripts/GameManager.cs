using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject exit;
    public GameObject[] spawnPoints;
    public GameObject bear;
    public Text healthText;
    public Text scoreText;
    public AudioSource ambience;
    public GameObject pauseMenu;
    public Texture2D cursor;

    private bool isPaused = false;

    public int maxBearsOnScreen;
    public int totalBears;
    public float minSpawnTime;
    public float maxSpawnTime;
    public int bearsPerSpawn;

    private int bearsOnScreen = 0;
    private int bearsRemoved = 0;
    private float generatedSpawnTime = 0;
    private float currentSpawnTime = 0;

    private int playerHealth = 3;
    private int playerScore = 0;

    private float timer = 0f;
    private int seconds = 0;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
    }

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Start))
        {
            if (isPaused == false)
            {
                Time.timeScale = 0;
                pauseMenu.SetActive(true);
                isPaused = true;
            }
            else
            {
                Time.timeScale = 1;
                pauseMenu.SetActive(false);
                isPaused = false;
            }
        }

        timer += Time.deltaTime;
        seconds = (int)(timer % 60);

        currentSpawnTime += Time.deltaTime;

        if (currentSpawnTime > generatedSpawnTime)
        {
            currentSpawnTime = 0;

            generatedSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);

            if (bearsPerSpawn > 0 && bearsOnScreen < totalBears)
            {
                List<int> previousSpawnLocations = new List<int>();

                if (bearsPerSpawn > spawnPoints.Length)
                {
                    bearsPerSpawn = spawnPoints.Length - 1;
                }

                bearsPerSpawn = (bearsPerSpawn > totalBears) ? bearsPerSpawn - totalBears : bearsPerSpawn;

                for (int i = 0; i < bearsPerSpawn; i++)
                {
                    if (bearsOnScreen < maxBearsOnScreen)
                    {
                        bearsOnScreen += 1;

                        int spawnPoint = -1;

                        while (spawnPoint == -1)
                        {
                            int randomNumber = Random.Range(0, spawnPoints.Length);

                            if (!previousSpawnLocations.Contains(randomNumber))
                            {
                                previousSpawnLocations.Add(randomNumber);
                                spawnPoint = randomNumber;
                            }
                        }

                        GameObject spawnLocation = spawnPoints[spawnPoint];
                        GameObject newBear = Instantiate(bear) as GameObject;
                        newBear.transform.position = spawnLocation.transform.position;

                        Bear bearScript = newBear.GetComponent<Bear>();
                        bearScript.target = exit.transform;

                        Vector3 targetRotation = new Vector3(exit.transform.position.x, newBear.transform.position.y, exit.transform.position.z);
                        newBear.transform.LookAt(targetRotation);
                    }
                }
            }
        }
    }

    public void lowerHealth(int loss)
    {
        playerHealth = playerHealth - loss;
        string health = playerHealth.ToString();
        healthText.text = "Health: " + health;

        if (playerHealth <= 0)
        {
            end("Lose");
        }
    }

    public void addScore(int gain)
    {
        playerScore = playerScore + gain;
        string score = playerScore.ToString();
        scoreText.text = "Kills: " + score;
    }

    public void countRemovedBears()
    {
        bearsRemoved++;

        if (bearsRemoved >= totalBears)
        {
            end("Win");
        }
    }

    public void start()
    {
        SceneManager.LoadScene("Level");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
        ambience.Play();
    }

    public void end(string result)
    {
        ambience.Stop();

        if (result == "Win")
        {
            int finalScore = CalculateScore();
            PlayerPrefs.SetInt("score", finalScore);
            if (!PlayerPrefs.HasKey("highScore"))
            {
                PlayerPrefs.SetInt("highScore", finalScore);
                PlayerPrefs.SetString("isHigh", "true");
            }
            else
            {
                if (PlayerPrefs.GetInt("highScore") < finalScore)
                {
                    PlayerPrefs.SetInt("highScore", finalScore);
                    PlayerPrefs.SetString("isHigh", "true");
                }
                else
                {
                    PlayerPrefs.SetString("isHigh", "false");
                }
            }
            SceneManager.LoadScene("Win");
        }

        if (result == "Lose")
        {
            SceneManager.LoadScene("Lose");
        }

        Cursor.lockState = CursorLockMode.None;
    }

    public void restart()
    {
        SceneManager.LoadScene("Start");
        Cursor.lockState = CursorLockMode.None;
    }

    public int CalculateScore()
    {
        return ((playerScore + playerHealth) * 50 - seconds);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
