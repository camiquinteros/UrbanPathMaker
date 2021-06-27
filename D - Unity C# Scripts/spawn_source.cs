using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.Barracuda;


public class spawn_source : MonoBehaviour
{




    //M.L Barracuda
    public NNModel model;
    private Model nmmodel;
    private IWorker worker;
    public Tensor input;


    private int time_step = Dropdown.step_time;
    public int year = 2017;
    private int month= Dropdown.step_month;
    private double time_binary= Dropdown.step_timeBinary;
    private double week_binary= Dropdown.step_week;
    private double amenities_total;
    private double public_transport;


    private float year1;
    private float month1;
    private float time_binary1;
    private float week_binary1;
    private float amenities_total1;
    private float public_transport1;







    //Spawining
    private float spawncount_cafe;
    private float spawncount_pharm;
    private float spawncount_store;
    private float spawncount_bank;
    private float spawncount_dest;
    private float spawncount_train;


    public GameObject agent;
    private GameObject nagent;
    private Transform transf;
    private Vector3 des;
    private Transform far;
    private Vector3 farvec;
    private GameObject pl;
    private Vector3 plan_pos;
    private float y_plane;
    private float runtime = 30;


    public Vector3 findPlaneY(){
        GameObject pl= GameObject.Find("Plane"); 
        Transform plane = pl.transform;
        Transform child = plane.GetChild(0);
        Vector3 plan_pos = child.GetComponent<Renderer>().bounds.center;   
        return plan_pos;
    }
    private float nowTime;
    // number of repetitive method
    private int count;
    // Use this for initialization
    

    void spawnner()
    {
        //M.L
        //scaling year
        if (year == 2017)
        {
            year1 = 1;
        }
        else
        {
            year1 = -1;
        }

        //scaling month
        if (month == 9)
        {
            month1 = 1;
        }
        else
        {
            month1 = -1;
        }

        //scaling time
        if (time_binary == 1)
        {
            time_binary1 = (float)1.22474487;
        }
        else if (time_binary == 0.5)
        {
            time_binary1 = 0;
        }
        else
        {
            time_binary1 = (float)-1.22474487;
        }
        //scaling week
        if (week_binary == 1)
        {
            week_binary1 = (float)0.70710678;
        }
        else
        {
            week_binary1 = (float)-1.41421356;
        }



        //Finding GameObject number in Layers
        GameObject banks = GameObject.Find("Bank"); 
        Transform bank1 = banks.transform;
        int bankno =bank1.childCount;

        GameObject cafes = GameObject.Find("Food_and_drinks"); 
        Transform cafe1 = cafes.transform;
        int cafeno =cafe1.childCount;

        GameObject pharms = GameObject.Find("Pharmacy"); 
        Transform pharm1 = pharms.transform;
        int pharmno = pharm1.childCount; 

        GameObject stores = GameObject.Find("Stores"); 
        Transform store1 = stores.transform;
        int storeno = store1.childCount;


        GameObject bus = GameObject.Find("Bus_station"); 
        Transform bus1 = bus.transform;
        int busno = bus1.childCount;


        GameObject train = GameObject.Find("Train_station"); 
        Transform train1 = train.transform;
        int trainno = train1.childCount;

        double amenities_total = (double)bankno + (double)cafeno + (double)pharmno + (double)storeno; 
        double public_transport = (double)busno + (double)trainno;




        

        //scaling Aminities
        double amenities = (amenities_total - 24.47)/37.20945;
        double public1 = (public_transport - 19.64)/26.92498;

        float amenities_total1 = (float)amenities;
        float public_transport1 = (float)public1;



        nmmodel = ModelLoader.Load(model);
        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.CSharpBurst, nmmodel);
        Tensor input = new Tensor(0,6);
        input[0,0] = year1;
        input[0,1] = month1;
        input[0,2] = time_binary1;
        input[0,3] = week_binary1;
        input[0,4] = amenities_total1;
        input[0,5] = public_transport1;

        worker.Execute(input);
        Tensor O = worker.CopyOutput();
        float prediction = O[0,0];
        double pedestrian_total = ((double)prediction * 38841)+44;
        int ped = (int)pedestrian_total;
        Debug.Log("Predicted ped count ="+ped);
        input.Dispose();
        O.Dispose();
        worker.Dispose();

        float prednew = (float)(ped/time_step);       


