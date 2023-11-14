using UnityEngine;

public class FixScale : MonoBehaviour
{
    public Transform parent;

    // Update is called once per frame
    void Update()
    {
        if(parent.localScale.x > 0 && transform.localScale.x < 0 || parent.localScale.x < 0 && transform.localScale.x > 0)
		{
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
		}
    }
}