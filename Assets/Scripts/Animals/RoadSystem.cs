using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

[RequireComponent(typeof(VehiculePool))]
public class RoadSystem : MonoBehaviour
{
    VehiculePool vehiculePool;
    List<Vehicule> cars;

    [SerializeField] PathCreator pathCreator;
    [SerializeField] float circulationSpeed;
    [SerializeField] float rotationSpeed;

    [Header("Traffic")]
    [SerializeField] int trafficDensity;
    [SerializeField] float spawnSpeed;
    float spawnTimer;
    List<Vehicule> blockingVehicules;

    Vector3 point;
    Vector3 previousPoint;
    Vector3 dir;

    private void Start()
    {
        vehiculePool = GetComponent<VehiculePool>();
        cars = new List<Vehicule>();
        blockingVehicules = new List<Vehicule>();
        MyDebug.Log("Road length = " + pathCreator.path.length);
    }

    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer > spawnSpeed && cars.Count < trafficDensity && blockingVehicules.Count == 0)
        {
            cars.Add(vehiculePool._Pool.Get());

            spawnTimer = 0f;
        }

        // handle the speed of the cars
        HandleCirculation();

        // remove cars if needed
        HandleTraffic();
    }

    void HandleCirculation()
    {
        foreach (Vehicule car in cars)
        {
            point = pathCreator.path.GetPointAtDistance(car.distanceTravelled);
            dir = -(car.previous_Point - point).normalized;

            Quaternion targetRotation = Quaternion.LookRotation(dir);

            car.transform.rotation = Quaternion.Slerp(car.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            car.transform.rotation = Quaternion.Euler(car.transform.rotation.eulerAngles.x, car.transform.rotation.eulerAngles.y, 0f);
            Debug.DrawRay(car.transform.position, dir, Color.red);
            car.transform.position = point;

            Quaternion headingPoint = Quaternion.FromToRotation(car.transform.forward, dir);
            //car.localRotation *= headingPoint;


            car.distanceTravelled += Time.deltaTime * circulationSpeed;
            AjustVehiculeHeight(car,point.y);
            car.previous_Point = point;
        }
        

    }

    void HandleTraffic()
    {
        for (int i = cars.Count-1; i > 0; i--)
        {
            if (cars[i].distanceTravelled >= pathCreator.path.length)
            {
                cars[i].distanceTravelled = 0f;
                if (cars.Count > trafficDensity)
                {
                    vehiculePool._Pool.Release(cars[i]);
                    
                    cars.RemoveAt(i);
                }
            }
        }
        
    }

    void AjustVehiculeHeight(Vehicule car, float pointHeight)
    {
        car.transform.position = new Vector3(car.transform.position.x,
            car.height+pointHeight,
            car.transform.position.z
            );

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Vehicule>(out Vehicule v))
        {
            blockingVehicules.Add(v);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Vehicule>(out Vehicule v))
        {
            blockingVehicules.Remove(v);
        }
    }
}
