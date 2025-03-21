using System.Collections.Generic;
using UnityEngine;

public class ToolController : MonoBehaviour
{
    [SerializeField] List<GameObject> tools;
    [SerializeField] int activeTool = -1;
    KeyCode lastPressedKey = 0;

    void Start()
    {
        firstRun();
    }

    void firstRun() {
        if (activeTool < 0 || activeTool >= tools.Count || tools[activeTool] == null) {
            for (int i = 0; i < tools.Count; i++) {
                if (tools[i] != null) {
                    activeTool = i;
                    break;
                }
            }
        }

        for (int i = 0; i < tools.Count; i++) {
            tools[i].SetActive(i == activeTool);
        }
    }

    public void disableAll() {
        for (int i = 0; i < tools.Count; i++) {
            tools[i].SetActive(false);
        }
    }

    public bool setTool(int idx) {
        if (idx < 0 || idx >= tools.Count)
            return false;

        if (tools[idx] == null)
            return false;

        activeTool = idx;
        for (int i = 0; i < tools.Count; i++) {
            tools[i].SetActive(i == idx);
            if (i == idx)
                updateStatus(tools[i]);
        }

        return true;
    }

    void updateStatus(GameObject obj) {
        WeaponController wc = obj.GetComponent<WeaponController>();
        if (wc != null) {
            wc.changedToActive();
        }
    }

    void FixedUpdate()
    {
        KeyCode[] keys = new KeyCode[] {KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5};

        for (int i = 0; i < keys.Length; i++) {
            if (Input.GetKey(keys[i]) && lastPressedKey != keys[i]) {
                lastPressedKey = keys[i];
                setTool(i);
            }
            lastPressedKey = keys[i];
        }
    }

    public int getToolId() {
        return activeTool;
    }
}
