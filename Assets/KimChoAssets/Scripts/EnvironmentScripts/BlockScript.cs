using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour
{

    public bool isBreakable = true;
    public int hasBeenHit = 0;
    public int blockHitPoints = 1;
    public float timer = 0.0f;

    private SimpleDemos.DeleteObject_Example deleteScript;

    // Start is called before the first frame update
    void Start()
    {
        hasBeenHit = 0;
        timer = 0.0f;

        deleteScript = this.gameObject.GetComponent<SimpleDemos.DeleteObject_Example>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasBeenHit >= blockHitPoints && isBreakable)
        {
            spawnGrabbable();
            deleteScript.m_Delete = true;
            hasBeenHit = 0;
        }

        if (hasBeenHit > 0)
        {
            timer += Time.fixedDeltaTime;
        }

        if (timer >= 1f)
        {
            hasBeenHit = 0;
            timer = 0.0f;
        }
    }

    void spawnGrabbable()
    {
        string blockName = this.gameObject.name;
        blockName = blockName.Substring(0, blockName.IndexOf('('));

        ASL.ASLHelper.InstanitateASLObject("Grabbables/Blocks/" + blockName,
                        this.transform.position, Quaternion.identity, "InteractiveContainer", "",
                        SimpleDemos.CreateObject_Example.RepositionObject,
                        SimpleDemos.CreateObject_Example.ClaimRecoveryFunction,
                        null);
        ASL.ASLHelper.InstanitateASLObject("ASLSyncObject",
            this.transform.position, Quaternion.identity, "InteractiveContainer", "",
            SimpleDemos.CreateObject_Example.RepositionObject,
            SimpleDemos.CreateObject_Example.ClaimRecoveryFunction,
            null);
    }
}
