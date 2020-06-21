using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Cw4.DTOs.Requests;
using Cw4.Models;
using Cw4.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Cw4.Controllers
{
    [Route("api/students")]
    [ApiController]


    public class StudentsController : ControllerBase
    {

        [HttpGet("{id}")]
        public IActionResult ModifyStudent(string id, [FromServices] EF_IStudentsDAL dbService)
        {
            var res = dbService.ModifyStudent(id, "Nowe", "Nowe");
            return Ok(res);
        }

        [HttpGet("enroll/{id}")]
        public IActionResult EnrollStudents(string id, [FromServices] EF_IStudentsDAL dbService)
        {
            var list = dbService.EnrollStudent(id, "Informatyka");
            return Ok(list);
        }

        [HttpGet]
        public IActionResult GetStudents([FromServices] EF_IStudentsDAL dbService)
        {
            var list = dbService.GetStudents();
            return Ok(list);
        }
          

    }
}