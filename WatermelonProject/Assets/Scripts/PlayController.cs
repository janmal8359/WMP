using UnityEngine;

public class PlayController : MonoBehaviour
{
    public GameManager manager;

    public float clickCooltime = 0f;


    private GameObject fruit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (manager == null) return;

        fruit = manager.nextFruit;
    }

    // Update is called once per frame
    void Update()
    {
        if (manager.state != GAMESTATE.PLAYING) return;

        if (fruit == null) fruit = manager.nextFruit;
        fruit.transform.position = new Vector2(Input.mousePosition.x, fruit.transform.position.y);

        if (Input.GetMouseButtonUp(0))
        {
            // 클릭 쿨타임
            if (Time.realtimeSinceStartup < manager.lastPickTime + clickCooltime) return;
            if (fruit.GetComponent<FruitsStateManager>() == null) return;

            manager.lastPickTime = Time.realtimeSinceStartup;
            FruitsStateManager fruitSM = fruit.GetComponent<FruitsStateManager>();
            fruitSM.state = FRUITSTATE.DROP;
            fruitSM.rigid.simulated = true;

            manager.GetNextFruit();
            fruit = manager.nextFruit;
        }
    }
}
