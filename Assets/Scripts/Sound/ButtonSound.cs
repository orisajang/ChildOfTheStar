using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    public string soundName;

    public void PlayClickSound()
    {
        SoundManager.Instance.PlayEffect(soundName);
    }
}
