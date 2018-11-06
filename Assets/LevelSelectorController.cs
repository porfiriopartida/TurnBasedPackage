using UnityEngine;

public class LevelSelectorController : MonoBehaviour
{
    private GameObject ready;
    private GameObject done;
    private GameObject closed;

    public int status = 1;

    // Use this for initialization
    void Start ()
    {
        ready = transform.Find("ready").gameObject;
        done = transform.Find("done").gameObject;
        closed = transform.Find("closed").gameObject;
        setStatus(status);
    }

    public void setStatus(int i) {
        switch (i)
        {
            case 1:
                ready.SetActive(false);
                done.SetActive(false);
                closed.SetActive(true);
                break;
            case 2:
                ready.SetActive(true);
                done.SetActive(false);
                closed.SetActive(false);
                break;
            case 3:
                ready.SetActive(false);
                done.SetActive(true);
                closed.SetActive(false);
                break;
        }
    }
}
