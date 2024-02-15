using SFB;
using UnityEngine;
using UnityEngine.UI;

public class dd : MonoBehaviour
{


    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(() =>
        {
            var path = StandaloneFileBrowser.SaveFilePanel("Save File", "", "", "");

        });
    }

    void Update()
    {
        
    }
}
