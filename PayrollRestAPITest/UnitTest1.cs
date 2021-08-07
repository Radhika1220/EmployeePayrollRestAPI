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
            Assert.AreEqual(6, res.Count);
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
        //add data to json server
        public void AddingInJsonServer(JsonObject jsonObject)
        {
            RestRequest request = new RestRequest("/employees", Method.POST);
            request.AddParameter("application/json", jsonObject, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

        }
        [TestMethod]
        public void OnCallingPostAPI_Adding_MultipleData()
        {
            List<JsonObject> jsonList = new List<JsonObject>();

            JsonObject jsonObject = new JsonObject();
            jsonObject.Add("name", "Subhiksha");
            jsonObject.Add("Salary", 55000);
            jsonList.Add(jsonObject);

            JsonObject jsonObject1 = new JsonObject();
            jsonObject1.Add("name", "Kishore");
            jsonObject1.Add("salary", 60000);
         
            jsonList.Add(jsonObject1);
          foreach(var i in jsonList)
            {
                AddingInJsonServer(i);
            }

            IRestResponse response = GetAllEmployees();

            var res = JsonConvert.DeserializeObject<List<Employee>>(response.Content);
     
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
