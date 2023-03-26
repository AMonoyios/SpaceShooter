using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EnviromentManager : MonoBehaviour
{
    [SerializeField]
    private EnviromentLayerData[] sceneLayers;

    // Start is called before the first frame update
    void Start()
    {
        float delta = (Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, Camera.main.farClipPlane)).z - PlayerController.Instance.transform.position.z) / sceneLayers.Length;
        float newChildPos = delta;
        for (int i = 0; i < sceneLayers.Length; i++)
        {
            if (!sceneLayers[i].enabled)
            {
                continue;
            }

            GameObject enviromentLayer = GameObject.CreatePrimitive(PrimitiveType.Quad);
            enviromentLayer.transform.parent = transform;

            enviromentLayer.AddComponent<MovingMesh>().Init(sceneLayers[i].moveFactor, sceneLayers[i].scaleToDeviceHeight, sceneLayers[i].material);
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).position = i == transform.childCount - 1 ?
                new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, newChildPos - 0.1f) :
                new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, newChildPos);

            newChildPos += delta;
        }
    }
}
