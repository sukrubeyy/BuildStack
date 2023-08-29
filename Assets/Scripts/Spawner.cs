using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Spawner : MonoBehaviour
{
    [Header("Prefab")] [SerializeField] private MoveCube prefabCube;
    [Header("MoveDirection Enum Class")] [SerializeField]
    private MoveDirection moveDirection;
    /// <summary>
    /// Spawn cube oluşturmak için
    /// </summary>
    public void SpawnCube()
    {
        
        // MoveCube tipinde(aslında cube objesi yaratıyoruz) bir obje yaratıyoruz.
        var cube = Instantiate(prefabCube);

        //Eğer MoveCube içerisinde yer alan LastCube değeri null değilse 
        //VE
        //LastCube StartCube isimli objeye eşit değilse bu şart içerisine gir.
        if (MoveCube.LastCube != null && MoveCube.LastCube.gameObject != GameObject.Find("StartCube"))
        {
            //Eğer seçilen spawn objesinin MoveDirection değeri X'e eşitse float olarak tanımladığımız X değerine
            //transform.position.x 'e eşitle.
            //Eğer değilse, Son cube' ün transform.position.x ' e eşitle.
            float x = moveDirection == MoveDirection.X ? transform.position.x : MoveCube.LastCube.transform.position.x;

            //Eğer seçilen spawn objesinin MoveDirection değeri Z'e eşitse float olarak tanımladığımız Z değerine
            //transform.position.z 'e eşitle.
            //Eğer değilse, Son cube' ün transform.position.x ' e eşitle.
            float z = moveDirection == MoveDirection.Z ? transform.position.z : MoveCube.LastCube.transform.position.z;

            //Oluşturduğumuz cube pozisyonuna = new Vector3 diyerek float x ve z olarak tanımladığımız bu koordinatları veriyoruz.
            //Y pozisyonuna verdiğimiz değer ise her eklenen cube objesinin bir üzerine doğru spawnlanabilmesi için yazılmıştır.
            cube.transform.position = new Vector3(x,
                MoveCube.LastCube.transform.position.y + prefabCube.transform.localScale.y,
                z);
        }

        else
        {
            cube.transform.position = transform.position;
        }

        //Oluşturduğumuz Cube MoveCube scriptini içerisinde barındırmaktadır.
        //MoveCube scripti içerisinde ise static enum olan MoveDirection değeri vardır bu değeri 
        // oluştuğu spawn objesinin moveDirection değerine eşitliyoruz. Bu sayede oluşan cube objesinin
        //Spawn bölgesi belli oluyor.
        cube.MoveDirection = moveDirection;
    }

    /// <summary>
    /// Spawn noktalarını belli olması için obje scale ve pozisyonları drawgizmos ile çizilmiştir.
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, prefabCube.transform.localScale);
    }
}