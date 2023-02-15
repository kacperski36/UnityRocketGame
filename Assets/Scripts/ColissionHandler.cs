using UnityEngine;
using UnityEngine.SceneManagement;

public class ColissionHandler : MonoBehaviour
{
    AudioSource collisiontSound;
    ParticleSystem doEffect;
    [SerializeField] float loadDelay = 2f;
    [SerializeField] AudioClip finish;
    [SerializeField] AudioClip crash;

    [SerializeField] ParticleSystem finishParticle;
    [SerializeField] ParticleSystem crashParticle;
    bool isTransitioning = false;
    bool collisionStatus = false;
    void Start()
    {
        collisiontSound = GetComponent<AudioSource>();
        doEffect = GetComponent<ParticleSystem>();

    }
    void Update()
    {
        RespondToDebugKeys();
    }
    void RespondToDebugKeys()
    {
        if (Input.GetKey(KeyCode.L))
        {
            NextLevel();
        }
        else if (Input.GetKey(KeyCode.C))
        {
            collisionStatus = !collisionStatus;
            Debug.Log(collisionStatus);
        }
    }
    void OnCollisionEnter(Collision other)
    {
        if (isTransitioning || collisionStatus) { return; }
        else
        {
            switch (other.gameObject.tag)
            {
                case "Finish":
                    StartNewLevel();
                    break;
                case "Friendly":
                    Debug.Log("NIC");
                    break;
                default:
                    StartCrash();
                    break;
            }
        }

    }
    void StartCrash()
    {
        isTransitioning = true;
        collisiontSound.Stop();
        collisiontSound.PlayOneShot(crash);
        crashParticle.Play();
        GetComponent<Movment>().enabled = false;
        Invoke("ReloadLevel", loadDelay);
    }
    void StartNewLevel()
    {
        isTransitioning = true;
        collisiontSound.Stop();
        collisiontSound.PlayOneShot(finish);
        finishParticle.Play();
        GetComponent<Movment>().enabled = false;
        Invoke("NextLevel", loadDelay);

    }
    void NextLevel()
    {
        int sceneId = SceneManager.GetActiveScene().buildIndex;
        int nextSceneId = sceneId + 1;
        if (nextSceneId == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneId = 0;
        }
        SceneManager.LoadScene(nextSceneId);

    }

    void ReloadLevel()
    {
        int sceneId = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneId);
    }
}
