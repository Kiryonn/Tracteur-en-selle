using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class VehiculePool : MonoBehaviour
{
    [SerializeField] GameObject[] vehicules;
    public ObjectPool<Vehicule> _Pool { get; private set; }
    // Start is called before the first frame update
    void Awake()
    {
        _Pool = new ObjectPool<Vehicule>(createFunc: () =>
        {
            return CreateVehicule();
        },
        actionOnGet: (obj) =>
        {
            obj.gameObject.SetActive(true);
        },
        actionOnRelease: (obj) =>
        {
            obj.gameObject.SetActive(false);
        },
        actionOnDestroy: (obj) =>
        {
            Destroy(obj.gameObject);
        }
        );
    }



    Vehicule CreateVehicule()
    {
        int randomIndex = Random.Range(0, vehicules.Length);

        GameObject temp = Instantiate(vehicules[randomIndex]);

        if (temp.TryGetComponent<Vehicule>(out Vehicule comp))
        {
            return comp;
        }
        else
        {
            return temp.AddComponent<Vehicule>();
        }
        
    }
}
