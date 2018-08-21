using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataAccess;


namespace WebAPIDemo.Controllers
{
    public class EmployeesController : ApiController
    {
        public HttpResponseMessage Get(string gender = "all")
        {
            using(WebAPIDemoEntities entity = new WebAPIDemoEntities())
            {
                switch (gender)
                {
                    case "all":
                        return Request.CreateResponse(HttpStatusCode.OK, entity.Employee.ToList());
                    case "male":
                        return Request.CreateResponse(HttpStatusCode.OK, entity.Employee.Where(e => e.Gender.ToLower()=="male").ToList());
                    case "female":
                        return Request.CreateResponse(HttpStatusCode.OK, entity.Employee.Where(e => e.Gender.ToLower() == "female").ToList());
                    default:
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Value of genger must be All, Male, Female " + gender + " is invalid.");

                }
            }
        }
        public HttpResponseMessage Get(int id)
        {
            using (WebAPIDemoEntities entity = new WebAPIDemoEntities())
            {
                var ent= entity.Employee.FirstOrDefault(e => e.ID == id);
                if(ent != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, ent);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with id : " + id + " not found.");
                }

            }
        }
        public HttpResponseMessage Post([FromBody]Employee employee)
        {
            try
            {
                using (WebAPIDemoEntities entity = new WebAPIDemoEntities())
                {
                    entity.Employee.Add(employee);
                    entity.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, employee);
                    message.Headers.Location = new Uri(Request.RequestUri + "/" + employee.ID.ToString());
                    return message;
                }
            }catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using (WebAPIDemoEntities entity = new WebAPIDemoEntities())
                {
                    var ent = entity.Employee.FirstOrDefault(e => e.ID == id);
                    if (ent == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with id : " + id + " not found.");
                    }
                    else
                    {
                        entity.Employee.Remove(ent);
                        entity.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, ent);
                    }

                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Put(int id, [FromBody]Employee employee)
        {
            try
            {
                using (WebAPIDemoEntities entity = new WebAPIDemoEntities())
                {
                    var ent = entity.Employee.FirstOrDefault(e => e.ID == id);
                    if (ent == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with id : " + id + " not found.");
                    }
                    else
                    {
                        ent.FirstName = employee.FirstName;
                        ent.LastName = employee.LastName;
                        ent.Gender = employee.Gender;
                        ent.Salary = employee.Salary;
                        entity.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, ent);
                    }

                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
