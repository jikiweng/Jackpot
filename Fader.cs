using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

//Attach to fader, which controls language, difficulty and volume.
public class Fader : MonoBehaviour
{
    Coroutine currentAction = null;

    [SerializeField]
    float fadeOutTime = 1f;

    [SerializeField]
    float fadeInTime = 1f;

    [SerializeField]
    GameObject loadingText = null;

    private void Start()
    {
        //all the object insides will be brought to the new scene.
        DontDestroyOnLoad (gameObject);
    }

    //Called by any other object wants move to another scene.
    public void LoadNewScene(int sceneIndex)
    {
        StartCoroutine(Transition(sceneIndex));
    }

    //Controls current coroutine action.
    public Coroutine Fade(float target, float time)
    {
        //stop the running coroutine if there is a new one.
        if (currentAction != null) StopCoroutine(currentAction);

        currentAction = StartCoroutine(fadeRoutine(target, time));
        return currentAction;
    }

    //Change the alpha for this gameObjectto the target value.
    private IEnumerator fadeRoutine(float target, float time)
    {
        //the function can change the alpha smoothhly.
        while (!Mathf.Approximately(GetComponent<CanvasGroup>().alpha, target))
        {
            GetComponent<CanvasGroup>().alpha =
                Mathf
                    .MoveTowards(GetComponent<CanvasGroup>().alpha,
                    target,
                    Time.deltaTime / time);
            yield return null;
        }
    }

    //Move to another scece.
    public IEnumerator Transition(int sceneIndex)
    {
        if (sceneIndex < 0)
        {
            print("please set the sceneIndex.");
            yield break;
        }

        //the screen will be white.
        yield return Fade(1f, fadeOutTime);

        loadingText.SetActive(true);

        //after new scene loaded, the color of the screen fades out.
        yield return SceneManager.LoadSceneAsync(sceneIndex);

        loadingText.SetActive(false);

        Fade(0f, fadeInTime);
        yield break;
    }
}
