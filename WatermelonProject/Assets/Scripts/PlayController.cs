using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayController : MonoBehaviour
{
    GameManager manager;

    public float clickCooltime = 0f;
    public NativeHashMap<int, FRUIT> fruitNum = new NativeHashMap<int, FRUIT>();

    //
    private FruitsStateManager fruit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        manager = GameManager.Instance;
        if (manager == null) return;
    }

    // Update is called once per frame
    void Update()
    {
        if (fruit != null) 
            fruit.transform.position = new Vector2(Input.mousePosition.x, fruit.transform.position.y);

        if (manager.state != GAMESTATE.PLAYING || manager.startPage.activeSelf) return;
        if (fruit == null) fruit = manager.GetFruit();

        if (Input.GetMouseButtonUp(0))
        {
            // 클릭 쿨타임
            if (Time.realtimeSinceStartup < manager.lastPickTime + clickCooltime) return;
            if (!fruit.TryGetComponent<FruitsStateManager>(out FruitsStateManager fsm)) return;

            manager.lastPickTime = Time.realtimeSinceStartup;

            //fruit.TryGetComponent<FruitsStateManager>(out FruitsStateManager fruitSM);
            fruit.state = FRUITSTATE.DROP;
            fruit.rigid.simulated = true;

            fruit = manager.GetFruit();
            //FRUIT fIndex = (FRUIT)UnityEngine.Random.Range(0, 2);
            //fruit.GetComponent<FruitsStateManager>()?.Init().SetFruitInfo(fIndex);
        }
    }
}
