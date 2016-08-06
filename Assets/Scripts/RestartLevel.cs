using UnityEngine;
using System.Collections;

public class RestartLevel : MonoBehaviour {

    public void Restart()
    {
        GameManager.Instance.LoadLevel(-1);
    }
}
