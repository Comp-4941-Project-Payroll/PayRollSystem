using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using PayRoll.Models;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace PayRoll.Controllers
{
    public class AttendanceController : ApiController
    {
        private PayrollDbContext db = new PayrollDbContext();

        // GET: api/Attendance
        public IQueryable<Attendance> GetAttendances()
        {
            return db.Attendances;
        }

        // GET: api/Attendance/5
        [ResponseType(typeof(Attendance))]
        public IHttpActionResult GetAttendance(string id)
        {
            Attendance attendance = db.Attendances.Find(id);
            if (attendance == null)
            {
                return NotFound();
            }

            return Ok(attendance);
        }

        // PUT: api/Attendance/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAttendance(string id, Attendance attendance)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != attendance.AttendanceId)
            {
                return BadRequest();
            }

            db.Entry(attendance).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AttendanceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Attendance
        [ResponseType(typeof(Attendance))]
        public IHttpActionResult PostAttendance(Attendance attendance)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Attendances.Add(attendance);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (AttendanceExists(attendance.AttendanceId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = attendance.AttendanceId }, attendance);
        }

        // DELETE: api/Attendance/5
        [ResponseType(typeof(Attendance))]
        public IHttpActionResult DeleteAttendance(string id)
        {
            Attendance attendance = db.Attendances.Find(id);
            if (attendance == null)
            {
                return NotFound();
            }

            db.Attendances.Remove(attendance);
            db.SaveChanges();

            return Ok(attendance);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

       
        private bool AttendanceExists(string id)
        {
            return db.Attendances.Count(e => e.AttendanceId == id) > 0;
        }
    }
}