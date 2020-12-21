using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelPerfectPosition : MonoBehaviour
{

	public static float pixelsPerUnit = 8;

	private Vector3 pixelPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        pixelPosition = new Vector3 (
        	Mathf.RoundToInt(transform.position.x*pixelsPerUnit),
        	Mathf.RoundToInt(transform.position.y*pixelsPerUnit),
        	Mathf.RoundToInt(transform.position.z*pixelsPerUnit));

        transform.position = pixelPosition/pixelsPerUnit;
    }

}
