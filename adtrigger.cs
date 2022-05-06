using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class adtrigger : MonoBehaviour
{
    //������ ������ �ٰ����� ������ �ʴ� ��ü�� �浹���� ��
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

    //������ ������ ��� ��
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
    
