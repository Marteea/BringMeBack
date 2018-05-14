using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BringMeBack.Class
{
    class Post
    {
        private int userId;
        private int id;
        private String title;
        private String body;

        public int UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public String Title
        {
            get { return title; }
            set { title = value; }
        }

        public String Body
        {
            get { return title; }
            set { title = value; }
        }


    }
}
