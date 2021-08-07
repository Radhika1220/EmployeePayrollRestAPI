using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;

namespace PayrollRestAPITest
{
    [TestClass]
    public class UnitTest1
    {
        //Initializing the restclient as null
        RestClient client = null;
        [TestInitialize]
        //This method is calling evrytime to initialzie the restclient object
        public void SetUp()
        {
            client = new RestClient("http://localhost:3000");
        }
        /// <summary>
        /// UC1-->Getting all the employees from json 
        /// </summary>
        /// <returns></returns>
        public IRestResponse GetAllEmployees()
        {
            //Get request 
            RestRequest request = new RestRequest("/employees", Method.GET);
            //Passing the request and execute 
            IRestResponse response = client.Execute(request);
            //Return the response
            return response;
        }
        /// <summary>
        /// Testmethod to pass the test case
        /// </summary>
        [TestMethod]
        public void OnCallingGetAPI_ReturnEmployees()
        {
            IRestResponse response = GetAllEmployees();
            //Convert the json object to list(deserialize)
            var res = JsonConvert.DeserializeObject<List<Employee>>(response.Content);
            Assert.AreEqual(8, res.Count);
            //Check the status code 
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            //printing the data in console
            foreach(var i in res)
            {
                Console.WriteLine("Id: {0}\t Name: {1}\t Salary :{2} ", i.id, i.name, i.salary);
            }
        }
        /// <summary>
        /// UC2--->Adding a employee in json server
        /// </summary>
        [TestMethod]
        public void OnCallingPostAPI_ReturnEmployee()
        {
            //Passing the method type as post(add details)
            RestRequest request = new RestRequest("/employees", Method.POST);
            //Creating a object
            JsonObject json = new JsonObject();
            //Adding the details
            json.Add("name", "Vishnu Priya");
            json.Add("salary", 78000);
            //passing the type as json 
            request.AddParameter("application/json", json, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            //convert the jsonobject to employee object
            var res = JsonConvert.DeserializeObject<Employee>(response.Content);
            Assert.AreEqual("Vishnu Priya", res.name);
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }
       /// <summary>
       /// UC3-->Adding the mutiple data in json server
       /// </summary>
       /// <param name="jsonObject"></param>
        public void AddingInJsonServer(JsonObject jsonObject)
        {
            RestRequest request = new RestRequest("/employees", Method.POST);
            request.AddParameter("application/json", jsonObject, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

        }
        /// <summary>
        /// UC3-->TestMethod
        /// </summary>
        [TestMethod]
        public void OnCallingPostAPI_Adding_MultipleData()
        {
            //Creating a list for adding multiple data in list
            List<JsonObject> jsonList = new List<JsonObject>();
            //Adding the details
            JsonObject jsonObject = new JsonObject();
            jsonObject.Add("name", "Subhiksha");
            jsonObject.Add("Salary", 55000);
            jsonList.Add(jsonObject);

            JsonObject jsonObject1 = new JsonObject();
            jsonObject1.Add("name", "Kishore");
            jsonObject1.Add("salary", 60000);
            jsonList.Add(jsonObject1);
            //Adding the details in json server
           foreach(var i in jsonList)
            {
                AddingInJsonServer(i);
            }
           //getting the respnse from getallemployee method
            IRestResponse response = GetAllEmployees();
            var res = JsonConvert.DeserializeObject<List<Employee>>(response.Content);
            //checkin the status code
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
        /// <summary>
        /// UC4--Update theexisting employee details (PUT METHOD)
        /// </summary>
        [TestMethod]
        public void OnCallingPutAPI_UpdateEmployeeDetails()
        {
            //Passing the method type as put(update existing employee details)
            RestRequest request = new RestRequest("/employees/5", Method.PUT);
            //Creating a object
            JsonObject json = new JsonObject();
            //Adding the details
            json.Add("name", "Praveena");
            json.Add("salary", 40000);
            //passing the type as json 
            request.AddParameter("application/json", json, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            //convert the jsonobject to employee object
            var res = JsonConvert.DeserializeObject<Employee>(response.Content);          
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
        /// <summary>
        /// UC5--->Delete the employee details using the id
        /// </summary>

        [TestMethod]
        public void OnCallingDeleteAPI_DeleteEmployeeDetails()
        {
            //Passing the method type as put(update existing employee details)
         
                RestRequest request = new RestRequest("/employees/16", Method.DELETE);
                IRestResponse response = client.Execute(request);
                //check count after deletion
              IRestResponse response1 = GetAllEmployees();
                List<Employee> result = JsonConvert.DeserializeObject<List<Employee>>(response1.Content);
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            }
        }
}
