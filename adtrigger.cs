using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class adtrigger : MonoBehaviour
{
    //광고판 가까이 다가가서 보이지 않는 구체와 충돌했을 때
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "sphere")
        {
            Debug.Log("Trigger!");
            Transform adPanelTF = col.transform.root.Find("RawImage");
            BigAdPanel bigAdPanel = adPanelTF.GetComponent<BigAdPanel>();

            if (bigAdPanel == null)
            {
                SmallAdPanel smallAdPanel = adPanelTF.GetComponent<SmallAdPanel>();
                smallAdPanel.OnAdTriggerEnter();
            }
            else
            {
                bigAdPanel.OnAdTriggerEnter();
            }
        }
    }

    //광고판 주위을 벗어날 때
    void OnTriggerExit(Collider col)
    {
        if (col.tag == "sphere")
        {
            Debug.Log("Trigger!");
            Transform adPanelTF = col.transform.root.Find("RawImage");
            BigAdPanel bigAdPanel = adPanelTF.GetComponent<BigAdPanel>();

            if (bigAdPanel == null)
            {
                SmallAdPanel smallAdPanel = adPanelTF.GetComponent<SmallAdPanel>();
                smallAdPanel.OnAdTriggerExit();
            }
            else
            {
                bigAdPanel.OnAdTriggerExit();
            }
        }
    }
}
    
