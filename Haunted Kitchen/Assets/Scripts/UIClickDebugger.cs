using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIClickDebugger : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PointerEventData data = new PointerEventData(EventSystem.current);
            data.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(data, results);

            Debug.Log("---- UI objects under cursor (top = blocking) ----");

            for (int i = 0; i < results.Count; i++)
            {
                Debug.Log($"{i}: {results[i].gameObject.name}");
            }

            if (results.Count == 0)
            {
                Debug.Log("No UI hit. Click passed through UI.");
            }
        }
    }
}
