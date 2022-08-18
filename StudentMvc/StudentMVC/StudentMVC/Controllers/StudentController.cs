using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StudentMVC.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StudentMVC.Controllers
{
    public class StudentController : Controller
    {
        HttpClientHandler httpClientHandler = new HttpClientHandler();
        Students _student = new Students();
        List<Students> _students = new List<Students>();
        public StudentController()
        {
            httpClientHandler.ServerCertificateCustomValidationCallback =

                 (sender, cert, chain, sslpolicyErrors) => { return true; };

        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetStudents()
        {
            _students = new List<Students>();
            using (var httpclient = new HttpClient(httpClientHandler))
            {
                using (var response = await httpclient.GetAsync("http://localhost:37499/api/students"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    _students = JsonConvert.DeserializeObject<List<Students>>(apiResponse);
                }
            }
            return View(_students);
        }
        [HttpGet]
        public async Task<IActionResult> GetStudentByID(int id)
        {
            _student = new Students();
            using (var httpclient = new HttpClient(httpClientHandler))
            {
                using (var response = await httpclient.GetAsync("http://localhost:37499/api/students/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    _student = JsonConvert.DeserializeObject<Students>(apiResponse);
                }
            }
            return View(_student);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            _student = new Students();
            using (var httpclient = new HttpClient(httpClientHandler))
            {
                using (var response = await httpclient.GetAsync("http://localhost:37499/api/students/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    _student = JsonConvert.DeserializeObject<Students>(apiResponse);
                }
            }
            return View(_student);
        }
        [HttpPost]
        public async Task<IActionResult> SaveEdit(Students student)
        {

            using (var httpclient = new HttpClient(httpClientHandler))
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(student), Encoding.UTF8, "application/json");
                using (var response = await httpclient.PutAsync("http://localhost:37499/api/students/" + student.Id, content))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("GetStudents");
                    }
                }
            }


            return View("Create", student);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddStudent(Students student)
        {
            _student = new Students();
            using (var httpclient = new HttpClient(httpClientHandler))
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(student), Encoding.UTF8, "application/json");
                using (var response = await httpclient.PostAsync("http://localhost:37499/api/students", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    _student = JsonConvert.DeserializeObject<Students>(apiResponse);
                }
            }
            return RedirectToAction("GetStudents");
        }
        
        public async Task<IActionResult> Delete(int id)
        {
            
            using (var httpclient = new HttpClient(httpClientHandler))
            {
                  
                using (var response = await httpclient.DeleteAsync("http://localhost:37499/api/students/" + id))
                {

                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        return RedirectToAction("GetStudents");
                    }
                }
            }
            return RedirectToAction("Index");
        }
    }
}
