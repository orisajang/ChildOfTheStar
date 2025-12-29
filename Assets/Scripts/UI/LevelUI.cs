using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelUI : MonoBehaviour
{
    [SerializeField] float _waitTime = 2f;

    private void Start()
    {
        StartCoroutine(StartDisplay());
    }
    private IEnumerator StartDisplay()
    {
        yield return new WaitForSeconds(_waitTime);
        Destroy(gameObject);
    }

}