        spawncount_bank = (float)(Mathf.Round(prednew)*0.05);
        spawncount_cafe = (float)(Mathf.Round(prednew)*0.35);
        spawncount_pharm = (float)(Mathf.Round(prednew)*0.13);
        spawncount_store = (float)(Mathf.Round(prednew)*0.27);
        spawncount_dest = (float)(Mathf.Round(prednew)*0.10);
        spawncount_train = (float)(Mathf.Round(prednew)*0.10);

        
        //Cafe : Intiailisation 
        GameObject dest_object = GameObject.Find("cafe_gate");
        Transform destchild =dest_object.transform;

               
        float gates = (float)destchild.childCount;
        float spawnnumber = spawncount_cafe/gates;
        float interval = Mathf.Round(runtime/spawnnumber)*0.1f;
        InvokeRepeating("spawncafe_randm",1,interval);

        
        //Pharmacy : Intiailisation
        GameObject dest_object1 = GameObject.Find("pharm_gate");
        Transform destchild1 =dest_object1.transform;

               
        float gates1 = (float)destchild1.childCount;
        float spawnnumber1 = spawncount_pharm/gates1;
        float interval1 = Mathf.Round(runtime/spawnnumber1)*0.1f;
        InvokeRepeating("spawnpharm_randm",1,interval1);       

        
        //Store : Intiailisation
        GameObject dest_object2 = GameObject.Find("store_gate");
        Transform destchild2 =dest_object2.transform;

               
        float gates2 = (float)destchild2.childCount;
        float spawnnumber2 = spawncount_store/gates2;
        float interval2 = Mathf.Round(runtime/spawnnumber2)*0.1f;
        InvokeRepeating("spawnstore_randm",1,interval2); 


        //Bank : Intiailisation
        GameObject dest_object3 = GameObject.Find("bank_gate");
        Transform destchild3 =dest_object3.transform;

               
        float gates3 = (float)destchild3.childCount;
        float spawnnumber3 = spawncount_bank/gates3;
        float interval3 = Mathf.Round(runtime/spawnnumber3)*0.1f;
        InvokeRepeating("spawnbank_randm",1,interval3);       

        //Destination : Intiailisation
        GameObject dest_object4 = GameObject.Find("Gates");
        Transform destchild4 =dest_object4.transform;

               
        float gates4 = (float)destchild4.childCount;
        float spawnnumber4 = spawncount_dest/gates4;
        float interval4 = Mathf.Round(runtime/spawnnumber4)*0.1f;
        InvokeRepeating("spawndest_randm",1,interval4);

