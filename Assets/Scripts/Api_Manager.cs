using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using Newtonsoft.Json; // This library USe for Json Serialization & Deserialization You can Download THis Package from Here Free https://assetstore.unity.com/packages/tools/input-management/json-net-converters-simple-compatible-solution-58621
using UnityEngine.Networking;// WE USE THIS NAME SPACE TO CALL OUR HTTP API IN UNTIY 
using UnityEngine.UI;
using TMPro;
using System.Diagnostics.CodeAnalysis;

public class Api_Manager : MonoBehaviour
{
    // Hello My NAme Is Tariq Jaffar And In This Project I will let you Know How to use RestApi In Unity By GET Data,PUT Data,Post Data And Delete Data 

    // Making this Delegate Event So I can Refference My Function In This Delegate 
    public delegate void ONSuccessResp(string Data);
    public static ONSuccessResp onSuccess;
    public string GET_DataApi;
    public string POST_DataApi;
    public string PUT_DataApi;
    public string DELETE_DataApi;
    public string UPDATE_DataApi;

    private void Awake()
    {
        onSuccess += OnSuccessfullResponse;
    }

    private void Start()
    {
        StartCoroutine(GEtRequest());
       
        
        UserDataManager.Instance.CreateEmployee.onClick.AddListener(UpdatePostData);//Put the Listener On when I click on the Button It raise the event and call my function to post the data
        UserDataManager.Instance.UpdateEmployee.onClick.AddListener(UpdatePutData);//Put the Listener On when I click on the Button It raise the event and call my function to post the data
    }

    #region GETUserDataApi 
    //So We Use The IEnumerator because it is an interface used primarily for creating coroutines.
    //Coroutines are functions that can pause execution and yield control back to Unity's main loop, allowing other code to run before continuing
    IEnumerator GEtRequest()
    {
        using (UnityWebRequest Request_To_GET_Data = UnityWebRequest.Get(GET_DataApi))
        {
            // So now we have send the request to the desired page and wait for the response
            yield return Request_To_GET_Data.SendWebRequest();

            //By Using the switch statement We Will Check That Which Type Of Response We Get 
            //There Are Five type of respone we can get which are listed below.So we can align the code according to code 
            switch (Request_To_GET_Data.result)
            {
                case UnityWebRequest.Result.InProgress://This type of response tell that the it is still in progress so you can manage the loading panel in this section.
                    break;
                case UnityWebRequest.Result.Success://This will tell that response is successful now we have the data from the server 
                    onSuccess.Invoke(Request_To_GET_Data.downloadHandler.text);//We are Sending the Data through parameters which we get download from the server on success response
                    break;
                case UnityWebRequest.Result.ConnectionError://This Will tell that the Connection is not reliable or there is something error in connection
                    break;
                case UnityWebRequest.Result.ProtocolError: // This will tell which type or error we got so 400 means successfull,404 or 500 is error 
                    break;
                case UnityWebRequest.Result.DataProcessingError://This will tell the request succeeded in communicating with the server, but encountered an error when processing the received data
                    break;
                default:
                    break;
            }
        }
    }
  
    public void OnSuccessfullResponse(string Data)
    {
        var DataOnResponse = JsonConvert.DeserializeObject<Root>(Data);//This process IS call Deserialization the data in Which I Convert he json Data TO C#
        Debug.Log(DataOnResponse.data[0].employee_name);
        //So I have make a static class of UserDataManager so i can put my all data on list that i have made so i dont need to call the server every time to get the data 
        foreach (var item in DataOnResponse.data)
        {
            UserDataManager.Instance.Salary.Add(item.employee_salary.ToString());
            UserDataManager.Instance.ID.Add(item.id.ToString());
            UserDataManager.Instance.Name.Add(item.employee_name.ToString());
            UserDataManager.Instance.Age.Add(item.employee_age.ToString());
            UserDataManager.EmpData.Invoke(0);//Here I Make A static delegate function in UserDataManager to raise the event and show my data on screen 
            Debug.Log(item.profile_image);
        }
    }

    #endregion
    #region PostUserDataApi

  

  

