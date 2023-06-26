using Microsoft.AspNetCore.Mvc;

namespace HSEPractice2.Controllers
{
    public class StaffTabController : Controller
    {

        public class StaffTavController
        {
            public Tab ActiveTab { get; set; }
        }

        public enum Tab {
            Staff, 
            Post
        }

    }
}
