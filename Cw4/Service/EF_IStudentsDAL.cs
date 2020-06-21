using Cw4.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cw4.Service
{
    public interface EF_IStudentsDAL
    {
        public IEnumerable<Student> GetStudents();
        public void RemoveStudent (string indexNum);
        public Student ModifyStudent (string indexNum, string firstName, string lastName);
        public Student EnrollStudent (string indexNum, string studyName);
        public void PromoteStudents (int semester, string studies);
        public Student GetStudent(string indexNum);
        public bool StudentExists(string indexNum);
        public bool LoginCredentialsCorrect(string indexNum, string passHash, string salt);
    }
}