        //Destination : Intiailisation
        GameObject dest_object5 = GameObject.Find("Train_station");
        Transform destchild5 = dest_object5.transform;

               
        float gates5 = (float)destchild5.childCount;
        float spawnnumber5 = spawncount_train/gates5;
        float interval5 = Mathf.Round(runtime/spawnnumber5)*0.1f;
        InvokeRepeating("spawntrain_randm",1,interval5);
    }    


    // Start is called before the first frame update
    void OnEnable()
    {
           
        spawnner();


    }

    void OnDisable()
    {
        CancelInvoke();
        var clones = GameObject.FindGameObjectsWithTag ("spawns");
        foreach (var clone in clones){
            Destroy(clone);
        }
        
    }





    void spawncafe_randm()
    {
        //Finding all source.transform
        GameObject source1 = GameObject.Find("cafe_gate"); 
        Transform source = source1.transform;


        //Iterating through all child in source
        foreach (Transform child in source){

             //Finding position for current source
            Vector3 pos = child.GetComponent<Renderer>().bounds.center;

            //intializing vairables

           
                        

        
            //Finding all destinations
            GameObject dest_object = GameObject.Find("Food_and_drinks");
            Transform destchild =dest_object.transform;

            int rand = Random.Range(0,destchild.childCount);
           
            Vector3 bankcenter = new Vector3(0,0,0);
            Transform destin = destchild.GetChild(rand);


            Vector3 center = destin.GetComponent<Renderer>().bounds.center;
            Vector3 c = findPlaneY();
            Vector3 newcenter = new Vector3(center.x,c.y,center.z);
            float total_d = Vector3.Distance(newcenter,pos);
           

            bankcenter = newcenter;

            
            SpawnAgentBuilding(agent,pos,bankcenter);         
        }                     
    }
    


    void spawnpharm_randm()
    {
        //Finding all source.transform
        GameObject source1 = GameObject.Find("pharm_gate"); 
        Transform source = source1.transform;


        //Iterating through all child in source
        foreach (Transform child in source){

             //Finding position for current source
            Vector3 pos = child.GetComponent<Renderer>().bounds.center;

            //intializing vairables

           
                        

        
            //Finding all destinations
            GameObject dest_object = GameObject.Find("Pharmacy");
            Transform destchild =dest_object.transform;

            int rand = Random.Range(0,destchild.childCount);
           
            Vector3 bankcenter = new Vector3(0,0,0);
            Transform destin = destchild.GetChild(rand);


            Vector3 center = destin.GetComponent<Renderer>().bounds.center;
            Vector3 c = findPlaneY();
            Vector3 newcenter = new Vector3(center.x,c.y,center.z);
            float total_d = Vector3.Distance(newcenter,pos);
           

            bankcenter = newcenter;

            
            SpawnAgentBuilding(agent,pos,bankcenter);         
        }                     
    }   

    void spawnstore_randm()
    {
        //Finding all source.transform
        GameObject source1 = GameObject.Find("store_gate"); 
        Transform source = source1.transform;


        //Iterating through all child in source
        foreach (Transform child in source){

             //Finding position for current source
            Vector3 pos = child.GetComponent<Renderer>().bounds.center;

            //intializing vairables

           
                        

        
            //Finding all destinations
            GameObject dest_object = GameObject.Find("Stores");
            Transform destchild =dest_object.transform;

            int rand = Random.Range(0,destchild.childCount);
           
            Vector3 bankcenter = new Vector3(0,0,0);
            Transform destin = destchild.GetChild(rand);


            Vector3 center = destin.GetComponent<Renderer>().bounds.center;
            Vector3 c = findPlaneY();
            Vector3 newcenter = new Vector3(center.x,c.y,center.z);
            float total_d = Vector3.Distance(newcenter,pos);
           

            bankcenter = newcenter;

            
            SpawnAgentBuilding(agent,pos,bankcenter);         
        }                     
    }


    void spawnbank_randm()
    {
        //Finding all source.transform
        GameObject source1 = GameObject.Find("bank_gate"); 
        Transform source = source1.transform;


        //Iterating through all child in source
        foreach (Transform child in source){

             //Finding position for current source
            Vector3 pos = child.GetComponent<Renderer>().bounds.center;

            //intializing vairables

           
                        

        
            //Finding all destinations
            GameObject dest_object = GameObject.Find("Bank");
            Transform destchild =dest_object.transform;

            int rand = Random.Range(0,destchild.childCount);
           
            Vector3 bankcenter = new Vector3(0,0,0);
            Transform destin = destchild.GetChild(rand);


            Vector3 center = destin.GetComponent<Renderer>().bounds.center;
            Vector3 c = findPlaneY();
            Vector3 newcenter = new Vector3(center.x,c.y,center.z);
            float total_d = Vector3.Distance(newcenter,pos);
           

            bankcenter = newcenter;

            
            SpawnAgentBuilding(agent,pos,bankcenter);         
        }                     
    }



    void spawndest_randm()
    {
        //Finding all source.transform
        GameObject source1 = GameObject.Find("Gates"); 
        Transform source = source1.transform;


        //Iterating through all child in source
        foreach (Transform child in source){

             //Finding position for current source
            Vector3 pos = child.GetComponent<Renderer>().bounds.center;

            //intializing vairables

           
                        

        
            //Finding all destinations
            GameObject dest_object = GameObject.Find("Destination");
            Transform destchild =dest_object.transform;

            int rand = Random.Range(0,destchild.childCount);
           
            Vector3 bankcenter = new Vector3(0,0,0);
            Transform destin = destchild.GetChild(rand);


            Vector3 center = destin.GetComponent<Renderer>().bounds.center;
            Vector3 c = findPlaneY();
            Vector3 newcenter = new Vector3(center.x,c.y,center.z);
            float total_d = Vector3.Distance(newcenter,pos);
           

            bankcenter = newcenter;

            
            SpawnAgentBuilding(agent,pos,bankcenter);         
        }                     
    }


    void spawntrain_randm()
    {
        //Finding all source.transform
        GameObject source1 = GameObject.Find("Train_station"); 
        Transform source = source1.transform;


        //Iterating through all child in source
        foreach (Transform child in source){

             //Finding position for current source
            Vector3 pos = child.GetComponent<Renderer>().bounds.center;

            //intializing vairables

           
                        

        
            //Finding all destinations
            GameObject dest_object = GameObject.Find("Destination");
            Transform destchild =dest_object.transform;

            int rand = Random.Range(0,destchild.childCount);
           
            Vector3 bankcenter = new Vector3(0,0,0);
            Transform destin = destchild.GetChild(rand);


            Vector3 center = destin.GetComponent<Renderer>().bounds.center;
            Vector3 c = findPlaneY();
            Vector3 newcenter = new Vector3(center.x,c.y,center.z);
            float total_d = Vector3.Distance(newcenter,pos);
           

            bankcenter = newcenter;

            
            SpawnAgentBuilding(agent,pos,bankcenter);         
        }                     
    }

    void SpawnAgentBuilding(GameObject nagent, Vector3 des,Vector3 farvec)
    {
        


            
        GameObject na = (GameObject)Instantiate(nagent ,des , Quaternion.identity);
        na.tag = "spawns";
        NavMeshAgent agent= na.GetComponent<NavMeshAgent>();
        
  
        na.GetComponent<NavMeshAgent>().SetDestination(farvec);
 
        


                     
                    
 
        Destroy(na,60f);       
    }


    

    // Update is called once per frame
    void Update()
    {



        nowTime = Time.time;
        count = (int)nowTime++;
        Debug.Log("Runtime :"+count);
        if(count ==30){
            CancelInvoke();
        }

        

    }


}
