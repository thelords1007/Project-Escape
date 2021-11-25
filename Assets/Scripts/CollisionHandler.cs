using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 1f;
    [SerializeField] AudioClip crash;
    [SerializeField] AudioClip success;

    [SerializeField] ParticleSystem crashParticle;
    [SerializeField] ParticleSystem successParticle;
    AudioSource audioSource;
    bool isTransitioning = false;   //so that finish status cant be overlapped by crash status while in delay phase and vice versa
    bool collisionDisable = false;  //for cheat key 'C'

    void Start()
    {
        audioSource = GetComponent<AudioSource>();    
    }

    void Update()
    {
        RespondToDebugKeys();
    }

    void RespondToDebugKeys()       //Cheat Keys
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if(Input.GetKeyDown(KeyCode.C))
        {
            collisionDisable = !collisionDisable;  //toggle collision(change value of collision wrt its current value)
        }
    }

    private void OnCollisionEnter(Collision other) 
    {
        if(isTransitioning || collisionDisable) {   return;    }
        switch(other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Collided with launch pad");
                break;
            case "Finish":
                GoToNextLevelSequence();
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    void StartCrashSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(crash);//todo add SFX upon crash
        crashParticle.Play();//todo add particle effect upon crash
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", levelLoadDelay);
    }

    void GoToNextLevelSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(success);//todo add SFX upon success
        successParticle.Play();//todo add particle effect upon success
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", levelLoadDelay);
    }
    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
}
