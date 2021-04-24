using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Layout_SectionInAspCore.Models
{
    public class UserModel
    {
        public string Name { get; set; }
        private int age;

        public int Age
        {
            get { return age; }
            set { age = value; }
        }

    }
}
