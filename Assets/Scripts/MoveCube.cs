using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//Enum Particle Class olan MoveDirection SpawnerZ den türetilmiştir bu yüzden burada kütüphane gibi kullanmamız gerekmektedir.
using static Spawner;

public class MoveCube : MonoBehaviour
{   [Header("Movement")]
    public float _Speed=2f;
    //MoveCube currentCube hareket eden cube olarak ataması yapılmaktadır.
    public static MoveCube currentCube { get; private set; }
    //MoveCube Lastcube Sabit cube üzerine oturmuş olan en son cube objemizdir.
    public static MoveCube LastCube { get; private set; }

    //MoveDirectin classı türünde üretilen MoveDirection değişkeni.
    public MoveDirection MoveDirection { get;  set; }
    /// <summary>
    /// Obje Aktif haldeyken sürekli dönecek kod.
    /// </summary>
    private void OnEnable()
    {
        //Lastcube Null ise
        if(LastCube==null)
        {
            //Oyun ilk başladığında LastCube objesi null olacağından dolayı
            //LastCube olarak sabit olarak duren ve adı StartCube olan objeyi al.
            LastCube = GameObject.Find("StartCube").GetComponent<MoveCube>();
        }
        //Hareket Eden Mevcut obje currentCube ataması yapıyoruz.
        currentCube = this;
        //Random Color Seçmek İçin RandomChooseColor methodu oluşturduk ve burada bu methodu çağırıyoruz.
        GetComponent<Renderer>().material.color = RandomChooseColor();
        //Hareket eden objenin boyutu son eklenen objenin scale 'i kadar olsun.
        transform.localScale = new Vector3(LastCube.transform.localScale.x,transform.localScale.y,LastCube.transform.localScale.z);
    }
    /// <summary>
    /// Random Material Rengi Seçiyoruz.
    /// </summary>
    /// <returns></returns>
    private Color RandomChooseColor()
    {
        return new Color(UnityEngine.Random.Range(0,1f), UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f));
    }

    /// <summary>
    /// GameControl scriptinde Eğer Mouse Sol tıklarsak bu method çalışacaktır. 
    /// Ve currentCube objesini durdurmaktadır.
    /// </summary>
    internal void Stop()
    {
        _Speed = 0;
        CameraFollower.Instance.UpdateTransform();
        //float olarak tanımladığımız hangover değerine GetHangover methodu içerisinde LastCube ile currentCube obje arasındaki
        //koordinat farkını buluyoruz ve hangover değişkenine atıyoruz.
        float hangover = GetHangover();
        //Burada ise LastCube localScale değerini max değişkenine atıyoruz
        float max = MoveDirection == MoveDirection.Z ? LastCube.transform.localScale.z : LastCube.transform.localScale.x; 
        //Eğer hangover yani aradaki fark max değerinden büyükse oyun tekrar başlasın.
        if (Mathf.Abs(hangover) >= max)
        {
            // LastCube = null;
            // currentCube = null;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        //Eğer hangover 0 değerinden büyük ise 1 değerini ata
        //Değilse -1 değerini ata.
        float direction = hangover > 0 ? 1f : -1f;
        //SpawnerZ objesinde yer alan class içerisinden gelen MoveDirection bilgisi eğer
        //MoveDirection.Z ' ye eşitse
        if (MoveDirection == MoveDirection.Z)
        {
            //Z pozisyonunda bu objeyi kırmak için gerekli olan methodu çağır.
            SplitZpos(hangover, direction);
        }
        else
        {
            //X pozisyonunda bu objeyi kırmak için gerekli olan methodu çağır.
            SplitXPos(hangover, direction);
        }

    }
    /// <summary>
    /// Objelerin arasındaki farkı belirlemek için oluşturulmuş bir methoddur.
    /// </summary>
    /// <returns>Stop methodu içerisinde yer alan float türünde hangover değişkenine değer döndürür.</returns>
    private float GetHangover()
    {
        if(MoveDirection==MoveDirection.Z)
        {
            return transform.position.z - LastCube.transform.position.z;
        }
        else
        {
            return transform.position.x - LastCube.transform.position.x;
        }
        
    }

    /// <summary>
    /// X pozisyonunda cube scale değişmesini sağlar. 
    /// </summary>
    /// <param name="hangXPos">Aradaki farkı alan parametre</param>
    /// <param name="direction">hangover büyüklüğüne göre 1 veya -1 değeri alan parametre</param>
    private void SplitXPos(float hangXPos,float direction)
    {
        //Son durdurulan cube ile aradaki hangover yani farkı alıp newXsize atıyoruz.
        float newXsize = LastCube.transform.localScale.x - Math.Abs(hangXPos);
        //daha sonra mevcut currentCube localScale2den newXsize ' ı çıkarıyoruz.
        float BlockSize = transform.localScale.x - newXsize;
        //LastCube x pozisyonunu alıp üzerine farkın yarısı kadar ekleme yaparak LastCube üzerine tam oturmasını sağlıyoruz.
        float newXpos = LastCube.transform.position.x + (hangXPos / 2f);

        //currentCube  Scale X 'i newXsize ' a eşitliyoruz. Bu sayede üstüne eklendiği objenin ScaleX'i kadar oluyoru
        //Kısacası altındaki yer alan cube kadar yer kaplıyor.
        transform.localScale = new Vector3(newXsize , transform.localScale.y, transform.localScale.z);
        //Durdurulan currentCube altında bulunan cube nesnesinin üzerine tam oturması için x eksenine newXpos olarak tanımladığımız
        //değeri atıyoruz. Bu değer de altındaki cube ve farkın yarısının toplamını barındırmaktadır.
        transform.position = new Vector3(newXpos , transform.position.y, transform.position.z);
        //
        float edgeCub = transform.position.x + (newXsize / 2f * direction);

        float blockXPosition = edgeCub + (BlockSize / 2f * direction);

        LastCube = this;
        SpawnCube(blockXPosition, BlockSize);
    }

    private void SplitZpos(float hangZPos,float direction)
    {
        float newZsize = LastCube.transform.localScale.z - Math.Abs(hangZPos);

        float BlockSize = transform.localScale.z - newZsize;

        float newZpos = LastCube.transform.position.z + (hangZPos / 2f);

        transform.localScale = new Vector3(transform.localScale.x,transform.localScale.y,newZsize);
        transform.position = new Vector3(transform.position.x,transform.position.y,newZpos);

        float edgeCub = transform.position.z + (newZsize / 2f* direction);
        float blockZPosition = edgeCub + (BlockSize / 2f* direction);
        LastCube = this;
        SpawnCube(blockZPosition, BlockSize);
    }

    private void SpawnCube(float blockZPosition,float blockZSize)
    {
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        if(MoveDirection==MoveDirection.Z)
        {
            cube.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, blockZSize);
            cube.transform.position = new Vector3(transform.position.x, transform.position.y, blockZPosition);
        }
        else
        {
            cube.transform.localScale = new Vector3(blockZSize , transform.localScale.y, transform.localScale.z);
            cube.transform.position = new Vector3(blockZPosition , transform.position.y,transform.position.z );

        }
        cube.GetComponent<Renderer>().material.color = GetComponent<Renderer>().material.color; 
      
        cube.AddComponent<Rigidbody>();
        Destroy(cube.gameObject, 1f);
    }

    void Update()
    {

        if(MoveDirection==MoveDirection.Z)
        {
            transform.position += transform.forward * Time.deltaTime * _Speed;
        }
        else
        {
            transform.position += transform.right * Time.deltaTime * _Speed;
        }
       
    }
}
