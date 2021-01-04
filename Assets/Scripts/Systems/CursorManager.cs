using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [System.Serializable] //클래스 관리
    public class TheCursor
    {
        public string tag; //public string[] tags; -> element여러개
        public Texture2D cursorTexture;
    }

    public List<TheCursor> cursorList = new List<TheCursor>();

    // Start is called before the first frame update
    void Start()
    {
        SetCursorTexture(cursorList[0].cursorTexture);
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 1000))
        {
            for(int i = 0; i < cursorList.Count; i++)
            {
                if(hit.collider.tag == cursorList[i].tag)
                {
                    SetCursorTexture(cursorList[i].cursorTexture);
                    return;
                }
            }
        }
        SetCursorTexture(cursorList[0].cursorTexture);
    }

    void SetCursorTexture(Texture2D tex)
    {
        Cursor.SetCursor(tex, Vector2.zero, CursorMode.Auto);
    }
}
