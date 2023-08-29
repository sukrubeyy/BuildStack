using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    [Header("Spawn Points")] [SerializeField]
    private Spawner[] spawners;

    [Header("Spawn Points Array Index")] [SerializeField]
    private int spawnerIndex;

    [Header("Spawner Particle Class Scrip (Enum Script) )")] [SerializeField]
    private Spawner currentSpawner;

    public static GameControl instance;
    private void Awake()
    {
        //Mevcut bulunan iki spawn objesine ulaşıp spawners dizisine atıyoruz.
        spawners = FindObjectsOfType<Spawner>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //MoveCube içerisinde bulunan statik obje olan currentCube null değilse hareket eden cube dursun.
            if (MoveCube.currentCube != null)
                MoveCube.currentCube.Stop();

            //fakat oyun ilk başlangıcında bu değer null olduğu için alt satırda SpawnCube methoduna doğru ilerleyecektir.
            //spawnerIndex değeri eğer 0'a eşitse 1 değerini ata 
            //1'e eşitse 0 değeri ataması yap. 
            //Bu sayede bir X ekseninde sonrakinde Z ekseninde spawn olacaktır.
            spawnerIndex = spawnerIndex == 0 ? 1 : 0;
            //currentSpawner içerisine random olarak aldığı pozisyonda bulunan objeyi atıyoruz.
            currentSpawner = spawners[spawnerIndex];
            //Daha sonra SpawnerZ class'ı içerisinde bulunan SpawnCube methodunu çağırıyoruz.
            //Bu sayede obje yaratacağız.
            currentSpawner.SpawnCube();
        }
    }
}