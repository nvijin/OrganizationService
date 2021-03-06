﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrgDAL;

namespace OrgAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : Controller
    {
        OrganizationDbContext dbContext;
        public DepartmentsController(OrganizationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var Depts = await dbContext.Departments.ToListAsync();
            if (Depts.Count != 0)
                return Ok(Depts);
            else
                return NotFound();
        }

        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var Dept = await dbContext.Departments.Where(x => x.Did == id).FirstOrDefaultAsync();
            if (Dept != null)
            {
                return Ok(Dept);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("getByName/{dName}")]
        public async Task<IActionResult> GetByName(string dName)
        {
            var Dept = await dbContext.Departments.Where(x => x.DName == dName).FirstOrDefaultAsync();
            if (Dept != null)
            {
                return Ok(Dept);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("getByIdAndName/{id}/{dName}")]
        public async Task<IActionResult> GetByIdAndName(int id, string dName)
        {
            var Dept = await dbContext.Departments.Where(x => x.Did == id && x.DName == dName).FirstOrDefaultAsync();
            if (Dept != null)
            {
                return Ok(Dept);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var Dept = await dbContext.Departments.Where(x => x.Did == id).FirstOrDefaultAsync();
            if (Dept != null)
            {
                dbContext.Remove(Dept);
                await dbContext.SaveChangesAsync();
                return Ok(Dept);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(Department D)
        {
            if (ModelState.IsValid)
            {
                dbContext.Add(D);
                await dbContext.SaveChangesAsync();
                return CreatedAtAction("Get", new { id = D.Did }, D);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(Department D)
        {
            if (ModelState.IsValid)
            {
                var Dept = await dbContext.Departments
                            .Where(x => x.Did == D.Did)
                            .AsNoTracking().FirstOrDefaultAsync();
                if (Dept != null)
                {
                    dbContext.Update(D);
                    await dbContext.SaveChangesAsync();
                    return NoContent();//or Ok(D);
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}