    // Method to update the postData string based on user input
    void UpdatePostData()
    {
        // Construct a JSON string based on the user input
        string name =UserDataManager.Instance. NameOFEmp.text;
        int age = int.Parse(UserDataManager.Instance.AgeOFEmp.text); // Assuming age is a number
        string Salary = UserDataManager.Instance.SalaryOFEmp.text;
       
     
        // Create JSON payload object
        var postData = new
        {
          
            title = name,
            body = Salary,
            userId = age
        };
        string jsonString = JsonConvert.SerializeObject(postData);
        StartCoroutine ( PostRequest(POST_DataApi, jsonString));
    }


    IEnumerator PostRequest(string url, string json)
    {
        // Create a UnityWebRequest object for POST request
        UnityWebRequest request = new UnityWebRequest(url,UnityWebRequest.kHttpVerbPOST);

        // Set the content type to JSON
      
        request.SetRequestHeader("Content-Type", "application/json; charset=UTF-8");
       

        // Convert the JSON string to byte array
        byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(json);

        // Set the request body with the JSON data
        request.uploadHandler = new UploadHandlerRaw(jsonBytes);

        // Set the download handler to receive the response from the server
        request.downloadHandler = new DownloadHandlerBuffer();

        // Send the request and wait for the response
        yield return request.SendWebRequest();

        switch (request.result)
        {
            case UnityWebRequest.Result.InProgress:
                break;
            case UnityWebRequest.Result.Success:
                Debug.Log("Post request successful!");
                Debug.Log("Response: " + request.downloadHandler.text);
                break;
            case UnityWebRequest.Result.ConnectionError:
                Debug.Log("Response: ConnectionError" );
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.Log("Response:Protocol Error "+request.downloadHandler.text);
                break;
            case UnityWebRequest.Result.DataProcessingError:
                Debug.Log("Response: DataProcessing Error" );
                break;
            default:
                break;
        }
        
    }
    #endregion

    #region PutUserData
    void UpdatePutData()
    {
        // Construct a JSON string based on the user input
        string name = UserDataManager.Instance.NameOFEmp.text;
        string age = UserDataManager.Instance.AgeOFEmp.text; // Assuming age is a number
        string Salary = UserDataManager.Instance.SalaryOFEmp.text;
        int ID = int.Parse(UserDataManager.Instance.EmployeeID.text);

        // Create JSON payload object
        var postData = new
        {
            id = ID,
            title = name,
            body = Salary,
            userId = age
        };
        string jsonString = JsonConvert.SerializeObject(postData);
        StartCoroutine(PutRequest(PUT_DataApi, jsonString));
    }


    IEnumerator PutRequest(string url, string json)
    {
        // Create a UnityWebRequest object for POST request
        UnityWebRequest request = new UnityWebRequest(url+UserDataManager.Instance.EmployeeID.text, UnityWebRequest.kHttpVerbPUT);

        // Set the content type to JSON

        request.SetRequestHeader("Content-Type", "application/json; charset=UTF-8");


        // Convert the JSON string to byte array
        byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(json);

        // Set the request body with the JSON data
        request.uploadHandler = new UploadHandlerRaw(jsonBytes);

        // Set the download handler to receive the response from the server
        request.downloadHandler = new DownloadHandlerBuffer();

        // Send the request and wait for the response
        yield return request.SendWebRequest();

        switch (request.result)
        {
            case UnityWebRequest.Result.InProgress:
                break;
            case UnityWebRequest.Result.Success:
                Debug.Log("Post request successful!");
                Debug.Log("Response: " + request.downloadHandler.text);
                break;
            case UnityWebRequest.Result.ConnectionError:
                Debug.Log("Response: ConnectionError");
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.Log("Response:Protocol Error " + request.downloadHandler.text);
                break;
            case UnityWebRequest.Result.DataProcessingError:
                Debug.Log("Response: DataProcessing Error");
                break;
            default:
                break;
        }

    }

    #endregion


}




#region ResponseJsonData_ToCSharp
// you can simpy get this classes by converting the json data to c# on json2csharp website 


[Serializable]
public class Datum
{
    public int id { get; set; }
    public string employee_name { get; set; }
    public int employee_salary { get; set; }
    public int employee_age { get; set; }
    public string profile_image { get; set; }
}

[Serializable]
public class Root
{
    public string status { get; set; }
    public List<Datum> data { get; set; }
    public string message { get; set; }
}

#endregion