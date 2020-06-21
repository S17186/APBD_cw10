using Cw4.DTOs.Requests;
using Cw4.EntityModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cw4.Service
{
    public class EF_SQLServerDbDAL : EF_IStudentsDAL
    {

        s17186Context db = new s17186Context();


        //EF_IStudentDAL
        public Student GetStudent(string indexNum)
        {
            var res = db.Student
                .Where(s => s.IndexNumber == indexNum)
                .FirstOrDefault();

            return res; 
        }

        public void RemoveStudent(string indexNum)
        {

            var s = new Student
            {
                IndexNumber = indexNum
            };
            db.Attach(s);
            db.Remove(s);

            db.SaveChanges(); 
        }

        public IEnumerable<Student> GetStudents()
        {
            List<EntityModels.Student> result;

            result = db.Student.ToList();

            return result;
        }

        public bool StudentExists(string indexNum)
        {
            throw new NotImplementedException();
        }

        public bool LoginCredentialsCorrect(string indexNum, string passHash, string salt)
        {
            throw new NotImplementedException();
        }

        
        public Student ModifyStudent(string indexNum, string firstName, string lastName)
        {
            // var s = db.Student.Find(indexNum);
            // s.FirstName = firstName;
            // s.LastName = lastName; 

            var s = new Student
            {
                IndexNumber = indexNum,
                FirstName = firstName,
                LastName = lastName
            };

            db.Attach(s);
            db.Entry(s).Property("FirstName").IsModified=true;
            db.Entry(s).Property("LastName").IsModified = true;

            db.SaveChanges();

            return s; 
        }

        public Student EnrollStudent(string indexNum, string studyName)
        {
            Student studentOfRequestedIndex = GetStudent(indexNum);

            Studies studyOfRequestedName = db.Studies.Where(s => s.Name == studyName).Single();

            //  List<Enrollment> enrollmentsDescending = db.Enrollment
            //      .OrderByDescending(e => e.StartDate).ToList(); 

            Enrollment enrollmentToRequestedStudyOfMostRecentDate = db.Enrollment
               .Where(e => e.Semester.Equals(1))
               .Where(e => e.IdStudy.Equals(studyOfRequestedName.IdStudy))
               .OrderByDescending(e => e.StartDate)
               .SingleOrDefault();

            Console.WriteLine(enrollmentToRequestedStudyOfMostRecentDate);

           // Enrollment enrollmentToRequestedStudyOfMostRecentDate = enrollmentsDescending
           //     .Where(e => e.IdStudy.Equals(studyOfRequestedName.IdStudy))
           //     .Single();

            if (enrollmentToRequestedStudyOfMostRecentDate.StartDate.Equals(DBNull.Value))
                enrollmentToRequestedStudyOfMostRecentDate.StartDate = DateTime.Today; 

            studentOfRequestedIndex.IdEnrollment = enrollmentToRequestedStudyOfMostRecentDate.IdEnrollment;


            //Update DB
            //db.Attach(studyOfRequestedName);
            db.Attach(enrollmentToRequestedStudyOfMostRecentDate); 
            db.Entry(studentOfRequestedIndex).Property("IdEnrollment").IsModified = true;
            db.Entry(enrollmentToRequestedStudyOfMostRecentDate).Property("StartDate").IsModified = true;

            return studentOfRequestedIndex; //now with modified enrollment 

        }

        public void PromoteStudents(int semester, string studyName)
        {
            //Assume the @semester param is NOT already for the current+1 value

            Studies requestedStudies = db.Studies
                .Where(s => s.Name.Equals(studyName))
                .SingleOrDefault();

            // Enrollmentt for the next (@semester+1) semester
            Enrollment requestedEnrollment = db.Enrollment
                .Where(e => e.Semester.Equals(semester+1))
                .Where(e => e.IdStudy.Equals(requestedStudies.IdStudy))
                .SingleOrDefault();

            if (requestedEnrollment.Equals(DBNull.Value))
            {
                Enrollment e = new Enrollment
                {
                    Semester = semester + 1,
                    IdStudy = requestedStudies.IdStudy, 
                    StartDate = DateTime.Today
                };
                // Update Enrollment to have an ID assigned
                db.Enrollment.Add(e);
                db.SaveChanges(); 

            }
            var res = db.Student
                .Where(s => s.IdEnrollment.Equals(requestedEnrollment.IdEnrollment))
                .Select(s => new 
                {
                    IdEnrollment = requestedEnrollment, 
                });


            //Update DB
            db.Attach(res);
            db.Entry(res).Property("IdEnrollment").IsModified = true;

            db.SaveChanges(); 
        }
    }






}
